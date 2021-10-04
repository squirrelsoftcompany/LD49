using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableSwitchView : ActivableBehaviour
{
    protected override void Start()
    {
        base.Start();
        mParentModule = GetComponentInParent<ModuleBehavior>();
    }

    public override void clickDownBehavior()
    {
        Active();
        OnMouseExit();
    }

    public override void Active()
    {
        PovManager.Inst.SetCurrentRocketPOV(GetRocketPOV());
    }

    public override bool IsActivable()
    {
        return PovManager.Inst.CurrentRocketPOV != GetRocketPOV();
    }
}
