using System;
using uPLibrary.Networking.M2Mqtt;

namespace MQTT_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello IOT!");

            var client = new Client();

            client.Subscribe("iot/a6");
            
            while (true)
            {
                //Console.WriteLine("Select QOS Level to Publish:");

                var input = Console.ReadLine();
                client.Publish_AtLeastOnce("iot/a6", input);
            }
        }

    }
}
