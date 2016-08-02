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
    public partial class ReportChangeColumnSet : BasePage
    {
        private int __Id
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
                    var id = Request.QueryString["id"];
                    if (!string.IsNullOrEmpty(id))
                    {
                        __Id = id.ToInt();
                    }
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
            var _GroupFields = this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id);
            _GroupFields.ForEach(p =>
            {
                this.ccColumnFieldAliases.Items.Add(new ComboItem()
                {
                    Text = p.QueryFieldDisplayText,
                    Value = p.QueryFieldAliases,
                });
                this.ccRowFieldAliases.Items.Add(new ComboItem()
                {
                    Text = p.QueryFieldDisplayText,
                    Value = p.QueryFieldAliases,
                });
            });
            var _SumFields = this.DataHelper.Where<SysReportSumFiled>(p => p.ReportId == this.__Id);
            _SumFields.ForEach(p =>
            {
                this.ccValueFieldAliases.Items.Add(new ComboItem()
                {
                    Text = p.DisplayText,
                    Value = p.Aliases,
                });
            });
            var model = this.DataHelper.FirstOrDefault<SysReportChangeColumn>(p=>p.ReportId == this.__Id);
            if (model != null)
            {
                this.ccRowFieldAliases.SetValue(model.RowFieldAliases);
                this.ccColumnFieldAliases.SetValue(model.ColumnFieldAliases);
                this.ccValueFieldAliases.SetValue(model.ValueFieldAliases);
            }
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportHavingFieldSet.aspx?id={0}", this.__Id));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                var model = this.DataHelper.FirstOrDefault<SysReportChangeColumn>(p => p.ReportId == this.__Id);
                bool IsAdd = false;
                if (model == null)
                {
                    IsAdd = true;
                    model = new SysReportChangeColumn();
                    model.ChangeColumnId = this.DataHelper.GetNextIdentity_Int();
                    model.ReportId = this.__Id;
                }

                model.RowFieldAliases = this.ccRowFieldAliases.SelectedValue;
                model.RowFieldDisPlayText = this.ccRowFieldAliases.SelectedText;
                model.ColumnFieldAliases = this.ccColumnFieldAliases.SelectedValue;
                model.ValueFieldAliases = this.ccValueFieldAliases.SelectedValue;

                if (IsAdd)
                {
                    this.DataHelper.Insert(model);
                }
                else
                {
                    this.DataHelper.Update(model);
                }
                Response.Redirect(string.Format("ReportSortFieldSet.aspx?id={0}", this.__Id));
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }
}