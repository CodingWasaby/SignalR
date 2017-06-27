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
        private List<UserModel> UserList = new List<UserModel>();

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
            if (message.ReciveConnectionID == null)
            {
                message.ReciveConnectionID = UserList.First(m => m.OperatorId == message.ReciveID).ConnectionId;
            }
            Clients.Client(message.ReciveConnectionID).SendMessage(message);
        }

        public void GetUserList()
        {
            Clients.Client(Context.ConnectionId).SendUserList(UserList);
        }

        public void RegisterConnection(string operatorId, string userName)
        {
            UserList.RemoveAll(n => n.OperatorId == operatorId || n.ConnectionId == Context.ConnectionId);
            UserList.Add(new UserModel
            {
                UserName = userName,
                OperatorId = operatorId,
                ConnectionId = Context.ConnectionId
            });
            Clients.All.SendUserList(UserList);
            Clients.AllExcept(Context.ConnectionId).LoginTip(userName);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserList.RemoveAll(n => n.ConnectionId == Context.ConnectionId);
            Clients.All.SendUserList(UserList);
            return base.OnDisconnected(stopCalled);
        }
    }
}