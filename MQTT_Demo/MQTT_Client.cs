using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Windows.Controls;
using System.Threading;
using System.Diagnostics;
using System.Windows;

namespace MQTT_Demo
{
    public class MQTT_Client
    {
        private MqttClient client;
        //private readonly string MQTT_BROKER_ADDRESS = "broker.hivemq.com";
        //private readonly string MQTT_BROKER_ADDRESS = "192.168.2.199";
        private readonly string MQTT_BROKER_ADDRESS = "0.tcp.ngrok.io";
        private readonly int MQTT_BROKER_PORT = 13489;

        public bool Connected { get; private set; } = false;

        public void Connect(string clientID, string lastWillTopic, string lastwillMsg)
        {
            client = new MqttClient(MQTT_BROKER_ADDRESS, MQTT_BROKER_PORT, false, null, null, MqttSslProtocols.None);
            client.Connect(clientID, "", "", false, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true, lastWillTopic, lastwillMsg, false, 60);
            Connected = true;
        }

        public void Disconnect()
        {
            client.Disconnect();
            Connected = false;

            //unregister
            client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
        }

        public void Subscribe(string topic, byte qosLevel)
        {
            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic
            client.Subscribe(new string[] { topic }, new byte[] { qosLevel });
        }

        public void Publish(string topic, byte qosLevel, bool retain, string message)
        {
            string strValue = Convert.ToString(message);
            // publish a message
            client.Publish(topic, Encoding.UTF8.GetBytes(strValue), qosLevel, retain);
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var messages = Application.Current.MainWindow.FindName("messages") as TextBox;
                using (Stream s = new MemoryStream(e.Message))
                {
                    using (StreamReader reader = new StreamReader(s))
                    {
                        while (!reader.EndOfStream)
                        {
                            var message = $"{reader.ReadLine()}\n";
                            messages.Text += message;
                        }
                    }
                }
            });           
        }

    }
}
