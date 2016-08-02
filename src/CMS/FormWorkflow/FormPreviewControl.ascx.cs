using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;
using System.Web.UI.HtmlControls;
using Drision.Framework.WebControls.Super;
using Drision.Framework.WorkflowEngineCore.Cache;
using System.Text;
using Drision.Framework.Manager;
using Drision.Framework.WorkflowEngineCore;
using Drision.Framework.Entity;

namespace Drision.Framework.Web.FormWorkflow
{
    /// <summary>
    /// 表单解析控件
    /// </summary>
    public partial class FormPreviewControl : System.Web.UI.UserControl
    {
        #region 属性

        protected BasePage BasePage
        {
            get { return this.Page as BasePage; }
        }

        protected int LoginUserId
        {
            get { return this.BasePage.LoginUserID; }
        }

        protected long FormId
        {
            get { return BasePage.VS<long>("FormId_InControl"); }
            set { BasePage.VS<long>("FormId_InControl", value); }
        }

        protected long EntityId
        {
            get { return BasePage.VS<long>("EntityId_InControl"); }
            set { BasePage.VS<long>("EntityId_InControl", value); }
        }

        protected int? FormInstanceId
        {
            get { return BasePage.VS<int?>("FormInstanceId_InControl"); }
            set { BasePage.VS<int?>("FormInstanceId_InControl", value); }
        }

        protected int? ObjectId
        {
            get { return BasePage.VS<int?>("ObjectId_InControl"); }
            set { BasePage.VS<int?>("ObjectId_InControl", value); }
        }

        protected SysFormInstance FormInstance { get; set; }

        protected SysForm Form { get; set; }

        protected BizDataContext DataHelper
        {
            get { return this.BasePage.DataHelper; }
        }

        protected Drision.Framework.Entity.EntityCache EntityCache
        {
            get { return this.BasePage.EntityCache; }
        }

        public bool IsDetailPage
        {
            get { return BasePage.VS<bool>("IsDetailPage"); }
            set { BasePage.VS<bool>("IsDetailPage", value); }
        }

        #endregion

        /// <summary>
        /// 加载预览（测试用数据），不判断权限
        /// </summary>
        /// <param name="formId"></param>
        public void LoadPreview(long formId)
        {
            SysForm form = this.DataHelper.FindById<SysForm>(formId);
            if (form != null && form.EntityId != null)
            {
                this.Form = form;
                this.FormId = formId;
                this.EntityId = form.EntityId.Value;
                
                LoadFormContentControl(null);

                if (!this.Page.IsPostBack)
                {
                    LoadPreivewTestData();
                    LoadFormInstance();
                }
            }
            else
            {
                throw new Exception("表单不存在");
            }
        }


        /// <summary>
        /// 加载表单，根据表单权限显示
        /// </summary>
        /// <param name="formId">表单ID</param>        
        public void LoadForm(long formId)
        {
            SysForm form = this.DataHelper.FindById<SysForm>(formId);
            if (form != null && form.EntityId != null)
            {
                this.Form = form;
                this.FormId = formId;
                this.EntityId = form.EntityId.Value;

                var fpDict = GetFormPrivilege();                
                LoadFormContentControl(fpDict);

                if (!this.Page.IsPostBack)
                {
                    LoadDefaultFormInstanceData(); //默认的表单实例信息
                    LoadFormInstance();
                }
            }
            else
            {
                throw new Exception("表单不存在");
            }
        }

