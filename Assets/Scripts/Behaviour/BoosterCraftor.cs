using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterCraftor : MonoBehaviour
{
    public List<GameObject> m_pressurePrefabs;
    public List<GameObject> m_purgeErgolPrefabs;
    public List<GameObject> m_purgeAllPrefabs;
    
    public List<Transform> m_activableLocations;
    private List<GameObject> _activables;

    [HideInInspector] public RocketData data;

    // Start is called before the first frame update
    void Start()
    {
        _activables = new List<GameObject>();
        foreach (var x in m_activableLocations)
            _activables.Add(null);

        // manage buttons
        if (data.m_twoDistinctPurgeButton)
        {
            PlaceActivable(RocketCraftor.RandomGet(m_purgeErgolPrefabs));
            PlaceActivable(RocketCraftor.RandomGet(m_pressurePrefabs));
        }
        else
        {
            PlaceActivable(RocketCraftor.RandomGet(m_purgeAllPrefabs));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceActivable(GameObject go)
    {
        if (_activables.TrueForAll(x => x != null))
        {
            Debug.LogError("Trying to add an activable but there is no position left.");
            return;
        }

        int index = Random.Range(0, m_activableLocations.Count);
        while (_activables[index] != null)
            index = Random.Range(0, m_activableLocations.Count);

        _activables[index] = Instantiate(go, m_activableLocations[index]);
    }
}