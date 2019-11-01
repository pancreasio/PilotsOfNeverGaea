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
            if (transform.position.y > 0)
                rig.velocity = (rig.velocity + Vector2.down).normalized * initialSpeed;
            else
                rig.velocity = (rig.velocity + Vector2.up).normalized * initialSpeed;
        }
        else
        {
            if (collision.transform.tag == "Palette" || collision.transform.tag == "Platform")
                Destroy(this.gameObject);
        }

    }
}
