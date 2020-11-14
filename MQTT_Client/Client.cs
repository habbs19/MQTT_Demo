using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT_Client
{
    public class Client
    {
        private readonly string MQTT_BROKER_ADDRESS = "broker.hivemq.com";

        public void Subscribe(string topic)
        {                        
            // create client instance 
            MqttClient client = new MqttClient(MQTT_BROKER_ADDRESS);

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic  with QoS 2 
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }       

        public void Publish_AtLeastOnce(string topic, string message)
        {
            Publish(topic, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, message);
        }
        public void Publish_AtMostOnce(string topic, string message)
        {
            Publish(topic, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, message);
        }
        public void Publish_ExactlyOnce(string topic, string message)
        {
            Publish(topic, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, message);
        }
        public void Publish_GrantedFailure(string topic, string message)
        {
            Publish(topic, MqttMsgBase.QOS_LEVEL_GRANTED_FAILURE, message);
        }
        private void Publish(string topic, byte qosLevel, string message)
        {
            // create client instance 
            MqttClient client = new MqttClient(MQTT_BROKER_ADDRESS);

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            string strValue = Convert.ToString(message);

            // publish a message on "/home/temperature" topic with QoS 2 
            client.Publish(topic, Encoding.UTF8.GetBytes(strValue), qosLevel, false);
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received 
            using(Stream s = new MemoryStream(e.Message))
            {
                using(StreamReader reader = new StreamReader(s))
                {

                    while (!reader.EndOfStream)
                    {
                        Console.WriteLine(reader.ReadLine());
                    }
                    Console.WriteLine("Done reading");
                }
            }
        }

    }
}
