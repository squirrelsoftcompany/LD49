using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableSwitchViewCap : ActivableSwitchView
{
    public PovManager.RocketPOV capPOV;

    public override PovManager.RocketPOV GetRocketPOV()
    {
        return capPOV;
    }
}
