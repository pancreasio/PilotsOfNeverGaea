using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    public LayerMask raycastMask;

    public float speed;
    private float moveDelta, chargeDelay = 0.1f, chargeClock = 0f;
    protected float moveSpeed;
    protected Vector2 moveDirection;
    public bool isPlayer1;
    private SpriteRenderer sprite;
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
        GetInput();
        Move();
        chargeClock += Time.deltaTime;
    }

    public virtual void ResetPalette()
    {

    }

    private void GetInput()
    {
        if (isPlayer1)
        {
            moveSpeed = Input.GetAxisRaw("P1Vertical");
            if (Input.GetAxisRaw("P1Submit") > 0)
                action = true;
            else
                action = false;
        }
        else
        {
            moveSpeed = -Input.GetAxisRaw("P2Vertical");
            if (Input.GetAxisRaw("P2Submit") > 0)
                action = true;
            else
                action = false;
        }
    }

    private void Move()
    {
        moveDelta = speed * Time.deltaTime;
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

    private void CheckBorder()
    {
        float moveDistance = moveDelta + sprite.bounds.extents.y;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up * moveSpeed, moveDistance, raycastMask);
        if (hit)
        {
            moveDelta = hit.distance - sprite.bounds.extents.y;
        }
    }
}