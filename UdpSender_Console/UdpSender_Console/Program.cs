using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpSender_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string remoteHost = "10.172.244.102";
            int remotePort = 10000;

            UdpClient udpClient = new UdpClient();

            for(; ; )
            {
                Console.WriteLine("input message: ");
                string sendMsg = Console.ReadLine();
                byte[] sendbytes = Encoding.UTF8.GetBytes(sendMsg);

                udpClient.Send(sendbytes, sendbytes.Length, remoteHost, remotePort);

                if (sendMsg.Equals("exit"))
                {
                    break;
                }
            }

            udpClient.Close();

            Console.WriteLine("終了しました");

        }
    }
}
