using LinkedListLoop.entities;
using LinkedListLoop.src.server.transaction_types.Cheque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities.transaction_types.EInvoice
{
    public class EInvoiceProcessor : TransactionProcessor<EInvoiceInfo>
    {
        public override void SaveFileRecords(TranFileInfo fileInfo, object records)
        {
            List<EInvoiceInfo> einvoiceRecords = records as List<EInvoiceInfo>;
            einvoiceRecords = einvoiceRecords.Select(a => { a.id = Guid.NewGuid().ToString(); a.FileId = fileInfo.Id; return a; }).ToList();

            EInvoiceService service = new EInvoiceService();
            service.BulkInsert(einvoiceRecords);
        }

        protected override List<TransactionTypeDefinition> GetTranBaseList(List<EInvoiceInfo> senderList)
        {
            return senderList.Select(
                a => new TransactionTypeDefinition
                {
                    Amount = a.Amount,
                    Date = a.Date,
                    id = a.id,
                    Receiver = a.Receiver,
                    Sender = a.Sender
                }
            ).ToList();
        }
             
    }
}