using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;
using GameEventSystem;


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

    static float getPressureFactorFuel(Fuel pFuel)
    {
        switch(pFuel)
        {
            case Fuel.eE1:
                return PRESSURE_FACTOR_E1;
            case Fuel.eE2:
                return PRESSURE_FACTOR_E2;
            case Fuel.eE3:
                return PRESSURE_FACTOR_E3;
            case (Fuel.eE1 | Fuel.eE2):
                return PRESSURE_FACTOR_E4;
            case (Fuel.eE1 | Fuel.eE3):
                return PRESSURE_FACTOR_E5;
            case (Fuel.eE2 | Fuel.eE3):
                return PRESSURE_FACTOR_E6;
            case (Fuel.eE1 | Fuel.eE2 | Fuel.eE3):
                return PRESSURE_FACTOR_E7;
            default:
                return 1.0f;
        }
    }

    static float getTempFactorFuel(Fuel pFuel)
    {
        switch (pFuel)
        {
            case Fuel.eE1:
                return TEMP_FACTOR_E1;
            case Fuel.eE2:
                return TEMP_FACTOR_E2;
            case Fuel.eE3:
                return TEMP_FACTOR_E3;
            case (Fuel.eE1 | Fuel.eE2):
                return TEMP_FACTOR_E4;
            case (Fuel.eE1 | Fuel.eE3):
                return TEMP_FACTOR_E5;
            case (Fuel.eE2 | Fuel.eE3):
                return TEMP_FACTOR_E6;
            case (Fuel.eE1 | Fuel.eE2 | Fuel.eE3):
                return TEMP_FACTOR_E7;
            default:
                return 1.0f;
        }
    }

    public const float MIN_TEMP = -40.0f;
    public const float MAX_TEMP = 200.0f;
    public const float TEMP_FACTOR = 0.5f;

    public const float PRESSURE_FACTOR_E1 = 0.7f;    
    public const float PRESSURE_FACTOR_E2 = 0.8f;
    public const float PRESSURE_FACTOR_E3 = 1.0f;
    public const float PRESSURE_FACTOR_E4 = 1.2f;
    public const float PRESSURE_FACTOR_E5 = 1.5f;
    public const float PRESSURE_FACTOR_E6 = 1.5f;
    public const float PRESSURE_FACTOR_E7 = 1.8f;

    public const float TEMP_FACTOR_E1 = 0.8f;
    public const float TEMP_FACTOR_E2 = 0.8f;
    public const float TEMP_FACTOR_E3 = 1.0f;
    public const float TEMP_FACTOR_E4 = -1.4f;
    public const float TEMP_FACTOR_E5 = 1.7f;
    public const float TEMP_FACTOR_E6 = 1.3f;
    public const float TEMP_FACTOR_E7 = 2.0f;

    public PovManager.RocketPOV RocketPOV;

    public GameObject mSoundManager;
    private SoundManager mSoundManagerScript;


    public ParticleSystem mParticuleSystem;


    //Goal to valid module
    public float mGoalPressure;
    public float mGoalTemp;

    public List<ergolInTank> mGoalErgolStack;
    public GameObject mMonitor;

    // Limit for the module
    public float mPressureLimit = 100;
    public float mTempLimit = MAX_TEMP; //Set to MAX_TEMP but can change if need it 

    //Runtime variables
    [Range(0.0f, 100.0f)]
    public float mPressure;
    [Range(MIN_TEMP, MAX_TEMP)]
    public float mTemp;
    private List<ergolInTank> mErgolStack = new List<ergolInTank>();
    public int mNbPipeConnected = 0;
    private float mLife = 100.0f;

    // 3 full slots max. eNull if not used
    public Fuel[] mFillSlots = { Fuel.eNull, Fuel.eNull, Fuel.eNull };

    public bool mActiveFill = false;  //If true, we call fill() at update
    public bool mActivePurge = false;  //If true, we call purge() at update
    public bool mActiveFreeze = false;  //If true, we call purge() at update
    public bool mActivePressureEvacuation = false;  //If true, we call purge() at update

    private float mFillSpeed = 5.0f; // Percent of the fulltank fill in 1 sec
    private float mFullTankPressureSpeed = 5.0f; 
    private float mPurgeSpeed = 20.0f; // Percent of the fulltank purge in 1 sec
    private float mPressureEvacuationSpeed = 10.0f; // MPa.s-1
    private float mTempEvacuationSpeed = 10.0f; // C.s-1

    //Dommage ratio
    private float mDommageOverPressure = 2.0f; // Dommage in 1 sec
    private float mDommageOverTemp = 2.0f; // Dommage in 1 sec

    //Duration temperature diminution
    private float mFreezeDuration = 3.0f;
    private float mFreezeDurationTimeRemaining = 0.0f;


    // Others variables
    private bool mOverPressure = false;
    private bool mOverTemp = false;
    private bool mFuelOverflow= false;


    // Start is called before the first frame update
    void Start()
    {
        mSoundManagerScript = mSoundManager.GetComponent<SoundManager>();

        ParticleSystem.EmissionModule emission = mParticuleSystem.emission;
        emission.enabled = false;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, 0)});

        mMonitor.GetComponent<Monitor>().setModuleGoalInformation(mGoalErgolStack);
    }

    public void setGoal(List<ergolInTank> pGoal)
    {
        mGoalErgolStack = pGoal;
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

        updateModuleState();

        //Send information to the monitor
        mMonitor.GetComponent<Monitor>().setModuleInformation(mErgolStack, mPressure, Mathf.InverseLerp(MIN_TEMP, MAX_TEMP, mTemp)*100.0f );
    }

    private void updateModuleState()
    {
        //Compute temperature progression 
        float ergolTempFactor = 1;
        float lastfuelQt = 0;
        for (int i = 0; i < mErgolStack.Count; i++)
        {
            // Warning : Remove last fuel quantity because quantity is the addion of the fuel quantity stack
            ergolTempFactor += getTempFactorFuel(mErgolStack[i].ergolType) * ((mErgolStack[i].quantity - lastfuelQt) / 100.0f);
            lastfuelQt = mErgolStack[i].quantity;
        }
        mTemp += (Time.deltaTime * ((lastfuelQt / 100.0f) * ergolTempFactor + (1.0f - (lastfuelQt / 100.0f)) * TEMP_FACTOR));  // The ergole part is lastfuelQt of the tank. The empty part is (100 - lastfuelQt)
        mTemp = Mathf.Min(MAX_TEMP, mTemp);


        // ----- Check anomalies

        //1. Check pressure
        bool lastPressureState = mOverPressure;
        if (mPressure >= mPressureLimit)
        {
            mOverPressure = true;
            reduceLife(Time.deltaTime * mDommageOverPressure);
        }
        else
        {
            mOverPressure = false;
        }
        //Send event if state changed
        if(lastPressureState != mOverPressure)
        {
            mSoundManagerScript.playOverPressure(mOverPressure);
        }
        //2. Check temp
        bool lastTempState = mOverTemp;
        // TODO : compute mTempLimit with full type in tank
        if (mTemp >= mTempLimit)
        {
            mOverTemp = true;
            reduceLife(Time.deltaTime * mDommageOverTemp);
        }
        else
        {
            mOverTemp = false;
        }
        //Send event if state changed
        if (lastTempState != mOverTemp)
        {
            mSoundManagerScript.playOverTemp(mOverTemp);
        }

        // ----- End of anomalis check

    }

    public void reduceLife(float pNewLife)
    {
        if(mLife > 0)
        {
            mLife -= pNewLife;
            int burstCount = 5 * (10 - (Mathf.FloorToInt(mLife) / 10));
            ParticleSystem.EmissionModule emission = mParticuleSystem.emission;
            emission.enabled = true;
            emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, burstCount) });
        }
        else
        {
            // Explosion I guess..
        }
    }

    void fill()
    {

        Fuel currentFull = (Fuel)(mFillSlots[0] | mFillSlots[1] | mFillSlots[2]);

        if (currentFull != Fuel.eNull )
        {
            bool lastOverflowState = mFuelOverflow;
            float totalFuel = 0;
            for(int i = 0; i < mErgolStack.Count; i++ )
            {
                totalFuel += mErgolStack[i].quantity;
            }
            if (mErgolStack.Count < 1 || totalFuel < 100.0f)
            {
                mFuelOverflow = false;
                if (mErgolStack.Count > 0 && mErgolStack[mErgolStack.Count - 1].ergolType == currentFull)
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
                    mErgolStack.Add(new ergolInTank(currentFull, Time.deltaTime * mFillSpeed));
                }
                // TODO : Add here effects on temp and pressure when adding some fuel
                //Add pressure
                mPressure += (Time.deltaTime * mFillSpeed) * getPressureFactorFuel(currentFull);
                mPressure = Mathf.Min(mPressureLimit, mPressure);
            }
            else if(mErgolStack.Count > 0 && mErgolStack[mErgolStack.Count - 1].quantity >= 100.0f)
            {
                mFuelOverflow = true;
                mPressure += (Time.deltaTime * mFillSpeed) * mFullTankPressureSpeed;
                mPressure = Mathf.Min(mPressureLimit, mPressure);
            }
            //Send overflow event if state changed
            if (lastOverflowState != mFuelOverflow)
            {
                mSoundManagerScript.playFuelOverflow(mFuelOverflow);
            }
        }
    }

    void purge()
    {
        if( mErgolStack.Count > 0 )
        {
            mErgolStack[0].quantity -= Time.deltaTime * mPurgeSpeed;
            if (mErgolStack[0].quantity <= 0)
            {
                mErgolStack.RemoveAt(0);
            }
        }
        else
        {
            //Stop purge
            activePurge(false);
        }
    }

    public bool moduleValidity()
    {
        bool moduleValid = true;

        //Check temp
        if ( mTemp < mGoalTemp +1 )
        {
            moduleValid &= true;
        }

        //Check pressure
        if ( mPressure < mGoalPressure+ 1 )
        {
            moduleValid &= true;
        }

        //Check full
        if(mErgolStack.Count == mGoalErgolStack.Count)
        {
            float currentFuelGoal = 0;
            for ( int i=0; i < mErgolStack.Count; i++)
            {
                if (mErgolStack[i].ergolType == mGoalErgolStack[i].ergolType)
                {
                    currentFuelGoal = (i>0) ? mGoalErgolStack[i].quantity - mGoalErgolStack[i-1].quantity : mGoalErgolStack[i].quantity;
                    if (mErgolStack[i].quantity < currentFuelGoal + 1 && mErgolStack[i].quantity > currentFuelGoal - 1)
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
        if (mPressure > 0)
        {
            mPressure -= Time.deltaTime * mPressureEvacuationSpeed;
        }
        else
        {
            mPressure = 0;
            activePressureEvacuation(false);
        }
    }

    //Callable methode by event system

    public void activePurge(bool active)
    {
        mActivePurge = active;
        mSoundManagerScript.playFuelPurge(active);
    }

    public void activeFreeze(bool active)
    {
        mActiveFreeze = active;
        mSoundManagerScript.playFreeze();
    }

    public void activeFill(bool active)
    {
        mActiveFill = active;
        if(!active)
        {
            mFuelOverflow = false; // Stop overflow state
        }
        mSoundManagerScript.playFuelFill(active);
    }
    public void activePressureEvacuation(bool active)
    {
        mActivePressureEvacuation = active;
        mSoundManagerScript.playPressurePurge(active);
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
        mSoundManagerScript.playPipeClip();
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
        mSoundManagerScript.playPipeClip();
    }
}
