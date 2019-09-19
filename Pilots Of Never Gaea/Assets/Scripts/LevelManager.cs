using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private int p1Score, p2Score;
    public TextMeshProUGUI p1ScoreText, p2ScoreText;

    private void Start()
    {
        p1Score = 0;
        p2Score = 0;
        Ball.onScore = PlayerScored;
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
    }

    private void Update()
    {
        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
        if (p1Score >= 3)
        {

        }
        if (p2Score >= 3)
        {

        }
    }

    private void GameOver()
    {

    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}
