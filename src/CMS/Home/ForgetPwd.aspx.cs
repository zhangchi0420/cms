using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Repository.EF;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.Home
{
    public partial class ForgetPwd : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            var UserCode = this.txtUserCode.Text;
            var Email = this.txtEMail.Text;
            if (string.IsNullOrWhiteSpace(UserCode) || string.IsNullOrWhiteSpace(Email))
            {
                this.AjaxAlert("请输入用户名和邮箱！");
                return;
            }

            using (DrisionDbContext context = new DrisionDbContext())
            {
                var User = context.T_User.FirstOrDefault(p => p.User_Code == UserCode && p.User_EMail == Email);
                if (User == null)
                {
                    this.AjaxAlert("输入的用户名和邮箱不匹配！");
                    return;
                }
                string NewPwd = new Random().Next(100000, 1000000).ToString();
                User.User_Password = this.Md5(NewPwd);
                context.SaveChanges();
                //调用邮件接口
                Drision.Framework.Manager.EmailManager.SendEmail(User.User_EMail, string.Empty, "密码帮助", string.Format("新密码为：{0}，请重新登录！", NewPwd));
                Response.Redirect("Login.aspx", false);
            }
        }
    }
}