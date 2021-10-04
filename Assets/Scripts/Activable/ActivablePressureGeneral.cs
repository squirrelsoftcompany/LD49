using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivablePressureGeneral : ActivablePressure
{
    public override void Active()
    {
        System.Array.ForEach(FindObjectsOfType<ModuleBehavior>(), x => x.activePressureEvacuation(true));
    }

    public override void Stop()
    {
        System.Array.ForEach(FindObjectsOfType<ModuleBehavior>(), x => x.activePressureEvacuation(false));
    }

    public override bool IsActivable()
    {
        return PovManager.Inst.CurrentRocketPOV == PovManager.RocketPOV.eBooster;
    }
}
