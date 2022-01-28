using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ManipulateObject : MonoBehaviour
{
    private MLInput.Controller controller;
    GameObject selectedGameObject;
    public GameObject attachPoint;
    public GameObject ControllerObject;
    bool trigger;

    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);
    }

    void UpdateTriggerInfo(){
        // Controller trigger is pressed
        if(controller.TriggerValue > 0.8f){
            if(trigger == true){
                RaycastHit hit;
                if(Physics.Raycast(controller.Position, transform.forward, out hit)){
                    //Check if the object hit has a tag = Interactable. If it does, it becomes the selectedGameObject
                    if(hit.transform.gameObject.tag == "Interactable"){
                        selectedGameObject = hit.transform.gameObject;
                        //Disable gravity
                        selectedGameObject.GetComponent<Rigidbody>().useGravity = false;
                        // Hit position is the attachposition
                        attachPoint.transform.position = hit.transform.position;
                    }
                }
                //We are allowed to press the trigger again
                trigger = false;
            } 
        }
        //Reset trigger
        if(controller.TriggerValue < 0.2f){
            trigger = true;
            //Let gp the game object when we let go the trigger
            if(selectedGameObject != null){
                selectedGameObject.GetComponent<Rigidbody>().useGravity = true;
                selectedGameObject = null;
            }
        }
    }

    void UpdateTouchPad(){
        if(controller.Touch1Active){
            float x = controller.Touch1PosAndForce.x;
            float y = controller.Touch1PosAndForce.y;
            float force = controller.Touch1PosAndForce.z;

            if(force > 0){
                if(x > 0.5 || x < -0.5){
                    selectedGameObject.transform.localScale += selectedGameObject.transform.localScale * x * Time.deltaTime;
                }
                if(y > 0.3 || y < -0.3){
                    attachPoint.transform.position = Vector3.MoveTowards(attachPoint.transform.position, gameObject.transform.position, -y * Time.deltaTime);

                }
            }

        }
    }

    // Update is called once per frame
    private void OnDestroy() {
        MLInput.Stop();
    }

    void Update()
    {
        transform.position = controller.Position;
        transform.rotation = controller.Orientation;
        //If we have a selectedGameObject, the its position remains attached to the attachPoint
        if(selectedGameObject){
            selectedGameObject.transform.position = attachPoint.transform.position;
            selectedGameObject.transform.rotation = gameObject.transform.rotation;
        }
        UpdateTriggerInfo();
        UpdateTouchPad();
        
    }


}

