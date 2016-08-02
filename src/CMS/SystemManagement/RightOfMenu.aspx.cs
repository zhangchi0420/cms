using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Drision.Framework.Web.Common;
using Drision.Framework.Entity;
using Drision.Framework.Repository.EF;
using Drision.Framework.OrgLibrary;
using Drision.Framework.Common;
using Drision.Framework.Manager;
using Drision.Framework.Enum;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class RightOfMenu : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BingDataToRoleId(Request["RoleId"]);
                //BingDataToDataList();
            }

        }

        /// <summary>
        /// 当前的子系统Id
        /// </summary>
        public long? CategoryId
        {
            get
            {
                return ViewState["CategoryId"] as Nullable<long>;
            }
            set
            {
                ViewState["CategoryId"] = value;
            }
        }

        #region 角色ID号

        /// <summary>
        /// 角色ID号
        /// </summary>
        public int RoleId
        {
            get { return (int)ViewState["RoleId"]; }
            set { ViewState["RoleId"] = value; }
        }


        /// <summary>
        /// 绑定角色ID号
        /// </summary>
        private void BingDataToRoleId(string roleId)
        {
            //如果传入ID号有误,则变成下拉菜单来处理
            bool nextSetp = true;
            using (BizDataContext Context = new BizDataContext())
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    this.RoleId = Convert.ToInt32(roleId);

                    var role = Context.FindById<T_Role>(RoleId);
                    if (null != role)
                    {
                        this.lblRoleName.Text = role.Role_Name;
                        this.lblRoleName.Visible = true;
                        this.ddlRoleList.Visible = false;
                        nextSetp = false;
                        BingDataToDataList();
                    }

                }

                if (nextSetp)
                {
                    var roleList = Context.Set<T_Role>().ToList();
                    this.ddlRoleList.DataSource = roleList;
                    this.ddlRoleList.DataValueField = "Role_ID";
                    this.ddlRoleList.DataTextField = "Role_Name";
                    this.ddlRoleList.DataBind();

                    this.RoleId = Convert.ToInt32(this.ddlRoleList.SelectedValue);

                    this.ddlRoleList.Visible = true;
                    this.lblRoleName.Visible = false;
                }
            }
        }

        /// <summary>
        /// 变换角色
        /// </summary>
        protected void ddlRoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RoleId = Convert.ToInt32(this.ddlRoleList.SelectedValue);
            BingDataToDataList();
        }

        #endregion

        #region 目录菜单

        private RoleFunctionManager _roleFuncManager;
        public RoleFunctionManager RoleFuncManager
        {
            get
            {
                if (_roleFuncManager == null)
                {
                    _roleFuncManager = new RoleFunctionManager(this.DataHelper);
                }
                return _roleFuncManager;
            }
        }

        private List<RightOfMenuEntity> _functionItemList;

        /// <summary>
        /// 目录菜单
        /// </summary>
        public List<RightOfMenuEntity> FunctionItemList
        {
            get
            {
                if (_functionItemList == null)
                {
                    var modelList = this.DataHelper.FetchAll<SysFunction>();
                    var roleFunctionList = this.RoleFuncManager.GetFunctions(this.RoleId);

                    _functionItemList = modelList.Select(x => new RightOfMenuEntity()
                    {
                        CategoryId = x.OwnerEntity == null ? null : x.OwnerEntity.OwnerModule.CategoryId,
                        PageId = x.PageId,
                        Function_ID = x.Function_ID,
                        Permission_Name = x.Permission_Name,
                        Permission_Type = x.Permission_Type,
                        Description = x.Function_Comment,
                        Category = x.Category,
                        CheckedFlage = roleFunctionList.Where(y => y.Function_ID == x.Function_ID).FirstOrDefault() == null ? false : true
                    }).OrderBy(p=>p.Category).ToList(); 
                }
                return _functionItemList;
            }
        }

        /// <summary>
        /// 绑定显示界面
        /// </summary>
        private void BingDataToDataList()
        {
            this.dataListModule.DataSource = FunctionItemList.Where(x =>
                {
                    bool result = x.Permission_Type == 0 || x.Permission_Type == null;
                    if (this.CategoryId != null)
                    {
                        result = result && (x.CategoryId == this.CategoryId || x.CategoryId == null);
                    }
                    return result;
                }).ToList();
            this.dataListModule.DataBind();
        }

        protected void dataListModule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RightOfMenuEntity menu = e.Item.DataItem as RightOfMenuEntity;
                var dlfun = e.Item.FindControl("dataListFunction") as DataList;
                
                dlfun.DataSource = FunctionItemList.Where(x => x.Permission_Type == menu.Function_ID).ToList();
                dlfun.DataBind();
            }
        }

        protected void dataListFunction_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RightOfMenuEntity menu = e.Item.DataItem as RightOfMenuEntity;
                var dlfun = e.Item.FindControl("repeaterThird") as Repeater;
                dlfun.DataSource = FunctionItemList.Where(x => x.Permission_Type == menu.Function_ID).ToList();
                dlfun.DataBind();
            }
        }

        #endregion

        #region 页面脚步控制信息

        protected void CheckBoxModule_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ckall = sender as CheckBox;
            var dlfun = ckall.NamingContainer.FindControl("dataListFunction") as DataList;
            foreach (DataListItem item in dlfun.Items)
            {
                (item.FindControl("CheckBoxItem") as CheckBox).Checked = ckall.Checked;
            }
        }

        protected void CheckBoxFunction_CheckedChanged(object sender, EventArgs e)
        {
            var mList = (sender as Control).NamingContainer.NamingContainer as DataList;
            this.SetCheckAll(mList);
        }

        protected void dataListFunction_PreRender(object sender, EventArgs e)
        {
            var fList = sender as DataList;
            this.SetCheckAll(fList);
        }

        private void SetCheckAll(DataList mList)
        {
            bool checkAll = true;
            foreach (DataListItem item in mList.Items)
            {
                checkAll &= (item.FindControl("CheckBoxItem") as CheckBox).Checked;
            }

            CheckBox ckall = mList.NamingContainer.FindControl("CheckBoxAll") as CheckBox;
            if (ckall.Checked != checkAll)
            {
                ckall.Checked = checkAll;
            }
        }

        #endregion

        /// <summary>
        /// 添加原则是先清除数据库中原有数据,然后添加新数据
        /// </summary>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            List<int> selectdList = new List<int>(1000);

            foreach (RepeaterItem mItem in this.dataListModule.Items)
            {
                DataList fList = mItem.FindControl("dataListFunction") as DataList;
                foreach (DataListItem fItem in fList.Items)
                {
                    var chk = fItem.FindControl("CheckBoxItem") as CheckBox;
                    if (chk.Checked)
                    {
                        var hiddenFun = fItem.FindControl("hiddenFun") as HiddenField;
                        selectdList.Add(Convert.ToInt32(hiddenFun.Value));
                    }
                    Repeater r = fItem.FindControl("repeaterThird") as Repeater;
                    foreach (RepeaterItem ri in r.Items)
                    {
                        var c = ri.FindControl("CheckBoxItemThird") as CheckBox;
                        if (c.Checked)
                        {
                            var h = ri.FindControl("hiddenFunThird") as HiddenField;
                            selectdList.Add(Convert.ToInt32(h.Value));
                        }
                    }
                }
            }

            if (selectdList.Count == 0)
            {
                this.AjaxAlert("没有选中任何项！");
                return;
            }

            //数据库操作过程
            this.RoleFuncManager.AddFunctionsToRole(this.RoleId, selectdList);

            //清除菜单的缓存，2011-10-8 zhu min
            Session["T_Function"] = null;
            Response.Redirect(this.Request.Url.PathAndQuery);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.Page.Request.UrlReferrer != null)
            {
                Response.Redirect(this.Page.Request.UrlReferrer.PathAndQuery);
            }
            else
            {
                Response.Redirect("../HR_Common/RoleQuery.aspx");
            }
        }

    }

    /// <summary>
    /// 用于界面显示控制实体
    /// </summary>
    [Serializable]
    public class RightOfMenuEntity
    {
        public long? PageId { get; set; }
        public long? CategoryId { get; set; }
        public long Function_ID { get; set; }
        public string Permission_Name { get; set; }
        public long? Permission_Type { get; set; }
        public bool CheckedFlage { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }        
    }

}
