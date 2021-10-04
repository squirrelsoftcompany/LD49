using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivablePressure : ActivableBehaviour
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
    }

    public override void Stop()
    {
        mParentModule.GetComponent<ModuleBehavior>().activePressureEvacuation(false);
    }
}
