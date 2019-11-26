using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illusion : MonoBehaviour
{
    public float initialSpeed;
    private Rigidbody2D rig;

    private void Awake()
    {
        rig = transform.GetComponent<Rigidbody2D>();
    }

    public void InitialKick(Vector2 kickDirection)
    {
        rig.AddForce(kickDirection.normalized * initialSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
                rig.velocity = new Vector2(rig.velocity.x, -rig.velocity.y).normalized * initialSpeed;
        }

        if (collision.transform.tag == "Platform")
        {
            if (transform.position.x > 0)
            {
                if (rig.velocity.y > 0)
                {
                    rig.velocity = (Vector2.left + Vector2.up).normalized * initialSpeed;
                }
                else
                {
                    rig.velocity = (Vector2.left + Vector2.down).normalized * initialSpeed;
                }
            }
            else
            {
                if (rig.velocity.y > 0)
                {
                    rig.velocity = (Vector2.right + Vector2.up).normalized * initialSpeed;
                }
                else
                {
                    rig.velocity = (Vector2.right + Vector2.down).normalized * initialSpeed;
                }
            }
        }

        {
            if (collision.transform.tag == "Palette")
                Destroy(this.gameObject);
        }
    }

}