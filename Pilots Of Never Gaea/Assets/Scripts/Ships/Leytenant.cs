﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leytenant : Palette
{
    public float speedReduction, recieveTime, holdTime, cannonRotationSpeed;
    private bool shooting, recieving;
    public GameObject cannon;
    public Transform cannonVector;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        recieving = false;
        shooting = false;
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (power && action)
            StartCoroutine(Artillery());
    }

    private IEnumerator Artillery()
    {
        power = false;
        action = false;
        charges -= chargesRequired;
        UpdateCharges(charges);
        speed = speed / speedReduction;
        float recieveClock = 0f;
        while (recieveClock < recieveTime)
        {
            recieveClock += Time.deltaTime;
            recieving = true;
            yield return null;
        }
        recieving = false;
        speed *= speedReduction;
    }

    private IEnumerator Arm(GameObject ball)
    {
        Vector2 armPosition = transform.position;
        float holdClock = 0f, rotation = 0f;
        cannon.SetActive(true);
        cannon.transform.rotation = Quaternion.identity;
        if (!left)
        {
            cannon.transform.Rotate(Vector3.back, 180f);
        }
        while (holdClock < holdTime)
        {
            holdClock += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, armPosition.y);
            ball.transform.position = transform.position;
            rotation = 0f;
            if (up)
                rotation = -cannonRotationSpeed;

            if (down)
                rotation = cannonRotationSpeed;

            cannon.transform.Rotate(Vector3.back, rotation * Time.deltaTime);
            yield return null;
        }
        ball.GetComponent<Ball>().InitialKick(cannonVector.position - cannon.transform.position);
        ball.GetComponent<Ball>().ArtilleryShot();
        cannon.SetActive(false);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!recieving && !shooting)
            base.OnCollisionEnter2D(collision);

        else
        {
            if (collision.transform.tag == "Ball" && recieving)
            {
                StartCoroutine(Arm(collision.gameObject));
            }
        }
    }

}