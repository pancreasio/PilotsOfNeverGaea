using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rig;
    public delegate void OnScoreAction(bool player1);
    public static OnScoreAction onScore;
    public static LevelManager.LevelAction ElectrifyAction, UnstickAction;
    public float initialSpeed, stunTime, minXVelocity, correctionSpeed;
    private float stunClock, stuckYPosition, stuckSpeed;
    private bool stunned, charged, stuck, shot, moving;
    private Vector2 stunnedPosition;

    private void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
        stunned = false;
        stunClock = 0;
        charged = false;
        stuck = false;
        shot = false;
        moving = false;
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

        if (moving && Mathf.Abs(rig.velocity.x) < minXVelocity)
            rig.AddForce(Vector2.right * Mathf.Sign(rig.velocity.x) * correctionSpeed, ForceMode2D.Impulse);

    }

    private void FixedUpdate()
    {

    }

    public void StopMovement()
    {
        stuck = false;
        charged = false;
        rig.velocity = Vector2.zero;
        moving = false;
    }

    public void InitialKick(Vector2 kickDirection)
    {
        rig.velocity = Vector2.zero;
        rig.AddForce(kickDirection.normalized * initialSpeed, ForceMode2D.Impulse);
        AkSoundEngine.PostEvent("sfx_startball", gameObject);
        moving = true;
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
                rig.AddForce((Vector2.down + Vector2.left).normalized * initialSpeed, ForceMode2D.Impulse);
            }
            else
            {
                rig.AddForce((Vector2.up + Vector2.left).normalized * initialSpeed, ForceMode2D.Impulse);
            }
        }
        else
        {
            if (!up)
            {
                rig.AddForce((Vector2.down + Vector2.right).normalized * initialSpeed, ForceMode2D.Impulse);
            }
            else
            {
                rig.AddForce((Vector2.up + Vector2.right).normalized * initialSpeed, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.transform.tag;
        if (collisionTag == "Laser")
        {
            HitStun();
            rig.velocity = new Vector2(rig.velocity.x, -rig.velocity.y);
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
            AkSoundEngine.PostEvent("sfx_goal", gameObject);
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
            AkSoundEngine.PostEvent("sfx_palette_bounce", gameObject);
            if (stuck)
                Unstick();
            CornerBounce(collision.gameObject);
        }

        if (collisionTag == "FrontShot")
        {
            HitStun();
            shot = true;
            if (transform.position.x < 0)
            {
                StopMovement();
                rig.AddForce((new Vector2(1f, 0f).normalized * initialSpeed * 1.9f), ForceMode2D.Impulse);

            }
            else
            {
                StopMovement();
                rig.AddForce((new Vector2(-1f, 0f).normalized * initialSpeed * 1.9f), ForceMode2D.Impulse);
            }
        }

        if (collisionTag == "SideShot")
        {
            HitStun();
            CornerBounce(collision.gameObject);
        }

        if (collisionTag == "Reply")
        {
            BounceToCenter(transform.position.y > collision.transform.position.y);
            rig.velocity *= 1.4f;
            shot = true;
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
        if (collisionTag == "Wall")
        {
            AkSoundEngine.PostEvent("sfx_wall", gameObject);
            if (charged)
                Stick();
        }

        if ((collisionTag == "Palette"))
        {
            AkSoundEngine.PostEvent("sfx_palette_bounce", gameObject);
            if (stuck)
                Unstick();
            if (shot)
            {
                BounceToCenter(transform.position.y > collision.transform.position.y);
                shot = false;
            }
        }
    }

    public void ArtilleryShot()
    {
        rig.velocity *= 1.4f;
        shot = true;
    }
}
