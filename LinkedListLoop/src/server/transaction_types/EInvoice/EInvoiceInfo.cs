using LinkedListLoop.src;
using LinkedListLoop.src.server;
using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LinkedListLoop.entities
{
    [Table("EInvoice")]
    public class EInvoiceInfo : TransactionTypeDefinition
    {
        public string InvoiceNumber { get; set; }
    }
}