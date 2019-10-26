﻿using System;
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
    protected int charges;
    [HideInInspector]
    protected bool up, down, action, power;
    private float moveDelta, chargeDelay = 0.1f, chargeClock = 0f;
    //protected bool[] upgrades;
    public delegate void PaletteAction();

    protected virtual void Start()
    {
        //upgrades = new bool[4];
        sprite = transform.GetComponent<SpriteRenderer>();
        rigi = transform.GetComponent<Rigidbody2D>();        
        power = false;
        up = false;
        down = false;
        action = false;
        charges = 0;
    }

    protected virtual void Update()
    {
        //input

        if (!left)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                down = true;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                up = true;
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

        chargeClock += Time.deltaTime;
    }

    public virtual void UpdateUpgrades()
    {

    }

    public virtual void ResetPalette()
    {
        charges = 0;
        power = false;
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

    private void ChargeParentPalette()
    {
        //this.transform.parent.GetComponent<Palette>().Charge();
    }

    public void Charge()
    {
        if (chargeClock > chargeDelay)
        {
            chargeClock = 0f;
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

    //public void ActivateUpgrade(int upgrade)
    //{
    //    upgrades[upgrade] = true;
    //}    
}
