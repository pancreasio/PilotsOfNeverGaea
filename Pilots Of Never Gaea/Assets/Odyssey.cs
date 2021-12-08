using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odyssey : Palette
{
    private bool isUsingVoyage;
    public float voyageTime;
    public float voyageSpeed;
    private float voyageClock;

    public delegate void VoyageFunction(GameObject ship, float speed);
    public static VoyageFunction VoyageAction;
    public Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        isUsingVoyage = false;
    }

    protected override void Update()
    {
        base.Update();
        if (action && power && !isUsingVoyage)
        {
            isUsingVoyage = true;
            voyageClock = 0f;
            animator.SetBool("IDLE", false);
            animator.SetBool("HIGHLIGHTED", true);
            charges -= chargesRequired;
            if(charges<=0)
                power = false;
            if (UpdateCharges != null)
                    UpdateCharges(charges);
        }

        if (isUsingVoyage)
        {
            voyageClock += Time.deltaTime;
            if (VoyageAction != null)
                VoyageAction.Invoke(this.gameObject, voyageSpeed*Time.deltaTime);

            if (voyageClock >= voyageTime)
            {
                isUsingVoyage = false;
                animator.SetBool("HIGHLIGHTED", false);
                animator.SetBool("IDLE", true);
            }
        }
    }
}
