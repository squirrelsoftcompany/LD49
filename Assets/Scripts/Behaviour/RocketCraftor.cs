using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketCraftor : MonoBehaviour
{
    public RocketData rocketData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CraftNewRocket()
    {
        rocketData = RocketData.GenerateHardRocket();
    }
}
