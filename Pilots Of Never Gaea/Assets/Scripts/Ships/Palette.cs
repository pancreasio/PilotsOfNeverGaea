using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    public LayerMask raycastMask;

    public float speed;
    public bool  left;
    private SpriteRenderer sprite;
    private Rigidbody2D rigi;
    public int chargesRequired , maxCharges;
    [HideInInspector]
    protected int charges;
    [HideInInspector]
    protected bool up, down, action, power;
    private float moveDelta;

    public virtual void Start()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
        rigi = transform.GetComponent<Rigidbody2D>();        
        power = false;
        up = false;
        down = false;
        action = false;
        charges = 0;
    }

    public virtual void Update()
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
                down = true;
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
    }

    private void Move(bool upMovement)
    {
        moveDelta = speed * Time.deltaTime;
        CheckBorder(upMovement);
        if (upMovement)
        {
            transform.Translate(0.0f, moveDelta, 0.0f);
        }
        else
        {
            transform.Translate(0.0f, -moveDelta, 0.0f);
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
            if (charges < maxCharges)
            {
            charges++;
                if (charges >= chargesRequired)
                {
                    power = true;
                }
            }
        }
    }

    private void CheckBorder(bool upCheck)
    {
        float moveDistance = moveDelta + sprite.bounds.extents.y;

        if (upCheck)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveDistance, raycastMask);
            if (hit)
            {
                moveDelta = hit.distance - sprite.bounds.extents.y;
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, moveDistance, raycastMask);
            if (hit)
            {
                moveDelta = hit.distance - sprite.bounds.extents.y;
            }
        }
    }

    
}
