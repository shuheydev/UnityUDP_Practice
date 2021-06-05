using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UdpController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var udpReceiver = new UdpReceiver();
        udpReceiver.TestCallBack = TestMethod;
        udpReceiver.Start("10.172.244.102", 10000);
    }

    private void TestMethod(string testMessage)
    {
        Debug.Log(testMessage);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
