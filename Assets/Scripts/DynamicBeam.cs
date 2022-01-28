using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class DynamicBeam : MonoBehaviour
{
    //private MLInput.Controller controller2;
    public GameObject controller;
    private LineRenderer beamLine;

    public Color startColor;
    public Color endColor;
    
    void Start()
    {
        //controller2 = MLInput.GetController(MLInput.Hand.Left);
        beamLine = GetComponent<LineRenderer>();
        beamLine.startColor = startColor;
        beamLine.endColor = endColor;
    }

    void Update()
    {
        //transform.position = controller2.Position;
        //transform.rotation = controller2.Orientation;
        transform.position = controller.transform.position;
        transform.rotation = controller.transform.rotation;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            beamLine.useWorldSpace = true;
            beamLine.SetPosition(0, transform.position);
            beamLine.SetPosition(1, hit.point);
        }
        else
        {
            beamLine.useWorldSpace = false;
            beamLine.SetPosition(0, Vector3.zero);
            beamLine.SetPosition(1, Vector3.forward * 5);

        }
    }
}
