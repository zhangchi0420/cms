using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using System.Web.Script.Serialization;
using Drision.Framework.OrgLibrary;


namespace Drision.Framework.Web.SystemManagement
{
    public partial class FunctionManagement : BasePage
    {
        #region 属性

        //protected BizDataContext db;

        public string SortField
        {
            get { return VS<string>("SortField"); }
            set { VS<string>("SortField", value); }
        }

        //public SortDirection SortDirection
        //{
        //    get { return VS<SortDirection>("SortDirection"); }
        //    set { VS<SortDirection>("SortDirection", value); }
        //}

        public string SortFilter
        {
            get { return VS<string>("SortFilter"); }
            set { VS<string>("SortFilter", value); }
        }

        public int PageIndex
        {
            get { return VS<int>("PageIndex"); }
            set { VS<int>("PageIndex", value); }
        }

        public int PageSize
        {
            get { return VS<int>("PageSize"); }
            set { VS<int>("PageSize", value); }
        }
        
        #endregion
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {
                Try(() =>
                {
                    Initialize();

                    //这个HiddenControl作用为1、保存当前编辑的FunctionId，2、刷新Grid
                    this.hcId.Text = this.ClientScript.GetPostBackEventReference(this.hcId, "refresh");
                });
            }
            else if (!IsCallback)
            {
                if (Request["__EVENTARGUMENT"] == "refresh")
                {
                    BindGrid();
                    BindDropdown();
                }
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "button", "ButtonStyle();", true);
        }

        #region 保存和新增

        /// <summary>
        /// 保存和新增的回调，前台收集表单数据后作为参数传回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbcSave_CallBack(object sender, CallBackEventArgs e)
        {
            //e.Context是一个“属性名：值”形式的Dictionary
            long? functionId = Convert.ToString(e.Context["FunctionId"]).ToLongNullable();
            AddOrSave(functionId, e);
            e.Result = "1";
        }

        /// <summary>
        /// 从e.Context中获得属性值，并赋值给实体对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="data"></param>
        private void CopyValueFromCallBackData(SysFunction item, Dictionary<string, object> data)
        {
            if (item != null)
            {
                item.Permission_Name = Convert.ToString(data["PermissionName"]);

                if (!string.IsNullOrEmpty(Convert.ToString(data["SecondPermissionType"])))
                {
                    item.Permission_Type = Convert.ToString(data["SecondPermissionType"]).ToLongNullable();
                }
                else
                {
                    item.Permission_Type = Convert.ToString(data["PermissionType"]).ToLongNullable();
                }

                item.Function_Comment = Convert.ToString(data["FunctionComment"]);
                item.URL = Convert.ToString(data["URL"]);
                item.Is_Menu = Convert.ToBoolean(data["IsMenu"]).ToInt();
                item.Entity_ID = Convert.ToString(data["EntityId"]).ToLongNullable();
                item.PageId = Convert.ToString(data["PageId"]).ToLongNullable();
                item.OrderIndex = Convert.ToString(data["OrderIndex"]).ToIntNullable();
                item.Category = Convert.ToString(data["Category"]);
                item.ClassName = Convert.ToString(data["ClassName"]);
            }
        }

        /// <summary>
        /// 保存或新增到数据库
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="e"></param>
        private void AddOrSave(long? functionId, CallBackEventArgs e)
        {
            SysFunction item = new SysFunction();

            if (functionId == null) //新增
            {
                item.Function_ID = this.DataHelper.GetNextIdentity_Int();
                CopyValueFromCallBackData(item, e.Context);
                this.DataHelper.Insert(item);
            }
            else //保存
            {
                item = this.DataHelper.FindById<SysFunction>(functionId);
                if (item != null)
                {
                    CopyValueFromCallBackData(item, e.Context);
                }
                this.DataHelper.Update(item);
            }
        }

        #endregion

        #region 初始化和查询

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            this.PageIndex = this.grid.PagerSettings.PageIndex;
            this.PageSize = this.grid.PagerSettings.PageSize;

            //给个默认的排序吧
            this.SortField = "OrderIndex";
            this.SortFilter = "asc";
            //this.grid.SortDirections[this.SortField] = this.SortDirection;

