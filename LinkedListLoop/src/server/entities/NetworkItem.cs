using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class NetworkItem
    {
        public string Sender { get; set; }

        public string Receiver { get; set; }

        public int NodeValue { get; set; }

        public string EdgeTitle { get; set; }
    }
}