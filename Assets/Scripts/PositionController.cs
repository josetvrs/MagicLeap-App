using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using TMPro;

public class PositionController : MonoBehaviour
{
    private MLInput.Controller controller;
    
    string gripper;
    public TMP_Text xController, yController, zController;
    public TMP_Text xPos, yPos, zPos;
    private float moveX, moveY, moveZ;

    private string json;
    
    [Serializable]
    public class PositionClass
    {
        public string positionX;
        public string positionY;
    }

    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);

        moveX = float.Parse(xPos.text);
        moveY = float.Parse(yPos.text);
        moveZ = float.Parse(zPos.text);


        xController.SetText(xPos.text);
        yController.SetText(yPos.text);
        zController.SetText(zPos.text);

    }
    
    void UpdateTouchPad(){
        if (controller.Touch1Active)
        {
            float x = controller.Touch1PosAndForce.x;
            float y = controller.Touch1PosAndForce.y;
            float force = controller.Touch1PosAndForce.z;

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


    // Update is called once per frame
    void Update()
    {
        UpdateTouchPad();
        xController.SetText(moveX.ToString());
        yController.SetText(moveY.ToString());
        zController.SetText(moveZ.ToString());
    }

    void OnDestroy()
    {
        MLInput.Stop();
    }

    private string GetPosition()
    {
        PositionClass position = new PositionClass();
        position.positionX = xController.ToString();
        position.positionY = yController.ToString();

        json = JsonUtility.ToJson(position); 
        return json;
    }
    

}
