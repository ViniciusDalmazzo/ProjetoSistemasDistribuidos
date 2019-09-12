using System.Linq;

namespace Client.Entities
{
    public class RecieveObject
    {
        public string Mensagem { get; set; }
        public string Ip { get; set; }

        public int Porta { get; set; }

        public RecieveObject()
        {

        }

        public RecieveObject(string msg, string ip, int porta)
        {
            Mensagem = msg;
            Ip = ip;
            Porta = porta;
        }

        public bool ValidaSePrecisaRetornarUmaMensagem()
        {
            if (Mensagem.Contains("reply"))
                return false;

            return true;
        }
    }
}
