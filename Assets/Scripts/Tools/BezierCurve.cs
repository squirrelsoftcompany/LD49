using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Approximate the rope with a bezier curve
public static class BezierCurve
{
    //Update the positions of the rope section
    public static void GetBezierCurve(List<Vector3> allRopeSections, float resolution, params Vector3[] positions)
    {
        //Clear the list
        allRopeSections.Clear();

        if (positions.Length < 3)
            return;

        float t = 0;
        while (t <= 1f)
        {
            //Find the coordinates between the control points with a Bezier curve
            Vector3 newPos;
            switch
                (positions.Length)
            {
                case 3:
                    newPos = DeCasteljausAlgorithm(positions[0], positions[1], positions[2], t);
                    break;
                case 4:
                    newPos = DeCasteljausAlgorithm(positions[0], positions[1], positions[2], positions[3], t);
                    break;
                default:
                    newPos = DeCasteljausAlgorithm(positions[0], positions[1], positions[2], positions[3], positions[4], t);
                    break;
            }

            allRopeSections.Add(newPos);

            //Which t position are we at?
            t += resolution;
        }

        allRopeSections.Add(positions[positions.Length-1]);
    }

    public static void GetBezierCurve(Vector3 A, Vector3 B, Vector3 C, Vector3 D, Vector3 E, List<Vector3> allRopeSections)
    {
        //The resolution of the line
        //Make sure the resolution is adding up to 1, so 0.3 will give a gap at the end, but 0.2 will work
        float resolution = 0.1f;

        //Clear the list
        allRopeSections.Clear();


        float t = 0;

        while (t <= 1f)
        {
            //Find the coordinates between the control points with a Bezier curve
            Vector3 newPos = DeCasteljausAlgorithm(A, B, C, D, E, t);

            allRopeSections.Add(newPos);

            //Which t position are we at?
            t += resolution;
        }

        allRopeSections.Add(D);
    }

    //The De Casteljau's Algorithm
    static Vector3 DeCasteljausAlgorithm(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        //To make it faster
        float oneMinusT = 1f - t;

        //Layer 3 points
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;

        //Final interpolated position
        Vector3 U = oneMinusT * Q + t * R;

        return U;
    }

    static Vector3 DeCasteljausAlgorithm(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        //To make it faster
        float oneMinusT = 1f - t;

        //Layer 4 points
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * D;

        return DeCasteljausAlgorithm(Q, R, S, t);
    }

    static Vector3 DeCasteljausAlgorithm(Vector3 A, Vector3 B, Vector3 C, Vector3 D, Vector3 E, float t)
    {
        //To make it faster
        float oneMinusT = 1f - t;

        //Layer 1
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * D;
        Vector3 T = oneMinusT * D + t * E;

        return DeCasteljausAlgorithm(Q, R, S, T, t);
    }
}