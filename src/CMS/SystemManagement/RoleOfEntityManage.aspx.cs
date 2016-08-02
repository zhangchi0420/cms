using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using Drision.Framework.Enum;
using Drision.Framework.Repository.EF;
using Drision.Framework.OrgLibrary;
using Drision.Framework.Manager;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class RoleOfEntityManage : BasePage
    {
        protected void btnOff_Click(object sender, EventArgs e)
        {
            LoginUser = null;
            Session.Clear();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BingDataToRoleId(Request["RoleId"]);
                BingCategory();
                BingDataToUserControl();
            }
        }

        #region 变换角色
        
        public int RoleId
        {
            get
            {
                int roleId;
                if (ViewState["RoleId"] == null)
                {
                    throw new Exception("角色信息不能为空！");
                }
                roleId = Convert.ToInt32(ViewState["RoleId"]);
                return roleId;
            }
            set { ViewState["RoleId"] = value; }
        }

        private void BingDataToRoleId(string roleId)
        {
            //如果传入ID号有误,则变成下拉菜单来处理
            bool nextSetp = true;

            if (!string.IsNullOrEmpty(roleId))
            {
                this.RoleId = Convert.ToInt32(roleId);
                using (BizDataContext context = new BizDataContext())
                {
                    var query = context.FindById<T_Role>(this.RoleId);
                    if (query != null)
                    {
                        lblRoleName.Text += query.Role_Name;

                        this.ddlRoleList.Visible = false;
                        nextSetp = false;
                    }
                }
            }

            if (nextSetp)
            {
                using (BizDataContext Context = new BizDataContext())
                {
                    var roleList = Context.FetchAll<T_Role>();
                    this.ddlRoleList.DataSource = roleList;
                    this.ddlRoleList.DataValueField = "Role_ID";
                    this.ddlRoleList.DataTextField = "Role_Name";
                    this.ddlRoleList.DataBind();

                    this.RoleId = Convert.ToInt32(this.ddlRoleList.SelectedValue);

                    this.ddlRoleList.Visible = true;
                }
            }

        }
        /// <summary>
        /// 绑定子系统
        /// </summary>
        private void BingCategory()
        {
            //using (BizDataContext Context = new BizDataContext())
            {
                //var CategoryList = Context.FetchAll<SysEntityCategory>();
                var CategoryList = this.EntityCache.SysEntityCategory.ToList();
                this.ddlCategory.DataSource = CategoryList;
                this.ddlCategory.DataValueField = "CategoryId";
                this.ddlCategory.DataTextField = "CategoryName";
                this.ddlCategory.DataBind();
            }
        }

        /// <summary>
        /// 变换角色
        /// </summary>
        protected void ddlRoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RoleId = Convert.ToInt32(this.ddlRoleList.SelectedValue);
            BingDataToUserControl();
            this.RoleOfEntityControl1.Reflush();
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RoleId = Convert.ToInt32(this.ddlRoleList.SelectedValue);
            BingDataToUserControl();
            this.RoleOfEntityControl1.Reflush();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ddlRoleList_SelectedIndexChanged(null, null);
        }
        #endregion

        /// <summary>
        /// 绑定数据到用户控件上
        /// </summary>
        private void BingDataToUserControl()
        {
            if (string.IsNullOrEmpty(this.ddlCategory.SelectedValue))
            {
                return;
            }
            long CategoryId = Convert.ToInt64(this.ddlCategory.SelectedValue);
            using (BizDataContext Context = new BizDataContext())
            {
                //var moduleList = Context.Where<SysModule>(p => p.CategoryId == CategoryId).Select(p => p.ModuleId).ToList();
                var moduleList = this.EntityCache.SysModule.Where(p => p.CategoryId == CategoryId).Select(p => p.ModuleId).ToList();

                //获取实体信息,如果不存在权限等级则默认使用个人级别
                //var entityList = Context.Where<SysEntity>(p => p.ModuleId != null)
                var entityList = this.EntityCache.SysEntity.Where(p => p.ModuleId != null)
                    .Where(p => moduleList.Contains(p.ModuleId.Value))
                    .Select(x => new
                {
                    x.EntityId,
                    x.EntityName,
                    x.DisplayText,
                    x.Description,
                    PrivilegeMode = x.PrivilegeMode ?? (int)PrivilegeModel.Persional
                }).ToList();
                //获取该角色权限信息
                var dataList = Context.Where<SysDataPrivilege>(x => x.RoleId == this.RoleId);
                //组合数据
                var sourceList = entityList.Select(x => new ReptBindItem()
                {
                    EntityId = x.EntityId,
                    EntityName = x.EntityName,
                    DisplayText = x.DisplayText,
                    Description = x.Description,
                    PrivilegeMode = x.PrivilegeMode,
                    addRight = dataList.Where(y => y.EntityId == x.EntityId && y.OperationId == (int)EntityOperationEnum.Add).FirstOrDefault() == null ? (int)EntityPrivilegeEnum.NoPermission :
                               dataList.Where(y => y.EntityId == x.EntityId && y.OperationId == (int)EntityOperationEnum.Add).FirstOrDefault().PrivelegeLevel,
                    delRight = dataList.Where(y => y.EntityId == x.EntityId && y.OperationId == (int)EntityOperationEnum.Delete).FirstOrDefault() == null ? (int)EntityPrivilegeEnum.NoPermission :
                               dataList.Where(y => y.EntityId == x.EntityId && y.OperationId == (int)EntityOperationEnum.Delete).FirstOrDefault().PrivelegeLevel,
                    updtRight = dataList.Where(y => y.EntityId == x.EntityId && y.OperationId == (int)EntityOperationEnum.Update).FirstOrDefault() == null ? (int)EntityPrivilegeEnum.NoPermission :
                                dataList.Where(y => y.EntityId == x.EntityId && y.OperationId == (int)EntityOperationEnum.Update).FirstOrDefault().PrivelegeLevel,
                    queryRight = dataList.Where(y => y.EntityId == x.EntityId && y.OperationId == (int)EntityOperationEnum.Query).FirstOrDefault() == null ? (int)EntityPrivilegeEnum.NoPermission :
                                 dataList.Where(y => y.EntityId == x.EntityId && y.OperationId == (int)EntityOperationEnum.Query).FirstOrDefault().PrivelegeLevel
                }).ToList();

                #region 左连接不成功,传说EF里面不支持左连接,本人测试结果是简单语句可以左连接,复杂条件下不成功
                //var source = from n in Context.SysEntity
                //             join d in Context.SysDataPrivilege on n.EntityId equals d.EntityId into right
                //             select new
                //             {
                //                 n.EntityId,
                //                 n.EntityName,
                //                 n.DisplayText,
                //                 n.Description,
                //                 RoleId = 1,
                //                 addRight = right.Where(x => x.RoleId == 1 && x.OperationId == 1).FirstOrDefault() == null ? -1 : 1,
                //                 delRight = right.Where(x => x.RoleId == 1 && x.OperationId == 2).FirstOrDefault() == null ? -1 : 2,
                //                 updtRight = right.Where(x => x.RoleId == 1 && x.OperationId == 3).FirstOrDefault() == null ? -1 : 3,
                //                 queryRight = right.Where(x => x.RoleId == 1 && x.OperationId == 4).FirstOrDefault() == null ? -1 : 4,
                //                 //delRight = gb.FirstOrDefault().right == null ? -1 : gb.FirstOrDefault().right.Where(r => r.OperationId == 2).FirstOrDefault() == null ? -1 : 2,
                //                 //updtRight = gb.FirstOrDefault().right == null ? -1 : gb.FirstOrDefault().right.Where(r => r.OperationId == 3).FirstOrDefault() == null ? -1 : 3,
                //                 //queryRight = gb.FirstOrDefault().right == null ? -1 : gb.FirstOrDefault().right.Where(r => r.OperationId == 4).FirstOrDefault() == null ? -1 : 4,
                //                 //OperationId = gb.Key.OperationId, //增删改查方式
                //                 //PrivelegeLevel = gb.Key.PrivelegeLevel
                //             };
                //var sourceList = source.ToList();
                #endregion

                RoleOfEntityControl1.RightSource = sourceList;
            }
        }

        protected void btnSaveAA_Click(object sender, EventArgs e)
        {
            var dataList = RoleOfEntityControl1.GetReptBindItem();
            try
            {
                if (string.IsNullOrEmpty(this.ddlCategory.SelectedValue))
                {
                    return;
                }
                long CategoryId = Convert.ToInt64(this.ddlCategory.SelectedValue);
                string msg = string.Empty;
                //先清除该角色权限,然后添加当前设定的权限
                using (BizDataContext Context = new BizDataContext())
                {
                    //var moduleList = Context.Where<SysModule>(p => p.CategoryId == CategoryId).Select(p => p.ModuleId).ToList();
                    var moduleList = this.EntityCache.SysModule.Where(p => p.CategoryId == CategoryId).Select(p => p.ModuleId).ToList();

                    //var entityList = Context.Where<SysEntity>(p => p.ModuleId != null)
                    var entityList = this.EntityCache.SysEntity.Where(p => p.ModuleId != null)
                    .Where(p => moduleList.Contains(p.ModuleId.Value))
                    .Select(p => p.EntityId);

                    var delList = Context.Where<SysDataPrivilege>(x => x.RoleId == this.RoleId)
                        .Where(x=> entityList.Contains(x.EntityId)).ToList();
                    //delList.Clear();
                    foreach (var del in delList)
                    {
                        Context.Delete(del);
                    }

                    foreach (var add in dataList)
                    {
                        Context.Insert(new SysDataPrivilege()
                        {
                            EntityId = add.EntityId,
                            OperationId = (int)EntityOperationEnum.Add,
                            RoleId = this.RoleId,
                            PrivelegeLevel = add.addRight
                        });
                        Context.Insert(new SysDataPrivilege()
                        {
                            EntityId = add.EntityId,
                            OperationId = (int)EntityOperationEnum.Delete,
                            RoleId = this.RoleId,
                            PrivelegeLevel = add.delRight
                        });
                        Context.Insert(new SysDataPrivilege()
                        {
                            EntityId = add.EntityId,
                            OperationId = (int)EntityOperationEnum.Update,
                            RoleId = this.RoleId,
                            PrivelegeLevel = add.updtRight
                        });
                        Context.Insert(new SysDataPrivilege()
                        {
                            EntityId = add.EntityId,
                            OperationId = (int)EntityOperationEnum.Query,
                            RoleId = this.RoleId,
                            PrivelegeLevel = add.queryRight
                        });
                    }
                                       
                    
                    //1.无权限，2.个人，3.部门，4.部门及子部门，5.全部权限
                    //循环遍历所有实体表（即父实体）
                    foreach (var reptBindItem in dataList)
                    {
                        //获取当前父实体的一对多关系
                        var sysOneMoreRelations =
                        //    Context.Where<SysOneMoreRelation>(
                        this.EntityCache.SysOneMoreRelation.Where(
                                p => p.ParentEntityId == reptBindItem.EntityId && p.IsParentChild == true);
                        //循环所有一对多关系
                        foreach (var sysOneMoreRelation in sysOneMoreRelations)
                        {
                            //sysOneMoreRelation.ChildEntity = Context.FindById<SysEntity>(sysOneMoreRelation.ChildEntityId);

                            //获取父实体的子实体的所有权限
                            var delChild =
                                Context.Where<SysDataPrivilege>(
                                    x => x.RoleId == this.RoleId && sysOneMoreRelation.ChildEntityId == x.EntityId).ToList();
                            //删除子实体的所有权限
                            foreach (var sysDataPrivilege in delChild)
                            {
                                Context.Delete(sysDataPrivilege);
                            }
                            msg += string.Format("{0}为{1}的子实体，权限随{1}的权限而改变！\\n",
                                                 sysOneMoreRelation.ChildEntity.DisplayText,
                                                 reptBindItem.DisplayText);
                            //判断子实体的授权方式
                            //1.个人类，在此情况下子实体无需担心父实体的授权方式，子实体所包含的权限（即上面5种）>=父实体所包含权限
                            //2.组织类（只含有无权限和全部权限），在此情况下需判断父实体所选择的权限（即上面5种之一），如果父实体选择无权限，子实体即无权限，否则子实体为全部权限
                            if (sysOneMoreRelation.ChildEntity.PrivilegeMode == (int)PrivilegeModel.Persional)//子实体为个人类
                            {
                                //子实体的Add权限跟随父实体的Update权限，其余权限与父实体一一对应
                                Context.Insert(new SysDataPrivilege()
                                {
                                    EntityId = sysOneMoreRelation.ChildEntityId.Value,
                                    OperationId = (int)EntityOperationEnum.Add,
                                    RoleId = this.RoleId,
                                    PrivelegeLevel = reptBindItem.updtRight
                                });
                                Context.Insert(new SysDataPrivilege()
                                {
                                    EntityId = sysOneMoreRelation.ChildEntityId.Value,
                                    OperationId = (int)EntityOperationEnum.Delete,
                                    RoleId = this.RoleId,
                                    PrivelegeLevel = reptBindItem.delRight
                                });
                                Context.Insert(new SysDataPrivilege()
                                {
                                    EntityId = sysOneMoreRelation.ChildEntityId.Value,
                                    OperationId = (int)EntityOperationEnum.Update,
                                    RoleId = this.RoleId,
                                    PrivelegeLevel = reptBindItem.updtRight
                                });
                                Context.Insert(new SysDataPrivilege()
                                {
                                    EntityId = sysOneMoreRelation.ChildEntityId.Value,
                                    OperationId = (int)EntityOperationEnum.Query,
                                    RoleId = this.RoleId,
                                    PrivelegeLevel = reptBindItem.queryRight
                                });
                            }
                            else//子实体为组织类
                            {
                                if (reptBindItem.delRight == 1)//父实体对于删除选择无权限
                                {
                                    Context.Insert(new SysDataPrivilege()
                                                                     {
                                                                         EntityId =
                                                                             sysOneMoreRelation.ChildEntityId.Value,
                                                                         OperationId = (int) EntityOperationEnum.Delete,
                                                                         RoleId = this.RoleId,
                                                                         PrivelegeLevel = reptBindItem.delRight
                                                                     });
                                }
                                else
                                {
                                    Context.Insert(new SysDataPrivilege()
                                                                     {
                                                                         EntityId =
                                                                             sysOneMoreRelation.ChildEntityId.Value,
                                                                         OperationId = (int) EntityOperationEnum.Delete,
                                                                         RoleId = this.RoleId,
                                                                         PrivelegeLevel = 5
                                                                     });
                                }
                                if (reptBindItem.updtRight == 1)//父实体对于更新选择无权限，子实体Add权限跟随父实体update权限
                                {
                                    Context.Insert(new SysDataPrivilege()
                                                                     {
                                                                         EntityId =
                                                                             sysOneMoreRelation.ChildEntityId.Value,
                                                                         OperationId = (int) EntityOperationEnum.Update,
                                                                         RoleId = this.RoleId,
                                                                         PrivelegeLevel = reptBindItem.updtRight
                                                                     });
                                    Context.Insert(new SysDataPrivilege()
                                                                     {
                                                                         EntityId =
                                                                             sysOneMoreRelation.ChildEntityId.Value,
                                                                         OperationId = (int) EntityOperationEnum.Add,
                                                                         RoleId = this.RoleId,
                                                                         PrivelegeLevel = reptBindItem.updtRight
                                                                     });
                                }
                                else
                                {
                                    Context.Insert(new SysDataPrivilege()
                                                                     {
                                                                         EntityId =
                                                                             sysOneMoreRelation.ChildEntityId.Value,
                                                                         OperationId = (int) EntityOperationEnum.Update,
                                                                         RoleId = this.RoleId,
                                                                         PrivelegeLevel = 5
                                                                     });
                                    Context.Insert(new SysDataPrivilege()
                                                                     {
                                                                         EntityId =
                                                                             sysOneMoreRelation.ChildEntityId.Value,
                                                                         OperationId = (int) EntityOperationEnum.Add,
                                                                         RoleId = this.RoleId,
                                                                         PrivelegeLevel = 5
                                                                     });
                                }
                                if (reptBindItem.queryRight == 1)//父实体对于查询选择无权限
                                {
                                    Context.Insert(new SysDataPrivilege()
                                                                     {
                                                                         EntityId =
                                                                             sysOneMoreRelation.ChildEntityId.Value,
                                                                         OperationId = (int) EntityOperationEnum.Query,
                                                                         RoleId = this.RoleId,
                                                                         PrivelegeLevel = reptBindItem.queryRight
                                                                     });
                                }
                                else
                                {
                                    Context.Insert(new SysDataPrivilege()
                                                                     {
                                                                         EntityId =
                                                                             sysOneMoreRelation.ChildEntityId.Value,
                                                                         OperationId = (int) EntityOperationEnum.Query,
                                                                         RoleId = this.RoleId,
                                                                         PrivelegeLevel = 5
                                                                     });
                                }
                            }
                        }
                    }                    
                }

                IOperationManager dom = new DefaultOperationManager(this.DataHelper,this.LoginUser);
                int count = dom.ValidateSharedPrivilege();
                if (count > 0)
                {
                    msg = string.Format("{0}，修改导致{1}条权限共享规则被移除",msg,count);
                }

                //清除菜单的缓存，2011-10-8 zhu min
                Session["T_Function"] = null;                
                this.AjaxAlert("保存成功！\\n"+msg, "window.location.reload()");                
                
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}