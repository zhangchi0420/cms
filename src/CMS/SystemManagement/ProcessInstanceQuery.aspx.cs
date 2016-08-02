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
    public partial class ProcessInstanceQuery : BasePage
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
                    LoadDropDown();                    

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

        private void LoadDropDown()
        {
            try
            {
                using (BizDataContext context = new BizDataContext())
                {
                    var processList = context.Where<SysProcess>(p => 
                        p.ProcessStatus == (int)ProcessState.StartUsed
                        && p.ProcessCategory != (int)ProcessCategory.FormApprove //2013-9-13 zhumin 排除表单流程
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
                         join u in this.DataHelper.Set<T_User>()
                         on pi.StartUserId equals u.User_ID
                         join d in this.DataHelper.Set<T_Department>()
                         on pi.StartDeptId equals d.Department_ID
                         //join et in this.DataHelper.Set<SysEntity>()
                         //on p.EntityId equals et.EntityId
                         where (p.ProcessStatus == (int)ProcessState.StartUsed || p.ProcessStatus == (int)ProcessState.Updated)
                         && p.ProcessCategory != (int)ProcessCategory.FormApprove //2013-9-13 zhumin 排除表单流程
                         orderby pi.StartTime descending
                         select new
                         {
                             pi.ProcessInstanceId,
                             p.ProcessName,
                             p.ProcessType,
                             //et.EntityName,
                             //et.DisplayText,
                             EntityName = this.EntityCache.FindById<SysEntity>(p.EntityId).EntityName,
                             DisplayText = this.EntityCache.FindById<SysEntity>(p.EntityId).DisplayText,
                             pi.StartUserId,
                             u.User_Name,
                             pi.ObjectId,
                             OwnerItemName = "",
                             d.Department_Name,
                             pi.StartTime,
                             pi.EndTime,
                             pi.InstanceStatus,
                             pi.ApproveResult,
                             p.EntityId
                         };
            //加上条件
            var list = result.Where(p => p.ProcessName.Contains(tbT_ProcessName.Text));
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
                    p.ProcessType,
                    p.EntityName,
                    p.DisplayText,
                    p.StartUserId,
                    p.User_Name,
                    p.ObjectId,
                    OwnerItemName = orgManager.GetDisplayValue(p.EntityName, p.ObjectId),
                    p.Department_Name,
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
                    p.ProcessType,
                    p.EntityName,
                    p.DisplayText,
                    p.StartUserId,
                    p.User_Name,
                    p.ObjectId,
                    OwnerItemName = orgManager.GetDisplayValue(p.EntityName, p.ObjectId),
                    p.Department_Name,
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
        //        using (BizDataContext context = new BizDataContext(GlobalObject.ConnString))
        //        {
        //            var uList = context.FetchAll<T_User>();
        //            var dList = context.FetchAll<T_Department>();
        //            var aiList = context.FetchAll<SysActivityInstance>();
        //            var piList = context.FetchAll<SysProcessInstance>();
        //            var pList = context.FetchAll<SysProcess>();
        //            var eList = context.FetchAll<SysEntity>();


        //            var query = from p in pList
        //                        from pi in piList
        //                        from u in uList
        //                        from d in dList
        //                        from et in eList
        //                        where p.ProcessId == pi.ProcessId.Value
        //                        && pi.StartUserId == u.User_ID
        //                        && pi.StartDeptId == d.Department_ID
        //                        && (p.ProcessStatus == (int)ProcessState.StartUsed || p.ProcessStatus == (int)ProcessState.Updated) && p.EntityId == et.EntityId
        //                        orderby pi.StartTime descending
        //                        select new
        //                        {
        //                            pi.ProcessInstanceId,
        //                            p.ProcessName,
        //                            p.ProcessType,
        //                            et.EntityName,
        //                            et.DisplayText,
        //                            pi.StartUserId,
        //                            u.User_Name,
        //                            pi.ObjectId,
        //                            OwnerItemName = "",
        //                            d.Department_Name,
        //                            pi.StartTime,
        //                            pi.EndTime,
        //                            pi.InstanceStatus,
        //                            pi.ApproveResult,
        //                            p.EntityId
        //                        };
        //            #region modifyBy Youtian 2012/7/5
        //            //var list = query.ToList().Distinct().Where(p => p.ProcessName.Contains(tbT_ProcessName.Text));
        //            var list = query.ToList().Distinct().Where(p => p.ProcessName.Contains(tbT_ProcessName.Text)).Select
        //                (p => new
        //                          {
        //                              ProcessInstanceId = p.ProcessInstanceId,
        //                              ProcessName = p.ProcessName,
        //                              ProcessType = p.ProcessType,
        //                              EntityName = p.EntityName,
        //                              DisplayText = p.DisplayText,
        //                              StartUserId = p.StartUserId,
        //                              User_Name = p.User_Name,
        //                              ObjectId = p.ObjectId,
        //                              OwnerItemName = (new OrgManager(context)).GetDisplayValue(p.EntityName, p.ObjectId),
        //                              Department_Name = p.Department_Name,
        //                              StartTime = p.StartTime,
        //                              EndTime = p.EndTime,
        //                              InstanceStatus = p.InstanceStatus,
        //                              ApproveResult = p.ApproveResult,
        //                              EntityId = p.EntityId
        //                          }).Where(p => p.OwnerItemName.Contains(tbT_OwnerItemName.Text.Trim()));

        //            #endregion
        //            if (cbStartUser.SelectedValue != null)
        //            {
        //                int startUserId = Int32.Parse(cbStartUser.SelectedValue);
        //                list = list.Where(p => p.StartUserId == startUserId);
        //            }
        //            if (cbStatus.SelectedValue != null)
        //            {
        //                int stateId = Int32.Parse(cbStatus.SelectedValue);
        //                list = list.Where(p => p.InstanceStatus == stateId);
        //            }
        //            if (dtStartTime1.Text != "")
        //            {
        //                DateTime st1 = DateTime.Parse(dtStartTime1.Text);
        //                list = list.Where(p => p.StartTime > st1);
        //            }
        //            if (dtStartTime2.Text != "")
        //            {
        //                DateTime st2 = DateTime.Parse(dtStartTime2.Text);
        //                list = list.Where(p => p.StartTime < st2);
        //            }
        //            if (dtEndTime1.Text != "")
        //            {
        //                DateTime et1 = DateTime.Parse(dtEndTime1.Text);
        //                list = list.Where(p => p.EndTime > et1);
        //            }
        //            if (dtEndTime2.Text != "")
        //            {
        //                DateTime et2 = DateTime.Parse(dtEndTime2.Text);
        //                list = list.Where(p => p.EndTime < et2);
        //            }
        //            if (cbProcess.SelectedValue != null)
        //            {
        //                long typeid = long.Parse(cbProcess.SelectedValue);
        //                list = list.Where(p => p.ProcessType == typeid);
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

        //            gcProcessInstance.DataSource = result;
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
                tbT_ProcessName.Text = "";
                cbProcess.SelectedValue = null;
                cbStartUser.SelectedValue = null;
                cbStatus.SelectedValue = null;
                dtStartTime1.Text = "";
                dtStartTime2.Text = "";
                dtEndTime1.Text = "";
                dtEndTime2.Text = "";
                tbT_OwnerItemName.Text = "";
                //查询数据
                BindGrid();
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
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
                        if(DataBinder.Eval(e.Row.DataItem, "ApproveResult")!=null)
                        {
                            int approveResult = Int32.Parse(DataBinder.Eval(e.Row.DataItem, "ApproveResult").ToString());

                            string approveResultStr = EnumHelper.GetDescription(typeof(Drision.Framework.Common.Workflow.ApproveResultEnum), approveResult);
                            gcProcessInstance.SetRowText(e.Row, "ApproveResult", approveResultStr);
                        }
                        gcProcessInstance.SetRowText(e.Row, "InstanceStatus", stateStr);
                        #region modifyBy Youtian 2012/7/5

                        //var p = new DataWrapper(e.Row.DataItem);
                        //int objectId = p["ObjectId"].ToInt();
                        //string entityName = p["EntityName"];
                        //string displayValue = OrgManager.GetDisplayValue(entityName, objectId);
                        //gcProcessInstance.SetRowText(e.Row, "OwnerItemName", displayValue);

                        #endregion
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
            catch(ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }
    }
}