using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reply : MonoBehaviour
{
    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
