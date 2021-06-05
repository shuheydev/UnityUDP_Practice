using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class UdpReceiver : IDisposable
{
    UdpClient mudp;

    //ソケット通信に何か問題が発生したときのコールバック
    public Action<SocketException> SocketExceptionCallBack;
    //UdpClientがDisposeされているときのコールバック
    public Action<ObjectDisposedException> ObjectDisposedExceptionCallBack;

    public Action<string> TestCallBack;


    public void Start(string localIpString, int localPort)
    {
        IPAddress localAddress = IPAddress.Parse(localIpString);
        IPEndPoint localEP = new IPEndPoint(localAddress, localPort);
        mudp = new UdpClient(localEP);
        mudp.BeginReceive(UDPReceive, mudp);

        Debug.Log("Start listening");
    }

    private void UDPReceive(IAsyncResult res)
    {
        UdpClient getUdp = (UdpClient)res.AsyncState;
        IPEndPoint iPEnd = null;

        try
        {
            byte[] getByte = getUdp.EndReceive(res, ref iPEnd);

            string text = Encoding.ASCII.GetString(getByte);

            //ここに受け取ったデータ(JSONなど)をパースする処理を書く

            TestCallBack(text);
        }
        catch (SocketException ex)
        {
            SocketExceptionCallBack(ex);
            return;
        }
        catch (ObjectDisposedException ex)
        {
            ObjectDisposedExceptionCallBack(ex);
            return;
        }

        getUdp.BeginReceive(UDPReceive, getUdp);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