        /// <summary>
        /// 加载表单（含表单实体对象数据），根据表单权限显示，审核页面上会额外根据活动判断权限
        /// </summary>        
        /// <param name="formInstanceId">表单实例ID</param>
        /// <param name="activityId">当前活动ID，审核页面用</param>
        public void LoadFormWithInstance(int formInstanceId, long? activityId = null)
        {
            SysFormInstance fi = this.DataHelper.FindById<SysFormInstance>(formInstanceId);
            if (fi == null)
            {
                throw new Exception("表单实例不存在");
            }

            SysForm form = this.DataHelper.FindById<SysForm>(fi.FormId);
            if (form != null && form.EntityId != null)
            {
                this.Form = form;
                this.FormId = form.FormId;
                this.EntityId = form.EntityId.Value;
                this.FormInstanceId = fi.FormInstanceId;
                this.FormInstance = this.DataHelper.FindById<SysFormInstance>(this.FormInstanceId);
                if (this.FormInstance != null)
                {
                    this.ObjectId = this.FormInstance.ObjectId;

                    var fpDict = GetFormPrivilege(activityId);
                    var controlDict = LoadFormContentControl(fpDict);

                    if (!this.Page.IsPostBack)
                    {
                        LoadFormInstance();
                        LoadObjectData(controlDict);
                        LoadProcessApproveHistory();
                    }
                }
                else
                {
                    throw new Exception("流程实例不存在");
                }
            }
            else
            {
                throw new Exception("表单不存在");
            }
        }

