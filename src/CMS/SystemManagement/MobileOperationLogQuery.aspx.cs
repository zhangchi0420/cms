using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Drision.Framework.Manager;
using System.Data.Common;
using Tension;
using Drision.Framework.Enum;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using System.Linq.Expressions;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using Drision.Framework.WorkflowEngine;
using Drision.Framework.Entity;
using Drision.Framework.OrgLibrary;
using System.Web.Script.Serialization;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class MobileOperationLogQuery : BasePage
    {
        public int PageIndex
        {
            get { return VS<int>("PageIndex"); }
            set { VS<int>("PageIndex", value); }
        }

        public int PageSize
        {
            get { return VS<int>("PageSize"); }
            set { VS<int>("PageSize", value); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.PageSize = this.gcProcessInstance.PagerSettings.PageSize;
                    this.PageIndex = this.gcProcessInstance.PagerSettings.PageIndex;
                    BindGrid();
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        protected void grid_PageChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex = e.PageIndex;
            this.PageSize = e.PageSize;
            BindGrid();
        }

        private void BindGrid()
        {
            try
            {
                using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
                {

                    var list = context.T_MobileOperationErrorLog.ToList();

                    var result = list.OrderBy(p => p.State).ThenByDescending(p => p.CreateTime).ToList();

                    this.gcProcessInstance.PagerSettings.DataCount = result.Count();
                    //绑定
                    if (result.Count() > this.PageIndex * this.PageSize)
                    {
                        result = result.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
                    }
                    else
                    {
                        this.PageIndex = 0;
                        this.gcProcessInstance.PagerSettings.PageIndex = 0;

                        result = result.Take(this.PageSize).ToList();
                    }
                    gcProcessInstance.DataSource = result;
                    gcProcessInstance.DataBind();
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gcProcessInstance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton btnCanExec = e.Row.FindControl("btnCanExec")  as LinkButton;
                    LinkButton btnComplete = e.Row.FindControl("btnComplete") as LinkButton;
                    LinkButton btnView = e.Row.FindControl("btnView") as LinkButton;
                    if (e.Row.DataItem != null)
                    {
                        int state = DataBinder.Eval(e.Row.DataItem, "State") == null ? -1 : Int32.Parse(DataBinder.Eval(e.Row.DataItem, "State").ToString());
                        int createUserId = DataBinder.Eval(e.Row.DataItem, "CreateUserId") == null ? -1 : Int32.Parse(DataBinder.Eval(e.Row.DataItem, "CreateUserId").ToString());
                        string record = DataBinder.Eval(e.Row.DataItem, "OperationRecord") == null ? string.Empty : DataBinder.Eval(e.Row.DataItem, "OperationRecord").ToString();
                        if (createUserId > 0)
                        {
                            gcProcessInstance.SetRowText(e.Row, "CreateUserId", OrgManager.GetDisplayValue("T_User", createUserId));
                        }
                        if (state > 0)
                        {
                            gcProcessInstance.SetRowText(e.Row, "State", EnumHelper.GetDescription(typeof(OperationErrorLogStateEnum), state));
                            if (state == (int)OperationErrorLogStateEnum.CanExec)
                            {
                                if (btnCanExec != null)
                                {
                                    btnCanExec.Visible = false;
                                }
                            }
                            if (state == (int)OperationErrorLogStateEnum.Finish)
                            {
                                if (btnCanExec != null)
                                {
                                    btnCanExec.Visible = false;
                                }
                                if (btnComplete != null)
                                {
                                    btnComplete.Visible = false;
                                }
                            }
                        }
                        if (record.Length > 30)
                        {
                            if (btnView != null)
                            {
                                btnView.Text = record.Substring(0, 30) + "...";
                                btnView.ToolTip = record;
                            }
                            gcProcessInstance.SetRowText(e.Row, "OperationRecord", record.Substring(0,30)+"...");
                            //e.Row.ToolTip = record;
                        }
                    }
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        /// <summary>
        /// 将日志状态置为“可再执行”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCanExec_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (sender as LinkButton).CommandArgument.ToInt();
                UpdateState(id, OperationErrorLogStateEnum.CanExec);
                BindGrid();
                this.AjaxAlert("修改成功！");
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        /// <summary>
        /// 将日志状态置为“完成”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (sender as LinkButton).CommandArgument.ToInt();
                UpdateState(id, OperationErrorLogStateEnum.Finish);
                BindGrid();
                this.AjaxAlert("修改成功！");
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        /// <summary>
        /// 更新日志状态
        /// </summary>
        /// <param name="id">日志ID</param>
        /// <param name="state">日志状态</param>
        void UpdateState(int id, OperationErrorLogStateEnum state)
        {
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                T_MobileOperationErrorLog log = context.T_MobileOperationErrorLog.FirstOrDefault(p => p.Id == id);
                if (log != null)
                {
                    log.StateEnum = state;
                    context.SaveChanges();
                }
            }
        }
    }
}