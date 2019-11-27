using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    private int p1Score, p2Score;
    public int roundsToWin;
    public TextMeshProUGUI p1ScoreText, p2ScoreText, startRoundText;
    public GameObject leftPlatform, rightPlatform, p1Light, p2Light,
        ballPrefab, magPrefab, railPrefab, kunstPrefab, knockoutPrefab, djinnPrefab, leytenantPrefab, nullPrefab,
        p1Position, p2Position, topRightArrow, topLeftArrow, bottomRightArrow, bottomLeftArrow,
        p1NullBarEmpty, p2NullBarEmpty,
        pauseCanvas;
    public SpriteRenderer fadeoutSprite, p1NullBarFull, p2NullBarFull;
    private GameObject ballReference = null,
        topSparks, bottomSparks,
        p1Instance = null, p2Instance = null,
        pauseLastSelected;
    private Ball ballScriptReference;
    public static GameManager.GameOverFunction GameOverAction;
    public static CharacterSelectionManager.Character p1Selected, p2Selected;
    public Animator p1PlatformAnimator, p2PlatformAnimator, p1EXAnimator, p2EXAnimator;
    public Shake2D cameraShake;
    public delegate void LevelAction();
    public float startRoundTime, resetRoundTime,
        platformDelay, platformSpeed, platformResetTime, platformLimit,
        ballResetSpeed, 
        cameraShakeDuration, cameraShakeIntensity, 
        fadeoutTime;
    private float platformClock, platformInitialX;
    private bool platformsClosing, platformsArrived, paused;
    public static GameManager.SceneChange CharacterSelectButton, ExitButton;
    public delegate void ChargeAction(int charges);
    public delegate void UpdatePower(float power);
    public delegate void PaletteAction();
    public EventSystem pauseEventSystem;

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
        platformClock = 0;
        platformsClosing = false;
        platformsArrived = false;
        paused = false;
        pauseLastSelected = pauseEventSystem.firstSelectedGameObject;
        Ball.onScore = PlayerScored;
        Time.timeScale = 1;
        platformInitialX = rightPlatform.transform.position.x;
        p1PlatformAnimator.SetInteger("STATE", 0);
        p2PlatformAnimator.SetInteger("STATE", 0);
        InitializeGame();
        StartCoroutine(StartRound());
        AkSoundEngine.PostEvent("music_gameplay", gameObject);
    }

    private void InitializeGame()
    {
        switch (p1Selected)
        {
            case CharacterSelectionManager.Character.none:
                break;
            case CharacterSelectionManager.Character.raildrive:
                p1Instance = Instantiate(railPrefab, p1Position.transform);
                break;
            case CharacterSelectionManager.Character.magstream:
                p1Instance = Instantiate(magPrefab, p1Position.transform);
                break;
            case CharacterSelectionManager.Character.kunst:
                p1Instance = Instantiate(kunstPrefab, p1Position.transform);
                break;
            case CharacterSelectionManager.Character.knockout:
                p1Instance = Instantiate(knockoutPrefab, p1Position.transform);
                break;
            case CharacterSelectionManager.Character.djinn:
                p1Instance = Instantiate(djinnPrefab, p1Position.transform);
                break;
            case CharacterSelectionManager.Character.Leytenant:
                p1Instance = Instantiate(leytenantPrefab, p1Position.transform);
                break;
            case CharacterSelectionManager.Character.NULL:
                p1Instance = Instantiate(nullPrefab, p1Position.transform);
                p1EXAnimator.gameObject.SetActive(false);
                p1NullBarEmpty.SetActive(true);
                p1NullBarFull.gameObject.SetActive(true);
                p1Instance.GetComponent<NULL>().UpdateGlitchPower = P1UpdateNULL;
                p1Instance.GetComponent<NULL>().GlitchAction = P1Glitch;
                break;
            default:
                break;
        }


        if (p1Instance != null)
        {
            p1Instance.GetComponent<Palette>().left = true;
            p1Instance.transform.position = p1Position.transform.position;
            p1Light.transform.parent = p1Instance.transform;
            p1EXAnimator.SetTrigger("RESET");
            p1EXAnimator.SetInteger("MAX_CHARGES", p1Instance.GetComponent<Palette>().maxCharges);
            p1EXAnimator.SetInteger("CHARGES", 0);
            p1Instance.GetComponent<Palette>().UpdateCharges = P1UpdateCharges;
        }

        switch (p2Selected)
        {
            case CharacterSelectionManager.Character.none:
                break;
            case CharacterSelectionManager.Character.raildrive:
                p2Instance = Instantiate(railPrefab, p2Position.transform);
                break;
            case CharacterSelectionManager.Character.magstream:
                p2Instance = Instantiate(magPrefab, p2Position.transform);
                break;
            case CharacterSelectionManager.Character.kunst:
                p2Instance = Instantiate(kunstPrefab, p2Position.transform);
                break;
            case CharacterSelectionManager.Character.knockout:
                p2Instance = Instantiate(knockoutPrefab, p2Position.transform);
                break;
            case CharacterSelectionManager.Character.djinn:
                p2Instance = Instantiate(djinnPrefab, p2Position.transform);
                break;
            case CharacterSelectionManager.Character.Leytenant:
                p2Instance = Instantiate(leytenantPrefab, p2Position.transform);
                break;
            case CharacterSelectionManager.Character.NULL:
                p2Instance = Instantiate(nullPrefab, p2Position.transform);
                p2EXAnimator.gameObject.SetActive(false);
                p2NullBarEmpty.SetActive(true);
                p2NullBarFull.gameObject.SetActive(true);
                p2Instance.GetComponent<NULL>().UpdateGlitchPower = P2UpdateNULL;
                p2Instance.GetComponent<NULL>().GlitchAction = P2Glitch;
                break;
            default:
                break;
        }

        if (p2Instance != null)
        {
            p2Instance.GetComponent<Palette>().left = false;
            p2Instance.transform.position = p2Position.transform.position;
            p2Instance.transform.Rotate(Vector3.back, 180.0f);
            p2Light.transform.parent = p2Instance.transform;
            p2EXAnimator.SetTrigger("RESET");
            p2EXAnimator.SetInteger("MAX_CHARGES", p2Instance.GetComponent<Palette>().maxCharges);
            p2EXAnimator.SetInteger("CHARGES", 0);
            p2Instance.GetComponent<Palette>().UpdateCharges = P2UpdateCharges;
        }
        ballReference = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
        ballReference.transform.parent = transform;
        ballScriptReference = ballReference.GetComponent<Ball>();
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
    }

    private IEnumerator ResetRound(bool player1Scored)
    {
        GameObject actualArrow;
        startRoundText.gameObject.SetActive(true);
        startRoundText.text = "resetting arena";
        ballScriptReference.StopMovement();
        DeactivateSparks();
        platformClock = 0;
        float resetClock = 0f;
        platformsClosing = false;
        platformsArrived = false;
        float platformFinalX = rightPlatform.transform.position.x;
        bool platformsResetting = true, ballResetting = true;
        float speed = (platformInitialX - platformFinalX) / platformResetTime;
        p1PlatformAnimator.SetInteger("STATE", 2);
        p2PlatformAnimator.SetInteger("STATE", 2);
        if(p1Instance !=null)
            p1Instance.GetComponent<Palette>().ResetPalette();
        if (p2Instance != null)
            p2Instance.GetComponent<Palette>().ResetPalette();
        //AkSoundEngine.PostEvent("sfx_openwalls", gameObject);

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

        while (resetClock < resetRoundTime || platformsResetting || ballResetting)
        {
            platformClock = 0f;
            resetClock += Time.deltaTime;
            if (resetClock > 3 * resetRoundTime / 4)
                startRoundText.text = "engage!";
            if (platformsResetting && rightPlatform.transform.position.x < platformInitialX)
            {
                leftPlatform.transform.Translate(-transform.right * speed * Time.deltaTime);
                rightPlatform.transform.Translate(transform.right * speed * Time.deltaTime);
            }
            else
            {
                rightPlatform.transform.position = new Vector2(platformInitialX, 0f);
                leftPlatform.transform.position = new Vector2(-platformInitialX, 0f);
                p1PlatformAnimator.SetInteger("STATE", 0);
                p2PlatformAnimator.SetInteger("STATE", 0);
                platformsResetting = false;
            }

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

        p1PlatformAnimator.SetInteger("STATE", 0);
        p2PlatformAnimator.SetInteger("STATE", 0);
        startRoundText.gameObject.SetActive(false);
        ballScriptReference.InitialKick(initialDirection);
        actualArrow.SetActive(false);
    }

    private void Update()
    {
        platformClock += Time.deltaTime;
        if (platformsClosing && !platformsArrived)
        {
            leftPlatform.transform.Translate(transform.right * platformSpeed * Time.deltaTime);
            rightPlatform.transform.Translate(-transform.right * platformSpeed * Time.deltaTime);
            if (leftPlatform.transform.position.x >= platformLimit)
            {
                platformsArrived = true;
                platformsClosing = false;
                p1PlatformAnimator.SetInteger("STATE", 0);
                p2PlatformAnimator.SetInteger("STATE", 0);
            }
        }
        else
        {
            if (!platformsArrived && platformClock >= platformDelay)
            {
                platformsClosing = true;
                p1PlatformAnimator.SetInteger("STATE", 1);
                p2PlatformAnimator.SetInteger("STATE", 1);
                AkSoundEngine.PostEvent("sfx_closewalls", gameObject);
            }
        }

        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
        if (p1Score >= roundsToWin)
        {
            StartCoroutine(FadeOut(true));
        }
        if (p2Score >= roundsToWin)
        {
            StartCoroutine(FadeOut(false));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCanvas.SetActive(true);
            paused = true;
            Time.timeScale = 0f;
        }

        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                AkSoundEngine.PostEvent((string)"sfx_ui_select", gameObject);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl))
                AkSoundEngine.PostEvent((string)"sfx_ui_ok", gameObject);

            if (pauseEventSystem.currentSelectedGameObject == null)
                pauseEventSystem.SetSelectedGameObject(pauseLastSelected);
            else
                pauseLastSelected = pauseEventSystem.currentSelectedGameObject;
        }
    }

    private void GameOver(bool player1won)
    {
        if (GameOverAction != null)
        {
            GameOverAction(player1won);
        }
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
        paused = false;
        Time.timeScale = 1f;
        pauseLastSelected = pauseEventSystem.firstSelectedGameObject;
        pauseCanvas.SetActive(false);
    }

    public void CharacterSelect()
    {
        Time.timeScale = 1f;
        if (CharacterSelectButton!= null)
        CharacterSelectButton(1);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        if (ExitButton != null)
            ExitButton(0);
    }
}
