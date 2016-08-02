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
    public partial class ReportGraphSet : BasePage
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
            this.ccChartType.DataSource = typeof(ReportChartTypeEnum);
            this.ccChartType.DataBind();
            var _GroupFields = this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id);
            //如果行转列，转行的列不能作为横轴和多组系列
            var _Report = this.DataHelper.FindById<SysReport>(this.__Id);
            if (_Report.IsChangeColumn == true)
            {
                var _ReportChangeColumn = this.DataHelper.FirstOrDefault<SysReportChangeColumn>(p => p.ReportId == this.__Id);
                _GroupFields.RemoveAll(p => p.QueryFieldAliases == _ReportChangeColumn.ColumnFieldAliases);
            }
            _GroupFields.ForEach(p =>
            {
                this.ccXMember.Items.Add(new ComboItem()
                {
                    Text = p.QueryFieldDisplayText,
                    Value = p.QueryFieldAliases,
                });
                this.ccSeriesMember.Items.Add(new ComboItem()
                {
                    Text = p.QueryFieldDisplayText,
                    Value = p.QueryFieldAliases,
                });
            });
            var _SumFields = this.DataHelper.Where<SysReportSumFiled>(p => p.ReportId == this.__Id);
            _SumFields.ForEach(p =>
            {
                this.ccYMember.Items.Add(new ComboItem()
                {
                    Text = p.DisplayText,
                    Value = p.Aliases,
                });
            });
            var model = this.DataHelper.FirstOrDefault<SysReportGraph>(p => p.ReportId == this.__Id);
            if (model != null)
            {
                this.ccChartType.SetValue(model.ChartType);
                this.ccXMember.SetValue(model.XMember);
                this.ccYMember.SetValue(model.YMember);
                this.ccSeriesMember.SetValue(model.SeriesMember);
            }
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportDisplayFieldSet.aspx?id={0}", this.__Id));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                //如果是行转列，显示为行的字段只能是显示为行的字段
                var _Report = this.DataHelper.FindById<SysReport>(this.__Id);
                if (_Report.IsChangeColumn == true)
                {
                    var _ReportChangeColumn = this.DataHelper.FirstOrDefault<SysReportChangeColumn>(p => p.ReportId == this.__Id);
                    if (_ReportChangeColumn.RowFieldAliases != this.ccXMember.SelectedValue)
                    {
                        this.AjaxAlertAndEnableButton("因为配置了行转列，只能用显示为行的列显示为横轴！");
                        return;
                    }
                }
                var model = this.DataHelper.FirstOrDefault<SysReportGraph>(p => p.ReportId == this.__Id);
                bool IsAdd = false;
                if (model == null)
                {
                    IsAdd = true;
                    model = new SysReportGraph();
                    model.ReportGraphId = this.DataHelper.GetNextIdentity_Int();
                    model.ReportId = this.__Id;
                }

                model.ChartType = this.ccChartType.SelectedValue.ToInt();
                model.SeriesMember = this.ccSeriesMember.SelectedValue;
                if (model.ChartType == (int)ReportChartTypeEnum.Pie)
                {
                    var _GroupFields = this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id);
                    if (_GroupFields.Count > 1)
                    {

                        this.AjaxAlertAndEnableButton("多个分组字段不能显示饼图！");
                        return;
                    }
                    if (!string.IsNullOrEmpty(model.SeriesMember))
                    {
                        this.AjaxAlertAndEnableButton("饼状图不能显示多组图！");
                        return;
                    }
                }
                model.XMember = this.ccXMember.SelectedValue;
                model.YMember = this.ccYMember.SelectedValue;


                if (IsAdd)
                {
                    this.DataHelper.Insert(model);
                }
                else
                {
                    this.DataHelper.Update(model);
                }
                Response.Redirect(string.Format("ReportMenuRoleSet.aspx?id={0}", this.__Id));
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }
}