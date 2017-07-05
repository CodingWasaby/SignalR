using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using SignalRChat_MI.Model;
using SignalRChat_MI.Interface;
using log4net;

namespace SignalRChat.Hubs
{
    public class MessageHub : Hub<IMessageClient>, IMessageServer
    {
        public void SendtoAll(MessageModel message)
        {
            Clients.All.SendMessage(message);
            if (message.IsLog)
                WriteLog(message.Message);
        }

        public void SendToOthers(MessageModel message)
        {
            Clients.AllExcept(Context.ConnectionId).SendMessage(message);
            if (message.IsLog)
                WriteLog(message.Message);
        }

        public void SendToOne(MessageModel message)
        {
            var UserList = GetUserListFromRedis();
            if (message.ReciveConnectionID == null)
            {
                message.ReciveConnectionID = UserList.First(m => m.OperatorId == message.ReciveID).ConnectionId;
            }
            Clients.Client(message.ReciveConnectionID).SendMessage(message);
            if (message.IsLog)
                WriteLog(message.Message);
        }

        public void GetUserList()
        {
            var UserList = GetUserListFromRedis();
            Clients.Client(Context.ConnectionId).SendUserList(UserList);
        }

        private List<UserModel> GetUserListFromRedis()
        {
            return CacheClass._UserList;
        }

        private void WriteLog(string message)
        {
            ILog m_log = LogManager.GetLogger(typeof(MessageHub));
            m_log.Info(message);
        }

        public void RegisterConnection(string operatorId, string userName)
        {
            lock (CacheClass.lockobj)
            {
                var UserList = GetUserListFromRedis();
                var user = UserList.FirstOrDefault(m => m.OperatorId == operatorId);
                var serverVars = Context.Request.GetHttpContext().Request.ServerVariables;
                var Ip = serverVars["REMOTE_ADDR"];
                if (user != null && Ip != user.IP)
                {
                    var message = new MessageModel();
                    message.OperationType = "DownLine";
                    Clients.Client(user.ConnectionId).SendMessage(message);
                }
                UserList.RemoveAll(n => n.OperatorId == operatorId || n.ConnectionId == Context.ConnectionId);
                UserList.Add(new UserModel
                {
                    UserName = userName,
                    OperatorId = operatorId,
                    ConnectionId = Context.ConnectionId,
                    IP = Ip
                });
                CacheClass._UserList = UserList;
                Clients.All.SendUserList(UserList);
                Clients.AllExcept(Context.ConnectionId).LoginTip(userName);
            }
        }
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var UserList = GetUserListFromRedis();
            UserList.RemoveAll(n => n.ConnectionId == Context.ConnectionId);
            CacheClass._UserList = UserList;
            Clients.All.SendUserList(UserList);
            return base.OnDisconnected(true);
        }
    }
}