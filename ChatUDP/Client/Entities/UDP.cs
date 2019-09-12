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
            ClientRecieve = new UdpClient(6969);
            ClientSend = new UdpClient(6968);
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
            var RecieveEndPoint = new IPEndPoint(IPAddress.Any, 6969);
            var RecieveBuffer = ClientRecieve.Receive(ref RecieveEndPoint);
            var ip = RecieveEndPoint.Address.ToString();
            var mensagemRecebida = Encoding.ASCII.GetString(RecieveBuffer);

            if (!string.IsNullOrEmpty(ip))
                return new RecieveObject(mensagemRecebida, ip);

            return recieveObject;
        }
    }
}
