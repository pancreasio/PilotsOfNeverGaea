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
    public GameObject gameOverUI, leftPlatform, rightPlatform, p1Light, p2Light,
        ballPrefab, magPrefab, railPrefab, kunstPrefab,
        p1Position, p2Position;
    private GameObject ballReference, topSparks, bottomSparks, p1Instance = null, p2Instance = null;
    public static GameManager.ButtonAction RetryAction, BackToSelectAction;
    public static GameManager.SceneChange ExitAction;
    public static CharacterSelectionManager.Character p1Selected, p2Selected;
    public Shake2D cameraShake;
    public delegate void LevelAction();
    public float platformDelay, platformSpeed, platformLimit, cameraShakeDuration, cameraShakeIntensity;
    private float platformClock, platformInitialX, playerInitialX;
    private bool platformsClosing, platformsArrived;

    private void Start()
    {
        Ball.ElectrifyAction = ActivateSparks;
        Ball.UnstickAction = DeactivateSparks;
        playerInitialX = p2Position.transform.position.x;
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
        ballReference = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
        InitializeCharacters();
    }

    private void InitializeCharacters()
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
    }

    private void PlayerScored(bool player1)
    {

        StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeIntensity));

        if (player1)
        {
            p1Score++;
        }
        else
        {
            p2Score++;
        }
        ResetRound();
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

    private void ResetRound()
    {
        Destroy(ballReference.gameObject);
        DeactivateSparks();
        rightPlatform.transform.position = new Vector2(platformInitialX, 0.0f);
        leftPlatform.transform.position = new Vector2(-platformInitialX, 0.0f);
        ballReference = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
        platformClock = 0;
        platformsClosing = false;
        platformsArrived = false;
        p1Instance.transform.position = new Vector2(-playerInitialX, 0.0f);
        p2Instance.transform.position = new Vector2(playerInitialX, 0.0f);
        p1Instance.GetComponent<Palette>().ResetPalette();
        p2Instance.GetComponent<Palette>().ResetPalette();
        p1Instance.GetComponent<Palette>().UpdateUpgrades();
        p2Instance.GetComponent<Palette>().UpdateUpgrades();
    }

    private void Update()
    {
        platformClock += Time.deltaTime;
        if (platformsClosing)
        {
            leftPlatform.transform.Translate(transform.right * platformSpeed * Time.deltaTime);
            rightPlatform.transform.Translate(-transform.right * platformSpeed * Time.deltaTime);
            if (leftPlatform.transform.position.x >= platformLimit)
            {
                platformsArrived = true;
                platformsClosing = false;
            }
        }
        else
        {
            if (!platformsArrived && platformClock >= platformDelay)
                platformsClosing = true;
        }

        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
        if (p1Score >= roundsToWin)
        {
                GameOver(true);
        }
        if (p2Score >= roundsToWin)
        {
                GameOver(false);
        }
    }

    private void GameOver(bool player1won)
    {
        gameOverUI.SetActive(true);
        if (player1won)
        {
            p2WinText.gameObject.SetActive(false);
        }
        else
        {
            p1WinText.gameObject.SetActive(false);
        }
        Time.timeScale = 0;
    }

    public void Retry()
    {
        if (RetryAction != null)
            RetryAction();
    }

    public void BackToSelection()
    {
        if (BackToSelectAction != null)
        {
            p1Selected = CharacterSelectionManager.Character.nullCharacter;
            p2Selected = CharacterSelectionManager.Character.nullCharacter;
            BackToSelectAction();
        }
    }

    public void Menu()
    {
        if (ExitAction != null)
            ExitAction(0);
    }
}
