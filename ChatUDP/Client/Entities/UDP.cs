using Client.Entities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class UDP
    {
        private UdpClient ClientSend, ClientRecieve;

        public UDP()
        {
            ClientRecieve = new UdpClient(60000);
            ClientSend = new UdpClient();
        }

        public void Send(string text, string ip, int port)
        {
            ClientSend.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            byte[] data = Encoding.ASCII.GetBytes(text);
            ClientSend.Send(data, data.Length);
        }

        public RecieveObject Receive()
        {
            RecieveObject recieveObject = new RecieveObject();
            var RecieveEndPoint = new IPEndPoint(IPAddress.Any, 60000);
            var RecieveBuffer = ClientRecieve.Receive(ref RecieveEndPoint);
            var ip = RecieveEndPoint.Address.ToString();
            var porta = RecieveEndPoint.Port;
            var mensagemRecebida = Encoding.ASCII.GetString(RecieveBuffer);

            if (!string.IsNullOrEmpty(ip))
                return new RecieveObject(mensagemRecebida, ip, porta);

            return recieveObject;
        }
    }
}
