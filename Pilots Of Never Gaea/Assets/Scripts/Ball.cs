using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rig;
    private float bounceTime = 0.1f, bounceClock;
    public float initialSpeed, bound;
    void Start()
    {
        bounceClock = 0;
        rig.AddForce((-transform.up + transform.right) * initialSpeed, ForceMode2D.Impulse);
    }

    void Update()
    {
        bounceClock += Time.deltaTime;
        if (transform.position.x >= bound || transform.position.x <= -bound)
        {
            if (bounceClock > bounceTime)
            {
                Debug.Log(rig.velocity);
                rig.velocity = new Vector2(-rig.velocity.x, rig.velocity.y);
                bounceClock = 0;
            }
        }
    }
}
