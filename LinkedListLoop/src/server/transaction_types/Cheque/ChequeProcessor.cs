using LinkedListLoop.entities;
using LinkedListLoop.src.server.transaction_types.Cheque;
using LinkedListLoop.src.server.transaction_types.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace LinkedListLoop.src.server.entities.transaction_types.Cheque
{
    public class ChequeProcessor : TransactionProcessor<ChequeInfo>
    {
        protected override List<TransactionTypeDefinition> GetTranBaseList(List<ChequeInfo> senderList)
        {
            return senderList.Select(
                a => new TransactionTypeDefinition { 
                    Amount = a.Amount,
                    Date = a.Date,
                    id = a.id,
                    Receiver = a.Receiver,
                    Sender = a.Sender
                }
            ).ToList();
        }

        public override void SaveFileRecords(TranFileInfo fileInfo, object records)
        {
            List<ChequeInfo> chequeList = records as List<ChequeInfo>;
            chequeList = chequeList.Select(a => { a.id = Guid.NewGuid().ToString(); a.FileId = fileInfo.Id; return a; }).ToList();

            ChequeService service = new ChequeService();
            service.BulkInsert(chequeList);
        }
    }
}