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
    /// <summary>
    /// Socket 客户端
    /// </summary>
    public class FMWebSocket : IMessageServer
    {
        private IHubProxy HubProxy_Message { get; set; }
        private IHubProxy HubProxy_Chat { get; set; }
        public HubConnection Connection { get; set; }
        private IMessageClient _MessageClient { get; set; }

        /// <summary>
        /// Socket 客户端
        /// </summary>
        /// <param name="client"></param>
        public FMWebSocket(IMessageClient client = null)
        {
            _MessageClient = client;
            ReConnected();
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

        /// <summary>
        /// SendtoAll
        /// </summary>
        /// <param name="message"></param>
        public void SendtoAll(MessageModel message)
        {
            try
            {
                HubProxy_Message.Invoke("SendtoAll", message);
            }
            catch
            {
                ReConnected();
                HubProxy_Message.Invoke("SendtoAll", message);
            }
        }
        /// <summary>
        /// SendToOthers
        /// </summary>
        /// <param name="message"></param>
        public void SendToOthers(MessageModel message)
        {
            try
            {
                HubProxy_Message.Invoke("SendToOthers", message);
            }
            catch
            {
                ReConnected();
                HubProxy_Message.Invoke("SendToOthers", message);
            }
        }
        /// <summary>
        /// SendToOne
        /// </summary>
        /// <param name="message"></param>
        public void SendToOne(MessageModel message)
        {
            try
            {
                HubProxy_Message.Invoke("SendToOne", message);
            }
            catch
            {
                ReConnected();
                HubProxy_Message.Invoke("SendToOne", message);
            }
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        public void GetUserList()
        {
            try
            {
                HubProxy_Message.Invoke("GetUserList");
            }
            catch
            {
                ReConnected();
                HubProxy_Message.Invoke("GetUserList");
            }
        }
        /// <summary>
        /// 注册链接
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="userName"></param>
        public void RegisterConnection(string operatorId, string userName)
        {
            try
            {
                HubProxy_Message.Invoke("RegisterConnection", operatorId, userName);
            }
            catch
            {
                ReConnected();
                HubProxy_Message.Invoke("RegisterConnection", operatorId, userName);
            }
        }

        private void ReConnected()
        {
            var ServerUri = Convert.ToString(ConfigurationManager.AppSettings["WebSocketUri"]);
            Connection = new HubConnection(ServerUri);
            HubProxy_Message = Connection.CreateHubProxy("MessageHub");
            if (_MessageClient != null)
                RegisteMessageClientHandler(_MessageClient);
            try
            {
                Connection.Start().Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
