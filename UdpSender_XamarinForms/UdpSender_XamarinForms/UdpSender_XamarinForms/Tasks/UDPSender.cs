using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace UdpSender_XamarinForms.Tasks
{
    public class UDPSender:IDisposable
    {
        readonly UdpClient udpClient;
        private readonly string remoteIPAddress;
        private readonly int remotePort;

        public UDPSender(string remoteIPAddress,int remotePort)
        {
            udpClient = new UdpClient();
            this.remoteIPAddress = remoteIPAddress;
            this.remotePort = remotePort;
        }

        public void Send(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            udpClient.Send(messageBytes, messageBytes.Length, remoteIPAddress, remotePort);
        }

        public void Dispose()
        {
            udpClient.Dispose();
        }
    }
}
