using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.SceneManagement;

public class InstructionScript : MonoBehaviour
{
    private MLInput.Controller controller;
    public GameObject HeadlockedCanvas;
    public GameObject controllerInput;

    public GameObject Step1Panel;
    public GameObject Step2Panel;
    public GameObject Step3Panel;
    public GameObject Step4Panel;


    private bool trigger;


    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);

        Step1Panel.gameObject.SetActive(true);
        Step2Panel.gameObject.SetActive(false);
        Step3Panel.gameObject.SetActive(false);
        Step4Panel.gameObject.SetActive(false);


        trigger = true;
    }
    private void OnDestroy(){
        MLInput.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckTrigger();
        if (trigger == true){
            ChangeTrigger();
        }
        
    }
    
    void CheckTrigger(){
        if(controller.TriggerValue > 0.2f){
            RaycastHit hit;
            if(Physics.Raycast(controllerInput.transform.position, controllerInput.transform.forward, out hit)){
                if(hit.transform.gameObject.name == "Continue1" && trigger == false)
                {
                    trigger = true;

                    Step1Panel.gameObject.SetActive(false);
                    Step2Panel.gameObject.SetActive(true);
                    Step3Panel.gameObject.SetActive(false);
                    Step4Panel.gameObject.SetActive(false);  
                }
                else if(hit.transform.gameObject.name == "Continue2" && trigger == false)
                {
                    trigger = true;

                    Step1Panel.gameObject.SetActive(false);
                    Step2Panel.gameObject.SetActive(false);
                    Step3Panel.gameObject.SetActive(true);
                    Step4Panel.gameObject.SetActive(false);
                }
                else if(hit.transform.gameObject.name == "Continue3" && trigger == false)
                {
                    trigger = true;

                    Step1Panel.gameObject.SetActive(false);
                    Step2Panel.gameObject.SetActive(false);
                    Step3Panel.gameObject.SetActive(false);
                    Step4Panel.gameObject.SetActive(true);
                }
                else if(hit.transform.gameObject.name == "Continue4")
                {
                    SceneManager.LoadScene("RobotScene");
                }
                
            }
        }
        else{
            trigger = false;
        }
    }
    IEnumerator ChangeTrigger()
    {
        yield return new WaitForSeconds(3);
        trigger = false;
    }
}
