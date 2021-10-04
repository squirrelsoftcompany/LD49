using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableConnector : ActivableBehaviour
{
    public GameObject mPieMenu;
    private RadialMenu mEffectivePieMenu = null;

    public bool mPipeConnected = false;
    private Fuel mTypeFuelConnected;

    public Transform m_hotspotConnector;

    protected override void Start()
    {
        base.Start();
        mEffectivePieMenu = FindObjectOfType<RadialMenu>();
        if (mEffectivePieMenu == null)
        {
            mPieMenu = Instantiate(mPieMenu, transform.root);
            mEffectivePieMenu = mPieMenu.GetComponentInChildren<RadialMenu>();
        }
    }

    public override void clickDownBehavior()
    {
        if( !mPipeConnected )
        {
            //Open the pie menus
            mEffectivePieMenu.setConnector(this);
            mEffectivePieMenu.show();
        }
        else
        {
            mEffectivePieMenu.setConnector(null);
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

    public void connectFuel(Fuel pFuel)
    {
        mParentModule.connectPipe(pFuel);
        mTypeFuelConnected = pFuel;
        mPipeConnected = true;
        if (! GeneralConnectorManagement.Inst.Connect(m_hotspotConnector))
        {
            Debug.Log("Can't found a free connector.");
        }
    }

    public void disconnectFuel(Fuel pFuel)
    {
        mParentModule.disconnectPipe(pFuel);
        mTypeFuelConnected = Fuel.eNull;
        mPipeConnected = false;
        if (!GeneralConnectorManagement.Inst.Disconnect(m_hotspotConnector))
        {
            Debug.Log("Can't find the correct connector to disconnect.");
        }
    }
}
