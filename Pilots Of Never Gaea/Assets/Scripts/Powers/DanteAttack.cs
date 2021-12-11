using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanteAttack : MonoBehaviour
{
    public GameManager.GameObjectFunction OnBallEnterTrigger;
    public GameManager.GameObjectFunction OnBallExitTrigger;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball" && OnBallEnterTrigger != null)
        {
            OnBallEnterTrigger.Invoke(collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball" && OnBallExitTrigger != null)
            OnBallExitTrigger.Invoke(collider.gameObject);
    }
}
