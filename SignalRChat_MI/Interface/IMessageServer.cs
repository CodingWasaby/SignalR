using SignalRChat_MI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat_MI.Interface
{
    public interface IMessageServer
    {
        void SendtoAll(MessageModel message);
        void SendToOthers(MessageModel message);
        void SendToOne(MessageModel message);
        void GetUserList();
        void RegisterConnection(string operatorId, string userName);
    }
}
