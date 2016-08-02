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
    public partial class ReportHavingFieldSet : BasePage
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
            EnumHelper.GetEnumItems(typeof(ReportCompareTypeEnum)).ForEach(p =>
            {
                if (p.Value >= 1 && p.Value <= 6)
                {
                    this.ccCompareType.Items.Add(new ComboItem() { Text = p.Description, Value = p.Value.ToString() });
                }
            });
            EnumHelper.GetEnumItems(typeof(FilterTypeEnum)).ForEach(p =>
            {
                if (p.Value >= 0 && p.Value <= 1)
                {
                    this.ccHavingRelation.Items.Add(new ComboItem() { Text = p.Description, Value = p.Value.ToString() });
                }
            });

            var model = this.DataHelper.FindById<SysReport>(this.__Id);
            this.ccHavingRelation.SetValue(model.HavingRelation);
            this.ccIsChangeColumn.SetValue(model.IsChangeColumn);
            var _SumFields = this.DataHelper.Where<SysReportSumFiled>(p => p.ReportId == this.__Id);
            _SumFields.ForEach(p =>
            {
                this.ccSumField.Items.Add(new ComboItem()
                {
                    Text = p.DisplayText,
                    Value = p.Aliases,
                });
            });
            //删除垃圾字段,在汇总字段中没找到就算垃圾字段，因为二次过滤只能是汇总字段
            this.DataHelper.Where<SysReportHavingField>(p => p.ReportId == this.__Id)
                .ForEach(p =>
                {
                    if (_SumFields.FirstOrDefault(q => q.Aliases == p.SumFieldAliases) == null)
                    {
                        this.DataHelper.Delete(p);
                    }
                });
            InitGrid();
        }

        private void InitGrid()
        {
            var result = this.DataHelper.Where<SysReportHavingField>(p => p.ReportId == this.__Id)
                .Select(p => new
                {
                    p.HavingFieldId,
                    p.SumFieldAliases,
                    p.SumFieldDisplayText,
                    CompareType = EnumHelper.GetDescription(typeof(ReportCompareTypeEnum), p.CompareType.Value),
                    p.CompareValue,
                }).ToList();
            this.gcList.DataSource = result;
            this.gcList.DataBind();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var model = new SysReportHavingField();
                model.HavingFieldId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                model.SumFieldAliases = this.ccSumField.SelectedValue;
                model.SumFieldDisplayText = this.ccSumField.SelectedText;
                model.CompareType = this.ccCompareType.SelectedValue.ToInt();
                model.CompareValue = this.tcCompareValue.Text.Trim().ToDecimal();
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
                var model = this.DataHelper.FindById<SysReportHavingField>(id);
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
            Response.Redirect(string.Format("ReportSumFiledSet.aspx?id={0}", this.__Id));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                //当分组列小于2个时不能行转列
                if (this.ccIsChangeColumn.Checked && this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id).Count < 2)
                {
                    this.AjaxAlertAndEnableButton("分组字段必须多于两个才能行转列！");
                    return;
                }
                var model = this.DataHelper.FindById<SysReport>(this.__Id);
                model.HavingRelation = this.ccHavingRelation.SelectedValue.ToInt();
                model.IsChangeColumn = this.ccIsChangeColumn.Checked;
                this.DataHelper.UpdatePartial(model, p => new { p.HavingRelation, p.IsChangeColumn });
                var _url = string.Format("ReportSortFieldSet.aspx?id={0}", this.__Id);
                if (this.ccIsChangeColumn.Checked)
                {
                    _url = string.Format("ReportChangeColumnSet.aspx?id={0}", this.__Id);
                }
                Response.Redirect(_url);
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

    }
}