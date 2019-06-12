using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1712169
{
    public partial class Form1 : Form
    {
        static ProxyServer server = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtInfo.Text = "--- Welcome to my PS app ! ---" + Environment.NewLine;
            txtInfo.Text += "--> Fill in parameters of server" + Environment.NewLine;
            txtInfo.Text += "--> Click start button to run" + Environment.NewLine;
            txtInfo.Text += Environment.NewLine + "  <3 Hope you enjoy my app <3";
        }
     
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtIP.Text == "")
            {
                MessageBox.Show("You have not already entered Listening IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPort.Text == "")
            {
                MessageBox.Show("You have not already entered Port", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Task t = new Task(new Action(() =>
            {                
                int port = int.Parse(txtPort.Text);
                string ip = txtIP.Text;
                int pendingConnectionLimit = 5;
                bool isStarted = false;

                txtInfo.Clear();
                txtInfo.Text = "Infomation of your server:" + Environment.NewLine;
                txtInfo.Text += "Server IP Address: 127.0.0.1" + Environment.NewLine;
                txtInfo.Text += "Listen IP Address: " + ip + Environment.NewLine;
                txtInfo.Text += "Port: " + port.ToString() + Environment.NewLine;
                txtInfo.Text += "Connection pendding limit: " + pendingConnectionLimit.ToString() + Environment.NewLine;
                txtInfo.Text += "Status: Running..." + Environment.NewLine;

                if (server == null)
                {
                    server = new ProxyServer(ip, port, pendingConnectionLimit, txtConsole);
                }
                else if (!isStarted && server != null)
                {
                    server.Setup(ip, port, pendingConnectionLimit);
                }

                server.StartServer();
                isStarted = true;

            }));

            t.Start();
            Task.WaitAll(t);
        }

        private void btnOpenBlackList_Click(object sender, EventArgs e)
        {
            FormBlackList frm = new FormBlackList();
            frm.Show();
        }

        public delegate void StopProgaram();

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.StopServer();
            if (txtInfo.Text.Contains("Status: Running..."))
            {
                txtInfo.Text = txtInfo.Text.Replace("Status: Running...", "Status: Stop!");
            }
        }
    }
}
