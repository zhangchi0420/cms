using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.EntityLibrary.Report;
using Drision.Framework.Entity;
using Drision.Framework.Enum;

namespace Drision.Framework.Web.Report
{
    public partial class ReportMenuRoleSet : BasePage
    {
        public int __Id
        {
            get
            {
                return Convert.ToInt32(ViewState["__Id"]);
            }
            set
            {
                ViewState["__Id"] = value;
            }
        }
        private ReportHelper _reportHelper;
        private ReportHelper _ReportHelper
        {
            get
            {
                if (_reportHelper == null)
                {
                    _reportHelper = new ReportHelper(this.DataHelper, this.__Id, this.LoginUser);
                }
                return _reportHelper;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                try
                {
                    __Id = Request.QueryString["id"].ToInt();
                    InitData();
                }
                catch (Exception ex)
                {
                    this.AjaxAlert(ex);
                }
            }
        }

        private void InitData()
        {
            //父功能下拉框，即父功能为null的，模块
            this.ccParent.Items.Clear();
            var parentItems = this.DataHelper.Where<SysFunction>(p => p.Permission_Type == 0 || p.Permission_Type == null);
            foreach (var p in parentItems)
            {
                this.ccParent.Items.Add(new ComboItem()
                {
                    Text = p.Permission_Name,
                    Value = p.Function_ID.ToString(),
                });
            }            

            var _Roles = this.DataHelper.Where<T_Role>(p => p.Role_Status == (int)StopFlagEnum.No);
            var CheckRoles = this.DataHelper.Where<SysReportMenuRole>(p => p.ReportId == this.__Id);
            foreach (var _role in _Roles)
            {
                var _node = new TNode()
                {
                    Text = _role.Role_Name,
                    Value = _role.Role_ID.ToString(),
                };
                if (CheckRoles.FirstOrDefault(p => p.RoleId == _role.Role_ID) != null)
                {
                    _node.IsChecked = true;
                }
                this.tcRole.Nodes.Add(_node);
            }
        }


        protected void btnPre_Click(object sender, EventArgs e)
        {
            var model = this.DataHelper.FindById<SysReport>(this.__Id);
            var _url = string.Format("ReportDisplayFieldSet.aspx?id={0}", this.__Id);
            if (model.IsGraph == true)
            {
                _url = string.Format("ReportGraphSet.aspx?id={0}", this.__Id);
            }
            Response.Redirect(_url);
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                this._ReportHelper.CreateSQL();
                Response.Redirect(string.Format("ReportShow.aspx?id={0}&preview=true", this.__Id));
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tcRole.CheckedValue.Count == 0)
                {
                    this.AjaxAlert("请选择有权限的角色后发布！");
                    return;
                }
                this._ReportHelper.AddFunctionRoleAndPublish(this.tcRole, this.ccParent.SelectedValue.ToLongNullable());
                Response.Redirect("ReportQuery.aspx");
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}