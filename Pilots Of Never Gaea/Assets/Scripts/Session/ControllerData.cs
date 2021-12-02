using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerData
{
    public InputDevice inputDevice;
    public int inputDeviceID;
    public string controlScheme;

    public ControllerData(InputDevice newDevice, string newControlScheme)
    {
        PairControllerData(newDevice, newControlScheme);
    }

    public void PairControllerData(InputDevice newDevice, string newControlScheme)
    {
        inputDevice = newDevice;
        controlScheme = newControlScheme;
        inputDeviceID = inputDevice.deviceId;
    }
}
