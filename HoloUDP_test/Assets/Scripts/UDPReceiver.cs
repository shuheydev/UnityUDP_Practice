using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPReceiver :IDisposable
{
    IPAddress localAddress = IPAddress.Any;
    int localPort = 2002;

    UdpClient mUdp;

    public Action<string> TestCallBack;

    public void Start()
    {
        localAddress = IPAddress.Any;//IPAddress.Parse("192.168.11.61");
        IPEndPoint localEP = new IPEndPoint(localAddress, localPort);
        mUdp = new UdpClient(localEP);
        mUdp.BeginReceive(UDPReceive, mUdp);
        Debug.Log("Start listening");
    }

    private void UDPReceive(IAsyncResult res)
    {
        UdpClient getUdp = (UdpClient)res.AsyncState;
        IPEndPoint ipEnd = null;

        try
        {
            byte[] getByte = getUdp.EndReceive(res, ref ipEnd);

            string text = Encoding.UTF8.GetString(getByte);

            TestCallBack(text);
        }
        catch (SocketException ex)
        {
            return;
        }
        catch(ObjectDisposedException ex)
        {
            return;
        }

        getUdp.BeginReceive(UDPReceive,getUdp);
    }

    public void Dispose()
    {
        mUdp.Dispose();
    }
}
