using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool top;
    public float activeTime;
    private float activeClock;
    private SpriteRenderer sprite;
    private BoxCollider2D collider;
    private void Start()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
        collider = transform.GetComponent<BoxCollider2D>();
        activeClock = 0;
        if (top)
        {
            sprite.color = Color.blue;
        }
        else
        {
            sprite.color = Color.red;
        }
    }

    private void Update()
    {
        activeClock += Time.deltaTime;
        if (activeClock >= activeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ball")
        {
            collider.enabled = false;
        }
    }
}