        /// <summary>
        /// 按规则合并两个表单权限
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <param name="useBigPrivilege">同一表单字段有两个权限，是否取其较大权限</param>
        private Dictionary<long, FormFieldPrivilege> MergeFormFieldPrivilege(Dictionary<long, FormFieldPrivilege> one, Dictionary<long, FormFieldPrivilege> two, bool useBigPrivilege)
        {
            Dictionary<long, FormFieldPrivilege> result = new Dictionary<long, FormFieldPrivilege>();
            
            foreach (var p in one)
            {
                if (!result.ContainsKey(p.Key))
                {
                    result[p.Key] = p.Value;
                }
            }

            foreach (var p in two)
            {
                if (!result.ContainsKey(p.Key))
                {
                    result[p.Key] = p.Value;
                }
                else
                {
                    int x = (int)p.Value;
                    int y = (int)result[p.Key];

                    if (useBigPrivilege)
                    {
                        result[p.Key] = (FormFieldPrivilege)(x > y ? x : y);
                    }
                    else
                    {
                        result[p.Key] = (FormFieldPrivilege)(x < y ? x : y);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 权限判断
        /// </summary>        
        /// <param name="activityId">活动ID</param>
        /// <returns>表单字段对应的权限</returns>
        private Dictionary<long, FormFieldPrivilege> GetFormPrivilege(long? activityId = null)
        {
            Dictionary<long, FormFieldPrivilege> result = new Dictionary<long,FormFieldPrivilege>();

            //表单字段角色权限判断
            var roleList = this.BasePage.OrgHelper.GetUserRoles(this.LoginUserId); //用户的所有角色
            foreach (var role in roleList)
            {
                var temp = this.DataHelper.Where<SysFormRolePrivilege>(p => p.FormId == this.FormId
                    && p.RoleId == role.Role_ID).ToDictionary(p => p.FormFieldId.Value, p => (FormFieldPrivilege)p.DisplayPrivilege);

                //取所有角色中最大的权限
                //如某人有角色A、B，对同一表单字段，角色A的权限只读，角色B的权限可编辑，结果为可编辑
                result = MergeFormFieldPrivilege(result, temp, true); 
            }

            //表单字段活动权限判断
            if (activityId != null)
            {
                var temp = this.DataHelper.Where<SysFormPrivilege>(p => p.FormId == this.FormId && p.ActivityId == activityId)
                    .ToDictionary(p => p.FormFieldId.Value, p => (FormFieldPrivilege)p.DisplayPrivilege);

                //如果有活动权限，取活动权限和角色权限中的最小权限
                //如某人角色权限配置了可编辑，但在该活动中权限配置为只读，则最终为只读；反之亦然                
                result = MergeFormFieldPrivilege(result, temp, false);
            }

            return result;
        }

        #region 加载表单实例数据

        /// <summary>
        /// 加载预览用测试数据
        /// </summary>
        private void LoadPreivewTestData()
        {
            this.FormInstance = new SysFormInstance()
            {
                FormCode = "F0000000000001",
                FormDate = DateTime.Now,
                FormTitle = this.Form.FormTitle,
                FormDescription = this.Form.FormDescription,
            };
        }

        /// <summary>
        /// 加载默认的表单实例数据
        /// </summary>
        private void LoadDefaultFormInstanceData()
        {
            int id = this.DataHelper.GetNextIdentity_Int(string.Format("FORM{0}", this.FormId));
            string code = this.FormId.ToString().PadRight(10, '0') + id.ToString().PadLeft(10, '0');

            this.FormInstance = new SysFormInstance()
            {
                FormCode = code,
                FormDate = DateTime.Now,
                FormTitle = this.Form.FormTitle,
                FormDescription = this.Form.FormDescription,
            };
        }

        /// <summary>
        /// 加载表单实例
        /// </summary>
        private void LoadFormInstance()
        {
            this.lblFormCode.Text = this.FormInstance.FormCode;
            this.lblFormDate.Text = string.Format("{0:yyyy-MM-dd}", this.FormInstance.FormDate);

            if (!string.IsNullOrEmpty(this.FormInstance.FormTitle))
            {
                this.lblFormTitle.Text = Server.HtmlEncode(this.FormInstance.FormTitle);
            }
            else
            {
                this.lblFormTitle.Text = Server.HtmlEncode(this.Form.FormTitle);
            }

            if (!string.IsNullOrEmpty(this.FormInstance.FormDescription))
            {
                this.lblFormDescription.Text = Server.HtmlEncode(this.FormInstance.FormDescription).Replace("\r\n", "<br />").Replace("\n", "<br />");
            }
            else
            {
                this.lblFormDescription.Text = Server.HtmlEncode(this.Form.FormDescription).Replace("\r\n", "<br />").Replace("\n", "<br />");
            }
        }

        /// <summary>
        /// 加载表单实体对象
        /// </summary>
        private void LoadObjectData(Dictionary<long,IDrisionControl> controlDict)
        {
            List<SysFormField> ffList = this.DataHelper.Where<SysFormField>(p => p.FormId == this.FormId);
            SysEntity entity = this.BasePage.EntityCache.FindById<SysEntity>(this.EntityId);
            if (entity != null) //元数据预置实体，利用EntitySchema查询，给控件赋值
            {
                #region 元数据预置实体

                var es = IEntitySchemaHelper.Get(this.EntityId);
                if (es == null)
                {
                    throw new Exception("元数据实体EntitySchema找不到");
                }

                object obj = this.DataHelper.FindById(es.EntityType, this.ObjectId);
                foreach (var ff in ffList)
                {
                    ff.Field = BasePage.GetField(ff.FieldId);
                    if (controlDict.ContainsKey(ff.FormFieldId))
                    {
                        IDrisionControl control = controlDict[ff.FormFieldId];
                        object value = obj.GetPropertyValue(control.FieldName);
                        if (this.IsDetailPage)
                        {
                            FormPreviewHelper.SetValueForDetailPage(ff, control, value);
                        }
                        else
                        {
                            FormPreviewHelper.SetValue(ff, control, value);
                        }
                    }
                    else
                    {
                        throw new Exception("表单字段找不到对应控件");
                    }
                }

                #endregion
            }
            else
            {
                #region 自定义实体

                entity = this.DataHelper.FindById<SysEntity>(this.EntityId);
                Dictionary<string, object> valueDict = this.DataHelper.DynamicFindById(entity, this.ObjectId);
                foreach (var ff in ffList)
                {
                    ff.Field = BasePage.GetField(ff.FieldId);
                    if (controlDict.ContainsKey(ff.FormFieldId))
                    {
                        IDrisionControl control = controlDict[ff.FormFieldId];
                        if (!valueDict.ContainsKey(control.FieldName.ToLower()))
                        {
                            throw new Exception(string.Format("字段{0}找不到", control.FieldName));
                        }
                        object value = valueDict[control.FieldName.ToLower()];
                        if (this.IsDetailPage)
                        {
                            FormPreviewHelper.SetValueForDetailPage(ff, control, value);
                        }
                        else
                        {
                            FormPreviewHelper.SetValue(ff, control, value);
                        }
                    }
                    else
                    {
                        throw new Exception("表单字段找不到对应控件");
                    }
                }

                #endregion
            }
        }

        #region 审核意见

        /// <summary>
        /// 加载表单实例涉及的表单流程审核意见
        /// </summary>
        private void LoadProcessApproveHistory()
        {
            ProcessInstanceCacheFactory piCache = new ProcessInstanceCacheFactory(this.DataHelper);
            var piList = this.DataHelper.Set<SysProcessInstance>().Where(p => p.FormInstanceId == this.FormInstanceId).OrderBy(p => p.StartTime).ToList();
            var source = piList.Select(p => piCache.GetProcessInstanceCache(p.ProcessInstanceId)).ToList();
            this.rProcess.DataSource = source;
            this.rProcess.DataBind();
        }

        /// <summary>
        /// 活动实例的审核状态
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetCheckState(SysActivityInstance p)
        {
            if (p.InstanceStatus == (int)ActivityInstanceStatusEnum.Suspending)
            {
                return "审核中";
            }
            else
            {
                return GetCheckState(p.ApproveResult);
            }
        }

        /// <summary>
        /// 审核状态
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetCheckState(int? p)
        {
            if (p == null)
            {
                return "审核中";
            }
            else if (p == (int)Drision.Framework.Common.Workflow.ApproveResultEnum.Pass)
            {
                return "通过";
            }
            else
            {
                return "驳回";
            }
        }

        /// <summary>
        /// 绑定流程实例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rProcess_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lblProcess = e.Item.FindControl("lblProcess") as Label;
            Label lblApproveResult = e.Item.FindControl("lblApproveResult") as Label;
            Label lblStartTime = e.Item.FindControl("lblStartTime") as Label;
            Repeater rActivity = e.Item.FindControl("rActivity") as Repeater;

            SysProcessInstance pi = e.Item.DataItem as SysProcessInstance;
            lblProcess.Text = pi.Process.ProcessName;
            lblApproveResult.Text = GetCheckState(pi.ApproveResult);
            lblStartTime.Text = string.Format("{0:yyyy/MM/dd HH:mm}", pi.StartTime);

            var result = pi.ActivityInstances.Where(p => p.Activity.ActivityType == (int)ActivityType.Approve)
                .OrderBy(p => p.StartTime)
                .Select(p => new
                {
                    p.StartTime,
                    ActivityId = p.ActivityId,
                    ActivityName = p.Activity.ActivityName,
                    ActivityInstanceId = p.ActivityInstanceId,
                    AI = p,
                    ApproveResult = GetCheckState(p),
                }).ToList();
            rActivity.DataSource = result.ToList();
            rActivity.DataBind();
        }

        /// <summary>
        /// 绑定活动实例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rActivity_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpApproveGroup = e.Item.FindControl("rApproveGroup") as Repeater;
            SysActivityInstance ai = e.Item.DataItem.GetPropertyValue("AI") as SysActivityInstance;

            //组里所有活动实例对应的活动都是本活动
            var result = ai.ApproveGroups
                .Select(p => new
                {
                    Group = p,
                    ApproveGroupId = p.ApproveGroupId,
                    ParticipantName = p.ActivityParticipantId == null ? "直接指定" : ai.Activity.ActivityParticipants.FirstOrDefault(i => i.ActivityParticipantId == p.ActivityParticipantId).ProcessParticipant.ParticipantName,
                    ApproveResult = (ai.InstanceStatus == (int)ActivityInstanceStatusEnum.Completed && p.ApproveResult == null) ? "未参与审核" : GetCheckState(p.ApproveResult),
                }).ToList();
            rpApproveGroup.DataSource = result;
            rpApproveGroup.DataBind();
        }

        /// <summary>
        /// 绑定审核分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rApproveGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rUser = e.Item.FindControl("rUser") as Repeater;
            SysWorkItemApproveGroup group = e.Item.DataItem.GetPropertyValue("Group") as SysWorkItemApproveGroup;
                    
            //参与审核的人
            var result = this.DataHelper.Where<SysApproveActivityData>(p => p.ApproveGroupId == group.ApproveGroupId)
                .Select(p => new UserApproveComment()
                {
                    WorkItemId = p.WorkItemId,
                    User_Name = this.DataHelper.FindById<T_User>(p.ApproveUserId).User_Name,
                    ApproveComment = p.ApproveComment,
                    AddingUser = p.IsAdded == true ? "[加签]" : "",
                    ProxyUser = p.IsProxy == true ? "[代理]" : "",
                    ApproveResult = GetCheckState(p.ApproveResult),
                    ApproveDate = string.Format("{0:yyyy/MM/dd HH:mm}", p.ApproveTime),
                }).ToList();

            //未参与审核的人
            var idList = result.Select(p => p.WorkItemId);
            var otherList = group.WorkItems.Where(p => !idList.Contains(p.WorkItemId));
            int? state = null;
            foreach (var other in otherList)
            {
                result.Add(new UserApproveComment()
                {
                    WorkItemId = other.WorkItemId,
                    User_Name = this.DataHelper.FindById<T_User>(other.OwnerId).User_Name,
                    ApproveComment = other.Status == (int)WorkItemStatus.CancelledBySystem ? "未参与审核" : GetCheckState(state),
                    AddingUser = string.Empty,
                    ProxyUser = string.Empty,
                    ApproveResult = other.Status == (int)WorkItemStatus.CancelledBySystem ? "未参与审核" : GetCheckState(state),
                });
            }

            rUser.DataSource = result;
            rUser.DataBind();
        }

        #endregion

        #endregion

        #region 加载表单控件

        /// <summary>
        /// 加载表单内容
        /// </summary>
        private Dictionary<long,IDrisionControl> LoadFormContentControl(Dictionary<long,FormFieldPrivilege> fpDict)
        {
            Dictionary<long, IDrisionControl> result = new Dictionary<long, IDrisionControl>();
            if (fpDict == null)
            {
                fpDict = new Dictionary<long, FormFieldPrivilege>();
            }

            var fsList = this.DataHelper.Set<SysFormFieldSection>()
                .Where(p => p.FormId == this.FormId)
                .OrderBy(p => p.DisplayOrder).ToList();

            foreach (var fs in fsList)
            {
                var section = LoadFormSection(fs,fpDict, ref result);
                this.pFormContent.Controls.Add(section);
            }

            return result;
        }

        /// <summary>
        /// 加载表单区域
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        private Control LoadFormSection(SysFormFieldSection fs,Dictionary<long,FormFieldPrivilege> fpDict, ref Dictionary<long, IDrisionControl> controlDict)
        {
            HtmlGenericControl section = new HtmlGenericControl("div");
            section.Attributes["class"] = "formFieldSection";

            var ffList = this.DataHelper.Set<SysFormField>()
                .Where(p => p.FormSectionId == fs.FormSectionId)
                .OrderBy(p => p.DisplayOrder).ToList();
            var inVisibleCount = 0;
            foreach (var ff in ffList)
            {
                ff.Field = BasePage.GetField(ff.FieldId);

                //权限
                FormFieldPrivilege fp = FormFieldPrivilege.ReadWrite;
                if (fpDict.ContainsKey(ff.FormFieldId))
                {
                    fp = fpDict[ff.FormFieldId];
                    if (fp == FormFieldPrivilege.Invisible)
                    {
                        inVisibleCount++;
                    }
                }
                var field = LoadFormField(ff,fp,ref controlDict);

                section.Controls.Add(field);
            }
            var cl = new HtmlGenericControl("div");
            cl.Attributes["class"] = "cl";
            section.Controls.Add(cl);

            //如果区域下所有的字段都是不可见的，将区域设为不可见
            if (inVisibleCount == ffList.Count)
            {
                section.Style[HtmlTextWriterStyle.Display] = "none";
            }

            return section;
        }

        /// <summary>
        /// 获得表单字段的显示名称
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetFormFieldDisplayText(SysFormField p)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(p.CustomLabel)) //自定义标签
            {
                result = p.CustomLabel;
            }
            else
            {
                var field = this.BasePage.GetField(p.FieldId);
                if (field != null)
                {
                    result = field.DisplayText;
                }
            }
            return result;
        }

