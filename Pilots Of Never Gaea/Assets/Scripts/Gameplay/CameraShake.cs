using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float intensity)
    {
        float shakeClock = 0f;
        Vector3 originalPosition = transform.position;
        while (shakeClock<duration)
        {
            shakeClock += Time.deltaTime;
            transform.position = new Vector3(originalPosition.x + intensity * Random.Range(-1f,1f), originalPosition.y + intensity * Random.Range(-1f, 1f), originalPosition.z);
            yield return null;
        }
        transform.position = originalPosition;
    }
}
