using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI2017.Controllers.common
{
    public class Message
    {
        public Message()
        {
            this.count = 0;
            this.result = new object[0];
            this.message = "";
        }

        public Message(string errMsg)
        {
            this.count = 0;
            this.result = new object[0];
            this.message = errMsg;
        }

        public int count { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}