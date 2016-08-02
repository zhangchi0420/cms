using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Entity;
using Drision.Framework.Common;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class RoleOfEntityManageNew : BasePage
    {
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
                this.AjaxAlert(ex);
            }
        }

        protected void Initialize()
        {
            this.scEntity.ListSettings.DataSource = this.EntityCache.SysEntity
                .OrderBy(p => p.EntityName)
                .Select(p => new
                {
                    p.EntityId,
                    p.EntityName,
                    p.OwnerModule.EntityCategory.CategoryName,
                }).ToList();
            this.scEntity.DataBind();

            this.scRole.ListSettings.DataSource = this.DataHelper.FetchAll<T_Role>();
            this.scRole.DataBind();

            var opSource = EnumHelper.GetEnumItems(typeof(EntityOperationEnum)).Where(p => p.Value != (int)EntityOperationEnum.None).ToList();
            this.scOperation.ListSettings.DataSource = opSource;
            this.scOperation.DataBind();
        }

        protected void Clear()
        {
            this.scEntity.SetValue(null);
            this.scEntity.SetText(null);
            this.scRole.SetValue(null);
            this.scRole.SetText(null);
            this.scOperation.SetValue(null);
            this.scOperation.SetText(null);
            this.ccPrivilege.SetValue(null);
        }

        protected void ccPrivilege_CustomCallBack(object sender, CallBackEventArgs e)
        {
            try
            {
                long? entityId = e.Parameter.ToLongNullable();
                SysEntity entity = this.EntityCache.FindById<SysEntity>(entityId);
                if (entity != null)
                {
                    this.ccPrivilege.Items.Add(new ComboItem() { Text = EnumHelper.GetDescription(EntityPrivilegeEnum.NoPermission), Value = ((int)EntityPrivilegeEnum.NoPermission).ToString() });
                    if (entity.PrivilegeMode == (int)PrivilegeModel.Persional)
                    {
                        this.ccPrivilege.Items.Add(new ComboItem() { Text = EnumHelper.GetDescription(EntityPrivilegeEnum.Personal), Value = ((int)EntityPrivilegeEnum.NoPermission).ToString() });
                        this.ccPrivilege.Items.Add(new ComboItem() { Text = EnumHelper.GetDescription(EntityPrivilegeEnum.Department), Value = ((int)EntityPrivilegeEnum.Department).ToString() });
                        this.ccPrivilege.Items.Add(new ComboItem() { Text = EnumHelper.GetDescription(EntityPrivilegeEnum.DepartmentAndSubSector), Value = ((int)EntityPrivilegeEnum.DepartmentAndSubSector).ToString() });
                    }
                    this.ccPrivilege.Items.Add(new ComboItem() { Text = EnumHelper.GetDescription(EntityPrivilegeEnum.AllRights), Value = ((int)EntityPrivilegeEnum.AllRights).ToString() });
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                long? entityId = this.scEntity.SelectedValues.FirstOrDefault().ToLongNullable();
                if (entityId == null)
                {
                    throw new Exception("请选择实体");
                }
                List<int> roleIdList = this.scRole.SelectedValues.Select(p => p.ToIntNullable()).Where(p => p != null).Select(p => p.Value).ToList();
                if (roleIdList.Count == 0)
                {
                    throw new Exception("请选择角色");
                }
                List<int> opList = this.scOperation.SelectedValues.Select(p => p.ToIntNullable()).Where(p => p != null).Select(p => p.Value).ToList();
                if (opList.Count == 0)
                {
                    throw new Exception("请选择操作");
                }
                int? privilege = this.ccPrivilege.SelectedValue.ToIntNullable();
                if (privilege == null)
                {
                    throw new Exception("请选择权限");
                }

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        var delList = db.Where<SysDataPrivilege>(p => p.EntityId == entityId.Value)
                             .Where(p => roleIdList.Contains(p.RoleId)).ToList();
                        foreach (var del in delList)
                        {
                            db.Delete(del);
                        }

                        var insertList = new List<SysDataPrivilege>();
                        foreach (var roleId in roleIdList)
                        {
                            foreach (var op in opList)
                            {
                                SysDataPrivilege data = new SysDataPrivilege()
                                {
                                    RoleId = roleId,
                                    EntityId = entityId.Value,
                                    OperationId = op,
                                    PrivelegeLevel = privilege.Value,
                                };
                                insertList.Add(data);
                            }
                        }
                        db.BatchInsert(insertList);
                    }
                    ts.Complete();
                }
                Clear();
                this.AjaxAlert("保存成功");
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}