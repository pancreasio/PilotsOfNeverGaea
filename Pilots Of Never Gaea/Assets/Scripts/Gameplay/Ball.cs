﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rig;
    public delegate void OnScoreAction(bool player1);
    public static OnScoreAction onScore;
    public static LevelManager.LevelAction ElectrifyAction, UnstickAction;
    public float initialSpeed, horizontalBounds, verticalBounds, bounceMultiplier, stunTime;
    private float stunClock, stuckYPosition, stuckSpeed;
    private bool stunned, charged, stuck;
    private Vector2 stunnedPosition;

    private void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
        stunned = false;
        stunClock = 0;
        charged = false;
        stuck = false;
    }

    private void Update()
    {
        if (stunned)
        {
            stunClock += Time.deltaTime;
            transform.position = stunnedPosition;
            if (stunClock >= stunTime)
            {
                stunned = false;
            }
        }

        if (stuck)
        {
            rig.velocity = new Vector2(stuckSpeed, 0.0f);
            transform.position = new Vector2(transform.position.x, stuckYPosition);
        }
    }

    private void HorizontalBounce()
    {
        rig.velocity = new Vector2(-rig.velocity.x, rig.velocity.y);
    }

    private void VerticalBounce()
    {
        rig.velocity = new Vector2(rig.velocity.x, -rig.velocity.y);
    }

    public void StopMovement()
    {
        stuck = false;
        charged = false;
        rig.velocity = Vector2.zero;
    }

    public void InitialKick(Vector2 kickDirection)
    {
        rig.AddForce(kickDirection * initialSpeed, ForceMode2D.Impulse);
    }

    private void HitStun()
    {
        stunnedPosition = transform.position;
        stunned = true;
        stunClock = 0;
    }

    private void Stick()
    {
        stuck = true;
        stuckYPosition = transform.position.y;
        stuckSpeed = rig.velocity.magnitude * rig.velocity.x / Mathf.Abs(rig.velocity.x);
    }

    private void Unstick()
    {
        stuck = false;
        charged = false;
        if (UnstickAction != null)
            UnstickAction();
        if (transform.position.y > 0)
        {
            BounceToCenter(false);
        }
        else
        {
            BounceToCenter(true);
        }
    }

    private void BounceToCenter(bool up)
    {
        rig.velocity = new Vector2(0.0f, 0.0f);
        if (transform.position.x > 0)
        {
            if (!up)
            {
                rig.AddForce((Vector2.down + Vector2.left) * initialSpeed, ForceMode2D.Impulse);
            }
            else
            {
                rig.AddForce((Vector2.up + Vector2.left) * initialSpeed, ForceMode2D.Impulse);
            }
        }
        else
        {
            if (!up)
            {
                rig.AddForce((Vector2.down + Vector2.right) * initialSpeed, ForceMode2D.Impulse);
            }
            else
            {
                rig.AddForce((Vector2.up + Vector2.right) * initialSpeed, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.transform.tag;
        if (collisionTag == "Laser")
        {
            HitStun();
            VerticalBounce();
        }

        if (collisionTag == "Ion Wave")
        {
            HitStun();
            if (ElectrifyAction != null)
                ElectrifyAction();
            charged = true;
        }

        if (collisionTag == "Platform")
        {
            if (transform.position.x >= 0)
            {
                if (onScore != null)
                {
                    onScore(true);
                }
            }
            if (transform.position.x <= 0)
            {
                if (onScore != null)
                {
                    onScore(false);
                }
            }
        }

        if (collisionTag == "Corner")
        {
            if (stuck)
                Unstick();
            CornerBounce(collision.gameObject);
        }
    }

    private void CornerBounce(GameObject corner)
    {
        if (corner.transform.position.y > corner.transform.parent.position.y)
        {
            BounceToCenter(true);
        }
        else
        {
            BounceToCenter(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionTag = collision.transform.tag;
        if (collisionTag == "Wall" && charged)
        {
            Stick();
        }

        if ((collisionTag == "Palette") && stuck)
        {
            Unstick();
        }
    }
}
