using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;
using TMPro;

public class PositionController : MonoBehaviour
{
    private MLInput.Controller controller;
    float moveX, moveY;
    string gripper;
    public TMP_Text xController, yController, gripperSt;
    bool trigger = true;

    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);
        moveX = 120.0f;
        moveY = 0.0f;
        gripper = "Open";

        xController.SetText(moveX.ToString());
        yController.SetText(moveY.ToString());
        gripperSt.SetText(gripper);
    }
    
    void UpdateTouchPad(){
        if (controller.Touch1Active)
        {
            float x = controller.Touch1PosAndForce.x;
            float y = controller.Touch1PosAndForce.y;
            float force =controller.Touch1PosAndForce.z;

            if (force > 0)
            {
                if(x > 0.5)
                {
                    moveX = moveX - 1.5f;

                }
                if(x < -0.5)
                {
                    moveX = moveX + 1.5f;

                }
                if(y > 0.3)
                {
                    moveY = moveY - 1.5f;

                }
                if(y < -0.3)
                {
                    moveY = moveY + 1.5f;

                }
            }
        }
        
    }

    void UpdateTriggerInfo(){
        // Controller trigger is pressed
        if(controller.TriggerValue > 0.8f){
            if(trigger == true){
                if(gripper == "Close")
                {
                    gripper = "Open";
                }
                else if(gripper == "Open")
                {
                    gripper = "Close";
                }
                trigger = false;
            }
            

        }
        if(controller.TriggerValue < 0.2f)
        {
            trigger = true;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTouchPad();
        xController.SetText(moveX.ToString());
        yController.SetText(moveY.ToString());
        UpdateTriggerInfo();
        gripperSt.SetText(gripper);
    }

    void OnDestroy()
    {
        MLInput.Stop();
    }
}
