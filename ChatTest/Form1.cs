using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            alertControl1.AutoFormDelay = 3000;
            ConnectAsync();
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            HubProxy.Invoke("SendtoAll", "a", "aaa");
        }

        public IHubProxy HubProxy { get; set; }
        const string ServerUri = "http://localhost:9988";
        public HubConnection Connection { get; set; }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerUri);
            HubProxy = Connection.CreateHubProxy("MessageHub");


            Action<List<UserModel>> act = list =>
            {
                foreach (var n in list)
                {
                    var lines = richTextBox1.Lines.ToList();
                    lines.Add(n.ConnectionId + "-" + n.OperatorId + "\r\n");
                    richTextBox1.Lines = lines.ToArray();
                }
            };

            Action<string, string> act1 = (x, y) =>
             {
                 var lines = richTextBox1.Lines.ToList();
                 lines.Add(x + ":::" + y + "\r\n");
                 richTextBox1.Lines = lines.ToArray();
                 alertControl1.Show(this, x, y);
             };
            Action<string> act2 = x =>
             {
                 alertControl1.Show(this, "登录提示", x + "登陆了");
             };

            HubProxy.On<List<UserModel>>("SendUserList", (list) =>
            {
                this.Invoke(act, list);
            });

            HubProxy.On<string, string>("SendMessage", (x, y) =>
             {
                 this.Invoke(act1, x, y);
             });
            HubProxy.On<string>("LoginTip", x =>
             {
                 this.Invoke(act2, x);
             });

            try
            {
                await Connection.Start();
                HubProxy.Invoke("RegisterConnection", "a", "a");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

    }
}
