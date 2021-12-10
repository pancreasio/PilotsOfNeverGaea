using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Francesco : Palette
{
    public float macchinarioSpeedIncrease;
    private float initialSpeed;
    private float macchinarioSpeedCounter;
    public float macchinarioSlowTime;
    public float macchinarioSlowSpeed;
    private float macchinarioSlowClock;

    private Animator animator;
    private bool isActive;
    protected override void Start()
    {
        base.Start();
        initialSpeed = speed;
        animator = GetComponent<Animator>();
        isActive = false;
        macchinarioSpeedCounter = initialSpeed;
    }

    protected override void Update()
    {
        base.Update();
        if(action&&power)
        {
            charges-=chargesRequired;
            power = false;
            if(UpdateCharges!= null)
                UpdateCharges.Invoke(charges);

            isActive = true;

            macchinarioSlowClock = 0f;
            speed = macchinarioSlowSpeed;
            macchinarioSpeedCounter += macchinarioSpeedIncrease;
            animator.SetBool("IDLE", false);
            animator.SetBool("HIGHLIGHTED", true);
        }

        if(isActive)
        {
            macchinarioSlowClock += Time.deltaTime;
            if(macchinarioSlowClock>= macchinarioSlowTime)
            {
                speed = macchinarioSpeedCounter;
                isActive = false;

                animator.SetBool("HIGHLIGHTED", false);
                animator.SetBool("IDLE", true);
            }
        }
    }

    public void OnResetRound()
    {
        speed = initialSpeed;
        macchinarioSpeedCounter = initialSpeed;
        animator.SetBool("HIGHLIGHTED", false);
        animator.SetBool("IDLE", true);
    }
}
