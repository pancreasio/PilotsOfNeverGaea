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
        //if (up)
        //{
        //    animator.SetBool("UP", true);
        //}
        //else
        //{
        //    animator.SetBool("UP", false);
        //    if (down)
        //        animator.SetBool("DOWN", true);
        //    else
        //        animator.SetBool("DOWN", false);
        //}
        if (power)
        {
            animator.SetBool("POWER", true);
            if (action)
            {
                animator.SetTrigger("SHOOT");
                power = false;
                charges -= chargesRequired;
                action = false;
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
