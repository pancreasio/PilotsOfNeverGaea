using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSign : MonoBehaviour
{
    public Animator spriteAnimator;

    public void SetIdle()
    {
        spriteAnimator.SetBool("IDLE", true);
        spriteAnimator.SetBool("HIGHLIGHTED", false);
    }

    public void SetHighlighted()
    {
        spriteAnimator.SetBool("IDLE", false);
        spriteAnimator.SetBool("HIGHLIGHTED", true);
    }

    public void SetSelected()
    {

    }
}
