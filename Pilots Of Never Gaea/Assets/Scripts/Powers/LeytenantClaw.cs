using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeytenantClaw : MonoBehaviour
{
    public Leytenant leytenant;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            leytenant.CatchBall(collision.GetComponent<Ball>());
        }
    }
}
