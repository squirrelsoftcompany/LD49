using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableConnector : ActivableBehaviour
{
    public GameObject mPieMenu;

    public bool mPipeConnected = false;
    private Fuel mTypeFuelConnected;

    public override void clickDownBehavior()
    {
        if( !mPipeConnected )
        {
            //Open the pie menus
            mPieMenu.GetComponent<RadialMenu>().setConnector(this);
            mPieMenu.GetComponent<RadialMenu>().show();
        }
        else
        {
            mPieMenu.GetComponent<RadialMenu>().setConnector(null);
            disconnectFuel(mTypeFuelConnected);
        }
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

    public void connectFuel(Fuel pFuel)
    {
        mParentModule.GetComponent<ModuleBehavior>().connectPipe(pFuel);
        mTypeFuelConnected = pFuel;
        mPipeConnected = true;

    }

    public void disconnectFuel(Fuel pFuel)
    {
        mParentModule.GetComponent<ModuleBehavior>().disconnectPipe(pFuel);
        mTypeFuelConnected = Fuel.eNull;
        mPipeConnected = false;
    }

    
}
