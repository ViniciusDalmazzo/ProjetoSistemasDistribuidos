using System;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class LoginForm : Form
    {
        private readonly UDP Udp;

        public LoginForm()
        {
            InitializeComponent();
            Udp = new UDP();
            InitThread();
        }

        private void BotaoEnviarMensagem_Click(object sender, EventArgs e)
        {
            var ip = editIP.Text;
            var porta = Convert.ToInt32(textBox3.Text);
            var msg = textBox1.Text;

            EnviarMensagem(msg, ip, porta);
        }

        public void EnviarMensagem(string msg, string ip, int porta)
        {
            Udp.Send(msg, ip, porta);
        }

        delegate void teste(string texto);
        public void EscreverMensagemNaTela(string msg)
        {
            textBox2.Text += msg;
        }

        public void InitThread()
        {
            var listen_thread = new Thread(new ThreadStart(Receive));
            listen_thread.IsBackground = true;
            listen_thread.Start();
        }

        public void Receive()
        {
            while (true)
            {
                var msg = Udp.Receive();

                if(!string.IsNullOrEmpty(msg))
                    this.Invoke(new teste(EscreverMensagemNaTela), msg);
            }
        }
    }
}
