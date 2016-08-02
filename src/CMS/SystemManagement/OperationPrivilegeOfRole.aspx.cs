using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using System.Configuration;
using Drision.Framework.Entity;
using Drision.Framework.Manager;


namespace Drision.Framework.Web.SystemManagement
{
    public partial class OperationPrivilegeOfRole : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRole();
                
                LoadPage();
                
            }
        }
        /// <summary>
        /// 加载角色
        /// </summary>
        private void LoadRole()
        {
            cbRole.DataSource = this.OperationPrivilegeHelper.GetRole();
            cbRole.DataBind();
        }
        
        /// <summary>
        /// 加载页面
        /// </summary>
        private void LoadPage()
        {
            dataListOperation.DataSource = this.OperationPrivilegeHelper.GetPage();
            dataListOperation.DataBind();
        }
        /// <summary>
        /// 绑定所有页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dataListOperation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var datalist = e.Item.FindControl("dataList") as DataList;
                SysPage pr = e.Item.DataItem as SysPage;
                //获取页面下所有的操作，不包括行操作
                var operation = this.OperationPrivilegeHelper.GetOperation(pr.PageName);
                datalist.DataSource = operation;
                datalist.DataBind();
                //判断页面下是否有受控制的操作，包括行操作，没有则此页面不显示
                if (!this.OperationPrivilegeHelper.HasOperation(pr.PageName))
                {
                    e.Item.Visible = false;
                }
                //获取页面下所有的视图
                var datalistview = e.Item.FindControl("dataListView") as Repeater;
                datalistview.DataSource = this.OperationPrivilegeHelper.GetView(pr.ControlId);
                datalistview.DataBind();
            }
        }
        /// <summary>
        /// 绑定视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dataListView_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField pageName = e.Item.NamingContainer.NamingContainer.FindControl("hiddenPageName") as HiddenField;
                var datalist = e.Item.FindControl("dataListViewOp") as DataList;
                SysViewControl pr = e.Item.DataItem as SysViewControl;
                HiddenField viewName = e.Item.FindControl("hiddenViewName") as HiddenField;
                if (pr != null)
                {
                    viewName.Value = this.OperationPrivilegeHelper.GetViewName(pr.ControlId);
                }
                //获取视图下所有的行操作
                var operation = this.OperationPrivilegeHelper.GetOperation(pageName.Value, viewName.Value);
                datalist.DataSource = operation;
                datalist.DataBind();
                //判断视图下是否存在受控制的行操作，不存在则此视图不显示
                if (!operation.Any())
                {
                    e.Item.Visible = false;
                }
                
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckBoxOperation_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ckall = sender as CheckBox;
            var dl = ckall.NamingContainer.FindControl("dataList") as DataList;
            foreach (DataListItem item in dl.Items)
            {
                (item.FindControl("CheckBoxItem") as CheckBox).Checked = ckall.Checked;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckBoxView_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ckall = sender as CheckBox;
            var dl = ckall.NamingContainer.FindControl("dataListViewOp") as DataList;
            foreach (DataListItem item in dl.Items)
            {
                (item.FindControl("CheckBoxItem") as CheckBox).Checked = ckall.Checked;
            }
        }
        
        protected void dataList_PreRender(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 页面下操作绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SysOperationPrivilege pr = e.Item.DataItem as SysOperationPrivilege;
                (e.Item.FindControl("CheckBoxItem") as CheckBox).Checked = this.OperationPrivilegeHelper.OperationPrivilege(pr.PageName, Convert.ToInt32(cbRole.SelectedValue), pr.OperationName);//pr.CheckedFlage;
            }
        }
        /// <summary>
        /// 视图下行操作绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dataListViewOp_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SysOperationPrivilege pr = e.Item.DataItem as SysOperationPrivilege;
                (e.Item.FindControl("CheckBoxItem") as CheckBox).Checked = this.OperationPrivilegeHelper.OperationPrivilege(pr.PageName, Convert.ToInt32(cbRole.SelectedValue), pr.OperationName, pr.ViewName);//pr.CheckedFlage;
            }
        }
        
        /// <summary>
        /// 保存方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int? roleId = Convert.ToInt32(cbRole.SelectedValue);

            var deleteList = this.OperationPrivilegeHelper.GetDeleteList(roleId);

            List<T_Role_OperationPrivilege> addList = new List<T_Role_OperationPrivilege>();

            foreach (RepeaterItem mItem in this.dataListOperation.Items)
            {
                DataList fList = mItem.FindControl("dataList") as DataList;
                foreach (DataListItem fItem in fList.Items)
                {
                    var chk = fItem.FindControl("CheckBoxItem") as CheckBox;
                    if (chk.Checked)
                    {
                        T_Role_OperationPrivilege r = new T_Role_OperationPrivilege();
                        var hiddenOp = fItem.FindControl("hiddenOp") as HiddenField;
                        var hiddenPage = fItem.FindControl("hiddenPage") as HiddenField;
                        r.Id = this.OperationPrivilegeHelper.GetGetNextIdentity();
                        r.OperationName = hiddenOp.Value;
                        r.PageName = hiddenPage.Value;
                        r.RoleId = Convert.ToInt32(cbRole.SelectedValue);
                        addList.Add(r);
                    }
                }
                Repeater rpView = mItem.FindControl("dataListView") as Repeater;
                foreach (RepeaterItem r in rpView.Items)
                {
                    DataList vList = r.FindControl("dataListViewOp") as DataList;
                    HiddenField viewName = r.FindControl("hiddenViewName") as HiddenField;
                    foreach (DataListItem vItem in vList.Items)
                    {
                        var vchk = vItem.FindControl("CheckBoxItem") as CheckBox;
                        if (vchk.Checked)
                        {
                            T_Role_OperationPrivilege vr = new T_Role_OperationPrivilege();
                            var hiddenOp = vItem.FindControl("hiddenViewOp") as HiddenField;
                            var hiddenPage = vItem.FindControl("hiddenViewPage") as HiddenField;
                            vr.Id = this.OperationPrivilegeHelper.GetGetNextIdentity();
                            vr.OperationName = hiddenOp.Value;
                            vr.PageName = hiddenPage.Value;
                            vr.RoleId = Convert.ToInt32(cbRole.SelectedValue);
                            vr.ViewName = viewName.Value;
                            addList.Add(vr);
                        }
                    }
                }
            }
            this.OperationPrivilegeHelper.Insert(deleteList, addList);
            this.AjaxAlert("保存成功！");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.Page.Request.UrlReferrer != null)
            {
                Response.Redirect(this.Page.Request.UrlReferrer.PathAndQuery);
            }
            else
            {
                Response.Redirect("../Home/HomePage.aspx");

            }
        }

        protected void ddlRoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPage();
        }
    }
}