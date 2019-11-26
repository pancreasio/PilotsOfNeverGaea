using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raildrive : Palette
{
    private Animator animator;
    public GameObject pointer;
    public Transform laserOrigin;
    public Laser laser;
    public float shotDelay, deploymentTime;
    private float shotClock;

    protected override void Start()
    {
        base.Start();
        shotClock = 0;
        animator = transform.GetComponent<Animator>();
        pointer.SetActive(false);
    }

    public override void ResetPalette()
    {
        base.ResetPalette();
        animator.SetBool("CHARGED", false);
    }

    protected override void Update()
    {
        base.Update();

        if (power)
        {
            animator.SetBool("CHARGED", true);
            if (action)
            {
                animator.SetBool("SHOT", true);
                shotClock = 0;
                charges -= chargesRequired;
                if (charges < chargesRequired)
                    power = false;
                action = false;
                animator.SetBool("CHARGED", false);
            }
        }
        if (animator.GetBool("SHOT"))
        {
            shotClock += Time.deltaTime;
            if (shotClock >= deploymentTime)
            {
                pointer.SetActive(true);
                if (shotClock >= shotDelay)
                {
                    Fire();
                }            
            }
        }
    }



    private void Fire()
    {
        if (left)
        {
            Laser laserInstance = Instantiate(laser, laserOrigin.position, transform.rotation);
        }
        else
        {
            Instantiate(laser, laserOrigin.position, transform.rotation);
        }
        animator.SetBool("SHOT", false);
        pointer.SetActive(false);
        AkSoundEngine.PostEvent("sfx_raildrive_power", gameObject);
        if (UpdateCharges != null)
            UpdateCharges(charges);
    }
}
