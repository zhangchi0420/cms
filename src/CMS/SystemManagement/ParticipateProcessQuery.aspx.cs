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
//using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using Drision.Framework.WorkflowEngineCore;


namespace Drision.Framework.Web.SystemManagement
{
    public partial class ParticipateProcessQuery : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.PageIndex = this.gcProcessInstance.PagerSettings.PageIndex;
                    this.PageSize = this.gcProcessInstance.PagerSettings.PageSize;
                    
                    //空条件查询（相当于重置）
                    btnQuery_Click(null, null);
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

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

        protected void grid_PageChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex = e.PageIndex;
            this.PageSize = e.PageSize;
            BindGrid();
        }

        /// <summary>
        /// 2013-1-5 zhumin 重写查询，提速
        /// </summary>
        private void BindGrid()
        {
            var result = from wi in this.DataHelper.Set<SysWorkItem>()
                         join ai in this.DataHelper.Set<SysActivityInstance>()
                         on wi.ActivityInstanceId equals ai.ActivityInstanceId
                         join pi in this.DataHelper.Set<SysProcessInstance>()
                         on ai.ProcessInstanceId equals pi.ProcessInstanceId
                         join p in this.DataHelper.Set<SysProcess>()
                         on pi.ProcessId equals p.ProcessId
                         //join et in this.DataHelper.Set<SysEntity>()
                         //on p.EntityId equals et.EntityId
                         where wi.OwnerId == LoginUserID 
                         && (p.ProcessStatus == (int)ProcessState.StartUsed || p.ProcessStatus == (int)ProcessState.Updated)
                         && p.ProcessCategory != (int)ProcessCategory.FormApprove //2013-9-13 zhumin 排除表单流程
                         orderby pi.StartTime descending
                         select new
                         {
                             pi.ProcessInstanceId,
                             p.ProcessName,
                             //et.EntityName,
                             EntityName = this.EntityCache.FindById<SysEntity>(p.EntityId).EntityName,
                             pi.StartTime,
                             pi.EndTime,
                             pi.InstanceStatus,
                             pi.ObjectId,
                             OwnerItemName = "",
                             pi.ApproveResult,
                             p.EntityId
                         };
            //加上条件
            var list = result.Where(p => p.ProcessName.Contains(this.tbT_ProcessName.Text.Trim()));            
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

            var orgManager = new OrgManager(this.DataHelper);
            if (string.IsNullOrEmpty(tbT_OwnerItemName.Text.Trim())) //不需要查询“对象”
            {
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

                gcProcessInstance.DataSource = source.ToList().Select(p => new
                {
                    p.ProcessInstanceId,
                    p.ProcessName,                    
                    p.EntityName,                    
                    p.ObjectId,
                    OwnerItemName = orgManager.GetDisplayValue(p.EntityName, p.ObjectId),                    
                    p.StartTime,
                    p.EndTime,
                    p.InstanceStatus,
                    p.ApproveResult,
                    p.EntityId
                }).ToList();
                gcProcessInstance.DataBind();
            }
            else //需要查询“对象”
            {
                var tempSource = list.ToList().Select(p => new
                {
                    p.ProcessInstanceId,
                    p.ProcessName,
                    p.EntityName,
                    p.ObjectId,
                    OwnerItemName = orgManager.GetDisplayValue(p.EntityName, p.ObjectId),
                    p.StartTime,
                    p.EndTime,
                    p.InstanceStatus,
                    p.ApproveResult,
                    p.EntityId
                }).Where(p => p.OwnerItemName.Contains(tbT_OwnerItemName.Text.Trim())).ToList();

                int count = tempSource.Count;
                this.gcProcessInstance.PagerSettings.DataCount = count;
                var source = tempSource;
                //绑定
                if (count > this.PageIndex * this.PageSize)
                {
                    source = tempSource.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
                }
                else
                {
                    this.PageIndex = 0;
                    this.gcProcessInstance.PagerSettings.PageIndex = 0;
                    source = tempSource.Take(this.PageSize).ToList();
                }
                gcProcessInstance.DataSource = source;
                gcProcessInstance.DataBind();
            }
        }

        //private void BindGrid_Old()
        //{
        //    try
        //    {
        //        using (BizDataContext context = new BizDataContext())
        //        {
        //            var wiList = context.Where<SysWorkItem>(p => p.OwnerId == LoginUserID);

        //            var aiIdList = wiList.Select(p=>p.ActivityInstanceId).Distinct().ToList();

        //            var aiList = context.FetchAll<SysActivityInstance>().Where(p => aiIdList.Contains(p.ActivityInstanceId)).ToList();

        //            var piIdList = aiList.Select(p => p.ProcessInstanceId).Distinct().ToList();

        //            var piList = context.FetchAll<SysProcessInstance>().Where(p => piIdList.Contains(p.ProcessInstanceId)).ToList();

        //            var pList = context.FetchAll<SysProcess>();
        //            var eList = context.FetchAll<SysEntity>();

        //            int userId = LoginUserID;

        //            var query = from pi in piList
        //                        from p in pList
        //                        from et in eList
        //                        where
        //                        pi.ProcessId == p.ProcessId
        //                        && (p.ProcessStatus == (int)ProcessState.StartUsed || p.ProcessStatus == (int)ProcessState.Updated) 
        //                        && p.EntityId == et.EntityId
        //                        orderby pi.StartTime descending
        //                        select new
        //                        {
        //                            pi.ProcessInstanceId,
        //                            p.ProcessName,
        //                            et.EntityName,
        //                            pi.StartTime,
        //                            pi.EndTime,
        //                            pi.InstanceStatus,
        //                            pi.ObjectId,
        //                            OwnerItemName = "",
        //                            pi.ApproveResult,
        //                            p.EntityId
        //                        };

        //            var list = query.ToList().Distinct().Select(p => new
        //            {
        //                ProcessInstanceId = p.ProcessInstanceId,
        //                ProcessName = p.ProcessName,
        //                EntityName = p.EntityName,
        //                StartTime = p.StartTime,
        //                EndTime = p.EndTime,
        //                InstanceStatus = p.InstanceStatus,
        //                ObjectId = p.ObjectId,
        //                OwnerItemName = (new OrgManager(context)).GetDisplayValue(p.EntityName, p.ObjectId),
        //                ApproveResult = p.ApproveResult,
        //                EntityId = p.EntityId
        //            }).Where(p => p.OwnerItemName.Contains(tbT_OwnerItemName.Text.Trim())).ToList();

        //            if (!string.IsNullOrEmpty(this.tbT_ProcessName.Text.Trim()))
        //            {
        //                list = list.Where(p => p.ProcessName.Contains(this.tbT_ProcessName.Text.Trim())).ToList();
        //            }
        //            if (dtStartTime1.Text != "")
        //            {
        //                DateTime st1 = DateTime.Parse(dtStartTime1.Text);
        //                list = list.Where(p => p.StartTime > st1).ToList();
        //            }
        //            if (dtStartTime2.Text != "")
        //            {
        //                DateTime st2 = DateTime.Parse(dtStartTime2.Text);
        //                list = list.Where(p => p.StartTime < st2).ToList();
        //            }

        //            var result = list.ToList();
        //            this.gcProcessInstance.PagerSettings.DataCount = result.Count();
        //            绑定
        //            if (result.Count() > this.PageIndex * this.PageSize)
        //            {
        //                result = result.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
        //            }
        //            else
        //            {
        //                this.PageIndex = 0;
        //                this.gcProcessInstance.PagerSettings.PageIndex = 0;

        //                result = result.Take(this.PageSize).ToList();
        //            }
        //            this.gcProcessInstance.PagerSettings.DataCount = list.Count();

        //            gcProcessInstance.DataSource = result.ToList();
        //            gcProcessInstance.DataBind();
        //        }
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        this.AjaxAlert(ex.Message);
        //    }
        //}


        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e)
        {

            BindGrid();

        }

        /// <summary>
        /// 清空条件
        /// </summary>
        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            this.tbT_ProcessName.Text = string.Empty;
            this.dtStartTime1.Text = string.Empty;
            this.dtStartTime2.Text = string.Empty;
            this.tbT_OwnerItemName.Text = string.Empty;
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

                        //var p = new DataWrapper(e.Row.DataItem);
                        //int objectId = p["ObjectId"].ToInt();
                        //string entityName = p["EntityName"];
                        //string displayValue = OrgManager.GetDisplayValue(entityName, objectId);
                        //gcProcessInstance.SetRowText(e.Row, "OwnerItemName", displayValue);
                    }
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }
        protected void lbtnObjDetail_Click(object sender, EventArgs e)
        {
            try
            {
                string[] args = ((LinkButton) sender).CommandArgument.Split(',');
                if (args.Length != 2)
                {
                    throw new ApplicationException("entityId或ObjectId为空！");
                }
                else
                {
                    long entityId = args[0].ToLong();
                    long objectId = args[1].ToLong();
                    //using (BizDataContext context = new BizDataContext())
                    {
                        var entity = this.EntityCache.FindById<SysEntity>(entityId);
                        if (entity == null)
                        {
                            throw new ApplicationException("entity不存在！");
                        }
                        var DefaultPage = this.EntityCache.SysPage.FirstOrDefault<SysPage>(
                                p => p.EntityId == entityId && p.PageType == (int)PageType.DetailPage);

                        //SysPage DefaultPage = null;
                        //foreach (var p in pageList)
                        //{
                        //    SysPage page = context.FindById<SysPage>(p.ControlId);
                        //    if (page.PageType == (int)PageType.DetailPage)
                        //    {
                        //        DefaultPage = page;
                        //        break;
                        //    }
                        //}

                        if (DefaultPage == null)
                        {
                            throw new ApplicationException("跳转页面不存在！");
                        }
                        string url = (this.Master as Site).GetPageUrl(entity, DefaultPage);
                        url = string.Format("{0}?id={1}", url, objectId);

                        this.ListURL.Add(url);
                        Response.Redirect(url, false);
                    }
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }
    }
}