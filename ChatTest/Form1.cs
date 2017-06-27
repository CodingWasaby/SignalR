using SignalRChat_MI;
using SignalRChat_MI.Interface;
using SignalRChat_MI.Model;
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
    public partial class Form1 : Form, IMessageClient
    {
        private WebSocketFactory _Socket;
        public Form1()
        {
            InitializeComponent();
            alertControl1.AutoFormDelay = 3000;
            _Socket = new WebSocketFactory(this);
            _Socket.RegisterConnection("123", "ZX");
        }

        public void SendMessage(MessageModel message)
        {
            Action<MessageModel> act = x =>
            {
                this.richTextBox1.Text += x.SenderName + ":" + x.Message + "。\r\n";
            };
            this.Invoke(act, message);
        }

        public void SendUserList(List<UserModel> userList)
        {
            Action<List<UserModel>> act = x =>
            {
                gridControl1.DataSource = x;
            };
            this.Invoke(act, userList);
        }

        public void LoginTip(string userName)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var random = new Random();
            var m = new MessageModel();
            m.SenderName = "ZX";
            m.Message = "Hello" + random.Next(0, 1000);
            _Socket.SendtoAll(m);
        }
    }
}
