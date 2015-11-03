using LinkedListLoop.entities;
using LinkedListLoop.src.client;
using LinkedListLoop.src.server;
using LinkedListLoop.src.server.entities;
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
                string callReference = Guid.NewGuid().ToString();

                string queryData = context.Request.Form["queryData"];

                if (string.IsNullOrEmpty(queryData))
                {
                    throw new Exception(ResourceHelper.GetString("InvalidQueryData"));
                }
                else
                {
                    var queryInfo = JsonConvert.DeserializeObject<QueryInfo>(queryData);

                    if (queryInfo.LogRequest)
                    {
                        LogManager.InsertServerCallLog(queryData, callReference, LogDirection.ServerRequest);
                    }

                    var result = MethodFinder.CallAjaxMethod(queryInfo.ServerSideMethod, queryInfo.Data);

                    if (queryInfo.LogRequest)
                    {
                        LogManager.InsertServerCallLog(result, callReference, LogDirection.ServerResponse);
                    }

                    context.Response.Write(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                LogManager.InsertExceptionLog(ex);

                AjaxResult errorResult = new AjaxResult();
                errorResult.IsError = true;

                if (ex != null)
                {
                    errorResult.ErrorMessage = ex.Message;
                    errorResult.ErrorDetail = ex.InnerException != null ? ex.InnerException.Message : ex.ToString();
                }
                else
                {
                    errorResult.ErrorMessage = ResourceHelper.GetString("UnrecognizedError");
                    errorResult.ErrorDetail = string.Empty;
                }

                context.Response.Write(JsonConvert.SerializeObject(errorResult));
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

            public bool LogRequest { get; set; }
        }
    }
}