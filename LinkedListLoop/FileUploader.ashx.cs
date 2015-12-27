using LinkedListLoop.entities;
using LinkedListLoop.src;
using LinkedListLoop.src.server.entities;
using LinkedListLoop.src.server.transaction_types.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace LinkedListLoop
{
    /// <summary>
    /// Summary description for FileUploader
    /// </summary>
    public class FileUploader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                List<ChequeInfo> chequeList = new List<ChequeInfo>();

                string filePath = SaveTempFile();

                if (!string.IsNullOrEmpty(filePath))
                {
                    context.Response.Write(filePath);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        private string SaveTempFile()
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
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedFile"];

                if (httpPostedFile != null)
                {
                    fileSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"data\tmp\{0}_{1}_{2}.csv", httpPostedFile.FileName, Guid.NewGuid(), DateTime.Now.ToString("dd.MM.yyyy")));
                    httpPostedFile.SaveAs(fileSavePath);
                    
                    file.Path = fileSavePath;
                    file.UploadEndDate = DateTime.Now;
                    file.UploadTime = file.UploadEndDate - file.UploadStartDate;

                    fileService.Create(file);
                }
            }

            return file.Id;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}