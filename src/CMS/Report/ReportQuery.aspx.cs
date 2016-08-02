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
    public partial class ReportQuery : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
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

            this.ccState.DataSource = typeof(ReportStateEnum);
            this.ccState.DataBind();

            this.ccReportModel.DataSource = typeof(ReportModelEnum);
            this.ccReportModel.DataBind();

            InitGrid();
        }
        private void InitGrid()
        {

            var result = this.DataHelper.Set<SysReport>().Where(p => p.State != 9999);

            var _tcReportName = tcReportName.Text;
            if (!string.IsNullOrEmpty(_tcReportName))
            {
                result = result.Where(p => p.ReportName.Contains(_tcReportName));
            }

            var _ReportModel = ccReportModel.SelectedValue;
            if (!string.IsNullOrEmpty(_ReportModel))
            {
                result = result.Where(p => p.ReportModel == _ReportModel.ToInt());
            }

            var _State = ccState.SelectedValue;
            if (!string.IsNullOrEmpty(_State))
            {
                result = result.Where(p => p.State == _State.ToInt());
            }

            this.gcList.PagerSettings.DataCount = result.Count();
            this.gcList.DataSource = result
                .Skip(this.gcList.PagerSettings.PageIndex * this.gcList.PagerSettings.PageSize)
                .Take(this.gcList.PagerSettings.PageSize)
                .OrderByDescending(p => p.ReportName)
                .Select(p => new
                {
                    p.ReportId,
                    p.ReportName,
                    p.EntityDisPlayText,
                    State = p.State == null ? "" : EnumHelper.GetDescription(typeof(ReportStateEnum), p.State.Value),
                    ReportModel = p.ReportModel == null ? "" : EnumHelper.GetDescription(typeof(ReportModelEnum), p.ReportModel.Value),
                }).ToList();
            this.gcList.DataBind();

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.tcReportName.SetValue(null);
                this.ccReportModel.SetValue(null);
                this.ccState.SetValue(null);
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        
        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
            try
            {
                var _id = (sender as LinkButton).CommandArgument.ToInt();

                var model = this.DataHelper.FindById<SysReport>(_id);
                model.State = (int)ReportStateEnum.NotPublish;
                this.DataHelper.UpdatePartial<SysReport>(model, p => new { p.State });
                //删除菜单,清除MenuId
                var _reportHelper = new ReportHelper(this.DataHelper, _id, this.LoginUser);
                _reportHelper.RemoveFunctionRole();
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void lbtnDel_Click(object sender, EventArgs e)
        {
            try
            {
                var _id = (sender as LinkButton).CommandArgument.ToInt();
                var model = this.DataHelper.FindById<SysReport>(_id);
                model.State = 9999;
                this.DataHelper.UpdatePartial<SysReport>(model, p => new { p.State });
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void gcList_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            try
            {
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void gcList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lbtnDisable = e.Row.FindControl("lbtnDisable") as LinkButton;
                    LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                    LinkButton lbtnDel = e.Row.FindControl("lbtnDel") as LinkButton;

                    var State = DataBinder.Eval(e.Row.DataItem, "State");
                    if (State != null && State.ToString() == "已发布")
                    {
                        lbtnDisable.Visible = true;
                        lbtnEdit.Visible = false;
                        lbtnDel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Report/ReportBaseSet.aspx?id={0}", (sender as LinkButton).CommandArgument));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.DataHelper.DatabaseType == LiteQueryDef.DatabaseTypeEnum.Oracle)
            {
                this.AjaxAlert("报表配置暂不支持Oracle数据库！");
                return;
            }
            Response.Redirect("ReportBaseSet.aspx");
        }
    }
}