using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneralConnectorManagement : MonoBehaviour
{
    public List<ConnectorManager> connectorManagers;

    public static GeneralConnectorManagement _inst;
    public static GeneralConnectorManagement Inst { get => _inst; }

    // Use this for initialization
    void Start()
    {
        if (_inst == null)
        {
            _inst = this;
        }

        connectorManagers = FindObjectsOfType<ConnectorManager>()
            .OrderBy(x => (x.transform.rotation.eulerAngles.y+360) % 360)
            .ToList();
    }

    public bool Connect(Transform t)
    {
        PipeSimulator pipe = GetClosestConnector(t);
        if (pipe.otherConnector) return false;
        pipe.otherConnector = t;
        return true;
    }

    public bool Disconnect(Transform t)
    {
        PipeSimulator pipe = GetClosestConnector(t);
        if (pipe.otherConnector != t) return false;
        pipe.otherConnector = null;
        return true;
    }

    private PipeSimulator GetClosestConnector(Transform t)
    {
        int index = Mathf.RoundToInt(((t.rotation.eulerAngles.y + 180) % 360) / 45);
        return connectorManagers[index].GetClosestConnector(t);
    }
}
