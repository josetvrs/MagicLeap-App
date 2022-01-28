using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class PlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    private MLInput.Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;
        controller = MLInput.GetController(MLInput.Hand.Left);

        
    }

    void OnButtonDown(byte controllerId, MLInput.Controller.Button button){
        if (button == MLInput.Controller.Button.Bumper)
        {
            Debug.Log("Button Pressed");
            RaycastHit hit;
            if (Physics.Raycast(controller.Position, transform.forward, out hit))
            {
                GameObject placeObject = Instantiate(objectToPlace, hit.point, Quaternion.Euler(hit.normal));
                Debug.Log("Object Created");
            }
        }

    }

    private void OnDestroy() {
        MLInput.Stop();
        MLInput.OnControllerButtonDown -= OnButtonDown;
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
