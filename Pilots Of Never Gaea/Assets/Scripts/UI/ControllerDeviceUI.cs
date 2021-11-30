using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerDeviceUI : MonoBehaviour
{
    public InputDevice assignedDevice = null;
    public string assignedScheme;
    private bool canMove = true;
    public void MoveToPlayer(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        if(inputValue.x>0.5f && canMove)
            TryMoving(false);
            else
            if(inputValue.x<-0.5f && canMove)
            TryMoving(true);

        if(inputValue == Vector2.zero)
            canMove=true;
    }

    private void TryMoving(bool isMovingLeft)
    {
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().MoveUIController(this, isMovingLeft);
        canMove = false;
    }

    public void TryConfirming()
    {
        
    }

    public void TryCancelling()
    {

    }
}