            BindGrid();
            BindDropdown();
        }

        /// <summary>
        /// 绑定分组和实体
        /// </summary>
        /// <param name="db"></param>
        private void BindEntiry()
        {
            //var entityList = this.DataHelper.FetchAll<SysEntity>().OrderBy(p => p.EntityName).ToList();
            var entityList = this.EntityCache.SysEntity.OrderBy(p => p.EntityName).ToList();
            this.cbEntity.DataSource = entityList;
            this.cbEntity.DataTextField = "EntityName";
            this.cbEntity.DataValueField = "EntityId";
            this.cbEntity.DataBind();
        }

        /// <summary>
        /// 查询父功能
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        private string GetParentFunction(long? functionId)
        {
            string result = null;
            if (functionId != null)
            {
                SysFunction item = this.DataHelper.FindById<SysFunction>(functionId);
                if (item != null)
                {
                    return item.Permission_Name;
                }
            }
            return result;
        }


        /// <summary>
        /// 顺便绑定下拉框……
        /// </summary>
        private void BindDropdown()
        {
            //父功能下拉框，即父功能为null的，模块
            this.cbPermissionType.Items.Clear();
            //var parentItems = db.Where<SysFunction>(p => p.Is_Menu==0);
            var parentItems = this.DataHelper.Where<SysFunction>(p => p.Permission_Type == 0 || p.Permission_Type == null);


            foreach (var p in parentItems)
            {
                this.cbPermissionType.Items.Add(new ComboItem()
                {
                    Text = p.Permission_Name,
                    Value = p.Function_ID.ToString(),
                });
            }

            //名称
            this.cbNameQuery.Items.Clear();
            foreach (var p in this.DataHelper.FetchAll<SysFunction>())
            {
                this.cbNameQuery.Items.Add(new ComboItem()
                {
                    Text = p.Permission_Name,
                    Value = p.Function_ID.ToString(),
                });
            }

            //2011-11-14 by zhongyan.tao for the addtional query condition
            ddlParent.DataSource = parentItems.ToList<SysFunction>().OrderBy(p => p.Category)
            .Select(p => new
            {
                Permission_Name = string.Format("{0}--{1}", p.Permission_Name, p.Category),
                p.Function_ID,
            }).ToList();
            ddlParent.DataTextField = "Permission_Name";
            ddlParent.DataValueField = "Function_ID";
            ddlParent.DataBind();
            ddlParent.Items.Insert(0, new ComboItem()
            {
                Text = "-无父功能-",
                Value = string.Empty,
            });
            //end


            BindEntiry();
        }

        private string GetEntityName(object entityId)
        {
            string res = string.Empty;
            //using (BizDataContext contex = new BizDataContext())
            {
                //SysEntity entity = this.DataHelper.FindById<SysEntity>(entityId);
                SysEntity entity = this.EntityCache.FindById<SysEntity>(entityId);
                if (entity != null)
                {
                    res = entity.EntityName;
                }
            }
            return res;
        }

        private string GetPageName(object pageId)
        {
            string res = string.Empty;
            //using (BizDataContext contex = new BizDataContext())
            {
                //SysPage page = this.DataHelper.FindById<SysPage>(pageId);
                SysPage page = this.EntityCache.FindById<SysPage>(pageId);
                if (page != null)
                {
                    res = page.PageName;
                }
            }
            return res;
        }

        private string GetPageUrl(SysFunction p)
        {
            if (p.Permission_Type == null || p.Permission_Type == 0)
            {
                return null;
            }
            return (this.Master as Site).GetPageUrl(p);
        }

        /// <summary>
        /// 绑定Grid
        /// </summary>
        private void BindGrid()
        {
            //查询条件
            string permissionName = this.cbNameQuery.SelectedText.Trim();

            //数据源
            var source = this.DataHelper.Where<SysFunction>(p => p.Permission_Name.Contains(permissionName)).OrderBy(p => p.Permission_Type).ToList();

            if (!string.IsNullOrEmpty(ddlParent.SelectedValue))
            {
                int? parentId = ddlParent.SelectedValue.ToIntNullable();
                source = source.Where(p => p.Permission_Type == parentId).ToList();
            }
            else if (ddlParent.SelectedValue == string.Empty)
            {
                source = source.Where(p => p.Permission_Type == null || p.Permission_Type == 0).ToList();
            }

            var result = source.Select(p => new
            {
                PermissionName = p.Permission_Name,
                p.URL,
                IsMenu = p.Is_Menu.ToBool() ? "是" : "否",
                PermissionType = GetParentFunction(p.Permission_Type),
                FunctionId = p.Function_ID,
                EntityName = GetEntityName(p.Entity_ID),
                PageName = GetPageName(p.PageId),
                p.OrderIndex,
                PageUrl = GetPageUrl(p),
                Description = p.Function_Comment,
                p.Category,
            });


            //排序
            if (this.SortFilter.Contains("asc"))
            {
                result = result.OrderBy(p => DataBinder.GetPropertyValue(p, this.SortField)).ToList();
            }
            else
            {
                result = result.OrderByDescending(p => DataBinder.GetPropertyValue(p, this.SortField)).ToList();
            }

            //绑定
            if (result.Count() > this.PageIndex * this.PageSize)
            {
                this.grid.DataSource = result.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
            }
            else
            {
                this.PageIndex = 0;
                this.grid.PagerSettings.PageIndex = 0;
                this.grid.DataSource = result.Take(this.PageSize).ToList();
            }
            this.grid.PagerSettings.DataCount = result.Count();
            this.grid.DataBind();
        }

        /// <summary>
        /// 表头点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_HeaderClick(object sender, GridPostBackEventArgs e)
        {
            this.SortField = e.FieldName;
            //this.SortDirection =e.Direction;
            this.SortFilter = e.SortFilter;
            BindGrid(); //重新绑定
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_PageChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex = e.PageIndex;
            this.PageSize = e.PageSize;
            BindGrid();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindGrid(); //重新绑定
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param 
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> msgList = new List<string>();
            foreach (string id in this.grid.SelectedValues)
            {
                long? functionId = id.ToLongNullable();
                SysFunction item = this.DataHelper.FindById<SysFunction>(functionId);
                var childs = this.DataHelper.Where<SysFunction>(p => p.Permission_Type == functionId);
                if (childs.Count > 0)
                {
                    msgList.Add(item.Permission_Name);
                    continue;
                }
                if (item != null)
                {
                    this.DataHelper.Delete(item);
                }
            }

            BindGrid();
            BindDropdown();
            string msg = string.Empty;
            msgList.ForEach(p =>
            {
                if (p == msgList.Last())
                {
                    msg += "[" + p + "]";
                }
                else
                {
                    msg += "[" + p + "]" + ",";
                }
            });
            msg += "中存在子菜单，请先删除子菜单后再试！";
            if (msgList.Count > 0)
            {
                AjaxAlert(msg);
            }
        }

        #endregion

        #region 绑定对话框

        /// <summary>
        /// 根据ID获得实体信息，返回JSON字符串
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        private string BindEditor(long? functionId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string result = js.Serialize(new
            {
                FunctionId = string.Empty,
                PermissionName = string.Empty,
                FunctionComment = string.Empty,
                URL = string.Empty,
                IsMenu = false,
                PermissionType = string.Empty,
                SecondPermissionType = string.Empty,
                EntityId = string.Empty,
                PageId = string.Empty,
                OrderIndex = string.Empty,
                Category = string.Empty,
                ClassName = string.Empty,
            });


            if (functionId != null)
            {
                SysFunction item = this.DataHelper.FindById<SysFunction>(functionId);
                if (item != null)
                {
                    SysFunction parent = this.DataHelper.FindById<SysFunction>(item.Permission_Type);
                    long? pType = parent != null && parent.Permission_Type != null ? parent.Permission_Type : item.Permission_Type;
                    long? spType = parent != null && parent.Permission_Type != null ? item.Permission_Type : null;

                    result = js.Serialize(new
                    {
                        FunctionId = functionId.ToString(),
                        PermissionName = item.Permission_Name,
                        FunctionComment = item.Function_Comment,
                        item.URL,
                        IsMenu = item.Is_Menu.ToBool(),
                        PermissionType = pType,
                        SecondPermissionType = spType, 
                        EntityId = item.Entity_ID,
                        PageId = item.PageId,
                        item.OrderIndex,
                        item.Category,
                        item.ClassName,
                    });
                }
            }

            return result;
        }

        

        /// <summary>
        /// 打开对话框的回调，获得信息后传给前台绑定、弹出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbcOpen_CallBack(object sender, CallBackEventArgs e)
        {
            e.Result = BindEditor(e.Parameter.ToLongNullable());
        }

        /// <summary>
        /// 根据实体重新绑定页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbPage_CallBack(object sender, CallBackEventArgs e)
        {
            long entityId = e.Parameter.ToLong();
            //int controlType = (int)ControlType.SysPage;
            //var pageList = this.DataHelper.Where<SysPageControl>(p => p.EntityId == entityId && p.ControlType == controlType)
            var pageList = this.EntityCache.SysPage.Where(p=>p.EntityId == entityId)
            .Select(p => new
                {
                    p.ControlId,
                    p.PageName //= this.DataHelper.FindById<SysPage>(p.ControlId).PageName,
                }).ToList();

            cbPage.DataSource = pageList;
            cbPage.DataTextField = "PageName";
            cbPage.DataValueField = "ControlId";
            cbPage.DataBind();
        }

        /// <summary>
        /// 根据一级菜单查询二级菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbSecondPermissionType_CallBack(object sender, CallBackEventArgs e)
        {
            long parentId = e.Parameter.ToLong();
            var source = this.DataHelper.Where<SysFunction>(p => p.Permission_Type == parentId);
            cbSecondPermissionType.DataSource = source;
            cbSecondPermissionType.DataTextField = "Permission_Name";
            cbSecondPermissionType.DataValueField = "Function_ID";
            cbSecondPermissionType.DataBind();
        }

        #endregion
    }
}