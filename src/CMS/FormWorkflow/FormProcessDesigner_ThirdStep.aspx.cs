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
    public partial class FormProcessDesigner_ThirdStep : BasePage
    {
        #region 属性

        /// <summary>
        /// 流程ID
        /// </summary>
        protected long ProcessId
        {
            get { return VS<long>("ProcessId"); }
            set { VS<long>("ProcessId", value); }
        }

        /// <summary>
        /// 表单ID
        /// </summary>
        protected long FormId
        {
            get { return VS<long>("FormId"); }
            set { VS<long>("FormId", value); }
        }

        /// <summary>
        /// 当前选中的活动ID        
        /// </summary>
        protected long SelectedActivityId
        {
            get { return this.hc.Value.ToLong(); }
        }

        /// <summary>
        /// 当前选中的活动
        /// </summary>
        protected SysActivity SelectedActivity
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    //选择某活动后由前台手动回发
                    if (Request["__EVENTARGUMENT"] == "select")
                    {
                        SelectActivity(this.hc.Value.ToLong());
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
            long? id = QueryString<long?>("id");

            SysProcess process = this.DataHelper.FindById<SysProcess>(id);
            if (process != null && process.FormId != null)
            {
                this.ProcessId = id.Value;
                this.FormId = process.FormId.Value;

                //hc.Text用于回发
                //hc.Value记录当前选中的活动，由前台选中某活动，或调用后台SelectActivity方法更新
                this.hc.Text = this.ClientScript.GetPostBackEventReference(this.hc, "select");

                //选中第一个活动
                var a = this.DataHelper.Set<SysActivity>()
                        .Where(p => p.ProcessId == this.ProcessId && p.ActivityType == (int)ActivityType.Approve)
                        .OrderBy(p => p.DisplayOrder).FirstOrDefault();
                SelectActivity(a);
            }
            else
            {
                throw new Exception("参数不正确");
            }
        }

        /// <summary>
        /// 绑定活动列表（只显示审核活动）
        /// </summary>
        private void BindActivityList()
        {
            var source = from a in this.DataHelper.Set<SysActivity>()
                         where a.ProcessId == this.ProcessId
                         && a.ActivityType == (int)ActivityType.Approve
                         orderby a.DisplayOrder
                         select new
                         {
                             a.DisplayOrder,
                             a.ActivityId,
                             a.ActivityName,
                             SelectClass = a.ActivityId == this.SelectedActivityId ? "selected" : "unselected",
                         };

            this.rActivity.DataSource = source.ToList();
            this.rActivity.DataBind();
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

        private bool[] GetFormFieldPrivilege(Dictionary<long?,SysFormPrivilege> fpDict, SysFormField ff)
        {
            bool[] result = new bool[3] { false,false,false };
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
            var fpDict = this.DataHelper.Set<SysFormPrivilege>()
                .Where(p => p.ProcessId == this.ProcessId && p.ActivityId == this.SelectedActivityId
                    && p.FormId == this.FormId).ToList().ToDictionary(p => p.FormFieldId);


            //所有该表单的字段，根据区域分组
            var formFieldList = this.DataHelper.Where<SysFormField>(p => p.FormId == this.FormId)
                .Select(x => new
                {
                    x.FormSectionId,
                    x.FormFieldId,
                    DisplayText = GetFormFieldDisplayText(x),
                    x.DisplayOrder,
                    Privilege = GetFormFieldPrivilege(fpDict,x),
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
        /// 选中某个活动
        /// </summary>
        /// <param name="activity"></param>
        private void SelectActivity(SysActivity activity)
        {
            if (activity == null)
            {
                throw new Exception("选择的活动不存在");
            }

            this.SelectedActivity = activity;
            this.hc.Value = activity.ActivityId.ToString();

            //重新绑定活动列表（更新选中状态）
            BindActivityList();

            //重新绑定表单字段
            BindFormField();
        }

        /// <summary>
        /// 选中某个活动
        /// </summary>
        /// <param name="activityId"></param>
        private void SelectActivity(long activityId)
        {
            SysActivity activity = this.DataHelper.FindById<SysActivity>(activityId);
            SelectActivity(activity);
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
                //现有配置
                var fpDict = this.DataHelper.Set<SysFormPrivilege>()
                    .Where(p => p.ProcessId == this.ProcessId && p.ActivityId == this.SelectedActivityId
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
                                        SysFormPrivilege fp = new SysFormPrivilege()
                                        {
                                            PrivilegeId = db.GetNextIdentity(),
                                            FormFieldId = id,
                                            FormId = this.FormId,
                                            ProcessId = this.ProcessId,
                                            ActivityId = this.SelectedActivityId,
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
            Response.Redirect(string.Format("FormProcessDesigner_SecondStep.aspx?id={0}", this.ProcessId));
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormProcessDesigner_FourthStep.aspx?id={0}", this.ProcessId));
        }
    }
}