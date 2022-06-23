using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using TMPro;

public class M2MqttController : M2MqttUnityClient
{
    public TMP_Text xPosition, yPosition, zPosition;
    private string xPos, yPos,zPos, json;
    private bool connected = false;

    [Serializable]
    public class PositionClass
    {
        public string positionX;
        public string positionY;
        public string positionZ;
    }
    //----------------------------------------PUBLISH-------------------------------------
    public void TestPublish(string msg)
    {
        client.Publish("AugmentedCell/manual", System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        Debug.Log("JSON MSG: " + msg);
    }
    //---------------------------------------CONNECTION----------------------------------------
    protected override void OnConnecting()
    {
        base.OnConnecting();
        Debug.Log("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
    }
    protected override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Connected to broker on " + brokerAddress + "\n");
        connected = true;
    }
    //----------------------------------------TOPICS-------------------------------------------
    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { "AugmentedCell/manual" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { "AugmentedCell/manual" });
    }
    //--------------------------------------DISCONNECTION-----------------------------------
    protected override void OnConnectionFailed(string errorMessage)
    {
        Debug.Log("CONNECTION FAILED! " + errorMessage);
    }
    protected override void OnDisconnected()
    {
        Debug.Log("Disconnected.");
    }
    protected override void OnConnectionLost()
    {
        Debug.Log("CONNECTION LOST!");
        connected = false;
    }
    //-------------------------------------------START-----------------------------------
    protected override void Start()
    {
        Connect();
        Debug.Log("Ready.");
        base.Start();
    }
    //------------------------------------POSITIONS-------------------------------
    protected override void Update()
    {
        base.Update(); // call ProcessMqttEvents()
        
    }
    public void SendButton()
    {
        json = GetPosition();
        if(connected==true)
        {
            TestPublish(json);
        }

    }
    private string GetPosition()
    {
        xPos = xPosition.text;
        yPos = yPosition.text;
        zPos = zPosition.text;
        PositionClass position = new PositionClass();
        position.positionX = xPos;
        position.positionY = yPos;
        position.positionZ = zPos;

        json = JsonUtility.ToJson(position);
        position = null;
        return json;
    }
    private void OnDestroy()
    {
        Disconnect();
    }
}
