using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rig;
    public delegate void OnScoreAction(bool player1);
    public static OnScoreAction onScore;
    public float initialSpeed, horizontalBounds, verticalBounds, bounceMultiplier;
    private void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
        ReStart();
    }

    private void ReStart()
    {
        transform.position = new Vector2(0.0f, 0.0f);
        rig.velocity = new Vector2(0.0f, 0.0f);
        rig.AddForce((Vector2.down + Vector2.left) * initialSpeed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (transform.position.x >= horizontalBounds)
        {
            if (onScore != null)
            {
                onScore(true);
            }
            ReStart();
        }
        if (transform.position.x <= -horizontalBounds)
        {
            if (onScore != null)
            {
                onScore(false);
            }
            ReStart();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Laser")
        {
            VerticalBounce();
        }
    }
}
