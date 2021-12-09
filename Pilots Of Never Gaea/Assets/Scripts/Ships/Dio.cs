using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dio : Palette
{
    private Animator animator;
    private bool isActive;

    public float timeStopTime;
    public float knifeThrowTime;
    public float knifeSpeed;
    private float timeStopClock;
    private bool shouldThrowKnife;
    public static bool isPaused;

    public static Hawking.WellActiveFunction timeStopAction;
    public GameObject knifePrefab;
    public Transform knifeOrigin;

    public int chargesUsed;


    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        animator.SetBool("HIGHLIGHTED", true);
        isActive = false;
        shouldThrowKnife = false;
        timeStopClock = 0f;
        isPaused = false;
    }

    protected override void Update()
    {
        if (!isPaused)
        {
            Move(Time.unscaledDeltaTime);
            chargeClock += Time.deltaTime;

            if (action && power && !isActive)
            {
                isActive = true;
                timeStopClock = 0f;
                charges -= chargesUsed;
                shouldThrowKnife = true;

                if (timeStopAction != null)
                    timeStopAction.Invoke(true);

                if (charges <= 0)
                    power = false;

                if (UpdateCharges != null)
                    UpdateCharges(charges);
            }
        }

        if (isActive)
        {
            timeStopClock += Time.unscaledDeltaTime;

            if (shouldThrowKnife && timeStopTime >= knifeThrowTime)
            {
                GenericProjectile knifeThrownReference = GameObject.Instantiate(knifePrefab, knifeOrigin.position, Quaternion.identity).GetComponent<GenericProjectile>();
                knifeThrownReference.InitialImpulse(transform.right);
                shouldThrowKnife = false;
                if (!isPlayer1)
                    knifeThrownReference.transform.localScale = new Vector3(-knifeThrownReference.transform.localScale.x, knifeThrownReference.transform.localScale.y, knifeThrownReference.transform.localScale.z);
            }

            if (timeStopClock >= timeStopTime)
            {
                power = (charges >= chargesUsed && action);
                isActive = false;
                if (timeStopAction != null)
                    timeStopAction.Invoke(false);
            }
        }
    }
}
