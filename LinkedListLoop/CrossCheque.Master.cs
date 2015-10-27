using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebMatrix.WebData;

namespace LinkedListLoop
{
    public partial class CrossCheque : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RootAddress.Value = GlobalConfiguration.Host;
        }

        public string UserName { get { return WebSecurity.CurrentUserName; } }

    }
}