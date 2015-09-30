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

        public List<string> NodeList { get; set; }
    }
}