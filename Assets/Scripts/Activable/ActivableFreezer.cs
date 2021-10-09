using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableFreezer : ActivableBehaviour
{
    enum FreezerStatus
    {
        eReady = 0,
        eOn,
        eOff,
        eStatusNb
    }

    public float actionTime = 0.0f;
    public float cooldownTime = 0.0f;

    private float timeRemaining = 0.0f;
    private FreezerStatus freezerStatus = FreezerStatus.eReady;

    protected override void Update()
    {
        base.Update();

        switch (freezerStatus)
        {
            case FreezerStatus.eReady:
                // Do nothing
                break;
            case FreezerStatus.eOn:
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    freezerStatus = FreezerStatus.eOff;
                    Stop();
                    timeRemaining = cooldownTime;
                }
                break;
            case FreezerStatus.eOff:
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    freezerStatus = FreezerStatus.eReady;
                    Ready();
                }
                break;
        }
    }

    public override void clickDownBehavior()
    {
        switch (freezerStatus)
        {
            case FreezerStatus.eReady:
                freezerStatus = FreezerStatus.eOn;
                Active();
                timeRemaining = actionTime;
                isActive = true;
                break;
            case FreezerStatus.eOn:
                freezerStatus = FreezerStatus.eOff;
                Stop();
                isActive = false;
                break;
            case FreezerStatus.eOff:
                // Do nothing
                break;
        }
    }

    public override void Active()
    {
        mParentModule.GetComponent<ModuleBehavior>().activeFreeze(true);
        GetComponentInChildren<Animator>()?.SetTrigger("On");
    }

    public override void Stop()
    {
        mParentModule.GetComponent<ModuleBehavior>().activeFreeze(false);
        GetComponentInChildren<Animator>()?.SetTrigger("Off");
    }

    public void Ready()
    {
        GetComponentInChildren<Animator>()?.SetTrigger("Ready");
    }
}
