using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Manager;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web
{
    public partial class Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(AppSettings.HomePageUrl);
        }
    }
}