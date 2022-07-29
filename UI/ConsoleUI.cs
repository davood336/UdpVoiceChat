using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdpVoiceChat.Audio;
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

            AudioController audioController = new AudioController(udp);

            while(true)
            {
                udp.Send(Console.ReadLine());
            }
        }
    }
}
