using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    private int p1Score, p2Score;
    public int roundsToWin;
    public TextMeshProUGUI p1ScoreText, p2ScoreText, p1WinText, p2WinText;
    public GameObject leftPlatform, rightPlatform, p1Light, p2Light,
        ballPrefab, magPrefab, railPrefab, kunstPrefab, knockoutPrefab, djinnPrefab,
        p1Position, p2Position;
    public SpriteRenderer fadeoutSprite;
    private GameObject ballReference = null, topSparks, bottomSparks, p1Instance = null, p2Instance = null;
    private Ball ballScriptReference;
    public static GameManager.GameOverFunction GameOverAction;
    public static CharacterSelectionManager.Character p1Selected, p2Selected;
    public Animator p1PlatformAnimator, p2PlatformAnimator;
    public Shake2D cameraShake;
    public delegate void LevelAction();
    public float startRoundTime, resetRoundTime, platformDelay, platformSpeed, platformResetTime, ballResetSpeed, platformLimit, cameraShakeDuration, cameraShakeIntensity, fadeoutTime;
    private float platformClock, platformInitialX;
    private bool platformsClosing, platformsArrived;

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
        Ball.onScore = PlayerScored;
        Time.timeScale = 1;
        platformInitialX = rightPlatform.transform.position.x;
        p1PlatformAnimator.SetInteger("STATE", 0);
        p2PlatformAnimator.SetInteger("STATE", 0);
        InitializeGame();
        StartCoroutine(StartRound());
    }

    private void InitializeGame()
    {
        switch (p1Selected)
        {
            case CharacterSelectionManager.Character.nullCharacter:
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
            default:
                break;
        }

        if (p1Instance != null)
        {
            p1Instance.GetComponent<Palette>().left = true;
            p1Instance.transform.position = p1Position.transform.position;
            p1Light.transform.parent = p1Instance.transform;
        }

        switch (p2Selected)
        {
            case CharacterSelectionManager.Character.nullCharacter:
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
            default:
                break;
        }

        if (p2Instance != null)
        {
            p2Instance.GetComponent<Palette>().left = false;
            p2Instance.transform.position = p2Position.transform.position;
            p2Instance.transform.Rotate(Vector3.back, 180.0f);
            p2Light.transform.parent = p2Instance.transform;
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
            StartCoroutine(ResetRound(true));
        }
        else
        {
            p2Score++;
            StartCoroutine(ResetRound(false));
        }
    }

    private IEnumerator FadeOut(bool player1Won)
    {
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
        float startClock = 0f;
        while (startClock < startRoundTime)
        {
            startClock += Time.deltaTime;
            yield return null;
        }
        ballScriptReference.InitialKick(Vector2.down + Vector2.left);
    }

    private IEnumerator ResetRound(bool player1Scored)
    {
        ballScriptReference.StopMovement();
        DeactivateSparks();
        platformClock = 0;
        float resetClock = 0f;
        platformsClosing = false;
        platformsArrived = false;
        //p1Instance.GetComponent<Palette>().ResetPalette();
        //p2Instance.GetComponent<Palette>().ResetPalette();
        float platformFinalX = rightPlatform.transform.position.x;
        bool platformsResetting = true, ballResetting = true;
        float speed = (platformInitialX - platformFinalX) / platformResetTime;
        p1PlatformAnimator.SetInteger("STATE", 2);
        p2PlatformAnimator.SetInteger("STATE", 2);
        while (resetClock < resetRoundTime || platformsResetting || ballResetting)
        {
            platformClock = 0f;
            resetClock += Time.deltaTime;
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
            }

            yield return null;
        }

        Vector2 initialVector;
        if (Mathf.FloorToInt(Time.fixedTime) % 2 == 0)
            initialVector = Vector2.up;
        else
            initialVector = Vector2.down;

        if (player1Scored)
            ballScriptReference.InitialKick(initialVector + Vector2.left);
        else
            ballScriptReference.InitialKick(initialVector + Vector2.right);

        p1PlatformAnimator.SetInteger("STATE", 0);
        p2PlatformAnimator.SetInteger("STATE", 0);
    }

    private void Update()
    {
        platformClock += Time.deltaTime;
        if (platformsClosing && !platformsArrived)
        {
            //p1PlatformAnimator.SetInteger("STATE", 1);
            //p2PlatformAnimator.SetInteger("STATE", 1);
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
    }

    private void GameOver(bool player1won)
    {
        if (GameOverAction != null)
        {
            GameOverAction(player1won);
        }
    }
}
