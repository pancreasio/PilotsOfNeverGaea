using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteCorner : MonoBehaviour
{
    private float hitClock = 0.0f, hitDelay = 0.25f;
    private bool hit = false;
    public Palette parentPalette;
    private void Update()
    {
        if (hit)
        {
            hitClock += Time.deltaTime;
            if (hitClock >= hitDelay)
            {
                hit = false;
                hitClock = 0.0f;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hit && collision.transform.tag == "Ball")
        {
            parentPalette.Charge();
            hit = true;
        }

        if(collision.transform.tag == "Knife")
        {
            parentPalette.KnifeHit();
        }
    }
}
