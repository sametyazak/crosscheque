using LinkedListLoop.src.server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop
{
    /// <summary>
    /// Summary description for ServerCall
    /// </summary>
    public class ServerCall : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string queryData = context.Request.Form["queryData"];

                if (string.IsNullOrEmpty(queryData))
                {
                    throw new Exception("Invalid Query Data");
                }
                else
                {
                    var queryInfo = JsonConvert.DeserializeObject<QueryInfo>(queryData);
                    var result = MethodFinder.CallAjaxMethod(queryInfo.ServerSideMethod, queryInfo.Data);

                    //context.Response.ContentType = "text/html";
                    context.Response.Write(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class QueryInfo
        {
            public string ServerSideMethod { get; set; }

            public object Data { get; set; }
        }
    }
}