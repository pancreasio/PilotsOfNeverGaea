using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hawking : Palette
{
    public delegate void WellFunction(float wellSpeed);
    public delegate void WellActiveFunction(bool isActive);
    public static WellFunction WellAction;
    public static WellActiveFunction WellActiveAction;

    public float wellTime;
    private float wellClock;
    public float wellSpeed;

    public int chargesUsed;

    private bool isActive;
    protected override void Start()
    {
        base.Start();
        isActive = false;
    }

    protected override void Update()
    {
        base.Update();
        if (action && power && !isActive)
        {
            isActive = true;
            wellClock = 0f;
            charges -= chargesUsed;
            
            if(WellActiveAction!=null)
                WellActiveAction.Invoke(true);

            if (charges <= 0)
                power = false;

            if (UpdateCharges != null)
                UpdateCharges(charges);
        }

        if (isActive)
        {
            wellClock += Time.deltaTime;
            if (wellClock >= wellTime)
            {
                power = (charges>=chargesUsed && action);
                isActive = false;
                if(WellActiveAction!=null)
                WellActiveAction.Invoke(false);
            }
            if (WellAction != null)
                WellAction.Invoke(wellSpeed * Time.deltaTime);
        }
    }
}
