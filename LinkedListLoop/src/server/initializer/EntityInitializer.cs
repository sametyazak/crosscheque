using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using LinkedListLoop.entities;

namespace LinkedListLoop.src.server.initializer
{
    public class EntityInitializer : CreateDatabaseIfNotExists<EntityContext>
    {
        protected override void Seed(EntityContext context)
        {
            var cheques = new List<ChequeInfo>{
                new ChequeInfo { 
                    Sender = "Sender", 
                    Receiver = "receiver", 
                    Amount = 0, 
                    Date = "1900-01-01", 
                    FileId = "-1"
                }
            };

            var files = new List<TranFileInfo>
            {
                new TranFileInfo {
                    CompanyInfo = string.Empty,
                    UserInfo = string.Empty,
                    Id = Guid.NewGuid().ToString(),
                    Path = string.Empty
                }
            };

            cheques.ForEach(category => context.ChequeList.Add(category));

            files.ForEach(category => context.FileList.Add(category));
        }
    }
}