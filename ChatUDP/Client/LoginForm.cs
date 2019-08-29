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

            this.listView1.Items.Clear();
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Name");
            this.listView1.Columns[0].Width = this.listView1.Width -
            4;
            this.listView1.HeaderStyle = ColumnHeaderStyle.None;
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
            if (!string.IsNullOrEmpty(msg))
            {
                Udp.Send(msg, ip, porta);
                EscreverMensagemNaTela($"Eu: {msg}");

                textBox1.Text = string.Empty;
            }
        }

        delegate void EscreveMensagem(string texto);
        public void EscreverMensagemNaTela(string msg)
        {
            listView1.Items.Add(new ListViewItem(msg));
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

                if (!string.IsNullOrEmpty(msg))
                    this.Invoke(new EscreveMensagem(EscreverMensagemNaTela), msg);
            }
        }
    }
}
