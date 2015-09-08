using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.entities
{
    public class ChequeCustomer
    {
        public ChequeCustomer(string customerNumber)
        {
            this.CustomerNumber = customerNumber;
            this.ReceiverList = new List<ChequeCustomer>();
        }

        public string CustomerNumber { get; set; }

        public List<ChequeCustomer> ReceiverList { get; set; }
    }
}