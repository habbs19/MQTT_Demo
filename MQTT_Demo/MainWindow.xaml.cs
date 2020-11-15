using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();

            this.qosLevels.ItemsSource = new List<string> { "AtLeastOnce", "AtMostOnce", "ExactlyOnce" };
            qosLevels.SelectedIndex = 1;
        }

        private void publishBtn_Click(object sender, RoutedEventArgs e)
        {
            MQTT_Client client = new MQTT_Client();

            var publishTopic = LayoutRoot.FindName("publishTopic") as TextBox;
            var message = LayoutRoot.FindName("message") as TextBox;

            switch(qosLevels.SelectedItem as string)
            {
                case "AtLeastOnce":
                    client.Publish_AtLeastOnce(publishTopic.Text, message.Text);
                    break;
                case "AtMostOnce":
                    client.Publish_AtMostOnce(publishTopic.Text, message.Text);
                    break;
                case "ExactlyOnce":
                    client.Publish_ExactlyOnce(publishTopic.Text, message.Text);
                    break;
                default:
                    break;
            }
        }

        private void subscribeBtn_Click(object sender, RoutedEventArgs e)
        {
            var subscribeTopic = LayoutRoot.FindName("subscribeTopic") as TextBox;            
            MQTT_Client client = new MQTT_Client();
            client.Subscribe(subscribeTopic.Text);
            var subscribeList = LayoutRoot.FindName("subscribeListBox") as ListBox;
            subscribeList.Items.Add(subscribeTopic.Text);
        }

       
    }
}
