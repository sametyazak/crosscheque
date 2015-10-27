using LinkedListLoop.src.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    [Serializable()]
    public class Log
    {
        public LogDirection Direction { get; set; }
        
        public string IP { get; set; }

        public string ComputerName { get; set; }

        public string Reference { get; set; }

        public object Message { get; set; }

        public DateTime Date { get; set; }

        public string Url { get; set; }

        public string Level { get; set; }

    }
}