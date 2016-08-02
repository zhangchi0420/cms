using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Entity;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.Home
{
    public partial class HomeFastAccess : BaseUserControl
    {
        public T_User LoginUser { get { return (this.Page as BasePage).LoginUser; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //projectNum.InnerText = "(" + ProjectHelper.GetAuditProjectCount(this.LoginUser.User_ID, this.DataHelper) +
                //                       ")";
            }
        }
    }
}