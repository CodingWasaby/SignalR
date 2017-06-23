using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using SignalRChat.Models;

namespace SignalRChat.Hubs
{
    public class MessageHub : Hub
    {
        private List<UserModel> UserList = new List<UserModel>();

        public void SendtoAll(string senderName, string message)
        {
            Clients.All.SendMessage(senderName, message);
        }

        public void SendToOthers(string senderName, string message)
        {
            Clients.AllExcept(Context.ConnectionId).SendMessage(senderName, message);
        }

        public void SendToOne(string senderName, string message, string reciverConnectionId)
        {
            Clients.Client(reciverConnectionId).SendMessage(senderName, message, Context.ConnectionId);
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