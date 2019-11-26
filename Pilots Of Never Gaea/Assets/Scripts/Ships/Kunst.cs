using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunst : Palette
{
    private Animator animator;
    public GameObject reply;
    public Transform replyParent;
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
                action = Input.GetKey(KeyCode.Space);
            else
                action = Input.GetKey(KeyCode.RightControl);

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

    private void Reply()
    {
        if (power)
        {
            AkSoundEngine.PostEvent("sfx_kunst_power", gameObject);
            Instantiate(reply, replyParent);
            reply.transform.position.Set(0f, 0f, 0f);
            charges -= chargesRequired;
            power = false;
            action = false;
            if (UpdateCharges != null)
                UpdateCharges(charges);
        }
    }
}
