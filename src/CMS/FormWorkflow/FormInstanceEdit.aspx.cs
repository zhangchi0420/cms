using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.Workflow;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormInstanceEdit : BasePage
    {
        public int FormInstanceId
        {
            get { return VS<int>("FormInstanceId"); }
            set { VS<int>("FormInstanceId", value); }
        }

        public long FormId
        {
            get { return VS<long>("FormId"); }
            set { VS<long>("FormId", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }
                this.formPreview.LoadFormWithInstance(this.FormInstanceId);

                //绑定提交按钮
                var btnList = this.DataHelper.Set<SysProcess>()
                    .Where(p => p.ProcessCategory == (int)ProcessCategory.FormApprove
                && p.ProcessStatus == (int)ProcessState.StartUsed && p.FormId == this.FormId).Select(p => new
                {
                    p.ProcessName,
                    p.ProcessId,
                    ButtonName = string.Format("提交（{0}）", p.ProcessName),
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
            int? id = QueryString<int?>("id");
            if (id == null)
            {
                throw new Exception("参数不正确");
            }
            SysFormInstance fi = this.DataHelper.FindById<SysFormInstance>(id);
            if (fi == null)
            {
                throw new Exception("表单实例不存在");
            }

            if (fi.State == (int)FormInstanceState.Approving)
            {
                throw new Exception("表单实例审核中，不能编辑");
            }


            SysForm form = this.DataHelper.FindById<SysForm>(fi.FormId);
            if (form == null)
            {
                throw new Exception("表单不存在");
            }
            this.FormInstanceId = fi.FormInstanceId;
            this.FormId = form.FormId;
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
                        //更新表单实例
                        this.formPreview.UpdateFormInstance(db);
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
                        //更新表单实例
                        this.formPreview.UpdateFormInstance(db);

                        //提交流程
                        string processName = (sender as LinkButton).CommandArgument;
                        Drision.Framework.WorkflowEngineCore.EngineProxy proxy = new WorkflowEngineCore.EngineProxy(db);
                        proxy.StartProcess(processName, this.LoginUserID, this.FormInstanceId);
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
    }
}