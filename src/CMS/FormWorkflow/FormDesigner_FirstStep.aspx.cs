using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Entity;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormDesigner_FirstStep : BasePage
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
            foreach (var p in this.EntityCache.SysEntity.OrderBy(p => p.DisplayText))
            {
                this.ddlSysEntity.Items.Add(new ComboItem()
                {
                    Text = p.DisplayText,
                    Value = p.EntityId.ToString(),
                });
            }

            foreach (var p in this.DataHelper.Set<SysEntity>()
                .Where(p=>p.IsFormEntity == true)//2013-9-24 zhumin
                .OrderBy(p => p.DisplayText))
            {
                this.ddlCustomEntity.Items.Add(new ComboItem()
                {
                    Text = p.DisplayText,
                    Value = p.EntityId.ToString(),
                });
            }

            long? id = QueryString<long?>("id");
            if (id == null)
            {
                this.ddlSysEntity.SetValue(null);
                this.txtFormName.SetValue(null);
                this.txtSysEntity.SetValue(null);
                this.lblCreateUser.Text = this.LoginUser.User_Name;
                this.ddlEntityType.SetValue(0);
                this.txtFormTitle.SetValue(null);
                this.txtFormDescription.SetValue(null);
            }
            else
            {
                SysForm form = this.DataHelper.FindById<SysForm>(id);
                if (form != null)
                {
                    this.FormId = id;

                    T_User user = this.DataHelper.FindById<T_User>(form.CreateUserId);
                    if (user != null)
                    {
                        this.lblCreateUser.Text = user.User_Name;
                    }

                    SysEntity tempEntity = EntityCache.FindById<SysEntity>(form.EntityId);
                    if (tempEntity == null)
                    {
                        this.ddlEntityType.SetValue(1);
                        this.ddlCustomEntity.SetValue(form.EntityId);
                    }
                    else
                    {
                        this.ddlEntityType.SetValue(0);
                        this.ddlSysEntity.SetValue(form.EntityId);
                    }
                    this.txtFormName.Text = form.FormName;
                    this.txtFormTitle.Text = form.FormTitle;
                    this.txtFormDescription.Text = form.FormDescription;

                    this.EntityId = form.EntityId;
                }
                else
                {
                    throw new Exception("参数不正确");
                }
            }            
        }

        protected void ddlEntityType_TextChanged(object sender, EventArgs e)
        {
            switch (this.ddlEntityType.SelectedValue)
            {
                case "0":
                    this.ddlSysEntity.Visible = true;
                    this.ddlCustomEntity.Visible = false;
                    this.txtSysEntity.Visible = false;
                    break;
                case "1":
                    this.ddlSysEntity.Visible = false;
                    this.ddlCustomEntity.Visible = true;
                    this.txtSysEntity.Visible = false;
                    break;
                case "2":
                    this.ddlSysEntity.Visible = false;
                    this.ddlCustomEntity.Visible = false;
                    this.txtSysEntity.Visible = true;
                    break;
                default: break;
            }

            this.txtSysEntity.SetValue(null);
            this.ddlSysEntity.SetValue(null);
            this.ddlCustomEntity.SetValue(null);
            this.txtSysEntityName.SetValue(null);
            this.EntityId = null;
        }

        protected void ddlSysEntity_TextChanged(object sender, EventArgs e)
        {
            long id = this.ddlSysEntity.SelectedValue.ToLong();
            SysEntity entity = EntityCache.FindById<SysEntity>(id);
            if (entity != null)
            {
                this.txtSysEntityName.Text = entity.EntityName;
                this.EntityId = id;
            }
        }

        protected void ddlCustomEntity_TextChanged(object sender, EventArgs e)
        {
            long id = this.ddlCustomEntity.SelectedValue.ToLong();
            SysEntity entity = this.DataHelper.FindById<SysEntity>(id);
            if (entity != null)
            {
                this.txtSysEntityName.Text = entity.EntityName;
                this.EntityId = id;
            }
        }       

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            try
            {
                string entityType = this.ddlEntityType.SelectedValue;
                string displayText = this.txtSysEntity.Text.Trim();
                string entityName = "T_" + displayText.ToPingYin();
                string formName = this.txtFormName.Text.Trim();
                string formTitle = this.txtFormTitle.Text.Trim();
                string formDecription = this.txtFormDescription.Text.Trim();

                if (string.IsNullOrEmpty(formName))
                {
                    throw new Exception("表单名称不能为空");
                }

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        switch (entityType)
                        {
                            case "0":
                            case "1":
                                if (this.EntityId == null)
                                {
                                    throw new Exception("关联实体不能为空");
                                }
                                break;
                            case "2":
                                if (string.IsNullOrEmpty(displayText))
                                {
                                    throw new Exception("关联实体不能为空");
                                }
                                if (string.IsNullOrEmpty(entityName))
                                {
                                    throw new Exception("关联实体表名不能为空");
                                }
                                SysEntity tempEntity = db.FirstOrDefault<SysEntity>(p =>
                                    p.EntityName == entityName || p.DisplayText == displayText);
                                if (tempEntity != null)
                                {
                                    throw new Exception("当前新增的关联实体名称已存在");
                                }
                                tempEntity = new SysEntity()
                                {
                                    EntityId = db.GetNextIdentity(),
                                    EntityName = entityName,
                                    DisplayText = displayText,
                                    Description = displayText,
                                    IsFormEntity = true,
                                    CreateTime = DateTime.Now,
                                };
                                db.Insert(tempEntity);
                                this.EntityId = tempEntity.EntityId;

                                //默认加一个主键字段
                                SysField tempField = new SysField()
                                {
                                    FieldId = db.GetNextIdentity(),
                                    EntityId = tempEntity.EntityId,
                                    FieldName = displayText.ToPingYin() + "_Id",
                                    DisplayText = displayText + "ID",
                                    Description = displayText + "ID",
                                    DataType = (int)DataTypeEnum.pkey,
                                    IsFormField = true,//2013-9-24 zhumin
                                };
                                db.Insert(tempField);
                                break;
                            default: break;
                        }

                        if (this.FormId != null)
                        {
                            SysForm form = db.FindById<SysForm>(this.FormId);
                            if (form != null)
                            {
                                form.FormName = formName;
                                form.EntityId = this.EntityId;
                                form.UpdateTime = DateTime.Now;
                                form.UpdateUserId = this.LoginUserID;
                                form.FormTitle = formTitle;
                                form.FormDescription = formDecription;

                                db.UpdatePartial(form, p => new { p.FormName,p.FormTitle,p.FormDescription, p.EntityId, p.UpdateTime, p.UpdateUserId });
                            }
                            else
                            {
                                throw new Exception("表单不存在");
                            }
                        }
                        else
                        {
                            SysForm form = new SysForm()
                            {
                                FormId = db.GetNextIdentity(),
                                FormName = formName,
                                FormTitle = formTitle,
                                FormDescription = formDecription,
                                EntityId = this.EntityId,
                                CreateTime = DateTime.Now,
                                CreateUserId = this.LoginUserID,
                                OwnerId = this.LoginUserID,
                                State = (int)FormState.New,
                            };
                            db.Insert(form);
                            this.FormId = form.FormId;
                        }
                    }
                    ts.Complete();
                }
                Response.Redirect(string.Format("FormDesigner_SecondStep.aspx?id={0}", this.FormId));
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }
}