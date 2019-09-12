using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Palette : MonoBehaviour
{
    public LayerMask raycastMask;

    public float speed, rightBound, leftBound;
    public bool power, left, mobile;
    public Laser laser;
    private SpriteRenderer sprite;
    private Rigidbody2D rigi;
    private bool up, down, action;
    private float moveDelta;

    private void Start()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
        rigi = transform.GetComponent<Rigidbody2D>();
        power = false;
        up = false;
        down = false;
        action = false;
    }

    private void Update()
    {
        //input

        if (!left)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                up = true;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                down= true;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
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
            if (Input.GetKey(KeyCode.W))
            {
                up = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                down = true;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                action = true;
            }
            else
            {
                action = false;
            }
        }

        //movement
        if (up)
        {
            Move(true);
            up = false;
        }
        if (down)
        {
            Move(false);
            down = false;
        }
        if (action)
        {
            Fire();
            action = false;
        }
    }

    private void Move(bool upMovement)
    {
        moveDelta = speed * Time.deltaTime;
        CheckBorder(upMovement);
        if (upMovement)
        {
            transform.Translate(-moveDelta, 0.0f, 0.0f);
        }
        else
        {
            transform.Translate(moveDelta, 0.0f, 0.0f);
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
            if (left)
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

    private void CheckBorder(bool upCheck)
    {
        float moveDistance = moveDelta + sprite.bounds.extents.x;

        if (upCheck)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, moveDistance, raycastMask);
            if (hit)
            {
                Debug.DrawRay(transform.position, transform.right);
                moveDelta = hit.distance - sprite.bounds.extents.x;
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, moveDistance, raycastMask);
            if (hit)
            {
                Debug.DrawRay(transform.position, -transform.right);
                moveDelta = hit.distance - sprite.bounds.extents.x;
            }
        }
    }

    private void Fire()
    {
        if (power)
        {
            sprite.color = Color.white;
            power = false;
            if (left)
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
}
