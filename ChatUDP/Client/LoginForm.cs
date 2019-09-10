using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class LoginForm : Form
    {
        private UDP Udp;
        private List<string> listaIps = new List<string>();
        private const string MensagemPadraoEnvio = "Heartbeat";
        private const string MensagemPadraoReceber = "Heartbeat recebido: ";

        public LoginForm()
        {
            InitializeComponent();

            listView1.Items.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("Name");
            listView1.Columns[0].Width = this.listView1.Width - 4;
            listView1.HeaderStyle = ColumnHeaderStyle.None;

            listView2.Items.Clear();
            listView2.View = View.Details;
            listView2.Columns.Add("Name");
            listView2.Columns[0].Width = this.listView2.Width - 4;
            listView2.HeaderStyle = ColumnHeaderStyle.None;
                      
        }

        public void InicializarInformacoesHeartbeat()
        {
            listaIps.Add("192.168.1.1");
            listaIps.Add("192.168.1.2");
            listaIps.Add("192.168.1.3");
            listaIps.Add("192.168.1.4");
            listaIps.Add("192.168.1.5");
            listaIps.Add("192.168.1.6");
        }

        delegate void EscreveMensagemRecebe(string texto);
        public void EscreverMensagemRecebe(string msg)
        {
            listView2.Items.Add(new ListViewItem(msg));
        }

        delegate void EscreveMensagemEnvio(string texto);
        public void EscreverMensagemEnvio(string msg)
        {
            listView1.Items.Add(new ListViewItem(msg));
        }

        public void InitThread()
        {
            var RecieveThread = new Thread(new ThreadStart(Send));
            RecieveThread.IsBackground = true;
            RecieveThread.Start();

            var SendThread = new Thread(new ThreadStart(Receive));
            SendThread.IsBackground = true;
            SendThread.Start();
        }

        public void Receive()
        {
            while (true)
            {
                var msg = Udp.Receive();

                if (!string.IsNullOrEmpty(msg))
                    Invoke(new EscreveMensagemRecebe(EscreverMensagemRecebe), $"{MensagemPadraoReceber} {msg}");

            }
        }

        public void Send()
        {
            while (true)
            {
                foreach (var ip in listaIps)
                {
                    Udp.Send(MensagemPadraoEnvio, ip, 6969);

                    Invoke(new EscreveMensagemEnvio(EscreverMensagemEnvio), $"Heartbeat enviado para: {ip}");
                }

                Thread.Sleep(1000);
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Udp = new UDP();
            InicializarInformacoesHeartbeat();
            InitThread();
        }
    }
}
