using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        transform.Rotate(0f,0f,rotationSpeed* Time.deltaTime);
    }
}
