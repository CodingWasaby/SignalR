using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using SignalRChat_MI.Model;
using SignalRChat_MI.Interface;

namespace SignalRChat.Hubs
{
    public class MessageHub : Hub<IMessageClient>, IMessageServer
    {
        public void SendtoAll(MessageModel message)
        {
            Clients.All.SendMessage(message);
        }

        public void SendToOthers(MessageModel message)
        {
            Clients.AllExcept(Context.ConnectionId).SendMessage(message);
        }

        public void SendToOne(MessageModel message)
        {
            var UserList = GetUserListFromRedis();
            if (message.ReciveConnectionID == null)
            {
                message.ReciveConnectionID = UserList.First(m => m.OperatorId == message.ReciveID).ConnectionId;
            }
            Clients.Client(message.ReciveConnectionID).SendMessage(message);
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

        public void RegisterConnection(string operatorId, string userName)
        {
            var UserList = GetUserListFromRedis();
            var user = UserList.FirstOrDefault(m => m.OperatorId == operatorId);
            if (user != null)
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
                ConnectionId = Context.ConnectionId
            });
            CacheClass._UserList = UserList;
            Clients.All.SendUserList(UserList);
            Clients.AllExcept(Context.ConnectionId).LoginTip(userName);
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