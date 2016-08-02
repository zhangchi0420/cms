using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Entity;
using Drision.Framework.Enum;
using Drision.Framework.Common;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormDesigner_ThirdStep : BasePage
    {
        protected long? FormId
        {
            get { return VS<long?>("FormId"); }
            set { VS<long?>("FormId", value); }
        }

        protected long? EntityId
        {
            get { return VS<long?>("EntityId"); }
            set { VS<long?>("EntityId", value); }
        }

        protected int? SelectedRoleId
        {
            get { return VS<int?>("RoleId"); }
            set { VS<int?>("RoleId", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }                
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndRedirect(ex, "FormQuery.aspx");
            }
        }

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
            this.EntityId = form.EntityId.Value;

            var roleList = this.DataHelper.Set<T_Role>().ToList();
            this.ddlRoleList.DataSource = roleList;
            this.ddlRoleList.DataBind();
        }

        /// <summary>
        /// 变换角色
        /// </summary>
        protected void ddlRoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int? id = this.ddlRoleList.SelectedValue.ToIntNullable();
                if (id != null)
                {
                    SelectRole(id);
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 选择某个角色
        /// </summary>
        /// <param name="id"></param>
        private void SelectRole(int? id)
        {
            this.SelectedRoleId = id;
            BindFormField();
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
                var field = GetField(p.FieldId);
                if (field != null)
                {
                    result = field.DisplayText;
                }
            }
            return result;
        }

        private bool[] GetFormFieldPrivilege(Dictionary<long?, SysFormRolePrivilege> fpDict, SysFormField ff)
        {
            bool[] result = new bool[3] { false, false, false };
            if (fpDict.ContainsKey(ff.FormFieldId))
            {
                var fp = fpDict[ff.FormFieldId];
                result[fp.DisplayPrivilege ?? (int)FormFieldPrivilege.ReadWrite] = true;
            }
            else
            {
                result[(int)FormFieldPrivilege.ReadWrite] = true;
            }
            return result;
        }


        /// <summary>
        /// 绑定表单字段
        /// </summary>
        private void BindFormField()
        {
            //现有配置
            var fpDict = this.DataHelper.Set<SysFormRolePrivilege>()
                .Where(p => p.RoleId == this.SelectedRoleId
                    && p.FormId == this.FormId).ToList().ToDictionary(p => p.FormFieldId);


            //所有该表单的字段，根据区域分组
            var formFieldList = this.DataHelper.Where<SysFormField>(p => p.FormId == this.FormId)
                .Select(x => new
                {
                    x.FormSectionId,
                    x.FormFieldId,
                    DisplayText = GetFormFieldDisplayText(x),
                    x.DisplayOrder,
                    Privilege = GetFormFieldPrivilege(fpDict, x),
                }).ToList();

            var formFieldDict = formFieldList.GroupBy(p => p.FormSectionId)
                .ToDictionary(p => p.Key, p => p.Select(x => new
                {
                    x.FormFieldId,
                    x.DisplayText,
                    x.DisplayOrder,
                    x.Privilege,
                    Invisible = x.Privilege[(int)FormFieldPrivilege.Invisible],
                    ReadOnly = x.Privilege[(int)FormFieldPrivilege.ReadOnly],
                    ReadWrite = x.Privilege[(int)FormFieldPrivilege.ReadWrite],
                }).OrderBy(x => x.DisplayOrder).ToList());


            //所有该表单的区域
            var sectionList = this.DataHelper.Set<SysFormFieldSection>()
                .Where(p => p.FormId == this.FormId)
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new
                {
                    p.FormId,
                    p.FormSectionId,
                    p.DisplayOrder,
                    Fields = formFieldDict.ContainsKey(p.FormSectionId) ? formFieldDict[p.FormSectionId] : null,
                });

            this.rSection.DataSource = sectionList;
            this.rSection.DataBind();
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
                if (this.SelectedRoleId == null)
                {
                    throw new Exception("请选择角色");
                }

                //现有配置
                var fpDict = this.DataHelper.Set<SysFormRolePrivilege>()
                    .Where(p => p.RoleId == this.SelectedRoleId
                        && p.FormId == this.FormId).ToList().ToDictionary(p => p.FormFieldId);

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        foreach (RepeaterItem section in rSection.Items)
                        {
                            Repeater rField = section.FindControl("rField") as Repeater;
                            foreach (RepeaterItem field in rField.Items)
                            {
                                var hf = field.FindControl("hf") as HiddenField;
                                var ccInvisible = field.FindControl("ccInvisible") as RadioButton;
                                var ccReadOnly = field.FindControl("ccReadOnly") as RadioButton;
                                var ccReadWrite = field.FindControl("ccReadWrite") as RadioButton;

                                if (hf != null && ccInvisible != null && ccReadOnly != null && ccReadWrite != null)
                                {
                                    var id = hf.Value.ToLong();
                                    FormFieldPrivilege privilege = FormFieldPrivilege.Invisible;
                                    if (ccReadOnly.Checked)
                                    {
                                        privilege = FormFieldPrivilege.ReadOnly;
                                    }
                                    else if (ccReadWrite.Checked)
                                    {
                                        privilege = FormFieldPrivilege.ReadWrite;
                                    }

                                    if (fpDict.ContainsKey(id))
                                    {
                                        var fp = fpDict[id];
                                        if (fp.DisplayPrivilege != (int)privilege)
                                        {
                                            fp.DisplayPrivilege = (int)privilege;
                                            db.UpdatePartial(fp, p => new { p.DisplayPrivilege });
                                        }
                                    }
                                    else
                                    {
                                        SysFormRolePrivilege fp = new SysFormRolePrivilege()
                                        {
                                            PrivilegeId = db.GetNextIdentity(),
                                            FormFieldId = id,
                                            FormId = this.FormId,
                                            RoleId = this.SelectedRoleId,
                                            DisplayPrivilege = (int)privilege,
                                            CreateTime = DateTime.Now,
                                            CreateUserId = this.LoginUserID,
                                        };
                                        db.Insert(fp);
                                    }
                                }
                            }
                        }
                    }
                    ts.Complete();
                }

                this.AjaxAlertAndEnableButton("保存成功");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }


        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormDesigner_SecondStep.aspx?id={0}", this.FormId));
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormDesigner_FourthStep.aspx?id={0}", this.FormId));
        }
    }
}