using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    private int p1Score, p2Score;
    public TextMeshProUGUI p1ScoreText, p2ScoreText, p1WinText, p2WinText;
    public GameObject gameOverUI, leftPlatform, rightPlatform, ballPrefab;
    private GameObject ballReference;
    public static GameManager.ButtonAction RetryAction;
    public static GameManager.SceneChange ExitAction;
    public float platformDelay, platformSpeed, platformLimit;
    private float platformClock, platformInitialX;
    private bool platformsClosing, platformsArrived;

    private void Start()
    {
        p1Score = 0;
        p2Score = 0;
        platformClock = 0;
        platformsClosing = false;
        platformsArrived = false;
        Ball.onScore = PlayerScored;
        Time.timeScale = 1;
        platformInitialX = rightPlatform.transform.position.x;
        ballReference = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
    }

    private void PlayerScored(bool player1)
    {
        if (player1)
        {
            p1Score++;
        }
        else
        {
            p2Score++;
        }
        RoundEnd();
    }

    private void RoundEnd()
    {
        Destroy(ballReference.gameObject);
        rightPlatform.transform.position = new Vector2(platformInitialX, 0.0f);
        leftPlatform.transform.position = new Vector2(-platformInitialX, 0.0f);
        ballReference = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
        platformClock = 0;
        platformsClosing = false;
        platformsArrived = false;
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
        if (p1Score >= 3)
        {
            gameOverUI.SetActive(true);
            p2WinText.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
        if (p2Score >= 3)
        {
            gameOverUI.SetActive(true);
            p1WinText.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
    }

    private void GameOver()
    {

    }

    public void Retry()
    {
        if (RetryAction != null)
            RetryAction();
    }

    public void Menu()
    {
        if (ExitAction != null)
            ExitAction(0);
    }
}
