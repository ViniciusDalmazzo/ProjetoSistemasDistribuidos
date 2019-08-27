using Client.Entities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    public class UDP
    {
        public UdpClient client_send = new UdpClient(6968), client_receive;
        Thread listen_thread;
        public FormClient FormClient;

        public UDP()
        {
            client_receive = new UdpClient(6969);
            listen_thread = new Thread(new ThreadStart(Receive));
            listen_thread.IsBackground = true;
            listen_thread.Start();
        }

        public void Send(string text, string ip, int port)
        {
            client_send.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            byte[] data = Encoding.ASCII.GetBytes(text);
            client_send.Send(data, data.Length);
        }

        private void Receive()
        {
            byte[] rec_buff;
            IPEndPoint recv_end;

            while (true)
            {
                recv_end = new IPEndPoint(IPAddress.Any, 6969);
                rec_buff = client_receive.Receive(ref recv_end);

                if (FormClient != null)
                    FormClient.EscreverMensagemNaTela(recv_end.ToString() + ": " + Encoding.ASCII.GetString(rec_buff) + Environment.NewLine);
            }
        }
    }
}
