using System.Linq;

namespace Client.Entities
{
    public class RecieveObject
    {
        public string Mensagem { get; set; }
        public string Ip { get; set; }

        public RecieveObject()
        {

        }

        public RecieveObject(string msg, string ip)
        {
            Mensagem = msg;
            Ip = ip;
        }

        public bool ValidaSePrecisaRetornarUmaMensagem()
        {
            var palavras = Mensagem.Split(' ');

            if(palavras.Count() > 2)
            {
                var reply = palavras[1];

                if (reply == "reply")
                    return false;
            }

            return true;
        }
    }
}
