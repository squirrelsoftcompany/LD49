using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleCraftor : MonoBehaviour
{
    public List<GameObject> m_connectorPrefabs;
    public List<GameObject> m_freezerPrefabs;
    public List<GameObject> m_pressurePrefabs;
    public List<GameObject> m_purgePrefabs;
    public List<GameObject> m_displayPrefabs;

    public List<Transform> m_activableLocations;
    private List<GameObject> _activables;

    public ModuleData data;
    private ModuleBehavior moduleBehavior;

    // Start is called before the first frame update
    void Start()
    {
        moduleBehavior = GetComponent<ModuleBehavior>();

        _activables = new List<GameObject>();
        foreach (var x in m_activableLocations)
            _activables.Add(null);

        // manage connectors
        int connectorCount = Random.Range(data.MinimalNumberOfConnector(), 3+1);
        for (int i = 0; i < connectorCount; i++)
        {
            PlaceActivable(RocketCraftor.RandomGet(m_connectorPrefabs));
        }

        // manage buttons
        if (data.m_purgeErgolPresent)
            PlaceActivable(RocketCraftor.RandomGet(m_purgePrefabs));
        if (data.m_purgePressionPresent)
            PlaceActivable(RocketCraftor.RandomGet(m_pressurePrefabs));

        // freezer
        PlaceActivable(RocketCraftor.RandomGet(m_freezerPrefabs));

        // display
        GameObject display = PlaceActivable(RocketCraftor.RandomGet(m_displayPrefabs));
        GetComponent<ModuleBehavior>().mMonitor = display;

        // Generate fuel goal
        List<ergolInTank> pGoal = new List<ergolInTank> {};
        int minFuel = 0;
        int newQuantity;
        int maxFull = (100 - data.m_fuels.Count * 10);
        for (int i=0;i< data.m_fuels.Count;i++)
        {
            if(i < data.m_fuels.Count-1)
            {
                newQuantity = Random.Range(minFuel+10, maxFull + 1);
            }
            else
            {
                newQuantity = 100;
            }

            minFuel = newQuantity;
            maxFull = (100 - (data.m_fuels.Count-i) * 10);
            pGoal.Add(new ergolInTank(data.m_fuels[i], minFuel));
        }
        moduleBehavior.setGoal(pGoal);
    }

    public GameObject PlaceActivable(GameObject go)
    {
        if (_activables.TrueForAll(x => x != null))
        {
            Debug.LogError("Trying to add an activable but there is no position left.");
            return null;
        }

        int index = Random.Range(0, m_activableLocations.Count);
        while(_activables[index] != null)
            index = Random.Range(0, m_activableLocations.Count);

        GameObject activable = Instantiate(go, m_activableLocations[index]);
        var ab = activable.GetComponentInChildren<ActivableBehaviour>();
        if (ab) ab.mParentModule = moduleBehavior;

        _activables[index] = activable;
        return _activables[index];
    }
}