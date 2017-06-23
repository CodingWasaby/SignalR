using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System.Net.Http;

namespace Test
{
    public class Class1
    {
        public IHubProxy _HubProxy;
        const string ServerUri = "http://localhost:8888/signalr";
        public HubConnection Connection { get; set; }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerUri);
            Connection.Received += Connection_Received;

            // 创建一个集线器代理对象
            _HubProxy = Connection.CreateHubProxy("ChatHub");

            // 供服务端调用，将消息输出到消息列表框中
            _HubProxy.On<string, string>("AddMessage", (x, y) => { });  

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

        private void Connection_Received(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
