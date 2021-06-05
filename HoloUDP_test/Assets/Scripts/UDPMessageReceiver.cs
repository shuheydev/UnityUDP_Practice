using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;


//参照:https://bluebirdofoz.hatenablog.com/entry/2019/03/11/190108

[System.Serializable]
public class MyIntEvent : UnityEvent<byte[]>
{

}

public class UDPMessageReceiver : MonoBehaviour
{
    [SerializeField, Tooltip("UDPメッセージ受信時実行処理")]
    private MyIntEvent UDPReceiveEventUnityEvent;

    [SerializeField, Tooltip("UDP受信ポート")]
    private int UDPReceivePort = 4602;

    private byte[] _udpReceivedData;

    private bool _udpReceivedFlag;

    private void Start()
    {
        _udpReceivedFlag = false;

        UDPClientReceiver_Init();
    }

    private void Update()
    {
        if (_udpReceivedFlag)
        {
            UDPReceiveEventUnityEvent.Invoke(_udpReceivedData);

            _udpReceivedFlag = false;
        }
    }

    private void UDPReceiveEvent(byte[] receiveData)
    {
        _udpReceivedFlag = true;

        _udpReceivedData = receiveData;
    }

    private void UDPClientReceiver_Init()
    {
        System.Net.IPEndPoint endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, UDPReceivePort);

        System.Net.Sockets.UdpClient udpClient = new System.Net.Sockets.UdpClient(endPoint);

        udpClient.BeginReceive(OnReceived, udpClient);

        Debug.Log("test::start listening");
    }

    private void OnReceived(System.IAsyncResult result)
    {
        System.Net.Sockets.UdpClient udpClient = (System.Net.Sockets.UdpClient)result.AsyncState;

        System.Net.IPEndPoint endPoint = null;
        byte[] receiveBytes = udpClient.EndReceive(result, ref endPoint);

        UDPReceiveEvent(receiveBytes);

        udpClient.BeginReceive(OnReceived, udpClient);
    }
}