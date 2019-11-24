using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockout : Palette
{
    private Animator animator;
    public GameObject frontShot, sideShot, frontOrigin, topOrigin, bottomOrigin;

    protected override void Start()
    {
        base.Start();
        animator = transform.GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (power)
        {
            animator.SetBool("POWER", true);
            if (action)
            {
                animator.SetTrigger("SHOOT");
                power = false;
                charges -= chargesRequired;
                action = false;
                if (UpdateCharges != null)
                    UpdateCharges(charges);
            }
        }
        else
        {
            animator.SetBool("POWER", false);
        }
    }

    public void FrontShot()
    {
        Instantiate(frontShot, frontOrigin.transform.position, transform.rotation);
    }

    public void UpShot()
    {
        GameObject downInstance = Instantiate(sideShot, topOrigin.transform.position, transform.rotation, topOrigin.transform);
    }

    public void DownShot()
    {
        GameObject downInstance = Instantiate(sideShot, bottomOrigin.transform.position, transform.rotation, bottomOrigin.transform);
        downInstance.transform.Rotate(Vector3.back, 180.0f);
    }
}
