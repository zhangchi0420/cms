using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class ProcessTemplateQuery : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        //string GetEntityName(long entityId,BizDataContext context)
        //{
        //    string res = string.Empty;
        //    SysEntity se= context.FindById<SysEntity>(entityId);
        //    if (se != null)
        //    {
        //        res = se.DisplayText;
        //    }
        //    return res;
        //}

        private void BindGrid()
        {            
            using (BizDataContext context = new BizDataContext())
            {
                var list = context.FetchAll<SysProcessRemindTemplate>();
                var result = list.Select(p => new
                {
                    p.TemplateId,
                    p.TemplateName,
                    p.TemplateType,
                    p.UseTimeType,
                    ProcessEntityName = this.EntityCache.FindById<SysEntity>(p.ProcessEntityId).EntityName, //GetEntityName(p.ProcessEntityId.Value,context),
                    ActivityEntityName = this.EntityCache.FindById<SysEntity>(p.ActivityEntityId).EntityName, //GetEntityName(p.ActivityEntityId.Value,context),
                    p.State
                }).ToList().Select(p => new
                {
                    p.TemplateId,
                    p.TemplateName,
                    TemplateType = EnumHelper.GetDescription(typeof(TemplateType), p.TemplateType.Value),
                    UseTimeType = EnumHelper.GetDescription(typeof(UseTimeType), p.UseTimeType.Value),
                    p.ProcessEntityName,
                    p.ActivityEntityName,
                    State = p.State == 1 ? "启用" : "禁用",
                }).ToList();
                this.gcProcessTemplateQuery.DataSource = result;
                this.gcProcessTemplateQuery.DataBind();
            }
        }
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEnable_Click(object sender, EventArgs e)
        {
            string id = (sender as LinkButton).CommandArgument;
            if (!string.IsNullOrEmpty(id))
            {
                using (BizDataContext context = new BizDataContext())
                {
                    var model = context.FindById<SysProcessRemindTemplate>(id.ToLong());
                    model.State = 1;
                    context.Update(model);
                    BindGrid();
                }
            }
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
            string id = (sender as LinkButton).CommandArgument;
            if (!string.IsNullOrEmpty(id))
            {
                using (BizDataContext context = new BizDataContext())
                {
                    var model = context.FindById<SysProcessRemindTemplate>(id.ToLong());
                    model.State = 2;
                    context.Update(model);
                    BindGrid();
                }
            }
        }
        /// <summary>
        /// 控制启用禁用按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gcProcessTemplateQuery_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var State = DataBinder.Eval(e.Row.DataItem, "State");
            if (State != null)
            {
                if (State.ToString() == "启用")
                {
                    LinkButton lbtnEnable = e.Row.FindControl("lbtnEnable") as LinkButton;
                    if (lbtnEnable != null)
                    {
                        lbtnEnable.Visible = false;
                    }
                }
                if (State.ToString() == "禁用")
                {
                    LinkButton lbtnDisable = e.Row.FindControl("lbtnDisable") as LinkButton;
                    if (lbtnDisable != null)
                    {
                        lbtnDisable.Visible = false;
                    }
                }
            }
        }
    }
}