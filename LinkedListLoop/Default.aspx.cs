using LinkedListLoop.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;

namespace LinkedListLoop
{
    public partial class Default : System.Web.UI.Page
    {
        public static Dictionary<string, List<string>> SenderReceiverList = new Dictionary<string, List<string>>();
        public static List<string> RootList = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //List<LoopResult> loops = GetTreeLoops();
            //var jsonLoops = JsonConvert.SerializeObject(loops);
        }

       
        public static List<ChequeInfo> GetChequeList()
        {
            List<ChequeInfo> chequeList = new List<ChequeInfo>();

            chequeList.Add(GetChequeInfo("A", "B", 0));
            chequeList.Add(GetChequeInfo("D", "E", 0));
            chequeList.Add(GetChequeInfo("C", "D", 0));
            chequeList.Add(GetChequeInfo("A", "D", 0));
            chequeList.Add(GetChequeInfo("D", "B", 0));
            chequeList.Add(GetChequeInfo("D", "C", 0));
            chequeList.Add(GetChequeInfo("C", "B", 0));
            chequeList.Add(GetChequeInfo("A", "A", 0));
            chequeList.Add(GetChequeInfo("B", "C", 0));
            chequeList.Add(GetChequeInfo("G", "H", 0));
            chequeList.Add(GetChequeInfo("H", "I", 0));
            chequeList.Add(GetChequeInfo("F", "K", 0));

            return chequeList;
        }

        [WebMethod]
        public static string GetChequeListJson()
        {
            return JsonConvert.SerializeObject(GetChequeList());
        }

        private ChequeTree GetChequeTree()
        {
            ChequeTree tree = new ChequeTree();

            tree.LinkedChequeInfo = new List<ChequeCustomer>();
            tree.RootCustomerNumbers = new List<string>();

            List<ChequeInfo> chequeList = GetChequeList();
            Dictionary<string, List<string>> srList = GetSenderReceiverList(chequeList);

            foreach (string key in srList.Keys)
            { 
                
            }

            /*
            List<ChequeInfo> chequeList = GetChequeList();
            
            foreach (ChequeInfo cheque in chequeList)
            {
                bool senderExists = tree.LinkedChequeInfo.Count(a => a.CustomerNumber.Equals(cheque.Sender)) > 0;
                bool receiverExists = tree.LinkedChequeInfo.Count(a => a.CustomerNumber.Equals(cheque.Receiver)) > 0;

                if (!senderExists)
                {
                    tree.RootCustomerNumbers.Add(cheque.Sender);
                    tree.LinkedChequeInfo.Add(new ChequeCustomer(cheque.Sender));
                }

                ChequeCustomer receiverNode = tree.LinkedChequeInfo.FirstOrDefault(a => a.CustomerNumber.Equals(cheque.Receiver));

                if (!receiverExists)
                {
                    receiverNode = new ChequeCustomer(cheque.Receiver);
                }
                
                ChequeCustomer senderNode = tree.LinkedChequeInfo.FirstOrDefault(a => a.CustomerNumber.Equals(cheque.Sender));

                if (!senderExists && receiverExists)
                {
                    tree.RootCustomerNumbers.Remove(cheque.Receiver);
                    tree.LinkedChequeInfo.Remove(receiverNode);
                }

                if (senderNode.ReceiverList.Count(a => a.CustomerNumber.Equals(receiverNode.CustomerNumber)) == 0)
                {
                    senderNode.ReceiverList.Add(receiverNode);
                }
            }
            */
            return tree;
        }

        private static Dictionary<string, List<string>> GetSenderReceiverList(List<ChequeInfo> chequeList)
        {
            Dictionary<string, List<string>> srList = new Dictionary<string, List<string>>();
            List<string> allReceiverList = new List<string>();

            string toBeRemovedRoot = string.Empty;

            foreach (ChequeInfo cheque in chequeList)
            {
                if (cheque.Sender != cheque.Receiver)
                {
                    bool senderExists = srList.Keys.Contains(cheque.Sender);
                    bool receiverExists = srList.Keys.Contains(cheque.Receiver);
                    bool allReceiverExists = allReceiverList.Contains(cheque.Sender);

                    allReceiverList.Add(cheque.Receiver);

                    if (!senderExists)
                    {
                        srList.Add(cheque.Sender, new List<string>());

                        if (!allReceiverExists)
                        {
                            RootList.Add(cheque.Sender);
                        }
                    }

                    List<string> receiverList = srList[cheque.Sender];

                    if (!receiverList.Contains(cheque.Receiver))
                    {
                        receiverList.Add(cheque.Receiver);
                    }

                    if (receiverExists)
                    {
                        if (!string.IsNullOrEmpty(toBeRemovedRoot))
                        {
                            RootList.Remove(toBeRemovedRoot);
                            toBeRemovedRoot = string.Empty;
                        }

                        if (RootList.Count > 1)
                        {
                            RootList.Remove(cheque.Receiver);
                        }
                        else
                        {
                            toBeRemovedRoot = cheque.Receiver;
                        }
                    }
                }
            }

            return srList;
        }

