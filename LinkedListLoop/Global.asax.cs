using LinkedListLoop.entities;
using LinkedListLoop.src;
using LinkedListLoop.src.server;
using LinkedListLoop.src.server.entities;
using LinkedListLoop.src.server.entities.transaction_types;
using LinkedListLoop.src.server.entities.transaction_types.Cheque;
using LinkedListLoop.src.server.entities.transaction_types.EInvoice;
using LinkedListLoop.src.server.transaction_types.Cheque;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using WebMatrix.WebData;

namespace LinkedListLoop
{
    public class Global : System.Web.HttpApplication
    {
        protected log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start(object sender, EventArgs e)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            log4net.Config.XmlConfigurator.ConfigureAndWatch(fi);

            LogManager.InsertInfoLog("application start");

            SimpleMembershipInitializer();

            GlobalConfiguration.IsClientSideLogEnabled = true;
            GlobalConfiguration.IsServerSideLogEnabled = true;
            GlobalConfiguration.CurrentCulture = new CultureInfo(Constants.DefaultCulture);

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Host = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
            GlobalConfiguration.MenuList = Common.GetMenuItems();
            GlobalConfiguration.TransctionTypes = Common.GetDefinition<List<TransactionType>>(Constants.TransactionTypePath);

            GlobalConfiguration.TransactionProcessors = TransactionHelper.GetTransactionProcessors();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            LogManager.InsertInfoLog("application end");
        }

        private void SimpleMembershipInitializer()
        {
            Database.SetInitializer<UsersContext>(null);
            Database.SetInitializer<EntityContext>(null);

            try
            {
                using (var context = new UsersContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Create the SimpleMembership database without Entity Framework migration schema
                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();

                    }
                }

                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                UserManager.CreateAccessRolesIfNeccessary();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ResourceHelper.GetString("SingleMembershipError"), ex);
            }
        }
    }
}