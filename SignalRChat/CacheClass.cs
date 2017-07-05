using SignalRChat_MI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat
{
    public static class CacheClass
    {
        public static List<UserModel> _UserList = new List<UserModel>();

        public static object lockobj = new object();
    }
}