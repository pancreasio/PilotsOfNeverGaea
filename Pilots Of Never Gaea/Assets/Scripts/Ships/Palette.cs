using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Palette : MonoBehaviour
{
    public LayerMask raycastMask;

    public float speed;
    protected float moveDelta;
    public float chargeDelay = 0.1f;
    protected float moveSpeed, chargeClock = 0f;
    protected Vector2 moveDirection;
    public bool isPlayer1;
    protected SpriteRenderer sprite;
    public int chargesRequired, maxCharges;
    protected int charges;
    protected bool action, power;
    public LevelManager.ChargeAction UpdateCharges;

    protected virtual void Start()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
        power = false;
        action = false;
        charges = 0;
    }

    protected virtual void Update()
    {
        Move(Time.deltaTime);
        chargeClock += Time.deltaTime;
    }

    public virtual void ResetPalette()
    {

    }


    public void TryMoving(InputAction.CallbackContext context)
    {
        if (isPlayer1)
            moveSpeed = context.ReadValue<Vector2>().y;
        else
            moveSpeed = -context.ReadValue<Vector2>().y;
    }

    public void TryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            action = true;
        else
            if (context.canceled)
            action = false;
    }

    public void TryCancel(InputAction.CallbackContext context)
    {

    }

    public void OnDeviceLost()
    {

    }
    // private void GetInput()
    // {
    //     if (isPlayer1)
    //     {
    //         moveSpeed = Input.GetAxisRaw("P1Vertical");
    //         if (Input.GetAxisRaw("P1Submit") > 0)
    //             action = true;
    //         else
    //             action = false;
    //     }
    //     else
    //     {
    //         moveSpeed = -Input.GetAxisRaw("P2Vertical");
    //         if (Input.GetAxisRaw("P2Submit") > 0)
    //             action = true;
    //         else
    //             action = false;
    //     }
    // }

    protected void Move(float timeVariable)
    {
        moveDelta = speed * timeVariable;
        CheckBorder();
        transform.Translate(0.0f, moveDelta * moveSpeed, 0.0f);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ball")
        {
            Charge();
        }
    }

    public void KnifeHit()
    {
        charges = 0;
        power = false;

        if (UpdateCharges != null)
            UpdateCharges(charges);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Knife")
            KnifeHit();
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
                    AkSoundEngine.PostEvent("sfx_powercharge", gameObject);
                }
            }
        }
        if (UpdateCharges != null)
            UpdateCharges(charges);
    }

    protected void CheckBorder()
    {
        float moveDistance = moveDelta + sprite.bounds.extents.y;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up * moveSpeed, moveDistance, raycastMask);
        if (hit)
        {
            moveDelta = hit.distance - sprite.bounds.extents.y;
        }
    }
}