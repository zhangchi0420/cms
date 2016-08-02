using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Drision.Framework.Web.Common;
using Drision.Framework.Entity;
using System.Linq.Expressions;
using Drision.Framework.Repository.EF;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class RoleInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BingRoleToDataGridview();
            }
        }

        private void BingRoleToDataGridview()
        {
            Expression<Func<T_Role, bool>> whereExp = null;
            if (!string.IsNullOrEmpty(txtSearchName.Text.Trim()))
            {
                whereExp = n => n.Role_Name.Contains(txtSearchName.Text.Trim());
            }

            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                if (whereExp == null)
                {
                    int totalCount = context.T_Role.Count();
                    Pager.Data_AspNetPager(AspNetPager1, totalCount, AspNetPager1.PageSize);

                    this.gvShow.DataSource = context.T_Role.OrderBy(x => x.Role_ID).Skip(AspNetPager1.PageSize * (AspNetPager1.CurrentPageIndex - 1))
                        .Take(AspNetPager1.PageSize).ToList();
                    this.gvShow.DataBind();
                }
                else
                {
                    int totalCount = context.T_Role.Where(whereExp).Count();
                    Pager.Data_AspNetPager(AspNetPager1, totalCount, AspNetPager1.PageSize);

                    this.gvShow.DataSource = context.T_Role.Where(whereExp).OrderBy(x => x.Role_ID).Skip(AspNetPager1.PageSize * (AspNetPager1.CurrentPageIndex - 1))
                        .Take(AspNetPager1.PageSize).ToList();
                    this.gvShow.DataBind();
                }

            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            BingRoleToDataGridview();
        }

        protected void gvShow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                T_Role role = e.Row.DataItem as T_Role;
                
                Label lblState = e.Row.FindControl("lblState") as Label;
                LinkButton lbtnState = e.Row.FindControl("lbtnState") as LinkButton;
                LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                LinkButton lbtnDel = e.Row.FindControl("lbtnDel") as LinkButton;
                LinkButton lbtnSet = e.Row.FindControl("lbtnSet") as LinkButton;

                lbtnState.CommandArgument = Convert.ToString(role.Role_ID);
                lbtnEdit.CommandArgument = Convert.ToString(role.Role_ID);
                lbtnDel.CommandArgument = Convert.ToString(role.Role_ID);
                lbtnSet.CommandArgument = Convert.ToString(role.Role_ID);

                if (role.Role_Status == 1)
                {
                    lblState.Text = "启用";

                    lbtnState.Text = "停用";
                    lbtnState.OnClientClick = "if (!confirm('是否停用角色？')) {return false;}";
                }
                else if (role.Role_Status == 0)
                {
                    lblState.Text = "停用";

                    lbtnState.Text = "启用";
                    lbtnState.OnClientClick = "if (!confirm('是否启用角色？')) {return false;}";
                }

                if (role.Role_ID == 1)
                {
                    lbtnState.Enabled = false;
                    lbtnEdit.Enabled = false;
                    lbtnDel.Enabled = false;
                    lbtnSet.Enabled = false;
                }

            }
        }

        #region AJAX操作方法

        protected void gvShow_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            T_Role role = null;
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                int roleId = Convert.ToInt32(e.CommandArgument);
                role = context.T_Role.Where(x => x.Role_ID == roleId).FirstOrDefault();

                if (e.CommandName == "state")
                {
                    ResetState(role, context);
                }
                else if (e.CommandName == "up")
                {
                    UpdateUser(role, context);
                }
                else if (e.CommandName == "del")
                {
                    DeleteUser(role, context);
                }
                else if (e.CommandName == "set")
                {
                    SetRight(role, context);
                }

            }
        }

        private void ResetState(T_Role role, DrisionDbContext context)
        {
            if (role.Role_Status == 1)
            {
                
                if (role.T_Users.Count > 0 ||
                    RoleFunctionManager.GetInstance().GetFunctions(role.Role_ID).Count > 0)
                {
                    this.AjaxAlert("此角色已使用，不能停用！");
                    return;
                }

                role.Role_Status = 0;
                context.SaveChanges();

                BingRoleToDataGridview();
                this.AjaxAlert("停用角色成功！");
                return;
            }
            else if (role.Role_Status == 0)
            {
                role.Role_Status = 1;
                context.SaveChanges();

                BingRoleToDataGridview();
                this.AjaxAlert("启用角色成功！");
                return;
            }
        }

        private void UpdateUser(T_Role role, DrisionDbContext context)
        {
            txtName.Text = role.Role_Name;
            rbtnState.SelectedValue = role.Role_Status.ToString();
            txtRemark.Text = role.Role_Comment;

            lblEdit.Text = role.Role_ID.ToString();
            btnAdd.Visible = false;
            btnEdit.Visible = true;
            Panel1.Visible = true;
        }

        private void DeleteUser(T_Role role, DrisionDbContext context)
        {
            if (role.T_Users.Count > 0 ||
                    RoleFunctionManager.GetInstance().GetFunctions(role.Role_ID).Count > 0)
            {
                this.AjaxAlert("此角色已使用，不能删除！");
                return;
            }

            context.T_Role.Remove(role);
            context.SaveChanges();

            BingRoleToDataGridview();
            if (lblEdit.Text.Trim() == role.Role_ID.ToString())
            {
                ClearControl();
            }
            this.AjaxAlert("删除角色成功！");
        }

        private void SetRight(T_Role role, DrisionDbContext context)
        {
            context.Dispose();
            Response.Redirect("RightOfMenu.aspx?RoleId=" + role.Role_ID);
            return;
        }

        private void ClearControl()
        {
            txtName.Text = "";
            rbtnState.SelectedValue = "1";
            txtRemark.Text = "";
            lblEdit.Text = "";
            btnAdd.Visible = true;
            btnEdit.Visible = false;
            Panel1.Visible = false;
        }

        #endregion

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();

            if (string.IsNullOrEmpty(lblEdit.Text.Trim()))
            {
                AddUser(name);
            }
            else
            {
                int id = Convert.ToInt32(lblEdit.Text.Trim());
                EditUser(name, id);
            }
        }

        private void AddUser(string name)
        {
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                if (context.T_Role.Where(x => x.Role_Name == name).Count() != 0)
                {
                    this.AjaxAlert("角色名称不能重复！");
                    return;
                }

                int roleId;
                var query = context.T_Role.OrderByDescending(x => x.Role_ID).FirstOrDefault();
                if (query == null)
                    roleId = 1;
                else
                    roleId = query.Role_ID + 1;

                context.T_Role.Add(new T_Role()
                {
                    Role_ID = roleId,
                    Role_Name = txtName.Text.Trim(),
                    Role_Status = Convert.ToInt32(rbtnState.SelectedValue),
                    Role_Comment = txtRemark.Text.Trim()
                });
                context.SaveChanges();
            }

            BingRoleToDataGridview();
            ClearControl();
            this.AjaxAlert("添加角色成功！");
        }

        private void EditUser(string name, int id)
        {
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                if (context.T_Role.Where(x => x.Role_ID != id && x.Role_Name == name).Count() != 0)
                {
                    this.AjaxAlert("角色名称不能重复！");
                    return;
                }

                var query = context.T_Role.Where(x => x.Role_ID == id).FirstOrDefault();
                query.Role_Name = txtName.Text.Trim();
                query.Role_Status = Convert.ToInt32(rbtnState.SelectedValue);
                query.Role_Comment = txtRemark.Text.Trim();
                context.SaveChanges();
            }

            BingRoleToDataGridview();
            ClearControl();
            this.AjaxAlert("编辑角色成功！");
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        protected void btnAddPanel_Click(object sender, EventArgs e)
        {
            ClearControl();
            Panel1.Visible = true;
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BingRoleToDataGridview();
        }
    }
}
