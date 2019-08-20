using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed, rightBound, leftBound;
    public bool power, top;
    private bool left, right, action;

    void Start()
    {
        power = false;
        left = false;
        right = false;
        action = false;
    }

    void Update()
    {
        //input
        if (top)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                right = true;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                left = true;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                action = true;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                right = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                left = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                action = true;
            }
        }

        //movement
        if (right)
        {
            if (transform.position.x < rightBound)
            {
                transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
            }
            right = false;
        }
        if (left)
        {
            if (transform.position.x > leftBound)
            {
                transform.Translate(-speed * Time.deltaTime, 0.0f, 0.0f);
            }
            left = false;
        }
        if (Input.GetKeyDown(KeyCode.W) && power)
        {
            Fire();
            power = false;
            action = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ball")
        {
            power = true;
        }
    }

    private void Fire()
    {

    }
}
