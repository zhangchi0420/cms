using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Drision.Framework.Entity;
using Drision.Framework.Web.Common;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using System.Configuration;
using Drision.Framework.OrgLibrary;
using Drision.Framework.Manager;

namespace Drision.Framework.Web
{
    public partial class Site : MasterPage
    {
        protected void SM_Error(object sender, AsyncPostBackErrorEventArgs e)
        {
            ScriptManager1.AsyncPostBackErrorMessage =
            this.BasePage.GetExceptionMessageWithStackTrace(e.Exception);
        }

        public int ModuleId
        {
            get { return (int)Session["ModuleId"]; }
            set { Session["ModuleId"] = value; }
        }

        public int FunctionId
        {
            get { return (int)Session["FunctionId"]; }
            set { Session["FunctionId"] = value; }
        }

        public BasePage BasePage
        {
            get
            {
                //return new BasePage();
                return this.Page as BasePage;
            }
        }

        public Drision.Framework.Entity.EntityCache EntityCache
        {
            get { return this.BasePage.EntityCache; }
        }

        public T_User LoginUser
        {
            get { return BasePage.LoginUser; }
            set { BasePage.LoginUser = value; }
        }

        public string WebSiteName
        {
            get;
            set;
        }

        public string TopTitleClassName
        {
            get
            {
                string TopTitleClassName = System.Configuration.ConfigurationManager.AppSettings[ConstKey.TopTitleClassNameKey];
                if (string.IsNullOrEmpty(TopTitleClassName))
                {
                    TopTitleClassName = "top_title";
                }
                return TopTitleClassName;
            }
        }
        public string WebSiteLogoUrl { get; set; }


        //protected void cbcLogin_CallBack(object sender, CallBackEventArgs e)
        //{
        //    string account = Convert.ToString(e.Context["Account"]);
        //    string pwd = Convert.ToString(e.Context["Password"]);
        //    pwd = this.Page.Md5(pwd);


        //    using (BizDataContext db = new BizDataContext())
        //    {
        //        var user = db.FirstOrDefault<T_User>(p => p.User_Code == account && p.User_Password == pwd);

        //        if (user == null)
        //        {
        //            e.Result = "用户名或密码错误！";
        //        }
        //        else
        //        {
        //            LoginUser = user;
        //            e.Result = "0";
        //        }
        //    }
        //}

        private void CheckFunctionId()
        {
            string homeUrl = AppSettings.HomePageUrl;
            if (homeUrl.StartsWith("~"))
            {
                homeUrl = homeUrl.Substring(1);
            }
            //如果自身是首页
            if (Request.Url.AbsolutePath.Contains(homeUrl))
            {
                return;
            }

            string msg = "非法路径";
            bool result = false;
            if (!string.IsNullOrEmpty(Request.Params["__fff"]))
            {
                long fid = Request.Params["__fff"].ToLong();
                if (fid != 0)
                {
                    var f = this.DataHelper.FindById<SysFunction>(fid);
                    if (f != null)
                    {
                        string url = GetPageUrl(f);
                        string trueName = url.Substring(url.LastIndexOf("/") + 1, url.LastIndexOf("?") - url.LastIndexOf("/") - 1);
                        string thisName = Request.RawUrl.Substring(Request.RawUrl.LastIndexOf("/") + 1, Request.RawUrl.LastIndexOf("?") - Request.RawUrl.LastIndexOf("/") -1);

                        if (trueName == thisName)
                        {
                            var p = this.FunctionItemList.FirstOrDefault(i => i.Function_ID == fid);
                            if (p != null)
                            {
                                result = true;
                            }
                            else
                            {
                                msg = "您没有权限访问此页面";
                            }
                        }
                    }
                }
            }

            if (!result)
            {
                Session["Error"] = msg;
                Response.Redirect("~/Error.aspx", true);
            }
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
                this.imgLogo.ImageUrl = this.WebSiteLogoUrl;


                InitLoad();
                ShowModuleList();
                //CheckFunctionId();
                //OnLoadStyle();   

                //SetSysRemindCount();
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "button", "ButtonStyle();", true);
        }

        //private void SetSysRemindCount()
        //{
        //    string sqlFormat = "select count(*) from SysRemind where OwnerId = {0} and State = {1} ";
        //    string sql = string.Format(sqlFormat, this.LoginUser.User_ID, (int)RemindStausEnum.New);
        //    int count = this.DataHelper.ExecuteScalar(sql).ToInt();
        //    this.btnSysRemind.Text = string.Format("站内信({0})", count);
        //}


        #region 目录菜单

        public BizDataContext DataHelper
        {
            get { return this.BasePage.DataHelper; }
        }

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

