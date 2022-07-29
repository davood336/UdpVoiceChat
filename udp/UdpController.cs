using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpVoiceChat.udp
{
    internal class UdpController
    {
        private const int LocalPort = 7777;

        private UdpClient client;

        public UdpController(String ip) 
        {
            client = new UdpClient();

            String[] ip_port = ip.Split(':');
            client.Connect(IPAddress.Parse(ip_port[0]), Int32.Parse(ip_port[1]));

            new Thread(new ThreadStart(ReceiveMessage)).Start();
        }

        public void Send(String message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data);
        }

        private void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(LocalPort);

            IPEndPoint remoteIp = null;

            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.UTF8.GetString(data);
                    Console.WriteLine("Собеседник: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
