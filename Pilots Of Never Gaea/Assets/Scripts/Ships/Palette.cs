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
    public float slowTime = 1f;
    protected float moveSpeed, chargeClock = 0f;
    protected Vector2 moveDirection;
    public bool isPlayer1;
    protected SpriteRenderer sprite;
    public int chargesRequired, maxCharges;
    protected int charges;
    protected bool action, power, isSlowedDown;
    public LevelManager.ChargeAction UpdateCharges;

    protected virtual void Start()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
        power = false;
        action = false;
        isSlowedDown = false;
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

    private IEnumerator Slowdown()
    {
        float slowClock = 0;
        isSlowedDown = true;
        while(slowClock<= slowTime)
        {
            slowClock += Time.deltaTime;
            yield return null;
        }
        isSlowedDown = false;
    }

    protected void Move(float timeVariable)
    {
        moveDelta = speed * timeVariable;
        if(isSlowedDown)
            moveDelta = speed/2 * timeVariable;
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

        if (collision.transform.tag == "Hado")
            StartCoroutine(Slowdown());
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