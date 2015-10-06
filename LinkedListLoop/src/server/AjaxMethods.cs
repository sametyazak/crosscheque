using LinkedListLoop.entities;
using LinkedListLoop.src.server.entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server
{
    public class AjaxMethods
    {
        public static AjaxResult GetSampleData()
        {
            AjaxResult result = new AjaxResult();
            List<ChequeInfo> sampleList = new List<ChequeInfo>();

            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\cc_sample_data.csv");
                sampleList = Common.GetFileChequeList(path, false);
                result.IsError = false;
            }
            catch (Exception ex)
            {
                sampleList = new List<ChequeInfo>();
                result.IsError = true;
                result.ErrorMessage = ex.Message;
            }

            result.ResultObject = sampleList;

            return result;
        }

        public static AjaxResult GetFileRecords(string path)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                result.ResultObject = Common.GetFileChequeList(path, true);
                result.IsError = false;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static byte[] GetSampleDownloadLink()
        {
            //string domain = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
            //string path = string.Concat(domain, @"/Data/");
            //System.Net.WebClient client = new System.Net.WebClient();
            //client.DownloadFile(Common.GetSampleDataFilePath(), @"c:\temp\cc_sample_data.csv");

            return File.ReadAllBytes(Common.GetSampleDataFilePath());

            //return path;
        }

        public static string GetSampleDataText()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\cc_sample_data.csv");
            return JsonConvert.SerializeObject((Common.GetFileChequeList(path, false)));
        }

        public static LoopProcessResult GetFilteredLoops(List<ChequeInfo> senderList)
        {
            LoopProcessResult result = new LoopProcessResult();

            if (senderList != null)
            {
                List<LoopResult> loops = GetTreeLoops(senderList);

                if (loops != null)
                {
                    loops = loops.Where(a => a.HasLoop).ToList();
                }

                List<string> nodeList = new List<string>();
                nodeList = senderList.Select(a => a.Sender).ToList();
                nodeList.AddRange(senderList.Select(a => a.Receiver).ToList());

                result.LoopList = loops;
                result.NodeList = nodeList.Distinct().ToList();
            }

            return result;
        }

        public static List<LoopResult> GetTreeLoops(List<ChequeInfo> chequeList)
        {
            List<LoopResult> loops = new List<LoopResult>();

            if (chequeList != null && chequeList.Count > 0)
            {
                SenderReceiverList list = GetSenderReceiverList(chequeList);

                if (list.RootList.Count == 0)
                {
                    throw new Exception("no root found");
                }

                foreach (string root in list.RootList)
                {
                    loops.AddRange(GetTreeLoopsRecursive(root, new List<string>(), list.TotalList));
                }
            }
            return loops;
        }

        private static List<LoopResult> GetTreeLoopsRecursive(string rootNode, List<string> path, Dictionary<string, List<string>> senderReceiverList)
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

            if (senderReceiverList.ContainsKey(rootNode))
            {
                List<string> subSenders = senderReceiverList[rootNode];

                foreach (string sub in subSenders)
                {
                    loops.AddRange(GetTreeLoopsRecursive(sub, subPath, senderReceiverList));
                }
            }

            loops.Add(GetLoopResult(subPath, string.Empty));
            return loops;
        }

        private static SenderReceiverList GetSenderReceiverList(List<ChequeInfo> chequeList)
        {
            SenderReceiverList senderReceiverList = new SenderReceiverList();
            List<string> rootList = new List<string>();

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
                            rootList.Add(cheque.Sender);
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
                            rootList.Remove(toBeRemovedRoot);
                            toBeRemovedRoot = string.Empty;
                        }

                        if (rootList.Count > 1)
                        {
                            rootList.Remove(cheque.Receiver);
                        }
                        else
                        {
                            toBeRemovedRoot = cheque.Receiver;
                        }
                    }
                }
            }

            senderReceiverList.RootList = rootList;
            senderReceiverList.TotalList = srList;

            return senderReceiverList;
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
                        string last = startIndex + loopText.Length == itemsText.Length ? string.Empty : itemsText.Substring(startIndex + +loopText.Length);

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
                
    }
}