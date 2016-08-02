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
using Drision.Framework.Repository.EF;
using Drision.Framework.Entity;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class SetRole : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BingDataToUserId(Request["UserId"]);
                ListBind();
            }
        }

        public int UserId
        {
            get { return (int)ViewState["UserId"]; }
            set { ViewState["UserId"] = value; }
        }
        /// <summary>
        /// 绑定用户ID号
        /// </summary>
        private void BingDataToUserId(string userId)
        {
            //如果传入ID号有误,则变成下拉菜单来处理
            bool nextSetp = true;
            
            if (!string.IsNullOrEmpty(userId))
            {
                this.UserId = Convert.ToInt32(userId);
                using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
                {
                    var query = context.T_User.Where(x => x.User_ID == this.UserId).FirstOrDefault();
                    if (query != null)
                    {
                        lblUserName.Text += query.User_Name;
                     
                        this.ddlRoleList.Visible = false;
                        nextSetp = false;
                    }
                }
            }

            if (nextSetp)
            {
                using (DrisionDbContext Context = new DrisionDbContext(GlobalObject.ConnString))
                {
                    var roleList = Context.T_User.Select(x => new { x.User_ID, x.User_Name }).ToList();
                    this.ddlRoleList.DataSource = roleList;
                    this.ddlRoleList.DataValueField = "User_ID";
                    this.ddlRoleList.DataTextField = "User_Name";
                    this.ddlRoleList.DataBind();

                    this.UserId = Convert.ToInt32(this.ddlRoleList.SelectedValue);

                    this.ddlRoleList.Visible = true;
                }
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void ListBind()
        {
            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                var activeRoleQuery = context.T_Role.Where(x => x.Role_Status == 0);
                int totalCount = activeRoleQuery.Count();
                Pager.Data_AspNetPager(AspNetPager1, totalCount, AspNetPager1.PageSize);
                var activeRoleList = activeRoleQuery.OrderBy(x => x.Role_ID).Skip(AspNetPager1.PageSize * (AspNetPager1.CurrentPageIndex - 1))
                                                    .Take(AspNetPager1.PageSize).ToList();
                //var userRoleList = context.T_User_Role.Where(x => x.User_ID == UserId).ToList();
                var userRoleList = context.T_User.FirstOrDefault(p => p.User_ID == UserId).T_Roles.ToList();
                this.gvShow.DataSource = activeRoleList.Select(x => new SetRoleEntity()
                {
                    Role_ID = x.Role_ID,
                    Role_Name = x.Role_Name,
                    Role_Comment = x.Role_Comment,
                    CheckedFlag = (userRoleList.Where(y => y.Role_ID == x.Role_ID).Count() != 0)
                }).ToList();
                this.gvShow.DataBind();
            }
        }
        /// <summary>
        /// 变换角色
        /// </summary>
        protected void ddlRoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UserId = Convert.ToInt32(this.ddlRoleList.SelectedValue);
            ListBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string RoleID = "";
            for (int i = 0; i < this.gvShow.Rows.Count; i++)
            {
                if (((CheckBox)this.gvShow.Rows[i].Cells[1].FindControl("CheckBox1")).Checked)
                {
                    RoleID += this.gvShow.DataKeys[i]["Role_ID"].ToString() + ",";
                }
            }

            if (RoleID == "")
            {
                this.AjaxAlert("没有选中任何项！");
                return;
            }

            RoleID = RoleID.Trim(',');
            string[] RoleArr = RoleID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
            {
                //var delList = context.T_User_Role.Where(x => x.User_ID == UserId).ToList();
                //foreach (var del in delList)
                //{
                //    context.T_User_Role.Remove(del);
                //}

                T_User user = context.T_User.FirstOrDefault(p => p.User_ID == UserId);
                if (user != null)
                {
                    user.T_Roles.Clear();                    

                    foreach (var str in RoleArr)
                    {
                        int roleId = int.Parse(str);
                        T_Role role = context.T_Role.FirstOrDefault(p => p.Role_ID == roleId);
                        if (role != null)
                        {
                            user.T_Roles.Add(role);
                        }
                    }

                    //int id;
                    //var query = context.T_User_Role.OrderByDescending(x => x.Id).FirstOrDefault();
                    //if (query == null)
                    //    id = 1;
                    //else
                    //    id = query.Id + 1;

                    //int roleId;
                    //foreach (var str in RoleArr)
                    //{
                    //    roleId = int.Parse(str);
                    //    context.T_User_Role.Add(new WebContext.Model.T_User_Role() {
                    //        Id = id,
                    //        User_ID = UserId,
                    //        Role_ID = roleId
                    //    });
                    //    id++;
                    //}

                    context.SaveChanges();
                }

            }

            this.AjaxAlert("设置成功！");
            //Response.Redirect("UserInfo.aspx");
            Response.Redirect("../HR_Common/UserQuery.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect("UserInfo.aspx");
            Response.Redirect("../HR_Common/UserQuery.aspx");
        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ListBind();
        }

    }

    /// <summary>
    /// 用于界面显示的实体信息
    /// </summary>
    [Serializable]
    public class SetRoleEntity
    {
        public int Role_ID { get; set; }
        public string Role_Name { get; set; }
        public string Role_Comment { get; set; }
        public bool CheckedFlag { get; set; }
    }

}
