using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoto : Palette
{
    public GameObject hadoPrefab;
    public Transform projectileOrigin;
    public float hadoSpeedIncrease;
    protected override void Start()
    {
        base.Start();
        GetComponent<Animator>().SetBool("IDLE",false);
        GetComponent<Animator>().SetBool("HIGHLIGHTED",true);
    }

    protected override void Update()
    {
        base.Update();
        if (action && power)
        {
            GenericProjectile hadoInstance = GameObject.Instantiate(hadoPrefab, projectileOrigin.position, Quaternion.identity).GetComponent<GenericProjectile>();
            if (!isPlayer1)
                hadoInstance.transform.localScale = new Vector3(-hadoInstance.transform.localScale.x, hadoInstance.transform.localScale.y, hadoInstance.transform.localScale.z);

            hadoInstance.SetImpulse(this.transform.right, hadoSpeedIncrease * charges);
            charges = 0;
            power = false;
            if (UpdateCharges != null)
                UpdateCharges.Invoke(charges);
        }
    }
}
