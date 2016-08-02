using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Entity;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.WorkflowEngine;
using Drision.Framework.Manager;
using Drision.Framework.OrgLibrary;
using Drision.Framework.Enum;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class MobileOperationRecordDetail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {                
                if (Request.UrlReferrer != null)
                {
                    URL = Request.UrlReferrer.ToString();
                }
                LoadData();
            }
        }


        public string URL
        {
            get{return ViewState["url"].ToString();}
            set{ViewState["url"]=value;}
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        void LoadData()
        {
            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                int piid = int.Parse(id);
                using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
                {
                    T_MobileOperationErrorLog log = context.T_MobileOperationErrorLog.FirstOrDefault(p => p.Id == piid);
                    if (log != null)
                    {
                        lblCreateUserId.Text =log.CreateUserId.HasValue?OrgManager.GetDisplayValue("T_User", log.CreateUserId.Value):string.Empty;
                        lblCreateTime.Text = log.CreateTime.HasValue ? log.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                        lblOperationRecord.Text = log.OperationRecord;
                        lblState.Text =log.State.HasValue? EnumHelper.GetDescription(typeof(OperationErrorLogStateEnum), log.State.Value):string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(URL);
        }
    }
}