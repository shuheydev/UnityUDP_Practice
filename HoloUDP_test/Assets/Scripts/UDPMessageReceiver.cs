using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UnityEventを利用するため
using UnityEngine.Events;
//Streamを使うため
using System.IO;

#if WINDOWS_UWP
using Windows.Networking.Sockets;
#endif


//参照:https://bluebirdofoz.hatenablog.com/entry/2019/03/11/190108
//今回:https://bluebirdofoz.hatenablog.com/entry/2019/03/12/070618


// 引数にByte列を受け取る UnityEvent<T0> の継承クラスを作成する
// UDP受信したバイト列を引数として渡す
// Inspector ビューに表示させるため、Serializable を設定する
[System.Serializable]
public class MyIntEvent : UnityEvent<byte[]>
{

}

public class UDPMessageReceiver : MonoBehaviour
{
    /// <summary>
    /// UDPメッセージ受信時実行処理
    /// </summary>
    [SerializeField, Tooltip("UDPメッセージ受信時実行処理")]
    private MyIntEvent UDPReceiveEventUnityEvent;

    /// <summary>
    /// UDP受信ポート
    /// </summary>
    [SerializeField, Tooltip("UDP受信ポート")]
    private int UDPReceivePort = 4602;

    /// <summary>
    /// UDP受信データ
    /// </summary>
    private byte[] _udpReceivedData;

    /// <summary>
    /// UDP受信イベント検出フラグ
    /// </summary>
    private bool _udpReceivedFlag;

    /// <summary>
    /// 起動処理
    /// </summary>
    private void Start()
    {
        //検出フラグOff
        _udpReceivedFlag = false;

        //初期化処理
        UDPClientReceiver_Init();
    }

    /// <summary>
    /// 定期実行
    /// </summary>
    private void Update()
    {
        if (_udpReceivedFlag)
        {
            //UDP受信を検出すればUnityEventを実行
            //受信データを引数として渡す
            UDPReceiveEventUnityEvent.Invoke(_udpReceivedData);

            //検出フラグをOff
            _udpReceivedFlag = false;
        }
    }

    /// <summary>
    /// UDP受信時処理
    /// </summary>
    /// <param name="receiveData"></param>
    private void UDPReceiveEvent(byte[] receiveData)
    {
        //検出フラグOnに変更する
        //UnityEventの実行はMainThreadで行う←ここ大事
        _udpReceivedFlag = true;

        //受信データを記録する
        _udpReceivedData = receiveData;
    }



#if WINDOWS_UWP

    DatagramSocket _socket;

    object _lockObject = new object();

    const int MAX_BUFFER_SIZE = 1024;

    private async void UDPClientReceiver_Init()
    {
        try
        {
            _socket = new DatagramSocket();
            _socket.MessageReceived += OnMessage;
            _socket.BindServiceNameAsync(UDPReceivePort.ToString());

            Debug.Log("test::start listening");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    async void OnMessage(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
    {
        using (Stream stream = args.GetDataStream().AsStreamForRead())
        {
            byte[] receiveBytes = new byte[MAX_BUFFER_SIZE];
            await stream.ReadAsync(receiveBytes, 0, MAX_BUFFER_SIZE);
            lock(_lockObject)
            {
                UDPReceiveEvent(receiveBytes);
            }
        }
    }

#else

    /// <summary>
    /// UDP受信初期化
    /// </summary>
    private void UDPClientReceiver_Init()
    {
        //UDP受信ポートに受信するすべてのメッセージを取得する
        System.Net.IPEndPoint endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, UDPReceivePort);

        //UDPクライアントインスタンスを初期化
        System.Net.Sockets.UdpClient udpClient = new System.Net.Sockets.UdpClient(endPoint);

        //非同期のデータ受信を開始する
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

#endif


}