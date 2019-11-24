using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magstream : Palette
{
    private Animator animator;
    public IonWave ionWave;

    protected override void Start()
    {
        base.Start();
        animator = transform.GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        animator.SetInteger("CHARGES", charges);
        if (power)
        {
            if (action)
            {
                Instantiate(ionWave, new Vector2(0.0f, 0.0f), Quaternion.identity);
                power = false;
                charges -= chargesRequired;
                action = false;
                if (UpdateCharges != null)
                    UpdateCharges(charges);
            }
        }
    }
}
