﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Djinn : Palette
{
    private Animator animator;
    public Illusion illusionPrefab;
    public Transform origin;
    public GameObject wishPrefab;

    protected override void Start()
    {
        base.Start();
        animator = transform.GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        animator.SetBool("POWER", power);
        if (action)
        {
            if (power)
            {
                Illusion illusionInstance = Instantiate(illusionPrefab.gameObject, origin.position, Quaternion.identity).GetComponent<Illusion>();
                if (moveSpeed == 0)
                {
                    illusionInstance.InitialKick(transform.right);
                }
                else
                {
                    illusionInstance.InitialKick(transform.up * moveSpeed+ transform.right);
                }
                AkSoundEngine.PostEvent("sfx_djinn_power", gameObject);
                Instantiate(wishPrefab, origin.position, transform.rotation);
                charges -= chargesRequired;
                power = false;
                action = false;
                if (UpdateCharges != null)
                    UpdateCharges(charges);
            }
        }

    }
}
