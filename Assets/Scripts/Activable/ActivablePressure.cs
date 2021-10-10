using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivablePressure : ActivableBehaviour
{
    public override void clickDownBehavior()
    {
        Active();
        isActive = true;
    }

    public override void clickUpBehavior()
    {
        Stop();
        isActive = false;
    }

    public override void mouseExit()
    {
        Stop();
        isActive = false;
    }


    public override void Active()
    {
        mParentModule.GetComponent<ModuleBehavior>().activePressureEvacuation(true);
        GetComponentInChildren<Animator>()?.SetBool("On", true);
    }

    public override void Stop()
    {
        mParentModule.GetComponent<ModuleBehavior>().activePressureEvacuation(false);
        GetComponentInChildren<Animator>()?.SetBool("On", false);
    }
}
