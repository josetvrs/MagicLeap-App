using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;
using TMPro;

public class RobotController : MonoBehaviour
{
    // Start is called before the first frame update
    private MLInput.Controller controller;
    float moveX, moveY;
    public TMP_Text xPosition, yPosition, gripperStatus;
    public TMP_Text xController, yController, gripperSt;

    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);
        moveX = float.Parse(xPosition.text.ToString(),System.Globalization.CultureInfo.InvariantCulture);
        moveY = float.Parse(yPosition.text.ToString(),System.Globalization.CultureInfo.InvariantCulture);

        //Copy real robot position to new labels
        xController.SetText(moveX.ToString());
        yController.SetText(moveY.ToString()); 
        gripperSt.SetText(gripperStatus.text.ToString());

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
    void Update()
    {
        UpdateTouchPad();
        xController.SetText(moveX.ToString());
        yController.SetText(moveY.ToString());
        gripperSt.SetText(gripperStatus.ToString());
    }

    void OnDestroy()
    {
        MLInput.Stop();
    }
}
