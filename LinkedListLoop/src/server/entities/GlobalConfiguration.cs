using LinkedListLoop.src.server.entities.transaction_types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public static class GlobalConfiguration
    {
        public static bool IsServerSideLogEnabled { get; set; }

        public static bool IsClientSideLogEnabled { get; set; }

        public static List<MenuItem> MenuList { get; set; }

        public static List<TransactionType> TransctionTypes { get; set; }

        public static string Host { get; set; }

        public static CultureInfo CurrentCulture { get; set; }

        public static Dictionary<string, TransactionProcessorBase> TransactionProcessors { get; set; }

    }
}