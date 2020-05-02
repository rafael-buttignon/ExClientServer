using System.Net.Sockets;
using System.Text;
using System.Net;
using System;

namespace Client.Udp
{
    public static class UdpClient
    {
        public static Socket UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public static IPAddress UdpAddress = IPAddress.Parse("127.0.0.1");
        public static  IPEndPoint UdpPort = new IPEndPoint(UdpAddress, 11000);

        public static void SocketUdp(string args)
        {
            byte[] sendbuf = Encoding.ASCII.GetBytes(args);

            Console.WriteLine("Enviado Para: " + UdpPort.Address + UdpPort.Port);

            UdpSocket.SendTo(sendbuf, UdpPort);

            Console.WriteLine("Mensagem enviada para o Servidor. ");
        }
    }
}

