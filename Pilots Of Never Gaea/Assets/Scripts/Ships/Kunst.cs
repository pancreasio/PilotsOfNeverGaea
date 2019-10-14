using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunst : Palette
{
    private Animator animator;
    bool active;

    public override void Start()
    {
        base.Start();
        active = false;
        animator = transform.GetComponent<Animator>();
    }

    public override void Update()
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
                action = Input.GetKey(KeyCode.RightArrow);

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
