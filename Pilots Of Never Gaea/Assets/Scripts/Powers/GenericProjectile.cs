using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericProjectile : MonoBehaviour
{
    public float initialSpeed;

    public bool shouldDieAgainstWall;

    public bool shouldDieAgainstPlatform;

    public bool shouldDieAgainstPalette;
    public bool shouldDieAgainstBall;

    public GameManager.GameObjectFunction WallAction;
    public GameManager.GameObjectFunction BallAction;
    public GameManager.GameObjectFunction PlatformAction;
    public GameManager.GameObjectFunction PaletteAction;


    private Rigidbody2D rig;

    private void Awake()
    {
        rig = transform.GetComponent<Rigidbody2D>();
    }

    public void InitialImpulse(Vector2 impulseDirection)
    {
        rig.AddForce(impulseDirection.normalized * initialSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            if (WallAction !=null)
                WallAction.Invoke(this.gameObject);

            if (shouldDieAgainstWall)
                Destroy(this.gameObject);
        }

        if (collision.transform.tag == "Ball")
        {
            if (BallAction !=null)
                BallAction.Invoke(this.gameObject);

            if (shouldDieAgainstBall)
                Destroy(this.gameObject);
        }

        if (collision.transform.tag == "Palette")
        {
            if (PaletteAction !=null)
                PaletteAction.Invoke(this.gameObject);

            if (shouldDieAgainstPalette)
                Destroy(this.gameObject);
        }

        if (collision.transform.tag == "Platform")
        {
            if (PlatformAction !=null)
                PlatformAction.Invoke(this.gameObject);

            if (shouldDieAgainstPlatform)
                Destroy(this.gameObject);
        }
    }
}
