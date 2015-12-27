using LinkedListLoop.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class LoopProcessResult
    {
        public List<LoopResult> LoopList { get; set; }

        public List<NetworkItem> NetworkList { get; set; }

        public List<NodeItem> NodeList { get; set; }

        public string Message { get; set; }

        public List<KeyValue> ProcessPerformace { get; set; }

        public bool CanSeePerformanceResult { get; set; }
    }
}