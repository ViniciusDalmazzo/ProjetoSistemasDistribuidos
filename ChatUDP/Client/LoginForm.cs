using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class LoginForm : Form
    {
        private UDP Udp;
        private List<string> listaIps = new List<string>();
        private const string MensagemPadraoEnvio = "Heartbeat request";
        private const string MensagemPadraoReceber = "Heartbeat reply";
        private int Porta;
        private int TempoAtualizacao;

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

            listView3.Items.Clear();
            listView3.View = View.Details;
            listView3.Columns.Add("Name");
            listView3.Columns[0].Width = this.listView2.Width - 4;
            listView3.HeaderStyle = ColumnHeaderStyle.None;

            InicializarInformacoesHeartbeat();
        }

        public void InicializarInformacoesHeartbeat()
        {
            listView3.Items.Add("172.18.0.32");

            RecuperaIPSConfigurados();
            RecuperaPortaConfigurada();
            RecuperaTempoDeAtualizacao();
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
                var recieveObject = Udp.Receive();

                if (!string.IsNullOrEmpty(recieveObject.Ip) && recieveObject.ValidaSePrecisaRetornarUmaMensagem())
                {
                    Udp.Send($"{MensagemPadraoEnvio}", recieveObject.Ip, Porta);
                    Invoke(new EscreveMensagemRecebe(EscreverMensagemRecebe), $"{MensagemPadraoReceber} {recieveObject.Ip}");
                }
            }
        }

        public void Send()
        {
            while (true)
            {
                foreach (var ip in listaIps)
                {
                    Udp.Send(MensagemPadraoEnvio, ip, Porta);

                    Invoke(new EscreveMensagemEnvio(EscreverMensagemEnvio), $"{MensagemPadraoEnvio} {ip}");
                }

                Thread.Sleep(TempoAtualizacao);
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Udp = new UDP();
            AlteraStatusDasConfiguracoes(false);
            InicializarInformacoesHeartbeat();
            InitThread();            
        }

        public void RecuperaIPSConfigurados()
        {
            listaIps = listView3.Items.Cast<ListViewItem>()
                                   .Select(item => item.Text)
                                   .ToList();
        }

        public void RecuperaPortaConfigurada()
        {
            var porta = textBox2.Text;

            Porta = Convert.ToInt32(porta);
        }

        public void RecuperaTempoDeAtualizacao()
        {
            var tempo = textBox3.Text;

            TempoAtualizacao = Convert.ToInt32(tempo) * 1000;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ip = textBox1.Text;

            listView3.Items.Add(ip);

            textBox1.Text = string.Empty;
        }

        private void AlteraStatusDasConfiguracoes(bool ativo)
        {
            groupBox3.Enabled = ativo;
        }

    }
}
