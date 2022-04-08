
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;



namespace M2MqttUnity.Examples
{
    
    public class M2MqttUnityTest : M2MqttUnityClient
    {
        public static string address;
        public static string username;
        public static string password;

        public InputField addressInputField;
        public InputField userInputField;
        public InputField pwdInputField;

        public Text connection_result;
        private List<string> eventMessages = new List<string>();
        

 
        public void UpdateBeforeConnect()
        {
            this.brokerAddress = addressInputField.text;
            M2MqttUnityTest.address = addressInputField.text;
            this.brokerPort = 1883;
            this.mqttUserName = userInputField.text;
            M2MqttUnityTest.username = userInputField.text;
            this.mqttPassword = pwdInputField.text;
            M2MqttUnityTest.password = pwdInputField.text;
            this.Connect();
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
            SceneManager.LoadScene("Scene 2");
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


        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            
        }
        
    }
}
