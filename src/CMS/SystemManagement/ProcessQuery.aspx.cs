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
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using System.Linq.Expressions;
using Drision.Framework.Repository.EF;
using Drision.Framework.OrgLibrary;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class ProcessQuery : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //空条件查询（相当于重置）
                    btnQuery_Click(null, null);
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
                using (BizDataContext context = new BizDataContext())
                {
                    string name = this.tbT_ProcessName.Text.Trim();
                    var result = context.Where<SysProcess>(p => 
                        p.ProcessName.Contains(name)
                        && p.ProcessCategory != (int)ProcessCategory.FormApprove //2013-9-13 zhumin 排除表单流程
                        && p.ProcessStatus == (int)ProcessState.StartUsed)
                        .OrderBy(p => p.ProcessType);
                    var source = result.Select(p => new
                    {
                        p.ProcessId,
                        ProcessCategory = EnumHelper.GetDescription(typeof(ProcessCategory), p.ProcessCategory.Value),
                        p.ProcessName,
                        //context.FindById<SysEntity>(p.EntityId).EntityName,
                        this.EntityCache.FindById<SysEntity>(p.EntityId).EntityName,
                        //ActivityEntityName = context.FindById<SysEntity>(p.ActivityEntityId).EntityName,
                        ActivityEntityName = this.EntityCache.FindById<SysEntity>(p.ActivityEntityId).EntityName,
                        p.ProcessType,
                        p.ProcessVersion,
                        ProcessStatus = EnumHelper.GetDescription(typeof(ProcessState), p.ProcessStatus.Value),
                    });
                    gcProcess.DataSource = source;
                    gcProcess.DataBind();
                }                
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
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
                //查询数据
                btnQuery_Click(null, null);
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

    }
}