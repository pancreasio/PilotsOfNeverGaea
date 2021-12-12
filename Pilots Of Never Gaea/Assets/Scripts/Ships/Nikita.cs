using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nikita : Palette
{
    public GameObject missilePrefab;
    public GameObject explosionPrefab;
    public Transform projectileOrigin;

    private GameObject missileInstance;
    private bool canDetonateMissile;
    protected override void Start()
    {
        base.Start();
        GetComponent<Animator>().SetBool("HIGHLIGHTED", true);
        missileInstance = null;
        canDetonateMissile = false;
    }

    protected override void Update()
    {
        base.Update();
        if (action && power)
        {
            GenericProjectile missileScriptInstance = GameObject.Instantiate(missilePrefab, projectileOrigin.position, projectileOrigin.rotation).GetComponent<GenericProjectile>();
            missileInstance = missileScriptInstance.gameObject;

            if (!isPlayer1)
                missileInstance.transform.localScale = new Vector3(-missileInstance.transform.localScale.x, missileInstance.transform.localScale.y, missileInstance.transform.localScale.z);

            missileScriptInstance.InitialImpulse(this.transform.right);
            missileScriptInstance.PlatformAction += SpawnExplosion;
            missileScriptInstance.PaletteAction += SpawnExplosion;
            missileScriptInstance.BallAction += SpawnExplosion;
            missileScriptInstance.WallAction += SpawnExplosion;

            canDetonateMissile = false;

            charges -= chargesRequired;
            power = false;
            if (UpdateCharges != null)
                UpdateCharges(charges);
        }
        else
        {
            if (!action && missileInstance != null)
                canDetonateMissile = true;
        }

        if (action && !power && canDetonateMissile && missileInstance != null)
        {
            SpawnExplosion(missileInstance);
            Destroy(missileInstance);
            canDetonateMissile = false;
        }
    }

    private void SpawnExplosion(GameObject originObject)
    {
        GameObject.Instantiate(explosionPrefab, originObject.transform.position, Quaternion.identity);
    }
}