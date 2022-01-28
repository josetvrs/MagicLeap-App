using System.Net;
using System.Text;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTC : MonoBehaviour
{
    MqttClient client;
    string clientId;

    public string BrokerAddress = "broker.hivemq.com";
    public int Port = 1883;
    public string userName = null;
    public string password = null;
    public string msg = "OFF";
    public string Topic = "/StatusTL1";

    private void Start()
    {

        Debug.LogWarning("connecting to " + BrokerAddress + ":" + Port);
        Debug.LogWarning("App supported protocols:   " + ServicePointManager.SecurityProtocol);

        client = new MqttClient(BrokerAddress);

        // register a callback-function (we have to implement, see below) which is called by the library when a message was received
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        // use a unique id as client id, each time we start the application
        clientId = "JAVP";//Guid.NewGuid().ToString();
        client.Connect(clientId);
        Publish();
    }

    void Publish()
    {
        // publish a message with QoS 2
        client.Publish(Topic, Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

    }

    void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
        Debug.LogWarning(ReceivedMessage);
    }


}