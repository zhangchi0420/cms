using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.Home
{
    public partial class HomePage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                (this.Master as Site).FindControl("linkPlus").Visible = false;
            }
        }
    }
}