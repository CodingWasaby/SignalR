using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat_MI.Model
{
    public class MessageModel
    {
        public string Message { get; set; }

        public string Module { get; set; }

        public string DataID { get; set; }

        public string OperationType { get; set; }

        public string ReciveID { get; set; }

        public string ReciveConnectionID { get; set; }

        public string SenderID { get; set; }

        public string SenderConnectionID { get; set; }

        public string SenderName { get; set; }

        public Font MessageFont { get; set; }

        public Color MessageColor { get; set; }

        public bool IsLog { get; set; }
    }
}
