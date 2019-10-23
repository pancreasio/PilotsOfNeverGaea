using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunst : Palette
{
    private Animator animator;
    bool active;

    protected override void Start()
    {
        base.Start();
        active = false;
        animator = transform.GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (!active)
        {
            base.Update();
            if (action)
                active = true;
        }
        else
        {
            if (left)
                action = Input.GetKey(KeyCode.D);
            else
                action = Input.GetKey(KeyCode.LeftArrow);

            if (action)
            {
                active = true;
                animator.SetBool("ACTIVE", true);
            }
            else
            {
                active = false;
                animator.SetBool("ACTIVE", false);
            }
        }
        
    }
}
