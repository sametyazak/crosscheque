using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace LinkedListLoop.src.server
{
    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        {
            base.Load += new EventHandler(BasePage_Load);
        }

        private void BasePage_Load(object sender, EventArgs e)
        {
            CheckUserLogin();
            CheckUserAuth();
            PageLoad();
        }

        protected virtual void PageLoad()
        { 
        
        }

        private void CheckUserLogin()
        {
            if (!WebSecurity.IsAuthenticated)
            {
                Session["ReturnUrl"] = HttpContext.Current.Request.Url.AbsoluteUri;
                Response.Redirect(string.Format("~/app/user/Login.aspx?returnUrl={0}", HttpContext.Current.Request.Url.PathAndQuery));
            }
        }

        private void CheckUserAuth()
        {
            string url = string.Concat(GlobalConfiguration.Host, HttpContext.Current.Request.Url.AbsolutePath);
            MenuItem requestItem = GlobalConfiguration.MenuList.FirstOrDefault(a=>a.Link.ToLower() == url.ToLower() );

            if (requestItem != null &&  requestItem.IsLocal)
            {
                List<string> availableRoles = Common.GetAvailableRoles(requestItem.MinAccessLevel);
                List<string> userRoles = UserManager.GetUserRoles(WebSecurity.CurrentUserName).ToList();

                foreach (string role in availableRoles)
                {
                    if (userRoles.Contains(role))
                    {
                        return;
                    }
                }

                Response.Redirect("~/app/unauthorized.aspx");
            }
        }
    }
}