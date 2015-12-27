using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LinkedListLoop.entities;
using LinkedListLoop.src.server.initializer;

namespace LinkedListLoop.src.server.entities
{
    public class EntityContext : DbContext
    {
        public EntityContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<EntityContext>(new EntityInitializer());
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<ChequeInfo> ChequeList { get; set; }

        public DbSet<TranFileInfo> FileList { get; set; }

        public DbSet<EInvoiceInfo> EInvoiceList { get; set; }
    }
}