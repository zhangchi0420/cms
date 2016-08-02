using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.WorkflowEngineCore;
using Drision.Framework.Common;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormInstanceApprove : BasePage
    {
        #region 属性

        public int WorkItemId
        {
            get { return VS<int>("WorkItemId"); }
            set { VS<int?>("WorkItemId",value); }
        }

        public int ProcessInstanceId
        {
            get { return VS<int>("ProcessInstanceId"); }
            set { VS<int>("ProcessInstanceId", value); }
        }

        public int FormInstanceId
        {
            get { return VS<int>("FormInstanceId"); }
            set { VS<int>("FormInstanceId", value); }
        }

        public long ActivityId
        {
            get { return VS<long>("ActivityId"); }
            set { VS<long>("ActivityId", value); }
        }

        public long ProcessId
        {
            get { return VS<long>("ProcessId"); }
            set { VS<long>("ProcessId", value); }
        }

        public int ActivityDispalyOrder
        {
            get { return VS<int>("ActivityDispalyOrder"); }
            set { VS<int>("ActivityDispalyOrder", value); }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }
                this.formPreview.LoadFormWithInstance(this.FormInstanceId,this.ActivityId);                
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndRedirect(ex, "FormInstanceQuery.aspx");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            int? id = QueryString<int?>("id");
            SysWorkItem wi = this.DataHelper.FindById<SysWorkItem>(id);
            if (wi == null)
            {
                throw new Exception("工作项不存在");
            }
            SysActivityInstance ai = this.DataHelper.FindById<SysActivityInstance>(wi.ActivityInstanceId);
            if (ai == null)
            {
                throw new Exception("活动实例不存在");
            }
            SysActivity activity = this.DataHelper.FindById<SysActivity>(ai.ActivityId);
            if (activity == null)
            {
                throw new Exception("活动不存在");
            }
            SysProcessInstance pi = this.DataHelper.FindById<SysProcessInstance>(ai.ProcessInstanceId);
            if (pi == null)
            {
                throw new Exception("流程实例不存在");
            }
            
            this.WorkItemId = id.Value;
            
            this.ProcessInstanceId = pi.ProcessInstanceId;
            this.ActivityId = activity.ActivityId;
            this.ActivityDispalyOrder = activity.DisplayOrder ?? 0;
            this.FormInstanceId = pi.FormInstanceId ?? 0;
            this.ProcessId = pi.ProcessId.Value;

            CheckRejectButton();
        }


        /// <summary>
        /// 处理驳回方式的按钮显示
        /// </summary>
        private void CheckRejectButton()
        {
            //找到当前活动驳回的连接线
            SysTransition t = this.DataHelper.Set<SysTransition>()
                .Where(p => p.PreActivityId == this.ActivityId && p.Direction == (int)FlowStepDirection.False).FirstOrDefault();
            if (t == null) //不存在，表示运行时用户指定驳回下一活动
            {
                var source = from a in this.DataHelper.Set<SysActivity>()
                             where a.ProcessId == this.ProcessId &&
                             ((a.ActivityType == (int)ActivityType.Approve && a.DisplayOrder < this.ActivityDispalyOrder)
                             || (a.ActivityType == (int)ActivityType.End && a.ActivityName == "驳回"))
                             orderby a.DisplayOrder
                             select new
                             {
                                 a.DisplayOrder,
                                 a.ActivityId,
                                 a.ActivityName,
                                 ButtonName = a.ActivityType == (int)ActivityType.End ? "驳回（结束流程）" : string.Format("驳回到（{0}）", a.ActivityName),
                             };

                this.rReject.DataSource = source;
                this.rReject.DataBind();

                this.rReject.Visible = true;
                this.btnReject.Visible = false;
            }
            else
            {
                this.rReject.Visible = false;
                this.btnReject.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccIsAddUser_CheckedChanged(object sender, EventArgs e)
        {
            this.scAddUserId.ReadOnly = !this.ccIsAddUser.Checked;
        }

        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPass_Click(object sender, EventArgs e)
        {
            try
            {
                bool isAddUser = this.ccIsAddUser.Checked;
                int? addUserId = this.scAddUserId.GetValue().ToStringNullable().ToIntNullable();
                string approveComment = this.txtApproveComment.Text.Trim();

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //更新表单实例
                        this.formPreview.UpdateFormInstance(db);

                        //完成工作项
                        EngineProxy proxy = new EngineProxy(db);
                        proxy.CompleteApproveWorkItem(this.WorkItemId, ApproveResultEnum.Pass, approveComment, isAddUser, addUserId);
                    }
                    ts.Complete();
                }

                Response.Redirect("FormInstanceQuery.aspx");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                bool isAddUser = this.ccIsAddUser.Checked;
                int? addUserId = this.scAddUserId.GetValue().ToStringNullable().ToIntNullable();
                string approveComment = this.txtApproveComment.Text.Trim();

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //更新表单实例
                        this.formPreview.UpdateFormInstance(db);

                        //完成工作项
                        EngineProxy proxy = new EngineProxy(db);
                        proxy.CompleteApproveWorkItem(this.WorkItemId, ApproveResultEnum.Reject, approveComment, isAddUser, addUserId);
                    }
                    ts.Complete();
                }

                Response.Redirect("FormInstanceQuery.aspx");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

        /// <summary>
        /// 驳回（指定下一活动）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTrueReject_Click(object sender, EventArgs e)
        {
            try
            {                
                string approveComment = this.txtApproveComment.Text.Trim();
                long nextActivityId = (sender as LinkButton).CommandArgument.ToLong();

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //更新表单实例
                        this.formPreview.UpdateFormInstance(db);

                        //完成工作项
                        EngineProxy proxy = new EngineProxy(db);
                        proxy.RejectApproveWorkItem(this.WorkItemId, approveComment, nextActivityId);
                    }
                    ts.Complete();
                }

                Response.Redirect("FormInstanceQuery.aspx");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }
}