namespace Client.Entities
{
    public class FormClient
    {
        public string IP { get; set; }
        public string Mensagem { get; set; }
        public int Porta { get; set; }
        LoginForm Form { get; set; }
        UDP Udp { get; set; }

        public FormClient(UDP udp,string ip, int porta, string mensagem)
        {
            Udp = udp;
            IP = ip;
            Mensagem = mensagem;
            Porta = porta;
            Udp.FormClient = this;
        }

        public void EnviarMensagem()
        {
            Udp.Send(Mensagem, IP, Porta);
        }

        public void EscreverMensagemNaTela(string msg)
        {
            Form.textBox3.Text += msg;
        }

    }
}