        /// <summary>
        /// 加载表单字段
        /// </summary>
        /// <param name="ff"></param>
        /// <returns></returns>
        private Control LoadFormField(SysFormField ff, FormFieldPrivilege fp, ref Dictionary<long, IDrisionControl> controlDict)
        {
            HtmlGenericControl field = new HtmlGenericControl("div");
            field.Attributes["class"] = "formField";

            //表单字段前面的文字说明
            HtmlGenericControl fieldDisplayText = new HtmlGenericControl("span");
            fieldDisplayText.Attributes["class"] = "formFieldDisplayText";
            fieldDisplayText.InnerText = GetFormFieldDisplayText(ff);
            field.Controls.Add(fieldDisplayText);

            //是否必输前面的星号
            HtmlGenericControl star = new HtmlGenericControl("span");
            star.Attributes["class"] = "left_star";
            if (!(ff.IsNullable ?? true))
            {
                star.InnerHtml = "*";
            }
            else
            {
                star.InnerHtml = "&nbsp;";
            }
            field.Controls.Add(star);

            //控件
            Control control;
            if (this.IsDetailPage) //详情页面的解析和其它不同
            {
                control = FormPreviewHelper.LoadControlForDetailPage(ff);
            }
            else
            {
                control = FormPreviewHelper.LoadControl(ff);
            }

            //权限处理
            if (fp == FormFieldPrivilege.Invisible)
            {
                control.Visible = false;
                field.Style[HtmlTextWriterStyle.Display] = "none";                
            }
            else if (fp == FormFieldPrivilege.ReadOnly)
            {
                //(control as IReadOnlyControl).ReadOnly = true;
            }

            field.Controls.Add(control);
            controlDict[ff.FormFieldId] = (control as IDrisionControl); //记住所有的IDrisionControl，后面有可能要赋值
            
            return field;
        }

