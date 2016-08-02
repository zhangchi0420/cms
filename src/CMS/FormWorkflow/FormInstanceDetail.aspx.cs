using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormInstanceDetail : BasePage
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

        public string URL
        {
            get { return (string)ViewState["url"]; }
            set { ViewState["url"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.UrlReferrer != null)
                    {
                        URL = Request.UrlReferrer.ToString();
                    }
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

            //2013-10-8 zhumin 审核中的不能再提交
            this.rApply.Visible = fi.State != (int)FormInstanceState.Approving;            

            SysForm form = this.DataHelper.FindById<SysForm>(fi.FormId);
            if (form == null)
            {
                throw new Exception("表单不存在");
            }
            this.FormInstanceId = fi.FormInstanceId;
            this.FormId = form.FormId;
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
                Response.Redirect("~/FormWorkflow/FormInstanceQuery.aspx");
            }
        }
    }
}