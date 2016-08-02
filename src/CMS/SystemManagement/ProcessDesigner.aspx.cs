using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class ProcessDesigner : BasePage
    {
        public long ProcessId
        {
            get { return VS<long>("ProcessId"); }
            set { VS<long>("ProcessId", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ProcessId = QueryString<long>("id");
            }
        }
    }
}