        private void InitLoad()
        {
            if (Request["mid"] != null)
            {
                this.ModuleId = Convert.ToInt32(Request["mid"]);
                if (Request["fid"] != null)
                {
                    this.FunctionId = Convert.ToInt32(Request["fid"]);
                }
            }

            lblUser.Text = LoginUser.User_Name;
            lblTime.Text = DateTime.Now.ToString("yyyy年MM月dd日");

        }

        /// <summary>
        /// 目录菜单缓存
        /// </summary>
        public List<SysFunction> FunctionItemList
        {
            get
            {
                if (Session["T_Function"] == null || (DateTime.Now - (DateTime)Session["Refunsh"]).Minutes > 5)
                {
                    int userid = LoginUser.User_ID;
                    var rfManager = this.RoleFuncManager;
                    var orgProxy = this.BasePage.OrgHelper;
                    var roleIdList = orgProxy.GetUserRoles(userid).Select(p => p.Role_ID).ToList();

                    var modelList = rfManager.GetFunctions(roleIdList);

                    //按照子菜单功能过滤获取所需要的主菜单
                    var menuList = this.DataHelper.Where<SysFunction>(x => x.Permission_Type == 0 || x.Permission_Type == null).ToList();
                    menuList = menuList.Where(x => modelList.Select(m => m.Permission_Type).Distinct().ToList().Contains(x.Function_ID)).ToList();
                    menuList.AddRange(modelList);

                    List<SysFunction> tmp = new List<SysFunction>();
                    foreach (var item in menuList)
                    {
                        if (tmp.Select(x => x.Function_ID).ToList().IndexOf(item.Function_ID) == -1)
                        {
                            tmp.Add(item);
                        }
                    }

                    Session["T_Function"] = tmp;

                    Session["Refunsh"] = DateTime.Now;  //刷新目录时间间隔为5分钟
                }
                return Session["T_Function"] as List<SysFunction>;

            }
            set
            {
                Session["T_Function"] = value;
            }
        }

        private void ShowModuleList()
        {
            this.repModule.DataSource = FunctionItemList.Where(x => (x.Permission_Type == 0 || x.Permission_Type == null)).OrderBy(p => p.OrderIndex).ToList();
            this.repModule.DataBind();

            this.repModule2.DataSource = this.repModule.DataSource;
            this.repModule2.DataBind();
        }

        protected void repModule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SysFunction mod = e.Item.DataItem as SysFunction;
                Repeater repFunction = e.Item.FindControl("repFunction") as Repeater;

