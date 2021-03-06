﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonWave : MonoBehaviour
{
    public float activeTime;
    private float activeClock;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = transform.GetComponent<BoxCollider2D>();
        activeClock = 0;
    }

    private void Update()
    {
        activeClock += Time.deltaTime;
        if (activeClock >= activeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ball")
        {
            boxCollider.enabled = false;
        }
    }
}
