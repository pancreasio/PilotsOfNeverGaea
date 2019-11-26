using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float fadeTime;
    private float fadeClock;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeClock = fadeTime;
    }

    private void Update()
    {
        fadeClock -= Time.deltaTime;
        spriteRenderer.color = new Color(255f, 255f, 255f, fadeClock / fadeTime);
        if (fadeClock < 0f)
            Destroy(this.gameObject);
    }
}
