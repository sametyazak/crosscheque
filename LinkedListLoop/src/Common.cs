using FileHelpers;
using LinkedListLoop.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src
{
    public static class Common
    {
        public static List<ChequeInfo> GetFileChequeList(string path)
        {
            var engine = new FileHelperEngine<ChequeInfoFile>();
            engine.Options.IgnoreFirstLines = 1;

            var sampleList = engine.ReadFileAsList(path);
            var convertedList = ConvertChequeListFromFile(sampleList);

            return convertedList;
        }

        private static List<ChequeInfo> ConvertChequeListFromFile(List<ChequeInfoFile> fileRecords)
        {
            List<ChequeInfo> chequeList = new List<ChequeInfo>();

            foreach (ChequeInfoFile record in fileRecords)
            {
                chequeList.Add(GetChequeInfo(record.Sender, record.Receiver, record.Amount));
            }

            return chequeList;
        }

        public static ChequeInfo GetChequeInfo(string sender, string receiver, decimal amount)
        {
            ChequeInfo cheque = new ChequeInfo();
            cheque.Amount = amount;
            cheque.Receiver = receiver;
            cheque.Sender = sender;
            cheque.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return cheque;
        }

    }
}