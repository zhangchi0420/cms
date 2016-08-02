using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using System.Web.Script.Serialization;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormProcessDesigner_FirstStep : BasePage
    {
        protected long ProcessId
        {
            get { return VS<long>("ProcessId"); }
            set { VS<long>("ProcessId", value); }
        }        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }
                else
                {
                    if (Request["__EventArgument"] == "refresh")
                    {
                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndRedirect(ex, "FormProcessQuery.aspx");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            var formList = this.DataHelper.Set<SysForm>().Where(p => p.State == (int)FormState.StartUsed).ToList();
            this.ddlForm.DataSource = formList;
            this.ddlForm.DataBind();

            long? id = QueryString<long?>("id");
            if (id == null)
            {
                this.txtProcessName.SetValue(null);
                this.ddlForm.SetValue(null);

                this.ProcessId = this.DataHelper.GetNextIdentity();
            }
            else
            {
                SysProcess process = this.DataHelper.FindById<SysProcess>(id);
                if (process != null)
                {
                    //不是“新增”状态，需要复制一份
                    if (process.ProcessStatus != (int)ProcessState.Created)
                    {
                        FormProcessPublishHelper helper = new FormProcessPublishHelper(process);
                        SysProcess newProcess = helper.CopyProcess();

                        this.ProcessId = newProcess.ProcessId;
                        this.txtProcessName.Text = newProcess.ProcessName;
                        this.ddlForm.SetValue(newProcess.FormId);
                    }
                    else
                    {
                        this.ProcessId = id.Value;
                        this.txtProcessName.Text = process.ProcessName;
                        this.ddlForm.SetValue(process.FormId);
                    }
                }
                else
                {
                    throw new Exception("参数不正确");
                }
            }

            this.hc.Value = this.ProcessId.ToString();
            this.hc.Text = this.ClientScript.GetPostBackEventReference(this.hc, "refresh");

            BindGrid();
            BindDropDown();
        }

        private string GetUserName(int? userId)
        {
            T_User user = this.DataHelper.FindById<T_User>(userId);
            if (user != null)
            {
                return user.User_Name;
            }
            return string.Empty;
        }

        private string GetRoleName(int? roleId)
        {
            T_Role role = this.DataHelper.FindById<T_Role>(roleId);
            if (role != null)
            {
                return role.Role_Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 绑定流程参与人
        /// </summary>
        private void BindGrid()
        {
            var source = this.DataHelper.Set<SysProcessParticipant>()
                .Where(p => p.ProcessId == this.ProcessId && p.FunctionType != (int)ParticipantFunctionType.StartUser)
                .Select(p => new
                {
                    p.ParticipantId,
                    p.FunctionType,
                    ParticipantFunctionType = EnumHelper.GetDescription((ParticipantFunctionType)p.FunctionType),
                    UserName = GetUserName(p.Param_UserId),
                    RoleName = GetRoleName(p.Param_RoleId),
                    p.ParticipantName,
                });

            this.gcProcessParticipant.DataSource = source.ToList();
            this.gcProcessParticipant.DataBind();
        }

        private void BindDropDown()
        {
            this.ddlUser.DataSource = this.DataHelper.Set<T_User>()
                .Where(p=>p.User_Status != (int)Enum.EffectiveFlagEnum.No).ToList();
            this.ddlUser.DataBind();

            this.ddlRole.DataSource = this.DataHelper.Set<T_Role>()
                .Where(p=>p.Role_Status != (int)Enum.StopFlagEnum.Yes).ToList();
            this.ddlRole.DataBind();
        }

        /// <summary>
        /// 保存流程参与人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbc_CallBack(object sender, CallBackEventArgs e)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string errorMsg = string.Empty; //错误消息
            bool isError = false;

            try
            {
                int? functionType = e.Context["FunctionType"].ToStringNullable().ToIntNullable();
                int? userId = e.Context["UserId"].ToStringNullable().ToIntNullable();
                int? roleId = e.Context["RoleId"].ToStringNullable().ToIntNullable();
                long processId = e.Context["ProcessId"].ToLong();
                string participantName = string.Empty;

                if (functionType == null)
                {
                    throw new Exception("参与人类型不能为空");
                }
                if (functionType == (int)ParticipantFunctionType.SpecialUser)
                {                    
                    T_User user = this.DataHelper.FindById<T_User>(userId);
                    if (user == null)
                    {
                        throw new Exception("请指定具体人员");
                    }
                    participantName = user.User_Name;
                }
                if (functionType == (int)ParticipantFunctionType.SpecialRole)
                {
                    T_Role role = this.DataHelper.FindById<T_Role>(roleId);
                    if (role == null)
                    {
                        throw new Exception("请指定具体角色");
                    }
                    participantName = string.Format("{0}角色", role.Role_Name);
                }
                if (functionType == (int)ParticipantFunctionType.DIRECT_MANAGER)
                {
                    participantName = "直属主管";
                }

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        int count = db.Set<SysProcessParticipant>().Where(p =>
                            p.ProcessId == processId && p.FunctionType == functionType && p.Param_RoleId == roleId && p.Param_UserId == userId).Count();
                        if (count > 0)
                        {
                            throw new Exception("已存在相同的流程参与人");
                        }

                        long? param_partId = null;
                        if (functionType == (int)ParticipantFunctionType.DIRECT_MANAGER)
                        {
                            SysProcessParticipant pStartUser = new SysProcessParticipant()
                            {
                                ParticipantId = db.GetNextIdentity(),
                                ProcessId = processId,
                                FunctionType = (int)ParticipantFunctionType.StartUser,
                                ParticipantName = "流程启动人",
                            };
                            param_partId = pStartUser.ParticipantId;
                            db.Insert(pStartUser);
                        }

                        SysProcessParticipant pCurrent = new SysProcessParticipant()
                        {
                            ParticipantId = db.GetNextIdentity(),
                            ProcessId = processId,
                            FunctionType = functionType,
                            Param_UserId = userId,
                            Param_RoleId = roleId,
                            Param_ParticipantId = param_partId,
                            ParticipantName = participantName,
                        };
                        db.Insert(pCurrent);
                        
                    }
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                isError = true;
                errorMsg = ex.Message;
            }

            string result = js.Serialize(new
            {
                IsError = isError,
                Error = errorMsg,
            });
            e.Result = result;
        }

        /// <summary>
        /// 删除流程参与人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                long? id = (sender as LinkButton).CommandArgument.ToLongNullable();
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        SysProcessParticipant p = db.FindById<SysProcessParticipant>(id);
                        if (p != null)
                        {
                            //同步删除关联的活动参与人
                            var apList = db.Where<SysActivityParticipant>(x => x.ParticipantId == id);
                            foreach (var ap in apList)
                            {
                                db.Delete(ap);
                            }

                            db.Delete(p);
                        }
                    }
                    ts.Complete();
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            try
            {
                string processName = this.txtProcessName.Text.Trim();
                long? formId = this.ddlForm.SelectedValue.ToLongNullable();
                SysForm form = this.DataHelper.FindById<SysForm>(formId);
                if (form == null)
                {
                    throw new Exception("流程关联的表单不能为空");
                }
                if (string.IsNullOrEmpty(processName))
                {
                    throw new Exception("流程名称不能为空");
                }
                
                int count = 0;
                SysProcess process = this.DataHelper.FindById<SysProcess>(this.ProcessId);
                if (process != null)
                {
                    count = this.DataHelper.Set<SysProcess>().Where(p => p.ProcessName == processName
                    && p.ProcessType != process.ProcessType).Count();
                }
                else
                {
                    count = this.DataHelper.Set<SysProcess>().Where(p => p.ProcessName == processName).Count();
                }
                if (count > 0)
                {
                    throw new Exception("流程名称重复，已有其它流程使用");
                }
                
                if (process != null)
                {
                    process.ProcessName = processName;
                    process.FormId = formId;
                    process.EntityId = form.EntityId;

                    this.DataHelper.UpdatePartial(process, p => new { p.ProcessName, p.FormId,p.EntityId });

                    //清除缓存
                    Drision.Framework.WorkflowEngineCore.Cache.ProcessCacheFactory pc = new WorkflowEngineCore.Cache.ProcessCacheFactory(this.DataHelper);
                    pc.ClearCache(process.ProcessId);
                }
                else
                {
                    long approveEntityId = this.EntityCache.SysEntity.FirstOrDefault(p => p.EntityName == typeof(SysApproveActivityData).Name).EntityId;

                    SysProcess temp = new SysProcess()
                    {
                        ProcessId = this.ProcessId,
                        ProcessName = processName,
                        FormId = formId,
                        EntityId = form.EntityId,
                        ActivityEntityId = approveEntityId,
                        ProcessCategory = (int)ProcessCategory.FormApprove,
                        ProcessStatus = (int)ProcessState.Created,
                        ProcessType = this.ProcessId,
                        ProcessVersion = "1",
                        ProcessHeight = 692,
                        ProcessWidth = 692,
                    };
                    this.DataHelper.Insert(temp);
                }

                Response.Redirect(string.Format("FormProcessDesigner_SecondStep.aspx?id={0}", this.ProcessId));
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }
}