using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;


namespace M2MqttUnity.Examples
{
    public class Status_Data
    {
        public string temperature { get; set; }
        public string humidity { get; set; }
        public string gas { get; set; }
    }
    public class M2MqttUnityTest1 : M2MqttUnityClient
    {
        public Image Led_On;
        public Image Led_Off;
        public Image Pump_On;
        public Image Pump_Off;

        public string Topic_Status;
        public string Topic_Led;
        public string Topic_Pump;

        
        public Text humidity_advanced;
        public Slider humidity_slider;
        public Text temperature_advanced;
        public Slider temperature_slider;
 
        public string msg_received_from_topic = "";
        public Text text_display;
        public Text connection_result;
        private List<string> eventMessages = new List<string>();

        public Status_Data _status_data;

        public void TurnOffLed()
        {
            Led_Off.gameObject.SetActive(true);
            Led_On.gameObject.SetActive(false);
            client.Publish(Topic_Led, System.Text.Encoding.UTF8.GetBytes("{\"device\":\"LED\",\"status\":\"OFF\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void TurnOnLed()
        {
            Led_Off.gameObject.SetActive(false);
            Led_On.gameObject.SetActive(true);
            client.Publish(Topic_Led, System.Text.Encoding.UTF8.GetBytes("{\"device\":\"LED\",\"status\":\"ON\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void TurnOffPump()
        {
            Pump_Off.gameObject.SetActive(true);
            Pump_On.gameObject.SetActive(false);
            client.Publish(Topic_Pump, System.Text.Encoding.UTF8.GetBytes("{\"device\":\"PUMP\",\"status\":\"OFF\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void TurnOnPump()
        {
            Pump_Off.gameObject.SetActive(false);
            Pump_On.gameObject.SetActive(true);
            client.Publish(Topic_Pump, System.Text.Encoding.UTF8.GetBytes("{\"device\":\"PUMP\",\"status\":\"ON\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Start()
        {
            UpdateBeforeConnect();
        }

        public void UpdateBeforeConnect()
        {
            this.brokerAddress = M2MqttUnityTest.address;
            this.brokerPort = 1883;
            this.mqttUserName = M2MqttUnityTest.username;
            this.mqttPassword = M2MqttUnityTest.password;
            this.Topic_Status = "/bkiot/1914845/status";
            this.Topic_Led = "/bkiot/1914845/led";
            this.Topic_Pump = "/bkiot/1914845/pump";
            this.Connect();
        }
        public void TestPublish()
        {
            //client.Publish(Topic_Led, System.Text.Encoding.UTF8.GetBytes("{\"device\":\"LED\",\"status\":\"ON\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            //client.Publish(Topic_Pump, System.Text.Encoding.UTF8.GetBytes("{\"device\":\"PUMP\",\"status\":\"ON\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            //Debug.Log("Test message published");
            
        }

        public void PublishStatus()
        {
            System.Random temp = new System.Random();
            System.Random humid= new System.Random();
            System.Random gas = new System.Random();
            int temperature = temp.Next(10, 40);
            int humidity = humid.Next(15, 100);
            int gasThreshold = gas.Next(0, 90);
            client.Publish(Topic_Status, System.Text.Encoding.UTF8.GetBytes("{\"temperature\":" + temperature.ToString() + ",\"humidity\":" + humidity.ToString() + ",\"gas\":" + gasThreshold.ToString() +  "}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Status published");
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        protected override void OnConnecting()
        {
            base.OnConnecting();
           
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            PublishStatus();
            
        }

        protected override void SubscribeTopics()
        {
            if (Topic_Status != "")
            {
                client.Subscribe(new string[] { Topic_Status }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                Debug.Log("Subscribed to topic: " + Topic_Status);
            }
            if (Topic_Led != "")
            {
                client.Subscribe(new string[] { Topic_Led }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                Debug.Log("Subscribed to topic: " + Topic_Led);
            }
            if (Topic_Pump != "")
            {
                client.Subscribe(new string[] { Topic_Pump }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                Debug.Log("Subscribed to topic: " + Topic_Pump);
            }
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { Topic_Status });
            client.Unsubscribe(new string[] { Topic_Led });
            client.Unsubscribe(new string[] { Topic_Pump });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            connection_result.text = "CONNECTION FAILED! " + errorMessage;
        }

        protected override void OnDisconnected()
        {
            
        }

        protected override void OnConnectionLost()
        {
            
        }



        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            //msg_received_from_topic = msg;
            //text_display.text = msg;
            
            if (topic == Topic_Status)
            {
                _status_data = JsonConvert.DeserializeObject<Status_Data>(msg);
                Debug.Log("Received: " + msg);
                
                humidity_advanced.text = _status_data.humidity + "%";
                humidity_slider.value = (float.Parse(_status_data.humidity)) / 100;
                temperature_advanced.text = _status_data.temperature + "°C";
                temperature_slider.value = (float.Parse(_status_data.temperature)) / 40;

                Neddle.ShowGas(float.Parse(_status_data.gas), 0, 90);
            }
            
        }
        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            
        }
        
    }
}
