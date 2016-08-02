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
using Drision.Framework.WorkflowEngineCore;
using Drision.Framework.Manager;
using Drision.Framework.WorkflowEngineCore.Cache;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormProcessInstanceDetail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                try
                {
                    if (Request.UrlReferrer != null)
                    {
                        URL = Request.UrlReferrer.ToString();
                    }
                    LoadData();
                    BindrpActivityInstance();
                }
                catch (Exception ex)
                {
                    this.AjaxAlert(ex);
                }
            }
        }

        public int ProcessInstanceId
        {
            get;
            set;
        }

        public SysProcessInstance PI { get; set; }
        public ProcessInstanceCacheFactory PICache { get; set; }

        public string TracerBackground
        {
            get;
            set;
        }

        public string URL
        {
            get { return (string)ViewState["url"]; }
            set { ViewState["url"] = value; }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        void LoadData()
        {
            this.TracerBackground = System.Configuration.ConfigurationManager.AppSettings[ConstKey.ProcessDetailBackground];
            if (string.IsNullOrEmpty(this.TracerBackground))
            {
                this.TracerBackground = "E8E8E8";
            }

            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                int piid = int.Parse(id);

                using (BizDataContext context = new BizDataContext())
                {
                    this.PICache = new ProcessInstanceCacheFactory(context);
                    this.PI = PICache.GetProcessInstanceCache(piid);

                    var data = this.PI;

                    lblProcessName.Text = data.Process.ProcessName;
                    lblFormName.Text = data.Process.Form.FormName;

                    var startUser = context.FindById<T_User>(data.StartUserId);
                    var startDept = context.FindById<T_Department>(data.StartDeptId);
                    if (startUser != null)
                    {
                        lblStartUser.Text = startUser.User_Name;
                    }
                    if (startDept != null)
                    {
                        lblStartDept.Text = startDept.Department_Name;
                    }

                    if (data.StartTime.HasValue)
                    {
                        lblStartTime.Text = data.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (data.EndTime.HasValue)
                    {
                        lblEndTime.Text = data.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    lblStatus.Text = GetStatusName(data.InstanceStatus);
                    this.ProcessInstanceId = data.ProcessInstanceId;                    
                }
            }
        }


        #region 这一块是历史审核记录

        /// <summary>
        /// 绑定第一层Repeater，这个是SysActivityInstance
        /// </summary>
        private void BindrpActivityInstance()
        {
            var pi = this.PI;

            if (pi != null && pi.Process.ProcessCategory == (int)ProcessCategory.FormApprove)
            {
                var result = pi.ActivityInstances.Where(p => p.Activity.ActivityType == (int)ActivityType.Approve)
                .OrderBy(p => p.StartTime)
                .Select(p => new
                {
                    p.StartTime,
                    ActivityId = p.ActivityId,
                    ActivityName = p.Activity.ActivityName,
                    ActivityInstanceId = p.ActivityInstanceId,
                    AI = p,
                }).ToList().Select(p => new
                {
                    p.AI,
                    p.StartTime,
                    p.ActivityId,
                    p.ActivityName,
                    p.ActivityInstanceId,
                    ApproveResult = GetCheckState(p.AI),
                }).Distinct();
                this.rpActivityInstance.DataSource = result.OrderBy(p => p.StartTime).ToList();
                this.rpActivityInstance.DataBind();
                this.divHistory.Visible = true;

                PICache.ClearCache(this.ProcessInstanceId);
            }
            else
            {
                this.divHistory.Visible = false;
            }
        }

        /// <summary>
        /// 绑定第二层Repeater，这个是SysWorkItemApproveGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpActivityInstance_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpApproveGroup = e.Item.FindControl("rpApproveGroup") as Repeater;
            var ai = e.Item.DataItem.GetPropertyValue("AI") as SysActivityInstance;

            //组里所有活动实例对应的活动都是本活动
            var result = ai.ApproveGroups
                .Select(p => new
                {
                    Group = p,
                    ApproveGroupId = p.ApproveGroupId,
                    ParticipantName = p.ActivityParticipantId == null ? "直接指定" : ai.Activity.ActivityParticipants.FirstOrDefault(i => i.ActivityParticipantId == p.ActivityParticipantId).ProcessParticipant.ParticipantName,
                    ApproveResult = p.ApproveResult,
                }).ToList()
                .Select(p => new
                {
                    p.Group,
                    p.ApproveGroupId,
                    p.ParticipantName,
                    ApproveResult = (ai.InstanceStatus == (int)ActivityInstanceStatusEnum.Completed && p.ApproveResult == null) ? "未参与审核" : GetCheckState(p.ApproveResult),
                });
            rpApproveGroup.DataSource = result;
            rpApproveGroup.DataBind();

        }

        /// <summary>
        /// 绑定第三层Repeater，这个是SysApproveActivityData（已经审核的）和SysWorkItem（未参与审核的）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpApproveGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpApproveActivity = e.Item.FindControl("rpApproveActivity") as Repeater;
            SysWorkItemApproveGroup group = e.Item.DataItem.GetPropertyValue("Group") as SysWorkItemApproveGroup;

            using (BizDataContext context = new BizDataContext())
            {
                var result = context.Where<SysApproveActivityData>(p => p.ApproveGroupId == group.ApproveGroupId)
                    .Select(p => new
                    {
                        p.ApproveTime,
                        p.WorkItemId,
                        User_Name = context.FindById<T_User>(p.ApproveUserId).User_Name,
                        ApproveResult = p.ApproveResult,
                        ApproveComment = p.ApproveComment,
                        AddingUser = p.IsAdded == true ? "[加签]" : "",
                        ProxyUser = p.IsProxy == true ? "[代理]" : "",
                    }).ToList().Select(p => new UserApproveComment()
                    {
                        WorkItemId = p.WorkItemId,
                        User_Name = p.User_Name,
                        ApproveComment = p.ApproveComment,
                        AddingUser = p.AddingUser,
                        ProxyUser = p.ProxyUser,
                        ApproveResult = GetCheckState(p.ApproveResult),
                        ApproveDate = string.Format("{0:MM/dd HH:mm}", p.ApproveTime),
                    }).ToList();

                var idList = result.Select(p => p.WorkItemId);
                var otherList = group.WorkItems.Where(p => !idList.Contains(p.WorkItemId));
                int? state = null;
                foreach (var other in otherList)
                {
                    result.Add(new UserApproveComment()
                    {
                        WorkItemId = other.WorkItemId,
                        User_Name = context.FindById<T_User>(other.OwnerId).User_Name,
                        ApproveComment = other.Status == (int)WorkItemStatus.CancelledBySystem ? "未参与审核" : GetCheckState(state),
                        AddingUser = string.Empty,
                        ProxyUser = string.Empty,
                        ApproveResult = other.Status == (int)WorkItemStatus.CancelledBySystem ? "未参与审核" : GetCheckState(state),
                    });
                }

                rpApproveActivity.DataSource = result;
                rpApproveActivity.DataBind();
            }
        }


        /// <summary>
        /// 活动实例的审核状态
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetCheckState(SysActivityInstance p)
        {
            if (p.InstanceStatus == (int)ActivityInstanceStatusEnum.Suspending)
            {
                return "审核中";
            }
            else
            {
                return GetCheckState(p.ApproveResult);
            }
        }

        /// <summary>
        /// 审核状态
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetCheckState(int? p)
        {
            if (p == null)
            {
                return "审核中";
            }
            else if (p == (int)Drision.Framework.Common.Workflow.ApproveResultEnum.Pass)
            {
                return "通过";
            }
            else
            {
                return "不通过";
            }
        }

        #endregion

        /// <summary>
        /// 绑定时将状态值转为状态名称
        /// </summary>
        /// <param name="StatusValue"></param>
        /// <returns></returns>
        public string GetStatusName(int StatusValue)
        {
            string StatusName = StatusValue.ToString();
            switch (StatusValue)
            {
                case (int)ProcessInstanceStatusEnum.Running: StatusName = "运行中"; break;
                case (int)ProcessInstanceStatusEnum.Suspending: StatusName = "挂起中"; break;
                case (int)ProcessInstanceStatusEnum.Completed: StatusName = "已完成"; break;
                case (int)ProcessInstanceStatusEnum.Cancelled: StatusName = "已取消"; break;
                default: break;
            }
            return StatusName;
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                Response.Redirect(URL);
            }
            else
            {
                RedirectToHomePage();
            }
        }
    }
}