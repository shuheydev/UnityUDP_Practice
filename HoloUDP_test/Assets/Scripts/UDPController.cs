using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UDPController : MonoBehaviour
{
    public TextMeshPro Message;

    // Start is called before the first frame update
    void Start()
    {
        var udpReceiver = new UDPReceiver();
        udpReceiver.TestCallBack = TestMethod;

        Message.text = "hello,world";

        udpReceiver.Start();
    }

    private string receivedMessage;
    private void TestMethod(string message)
    {
        receivedMessage = message;
        Debug.Log(message);
    }

    // Update is called once per frame
    void Update()
    {
        Message.text = receivedMessage;
    }
}
