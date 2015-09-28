using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LinkedListLoop
{
    public partial class FileDownload : System.Web.UI.Page
    {
        public string SampleDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\cc_sample_data.csv");

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}