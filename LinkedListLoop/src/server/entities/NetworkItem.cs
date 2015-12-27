using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class NetworkItem
    {
        [CustomeDbAttribute("Sender")]
        public string from { get; set; }

        [CustomeDbAttribute("Receiver")]
        public string to { get; set; }

        [CustomeDbAttribute("RecordCount")]
        public int value { get; set; }

        public string title { get; set; }
    }
}