                repFunction.DataSource = FunctionItemList.Where(x => x.Permission_Type == mod.Function_ID && x.Is_Menu == 1)
                    .Select(p => new
                    {
                        p.Function_ID,
                        p.Permission_Name,
                        p.Permission_Type,
                        PageURL = GetPageUrl(p),
                        p.OrderIndex,
                    }).OrderBy(p => p.OrderIndex)
                    .ToList();
                repFunction.DataBind();

            }
        }

        /// <summary>
        /// 计算URL
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public string GetPageUrl(SysFunction p)
        {
            var entityId = p.Entity_ID;
            var pageId = p.PageId;
            string result = string.Empty;

            if (!string.IsNullOrEmpty(p.URL)) //以自定义的为准
            {
                result = p.URL;
            }
            else //否则根据实体和页面来
            {
                if (entityId != null && pageId != null)
                {
                    var entity = this.EntityCache.FindById<SysEntity>(entityId);
                    var page = this.EntityCache.FindById<SysPage>(pageId);

                    result = GetPageUrl(entity, page);
                }
            }
            if (result.IsNullOrEmpty())
            {
                return result;
            }
            return AddModuleAndFunctionId(p, this.ResolveUrl(result));
        }

        public string AddModuleAndFunctionId(SysFunction p, string url)
        {
            long? mid = p.Permission_Type;
            long? fid = p.Function_ID;
            var parent = this.DataHelper.FindById<SysFunction>(p.Permission_Type);
            if (parent != null && parent.Permission_Type != null)
            {
                mid = parent.Permission_Type;
                fid = parent.Function_ID;
            }

            if (url.IndexOf('?') == -1)
            {
                url = string.Format("{0}?mid={1}&fid={2}&__fff={3}", url, mid, fid, p.Function_ID);
            }
            else
            {
                url = string.Format("{0}&mid={1}&fid={2}&__fff={3}", url, mid, fid, p.Function_ID);
            }
            return url;
        }

        public string GetPageUrl(SysEntity entity, SysPage page)
        {
            string result = string.Empty;
            //if (page != null && page.ModuleId != null)
            //{
            //    page.OwnerModule = db.FindById<SysModule>(page.ModuleId);
                if (page.OwnerModule != null)
                {
                    //page.OwnerModule.EntityCategory = db.FindById<SysEntityCategory>(page.OwnerModule.CategoryId);
                    if (page.OwnerModule.EntityCategory != null)
                    {
                        result = string.Format("~/{0}_{1}/{2}.aspx", page.OwnerModule.EntityCategory.CategoryName, page.OwnerModule.ModuleName, page.PageName);
                    }
                }
            //}
            else
            {
                if (entity != null)
                {
                    //entity.OwnerModule = db.FindById<SysModule>(entity.ModuleId);
                    if (entity.OwnerModule != null)
                    {
                        //entity.OwnerModule.EntityCategory = db.FindById<SysEntityCategory>(entity.OwnerModule.CategoryId);
                        if (entity.OwnerModule.EntityCategory != null)
                        {
                            result = string.Format("~/{0}_{1}/{2}.aspx", entity.OwnerModule.EntityCategory.CategoryName, entity.OwnerModule.ModuleName, page.PageName);
                        }
                    }
                }

            }

            return result;
        }

        #endregion

        #region 页面样式处理

        ///// <summary>
        ///// 页面框架样式
        ///// </summary>
        //public string LinkFrameStyle
        //{
        //    get { return Session["LinkFrame"] == null ? "/class/page.css" : Session["LinkFrame"] as string; }
        //    set { Session["LinkFrame"] = value; }
        //}
        //protected void lbtnFrameH_Click(object sender, EventArgs e)
        //{
        //    this.LinkFrameStyle = "/class/page.css";
        //    this.linkFrame.Href = this.LinkFrameStyle;
        //}
        //protected void lbtnFrameV_Click(object sender, EventArgs e)
        //{
        //    this.LinkFrameStyle = "/class/page.css";
        //    this.linkFrame.Href = this.LinkFrameStyle;
        //}

        ///// <summary>
        ///// 页面背景色样式
        ///// </summary>
        //public string LinkTemplateStyle
        //{
        //    get { return Session["LinkTemplate"] == null ? "/template_apple/template.css" : Session["LinkTemplate"] as string; }
        //    set { Session["LinkTemplate"] = value; }
        //}
        //protected void lbtnBlueColor_Click(object sender, EventArgs e)
        //{
        //    this.LinkTemplateStyle = "/template_apple/template.css";
        //    this.linkTemplate.Href = this.LinkTemplateStyle;
        //}
        //protected void lbtnOrangeColor_Click(object sender, EventArgs e)
        //{
        //    this.LinkTemplateStyle = "/template_android/template.css";
        //    this.linkTemplate.Href = this.LinkTemplateStyle;
        //}

        /// <summary>
        /// 加载页面样式
        /// </summary>
        //public void OnLoadStyle()
        //{
        //    this.lbtnFrameH.Attributes.Add("title", "横向布局");
        //    this.lbtnFrameH.Attributes.Add("onmouseover", "this.style.background= 'url(/images/shift_menu.png) left -15px'");
        //    this.lbtnFrameH.Attributes.Add("onmouseout", "this.style.background= 'url(/images/shift_menu.png) left 0'");

        //    this.lbtnFrameV.Attributes.Add("title", "纵向布局");
        //    this.lbtnFrameV.Attributes.Add("onmouseover", "this.style.background= 'url(/images/shift_menu.png) right -15px'");
        //    this.lbtnFrameV.Attributes.Add("onmouseout", "this.style.background= 'url(/images/shift_menu.png) right 0'");

        //    this.lbtnBlueColor.Attributes.Add("style", "display:inline; cursor: pointer; color:#FFF; background:#369");
        //    this.lbtnBlueColor.Attributes.Add("title", "蓝色");

        //    this.lbtnOrangeColor.Attributes.Add("style", "display:inline; cursor: pointer; color:#FFF; background:#C60");
        //    this.lbtnOrangeColor.Attributes.Add("title", "橙色");

        //    this.linkFrame.Href = this.LinkFrameStyle;
        //    this.linkTemplate.Href = this.LinkTemplateStyle;
        //}

        #endregion

        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            LoginUser = null;
            Session.Clear();
            FunctionItemList = null;
            Session["ModuleId"] = 1;
            Session["FunctionId"] = 1;
            BasePage.RedirectToLoginPage();
        }
        /// <summary>
        /// 供回调保存当前选中的标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbcTabSelectedIndex_CallBack(object sender, CallBackEventArgs e)
        {
            int TabSelectIndex = Convert.ToInt32(e.Context["Value"]);
            Session["TabSelectIndex"] = TabSelectIndex;
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            BasePage.RedirectToHomePage();
        }

        protected void btnSysRemind_Click(object sender, EventArgs e)
        {
            Response.Redirect("../SystemManagement/SysRemindManagement.aspx");
        }
    }
}