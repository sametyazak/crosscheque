using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinkedListLoop;
using LinkedListLoop.entities;
using LinkedListLoop.src.server.transaction_types.Cheque;
using System.Linq;
using System.Collections.Generic;

namespace LinkedListLoop.Test
{
    [TestClass]
    public class LinkedTest
    {
        [TestMethod]
        public void TestMain()
        {
            //TestMethod1();
            TestMethod4();
        }


        public void TestMethod1()
        {
            ChequeInfo chequeInfo = new ChequeInfo()
            {
                Amount = 0,
                Date = "2015-01-01",
                Sender = "steven",
                Receiver = "gerrard",
                FileId = string.Empty
            };

            ChequeService service = new ChequeService();
            service.Create(chequeInfo);
        }

        public void TestMethod2()
        {
            ChequeService service = new ChequeService();
            ChequeInfo test = service.GetRecordById("56d70e4e-976f-40d4-a811-f41069c5e694");
        }

        public void TestMethod3()
        {
            ChequeService service = new ChequeService();
            ChequeInfo test = service.GetRecordById("56d70e4e-976f-40d4-a811-f41069c5e694");

            test.Amount = 2012.31m;
            service.Update(test);

            ChequeInfo test2 = service.GetRecordById("56d70e4e-976f-40d4-a811-f41069c5e694");
        }

        public void TestMethod4()
        { 
            List<ChequeInfo> chequeList = new List<ChequeInfo>();
            ChequeService service = new ChequeService();

            ChequeInfo chequeInfo = new ChequeInfo()
            {
                Amount = 5,
                Date = "2015-01-01",
                Sender = "steven",
                Receiver = "gerrard",
                FileId = string.Empty,
                id = Guid.NewGuid().ToString()
            };

            ChequeInfo chequeInfo2 = new ChequeInfo()
            {
                Amount = 10,
                Date = "2015-01-01",
                Sender = "steven",
                Receiver = "gerrard",
                FileId = string.Empty,
                id = Guid.NewGuid().ToString()
            };

            chequeList.Add(chequeInfo);
            chequeList.Add(chequeInfo2);

            service.BulkInsert(chequeList);
        }
    }
}
