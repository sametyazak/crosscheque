using DataStreams.Csv;
using LinkedListLoop.entities;
using LinkedListLoop.src.server;
using LinkedListLoop.src.server.db_connection;
using LinkedListLoop.src.server.entities;
using LinkedListLoop.src.server.transaction_types;
using LinkedListLoop.src.server.transaction_types.File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebMatrix.WebData;

namespace LinkedListLoop
{
    public partial class FileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool endRequest = false;
            FileUploadResponse response = new FileUploadResponse();

            try
            {
                FileUploadCheck check = AjaxMethods.CheckFileUpload(HttpContext.Current.Request.ContentLength);
                if (!(check.IsUploadValid))
                {
                    throw new Exception(check.Message);
                }

                if (!HttpContext.Current.Request.QueryString.AllKeys.Contains("tranType"))
                {
                    throw new Exception(ResourceHelper.GetString("TranTypeParameterIsMissing"));
                }

                string tranType = HttpContext.Current.Request.QueryString["tranType"];

                TranFileInfo file = SaveTempFile(tranType);
                response.FileInfo = file;
                response.status = "server";

                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(JsonConvert.SerializeObject(response));
                endRequest = true;
            }
            catch (Exception ex)
            {
                response.status = "error";
                Response.Write(JsonConvert.SerializeObject(response));
                LogManager.InsertExceptionLog(ex);
            }

            if (endRequest)
                Response.End();
        }

        private TranFileInfo SaveTempFile(string tranType)
        {
            FileService fileService = new FileService();

            TranFileInfo file = new TranFileInfo()
            {
                CompanyInfo = string.Empty,
                Id = Guid.NewGuid().ToString(),
                UserInfo = WebSecurity.CurrentUserName,
                UploadStartDate = DateTime.Now
            };

            string fileSavePath = string.Empty;

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["upload"];

                if (httpPostedFile != null)
                {
                    var processor = TransactionHelper.GetDynamicTransactionProcessor(tranType);
                    string tableName = processor.UploadFile(tranType, httpPostedFile.InputStream, file.Id);

                    file.FileName = httpPostedFile.FileName;
                    file.Path = fileSavePath;
                    file.UploadEndDate = DateTime.Now;
                    file.UploadTime = file.UploadEndDate - file.UploadStartDate;
                    file.ContentLocation = FileContentLocation.TempTable;
                    file.TempTableName = tableName;
                    file.TransactionType = tranType;

                    fileService.Create(file);
                }
            }

            return file;
        }

    }
}