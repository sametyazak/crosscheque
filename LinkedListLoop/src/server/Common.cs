using LinkedListLoop.entities;
using LinkedListLoop.src.server.entities;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

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

        public static string GetMenuItemPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\definitions\LeftMenu.txt");
        }

        public static string GetGuestInfoPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\definitions\guest.txt");
        }

        public static string GetLogFileName()
        {
            var rootAppender = ((Hierarchy)log4net.LogManager.GetRepository())
                                             .Root.Appenders.OfType<FileAppender>()
                                             .Where(a => a.Name == "LogFileAppender").FirstOrDefault();

            string filename = rootAppender != null ? rootAppender.File : string.Empty;

            return filename;
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            T result;
            return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        }

        public static string GetXmlElementValue(XElement item, string childName)
        {
            return item.Element(childName) != null ? item.Element(childName).Value : string.Empty;
        }

        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        public static List<MenuItem> GetMenuItems()
        {
            string menuPath = GetMenuItemPath();

            if (File.Exists(menuPath))
            {
                using (StreamReader reader = new StreamReader(menuPath))
                {
                    string menuJson = reader.ReadToEnd();

                    if (!string.IsNullOrEmpty(menuJson))
                    {
                        List<MenuItem> menuList = JsonConvert.DeserializeObject<List<MenuItem>>(menuJson);
                        menuList.Select(a => { a.Link = a.IsLocal ? string.Concat(GlobalConfiguration.Host, a.Link) : a.Link; return a; }).ToList();

                        return menuList;
                    }
                    else
                    {
                        throw new Exception("geçersiz menu tanımı");
                    }
                }
            }
            else
            {
                throw new Exception("menu tanım dosyası bulunamadı!");
            }
        }

        public static LoginModel GetGuestInfo()
        {
            string guestPath = GetGuestInfoPath();

            if (File.Exists(guestPath))
            {
                using (StreamReader reader = new StreamReader(guestPath))
                {
                    string guestJson = reader.ReadToEnd();

                    LoginModel guestInfo = JsonConvert.DeserializeObject<LoginModel>(guestJson);
                    return guestInfo;
                }
            }
            else
            {
                throw new Exception("müsafir giriş dosyası bulunamadı!");
            }
        }

        public static List<string> GetAvailableRoles(AccessLevel minLevel)
        {
            var values = Enum.GetValues(typeof(AccessLevel)).Cast<AccessLevel>();

            return values.Where(a => a.CompareTo(minLevel) >= 0).Select(b=>b.ToString()).ToList();
        }
    }
}