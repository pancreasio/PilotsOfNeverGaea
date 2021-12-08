using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nagate : Palette
{
    public Animator animator;
    public Transform projectileOrigin;
    public GameObject smokeBombPrefab;
    public GameObject smokeScreenPrefab;

    protected override void Start()
    {
        base.Start();
        animator = transform.GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if(action && power)
        {
            GenericProjectile smokeBombInstance = GameObject.Instantiate(smokeBombPrefab, projectileOrigin.position, projectileOrigin.rotation).GetComponent<GenericProjectile>();

            smokeBombInstance.InitialImpulse(this.transform.right);
            smokeBombInstance.PlatformAction += SpawnSmokeScreen;
            smokeBombInstance.PaletteAction += SpawnSmokeScreenPlayer;

            animator.SetTrigger("SHOT");
            charges -= chargesRequired;
                power = false;
                if (UpdateCharges != null)
                    UpdateCharges(charges);
        }
    }

    private void SpawnSmokeScreen(GameObject smokeBomb)
    {
        SmokeBomb smokeScreenInstance = GameObject.Instantiate(smokeScreenPrefab, smokeBomb.transform.position, smokeBomb.transform.rotation).GetComponent<SmokeBomb>();
    }

    private void SpawnSmokeScreenPlayer(GameObject smokeBomb)
    {
        SmokeBomb smokeScreenInstance = GameObject.Instantiate(smokeScreenPrefab, smokeBomb.transform.position, smokeBomb.transform.rotation).GetComponent<SmokeBomb>();
        smokeScreenInstance.OnPlayerCollision();
    }
}
