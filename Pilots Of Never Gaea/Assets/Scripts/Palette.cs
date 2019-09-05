using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Palette : MonoBehaviour
{
    public LayerMask raycastMask;

    public float speed, rightBound, leftBound;
    public bool power, top, mobile;
    public Laser laser;
    private SpriteRenderer sprite;
    private Rigidbody2D rigi;
    private bool left, right, action;
    private float moveDelta;

    private void Start()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
        rigi = transform.GetComponent<Rigidbody2D>();
        power = false;
        left = false;
        right = false;
        action = false;
    }

    private void Update()
    {
        //input
        if (mobile)
        {
            if (top)
            {
                if (CrossPlatformInputManager.GetAxisRaw("Vertical")>0)
                {
                    right = true;
                }

                if (CrossPlatformInputManager.GetAxisRaw("Vertical")<0)
                {
                    left = true;
                }

                if (CrossPlatformInputManager.GetAxisRaw("Fire2")>0)
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
                if (CrossPlatformInputManager.GetAxisRaw("Horizontal") > 0)
                {
                    right = true;
                }

                if (CrossPlatformInputManager.GetAxisRaw("Horizontal") < 0)
                {
                    left = true;
                }

                if (CrossPlatformInputManager.GetAxisRaw("Fire1") > 0)
                {
                    action = true;
                }
                else
                {
                    action = false;
                }
            }
        }

        else
        {
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
        }
        //movement
        if (right)
        {
            Move(true);
            right = false;
        }
        if (left)
        {
            Move(false);
            left = false;
        }
        if (action)
        {
            Fire();
            action = false;
        }
    }

    private void Move(bool right)
    {
        moveDelta = speed * Time.deltaTime;
        CheckBorder(right);
        if (right)
        {
            transform.Translate(moveDelta, 0.0f, 0.0f);
        }
        else
        {
            transform.Translate(-moveDelta, 0.0f, 0.0f);
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

    private void CheckBorder(bool right)
    {
        float moveDistance = moveDelta + sprite.bounds.extents.x;

        if (right)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, moveDistance, raycastMask);
            if (hit)
            {
                moveDelta = hit.distance - sprite.bounds.extents.x;
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, moveDistance, raycastMask);
            if (hit)
            {
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
}
