using LinkedListLoop.src.server.entities.transaction_types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    [Serializable()]
    public class TransactionType
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string SampleDataPath { get; set; }

        public string FileReader { get; set; }

        public List<UiGridColumn> ColumnList { get; set; }

        public string ServiceName { get; set; }
    }
}