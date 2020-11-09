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
        private readonly string MQTT_BROKER_ADDRESS = "35.158.43.238";

        [Obsolete]
        public void Subscribe(string topic)
        {
            var address = IPAddress.Parse(MQTT_BROKER_ADDRESS);
            address.MapToIPv4();
            
            // create client instance 
            MqttClient client = new MqttClient(address);

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic  with QoS 2 
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        [Obsolete]
        public void Publish(string topic, string message)
        {
            // create client instance 
            MqttClient client = new MqttClient(IPAddress.Parse(MQTT_BROKER_ADDRESS));

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            string strValue = Convert.ToString(message);

            // publish a message on "/home/temperature" topic with QoS 2 
            client.Publish(topic, Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
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
