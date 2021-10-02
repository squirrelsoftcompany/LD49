using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class PipeSimulator : MonoBehaviour
{
    public Transform otherConnector;

    //Line renderer used to display the rope
    private LineRenderer lineRenderer;
    
    private float connecterForwardFactor = 0f;
    private float connectorUpFactor = 0f;
    private float otherForwardFactor = 0f;
    private float otherUpFactor = 0f;

    private float middleForwardFactor = 0f;
    private float middleUpFactor = 0f;

    //A list with all rope sections
    private List<Vector3> allRopeSections = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //Init the line renderer we use to display the rope
        lineRenderer = GetComponent<LineRenderer>();

        //Init factors
        connecterForwardFactor = Random.Range(0.1f, 0.4f);
        connectorUpFactor = Random.Range(-0.1f, 0.1f);
        otherForwardFactor = Random.Range(0.1f, 0.4f);
        otherUpFactor = Random.Range(-0.1f, 0.1f);

        middleForwardFactor = Random.Range(0.25f, 0.75f);
        middleUpFactor = -Random.Range(0.1f, 0.25f);

        //Init renderer visibility
        if (otherConnector == null)
        {
            lineRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (otherConnector != null)
        {
            DisplayRope();
        }
    }

    //Display the rope with a line renderer
    private void DisplayRope()
    {
        //Update the list with rope sections by approximating the rope with a bezier curve
        Vector3 A = transform.position;
        Vector3 E = otherConnector.position;

        float AEmagnitude = (A - E).magnitude;

        //This-Middle control point
        Vector3 B = A + transform.forward * (AEmagnitude * connecterForwardFactor) + transform.up * (AEmagnitude * connectorUpFactor);

        //Middle-Other control point
        Vector3 D = E + otherConnector.forward * (AEmagnitude * otherForwardFactor) + otherConnector.up * (AEmagnitude * otherUpFactor);

        //Middle control point
        Vector3 C = Vector3.Lerp(A, E, middleForwardFactor) + otherConnector.up * (AEmagnitude * middleUpFactor);

        //Get the positions
        BezierCurve.GetBezierCurve(allRopeSections, 0.05f, A, B, C, D, E);

        //An array with all rope section positions
        Vector3[] positions = new Vector3[allRopeSections.Count];

        for (int i = 0; i < allRopeSections.Count; i++)
        {
            positions[i] = allRopeSections[i];
        }

        //Add the positions to the line renderer
        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);
    }
}
