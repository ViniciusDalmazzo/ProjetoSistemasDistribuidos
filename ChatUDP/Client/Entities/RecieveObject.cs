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
            if (Mensagem.Contains("reply"))
                return false;

            return true;
        }
    }
}
