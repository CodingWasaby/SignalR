using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatTest
{
    public class MessageHandler
    {
        public IHubProxy HubProxy { get; set; }
        const string ServerUri = "http://localhost:9988";
        public HubConnection Connection { get; set; }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerUri);

            // 创建一个集线器代理对象
            HubProxy = Connection.CreateHubProxy("ChatHub");

            // 供服务端调用，将消息输出到消息列表框中
            //HubProxy.On<string, string>("AddMessage", (name, message) =>
            //     this.Dispatcher.Invoke(() =>
            //        RichTextBoxConsole.AppendText(String.Format("{0}: {1}\r", name, message))
            //    ));

            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                // 连接失败
                return;
            }
        }
    }
}