        [WebMethod]
        public static List<LoopResult> GetTreeLoops(List<ChequeInfo> chequeList)
        {
            List<LoopResult> loops = new List<LoopResult>();
            SenderReceiverList = new Dictionary<string, List<string>>();
            RootList = new List<string>();

            Dictionary<string, List<string>> list = GetSenderReceiverList(chequeList);
            
            SenderReceiverList = list;

            if (RootList.Count == 0)
            {
                throw new Exception("no root found");
            }

            foreach (string root in RootList)
            {
                loops.AddRange(GetTreeLoopsRecursive(root, new List<string>()));
            }
            
            return loops;
        }

        [WebMethod]
        public static List<LoopResult> GetFilteredLoops(List<ChequeInfo> senderList)
        {
            List<LoopResult> loops = GetTreeLoops(senderList);

            if (loops != null)
            {
                loops = loops.Where(a=>a.HasLoop).ToList();
            }

            return loops;
        }

        private static List<LoopResult> GetTreeLoopsRecursive(string rootNode, List<string> path)
        {
            List<LoopResult> loops = new List<LoopResult>();
            List<string> subPath = new List<string>();
            subPath.AddRange(path);

            if (subPath.Contains(rootNode))
            {
                subPath.Add(rootNode);
                loops.Add(GetLoopResult(subPath, rootNode));
                return loops;
            }

            subPath.Add(rootNode);

            if (SenderReceiverList.ContainsKey(rootNode))
            {
                List<string> subSenders = SenderReceiverList[rootNode];

                foreach (string sub in subSenders)
                {
                    loops.AddRange(GetTreeLoopsRecursive(sub, subPath));
                }
            }

            loops.Add(GetLoopResult(subPath, string.Empty));
            return loops;
        }

        private static LoopResult GetLoopResult(List<string> path, string loopElement)
        {
            LoopResult result = new LoopResult();
            result.Items = path;

            if (!string.IsNullOrEmpty(loopElement))
            {
                int start = path.IndexOf(loopElement);
                int end = path.LastIndexOf(loopElement);

                if (start < 0 || end < 0 || end <= start)
                {
                    throw new Exception("invalid loop index(es)");
                }

                result.Loop = path.GetRange(start, (end - start) + 1);
                result.HasLoop = true;
                result.LoopHtmlText = GetLoopText(result.Items, result.Loop);
            }

            return result;
        }

        private static string GetLoopText(List<string> items, List<string> loop)
        {
            string itemsText = items != null ? string.Join("->", items) : string.Empty;
            string loopText = loop != null ? string.Join("->", loop) : string.Empty;

            if (!string.IsNullOrEmpty(itemsText) && !string.IsNullOrEmpty(loopText))
            {
                int startIndex = itemsText.IndexOf(loopText);

                if (startIndex > -1)
                {
                    if (loopText.Length + startIndex <= itemsText.Length)
                    {
                        string first = startIndex > 0 ? itemsText.Substring(0, startIndex) : string.Empty;
                        string highLight = string.Format("<span class\"loopHightLight\" style=\"font-size: large;font-weight: bold;\">{0}</span>", itemsText.Substring(startIndex, loopText.Length));
                        string last = startIndex + loopText.Length == itemsText.Length ? string.Empty : itemsText.Substring(startIndex+ + loopText.Length);

                        return string.Format("{0}{1}{2}", first, highLight, last);
                    }
                    else
                    {
                        return "2:Döngü bulunamadı";
                    }
                }
                else
                {
                    return "1:Döngü Eşleşmesi bulunamadı";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private static ChequeInfo GetChequeInfo(string sender, string receiver, decimal amount)
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