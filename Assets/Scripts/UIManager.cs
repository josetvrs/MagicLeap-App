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
        if(controller.TriggerValue > 0.8f){
            RaycastHit hit;
            if(Physics.Raycast(controllerInput.transform.position, controllerInput.transform.forward, out hit)){
                if(hit.transform.gameObject.name == "AugmentedCell")
                {
                    MLHandTracking.Stop();
                    SceneManager.LoadScene("CellInstructions");
                    
                }
                else if(hit.transform.gameObject.name == "ContinueButton1")
                {
                    MLHandTracking.Stop();
                    SceneManager.LoadScene("RobotScene");
                }
                else if(hit.transform.gameObject.name == "ManualMode")
                {
                    MLHandTracking.Stop();
                    SceneManager.LoadScene("ManualScene");
                }
                
                else if(hit.transform.gameObject.name == "RobotTwin")
                {
                    MLHandTracking.Stop();
                    SceneManager.LoadScene("TwinScene");
                }
                else if(hit.transform.gameObject.name == "ExitButton"){
                    MLHandTracking.Stop();
                    Application.Quit();
                }
            }
        }
    }
    // HOME BUTTON
    void OnButtonUp(byte controllerId, MLInput.Controller.Button button){
        if (button == MLInput.Controller.Button.HomeTap)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    #endregion
}
