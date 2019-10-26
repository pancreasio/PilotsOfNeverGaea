using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AutoScroll : MonoBehaviour
{
    public float scrollSpeed, scrollDistance;
    private ScrollRect scrollRect;
    public bool left;
    public int options, initialOption;
    private int currentOption;
    private RectTransform contenRectTransform;
    private void Start()
    {
        currentOption = initialOption;
        this.scrollRect = GetComponent<ScrollRect>();
        this.contenRectTransform = this.scrollRect.content;
    }
    private void Update()
    {
        
        if (left)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Scroll(false);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Scroll(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Scroll(false);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Scroll(true);
            }
        }
        
    }

    private void Scroll(bool up)
    {
        Vector2 pos = scrollRect.content.localPosition;
        if (up)
        {
            if (currentOption < options)
            {
                pos.y -= scrollDistance;
                currentOption++;
            }
        }
        else
        {
            if (currentOption > 1)
            {
                pos.y += scrollDistance;
                currentOption--;
            }
        }
        scrollRect.content.localPosition = pos;
    }
}
