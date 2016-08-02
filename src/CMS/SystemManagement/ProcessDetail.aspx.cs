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

namespace Drision.Framework.Web.SystemManagement
{
    public partial class ProcessDetail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //加载数据
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    long ProcessId = long.Parse(Request.QueryString["id"]);
                    using (BizDataContext context = new BizDataContext())
                    {
                        SysProcess process = context.FindById<SysProcess>(ProcessId);
                        if (process != null)
                        {
                            this.lblProcessName.Text = process.ProcessName;
                            this.lblProcessDescription.Text = process.ProcessDescription;
                            this.lblStatus.Text = EnumHelper.GetDescription(typeof(ProcessState), process.ProcessStatus.Value);

                            this.gcProcessProxy.DataSource = context.Where<SysProcessProxy>(p => p.ProcessId == ProcessId).ToList();
                            this.gcProcessProxy.DataBind();
                        }
                    }
                    this.btnAdd.PostBackUrl = "~/SystemManagement/ProcessProxyAdd.aspx?ProcessId=" + Request.QueryString["id"];
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="serser"></param>
        /// <param name="e"></param>
        protected void lbtnDel_Click(object sender, EventArgs e)
        {
            using (BizDataContext context = new BizDataContext())
            {
                int id = int.Parse((sender as LinkButton).CommandArgument);
                SysProcessProxy ProcessProxyData = context.FirstOrDefault<SysProcessProxy>(p => p.ProcessProxyId == id);
                context.Delete(ProcessProxyData);
            }
            Response.Redirect(Request.Url.ToString());
        }
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="serser"></param>
        /// <param name="e"></param>
        protected void lbtnEnable_Click(object sender, EventArgs e)
        {
            using (BizDataContext context = new BizDataContext())
            {
                int id = int.Parse((sender as LinkButton).CommandArgument);
                SysProcessProxy ProcessProxyData = context.FirstOrDefault<SysProcessProxy>(p => p.ProcessProxyId == id);
                ProcessProxyData.Status = (int)ProxyStatus.Enable;
                context.Update(ProcessProxyData);
            }
            Response.Redirect(Request.Url.ToString());
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="serser"></param>
        /// <param name="e"></param>
        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
            using (BizDataContext context = new BizDataContext())
            {
                int id = int.Parse((sender as LinkButton).CommandArgument);
                SysProcessProxy ProcessProxyData = context.FirstOrDefault<SysProcessProxy>(p => p.ProcessProxyId == id);
                ProcessProxyData.Status = (int)ProxyStatus.Disable;
                context.Update(ProcessProxyData);
            }
            Response.Redirect(Request.Url.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gcProcessProxy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //LinkButton lbtnEnable = e.Row.FindControl("lbtnEnable") as LinkButton;
            //LinkButton lbtnDisable = e.Row.FindControl("lbtnDisable") as LinkButton;
            //if (lbtnEnable == null || lbtnDisable == null)
            //    return;
            //lbtnEnable.Visible = false;
            //lbtnDisable.Visible = false;
            //object Status = DataBinder.Eval(e.Row.DataItem, "Status");
            //if (Status != null)
            //{
            //    switch (int.Parse(Status.ToString()))
            //    {
            //        case (int)ProxyStatus.Enable: lbtnDisable.Visible = true; break;
            //        case (int)ProxyStatus.Disable: lbtnEnable.Visible = true; break;
            //        default:break;
            //    }
            //}
            LinkButton lbtnDisable = e.Row.FindControl("lbtnDisable") as LinkButton;
            object Status = DataBinder.Eval(e.Row.DataItem, "Status");
            if (lbtnDisable != null && Status != null)
            {
                if (int.Parse(Status.ToString()) == (int)ProxyStatus.Enable)
                    lbtnDisable.Visible = true;
                else
                    lbtnDisable.Visible = false;
            }
                
        }
        /// <summary>
        /// 绑定时将用户主键转为用户名称（前台调用）
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetUserName(object UserID)
        {
            string User_Name = UserID == null ? "" : UserID.ToString();
            if (UserID != null)
            {
                using (BizDataContext context = new BizDataContext())
                {
                    int id = int.Parse(UserID.ToString());
                    T_User User = context.FindById<T_User>(id);
                    if (User != null)
                    {
                        User_Name = User.User_Name;
                    }
                }
            }
            return User_Name;
        }
        /// <summary>
        /// 绑定时将状态值转为状态名称（前台调用）
        /// </summary>
        /// <param name="StatusValue"></param>
        /// <returns></returns>
        public string GetStatusName(object StatusValue)
        {
            string StatusName = StatusValue == null ? "" : StatusValue.ToString();
            if (StatusValue != null)
            {
                switch (int.Parse(StatusValue.ToString()))
                {
                    case (int)ProxyStatus.Enable: StatusName = "启用"; break;
                    case (int)ProxyStatus.Disable: StatusName = "禁用"; break;
                    case (int)ProxyStatus.OverDue: StatusName = "过期"; break;
                    default: break;
                }
            }
            return StatusName;
        }
    }
}