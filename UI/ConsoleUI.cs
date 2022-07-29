using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdpVoiceChat.udp;

namespace UdpVoiceChat.UI
{
    internal class ConsoleUI
    {
        public void Start()
        {
            Console.Write("Введите ip: ");
            String ip = Console.ReadLine();

            UdpController udp = new UdpController(ip);

            while(true)
            {
                udp.Send(Console.ReadLine());
            }
        }
    }
}