        #endregion                

        /// <summary>
        /// 保存表单实体，和表单实体对象
        /// </summary>
        /// <param name="db"></param>
        public int UpdateFormInstance(BizDataContext db)
        {
            SysFormInstance fi = db.FindById<SysFormInstance>(this.FormInstanceId);
            if (fi == null)
            {
                throw new Exception("表单实例不存在");
            }

            if (fi.State != (int)FormInstanceState.Approving)
            {
                fi.State = (int)FormInstanceState.New; //2013-10-8 zhumin 重新保存后状态改为新增，审核中除外
            }
            fi.UpdateUserId = this.BasePage.LoginUserID;
            fi.UpdateTime = DateTime.Now;

            //收集实体字段的值，创建实体对象            
            List<SysFormField> ffList = db.Where<SysFormField>(p => p.FormId == this.FormId);
            Dictionary<string, object> valueDict = new Dictionary<string, object>();
            foreach (var ff in ffList)
            {
                string controlId = string.Format("ff_{0}", ff.FormFieldId);
                IDrisionControl control = this.BasePage.GetControlById<IDrisionControl>(controlId);
                if (control == null)
                {
                    throw new Exception("表单控件解析出错");
                }
                valueDict[control.FieldName] = control.GetValue();
            }
            SysEntity entity = this.BasePage.EntityCache.FindById<SysEntity>(this.EntityId);
            if (entity != null) //元数据预置实体，利用EntitySchema创建对象插入
            {
                var es = IEntitySchemaHelper.Get(this.EntityId);
                if (es == null)
                {
                    throw new Exception("元数据实体EntitySchema找不到");
                }

                object obj = db.FindById(es.EntityType, this.ObjectId);
                List<string> cols = new List<string>();
                foreach (var p in valueDict)
                {
                    if (obj.GetPropertyValue(p.Key) != p.Value)
                    {
                        obj.SetPropertyConvertValue(p.Key, p.Value);
                        cols.Add(p.Key);
                    }
                }

                db.UpdatePartial(obj, cols);
            }
            else //自定义实体
            {
                entity = this.DataHelper.FindById<SysEntity>(this.EntityId);
                db.DynamicUpdate(entity, this.ObjectId, valueDict);           
            }

            db.UpdatePartial(fi, p => new {p.State, p.UpdateTime, p.UpdateUserId });
            return fi.FormInstanceId;
        }

