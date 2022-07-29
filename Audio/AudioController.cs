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
        private WaveIn waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider bufferStream;
        private Thread inThread;
        private UdpController udpController;
        private const int LocalPortVoices = 6666;

        public AudioController(UdpController udp)
        {
            waveIn = new WaveIn();
            waveOut = new WaveOut();

            waveIn.WaveFormat = new WaveFormat(8000, 16, 1);
            waveIn.DataAvailable += VoiceInput;

            bufferStream = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
            waveOut.Init(bufferStream);
            new Thread(new ThreadStart(ReceiveVoice)).Start();
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
