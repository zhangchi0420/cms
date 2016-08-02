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
using Drision.Framework.OrgLibrary;
using Drision.Framework.WorkflowEngineCore;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class StartProcessQuery : BasePage
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
            var result = from pi in this.DataHelper.Set<SysProcessInstance>()
                         join p in this.DataHelper.Set<SysProcess>()
                         on pi.ProcessId equals p.ProcessId
                         //join et in this.DataHelper.Set<SysEntity>()
                         //on p.EntityId equals et.EntityId
                         where pi.StartUserId == LoginUserID 
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
                    InstanceStatus = ConvertStatus(p.InstanceStatus),
                    CanCancel = p.InstanceStatus != (int)ProcessInstanceStatusEnum.Completed && p.InstanceStatus != (int)ProcessInstanceStatusEnum.Cancelled, 
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
                    CanCancel = p.InstanceStatus != (int)ProcessInstanceStatusEnum.Completed && p.InstanceStatus != (int)ProcessInstanceStatusEnum.Cancelled,                        
                    InstanceStatus = ConvertStatus(p.InstanceStatus),
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
        //            var piList = context.FetchAll<SysProcessInstance>();
        //            var pList = context.FetchAll<SysProcess>();
        //            var eList = context.FetchAll<SysEntity>();

        //            var query = from p in pList
        //                        from pi in piList
        //                        from et in eList
        //                        where p.ProcessId == pi.ProcessId.Value && pi.StartUserId == LoginUserID
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
        //                            pi.ApproveResult,
        //                            p.EntityId
        //                        };
        //            var list = query.Distinct().ToList().Select(p => new ProcessInstanceInfo()
        //            {
        //                ProcessInstanceId = p.ProcessInstanceId,
        //                ProcessName = p.ProcessName,
        //                StartTime = p.StartTime,
        //                EndTime = p.EndTime,
        //                EntityName = p.EntityName,
        //                InstanceStatus = ConvertStatus(p.InstanceStatus),
        //                OwnerItemName = (new OrgManager(context)).GetDisplayValue( p.EntityName,p.ObjectId),
        //                CanCancel = p.InstanceStatus != (int)ProcessInstanceStatusEnum.Completed && p.InstanceStatus != (int)ProcessInstanceStatusEnum.Cancelled,
        //                ApproveResult = ConvertApproveState(p.ApproveResult),
        //                EntityId = p.EntityId,
        //                ObjectId=p.ObjectId
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
        //            //绑定
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
        //            //this.gcProcessInstance.PagerSettings.DataCount = list.Count();

        //            gcProcessInstance.DataSource = result.ToList();
        //            this.gcProcessInstance.PagerSettings.DataCount = result.Count();
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

        
        private string ConvertApproveState(int? approveState)
        {
            string approveResultStr = string.Empty;
            if (approveState != null)
            {
                approveResultStr = EnumHelper.GetDescription(typeof(Drision.Framework.Common.Workflow.ApproveResultEnum), approveState.Value);
            }
            return approveResultStr;
        }
        private string ConvertStatus(int state)
        {
            string result = string.Empty;
            switch (state)
            {
                case (int)ProcessInstanceStatusEnum.Running:
                    result = "运行中";
                    break;
                case (int)ProcessInstanceStatusEnum.Suspending:
                    result = "挂起中";
                    break;
                case (int)ProcessInstanceStatusEnum.Completed:
                    result = "已完成";
                    break;
                case (int)ProcessInstanceStatusEnum.Cancelled:
                    result = "已取消";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 清空条件
        /// </summary>
        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            this.tbT_ProcessName.Text = string.Empty;
            this.dtStartTime1.Text = string.Empty;
            this.dtStartTime2.Text = string.Empty;
            tbT_OwnerItemName.Text = string.Empty;
            BindGrid();
        }

        /// <summary>
        /// 撤消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int piId = (sender as LinkButton).CommandArgument.ToInt();
                        
            try
            {
                this.EngineHelper.CancelProcess(piId);
                this.AjaxAlert("已成功撤消");
                BindGrid();
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
                string[] args = ((LinkButton)sender).CommandArgument.Split(',');
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

    public class ProcessInstanceInfo
    {
        public int ProcessInstanceId { get; set; }
        public string ProcessName { get; set; }
        public string EntityName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string InstanceStatus { get; set; }
        public string OwnerItemName { get; set; }
        public bool CanCancel { get; set; }
        public string ApproveResult { get; set; }
        public long? EntityId { get; set; }
        public int ObjectId { get; set; }
    }
}