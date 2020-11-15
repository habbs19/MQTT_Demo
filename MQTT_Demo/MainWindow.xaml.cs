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
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MQTT_Client client = new MQTT_Client();

        public MainWindow()
        {
            InitializeComponent();

            pubQOSLevels.ItemsSource = new List<string> { "At Most Once", "At Least Once", "Exactly Once" };
            pubQOSLevels.SelectedIndex = 1;

            subQOSLevels.ItemsSource = new List<string> { "At Most Once", "At Least Once", "Exactly Once" };
            subQOSLevels.SelectedIndex = 1;
        }

        private void publishBtn_Click(object sender, RoutedEventArgs e)
        {
            
            var publishTopic = LayoutRoot.FindName("publishTopic") as TextBox;
            var message = LayoutRoot.FindName("message") as TextBox;

            switch(pubQOSLevels.SelectedItem as string)
            {
                case "At Most Once":
                    client.Publish(publishTopic.Text, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, (bool)chkRetain.IsChecked, message.Text);
                    break;
                case "At Least Once":
                    client.Publish(publishTopic.Text, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, (bool)chkRetain.IsChecked, message.Text);
                    break;
                case "Exactly Once":
                    client.Publish(publishTopic.Text, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, (bool)chkRetain.IsChecked, message.Text);
                    break;
                default:
                    break;
            }
        }

        private void subscribeBtn_Click(object sender, RoutedEventArgs e)
        {
            var subscribeTopic = LayoutRoot.FindName("subscribeTopic") as TextBox;

            switch (subQOSLevels.SelectedItem as string)
            {
                case "At Most Once":
                    client.Subscribe(subscribeTopic.Text, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE);
                    break;
                case "At Least Once":
                    client.Subscribe(subscribeTopic.Text, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);
                    break;
                case "Exactly Once":
                    client.Subscribe(subscribeTopic.Text, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE);
                    break;
                default:
                    break;
            }

            var subscribeList = LayoutRoot.FindName("subscribeListBox") as ListBox;
            subscribeList.Items.Add(subscribeTopic.Text);
        }

        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (client.Connected == false) {
                client.Connect(txtbClientID.Text, txtbWillTopic.Text, txtbWillMsg.Text);
                lblConnection.Content = "Connection: Connected";
            }
            else
            {
                client.Disconnect();
                lblConnection.Content = "Connection: Disconnected";
                messages.Clear();
                subscribeListBox.Items.Clear();
            }
        }
    }
}
