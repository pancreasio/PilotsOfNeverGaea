using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int p1Score, p2Score;

    void Start()
    {
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

    void Update()
    {
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
}
