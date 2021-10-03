using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ManageFullDisplayVisibility : MonoBehaviour
{
    [TagSelector]
    public string m_tag = "";
    public float nearPlane = 3;

    List<FullDisplayVisibility> _FDVs;

    // Start is called before the first frame update
    void Start()
    {
        _FDVs = new List<FullDisplayVisibility>();

        GameObject[] gos = GameObject.FindGameObjectsWithTag(m_tag);
        foreach(var go in gos)
        {
            _FDVs.Add(go.AddComponent<FullDisplayVisibility>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        float camOrientation = transform.rotation.eulerAngles.y;
        //Vector3 projectedPosition = Vector3.ProjectOnPlane(transform.position, Vector3.up);
        foreach (var fdv in _FDVs)
        {
            float fdvOrientation = fdv.transform.rotation.eulerAngles.y;
            //Vector3 fdvProjectedPosition = Vector3.ProjectOnPlane(fdv.transform.position, Vector3.up);
            fdv.Visibility = Mathf.Abs(camOrientation - fdvOrientation) > nearPlane && Mathf.Abs((camOrientation -360) - fdvOrientation) > nearPlane;
            //fdv.Visibility = Vector3.Distance(projectedPosition, fdvProjectedPosition) > nearPlane;
        }
    }
}
