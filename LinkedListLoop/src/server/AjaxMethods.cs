using LinkedListLoop.entities;
using LinkedListLoop.src.client;
using LinkedListLoop.src.server.entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml;
using System.Xml.Linq;
using WebMatrix.WebData;

namespace LinkedListLoop.src.server
{
    public class AjaxMethods : System.Web.UI.Page
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

                List<NetworkItem> networkList = senderList
                    .GroupBy(l => new { l.Sender, l.Receiver })
                    .Select(cl => new NetworkItem
                    {
                        Sender = cl.First().Sender,
                        Receiver = cl.First().Receiver,
                        NodeValue = cl.Count(),
                        EdgeTitle = string.Format("{0} {1}", cl.Count(), "İşlem")
                    }).ToList();

                List<string> nodeTotalList = new List<string>();
                nodeTotalList.AddRange(senderList.Select(a => a.Sender).ToList());
                nodeTotalList.AddRange(senderList.Select(a => a.Receiver).ToList());

                List<NodeItem> nodeList = nodeTotalList
                    .GroupBy(a => a)
                    .Select(s => new NodeItem
                    {
                        Id = s.First(),
                        Name = s.First(),
                        Value = s.Count(),
                        Text = string.Format("{0} {1}", s.Count(), "İşlem")
                    }).ToList();

                result.LoopList = loops;
                result.NetworkList = networkList;
                result.NodeList = nodeList;
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
                LoopResult loopRes = GetLoopResult(subPath, rootNode);
                loops.Add(loopRes);
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
                            if (!string.IsNullOrEmpty(toBeRemovedRoot))
                            {
                                rootList.Remove(toBeRemovedRoot);
                                toBeRemovedRoot = string.Empty;
                            }

                            rootList.Add(cheque.Sender);

                            if (receiverExists)
                            {
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

                    List<string> receiverList = srList[cheque.Sender];

                    if (!receiverList.Contains(cheque.Receiver))
                    {
                        receiverList.Add(cheque.Receiver);
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

        public static void InsertUiErrorLog(string message)
        {
            LogManager.InsertUiErrorLog(message);
        }

        public static List<Log> GetLogList()
        {
            List<Log> logList = new List<Log>();

            string logPath = Common.GetLogFileName();

            if (File.Exists(logPath))
            {
                string logText = string.Empty;

                using (StreamReader reader = new StreamReader(logPath))
                {
                    logText = string.Format("{0}{1}{2}", "<LogEntries>", reader.ReadToEnd(), "</LogEntries>");
                }

                XDocument xmlFile = XDocument.Parse(logText);

                var data = from item in xmlFile.Descendants("LogEntry")
                    select new Log
                    {
                        ComputerName = Common.GetXmlElementValue(item, "ComputerName"),
                        Date = DateTime.Parse(Common.GetXmlElementValue(item, "Date")),
                        Level = Common.GetXmlElementValue(item, "Level"),
                        Direction = Common.GetXmlElementValue(item, "Direction").ToEnum<LogDirection>(LogDirection.None),
                        IP = string.Empty,
                        Message = Common.GetXmlElementValue(item, "Request"),
                        Reference = Common.GetXmlElementValue(item, "Reference"),
                        Url = Common.GetXmlElementValue(item, "Url")
                        
                    };

                logList.AddRange(data.ToList());
                
            }
            else
            {
                throw new Exception(string.Format("{0}-{0}", "log file not found", logPath));
            }

            return logList;
        }

        public static string RegisterUser(RegisterModel registerData)
        {
            UserManager.Register(registerData);

            return "Default.aspx".ToAbsoluteUrl();
        }

        public static string LoginUser(LoginModel loginData)
        {
            //string returnUrl = HttpContext.Current.ApplicationInstance.Session.Count > 0 ? HttpContext.Current.ApplicationInstance.Session["ReturnUrl"].ToString() : string.Empty;
            UserManager.Login(loginData);

            return string.Format("{0}{1}", GlobalConfiguration.Host, HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query)["returnUrl"]);
        }

        public static string LogoutUser()
        {
            UserManager.LogOut();
            return "app/user/Login.aspx".ToAbsoluteUrl();
        }

        public static List<MenuItem> GetMenuList()
        {
            return GlobalConfiguration.MenuList;
        }

        public static string[] GetAllRoles()
        {
            return UserManager.GetAllRoles();
        }

        public static List<string> GetAllUsers()
        {
            return UserManager.GetAllUsers();
        }

        public static string[] GetUserRoles(string userName)
        {
            return UserManager.GetUserRoles(userName);
        }

        public static AjaxResult SaveUser(SaveRequest userData)
        {
            string userName = userData.UserName;

            foreach (var role in userData.Roles)
            {
                if (!string.IsNullOrEmpty(role.Value) && role.Value.Equals("0"))
                {
                    UserManager.DeleteUserRole(userName, role.Key);
                }
                else if (!string.IsNullOrEmpty(role.Value) && role.Value.Equals("1"))
                {
                    UserManager.AddUserRole(userName, role.Key);
                }
                else
                {
                    throw new Exception(string.Format("Geçeriz yetki değeri : {0}, {1}", role.Key, role.Value));
                }
            }

            return new AjaxResult() { IsError = false };
        }

        public static AjaxResult ChangePassword(LocalPasswordModel passwordData)
        {
            UserManager.Manage(passwordData, WebSecurity.CurrentUserName);

            return new AjaxResult() { IsError = false };
        }

        public static string GuestLogin()
        {
            LoginModel loginInfo = Common.GetGuestInfo();

            return LoginUser(loginInfo);
        }
    }
}