using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableFill : ActivableBehaviour
{
    public override void clickDownBehavior()
    {
        if (!isActive)
        {
            Active();
        }
        else
        {
            Stop();
        }
        isActive = !isActive;
    }

    public override void clickUpBehavior()
    {
        //Do nothing.
    }

    public override void mouseExit()
    {
        //Do nothing.
    }


    public override void Active()
    {
        mParentModule.GetComponent<ModuleBehavior>().activeFill(true);
        GetComponentInChildren<Animator>()?.SetBool("On", true);
    }

    public override void Stop()
    {
        mParentModule.GetComponent<ModuleBehavior>().activeFill(false);
        GetComponentInChildren<Animator>()?.SetBool("On", false);
    }
}
