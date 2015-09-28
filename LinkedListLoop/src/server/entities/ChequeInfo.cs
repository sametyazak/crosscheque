using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.entities
{
    public class ChequeInfo
    {
        public string Sender { get; set; }

        public string Receiver { get; set; }
        
        public decimal Amount { get; set; }

        public string Date { get; set; }

        public string id { get; set; }
    }
}