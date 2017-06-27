using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using SignalRChat_MI.Interface;
using SignalRChat_MI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat_MI
{
    public class WebSocketFactory : IMessageServer
    {
        private IHubProxy HubProxy_Message { get; set; }
        private IHubProxy HubProxy_Chat { get; set; }
        private HubConnection Connection { get; set; }

        public WebSocketFactory(IMessageClient client = null)
        {
            var ServerUri = Convert.ToString(ConfigurationManager.AppSettings["WebSocketUri"]);
            Connection = new HubConnection(ServerUri);
            HubProxy_Message = Connection.CreateHubProxy("MessageHub");
            if (client != null)
                RegisteMessageClientHandler(client);
            try
            {
                Connection.Start().Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RegisteMessageClientHandler(IMessageClient clien)
        {
            HubProxy_Message.On<MessageModel>("SendMessage", x =>
            {
                clien.SendMessage(x);
            });
            HubProxy_Message.On<List<UserModel>>("SendUserList", x =>
            {
                clien.SendUserList(x);
            });
            HubProxy_Message.On<string>("LoginTip", x =>
            {
                clien.LoginTip(x);
            });
        }

        public void SendtoAll(MessageModel message)
        {
            HubProxy_Message.Invoke("SendtoAll", message);
        }

        public void SendToOthers(MessageModel message)
        {
            HubProxy_Message.Invoke("SendToOthers", message);
        }

        public void SendToOne(MessageModel message)
        {
            HubProxy_Message.Invoke("SendToOne", message);
        }

        public void GetUserList()
        {
            HubProxy_Message.Invoke("GetUserList");
        }

        public void RegisterConnection(string operatorId, string userName)
        {
            HubProxy_Message.Invoke("RegisterConnection", operatorId, userName);
        }

    }
}
