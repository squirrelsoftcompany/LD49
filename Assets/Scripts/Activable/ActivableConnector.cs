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
    public Material MatConnectorE1;
    public Material MatConnectorE2;
    public Material MatConnectorE3;
    public Material MatConnectorOff;

    protected override void Start()
    {
        base.Start();
        mEffectivePieMenu = FindObjectOfType<RadialMenu>();
        if (mEffectivePieMenu == null)
        {
            mPieMenu = Instantiate(mPieMenu, transform.root);
            mEffectivePieMenu = mPieMenu.GetComponentInChildren<RadialMenu>();
        }

        GetComponent<MeshRenderer>().material = MatConnectorOff;
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
        if (! GeneralConnectorManagement.Inst.Connect(m_hotspotConnector))
        {
            Debug.Log("Can't found a free connector.");
        }

        GetComponent<MeshRenderer>().material = MatConnectorE1;
        if (pFuel != Fuel.eE1) GetComponent<MeshRenderer>().material = pFuel != Fuel.eE2 ? MatConnectorE3 : MatConnectorE2;

        mPipeConnected = true;
    }

    public void disconnectFuel(Fuel pFuel)
    {
        mParentModule.disconnectPipe(pFuel);
        mTypeFuelConnected = Fuel.eNull;
        if (!GeneralConnectorManagement.Inst.Disconnect(m_hotspotConnector))
        {
            Debug.Log("Can't find the correct connector to disconnect.");
        }
        GetComponent<MeshRenderer>().material = MatConnectorOff;
        mPipeConnected = false;
    }
}
