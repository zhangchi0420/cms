using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Repository.EF;
using Drision.Framework.OrgLibrary;
using Drision.Framework.Common.Workflow;

namespace Drision.Framework.Web
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Error"] != null)
                {
                    divErrorMsg.InnerText = Session["Error"].ToString();
                }
            }
        }               
    }
}