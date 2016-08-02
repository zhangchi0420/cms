using System;
using System.Collections.Generic;
//using Drision.Framework.Meta;
using Drision.Framework.Entity;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
using Drision.Framework.Plugin.Web;
using System.Transactions;
using System.Web.UI;

namespace Drision.Framework.Web
{
    public partial class WebForm1 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.AjaxAlert("DDDDE");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}