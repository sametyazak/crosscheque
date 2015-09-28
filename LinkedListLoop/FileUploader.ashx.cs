using LinkedListLoop.entities;
using LinkedListLoop.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

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
            string fileSavePath = string.Empty;

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedFile"];

                if (httpPostedFile != null)
                {
                    fileSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"data\tmp\{0}_{1}_{2}.csv", httpPostedFile.FileName, Guid.NewGuid(), DateTime.Now.ToString("dd.MM.yyyy")));
                    httpPostedFile.SaveAs(fileSavePath);
                }
            }

            return fileSavePath;
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