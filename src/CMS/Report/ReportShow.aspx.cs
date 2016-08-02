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
using System.Data;
using System.Web.UI.DataVisualization.Charting;

namespace Drision.Framework.Web.Report
{
    public partial class ReportShow : BasePage
    {
        private int ReportId
        {
            get
            {
                var _ReportId = 0;
                int.TryParse(ViewState["_ReportId"].ToStringNullable(), out _ReportId);
                return _ReportId;
            }
            set
            {
                ViewState["_ReportId"] = value;
            }
        }

        private string PageTitle
        {
            get
            {
                return ViewState["_PageTitle"].ToStringNullable();
            }
            set
            {
                ViewState["_PageTitle"] = value;
            }
        }
        private ReportHelper _reportHelper;
        private ReportHelper _ReportHelper
        {
            get
            {
                if (_reportHelper == null)
                {
                    _reportHelper = new ReportHelper(this.DataHelper, this.ReportId, this.LoginUser);
                }
                return _reportHelper;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                var _ReportId = 0;
                if (!int.TryParse(Request.QueryString["id"], out _ReportId))
                {
                    this.AjaxAlert("请传入正确的ReportId！");
                    return;
                }
                this.ReportId = _ReportId;
                InitPageControl();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        private void InitPageControl()
        {
            SysReport model;
            if (Request.QueryString["preview"] == "true")
            {
                this.btnReturn.Visible = true;
                model = this.DataHelper.FirstOrDefault<SysReport>(p => p.ReportId == this.ReportId && p.ReportModel == (int)ReportModelEnum.Page_Statistics);
            }
            else
            {
                model = this.DataHelper.FirstOrDefault<SysReport>(p => p.ReportId == this.ReportId && p.ReportModel == (int)ReportModelEnum.Page_Statistics && p.State == (int)ReportStateEnum.Published);
            }
            if (model == null)
            {
                throw new Exception("传入的ReportId找不到页面统计类型的报表定义！");
            }
            //页面标题
            this.Title = model.ReportName;
            this.PageTitle = model.ReportName;
            //查询条件
            this._ReportHelper.CreateQueryCondition(this.divQueryCondition);
            if (!this._ReportHelper.IsHaveGraph())
            {
                this.divChart.Visible = false;
                this.btnExportChart.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.btnExportList);
                ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.btnExportChart);
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        public override void Refresh()
        {
            var _Result = this._ReportHelper.GetGroupSumData(this.divQueryCondition);
            if (_Result.Rows.Count == 0)
            {
                divEmptyData.Visible = true;
                divData.Visible = false;
            }
            else
            {
                divEmptyData.Visible = false;
                divData.Visible = true;
            }
            this._ReportHelper.ReportGridBind(this.gcList, _Result);
            this._ReportHelper.ReportChartBind(this.chReport, _Result);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Refresh();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void chReport_Click(object sender, ImageMapEventArgs e)
        {
            try
            {
                if (this._ReportHelper.IsPie())
                {
                    Refresh();

                    int pointIndex = int.Parse(e.PostBackValue);
                    Series series = this.chReport.Series[0];
                    if (pointIndex >= 0 && pointIndex < series.Points.Count)
                    {
                        series.Points[pointIndex].CustomProperties += "Exploded=true";
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportMenuRoleSet.aspx?id={0}", this.ReportId));
        }

        protected void btnExportList_Click(object sender, EventArgs e)
        {
            Refresh();
            this.gcList.ToExcel(string.Format("{0}.xls",this.PageTitle));
        }
        protected void btnExportChart_Click(object sender, EventArgs e)
        {
            Refresh();
            this.chReport.ExportChart(string.Format("{0}.jpg", this.PageTitle));
        }
        /// <summary>
        /// 重写了这个，什么都不用干，就能解决 类型“GridView”的控件“ctl00_ContentPlaceHolder1_gcList”必须放在具有 runat。。。的错误
        /// </summary>
        /// <param name="control"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            
        }
    }
}