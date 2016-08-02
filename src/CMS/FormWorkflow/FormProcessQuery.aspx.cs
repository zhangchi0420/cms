using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using System.Linq.Expressions;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormProcessQuery : BasePage
    {
        #region 属性

        protected int PageSize
        {
            get { return VS<int>("PageSize"); }
            set { VS<int>("PageSize", value); }
        }

        protected int PageIndex
        {
            get { return VS<int>("PageIndex"); }
            set { VS<int>("PageIndex", value); }
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialize()
        {
            this.ddlProcessState.DataSource = typeof(ProcessState);
            this.ddlProcessState.DataBind();

            this.txtFormName.SetValue(null);
            this.txtProcessName.SetValue(null);
            this.ddlProcessState.SetValue(((int)ProcessState.StartUsed).ToString());

            this.PageSize = this.gcProcess.PagerSettings.PageSize;
            this.PageIndex = this.gcProcess.PagerSettings.PageIndex;

            this.SortDirection = (int)System.Web.UI.WebControls.SortDirection.Ascending;
            this.SortField = "ProcessName";

            BindGrid();
        }
        
        private string GetDisplayText(SysForm p)
        {
            SysEntity entity = GetEntity(p.EntityId);
            if (entity != null)
            {
                return entity.DisplayText;
            }
            return string.Empty;
        }

        private string GetUserName(int? userId)
        {
            T_User user = this.DataHelper.FindById<T_User>(userId);
            if (user != null)
            {
                return user.User_Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void BindGrid()
        {
            //查询列
            var source = from p in this.DataHelper.Set<SysProcess>()
                         join f in this.DataHelper.Set<SysForm>()
                         on p.FormId equals f.FormId
                         where p.ProcessCategory == (int)ProcessCategory.FormApprove
                         select new
                         {
                             p.ProcessId,
                             p.ProcessName,
                             f.FormName,
                             f.EntityId,
                             ProcessEntity = GetDisplayText(f),
                             State = EnumHelper.GetDescription((ProcessState)p.ProcessStatus),
                             StateValue = p.ProcessStatus,
                             p.ProcessVersion,
                             p.ProcessStatus,
                             CanDelete = p.ProcessStatus == (int)ProcessState.Created,
                             CanStop = p.ProcessStatus == (int)ProcessState.StartUsed,
                             CanStart = p.ProcessStatus == (int)ProcessState.Stoped,
                         };

            //条件
            string formName = this.txtFormName.Text.Trim();
            string processName = this.txtProcessName.Text.Trim();
            int? processStatus = this.ddlProcessState.SelectedValue.ToIntNullable();
            if (!string.IsNullOrEmpty(formName))
            {
                source = source.Where(p => p.FormName.Contains(formName));
            }
            if (string.IsNullOrEmpty(processName))
            {
                source = source.Where(p => p.ProcessName.Contains(processName));
            }
            if (processStatus != null)
            {
                source = source.Where(p => p.ProcessStatus == processStatus);
            }

            //排序            
            dynamic orderFunc = CreateOrderFunc(source, this.SortField);
            if (!string.IsNullOrEmpty(this.SortField))
            {
                if (this.SortDirection == (int)System.Web.UI.WebControls.SortDirection.Ascending)
                {
                    source = Queryable.OrderBy(source, orderFunc);
                }
                else
                {
                    source = Queryable.OrderByDescending(source, orderFunc);
                }
            }

            //分页
            int totalCount = source.Count();
            this.gcProcess.PagerSettings.DataCount = totalCount;

            if (totalCount > this.PageSize * this.PageIndex)
            {
                this.gcProcess.DataSource = source.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
            }
            else
            {
                this.PageIndex = 0;
                this.gcProcess.PagerSettings.PageIndex = 0;
                this.gcProcess.DataSource = source.Take(this.PageSize).ToList();
            }
            this.gcProcess.DataBind();
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

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                long? processId = (sender as LinkButton).CommandArgument.ToLongNullable();                
                SysProcess process = this.DataHelper.FindById<SysProcess>(processId);
                FormProcessPublishHelper helper = new FormProcessPublishHelper(process);
                helper.DeleteProcess();                    
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 停用活动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                long? processId = (sender as LinkButton).CommandArgument.ToLongNullable();
                SysProcess process = this.DataHelper.FindById<SysProcess>(processId);
                FormProcessPublishHelper helper = new FormProcessPublishHelper(process);
                helper.StopProcess();
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 启用活动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                long? processId = (sender as LinkButton).CommandArgument.ToLongNullable();
                SysProcess process = this.DataHelper.FindById<SysProcess>(processId);
                FormProcessPublishHelper helper = new FormProcessPublishHelper(process);
                helper.StartProcess();
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gcProcess_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            try
            {
                this.PageIndex = e.PageIndex;
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 点击表头排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gcProcess_HeaderClick(object sender, GridPostBackEventArgs e)
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
    }
}