using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask raycastMask;

    public float speed, rightBound, leftBound;
    public bool power, top;
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
            moveDelta = speed * Time.deltaTime;
            CheckBorder(true);
            transform.Translate(moveDelta, 0.0f, 0.0f);
            Debug.DrawRay(transform.position, transform.up * 3);
            right = false;
        }
        if (left)
        {
            moveDelta = speed * Time.deltaTime;
            CheckBorder(false);
            transform.Translate(-moveDelta, 0.0f, 0.0f);
            Debug.DrawRay(transform.position, transform.up * 3);
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
