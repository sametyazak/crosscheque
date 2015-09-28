using LinkedListLoop.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;
using System.Reflection;
using System.IO;
using LinkedListLoop.src;

namespace LinkedListLoop
{
    public partial class ChequeLoop : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //List<LoopResult> loops = GetTreeLoops();
            //var jsonLoops = JsonConvert.SerializeObject(loops);

            //GetSampleData();

            //CsvReader<ChequeInfo> fileList = new CsvReader<ChequeInfo>();
            //List<ChequeInfo> chequeList = fileList.GetTempList(GetSampleDataFilePath());
        }

        public static List<ChequeInfo> GetChequeList()
        {
            List<ChequeInfo> chequeList = new List<ChequeInfo>();

            chequeList.Add(Common.GetChequeInfo("A", "B", 0));
            chequeList.Add(Common.GetChequeInfo("D", "E", 0));
            chequeList.Add(Common.GetChequeInfo("C", "D", 0));
            chequeList.Add(Common.GetChequeInfo("A", "D", 0));
            chequeList.Add(Common.GetChequeInfo("D", "B", 0));
            chequeList.Add(Common.GetChequeInfo("D", "C", 0));
            chequeList.Add(Common.GetChequeInfo("C", "B", 0));
            chequeList.Add(Common.GetChequeInfo("A", "A", 0));
            chequeList.Add(Common.GetChequeInfo("B", "C", 0));
            chequeList.Add(Common.GetChequeInfo("G", "H", 0));
            chequeList.Add(Common.GetChequeInfo("H", "I", 0));
            chequeList.Add(Common.GetChequeInfo("F", "K", 0));

            return chequeList;
        }

        //private ChequeTree GetChequeTree()
        //{
        //    ChequeTree tree = new ChequeTree();

        //    tree.LinkedChequeInfo = new List<ChequeCustomer>();
        //    tree.RootCustomerNumbers = new List<string>();

        //    List<ChequeInfo> chequeList = GetChequeList();
        //    Dictionary<string, List<string>> srList = GetSenderReceiverList(chequeList);

        //    foreach (string key in srList.Keys)
        //    {

        //    }

        //    /*
        //    List<ChequeInfo> chequeList = GetChequeList();
            
        //    foreach (ChequeInfo cheque in chequeList)
        //    {
        //        bool senderExists = tree.LinkedChequeInfo.Count(a => a.CustomerNumber.Equals(cheque.Sender)) > 0;
        //        bool receiverExists = tree.LinkedChequeInfo.Count(a => a.CustomerNumber.Equals(cheque.Receiver)) > 0;

        //        if (!senderExists)
        //        {
        //            tree.RootCustomerNumbers.Add(cheque.Sender);
        //            tree.LinkedChequeInfo.Add(new ChequeCustomer(cheque.Sender));
        //        }

        //        ChequeCustomer receiverNode = tree.LinkedChequeInfo.FirstOrDefault(a => a.CustomerNumber.Equals(cheque.Receiver));

        //        if (!receiverExists)
        //        {
        //            receiverNode = new ChequeCustomer(cheque.Receiver);
        //        }
                
        //        ChequeCustomer senderNode = tree.LinkedChequeInfo.FirstOrDefault(a => a.CustomerNumber.Equals(cheque.Sender));

        //        if (!senderExists && receiverExists)
        //        {
        //            tree.RootCustomerNumbers.Remove(cheque.Receiver);
        //            tree.LinkedChequeInfo.Remove(receiverNode);
        //        }

        //        if (senderNode.ReceiverList.Count(a => a.CustomerNumber.Equals(receiverNode.CustomerNumber)) == 0)
        //        {
        //            senderNode.ReceiverList.Add(receiverNode);
        //        }
        //    }
        //    */
        //    return tree;
        //}

    }

}