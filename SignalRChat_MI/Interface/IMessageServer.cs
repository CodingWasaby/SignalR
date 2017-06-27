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
        /// <summary>
        /// SendtoAll
        /// </summary>
        /// <param name="message"></param>
        void SendtoAll(MessageModel message);
        /// <summary>
        /// SendToOthers
        /// </summary>
        /// <param name="message"></param>
        void SendToOthers(MessageModel message);
        /// <summary>
        /// SendToOne
        /// </summary>
        /// <param name="message"></param>
        void SendToOne(MessageModel message);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        void GetUserList();
        /// <summary>
        /// 注册链接
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="userName"></param>
        void RegisterConnection(string operatorId, string userName);
    }
}
