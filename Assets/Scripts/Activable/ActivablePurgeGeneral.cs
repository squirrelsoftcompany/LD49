using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivablePurgeGeneral : ActivableBehaviour
{
    public bool managePressure;

    public override void Active()
    {
        System.Array.ForEach(FindObjectsOfType<ModuleBehavior>(), x => { x.activePurge(true); if (managePressure) x.activePressureEvacuation(true); });
    }

    public override void Stop()
    {
        System.Array.ForEach(FindObjectsOfType<ModuleBehavior>(), x => { x.activePurge(false); if (managePressure) x.activePressureEvacuation(false); });
    }

    public override bool IsActivable()
    {
        return PovManager.Inst.CurrentRocketPOV == PovManager.RocketPOV.eBooster;
    }
}
