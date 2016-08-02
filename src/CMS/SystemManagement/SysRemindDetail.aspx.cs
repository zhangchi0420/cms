using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.OrgLibrary.InternalEntities;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class SysRemindDetail : BasePage
    {
        public int RemindId
        {
            get
            {
                return VS<int>("id");
            }
            set
            {
                VS<int>("id", value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int? id = Request.Params["id"].ToIntNullable();
                if (id != null)
                {
                    this.RemindId = id.Value;
                    Initialize();
                }
            }
        }

        private void Initialize()
        {
            var remind = this.DataHelper.FindById<SysRemind>(this.RemindId);
            if (remind != null)
            {
                this.lblCreateTime.Text = Convert.ToString(remind.CreateTime);
                this.lblCreateUser.Text = GetUserName(remind.CreateUserId);
                this.lblOwner.Text = GetUserName(remind.OwnerId);
                this.lblTitle.Text = remind.RemindName;

                this.txtContent.Value = remind.Content;

                remind.State = (int)RemindStausEnum.Completed;
                this.DataHelper.UpdatePartial<SysRemind>(remind, p => new { p.State });
            }
        }

        private string GetUserName(int? userid)
        {
            var user = this.DataHelper.FindById<T_User>(userid);
            return user != null ? user.User_Name : string.Empty;
        }
    }
}