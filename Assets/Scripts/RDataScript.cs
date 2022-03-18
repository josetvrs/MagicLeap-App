using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using UnityEngine.XR.MagicLeap;
using TMPro;

public class RDataScript : MonoBehaviour
{
    
    public TMP_Text xPosition, yPosition, zPosition, gripperStatus;
    public TMP_Text modeText, statusText, iterationText;
    private string xPos, yPos, zPos, gripper; //TMP of gripper information
    private string status, location, iteration; //TMP of process information
    private string url;

    private float waitTime = 0.5f;
    private float timer = 0.0f;

    private MLInput.Controller controller;
    public GameObject HeadlockedCanvas;
    public GameObject controllerInput;

    // Start is called before the first frame update
    void Start()
    {
        url = "https://script.google.com/macros/s/AKfycbzzgEyuU0HdFlHQmelb544msq8PYig-V-_j1yU5oiz-jgkLf-g/exec";
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);
        
    }

    // Update is called once per frame
    void Update()
    {
        // timer implemented to retrieve robot information every half second
        //StartCoroutine(RetrieveData());
        timer += Time.deltaTime;
        if (timer > waitTime){
            StartCoroutine(RetrieveData());
            timer = 0.0f;
        }
    }

    IEnumerator RetrieveData()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(string.Format("Error: {0}",request.error));
        }

        else
        {
            // Response can be accessed through: request.downloadHandler.text
            Debug.Log(data);

            var robot = JsonConvert.DeserializeObject <MyObject> (data);
     
            xPosition.SetText(robot.xPos);
            yPosition.SetText(robot.yPos);
            zPosition.SetText(robot.zPos);
            gripperStatus.SetText(robot.gripper);
            statusText.SetText(robot.status);
            modeText.SetText(robot.mode);
            iterationText.SetText(robot.iterations);
        }
    }
    public class MyObject
    {
        public string xPos {
            get;
            set;
        }
        public string yPos {
            get;
            set;
        }
        public string zPos {
            get;
            set;
        }
        public string gripper {
            get;
            set;
        }
        public string mode {
            get;
            set;
        }
        public string iterations {
            get;
            set;
        }
        public string status {
            get;
            set;
        }
    }

    private void OnDestroy(){
        MLInput.Stop();
    }
}
