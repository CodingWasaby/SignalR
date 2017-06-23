using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.Models
{
    public class UserModel
    {
        public string UserName { get; set; }

        public string OperatorId { get; set; }

        public string ConnectionId { get; set; }
    }
}