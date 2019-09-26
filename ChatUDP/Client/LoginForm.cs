using Client.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class LoginForm : Form
    {
        private UDP Udp;
        private List<Configuracao> Configuracoes = new List<Configuracao>();
        public DateTime UltimoReplyRecebidoDoLider { get; set; }
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

            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        public void InicializarInformacoesHeartbeat()
        {
            dataGridView1.Rows.Add();

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "172.18.0.9";
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = 1;

            dataGridView1.Rows.Add();

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "172.18.0.31";
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = 3;

            dataGridView1.Rows.Add();

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "172.18.0.32";
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = 2;

            dataGridView1.Rows.Add();

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "172.18.0.23";
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = 0;

            dataGridView1.Rows.Add();

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "172.18.0.24";
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = 4;

            dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.IndianRed;
            dataGridView1.ClearSelection();

            //listView3.Items.Add("172.18.0.32");
            //listView3.Items.Add("172.18.0.29");
            //listView3.Items.Add("172.18.0.30");
            //listView3.Items.Add("172.18.0.23");
            //listView3.Items.Add("172.18.0.21");
            //listView3.Items.Add("172.18.0.20");
            //listView3.Items.Add("172.18.0.19");
            //listView3.Items.Add("172.18.0.18");
            //listView3.Items.Add("172.18.0.17");
            //listView3.Items.Add("172.18.0.15");
            //listView3.Items.Add("172.18.0.14");
            //listView3.Items.Add("172.18.0.13");
            //listView3.Items.Add("172.18.0.12");
            //listView3.Items.Add("172.18.0.11");
            //listView3.Items.Add("172.18.0.10");
            //listView3.Items.Add("172.18.0.9");
            //listView3.Items.Add("172.18.0.8");
            //listView3.Items.Add("172.18.0.7");
            //listView3.Items.Add("172.18.0.6");
            //listView3.Items.Add("172.18.0.5");
            //listView3.Items.Add("172.18.0.4");
            //listView3.Items.Add("172.18.0.3");
            //listView3.Items.Add("172.18.0.2");
            //listView3.Items.Add("172.18.0.1");
            //listView3.Items.Add("172.18.3.71");

            RecuperaIPSConfigurados();
            RecuperaPortaConfigurada();
            RecuperaTempoDeAtualizacao();
        }

        delegate void EscreveMensagemRecebe(RecieveObject recieveObject);
        public void EscreverMensagemRecebe(RecieveObject recieveObject)
        {

            listView2.Items.Add(new ListViewItem(recieveObject.Mensagem));
        }

        delegate void EscreveMensagemEnvio(string texto);
        public void EscreverMensagemEnvio(string msg)
        {
            listView1.Items.Add(new ListViewItem(msg));
        }

        delegate void PintaLinhaDoLider(Configuracao lider);
        public void PintarLinhaDoLider(Configuracao lider)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if(dataGridView1.Rows[i].Cells[0].Value.ToString() == lider.IP)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                    break;
                }
            }
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
                    recieveObject.Mensagem = $"Hearbeat request recebido {recieveObject.Ip}";
                    Invoke(new EscreveMensagemRecebe(EscreverMensagemRecebe), recieveObject);
                    Udp.Send($"{MensagemPadraoReceber}", recieveObject.Ip, Porta);
                    Invoke(new EscreveMensagemEnvio(EscreverMensagemEnvio), $"Hearbeat reply enviado {recieveObject.Ip}");
                }
                else
                {
                    recieveObject.Mensagem = $"Heartbeat reply recebido {recieveObject.Ip}";
                    Invoke(new EscreveMensagemRecebe(EscreverMensagemRecebe), recieveObject);

                    var lider = Configuracoes.FirstOrDefault(x => x.EhLider);
                    var recieveUser = Configuracoes.FirstOrDefault(x => x.IP == recieveObject.Ip);

                    if (lider == null)
                    {
                        recieveUser.EhLider = true;
                        Invoke(new PintaLinhaDoLider(PintarLinhaDoLider), recieveUser);
                        UltimoReplyRecebidoDoLider = DateTime.Now;
                        break;
                    }

                    if (recieveUser.Peso < lider.Peso)
                    {
                        lider.EhLider = false;
                        recieveUser.EhLider = true;
                        Invoke(new PintaLinhaDoLider(PintarLinhaDoLider), recieveUser);
                        UltimoReplyRecebidoDoLider = DateTime.Now;
                        break;
                    }

                    var diferencaTempo = DateTime.Now - UltimoReplyRecebidoDoLider;

                    if (diferencaTempo.TotalSeconds > 1)
                        lider.EhLider = false;

                }
            }
        }

        public void Send()
        {
            while (true)
            {
                foreach (var config in Configuracoes)
                {
                    Udp.Send(MensagemPadraoEnvio, config.IP, Porta);
                    Invoke(new EscreveMensagemEnvio(EscreverMensagemEnvio), $"Heartbeat request enviado {config.IP}");
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
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var ip = dataGridView1.Rows[i].Cells[0].Value.ToString();
                var peso = dataGridView1.Rows[i].Cells[1].Value.ToString();

                Configuracoes.Add(new Configuracao(ip, peso));
            }
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
            var peso = textBox4.Text;

            dataGridView1.Rows.Add();

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = ip;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = peso;

            textBox1.Text = string.Empty;
            textBox4.Text = string.Empty;
        }

        private void AlteraStatusDasConfiguracoes(bool ativo)
        {
            groupBox3.Enabled = ativo;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }
    }
}
