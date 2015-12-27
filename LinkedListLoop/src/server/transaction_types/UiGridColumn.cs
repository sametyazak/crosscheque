using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities.transaction_types
{
    [Serializable()]
    public class UiGridColumn
    {
        public string Name { get; set; }

        public string EntryType { get; set; }

        public string Header { get; set; }

        public int Width { get; set; }

        public string FileFieldName { get; set; }
    }
}