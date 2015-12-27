using LinkedListLoop.src.client;
using LinkedListLoop.src.server.entities;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace LinkedListLoop.src.server
{
    public class LogManager : XmlLayoutBase
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent)
        {
            if (loggingEvent != null && loggingEvent.MessageObject is Log)
            {
                Log logObject = loggingEvent.MessageObject as Log;
                string messageJson = JsonConvert.SerializeObject(logObject.Message);

                writer.WriteStartElement("LogEntry");

                writer.WriteStartElement("ComputerName");
                writer.WriteString(loggingEvent.Domain);
                writer.WriteEndElement();

                writer.WriteStartElement("Level");
                writer.WriteString(loggingEvent.Level.Name);
                writer.WriteEndElement();

                writer.WriteStartElement("Reference");
                writer.WriteString(logObject.Reference);
                writer.WriteEndElement();

                writer.WriteStartElement("Url");
                writer.WriteString(logObject.Url);
                writer.WriteEndElement();

                writer.WriteStartElement("Date");
                writer.WriteString(logObject.Date.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Direction");
                writer.WriteString(logObject.Direction.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Request");
                writer.WriteString(messageJson);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

        }

        public static void InsertServerCallLog(object message, string reference, LogDirection direction, System.Web.HttpContext context)
        {
            if (GlobalConfiguration.IsServerSideLogEnabled)
            {
                Log logEntity = new Log();

                logEntity.ComputerName = Common.GetClientIp(context);
                logEntity.Date = DateTime.Now;
                logEntity.Message = message;
                logEntity.Reference = reference;
                logEntity.Url = HttpContext.Current.Request.UrlReferrer.ToString();
                logEntity.Direction = direction;

                log.Debug(logEntity);
            }
        }

        public static void InsertInfoLog(string message)
        {
            Log logEntity = new Log();

            //logEntity.ComputerName = HttpContext.Current.Request.UserHostName;
            logEntity.Date = DateTime.Now;
            logEntity.Message = message;
            logEntity.Reference = string.Empty;
            //logEntity.Url = HttpContext.Current.Request.Url.ToString();
            logEntity.Direction = LogDirection.None;

            log.Info(logEntity);
        }

        public static void InsertExceptionLog(Exception ex)
        {
            Log logEntity = new Log();

            //logEntity.ComputerName = HttpContext.Current.Request.UserHostName;
            logEntity.Date = DateTime.Now;
            logEntity.Message = ex;
            logEntity.Reference = string.Empty;
            //logEntity.Url = HttpContext.Current.Request.Url.ToString();
            logEntity.Direction = LogDirection.None;

            log.Fatal(logEntity);
        }

        public static void InsertUiErrorLog(string message)
        {
            Log logEntity = new Log();

            logEntity.Date = DateTime.Now;
            logEntity.Message = message;
            logEntity.Reference = string.Empty;
            logEntity.Url = HttpContext.Current.Request.UrlReferrer.ToString();
            logEntity.Direction = LogDirection.UIRequest;

            log.Error(logEntity);
        }
    }
}