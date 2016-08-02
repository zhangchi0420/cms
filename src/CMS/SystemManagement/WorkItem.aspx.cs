using System;
using System.Linq;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;

using Drision.Framework.OrgLibrary.InternalEntities;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class WorkItem : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.PageIndex_WorkItem = this.gcWorkItem.PagerSettings.PageIndex;
                    this.PageSize_WorkItem = this.gcWorkItem.PagerSettings.PageSize;

                    this.PageIndex_Schedule = this.gcSchedule.PagerSettings.PageIndex;
                    this.PageSize_Schedule = this.gcSchedule.PagerSettings.PageSize;

                    this.PageIndex_Remind = this.gcRemind.PagerSettings.PageIndex;
                    this.PageSize_Remind = this.gcRemind.PagerSettings.PageSize;

                    Query();
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        private string GetStartUserName(BizDataContext context, string wiId)
        {
            try
            {
                int id = wiId.ToInt();
                var wi = context.FindById<SysWorkItem>(id);
                var ai = context.FindById<SysActivityInstance>(wi.ActivityInstanceId);
                var pi = context.FindById<SysProcessInstance>(ai.ProcessInstanceId);
                var user = context.FindById<T_User>(pi.StartUserId);
                return user.User_Name;
            }
            catch
            {
                return null;
            }
        }

        #region 工作项

        public int PageIndex_WorkItem
        {
            get { return VS<int>("PageIndex_WorkItem"); }
            set { VS<int>("PageIndex_WorkItem", value); }
        }

        public int PageSize_WorkItem
        {
            get { return VS<int>("PageSize_WorkItem"); }
            set { VS<int>("PageSize_WorkItem", value); }
        }

        private void BindWorkItem()
        {
            using (BizDataContext context = new BizDataContext())
            {
                var wibList = context.Set<T_WorkItemBase>().Where(p =>
                    p.WorkItemTypeID == null && p.Entity_Trace_PlanID == null
                    && p.OwnerId == LoginUserID && p.State == (int)WorkItemStatus.Created).OrderByDescending(p=>p.CreateTime);

                var result = wibList.Skip(this.PageIndex_WorkItem * this.PageSize_WorkItem).Take(this.PageSize_WorkItem).ToList();

                var source = from wi in result
                             orderby wi.CreateTime descending
                             select new
                             {
                                 wi.Title,
                                 wi.CreateTime,
                                 wi.CompletePageUrl,
                                 wi.WorkItemId,
                                 wi.WorkItemBase_Id,
                                 User_Name = GetStartUserName(context, wi.WorkItemId),
                             };

                gcWorkItem.DataSource = source.ToList();
                gcWorkItem.PagerSettings.DataCount = wibList.Count();
                gcWorkItem.DataBind();
            }
        }

        protected void gcWorkItem_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex_WorkItem = e.PageIndex;
            this.PageSize_WorkItem = e.PageSize;
            BindWorkItem();
        }

        #endregion

        #region 计划任务

        public int PageIndex_Schedule
        {
            get { return VS<int>("PageIndex_Schedule"); }
            set { VS<int>("PageIndex_Schedule", value); }
        }

        public int PageSize_Schedule
        {
            get { return VS<int>("PageSize_Schedule"); }
            set { VS<int>("PageSize_Schedule", value); }
        }

        private void BindSchedule()
        {
            using (BizDataContext context = new BizDataContext())
            {
                var source = context.Set<T_WorkItemBase>().Where(p =>
                    p.WorkItemTypeID != null && p.Entity_Trace_PlanID != null
                    && p.OwnerId == LoginUserID && p.State == (int)WorkItemStatus.Created)
                    .OrderByDescending(p => p.CreateTime);

                var result = source.Skip(this.PageIndex_Schedule * this.PageSize_Schedule).Take(this.PageSize_Schedule).ToList();

                var list = result.Select(p => new
                {
                    p.WorkItemId,
                    p.CreateTime,
                    p.Title,
                    ProcessPageUrl = p.CompletePageUrl,
                    DeadLine = p.EndTime,
                }).ToList();
                gcSchedule.DataSource = list;
                gcSchedule.PagerSettings.DataCount = source.Count();
                gcSchedule.DataBind();
            }
        }

        protected void gcSchedule_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex_Schedule = e.PageIndex;
            this.PageSize_Schedule = e.PageSize;
            BindSchedule();
        }

        #endregion

        #region 提醒

        public int PageIndex_Remind
        {
            get { return VS<int>("PageIndex_Remind"); }
            set { VS<int>("PageIndex_Remind", value); }
        }

        public int PageSize_Remind
        {
            get { return VS<int>("PageSize_Remind"); }
            set { VS<int>("PageSize_Remind", value); }
        }

        private void BindRemind()
        {
            using (BizDataContext context = new BizDataContext())
            {
                var query = context.Set<SysRemind>().Where(p => p.State == (int)RemindStausEnum.New
                    && p.OwnerId == LoginUserID).OrderByDescending(p => p.CreateTime);

                var result = query.Skip(this.PageIndex_Remind * this.PageSize_Remind).Take(this.PageSize_Remind).ToList();

                var source = result.Select(p => new
                            {
                                p.RemindId,
                                p.RemindName,
                                p.RemindURL,
                                CreateUserName = GetUserName(context, p.CreateUserId),
                                p.CreateTime,
                                p.DeadLine,
                                RemindUrl = CreateUrl(p.RemindURL, p.RemindId),
                            }).ToList();

                gcRemind.DataSource = source;
                gcRemind.PagerSettings.DataCount = query.Count();
                gcRemind.DataBind();
            }
        }

        protected void gcRemind_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex_Remind = e.PageIndex;
            this.PageSize_Remind = e.PageSize;
            BindRemind();
        }

        #endregion

        private string GetUserName(BizDataContext context, int? id)
        {
            string result = null;
            if (id != null)
            {
                var user = context.FindById<T_User>(id);
                if (user != null)
                {
                    result = user.User_Name;
                }
            }
            return result;
        }

        private string CreateUrl(string source, int id)
        {
            string result = source;
            if (!string.IsNullOrEmpty(source))
            {
                source = this.ResolveClientUrl(source);
                if (source.Contains("?"))
                {
                    source += string.Format("&remindId={0}", id);
                }
                else
                {
                    source += string.Format("?remindId={0}", id);
                }
                result = source;
            }
            return result;
        }

        protected void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                string idStr = (sender as LinkButton).CommandArgument;
                int id = Convert.ToInt32(idStr);
                using (BizDataContext context = new BizDataContext())
                {
                    var remind = context.FindById<SysRemind>(id);
                    if (remind != null)
                    {
                        remind.State = (int)RemindStausEnum.Completed;
                        context.Update(remind);
                        BindRemind();
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
        protected void Query()
        {
            try
            {
                BindWorkItem();
                BindSchedule();
                //BindRemind();
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }        

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn = sender as LinkButton;

                Response.Redirect(lbtn.CommandArgument);
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }
    }
}