using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;



// Fuel use binary flag system to be identified - Ex: E1 = 001 E3 = 100 -> E5 = 101 
public enum Fuel
{ 
    eNull = 0,
    eE1 = 1, 
    eE2 = 2,
    eE3 = 4,
    // Forget the rest (only for enum structure)
    eE4 = 3,
    eE5 = 5,
    eE6 = 6,
    eE7 = 7,
    eNum = 8
};

public class ModuleBehavior : MonoBehaviour
{

    public  const float MIN_TEMP = -40.0f;
    public  const float MAX_TEMP = 80.0f;

    //Goal to valid module
    public float mGoalPressure; // +/- 1
    public float mGoalTemp; // +/- 1

    public List<ergolInTank> mGoalErgolStack;
    public GameObject mMonitor;

    //Runtime variables
    [Range(0.0f, 100.0f)]
    public float mPressure;
    [Range(MIN_TEMP, MAX_TEMP)]
    public float mTemp;
    private List<ergolInTank> mErgolStack = new List<ergolInTank>();
    public int mNbPipeConnected = 0;
    private int mLife = 100;

    // 3 full slots max. eNull if not used
    public Fuel[] mFillSlots = { Fuel.eNull, Fuel.eNull, Fuel.eNull };

    public bool mActiveFill = false;  //If true, we call fill() at update
    public bool mActivePurge = false;  //If true, we call purge() at update
    public bool mActiveFreeze = false;  //If true, we call purge() at update
    public bool mActivePressureEvacuation = false;  //If true, we call purge() at update

    private float mFillSpeed = 5.0f; // Percent of the fulltank fill in 1 sec
    private float mPurgeSpeed = 20.0f; // Percent of the fulltank purge in 1 sec
    private float mPressureEvacuationSpeed = 1.0f; // MPa.s-1
    private float mTempEvacuationSpeed = 5.0f; // MPa.s-1

    //Duration temperature diminution
    private float mFreezeDuration = 5.0f;
    private float mFreezeDurationTimeRemaining = 0.0f;


    // Others variables


    // Start is called before the first frame update
    void Start()
    {
        mMonitor.GetComponent<Monitor>().setModuleGoalInformation(mGoalErgolStack);
    }

    // Update is called once per frame
    void Update()
    {
        if(mActiveFill)
        {
            fill();
        }
        if (mActivePurge)
        {
            purge();
        }
        if (mActivePressureEvacuation)
        {
            pressureEvacuation();
        }
        if (mActiveFreeze)
        {
            freezing();
        }

        //Send information to the monitor
        mMonitor.GetComponent<Monitor>().setModuleInformation(mErgolStack, mPressure, Mathf.InverseLerp(MIN_TEMP, MAX_TEMP, mTemp)*100.0f );
    }

    void fill()
    {

        int currentFull = Convert.ToInt32(mFillSlots[0] | mFillSlots[1] | mFillSlots[2]);

        if (currentFull != Convert.ToInt32(Fuel.eNull))
        {
            if (mErgolStack.Count > 0 && mErgolStack[mErgolStack.Count - 1].ergolType == (Fuel)currentFull)
            {
                mErgolStack[mErgolStack.Count - 1].quantity += Time.deltaTime * mFillSpeed;
            }
            else
            {
                //Remove last full if quantity is very low to help player
                if (mErgolStack.Count > 0 && mErgolStack[mErgolStack.Count - 1].quantity < 1.0f)
                {
                    mErgolStack.RemoveAt(mErgolStack.Count - 1);
                }
                mErgolStack.Add(new ergolInTank((Fuel)currentFull, Time.deltaTime * mFillSpeed));
            }
            // TODO : Add here effects on temp and pressure when adding some fuel
        }

    }

    void purge()
    {
        mErgolStack[0].quantity -= Time.deltaTime * mPurgeSpeed;
        if(mErgolStack[0].quantity <= 0)
        {
            mErgolStack.RemoveAt(0);
        }
    }

    public bool moduleValidity()
    {
        bool moduleValid = true;

        //Check temp
        if(mTemp < mGoalTemp +1 && mTemp > mGoalTemp-1)
        {
            moduleValid &= true;
        }

        //Check pressure
        if (mPressure < mGoalPressure+ 1 && mPressure > mGoalPressure - 1)
        {
            moduleValid &= true;
        }

        //Check full
        if(mErgolStack.Count == mGoalErgolStack.Count)
        {
            for( int i =0; i < mErgolStack.Count; i++)
            {
                if (mErgolStack[i].ergolType == mGoalErgolStack[i].ergolType)
                {
                    if(mErgolStack[i].quantity < mGoalErgolStack[i].quantity + 1 && mErgolStack[i].quantity > mGoalErgolStack[i].quantity - 1)
                    {
                        moduleValid &= true;
                    }
                }
            }
        }

        return moduleValid;
    }

    void freezing()
    {

        if (mFreezeDurationTimeRemaining > 0)
        {
            mTemp -= Time.deltaTime * mTempEvacuationSpeed;
            mFreezeDurationTimeRemaining -= Time.deltaTime;
        }
        else
        {
            mActiveFreeze = false;
            mFreezeDurationTimeRemaining = mFreezeDuration;
        }
    }

    void pressureEvacuation()
    {
        mPressure -= Time.deltaTime * mPressureEvacuationSpeed;
    }

    //Callable methode by event system

    public void activePurge(bool active)
    {
        mActivePurge = active;
    }

    public void activeFreeze(bool active)
    {
        mActiveFreeze = active;
    }

    public void activeFill(bool active)
    {
        mActiveFill = active;
    }
    public void activePressureEvacuation(bool active)
    {
        mActivePressureEvacuation = active;
    }

    public void connectPipe(Fuel pFuel)
    {

        if (mNbPipeConnected < 3) //TODO : Change max connector number 
        {
            for(int i = 0; i < mFillSlots.Length; i++)
            {
                if (mFillSlots[i] == Fuel.eNull)
                {
                    mFillSlots[i] = pFuel;
                    break;
                }
            }
            mNbPipeConnected++;
        }
    }

    public void disconnectPipe(Fuel pFuel) /* pFuel = Previous full*/
    {
        /* Disconnect first slot with corresponding fuel */
        for (int i = 0; i < mFillSlots.Length; i++)
        {
            if (mFillSlots[i] == pFuel)
            {
                mFillSlots[i] = Fuel.eNull;
                break;
            }
        }
        mNbPipeConnected--;
    }
}
