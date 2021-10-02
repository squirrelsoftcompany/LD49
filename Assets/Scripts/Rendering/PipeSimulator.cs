using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSimulator : MonoBehaviour
{
    public Transform otherConnector;

    //Line renderer used to display the rope
    private LineRenderer lineRenderer;
    
    private float bForwardFactor = 0f;
    private float bUpFactor = 0f;
    private float cForwardFactor = 0f;
    private float cUpFactor = 0f;

    //A list with all rope sections
    private List<Vector3> allRopeSections = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //Init the line renderer we use to display the rope
        lineRenderer = GetComponent<LineRenderer>();

        //Init factors
        bForwardFactor = Random.Range(0.1f, 0.5f) * (Random.value > 0.5 ? -1 : 1);
        bUpFactor = Random.Range(0.1f, 0.25f) * (Random.value > 0.5 ? -1 : 1);
        cForwardFactor = Random.Range(0.1f, 0.5f) * (Random.value > 0.5 ? -1 : 1);
        cUpFactor = Random.Range(0.1f, 0.25f) * (Random.value > 0.5 ? -1 : 1);
    }

    // Update is called once per frame
    void Update()
    {
        DisplayRope();
    }

    //Display the rope with a line renderer
    private void DisplayRope()
    {
        //This is not the actual width, but the width use so we can see the rope
        float ropeWidth = 0.2f;

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;


        //Update the list with rope sections by approximating the rope with a bezier curve
        //A Bezier curve needs 4 control points
        Vector3 A = transform.position;
        Vector3 D = otherConnector.position;

        //Upper control point
        //To get a little curve at the top than at the bottom
        Vector3 B = A + transform.forward * ((A - D).magnitude * bForwardFactor) + transform.up * ((A - D).magnitude * bUpFactor);

        //Lower control point
        Vector3 C = D + otherConnector.forward * ((A - D).magnitude * cForwardFactor) + otherConnector.up * ((A - D).magnitude * cUpFactor);

        //Get the positions
        BezierCurve.GetBezierCurve(A, B, C, D, allRopeSections);

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
