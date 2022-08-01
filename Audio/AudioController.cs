using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using UdpVoiceChat.udp;

namespace UdpVoiceChat.Audio
{
    internal class AudioController
    {
        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferStream;
        private UdpController udpController;
        private const int LocalPortVoices = 6666;

        public AudioController(UdpController udp)
        {
            udpController = udp;
            waveIn = new WaveInEvent();

            waveOut = new WaveOutEvent();

            waveIn.WaveFormat = new WaveFormat(44000, 16, 1);
            waveIn.DataAvailable += VoiceInput;

            bufferStream = new BufferedWaveProvider(new WaveFormat(44000, 16, 1));
            waveOut.Init(bufferStream);
            new Thread(new ThreadStart(ReceiveVoice)).Start();


            for (int deviceId = 0; deviceId < WaveIn.DeviceCount; deviceId++)
            {
                var deviceInfo = WaveIn.GetCapabilities(deviceId);
                Console.WriteLine(deviceId + ": " + deviceInfo.ProductName);
            }
            Console.WriteLine();
            for (int deviceId = 0; deviceId < WaveOut.DeviceCount; deviceId++)
            {
                var deviceInfo = WaveOut.GetCapabilities(deviceId);
                Console.WriteLine(deviceId + ": " + deviceInfo.ProductName);
            }
            Console.WriteLine();
            Console.Write("Введите номер микрофона, а затем номер динамиков через пробел, например, \"0 0\": ");
            var readDevices = Console.ReadLine();
            string[] splitDevices = readDevices.Split(" ");
            waveIn.DeviceNumber = Int32.Parse(splitDevices[0]);
            waveOut.DeviceNumber = Int32.Parse(splitDevices[1]);

            waveIn.StartRecording();
        }

        private void VoiceInput(object sender, WaveInEventArgs e)
        {
            try
            {
                udpController.Send(e.Buffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void ReceiveVoice()
        {
            UdpClient receiver = new UdpClient(LocalPortVoices);

            IPEndPoint remoteIp = null;

            waveOut.Play();

            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp);
                    bufferStream.AddSamples(data, 0, data.Length);
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
