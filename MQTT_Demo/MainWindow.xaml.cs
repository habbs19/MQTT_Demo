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
using System.Diagnostics;

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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }

        private void publishBtn_Click(object sender, RoutedEventArgs e)
        {
            
            var publishTopic = LayoutRoot.FindName("publishTopic") as TextBox;
            var message = LayoutRoot.FindName("message") as TextBox;
            if (string.IsNullOrWhiteSpace(publishTopic.Text))
            {
                MessageBox.Show("Specify a topic");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(message.Text))
                {
                    message.Text = "";
                }
                switch (pubQOSLevels.SelectedItem as string)
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
        }

        private void subscribeBtn_Click(object sender, RoutedEventArgs e)
        {
            var subscribeTopic = LayoutRoot.FindName("subscribeTopic") as TextBox;
            if (string.IsNullOrWhiteSpace(subscribeTopic.Text))
            {
                MessageBox.Show("Specify a topic");
            }
            else
            {
                try
                {
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                var subscribeList = LayoutRoot.FindName("subscribeListBox") as ListBox;
                subscribeList.Items.Add(subscribeTopic.Text);
            }
        }

        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (client.Connected == false) {
                if (string.IsNullOrEmpty(txtbClientID.Text) == false)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(txtbWillTopic.Text) && !string.IsNullOrWhiteSpace(txtbWillMsg.Text))
                        {
                            client.Connect(txtbClientID.Text, txtbWillTopic.Text, txtbWillMsg.Text);
                            lblConnection.Content = "Connection: Connected";
                        }
                        else
                        {
                            client.Connect(txtbClientID.Text, null, null);
                            lblConnection.Content = "Connection: Connected";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Specify a client ID");
                }
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
