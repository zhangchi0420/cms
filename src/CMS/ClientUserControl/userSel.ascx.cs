using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Manager;
using Drision.Framework.Entity;
using Drision.Framework.Enum;
using Drision.Framework.Repository;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web
{
    public partial class userSel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BandSelect();
            }
        }

        private void BandSelect()
        {
            var context = (this.Page as BasePage).DataHelper;

            var temp = context.Where<T_User>(p => p.User_Status == 1).ToList();
            var users = temp.Select(
            (p) => new
            {
                p.User_ID,
                p.User_Name,
                Department_Name = p.Department_ID == null ? "无部门" : context.FindById<T_Department>(p.Department_ID).Department_Name,
            }).ToList();

            this.sc2.ListSettings.DataSource = users;
            this.sc2.ListSettings.DataValueField = "User_ID";
            this.sc2.ListSettings.DataTextField = "User_Name";
            this.sc2.ListSettings.DataGroupField = "Department_Name";
            this.sc2.DataBind();
        }
    }
}