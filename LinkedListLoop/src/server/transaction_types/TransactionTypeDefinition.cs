using LinkedListLoop.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class TransactionTypeDefinition
    {
        public string Sender { get; set; }

        public string Receiver { get; set; }

        public decimal Amount { get; set; }

        public string Date { get; set; }

        public string id { get; set; }

        public string FileId { get; set; }
    }
}