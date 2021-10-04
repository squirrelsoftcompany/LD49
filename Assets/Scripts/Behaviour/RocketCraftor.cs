using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketCraftor : MonoBehaviour
{
    public List<ModuleCraftor> modulePrefabs;
    public List<BoosterCraftor> boosterPrefabs;
    public List<GameObject> capPrefabs;

    public RocketData rocketData;
    public Transform rocket;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchChangeRocketAnimation()
    {
        GetComponent<Animator>().SetTrigger("ChangeRocket");
    }

    public void CraftNewRocket()
    {
        // Clean
        foreach (Transform t in rocket)
        {
            Destroy(t.gameObject);
        }

        // Generate
        if (GameManager.Inst.Difficulty == 1)
            rocketData = RocketData.GenerateEasyRocket();
        else if (GameManager.Inst.Difficulty == 2)
            rocketData = RocketData.GenerateNormalRocket();
        else
            rocketData = RocketData.GenerateHardRocket();

        // add booster
        float height = 2.5f + rocket.position.y;
        BoosterCraftor bc = Instantiate(RandomGet(boosterPrefabs), new Vector3(0, height, 0), Quaternion.identity, rocket);
        bc.data = rocketData;
        height += 4;
        PovManager.RocketPOV rocketPOV = PovManager.RocketPOV.eStage1;

        // add each module
        foreach (ModuleData mod in rocketData.m_modules)
        {
            ModuleCraftor moduleCraftor = Instantiate(RandomGet(modulePrefabs), new Vector3(0, height, 0), Quaternion.AngleAxis(Random.Range(0, 8) * 45, Vector3.up), rocket);
            moduleCraftor.data = mod;
            moduleCraftor.GetComponent<ModuleBehavior>().RocketPOV = rocketPOV;
            rocketPOV += 1;
            height += 3;
        }

        // add cap
        GameObject cap = Instantiate(RandomGet(capPrefabs), new Vector3(0, height, 0), Quaternion.identity, rocket);
        cap.GetComponentInChildren<ActivableSwitchViewCap>().capPOV = rocketPOV;
    }

    public static T RandomGet<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
