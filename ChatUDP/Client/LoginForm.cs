using Client.Entities;
using System;
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
        }

        private void BotaoEnviarMensagem_Click(object sender, EventArgs e)
        {
            var ip = editIP.Text;
            var porta = Convert.ToInt32(textBox3.Text);
            var msg = textBox1.Text;

            Entities.FormClient formClient = new Entities.FormClient(Udp, ip, porta, msg);

            formClient.EnviarMensagem();
        }

    }
}
