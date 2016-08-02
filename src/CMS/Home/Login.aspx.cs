using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Entity;
using Drision.Reg;
using Drision.Framework.Common;

namespace Drision.Framework.Web.Home
{
    public partial class Login : BasePage
    {
        public string WebSiteName
        {
            get;
            set;
        }
        public string AdminMail
        {
            get
            {
                string Email = System.Configuration.ConfigurationManager.AppSettings[ConstKey.AdminEmail];
                if (string.IsNullOrEmpty(Email))
                {
                    return "mailto:";
                }
                return string.Format("mailto:{0}", Email);
            }
        }
        public string WebSiteLogoUrl { get; set; }
        
        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                this.WebSiteName = System.Configuration.ConfigurationManager.AppSettings[ConstKey.WebSiteNameKey];
                if (string.IsNullOrWhiteSpace(this.WebSiteName))
                {
                    this.WebSiteName = "迪锐信套版管理系统";
                }
                this.WebSiteLogoUrl = System.Configuration.ConfigurationManager.AppSettings[ConstKey.WebSiteLogoUrlKey];
                if (string.IsNullOrWhiteSpace(this.WebSiteLogoUrl))
                {
                    this.WebSiteLogoUrl = "~/images/logo.png";
                }                
            }
        }

        protected void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!RegValidate.ValidateFromRegistryKey())
                //{
                //    Drision.Framework.Common.MessageBox.AlertAndGo(this, "此产品未经注册或已经过期", "Error.aspx");
                //    return;
                //}

                Session["T_Function"] = null; //清一下菜单缓存
                Session["ModuleId"] = 1;
                Session["FunctionId"] = 1;

                string originalPwd = this.txtPwd.Text.Trim();
                string pwd = this.Md5(originalPwd);
                string account = this.txtLogin.Text.Trim();

                using (BizDataContext db = new BizDataContext())
                {
                    var user = db.FirstOrDefault<T_User>(p => p.User_Code == account && (p.User_Password == pwd || p.User_Password == originalPwd));
                    
                    if (user == null)
                    {
                        this.AjaxAlert("用户名或密码错误！");
                    }
                    else if (user.User_Status == (int)Enum.EffectiveFlagEnum.No)
                    {
                        this.AjaxAlert("此用户已经被禁用！");
                    }
                    else
                    {                        
                        LoginUser = user;
                        if (this.loginRemind.Checked)
                        {
                            UserToCookie(user);
                        }
                        else
                        {
                            Response.Cookies["UserCode"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["User_Password"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["isChecked"].Expires = DateTime.Now.AddDays(-1);
                        }

                        //跳转
                        string returnUrl = Request.QueryString[WebConfigConstKey.returnUrlKey];
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            Response.Redirect(returnUrl);
                        }
                        else
                        {
                            Response.Redirect(AppSettings.HomePageUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        private void UserToCookie(T_User user)
        {
            //清除Cookies
            Request.Cookies.Clear();

            //HttpCookie aCookie = new HttpCookie("User");
            //aCookie.Values["UserId"] = user.User_ID.ToString();
            //aCookie.Values["UserCode"] = user.User_Code;
            //aCookie.Values["UserName"] = user.User_Name;
            //aCookie.Values["LastVisit"] = DateTime.Now.ToString();
            //aCookie.Expires = DateTime.Now.AddDays(1);
            //Response.Cookies.Add(aCookie);

            int day = 1;
            HttpCookie userCodeCk = new HttpCookie("UserCode", user.User_Code);
            userCodeCk.Expires = DateTime.Now.AddDays(day);
            Response.Cookies.Add(userCodeCk);

            HttpCookie pwd = new HttpCookie("User_Password",user.User_Password);
            pwd.Expires = DateTime.Now.AddDays(day);
            Response.Cookies.Add(pwd);

            HttpCookie isChecked = new HttpCookie("isChecked", this.loginRemind.Checked.ToString());
            isChecked.Expires = DateTime.Now.AddDays(day);
            Response.Cookies.Add(isChecked);


        }

        protected void cbcForgetpwd_CallBack(object sender, CallBackEventArgs e)
        {

            try
            {
                string UserCode = Convert.ToString(e.Context["UserCode"]);
                string Email = Convert.ToString(e.Context["Email"]);

                if (string.IsNullOrWhiteSpace(UserCode) || string.IsNullOrWhiteSpace(Email))
                {
                    e.Result = "请输入用户名和邮箱！";
                }
                else
                {
                    using (BizDataContext context = new BizDataContext())
                    {
                        var user = context.FirstOrDefault<T_User>(p => p.User_Code == UserCode && p.User_EMail == Email);
                        if (user == null)
                        {
                            e.Result = "输入的用户名和邮箱不匹配！";
                        }
                        else if (user.User_Status == (int)Enum.EffectiveFlagEnum.No)
                        {
                            e.Result = "此用户已经被禁用！";
                        }
                        else
                        {
                            string NewPwd = new Random().Next(100000, 1000000).ToString();
                            user.User_Password = this.Md5(NewPwd);
                            context.Update(user);
                            //调用邮件接口
                            (new Drision.Framework.Manager.EmailManager(this.DataHelper)).SendEmail(user.User_EMail, string.Empty, "密码帮助", string.Format("新密码为：{0}，请重新登录！", NewPwd));
                            e.Result = "已将新密码发到您的邮箱！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }       

    }
}
