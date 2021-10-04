using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableFreezer : ActivableBehaviour
{

    //TODO : We need a timer to set active at false when module stop the freeze action
    /*
    private float mFreezeDuration = 5.0f;
    private float mFreezeDurationTimeRemaining = 0.0f;*/

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
        mParentModule.GetComponent<ModuleBehavior>().activeFreeze(true);
    }

    public override void Stop()
    {
        mParentModule.GetComponent<ModuleBehavior>().activeFreeze(true);
    }
}
