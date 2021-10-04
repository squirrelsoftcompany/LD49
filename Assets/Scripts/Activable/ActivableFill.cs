using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableFill : ActivableBehaviour
{
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
        mParentModule.GetComponent<ModuleBehavior>().activeFill(true);
    }

    public override void Stop()
    {
        mParentModule.GetComponent<ModuleBehavior>().activeFill(true);
    }
}
