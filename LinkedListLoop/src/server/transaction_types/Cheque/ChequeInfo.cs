using LinkedListLoop.src;
using LinkedListLoop.src.server;
using LinkedListLoop.src.server.entities;
using LinkedListLoop.src.server.entities.transaction_types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LinkedListLoop.entities
{
    [Table("Cheques")]
    public class ChequeInfo : TransactionTypeDefinition
    {
        
    }

}