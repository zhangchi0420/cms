using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Web.Common;
using Drision.Framework.Repository;
using Drision.Framework.Common;
using Drision.Framework.OrgLibrary;
using Drision.Framework.OrgLibrary.InternalEntities;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class ProcessProxyAdd : BasePage
    {
        /// <summary>
        /// 代理Id
        /// </summary>
        public int? Id 
        {
            get
            {
                if (ViewState["ProxyId"] == null)
                {
                    return null;
                }
                else
                {
                    return int.Parse(ViewState["ProxyId"].ToString());
                }
            }
            set
            {
                ViewState["ProxyId"] = value;
            }
        }
        /// <summary>
        /// 流程Id
        /// </summary>
        public long? ProcessId
        {
            get
            {
                if (ViewState["ProcessId"] == null)
                {
                    return null;
                }
                else
                {
                    return long.Parse(ViewState["ProcessId"].ToString());
                }
            }
            set
            {
                ViewState["ProcessId"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    Id = int.Parse(Request.QueryString["id"]);
                }
                if (Request.QueryString["ProcessId"] != null)
                {
                    ProcessId = long.Parse(Request.QueryString["ProcessId"]);
                }
                //查询并加载数据
                if (Id != null)
                {
                    using (BizDataContext context = new BizDataContext())
                    {
                        SysProcessProxy ProcessProxyData = context.FindById < SysProcessProxy>(Id);
                        this.scOwner.SetValue(ProcessProxyData.OwnerId);
                        this.scOwner.Text = context.FindById<T_User>(ProcessProxyData.OwnerId).User_Name;

                        this.scProxy.SetValue(ProcessProxyData.ProxyId);
                        this.scProxy.Text = context.FindById<T_User>(ProcessProxyData.ProxyId).User_Name;

                        this.dtcStartTime.SetValue(ProcessProxyData.StartTime);
                        this.dtcEndTime.SetValue(ProcessProxyData.EndTime);
                    }
                }
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    this.btnCancel.PostBackUrl = HttpContext.Current.Request.UrlReferrer.ToString();
                    ViewState["UrlReferrer"] = HttpContext.Current.Request.UrlReferrer;
                }
            }
        }
        /// <summary>
        /// 保存按钮添加验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.btnSave.Attributes.Add("onclick", "return Validate();");
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
                using (BizDataContext context = new BizDataContext())
                {
                    SysProcessProxy ProcessProxyData;
                    if (Id == null)
                    {
                        ProcessProxyData = new SysProcessProxy()
                        {
                            ProcessProxyId = context.GetNextIdentity_Int(),
                            ProcessId = ProcessId,
                            OwnerId = Convert.ToInt32(this.scOwner.GetValue()),
                            ProxyId = Convert.ToInt32(this.scProxy.GetValue()),
                            StartTime = Convert.ToDateTime(this.dtcStartTime.GetValue()),
                            EndTime = Convert.ToDateTime(this.dtcEndTime.GetValue()),
                            CreateTime = DateTime.Now,
                            Status = (int)ProxyStatus.Enable,
                        };
                        context.Insert(ProcessProxyData);
                    }
                    else
                    {
                        ProcessProxyData = context.FindById<SysProcessProxy>(Id);
                        ProcessProxyData.OwnerId = Convert.ToInt32(this.scOwner.GetValue());
                        ProcessProxyData.ProxyId = Convert.ToInt32(this.scProxy.GetValue());
                        ProcessProxyData.StartTime = Convert.ToDateTime(this.dtcStartTime.GetValue());
                        ProcessProxyData.EndTime = Convert.ToDateTime(this.dtcEndTime.GetValue());
                        context.Update(ProcessProxyData);
                    }                    
                }
                Response.Redirect(ViewState["UrlReferrer"].ToString());
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }
    }
}