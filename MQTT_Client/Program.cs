using System;
using uPLibrary.Networking.M2Mqtt;

namespace MQTT_Client
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new Client();

            client.Subscribe("/home");
            
            while (true)
            {
                var input = Console.ReadLine();
                client.Publish("/home", input);
            }
        }
    }
}
