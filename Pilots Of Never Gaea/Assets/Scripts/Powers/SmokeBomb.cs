using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    public float activeTime;
    public float playerActiveTime;
    public float playerScale;
    private float activeClock;
    private void Start()
    {
        activeClock = 0;
    }

    public void OnPlayerCollision()
    {
        activeTime = playerActiveTime;
        transform.localScale = new Vector3(playerScale, playerScale, playerScale);
    }

    private void Update()
    {
        activeClock += Time.deltaTime;
        if (activeClock >= activeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
