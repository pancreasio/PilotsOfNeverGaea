using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRandomizer : MonoBehaviour
{
    public List<Sprite> backgroundList;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetRandomBackground();
    }

    private Sprite GetRandomBackground()
    {
        Sprite background = backgroundList[Random.Range(0, backgroundList.Count)];
        return background;
    }
}
