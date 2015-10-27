using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    [Serializable()]
    public class MenuItem
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public AccessLevel MinAccessLevel { get; set; }

        public bool IsLocal { get; set; }

        public string Link { get; set; }

        public bool IsVisibleForNotAuthorized { get; set; }
    }
}