using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.EntityLibrary.Report;

namespace Drision.Framework.Web.Report
{
    public partial class ReportGroupFieldSet : BasePage
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
            var _QueryFields = this.DataHelper.Where<SysReportQueryField>(p => p.ReportId == this.__Id);
            _QueryFields.ForEach(p =>
            {
                this.ccQueryField.Items.Add(new ComboItem()
                {
                    Text = p.DisplayText,
                    Value = p.Aliases,
                });
            });
            //删除垃圾字段
            this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id)
                .ForEach(p =>
                {
                    if (_QueryFields.FirstOrDefault(q => q.Aliases == p.QueryFieldAliases) == null)
                    {
                        this.DataHelper.Delete(p);
                    }
                });
            InitGrid();
        }

        private void InitGrid()
        {
            var result = this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id);
            this.gcList.DataSource = result;
            this.gcList.DataBind();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var model = new SysReportGroupField();
                model.GroupFieldId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;

                model.QueryFieldAliases = this.ccQueryField.SelectedValue;
                model.QueryFieldDisplayText = this.ccQueryField.SelectedText;
                this.DataHelper.Insert(model);
                InitGrid();
                this.AjaxAlertAndEnableButton(string.Empty);
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (sender as LinkButton).CommandArgument.ToInt();
                var model = this.DataHelper.FindById<SysReportGroupField>(id);
                this.DataHelper.Delete(model);
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportQueryConditionSet.aspx?id={0}", this.__Id));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportSumFiledSet.aspx?id={0}", this.__Id));
        }

    }
}