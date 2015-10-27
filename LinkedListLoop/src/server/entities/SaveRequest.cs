using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    [Serializable()]
    public class SaveRequest
    {
        public string UserName { get; set; }

        public Dictionary<string, string> Roles { get; set; }

    }
}