using SignalRChat_MI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat_MI.Interface
{
    public interface IMessageClient
    {
        void SendMessage(MessageModel message);

        void SendUserList(List<UserModel> list);

        void LoginTip(string userName);
    }
}
