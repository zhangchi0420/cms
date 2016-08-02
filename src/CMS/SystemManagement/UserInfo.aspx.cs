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
using System.Data.Entity;
using Drision.Framework.Repository.EF;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class UserInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserShow();
                DeptShow();
            }
        }

        private void DeptShow()
        {
            using (DrisionDbContext context = new DrisionDbContext())
            {
                var departmentlist = context.T_Department.Select(x => new { x.Department_ID, x.Department_Name }).ToList();
                this.ddlDept.Items.Clear();
                this.ddlDept.Items.Add(new ListItem("请选择", "-1"));
                this.ddlDept.DataSource = departmentlist;
                this.ddlDept.DataTextField = "Department_Name";
                this.ddlDept.DataValueField = "Department_ID";
                this.ddlDept.DataBind();
            }
        }

        private void UserShow()
        {
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                IEnumerable<T_User> userQuery;
                if (string.IsNullOrEmpty(txtSearchName.Text.Trim()))
                {
                    userQuery = context.T_User.OrderBy(x => x.User_ID);
                }
                else
                {
                    userQuery = context.T_User.Where(x => x.User_Name.Contains(txtSearchName.Text.Trim())).OrderBy(x => x.User_ID);
                }
                int totalCount = userQuery.Count();
                Pager.Data_AspNetPager(AspNetPager1, totalCount, AspNetPager1.PageSize);
                this.gvShow.DataSource = userQuery.Skip(AspNetPager1.PageSize * (AspNetPager1.CurrentPageIndex - 1))
                    .Take(AspNetPager1.PageSize).ToList();
                this.gvShow.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            UserShow();
        }

        protected void gvShow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                T_User user = e.Row.DataItem as T_User;

                Label lblUserRole = e.Row.FindControl("lblUserRole") as Label;
                Label lblState = e.Row.FindControl("lblState") as Label;
                Label lblDept = e.Row.FindControl("lblDept") as Label;
                LinkButton lbtnReset = e.Row.FindControl("lbtnReset") as LinkButton;
                LinkButton lbtnState = e.Row.FindControl("lbtnState") as LinkButton;
                LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                LinkButton lbtnDel = e.Row.FindControl("lbtnDel") as LinkButton;
                LinkButton lbtnSet = e.Row.FindControl("lbtnSet") as LinkButton;

                using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
                {
                    //var query = from n in context.T_User_Role
                    //            join r in context.T_Role on n.Role_ID equals r.Role_ID
                    //            where n.User_ID == user.User_ID
                    //            select r.Role_Name;
                    var query = user.T_Roles.Select(p => p.Role_Name);
                    lblUserRole.Text = string.Join(" ", query.ToList());
                }
                
                if (user.User_Status == 1)
                {
                    lblState.Text = "启用";

                    lbtnState.Text = "停用";
                    lbtnState.OnClientClick = "if (!confirm('是否停用用户？')) {return false;}";
                }
                else
                {
                    lblState.Text = "停用";

                    lbtnState.Text = "启用";
                    lbtnState.OnClientClick = "if (!confirm('是否启用用户？')) {return false;}";
                }

                lblDept.Text = "";

                lbtnReset.CommandArgument = user.User_ID.ToString();
                lbtnState.CommandArgument = user.User_ID.ToString();
                lbtnEdit.CommandArgument = user.User_ID.ToString();
                lbtnDel.CommandArgument = user.User_ID.ToString();
                lbtnSet.CommandArgument = user.User_ID.ToString();

                if (user.User_ID == 1)
                {
                    lbtnReset.Enabled = false;
                    lbtnState.Enabled = false;
                    lbtnEdit.Enabled = false;
                    lbtnDel.Enabled = false;
                    lbtnSet.Enabled = false;
                }
            }
        }

        protected void gvShow_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                int id = Convert.ToInt32(e.CommandArgument);
                T_User user = context.T_User.Where(x => x.User_ID == id).FirstOrDefault();
                if (user == null)
                    return;

                if (e.CommandName == "re")
                {
                    ResetUser(user, context);
                }
                else if (e.CommandName == "state")
                {
                    ResetState(user, context);
                }
                else if (e.CommandName == "up")
                {
                    UpdateUser(user, context);
                }
                else if (e.CommandName == "del")
                {
                    DeleteUser(user, context);
                }
                else if (e.CommandName == "set")
                {
                    SetRole(user, context);
                }
            }
        }

        private void ResetUser(T_User user, DrisionDbContext context)
        {
            user.User_Password = this.Md5("1");
            context.SaveChanges();

            UserShow();
            this.AjaxAlert("重置密码成功！");
        }

        private void ResetState(T_User user, DrisionDbContext context)
        {
            if (user.User_Status == 1)
            {
                user.User_Status = 0;
                context.SaveChanges();

                UserShow();
                this.AjaxAlert("停用用户成功！");
                return;
            }
            else if (user.User_Status == 0)
            {
                user.User_Status = 1;
                context.SaveChanges();

                UserShow();
                this.AjaxAlert("启用用户成功！");
                return;
            }
        }

        private void UpdateUser(T_User user, DrisionDbContext context)
        {
            txtCode.Text = user.User_Code;
            txtName.Text = user.User_Name;

            if (user.EntryDate == null)
            {
                txtEntryTime.Text = "";
            }
            else
            {
                txtEntryTime.Text = Convert.ToDateTime(user.EntryDate).ToString("yyyy-MM-dd");
            }

            txtCardNum.Text = user.Card_No;
            txtPhone.Text = user.User_Mobile;
            txtEmail.Text = user.User_EMail;


            if (context.T_Department.Where(x => x.Department_ID == user.Department_ID).Count() != 0)
            {
                ddlDept.SelectedValue = user.Department_ID.ToString();
            }
            else
            {
                ddlDept.SelectedIndex = -1;
            }

            rbtnState.SelectedValue = user.User_Status.ToString();
            txtRemark.Text = user.User_Comment;

            lblEdit.Text = user.User_ID.ToString();
            btnAdd.Visible = false;
            btnEdit.Visible = true;
            Panel1.Visible = true;
        }

        private void DeleteUser(T_User user, DrisionDbContext context)
        {
            context.T_User.Remove(user);
            context.SaveChanges();

            UserShow();
            if (lblEdit.Text.Trim() == user.User_ID.ToString())
            {
                ClearControl();
            }
            
            if (user.User_ID == this.LoginUserID)
            {
                LoginUser = null;

                Session["ModuleId"] = 1;
                Session["FunctionId"] = 101;
                Response.Redirect("~/Home/Login.aspx");
            }
            this.AjaxAlert("删除用户成功！");
        }

        private void SetRole(T_User user, DrisionDbContext context)
        {
            Response.Redirect("SetRole.aspx?UserId=" + user.User_ID);
        }

        private void ClearControl()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtEntryTime.Text = "";
            txtCardNum.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            ddlDept.SelectedIndex = -1;
            rbtnState.SelectedValue = "1";
            rbtnIsME.SelectedValue = "0";
            txtRemark.Text = "";
            lblEdit.Text = "";
            btnAdd.Visible = true;
            btnEdit.Visible = false;
            Panel1.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();

            if (string.IsNullOrEmpty(lblEdit.Text.Trim()))
            {
                AddUser(code);
            }
            else
            {
                int id = Convert.ToInt32(lblEdit.Text.Trim());
                EditUser(code, id);
            }
        }

        private void AddUser(string code)
        {
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                T_User user = new T_User();
                if (ddlDept.Items.Count == 0)
                {
                    this.AjaxAlert("部门不存在，请先添加部门！");
                    return;
                }
                else
                {
                    user.Department_ID = Convert.ToInt32(ddlDept.SelectedValue);
                }

                if (context.T_User.Where(x => x.User_Code == code).Count() != 0)
                {
                    this.AjaxAlert("用户工号不能重复！");
                    return;
                }
                else
                {
                    user.User_Code = code;
                }

                var query = context.T_User.OrderByDescending(x => x.User_ID).FirstOrDefault();
                user.User_ID = query == null ? 1 : query.User_ID + 1;

                user.User_Password = this.Md5("1");
                user.User_Name = txtName.Text.Trim();
                user.EntryDate = Convert.ToDateTime(txtEntryTime.Text.Trim());
                user.Card_No = txtCardNum.Text.Trim();
                user.User_Mobile = txtPhone.Text.Trim();
                user.User_EMail = txtEmail.Text.Trim();
                user.User_Status = Convert.ToInt32(rbtnState.SelectedValue);
                user.User_Comment = txtRemark.Text.Trim();
                
                context.SaveChanges();

            }
            UserShow();
            ClearControl();
            this.AjaxAlert("添加用户成功！");

        }

        private void EditUser(string code, int id)
        {
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                T_User user = context.T_User.Where(x=>x.User_ID == id).FirstOrDefault();
                if (user == null)
                    return;

                if (ddlDept.Items.Count == 0)
                {
                    this.AjaxAlert("部门不存在，请先添加部门！");
                    return;
                }
                else
                {
                    user.Department_ID = Convert.ToInt32(ddlDept.SelectedValue);
                }

                if (context.T_User.Where(x => x.User_ID != id && x.User_Code == code).Count() != 0)
                {
                    this.AjaxAlert("用户工号不能重复！");
                    return;
                }
                else
                {
                    user.User_Code = code;
                }

                user.User_Name = txtName.Text.Trim();
                user.EntryDate = Convert.ToDateTime(txtEntryTime.Text.Trim());
                user.Card_No = txtCardNum.Text.Trim();
                user.User_Mobile = txtPhone.Text.Trim();
                user.User_EMail = txtEmail.Text.Trim();
                user.User_Status = Convert.ToInt32(rbtnState.SelectedValue);
                user.User_Comment = txtRemark.Text.Trim();

                context.SaveChanges();

            }

            UserShow();
            ClearControl();
            this.AjaxAlert("编辑用户成功！");

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
            UserShow();
        }
    }
}
