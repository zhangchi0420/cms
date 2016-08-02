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
    public partial class ProcessTemplate : BasePage
    {
        
        private long? TemplateId
        {
            get 
            {
                if (ViewState["TemplateId"] == null)
                {
                    return null;
                }
                else
                {
                    return Convert.ToInt64(ViewState["TemplateId"]);
                }
            }
            set
            {
                ViewState["TemplateId"] = value;
            }
        }
        /// <summary>
        /// 存储所有的节点信息，留着保存时替换，同时在加载树时也初始化此值
        /// </summary>
        private Dictionary<string, string> AllNodes
        {
            get
            {
                if (ViewState["AllNodes"] == null)
                {
                    ViewState["AllNodes"] = new Dictionary<string, string>();
                }
                return ViewState["AllNodes"] as Dictionary<string, string>;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string id = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(id))
                {
                    TemplateId = Convert.ToInt64(id);
                }
                using (BizDataContext context = new BizDataContext())
                {
                    BindDropDown(context);
                    this.ccResultType.SelectedValue = ((int)ProcessTemplateResultType.Pass).ToString();
                    InitData(context);
                }
            }
        }
        /// <summary>
        /// 绑定下拉
        /// </summary>
        private void BindDropDown(BizDataContext context)
        {
            //var EntityList = context.FetchAll<SysEntity>().OrderBy(p => p.DisplayText).ToList();
            var EntityList = this.EntityCache.SysEntity.OrderBy(p => p.DisplayText).ToList();
            this.ccProcessEntityId.DataSource = EntityList;
            this.ccProcessEntityId.DataValueField = "EntityId";
            this.ccProcessEntityId.DataTextField = "DisplayText";
            this.ccProcessEntityId.DataBind();

            this.ccActivityEntityId.DataSource = EntityList;
            this.ccActivityEntityId.DataValueField = "EntityId";
            this.ccActivityEntityId.DataTextField = "DisplayText";
            this.ccActivityEntityId.DataBind();

            this.ccTemplateType.DataSource = typeof(TemplateType);
            this.ccTemplateType.DataBind();

            this.ccUseTimeType.DataSource = typeof(UseTimeType);
            this.ccUseTimeType.DataBind();

            this.ccResultType.DataSource = typeof(ProcessTemplateResultType);
            this.ccResultType.DataBind();
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="context"></param>
        private void InitData(BizDataContext context)
        {
            if (TemplateId != null)
            {
                var RemindTemplate = context.FindById <SysProcessRemindTemplate>(TemplateId.Value);
                this.tcTemplateName.SetValue(RemindTemplate.TemplateName);
                this.ccProcessEntityId.SetValue(RemindTemplate.ProcessEntityId);
                this.ccActivityEntityId.SetValue(RemindTemplate.ActivityEntityId);
                this.ccTemplateType.SetValue(RemindTemplate.TemplateType);
                this.ccUseTimeType.SetValue(RemindTemplate.UseTimeType);
                this.ccResultType.SetValue(RemindTemplate.ResultType == null ? (int)ProcessTemplateResultType.Pass : RemindTemplate.ResultType.Value);
                //将两个字段赋到前台
                this.hcTemplate.Value = RemindTemplate.OriginalTitleText;
                this.hcTemplate.Text = RemindTemplate.OriginalContentText;
            }
            else
            {
                this.tcTemplateName.SetValue("模板名称");
            }
        }
        /// <summary>
        /// 选择时机后绑定树
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccUseTimeType_TextChanged(object sender, EventArgs e)
        {
            //先清空
            AllNodes.Clear();
            this.TreeParamer.Nodes.Clear();
            var UseTimeTypeValue = ccUseTimeType.GetValue();
            if (UseTimeTypeValue == null)
            {
                return;
            }
                    
            using (BizDataContext context = new BizDataContext())
            {
                List<TNode> Nodes = new List<TNode>();
                //所有时机都需要流程实体，先加进去
                TNode tnProcessEntity = new TNode() { Value = "-1", Text = "流程实体" };
                //AllNodes["@(Model.PI_Data)"] = "流程实体";
                Nodes.Add(tnProcessEntity);
                //流程实体字段
                var ProcessEntityId = this.ccProcessEntityId.GetValue();
                if (ProcessEntityId != null)
                {
                    var ProcessEntityIdValue = Convert.ToInt64(ProcessEntityId);
                    //var ProcessFields = context.Where < SysField>(p => p.EntityId == ProcessEntityIdValue).ToList();
                    var ProcessFields = this.EntityCache.FindById<SysEntity>(ProcessEntityIdValue).Fields.ToList();
                    ProcessFields.ForEach(p => {
                        tnProcessEntity.ChildNodes.Add(new TNode() { Value = p.FieldId.ToString(), Text = "流程实体-" + p.DisplayText });
                        AllNodes[string.Format("@(Model.PI_Data[\"{0}\"])", p.FieldName.ToLower())] = string.Format("<input id=\"Button1\" type=\"button\" value=\"流程实体-{0}\">", p.DisplayText);
                    });
                }

                //活动实体先建好
                TNode tnActivityEntity = new TNode() { Value = "-2", Text = "活动实体" };
                //AllNodes["@(Model.AI_Data)"] = "活动实体";
                //活动实体字段
                var ActivityEntityId = this.ccActivityEntityId.GetValue();
                if (ActivityEntityId != null)
                {
                    var ActivityEntityIdValue = Convert.ToInt64(ActivityEntityId);
                    //var ActivityFields = context.Where<SysField>(p => p.EntityId == ActivityEntityIdValue).ToList();
                    var ActivityFields = this.EntityCache.FindById<SysEntity>(ActivityEntityIdValue).Fields.ToList();
                    ActivityFields.ForEach(p =>
                    {
                        tnActivityEntity.ChildNodes.Add(new TNode() { Value = p.FieldId.ToString(), Text = "活动实体-" + p.DisplayText });
                        AllNodes[string.Format("@(Model.AI_Data[\"{0}\"])", p.FieldName.ToLower())] = string.Format("<input id=\"Button1\" type=\"button\" value=\"活动实体-{0}\">", p.DisplayText);
                    });
                }
                switch (UseTimeTypeValue.ToInt())
                {
                    case (int)UseTimeType.ActivityStart:
                        break;
                    case (int)UseTimeType.ActivityEnd:
                        break;
                    case (int)UseTimeType.WorkItemCreate:
                        //Nodes.Add(tnActivityEntity);
                        Nodes.Add(new TNode() { Value = "-3", Text = "内网审核页面" });
                        AllNodes["<a href = \"@(Model.AprovePageInnerURL)\">内网审核页面地址</a>"] = string.Format("<input id=\"Button1\" type=\"button\" value=\"内网审核页面\">");

                        Nodes.Add(new TNode() { Value = "-4", Text = "外网审核页面" });
                        AllNodes["<a href = \"@(Model.AprovePageOuterURL)\">外网审核页面地址</a>"] = string.Format("<input id=\"Button1\" type=\"button\" value=\"外网审核页面\">");
                        break;
                    case (int)UseTimeType.WorkItemFinished:
                        Nodes.Add(tnActivityEntity);
                        break;
                    case (int)UseTimeType.ProcessStart:
                    case (int)UseTimeType.ProcessFinished:
                    case (int)UseTimeType.ProcessCancel:
                        break;
                }
                Nodes.Add(new TNode() { Value = "-5", Text = "内网流程详情页面" });
                AllNodes["<a href = \"@(Model.ProcessDetailInnerURL)\">内网流程详情页面地址</a>"] = string.Format("<input id=\"Button1\" type=\"button\" value=\"内网流程详情页面\">");
                Nodes.Add(new TNode() { Value = "-6", Text = "外网流程详情页面" });
                AllNodes["<a href = \"@(Model.ProcessDetailOuterURL)\">外网流程详情页面地址</a>"] = string.Format("<input id=\"Button1\" type=\"button\" value=\"外网流程详情页面\">");

                Nodes.Add(new TNode() { Value = "-7", Text = "收件人" });
                AllNodes["@(Model.ReceiveUser)"] = string.Format("<input id=\"Button1\" type=\"button\" value=\"收件人\">");
                Nodes.Add(new TNode() { Value = "-8", Text = "当前时间" });
                AllNodes["@(Model.CurrentTime)"] = string.Format("<input id=\"Button1\" type=\"button\" value=\"当前时间\">");
                this.TreeParamer.Nodes.AddRange(Nodes);
                this.TreeParamer.Refresh();
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //保存文本
            string OriginalTitleText = this.hcTemplate.Value;
            //翻译成模板文本
            string TemplateTitleText = Server.HtmlDecode(OriginalTitleText);
            AllNodes.ToList().ForEach(p => {
                TemplateTitleText = TemplateTitleText.Replace(p.Value, p.Key);
            });

            //保存文本
            string OriginalContentText = this.hcTemplate.Text;
            //翻译成模板文本
            string TemplateContentText = Server.HtmlDecode(OriginalContentText);
            AllNodes.ToList().ForEach(p =>
            {
                TemplateContentText = TemplateContentText.Replace(p.Value, p.Key);
            });

            using (BizDataContext context = new BizDataContext())
            {
                SysProcessRemindTemplate model;
                if (TemplateId == null)
                {
                    model = new SysProcessRemindTemplate();
                    model.TemplateId = context.GetNextIdentity();
                    model.State = 1;
                }
                else
                {
                    model = context.FindById < SysProcessRemindTemplate>(TemplateId.Value);
                }
                model.TemplateName = this.tcTemplateName.Text;
                model.TemplateType = this.ccTemplateType.SelectedValue.ToInt();
                model.ProcessEntityId = this.ccProcessEntityId.SelectedValue.ToLong();
                model.ActivityEntityId = this.ccActivityEntityId.SelectedValue.ToLong();
                model.UseTimeType = this.ccUseTimeType.SelectedValue.ToInt();
                model.ResultType = this.ccResultType.SelectedValue.ToInt();
                model.OriginalTitleText = OriginalTitleText;
                model.OriginalContentText = OriginalContentText;
                model.TemplateContentText = TemplateContentText;
                model.TemplateTitleText = TemplateTitleText;
                if (TemplateId == null)
                {
                    context.Insert(model);
                }
                else
                {
                    context.Update(model);
                }
            }
            Response.Redirect("~/SystemManagement/ProcessTemplateQuery.aspx");
        }
    }
}