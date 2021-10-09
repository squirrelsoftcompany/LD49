using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivablePressureGeneral : ActivablePressure
{
    public override void Active()
    {
        System.Array.ForEach(FindObjectsOfType<ModuleBehavior>(), x => x.activePressureEvacuation(true));
        GetComponentInChildren<Animator>()?.SetBool("On", true);
    }

    public override void Stop()
    {
        System.Array.ForEach(FindObjectsOfType<ModuleBehavior>(), x => x.activePressureEvacuation(false));
        GetComponentInChildren<Animator>()?.SetBool("On", false);
    }

    public override PovManager.RocketPOV GetRocketPOV()
    {
        return PovManager.RocketPOV.eBooster;
    }
}
