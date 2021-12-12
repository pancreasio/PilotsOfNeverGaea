using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    private int p1Score, p2Score;
    public int roundsToWin;
    public TextMeshProUGUI p1ScoreText, p2ScoreText, startRoundText;
    public GameObject p1Light, p2Light,
        ballPrefab,
        p1Position, p2Position,
        topRightArrow, topLeftArrow, bottomRightArrow, bottomLeftArrow,
        p1NullBarEmpty, p2NullBarEmpty,
        pauseCanvas;

    public EventSystem pauseEventSystem;
    private PauseInput pauseInput;
    private bool isPaused;
    public Platform leftPlatform, rightPlatform;
    public List<GameObject> shipPrefabList;
    public SpriteRenderer fadeoutSprite, p1NullBarFull, p2NullBarFull;
    private GameObject ballReference = null,
        topSparks, bottomSparks,
        p1Instance = null, p2Instance = null;
    private Ball ballScriptReference;
    private GameManager.ButtonAction ResetRoundAction;
    public static GameManager.GameOverFunction GameOverAction;
    public static CharacterSelectionManager.Character p1Selected, p2Selected;

    public static GameManager.BindInputFunction BindPlayersFunction;
    public Animator p1EXAnimator, p2EXAnimator;

    public Animator gravityWellAnimator;
    public Shake2D cameraShake;
    public delegate void LevelAction();
    public float startRoundTime, resetRoundTime,
        platformDelay,
        ballResetSpeed,
        cameraShakeDuration, cameraShakeIntensity,
        fadeoutTime;
    private float platformClock, platformInitialX;
    private bool gameEnded = false, gameStarted = false, platformsClosing = false;
    public static GameManager.SceneChange CharacterSelectButton, ExitButton;
    public delegate void ChargeAction(int charges);
    public delegate void UpdatePower(float power);
    public delegate void PaletteAction();
    private void Start()
    {
        Ball.ElectrifyAction = ActivateSparks;
        Ball.UnstickAction = DeactivateSparks;
        topSparks = GameObject.Find("top sparks");
        bottomSparks = GameObject.Find("bottom sparks");
        topSparks.SetActive(false);
        bottomSparks.SetActive(false);
        p1Score = 0;
        p2Score = 0;
        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
        platformClock = 0f;
        Ball.onScore = PlayerScored;
        Time.timeScale = 1;
        InitializeGame();
        StartCoroutine(StartRound());
        AkSoundEngine.PostEvent("music_gameplay", gameObject);

        pauseInput = new PauseInput();
        pauseInput.Pausemap.Enable();
        isPaused = false;

        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().OnActivateAction += OnDeviceLost;
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().controlsSetAction += OnControlsSet;
    }

    private void Update()
    {
        platformClock += Time.deltaTime;
        if (platformClock > platformDelay && !platformsClosing)
        {
            AkSoundEngine.PostEvent("sfx_closewalls", gameObject);
            leftPlatform.ClosePlatform();
            rightPlatform.ClosePlatform();
            platformsClosing = true;
        }

        if (p1Score >= roundsToWin && !gameEnded)
        {
            StartCoroutine(FadeOut(true));
            gameEnded = true;
        }
        if (p2Score >= roundsToWin && !gameEnded)
        {
            StartCoroutine(FadeOut(false));
            gameEnded = true;
        }

        if (pauseInput.Pausemap.Start.WasPerformedThisFrame() && gameStarted && !gameEnded)
        {
            if(!isPaused)
                PauseGame();
            else
                UnPause();
        }
    }

    private void PauseGame()
    {
            isPaused = true;
            pauseCanvas.SetActive(true);
            Time.timeScale = 0f;
            Dio.isPaused = true;

            p1Instance.GetComponent<PlayerInput>().DeactivateInput();
            p2Instance.GetComponent<PlayerInput>().DeactivateInput();
    }
    private void InitializeGame()
    {
        if (p1Selected != CharacterSelectionManager.Character.none)
        {
            p1Instance = Instantiate(shipPrefabList[(int)p1Selected - 1], p1Position.transform);
        }

        if (p1Selected == CharacterSelectionManager.Character.NULL)
        {
            InitializeNULL(true);
        }

        if (p1Instance != null)
        {
            InitializeShip(true);
        }

        if (p2Selected != CharacterSelectionManager.Character.none)
            p2Instance = Instantiate(shipPrefabList[(int)p2Selected - 1], p2Position.transform);

        if (p2Selected == CharacterSelectionManager.Character.NULL)
        {
            InitializeNULL(false);
        }

        if (p2Instance != null)
        {
            InitializeShip(false);
        }

        BindPlayersFunction(p1Instance.GetComponent<PlayerInput>(), p2Instance.GetComponent<PlayerInput>());

        ballReference = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
        ballReference.transform.parent = transform;
        ballScriptReference = ballReference.GetComponent<Ball>();

        Odyssey.VoyageAction += ballScriptReference.Voyage;
        Hawking.WellAction += ballScriptReference.GravityWell;
        Hawking.WellActiveAction += UpdateGravityWell;
        if(p1Selected == CharacterSelectionManager.Character.Hawking || p2Selected == CharacterSelectionManager.Character.Hawking)
            gravityWellAnimator.gameObject.SetActive(true);

        Dio.timeStopAction = UpdateTheWorld;
        if(p1Selected == CharacterSelectionManager.Character.Francesco)
            ResetRoundAction += p1Instance.GetComponent<Francesco>().OnResetRound;
        
        if(p2Selected == CharacterSelectionManager.Character.Francesco)
            ResetRoundAction += p2Instance.GetComponent<Francesco>().OnResetRound;

        if(p1Selected == CharacterSelectionManager.Character.Nagate || p2Selected == CharacterSelectionManager.Character.Nagate)
            ResetRoundAction += SmokeBomb.OnRoundReset;
    }

   


    private void InitializeShip(bool player1)
    {
        if (player1)
        {
            p1Instance.GetComponent<Palette>().isPlayer1 = true;
            p1Instance.transform.position = p1Position.transform.position;
            p1Light.transform.parent = p1Instance.transform;
            p1EXAnimator.SetTrigger("RESET");
            p1EXAnimator.SetInteger("MAX_CHARGES", p1Instance.GetComponent<Palette>().maxCharges);
            p1EXAnimator.SetInteger("CHARGES", 0);
            p1Instance.GetComponent<Palette>().UpdateCharges = P1UpdateCharges;
        }
        else
        {
            p2Instance.GetComponent<Palette>().isPlayer1 = false;
            p2Instance.transform.position = p2Position.transform.position;
            p2Instance.transform.Rotate(Vector3.back, 180.0f);
            p2Light.transform.parent = p2Instance.transform;
            p2EXAnimator.SetTrigger("RESET");
            p2EXAnimator.SetInteger("MAX_CHARGES", p2Instance.GetComponent<Palette>().maxCharges);
            p2EXAnimator.SetInteger("CHARGES", 0);
            p2Instance.GetComponent<Palette>().UpdateCharges = P2UpdateCharges;
        }
    }
    private void InitializeNULL(bool player1)
    {
        if (player1)
        {
            p1EXAnimator.gameObject.SetActive(false);
            p1NullBarEmpty.SetActive(true);
            p1NullBarFull.gameObject.SetActive(true);
            p1Instance.GetComponent<NULL>().UpdateGlitchPower = P1UpdateNULL;
            p1Instance.GetComponent<NULL>().GlitchAction = P1Glitch;
        }
        else
        {
            p2EXAnimator.gameObject.SetActive(false);
            p2NullBarEmpty.SetActive(true);
            p2NullBarFull.gameObject.SetActive(true);
            p2Instance.GetComponent<NULL>().UpdateGlitchPower = P2UpdateNULL;
            p2Instance.GetComponent<NULL>().GlitchAction = P2Glitch;
        }
    }

    private IEnumerator StartRound()
    {
        Vector2 initialDirection;
        GameObject actualArrow;
        if (UnityEngine.Random.value < 0.5f)
        {
            initialDirection = Vector2.down;
            if (UnityEngine.Random.value < 0.5f)
            {
                initialDirection += Vector2.left;
                actualArrow = bottomLeftArrow;
            }
            else
            {
                initialDirection += Vector2.right;
                actualArrow = bottomRightArrow;
            }
        }
        else
        {
            initialDirection = Vector2.up;
            if (UnityEngine.Random.value < 0.5f)
            {
                initialDirection += Vector2.left;
                actualArrow = topLeftArrow;
            }
            else
            {
                initialDirection += Vector2.right;
                actualArrow = topRightArrow;
            }
        }

        initialDirection.Normalize();

        startRoundText.gameObject.SetActive(true);
        startRoundText.text = "arena prepared";
        float startClock = 0f;
        actualArrow.SetActive(true);
        while (startClock < startRoundTime)
        {
            startClock += Time.deltaTime;
            if (startClock > 3 * startRoundTime / 4)
                startRoundText.text = "collide!";
            yield return null;
        }
        startRoundText.gameObject.SetActive(false);
        actualArrow.SetActive(false);
        ballScriptReference.InitialKick(initialDirection);
        gameStarted = true;
    }

    private IEnumerator ResetRound(bool player1Scored)
    {
        if(ResetRoundAction != null)
            ResetRoundAction.Invoke();

        GameObject actualArrow;
        startRoundText.gameObject.SetActive(true);
        startRoundText.text = "resetting arena";
        ballScriptReference.StopMovement();
        DeactivateSparks();
        platformClock = 0;
        float resetClock = 0f;
        bool ballResetting = true;
        if (p1Instance != null)
            p1Instance.GetComponent<Palette>().ResetPalette();
        if (p2Instance != null)
            p2Instance.GetComponent<Palette>().ResetPalette();

        Vector2 initialDirection;
        if (UnityEngine.Random.value < 0.5f)
        {
            initialDirection = Vector2.up;
            if (player1Scored)
            {
                initialDirection += Vector2.left;
                actualArrow = topLeftArrow;
            }
            else
            {
                initialDirection += Vector2.right;
                actualArrow = topRightArrow;
            }
        }
        else
        {
            initialDirection = Vector2.down;
            if (player1Scored)
            {
                initialDirection += Vector2.left;
                actualArrow = bottomLeftArrow;
            }
            else
            {
                initialDirection += Vector2.right;
                actualArrow = bottomRightArrow;
            }
        }

        initialDirection.Normalize();
        leftPlatform.ResetPlatform();
        rightPlatform.ResetPlatform();
        AkSoundEngine.PostEvent("sfx_openwalls", gameObject);

        while (resetClock < resetRoundTime || ballResetting)
        {
            platformClock = 0f;
            resetClock += Time.deltaTime;
            if (resetClock > 3 * resetRoundTime / 4)
                startRoundText.text = "engage!";

            if (ballResetting && Mathf.Abs(ballReference.transform.position.x) > 0.2f)
            {
                ballReference.transform.Translate(-ballReference.transform.position * ballResetSpeed * Time.deltaTime);
            }
            else
            {
                ballResetting = false;
                ballReference.transform.position = Vector2.zero;
                actualArrow.SetActive(true);
            }

            yield return null;
        }
        startRoundText.gameObject.SetActive(false);
        ballScriptReference.InitialKick(initialDirection);
        actualArrow.SetActive(false);
        platformsClosing = false;
    }

    private void PlayerScored(bool player1)
    {

        StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeIntensity));
        if (player1)
        {
            p1Score++;
            if (p1Score < roundsToWin)
                StartCoroutine(ResetRound(true));
        }
        else
        {
            p2Score++;
            if (p2Score < roundsToWin)
                StartCoroutine(ResetRound(false));
        }
        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
    }

    private IEnumerator FadeOut(bool player1Won)
    {
        AkSoundEngine.PostEvent("sfx_endgame", gameObject);
        Time.timeScale = 0f;
        float clock = 0f;
        while (clock < fadeoutTime)
        {
            clock += Time.unscaledDeltaTime;
            fadeoutSprite.color = new Color(255f, 255f, 255f, clock / fadeoutTime);
            yield return null;
        }
        Time.timeScale = 1f;
        GameOver(player1Won);
    }


    private void ActivateSparks()
    {
        topSparks.SetActive(true);
        bottomSparks.SetActive(true);
    }

    private void DeactivateSparks()
    {
        topSparks.SetActive(false);
        bottomSparks.SetActive(false);
    }

    private void GameOver(bool player1won)
    {
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().OnActivateAction -= OnDeviceLost;
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().controlsSetAction -= OnControlsSet;

        Odyssey.VoyageAction -= ballScriptReference.Voyage;
        Hawking.WellAction -= ballScriptReference.GravityWell;
        Hawking.WellActiveAction -= UpdateGravityWell;
        ResetRoundAction = null;
        if (GameOverAction != null)
            GameOverAction(player1won);
    }

    private void UpdateGravityWell(bool isActive)
    {
        gravityWellAnimator.SetBool("ACTIVE", isActive);
    }

    private void UpdateTheWorld(bool isActive)
    {
        if(isActive)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    private void P1UpdateCharges(int charges)
    {
        p1EXAnimator.SetInteger("CHARGES", charges);
    }
    private void P2UpdateCharges(int charges)
    {
        p2EXAnimator.SetInteger("CHARGES", charges);
    }

    private void P1UpdateNULL(float power)
    {
        p1NullBarFull.color = new Color(255f, 255f, 255f, power);
    }

    private void P2UpdateNULL(float power)
    {
        p2NullBarFull.color = new Color(255f, 255f, 255f, power);
    }

    private void P1Glitch()
    {
        PlayerScored(true);
    }

    private void P2Glitch()
    {
        PlayerScored(false);
    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseCanvas.GetComponent<UIManager>().ResetSelected();
        pauseCanvas.SetActive(false);
        Dio.isPaused = false;
        p1Instance.GetComponent<PlayerInput>().ActivateInput();
        p2Instance.GetComponent<PlayerInput>().ActivateInput();
    }

    public void CharacterSelect()
    {
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().OnActivateAction -= OnDeviceLost;
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().controlsSetAction -= OnControlsSet;

        Time.timeScale = 1f;
        if (CharacterSelectButton != null)
            CharacterSelectButton(1);
    }

    public void MainMenu()
    {
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().OnActivateAction -= OnDeviceLost;
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().controlsSetAction -= OnControlsSet;

        Time.timeScale = 1f;
        if (ExitButton != null)
            ExitButton(0);
    }

    private void OnDeviceLost()
    {
        PauseGame();
        pauseEventSystem.enabled = false;
    }

    private void OnControlsSet(ControllerDeviceUI player1Device, ControllerDeviceUI player2Device)
    {
        p1Instance.GetComponent<PlayerInput>().SwitchCurrentControlScheme(player1Device.assignedScheme, player1Device.assignedDevice);
        p2Instance.GetComponent<PlayerInput>().SwitchCurrentControlScheme(player2Device.assignedScheme, player2Device.assignedDevice);

        pauseEventSystem.enabled = true;
        pauseEventSystem.SetSelectedGameObject(pauseEventSystem.firstSelectedGameObject);
    }

    public void ControlsButton()
    {
        pauseEventSystem.enabled = false;
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().OnActivate();
    }
}
