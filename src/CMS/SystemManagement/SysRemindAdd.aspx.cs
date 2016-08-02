using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.OrgLibrary.InternalEntities;
using Drision.Framework.Common;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class SysRemindAdd : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            BindUser();
        }

        private void BindUser()
        {
            var context = this.DataHelper;

            var temp = context.Where<T_User>(p => p.User_Status == 1).ToList();
            var users = temp.Select(
            (p) => new
            {
                p.User_ID,
                p.User_Name,
                Department_Name = p.Department_ID == null ? "无部门" : context.FindById<T_Department>(p.Department_ID).Department_Name,
            }).ToList();

            this.scReceiveUser.ListSettings.DataSource = users;
            this.scReceiveUser.ListSettings.DataValueField = "User_ID";
            this.scReceiveUser.ListSettings.DataTextField = "User_Name";
            this.scReceiveUser.ListSettings.DataGroupField = "Department_Name";
            this.scReceiveUser.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string idStr in this.scReceiveUser.SelectedValues)
                {
                    int? id = idStr.ToIntNullable();
                    if (id != null)
                    {
                        SysRemind remind = new SysRemind()
                        {
                            RemindName = this.txtTitle.Text.Trim(),
                            Content = this.txtContent.Value,
                            CreateUserId = this.LoginUserID,
                            OwnerId = id.Value,
                            CreateTime = DateTime.Now,
                            State = (int)RemindStausEnum.New,
                        };
                        this.RemindHelper.Save(remind);
                    }
                }
                Response.Redirect("../SystemManagement/SysRemindManagement.aspx");
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}