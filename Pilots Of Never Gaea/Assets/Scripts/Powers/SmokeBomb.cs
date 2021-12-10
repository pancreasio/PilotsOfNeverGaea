using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    public float activeTime;
    public float playerActiveTime;
    public float playerScale;
    private float activeClock;
    private static int roundResetCounter = 0;
    
    private int currentRound;
    private void Start()
    {
        activeClock = 0;
        currentRound = roundResetCounter;
    }

    public void OnPlayerCollision()
    {
        activeTime = playerActiveTime;
        transform.localScale = new Vector3(playerScale, playerScale, playerScale);
    }

    private void Update()
    {
        activeClock += Time.deltaTime;
        if (activeClock >= activeTime || currentRound != roundResetCounter)
        {
            Destroy(this.gameObject);
        }


    }

    public static void OnRoundReset()
    {
        roundResetCounter++;
    }
}
