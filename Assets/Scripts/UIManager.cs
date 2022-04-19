using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private MLInput.Controller controller;
    public GameObject HeadlockedCanvas;
    public GameObject controllerInput;
    
    #region Unity Methods
    void Start()
    {
        MLInput.Start();
        MLInput.OnControllerButtonUp += OnButtonUp;
        controller = MLInput.GetController(MLInput.Hand.Left);
    }
    private void OnDestroy(){
        MLInput.OnControllerButtonUp -= OnButtonUp;
        MLInput.Stop();
    }
    void Update()
    {
        CheckTrigger();
    }
    #endregion

    #region Private Methods
    void CheckTrigger(){
        if(controller.TriggerValue > 0.2f){
            RaycastHit hit;
            if(Physics.Raycast(controllerInput.transform.position, controllerInput.transform.forward, out hit)){
                if(hit.transform.gameObject.name == "AugmentedCell")
                {
                    SceneManager.LoadScene("RobotScene");
                }
                else if(hit.transform.gameObject.name == "ManualMode")
                {
                    SceneManager.LoadScene("ManualScene");
                }
                else if(hit.transform.gameObject.name == "RobotTwin")
                {
                    SceneManager.LoadScene("TwinScene");
                }
            }
        }
    }
    void OnButtonUp(byte controllerId, MLInput.Controller.Button button){
        if (button == MLInput.Controller.Button.HomeTap)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    #endregion
}
