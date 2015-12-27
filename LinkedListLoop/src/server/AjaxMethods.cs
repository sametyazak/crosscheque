using LinkedListLoop.entities;
using LinkedListLoop.src.client;
using LinkedListLoop.src.server.entities;
using LinkedListLoop.src.server.entities.transaction_types;
using LinkedListLoop.src.server.transaction_types.File;
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
        public static string GetSampleData(string typeName)
        {
            TransactionType tranType = TransactionHelper.GetTransactionType(typeName);

            string sampleDataPath = GetSampleDataPath(typeName);

            FileService fileService = new FileService();
            TranFileInfo file = fileService.GetRecordByPath(sampleDataPath);

            if (file == null || string.IsNullOrEmpty(file.Id))
            {
                TranFileInfo newSampleFile = new TranFileInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    CompanyInfo = string.Empty,
                    UserInfo = WebSecurity.CurrentUserName,
                    Path = sampleDataPath,
                    TransactionType = typeName
                };

                using (StreamReader reader = new StreamReader(sampleDataPath))
                {
                    var processor = TransactionHelper.GetDynamicTransactionProcessor(typeName);
                    string tableName = processor.UploadFile(typeName, reader.BaseStream, newSampleFile.Id);

                    newSampleFile.ContentLocation = FileContentLocation.TempTable;
                    newSampleFile.TempTableName = tableName;
                }

                fileService.Create(newSampleFile);
                file = newSampleFile;
            }

            return file.Id;
        }

        public static AjaxResult GetFileRecords(GetFilesRequest request)
        {
            var processor = TransactionHelper.GetDynamicTransactionProcessor(request.TranTypeName);
            TransactionType tranType = TransactionHelper.GetTransactionType(request.TranTypeName);
            AjaxResult result = processor.ReadFileRecords(request.FileId, tranType);
            
            return result;
        }

        public static LoopProcessResult ProcessSenderFile(ProcessFileRequest request)
        {
            var processor = TransactionHelper.GetDynamicTransactionProcessor(request.TranTypeName);
            TransactionType tranType = TransactionHelper.GetTransactionType(request.TranTypeName);
            LoopProcessResult result = processor.ProcessSenderFile(request.FileId, tranType);

            return result;
        }

        public static List<UiGridColumn> GetTransactionTypeColumnList(string typeName)
        {
            return TransactionHelper.GetTransactionTypeColumnList(typeName);
        }

        public static string GetSampleDataPath(string typeName)
        {
            TransactionType tranType = TransactionHelper.GetTransactionType(typeName);
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tranType.SampleDataPath);
        }

        public static List<TransactionType> GetTransactionTypeList()
        {
            return GlobalConfiguration.TransctionTypes;
        }

        public static byte[] GetSampleDownloadLink()
        {
            return null;
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

                logList.AddRange(data.ToList().OrderByDescending(a => a.Date));

            }
            else
            {
                throw new Exception(string.Format(ResourceHelper.GetString("LogFileNotFoundFormat"), logPath));
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
                    throw new Exception(string.Format(ResourceHelper.GetString("IncorrectAuthFormat"), role.Key, role.Value));
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

        public static AjaxResult ChangeCurrentCulture(string culture)
        {
            GlobalConfiguration.CurrentCulture = new System.Globalization.CultureInfo(culture);
            GlobalConfiguration.MenuList = Common.GetMenuItems();

            return new AjaxResult() { IsError = false };
        }

        public static List<Language> GetAvailableLanguages()
        {
            return Common.GetDefinition<List<Language>>(Constants.LanguageListPath);
        }

        public static List<TranFileInfo> GetUserFiles(string tranType)
        {
            FileService fileService = new FileService();
            List<TranFileInfo> fileList = fileService.GetUserFiles(WebSecurity.CurrentUserName, tranType);
            fileList = fileList.Select(a => {a.TransactionType = ResourceHelper.GetString(a.TransactionType); return a;}).ToList();

            return fileList;
        }

        public static FileUploadCheck CheckFileUpload(int fileSize)
        {
            FileUploadCheck control = new FileUploadCheck();
            
            control.IsUploadValid = fileSize <= Constants.MaxFileUploadBytes;
            control.Message = ResourceHelper.GetStringFormat("MaxFileUplaodSize", Constants.MaxFileUploadMBytes);

            return control;
        }
    }
}