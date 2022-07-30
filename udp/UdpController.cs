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
        private const int LocalPortMessages = 7777;
        private const int LocalPortVoices = 6666;

        private UdpClient client;
        private UdpClient voiceClient;

        public UdpController(String ip) 
        {
            client = new UdpClient();

            client.Connect(IPAddress.Parse(ip), LocalPortMessages);

            voiceClient = new UdpClient();

            voiceClient.Connect(IPAddress.Parse(ip), LocalPortVoices);

            new Thread(new ThreadStart(ReceiveMessage)).Start();
        }

        public void Send(String message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data);
        }

        public void Send(byte[] voice)
        {
            voiceClient.Send(voice);
        }

        private void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(LocalPortMessages);

            IPEndPoint remoteIp = null;

            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp);
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
