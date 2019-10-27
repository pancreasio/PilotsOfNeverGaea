﻿using System.Collections;
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
    private float targetPosition;
    private bool scrolling, scrollingDown;

    private void Start()
    {
        Time.timeScale = 1;
        currentOption = initialOption;
        scrolling = false;
        scrollRect = GetComponent<ScrollRect>();
        contenRectTransform = scrollRect.content;
    }
    private void Update()
    {
        if (!scrolling)
        {
            if (left)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    Debug.Log("scroll input");
                    if (currentOption > 1)
                    {
                        scrolling = true;
                        scrollingDown = true;
                        targetPosition = transform.position.y + scrollDistance;
                    }
                }

                if (Input.GetKey(KeyCode.W))
                {
                    Debug.Log("scroll input");
                    if (currentOption < options)
                    {
                        scrolling = true;
                        scrollingDown = false;
                        targetPosition = transform.position.y - scrollDistance;
                    }
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    Debug.Log("scroll input");
                    if (currentOption > 1)
                    {
                        scrolling = true;
                        scrollingDown = true;
                        targetPosition = transform.position.y + scrollDistance;
                    }
                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    Debug.Log("scroll input");
                    if (currentOption < options)
                    {
                        scrolling = true;
                        scrollingDown = false;
                        targetPosition = transform.position.y - scrollDistance;
                    }
                }
            }
        }
        else
        {
            if (scrollingDown)
            {
                Debug.Log("scrolling down");
                transform.position = new Vector2(transform.position.x, transform.position.y + scrollSpeed * Time.deltaTime);
                if (transform.position.y > targetPosition)
                {
                    transform.position = new Vector2(transform.position.x, targetPosition);
                    scrolling = false;
                    currentOption--;
                }
            }
            else
            {
                Debug.Log("scrolling up");
                transform.position = new Vector2(transform.position.x, transform.position.y - scrollSpeed * Time.deltaTime);
                if (transform.position.y < targetPosition)
                {
                    transform.position = new Vector2(transform.position.x, targetPosition);
                    scrolling = false;
                    currentOption++;
                }
            }
        }
    }
}
