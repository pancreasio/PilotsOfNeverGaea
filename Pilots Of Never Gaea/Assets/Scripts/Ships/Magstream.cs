using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magstream : Palette
{
    private Animator animator;

    public override void Start()
    {
        base.Start();
        animator = transform.GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        animator.SetInteger("CHARGES", charges);
        if (power)
        {
            if (action)
            {
                power = false;
                charges -= chargesRequired;
                action = false;
            }
        }
    }
}
