using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Entity;
using Drision.Framework.Common;
using Drision.Framework.WorkflowEngineCore;
using System.Linq.Expressions;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormProcessInstanceQuery : BasePage
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

        protected string SortField
        {
            get { return VS<string>("SortField"); }
            set { VS<string>("SortField", value); }
        }

        protected int SortDirection
        {
            get { return VS<int>("SortDeirection"); }
            set { VS<int>("SortDeirection", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    LoadDropDown();

                    this.PageSize = this.gcProcessInstance.PagerSettings.PageSize;
                    this.PageIndex = this.gcProcessInstance.PagerSettings.PageIndex;
                    this.SortDirection = (int)System.Web.UI.WebControls.SortDirection.Descending;
                    this.SortField = "StartTime";

                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        private void LoadDropDown()
        {
            try
            {
                using (BizDataContext context = new BizDataContext())
                {
                    var processList = context.Where<SysProcess>(p =>
                        p.ProcessStatus == (int)ProcessState.StartUsed
                        && p.ProcessCategory == (int)ProcessCategory.FormApprove
                        );

                    cbProcess.DataTextField = "ProcessName";
                    cbProcess.DataValueField = "ProcessType";
                    cbProcess.DataSource = processList;
                    cbProcess.DataBind();

                    cbStartUser.DataTextField = "User_Name";
                    cbStartUser.DataValueField = "User_ID";
                    cbStartUser.DataSource = context.FetchAll<T_User>();
                    cbStartUser.DataBind();

                    cbStartUser.SetValue(this.LoginUserID.ToString());
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        protected void grid_HeaderClick(object sender, GridPostBackEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Parameter))
                {
                    this.SortField = e.Parameter;
                }
                else
                {
                    this.SortField = e.FieldName;
                }
                this.SortDirection = (int)e.Direction;

                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void grid_PageChanging(object sender, GridPostBackEventArgs e)
        {
            try
            {
                this.PageIndex = e.PageIndex;
                this.PageSize = e.PageSize;
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void BindGrid()
        {
            var result = from pi in this.DataHelper.Set<SysProcessInstance>()
                         join p in this.DataHelper.Set<SysProcess>()
                         on pi.ProcessId equals p.ProcessId
                         join f in this.DataHelper.Set<SysForm>()
                         on p.FormId equals f.FormId
                         join fi in this.DataHelper.Set<SysFormInstance>()
                         on pi.FormInstanceId equals fi.FormInstanceId
                         join u in this.DataHelper.Set<T_User>()
                         on pi.StartUserId equals u.User_ID
                         join d in this.DataHelper.Set<T_Department>()
                         on pi.StartDeptId equals d.Department_ID
                         where (p.ProcessStatus == (int)ProcessState.StartUsed || p.ProcessStatus == (int)ProcessState.Updated)
                         && p.ProcessCategory == (int)ProcessCategory.FormApprove
                         orderby pi.StartTime descending
                         select new
                         {
                             pi.ProcessInstanceId,
                             p.ProcessName,
                             p.ProcessType,
                             pi.StartUserId,
                             pi.FormInstanceId,
                             u.User_Name,
                             f.FormName,
                             fi.FormCode,
                             d.Department_Name,
                             pi.StartTime,
                             pi.EndTime,
                             pi.InstanceStatus,
                             pi.ApproveResult,
                             p.EntityId
                         };
            //加上条件
            var list = result.Where(p => p.FormCode.Contains(this.txtFormCode.Text.Trim()));
            if (cbStartUser.SelectedValue != null)
            {
                int startUserId = Int32.Parse(cbStartUser.SelectedValue);
                list = list.Where(p => p.StartUserId == startUserId);
            }
            if (cbStatus.SelectedValue != null)
            {
                int stateId = Int32.Parse(cbStatus.SelectedValue);
                list = list.Where(p => p.InstanceStatus == stateId);
            }
            if (dtStartTime1.Text != "")
            {
                DateTime st1 = DateTime.Parse(dtStartTime1.Text);
                list = list.Where(p => p.StartTime > st1);
            }
            if (dtStartTime2.Text != "")
            {
                DateTime st2 = DateTime.Parse(dtStartTime2.Text);
                list = list.Where(p => p.StartTime < st2);
            }
            if (dtEndTime1.Text != "")
            {
                DateTime et1 = DateTime.Parse(dtEndTime1.Text);
                list = list.Where(p => p.EndTime > et1);
            }
            if (dtEndTime2.Text != "")
            {
                DateTime et2 = DateTime.Parse(dtEndTime2.Text);
                list = list.Where(p => p.EndTime < et2);
            }
            if (cbProcess.SelectedValue != null)
            {
                long typeid = long.Parse(cbProcess.SelectedValue);
                list = list.Where(p => p.ProcessType == typeid);
            }

            //排序            
            dynamic orderFunc = CreateOrderFunc(list, this.SortField);
            if (!string.IsNullOrEmpty(this.SortField))
            {
                if (this.SortDirection == (int)System.Web.UI.WebControls.SortDirection.Ascending)
                {
                    list = Queryable.OrderBy(list, orderFunc);
                }
                else
                {
                    list = Queryable.OrderByDescending(list, orderFunc);
                }
            }


            int count = list.Count();
            this.gcProcessInstance.PagerSettings.DataCount = count;
            var source = list;
            //绑定
            if (count > this.PageIndex * this.PageSize)
            {
                source = list.Skip(this.PageIndex * this.PageSize).Take(this.PageSize);
            }
            else
            {
                this.PageIndex = 0;
                this.gcProcessInstance.PagerSettings.PageIndex = 0;
                source = list.Take(this.PageSize);
            }

            this.gcProcessInstance.DataSource = source.ToList();
            gcProcessInstance.DataBind();
        }

        /// <summary>
        /// 排序Lambda
        /// </summary>        
        protected LambdaExpression CreateOrderFunc(dynamic source, string fieldName)
        {
            Type type = source.GetType().GetGenericArguments()[0];
            ParameterExpression p = Expression.Parameter(type, "p");
            MemberExpression member = Expression.Property(p, fieldName);
            LambdaExpression lambda = Expression.Lambda(member, p);
            return lambda;
        }

        protected void gcProcessInstance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        int state = Int32.Parse(DataBinder.Eval(e.Row.DataItem, "InstanceStatus").ToString());
                        string stateStr = string.Empty;
                        switch (state)
                        {
                            case (int)ProcessInstanceStatusEnum.Running:
                                stateStr = "运行中";
                                break;
                            case (int)ProcessInstanceStatusEnum.Suspending:
                                stateStr = "挂起中";
                                break;
                            case (int)ProcessInstanceStatusEnum.Completed:
                                stateStr = "已完成";
                                break;
                            case (int)ProcessInstanceStatusEnum.Cancelled:
                                stateStr = "已取消";
                                break;
                        }
                        if (DataBinder.Eval(e.Row.DataItem, "ApproveResult") != null)
                        {
                            int approveResult = Int32.Parse(DataBinder.Eval(e.Row.DataItem, "ApproveResult").ToString());

                            string approveResultStr = EnumHelper.GetDescription(typeof(Drision.Framework.Common.Workflow.ApproveResultEnum), approveResult);
                            gcProcessInstance.SetRowText(e.Row, "ApproveResult", approveResultStr);
                        }
                        gcProcessInstance.SetRowText(e.Row, "InstanceStatus", stateStr);                       
                    }
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
            try
            {
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 清空条件
        /// </summary>
        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            try
            {
                txtFormCode.Text = "";
                cbProcess.SelectedValue = null;
                cbStartUser.SelectedValue = null;
                cbStatus.SelectedValue = null;
                dtStartTime1.Text = "";
                dtStartTime2.Text = "";
                dtEndTime1.Text = "";
                dtEndTime2.Text = "";
                
                //查询数据
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}