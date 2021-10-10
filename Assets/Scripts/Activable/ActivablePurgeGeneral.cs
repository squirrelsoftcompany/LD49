using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivablePurgeGeneral : ActivablePurge
{
    public bool managePressure;

    public override void Active()
    {
        System.Array.ForEach(FindObjectsOfType<ModuleBehavior>(), x => { x.activePurge(true); if (managePressure) x.activePressureEvacuation(true); });
        GetComponentInChildren<Animator>()?.SetBool("On", true);
    }

    public override void Stop()
    {
        System.Array.ForEach(FindObjectsOfType<ModuleBehavior>(), x => { x.activePurge(false); if (managePressure) x.activePressureEvacuation(false); });
        GetComponentInChildren<Animator>()?.SetBool("On", false);
    }

    public override PovManager.RocketPOV GetRocketPOV()
    {
        return PovManager.RocketPOV.eBooster;
    }
}
