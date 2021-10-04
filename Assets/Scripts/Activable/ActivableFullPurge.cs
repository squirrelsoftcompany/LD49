using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableFullPurge : ActivableBehaviour
{
    // public GameObject mRocket; // Use the rocket to get every modules 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        // TODO : We should call thoses functions on every module of the rocket not only on on module
        mParentModule.GetComponent<ModuleBehavior>().activePressureEvacuation(true);
        mParentModule.GetComponent<ModuleBehavior>().activePurge(true);
    }

    public override void Stop()
    {
        // TODO : We should call thoses functions on every module of the rocket not only on on module
        mParentModule.GetComponent<ModuleBehavior>().activePressureEvacuation(false);
        mParentModule.GetComponent<ModuleBehavior>().activePurge(false);
    }
}
