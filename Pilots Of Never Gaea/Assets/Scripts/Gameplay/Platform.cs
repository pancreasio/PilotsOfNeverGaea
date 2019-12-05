using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float closeLimit, closingSpeed, resetTime;
    private Animator animator;
    private float initialPosition;
    private bool closing = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position.x;
        animator.SetInteger("STATE", 0);
    }

    private void Update()
    {
        if (closing)
        {
            transform.Translate(transform.right * closingSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x) < closeLimit)
            {
                closing = false;
                animator.SetInteger("STATE", 0);
            }
        }
    }

    private IEnumerator ResetPLatformRoutine()
    {
        float resetClock = 0f;
        float speed = (initialPosition - transform.position.x)/ resetTime;
        animator.SetInteger("STATE", 2);
        while (resetClock < resetTime)
        {
            resetClock += Time.deltaTime;
            transform.Translate(transform.right * speed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector2(initialPosition, transform.position.y);
        animator.SetInteger("STATE", 0);        
    }

    public void ResetPlatform()
    {
        closing = false;
        StartCoroutine(ResetPLatformRoutine());
    }

    public void ClosePlatform()
    {
        animator.SetInteger("STATE", 1);
        AkSoundEngine.PostEvent("sfx_closewalls", gameObject);
        closing = true;
    }
}
