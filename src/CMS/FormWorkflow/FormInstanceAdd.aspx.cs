using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormInstanceAdd : BasePage
    {
        protected long? FormId
        {
            get { return VS<long?>("FormId"); }
            set { VS<long?>("FormId", value); }
        }        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }
                this.formPreview.LoadForm(this.FormId.Value);

                //绑定提交按钮
                var btnList = this.DataHelper.Set<SysProcess>()
                    .Where(p => p.ProcessCategory== (int)ProcessCategory.FormApprove
                && p.ProcessStatus == (int)ProcessState.StartUsed && p.FormId == this.FormId).Select(p => new
                {
                    p.ProcessName,
                    p.ProcessId,
                    ButtonName = string.Format("提交（{0}）",p.ProcessName),
                }).ToList();
                this.rApply.DataSource = btnList;
                this.rApply.DataBind();
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
            long? id = QueryString<long?>("id");
            if (id == null)
            {
                throw new Exception("参数不正确");
            }
            SysForm form = this.DataHelper.FindById<SysForm>(id);
            if (form == null)
            {
                throw new Exception("参数不正确");
            }
            this.FormId = id.Value;
        }

        /// <summary>
        /// 提交流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //保存表单实例得到FormInstanceId
                        int fiId = this.formPreview.SaveFormInstance(db);

                        //提交流程
                        string processName = (sender as LinkButton).CommandArgument;
                        Drision.Framework.WorkflowEngineCore.EngineProxy proxy = new WorkflowEngineCore.EngineProxy(db);
                        proxy.StartProcess(processName, this.LoginUserID, fiId);
                    }
                    ts.Complete();
                }
                this.AjaxAlertAndRedirect("提交成功", "FormInstanceQuery.aspx");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //新增表单实例
                        this.formPreview.SaveFormInstance(db);
                    }
                    ts.Complete();
                }

                this.AjaxAlertAndRedirect("保存成功", "FormInstanceQuery.aspx");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }
}