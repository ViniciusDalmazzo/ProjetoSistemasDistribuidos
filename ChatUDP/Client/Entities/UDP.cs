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

        public UDP()
        {
            client_receive = new UdpClient(6969);
        }

        public void Send(string text, string ip, int port)
        {
            client_send.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            byte[] data = Encoding.ASCII.GetBytes(text);
            client_send.Send(data, data.Length);
        }

        public string Receive()
        {
            var recv_end = new IPEndPoint(IPAddress.Any, 6969);
            var rec_buff = client_receive.Receive(ref recv_end);
            var msg = recv_end.ToString();

            if (!string.IsNullOrEmpty(msg))
                return msg + ": " + Encoding.ASCII.GetString(rec_buff) + Environment.NewLine;

            return string.Empty;
        }
    }
}
