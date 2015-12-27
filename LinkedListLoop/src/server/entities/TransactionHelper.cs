using LinkedListLoop.entities;
using LinkedListLoop.src.server.entities.transaction_types;
using LinkedListLoop.src.server.entities.transaction_types.Cheque;
using LinkedListLoop.src.server.entities.transaction_types.EInvoice;
using LinkedListLoop.src.server.transaction_types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class TransactionHelper
    {
        public static List<UiGridColumn> GetTransactionTypeColumnList(string typeName)
        {
            TransactionType tranType = GetTransactionType(typeName);
            return tranType.ColumnList;
        }

        public static ITransactionService<T> GetDynamicTranTypeService<T>(string typeName)
        {
            TransactionType tranType = GetTransactionType(typeName);

            ITransactionService<T> typeDefinition = Common.GetDynamicObject(tranType.ServiceName) as ITransactionService<T>;
            return typeDefinition;
        }

        public static TransactionProcessorBase GetDynamicTransactionProcessor(string typeName)
        {
            if (GlobalConfiguration.TransactionProcessors == null)
            {
                GlobalConfiguration.TransactionProcessors = GetTransactionProcessors();
            }

            return GlobalConfiguration.TransactionProcessors[typeName];
        }

        public static IFileReader GetDynamicTranTypeFileReader(string typeName)
        {
            TransactionType tranType = GetTransactionType(typeName);

            IFileReader typeDefinition = Common.GetDynamicObject(tranType.FileReader) as IFileReader;
            return typeDefinition;
        }

        public static TransactionType GetTransactionType(string typeName)
        {
            if (GlobalConfiguration.TransctionTypes == null)
            {
                GlobalConfiguration.TransctionTypes = Common.GetDefinition<List<TransactionType>>(Constants.TransactionTypePath);
            }
            
            TransactionType tranType = GlobalConfiguration.TransctionTypes.FirstOrDefault(a => a.Name == typeName);

            if (tranType != null)
            {
                return tranType;
            }
            else
            {
                throw new Exception(ResourceHelper.GetStringFormat("TranTypeDefinitionNotFound", typeName));
            }
        }

        public static Dictionary<string, TransactionProcessorBase> GetTransactionProcessors()
        {
            Dictionary<string, TransactionProcessorBase> processors = new Dictionary<string, TransactionProcessorBase>();
            processors.Add("Cheque", new ChequeProcessor());
            processors.Add("EInvoice", new EInvoiceProcessor());

            return processors;
        }
    }
}