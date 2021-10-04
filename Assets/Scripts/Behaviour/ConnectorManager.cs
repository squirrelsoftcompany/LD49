using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectorManager : MonoBehaviour
{
    public List<PipeSimulator> Pipes;

    const float OFFSET_HEIGHT = 6.5f;

    // Start is called before the first frame update
    void Start()
    {
        Pipes = GetComponentsInChildren<PipeSimulator>()
            .OrderBy(x => x.transform.position.y)
            .ToList();
    }

    public PipeSimulator GetClosestConnector(Transform t)
    {
        int index = Mathf.RoundToInt((t.position.y - OFFSET_HEIGHT) / 3);
        return Pipes[index];
    }
}
