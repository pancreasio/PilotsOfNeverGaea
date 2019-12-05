using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunst : Palette
{
    private Animator animator;
    public GameObject reply;
    public Transform replyParent;
    private bool active, canReply;

    protected override void Start()
    {
        base.Start();
        active = false;
        canReply = false;
        animator = transform.GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (!active)
        {
            base.Update();
            if (action)
                active = true;

            if (power)
                canReply = true;
        }
        else
        {
            if (isPlayer1)
                action = Input.GetKey(KeyCode.Space);
            else
                action = Input.GetKey(KeyCode.RightControl);

            if (action)
            {
                active = true;
            }
            else
            {
                active = false;
            }
            animator.SetBool("ACTIVE", active);
        }
    }

    private void Reply()
    {
        if (canReply)
        {
            AkSoundEngine.PostEvent("sfx_kunst_power", gameObject);
            Instantiate(reply, replyParent);
            reply.transform.position.Set(0f, 0f, 0f);
            charges -= chargesRequired;
            power = false;
            action = false;
            canReply = false;
            if (UpdateCharges != null)
                UpdateCharges(charges);
        }
    }
}
