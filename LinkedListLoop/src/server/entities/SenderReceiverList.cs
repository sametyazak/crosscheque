using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class SenderReceiverList
    {
        public Dictionary<string, List<NetworkItem>> TotalList { get; set; }

        public List<string> RootList { get; set; }
    }
}