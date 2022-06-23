using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.Text;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json;

public class CanvasAlarm : MonoBehaviour
{
    private TcpClient socketConnection;

    public GameObject AlarmPanel; //Alarm warning
    public GameObject AboutPanel; //Panel showing mechanism 3D model and information
    public GameObject EndEffector; //Panel showing XYZ position from database
    public GameObject HomeButton;
    public GameObject OkButton;

    //Alarm instructions
    public GameObject Step1Panel;
    public GameObject Step2Panel;
    public GameObject Step3Panel;

    //Gripper position and status
    public TMP_Text xP, yP, status;
    private string clientMessage;

    //JSON for TCP/IP communication
    private string json;
    [Serializable]
    public class PositionClass
    {
        public string positionX;
        public string positionY;
    }

    //trigger variable for trigger timer
    bool trigger;
    
    private bool alarmTrigger; //Triggers the alarm in the UI
    private bool infoEnabled;

    //Magic Leap controller
    private MLInput.Controller controller;
    public GameObject controllerInput;

    // Start is called before the first frame update
    void Start()
    {
        //Enabling controller input
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);

        //Initialize variables
        alarmTrigger = false;
        trigger = false;
        infoEnabled = false;

        //Start trigger timer
        //StartCoroutine(ChangeTrigger());

        //Activate/Deactivate information panels
        AlarmPanel.gameObject.SetActive(false);
        EndEffector.gameObject.SetActive(true);
        AboutPanel.gameObject.SetActive(true);
        HomeButton.gameObject.SetActive(false);
        OkButton.gameObject.SetActive(false);

        //Deactivate alarm instructions
        Step1Panel.gameObject.SetActive(false);
        Step2Panel.gameObject.SetActive(false);
        Step3Panel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Check for an alarm and display warnining if detected (if not done already)
        if (alarmTrigger == false){
            if (status.text == "ALARM!"){
                AlarmPanel.gameObject.SetActive(true);
                EndEffector.gameObject.SetActive(false);
                alarmTrigger = true;
            }
        }
        //Check button interaction with controller trigger
        else{
            CheckTrigger();
        }
        
    }
    //On destroy method
    private void OnDestroy(){
        MLInput.Stop();
    }

    //Check controller trigger
    private void CheckTrigger(){
        if(controller.TriggerValue > 0.5f){
            RaycastHit hit;
            if(Physics.Raycast(controllerInput.transform.position, controllerInput.transform.forward, out hit)){
                if(hit.transform.gameObject.name == "AlarmButton" && trigger == false)
                {
                    trigger = true;

                    AlarmPanel.gameObject.SetActive(false);
                    
                    Step1Panel.gameObject.SetActive(true);
                    Step2Panel.gameObject.SetActive(false);
                    Step3Panel.gameObject.SetActive(false);
                    //Connect to server
                    ConnectToTcpServer();

                }
                else if(hit.transform.gameObject.name == "Continue1" && trigger == false)
                {
                    trigger = true;
                    Debug.Log("Continue 1 clicked");
                    Step1Panel.gameObject.SetActive(false);
                    Step2Panel.gameObject.SetActive(false);
                    Step3Panel.gameObject.SetActive(false);
                    Debug.Log("instructions");
                    
                    OkButton.gameObject.SetActive(true);
                    
                    Debug.Log("controller position");


                }
                else if(hit.transform.gameObject.name == "OkButton" && trigger == false)
                {
                    trigger = true;

                    Step1Panel.gameObject.SetActive(false);
                    Step2Panel.gameObject.SetActive(true);
                    Step3Panel.gameObject.SetActive(false);
                    Debug.Log("instructions");
                    
                    OkButton.gameObject.SetActive(false);
                }
                else if(hit.transform.gameObject.name == "CalibrationButton" && trigger == false)
                {
                    
                    trigger = true;

                    clientMessage = "home";
                    SendMessage(clientMessage); 
                    Debug.Log("calibraiton button clicked");
                    Step1Panel.gameObject.SetActive(false);
                    Step2Panel.gameObject.SetActive(false);
                    Step3Panel.gameObject.SetActive(true);
                    Debug.Log("instructions");
                    
                    EndEffector.gameObject.SetActive(false);
                    HomeButton.gameObject.SetActive(false);
                }
                else if(hit.transform.gameObject.name == "Continue2" && trigger == false)
                {
                    trigger = true;

                    Step1Panel.gameObject.SetActive(false);
                    Step2Panel.gameObject.SetActive(false);
                    Step3Panel.gameObject.SetActive(false);

                    HomeButton.gameObject.SetActive(true);
                    EndEffector.gameObject.SetActive(true);
                }
                //Instruction 3 done
                else if(hit.transform.gameObject.name == "Continue3" && trigger == false)
                {
                    trigger = true;
                    clientMessage = "reset";
                    EndEffector.gameObject.SetActive(true);

                    Step1Panel.gameObject.SetActive(false);
                    Step2Panel.gameObject.SetActive(false);
                    Step3Panel.gameObject.SetActive(false);

                    SendMessage(clientMessage); 

                    alarmTrigger = false;
                    socketConnection = null;
                }
                
                //Toggle function for About button showing about panel
                else if(hit.transform.gameObject.name == "AboutMechanism" && trigger == false)
                {
                    if(infoEnabled == true)
                    {
                        trigger = true;
                        AboutPanel.gameObject.SetActive(false);
                        infoEnabled = false;
                    }
                    else if(infoEnabled == false)
                    {
                        trigger = true;
                        AboutPanel.gameObject.SetActive(true);
                        infoEnabled = true;
                    }
                }
                
            }
            
        }
        else if(controller.TriggerValue < 0.2f){
            trigger = false;
        }
    }
    
    // ---------------------------------------TCP----------------------------------
    //Connect to server (Raspberry Pi)
    private void ConnectToTcpServer()
    {
        socketConnection = new TcpClient("10.22.245.178", 5050);

    }
    //Send reset message to server
    private void SendMessage(string clientMessage)
    {
        if(socketConnection == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if(stream.CanWrite)
            {
                byte[] clientMessageAsByteArray = Encoding.Default.GetBytes(clientMessage);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log(clientMessageAsByteArray);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);

        }
    }
    //Get position from controller XY and create JSON string
    private string GetPosition()
    {
        PositionClass position = new PositionClass();
        position.positionX = xP.text;
        position.positionY = yP.text;

        json = JsonUtility.ToJson(position); 
        position = null;
        return json;
    }
    private void SendPosition()
    {
        if(socketConnection == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if(stream.CanWrite)
            {
                string clientMessage = GetPosition();
                byte[] clientMessageAsByteArray = Encoding.Default.GetBytes(clientMessage);
                clientMessage = null;
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log(clientMessageAsByteArray);
                clientMessageAsByteArray = null;
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);

        }
    }
}
