using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.entities
{
    public class ChequeTree
    {
        public List<string> RootCustomerNumbers { get; set; }

        public List<ChequeCustomer> LinkedChequeInfo { get; set; }
    }
}