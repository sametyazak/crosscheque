using LinkedListLoop.entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src
{
    public static class Common
    {
        public static List<ChequeInfo> GetFileChequeList(string path, bool deleteFile)
        {
            CsvReader<ChequeInfo> fileList = new CsvReader<ChequeInfo>();
            List<ChequeInfo> chequeList = fileList.GetTempList(path);

            if (File.Exists(path) && deleteFile)
            {
                File.Delete(path);
            }

            return chequeList;
        }

        //private static List<ChequeInfo> ConvertChequeListFromFile(List<ChequeInfoFile> fileRecords)
        //{
        //    List<ChequeInfo> chequeList = new List<ChequeInfo>();

        //    foreach (ChequeInfoFile record in fileRecords)
        //    {
        //        chequeList.Add(GetChequeInfo(record.Sender, record.Receiver, record.Amount));
        //    }

        //    return chequeList;
        //}

        public static ChequeInfo GetChequeInfo(string sender, string receiver, decimal amount)
        {
            ChequeInfo cheque = new ChequeInfo();
            cheque.Amount = amount;
            cheque.Receiver = receiver;
            cheque.Sender = sender;
            cheque.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return cheque;
        }

        public static string GetSampleDataFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\cc_sample_data.csv");
        }

    }
}