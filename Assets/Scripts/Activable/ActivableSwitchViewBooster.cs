using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableSwitchViewBooster : ActivableSwitchView
{
    public override PovManager.RocketPOV GetRocketPOV()
    {
        return PovManager.RocketPOV.eBooster;
    }
}
