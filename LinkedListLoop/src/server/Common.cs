using LinkedListLoop.entities;
using LinkedListLoop.src.server;
using LinkedListLoop.src.server.entities;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace LinkedListLoop.src
{
    public static class Common
    {
        public static ChequeInfo GetChequeInfo(string sender, string receiver, decimal amount)
        {
            ChequeInfo cheque = new ChequeInfo();
            cheque.Amount = amount;
            cheque.Receiver = receiver;
            cheque.Sender = sender;
            cheque.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return cheque;
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
            List<MenuItem> menuList = GetDefinition<List<MenuItem>>(Constants.MenuItemsPath);

            menuList.Select(a => { a.Link = a.IsLocal ? string.Concat(GlobalConfiguration.Host, a.Link) : a.Link; return a; }).ToList();
            menuList.Select(a => { a.Title = ResourceHelper.GetString(a.Title); return a; }).ToList();

            return menuList;
        }

        public static T GetDefinition<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string json = reader.ReadToEnd();

                    if (!string.IsNullOrEmpty(json))
                    {
                        T list = JsonConvert.DeserializeObject<T>(json);
                        return list;
                    }
                    else
                    {
                        throw new Exception(ResourceHelper.GetString("InvalidDefinitionForm"));
                    }
                }
            }
            else
            {
                throw new Exception(ResourceHelper.GetStringFormat("DefinitionPathNotFound", path));
            }
        }

        public static LoginModel GetGuestInfo()
        {
            LoginModel guestLogin = GetDefinition<LoginModel>(Constants.GuestLoginInfoPath);
            return guestLogin;
        }

        public static List<string> GetAvailableRoles(AccessLevel minLevel)
        {
            var values = Enum.GetValues(typeof(AccessLevel)).Cast<AccessLevel>();

            return values.Where(a => a.CompareTo(minLevel) >= 0).Select(b=>b.ToString()).ToList();
        }

        public static string GenerateMLJavascript()
        {
            string jsPath = Constants.MultiLingualJSPath;
            string resourcePath = ResourceHelper.GetResourcePath();

            string[] resourceFiles = Directory.GetFiles(resourcePath, "*.resx");

            foreach (string file in resourceFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string resourceCulture = fileName.Replace("strings.", string.Empty);
                string jsFileName = string.Concat(jsPath, resourceCulture, ".js");

                ResourceSet allResource = ResourceHelper.GetResourceSet(new CultureInfo(resourceCulture));

                StringBuilder jsContent = new StringBuilder();
                jsContent.AppendLine("var ML = {");

                foreach (DictionaryEntry entry in allResource)
                {
                    string resourceKey = entry.Key.ToString();
                    string resource = entry.Value.ToString();

                    jsContent.AppendLine(string.Format("{0}: '{1}',", resourceKey, resource));
                }

                jsContent.AppendLine( string.Format("culture: '{0}'", resourceCulture));
                jsContent.AppendLine("};");

                if (!File.Exists(jsFileName))
                {
                    using (File.Create(jsFileName)) { }
                }

                using (StreamWriter writer = new StreamWriter(jsFileName))
                {
                    writer.Write(jsContent.ToString());
                }
            }

            return "ok";
        }

        public static string GetClientIp(System.Web.HttpContext context)
        {
            if (context != null)
            {
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return string.Empty;
            }
        }

        public static object GetDynamicObject(string className)
        {
            Type transportType = Type.GetType(className);

            if (transportType != null)
            {
                return Activator.CreateInstance(transportType);
            }
            else
            {
                throw new Exception(ResourceHelper.GetStringFormat("ClassNotFound", className));
            }
        }

    }
}