        /// <summary>
        /// 新增表单实例，新增表单实体对象
        /// </summary>
        public int SaveFormInstance(BizDataContext db)
        {
            SysFormInstance fi = new SysFormInstance()
            {
                FormInstanceId = db.GetNextIdentity_Int(),
                FormId = this.FormId,
                //--------------//
                FormDate = Convert.ToDateTime(this.lblFormDate.Text.Trim()),
                FormCode = this.lblFormCode.Text.Trim(),
                FormDescription = Server.HtmlDecode(this.lblFormDescription.Text.Trim()).Replace("<br />","\r\n"),
                FormTitle = this.lblFormTitle.Text.Trim(),
                State = (int)FormInstanceState.New,
                //---------------//
                CreateTime = DateTime.Now,
                CreateUserId = this.BasePage.LoginUserID,
                OwnerId = this.BasePage.LoginUserID,
            };

            #region 收集实体字段的值，创建实体对象            
            List<SysFormField> ffList = db.Where<SysFormField>(p => p.FormId == this.FormId);
            Dictionary<string, object> valueDict = new Dictionary<string, object>();
            foreach (var ff in ffList)
            {
                string controlId = string.Format("ff_{0}", ff.FormFieldId);
                IDrisionControl control = this.BasePage.GetControlById<IDrisionControl>(controlId);
                if (control == null)
                {
                    throw new Exception("表单控件解析出错");
                }
                valueDict[control.FieldName] = control.GetValue();
            }
            SysEntity entity = this.BasePage.EntityCache.FindById<SysEntity>(this.EntityId);
            if (entity != null) //元数据预置实体，利用EntitySchema创建对象插入
            {
                var es = IEntitySchemaHelper.Get(this.EntityId);
                if (es == null)
                {
                    throw new Exception("元数据实体EntitySchema找不到");
                }

                object obj = es.CreateInstance();
                int objectId = db.GetNextIdentity_Int();
                obj.SetPropertyConvertValue(es.KeyName, objectId); //主键
                foreach (var p in valueDict)
                {
                    obj.SetPropertyConvertValue(p.Key, p.Value);
                }

                //一些默认字段
                if (es.RequiredLevel() != RequireLevelEnum.PlatForm)
                {
                    if (obj.GetPropertyValue(ConstFieldNames.CreateTime) == null)
                    {
                        obj.SetPropertyConvertValue(ConstFieldNames.CreateTime, DateTime.Now);
                    }
                    if (obj.GetPropertyValue(ConstFieldNames.CreateUserId) == null)
                    {
                        obj.SetPropertyConvertValue(ConstFieldNames.CreateUserId, this.BasePage.LoginUserID);
                    }
                    if (obj.GetPropertyValue(ConstFieldNames.OwnerId) == null)
                    {
                        obj.SetPropertyConvertValue(ConstFieldNames.OwnerId, this.BasePage.LoginUserID);
                    }
                    if (obj.GetPropertyValue(ConstFieldNames.State) == null)
                    {
                        obj.SetPropertyConvertValue(ConstFieldNames.State, 0);
                    }
                    if (obj.GetPropertyValue(ConstFieldNames.StateDetail) == null)
                    {
                        obj.SetPropertyConvertValue(ConstFieldNames.StateDetail, 0);
                    }
                }

                db.Insert(obj);

                fi.ObjectId = objectId;
            }
            else //自定义实体
            {
                entity = this.DataHelper.FindById<SysEntity>(this.EntityId);
                fi.ObjectId = db.DynamicInsert(entity, valueDict);
            }
            #endregion
            
            db.Insert(fi);
            return fi.FormInstanceId;
        }
    }
}