using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed, rightBound, leftBound;
    public bool power, top;
    private SpriteRenderer sprite;
    public Laser laser;
    private bool left, right, action;

    void Start()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
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

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                action = true;
            }
            else
            {
                action = false;
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

            if (Input.GetKeyDown(KeyCode.W))
            {
                action = true;
            }
            else
            {
                action = false;
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
        if (action && power)
        {
            Fire();
            power = false;
            action = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ball")
        {
            Charge();
        }
    }

    private void Charge()
    {
        if (!power)
        {
            if (top)
            {
                sprite.color = Color.blue;
            }
            else
            {
                sprite.color = Color.red;
            }
            power = true;
        }
    }

    private void Fire()
    {
        sprite.color = Color.white;
        power = false;
        if (top)
        {
            Laser laserInstance = Instantiate(laser,
                new Vector3(transform.position.x, -0.88f),
                new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + 180.0f, transform.rotation.w));
            laserInstance.top = true;
        }
        else
        {
            Instantiate(laser, new Vector3(transform.position.x, 0.88f), transform.rotation);
        }
    }
}
