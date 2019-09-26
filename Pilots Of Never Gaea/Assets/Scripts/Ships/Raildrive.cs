using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raildrive : Palette
{
    private Animator animator;
    public GameObject pointer;
    public Laser laser;
    public float shotDelay;
    private float shotClock;

    public override void Start()
    {
        base.Start();
        shotClock = 0;
        animator = transform.GetComponent<Animator>();
        pointer.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (power)
        {
            animator.SetBool("CHARGED", true);
            if (action)
            {
                animator.SetBool("SHOT", true);
                shotClock = 0;
                power = false;
                charges -= chargesRequired;
                action = false;
                animator.SetBool("CHARGED", false);
            }
        }
        if (animator.GetBool("SHOT"))
        {
            shotClock += Time.deltaTime;
            if (shotClock >= 0.217f)
            {
                pointer.SetActive(true);
            }
            if (shotClock >= shotDelay)
            {
                Fire();
            }
        }
    }

    

    private void Fire()
    {
        if (left)
        {
            Laser laserInstance = Instantiate(laser,
                new Vector3(0.0f + 0.63f, transform.position.y),
                new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + 180.0f, transform.rotation.w));
        }
        else
        {
            Instantiate(laser, new Vector3(0.0f - 0.63f, transform.position.y), transform.rotation);
        }
        animator.SetBool("SHOT", false);
        pointer.SetActive(false);
    }
}
