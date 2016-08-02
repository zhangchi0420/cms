using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Drision.Framework.Manager;
using Drision.Framework.Entity;
using Drision.Framework.Enum;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
//using Drision.Framework.Repository.EF;
using Drision.Framework.WorkflowEngineCore;
using Drision.Framework.WebControls.Super;
using Drision.Framework.Plugin.Web;
using Drision.Framework.PageInterface;
using Drision.Framework.Common.Workflow;
using Drision.Framework.WorkflowEngineCore.Cache;

namespace Drision.Framework.Web
{
    public partial class UserDetail : BasePage,IPage_UserDetail
    {        
        //实现接口
        
                Label IPage_UserDetail.ctrl_Department_ID{get { return this.ctrl_Department_ID; }} 
                Label IPage_UserDetail.ctrl_User_Code{get { return this.ctrl_User_Code; }} 
                Label IPage_UserDetail.ctrl_User_Name{get { return this.ctrl_User_Name; }} 
                Label IPage_UserDetail.ctrl_EntryDate{get { return this.ctrl_EntryDate; }} 
                Label IPage_UserDetail.ctrl_Card_No{get { return this.ctrl_Card_No; }} 
                Label IPage_UserDetail.ctrl_User_Mobile{get { return this.ctrl_User_Mobile; }} 
                Label IPage_UserDetail.ctrl_User_EMail{get { return this.ctrl_User_EMail; }} 
                Label IPage_UserDetail.ctrl_User_Status{get { return this.ctrl_User_Status; }} 
                Label IPage_UserDetail.ctrl_Is_Prohibit_Web{get { return this.ctrl_Is_Prohibit_Web; }} 
                Label IPage_UserDetail.ctrl_Is_Prohibit_Mobile{get { return this.ctrl_Is_Prohibit_Mobile; }} 
                Label IPage_UserDetail.ctrl_User_Comment{get { return this.ctrl_User_Comment; }} 
                Label IPage_UserDetail.txt_User_Type25{get { return this.txt_User_Type25; }} 
                    IUserControl IPage_UserDetail.ViewControlYF{get { return this.ViewControlYF; }} 

        public override long PageID
        {
            get
            {
                return 1000000150;
            }
        }  
        public override string PageName
        {
            get
            {
                return "UserDetail";
            }
        }
        private int __Id
        {
            get
            {
                return Convert.ToInt32(ViewState["__Id"]);
            }
            set
            {
                ViewState["__Id"] = value;
            }
        }
        
        //页面定义的参数
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if(Request.QueryString["id"] == null)
                    {
                        //转到错误页
                        Response.Redirect("~/Error.aspx");
                    }
                    //赋值定义的参数
                    
                    //绑定启动流程下拉选择框
                    
                    //绑定枚举 
                    
                    //绑定引用字段下拉 
                    
                    //查询并加载数据
                    __Id = Convert.ToInt32(Request.QueryString["id"]);
                    InitData();
                    this.hcPostBack.Text = this.ClientScript.GetPostBackEventReference(this.hcPostBack, "refresh");                
                    SetTabSelectedIndex();

                    
                }     
                if (Request["__EVENTARGUMENT"] == "refresh")
                {
                    InitGrid();
                }   
                     
                //流程跟踪
                
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 重载BasePage的刷新方法
        /// </summary>
        public override void Refresh()
        {
            InitData();
        }

        private void InitData()
        {
            var entityModel = this.GenericHelper.FindById<T_User>(__Id);
            string FormatString = string.Empty;
                    
                                        try
                                        {
                                            ctrl_Department_ID.Text = Server.HtmlEncode(this.DataHelper.FindById<T_Department>(entityModel.Department_ID).Department_Name);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_User_Code.Text = Server.HtmlEncode(Convert.ToString(entityModel.User_Code));
                                            }
                                            else
                                            {
                                                ctrl_User_Code.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.User_Code));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_User_Name.Text = Server.HtmlEncode(Convert.ToString(entityModel.User_Name));
                                            }
                                            else
                                            {
                                                ctrl_User_Name.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.User_Name));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = "{0:" + this.ConfigHelper.GetConfigurationByKey("DateFormatString") + "}";
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_EntryDate.Text = Server.HtmlEncode(Convert.ToString(entityModel.EntryDate));
                                            }
                                            else
                                            {
                                                ctrl_EntryDate.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.EntryDate));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_Card_No.Text = Server.HtmlEncode(Convert.ToString(entityModel.Card_No));
                                            }
                                            else
                                            {
                                                ctrl_Card_No.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Card_No));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_User_Mobile.Text = Server.HtmlEncode(Convert.ToString(entityModel.User_Mobile));
                                            }
                                            else
                                            {
                                                ctrl_User_Mobile.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.User_Mobile));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_User_EMail.Text = Server.HtmlEncode(Convert.ToString(entityModel.User_EMail));
                                            }
                                            else
                                            {
                                                ctrl_User_EMail.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.User_EMail));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_User_Status.Text = Server.HtmlEncode(EnumUtil.ValueToDescription("EffectiveFlagEnum", entityModel.User_Status));
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Is_Prohibit_Web.Text = Server.HtmlEncode(EnumUtil.ValueToDescription("ForbiddenWebFlagEmum", entityModel.Is_Prohibit_Web));
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Is_Prohibit_Mobile.Text = Server.HtmlEncode(EnumUtil.ValueToDescription("ForbiddenMobileFlagEmum", entityModel.Is_Prohibit_Mobile));
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_User_Comment.Text = Server.HtmlEncode(Convert.ToString(entityModel.User_Comment));
                                            }
                                            else
                                            {
                                                ctrl_User_Comment.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.User_Comment));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_User_Type25.Text = Server.HtmlEncode(EnumUtil.ValueToDescription("User_User_TypeEnum", entityModel.User_Type));
                                        }
                                        catch(Exception){}
                                        

            InitGrid();
            //设置按钮的隐藏可用属性
            
            //设置按钮的权限隐藏
            
            //设置批量添加按钮的路径
                string __RedirectURL = string.Empty;

        }
        private void InitGrid()
        {
                    //加载视图数据
                    GridParma thisGridParma = null;
                    //IEnumerable<GridParma> thisGridParmaList;                

       thisGridParma = GridParmaList.FirstOrDefault(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "ViewControlYF");
       if(thisGridParma == null)
       {
          thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "ViewControlYF",QueryParmaList = new List<QueryParma>(),};
          GridParmaList.Add(thisGridParma);
       }
       QueryParma ViewControlYFQueryParma = thisGridParma.QueryParmaList.FirstOrDefault(p=>p.QueryField.FieldName == "User_ID");
       if(ViewControlYFQueryParma == null)
       {
           ViewControlYFQueryParma = new QueryParma()
           {
               QueryField = new QueryField(){FieldName = "User_ID",},
               CompareTypeEnum = CompareTypeEnum.Equal,
               Value = __Id,
           };
           thisGridParma.QueryParmaList.Add(ViewControlYFQueryParma);
       }
       thisGridParma.PageIndex = 0;
       ViewControlYFQueryParma.Value = __Id;
      ViewControlYF.InitData();
        }
/// <summary>
/// 返回
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnBack_Click(object sender, EventArgs e)
{
    string __RedirectURL = string.Empty;
                                    
                __RedirectURL = DequeueLastUrl("id",__Id); //2013-11-13 V3.0
                
                
                //如果没有id就加上id                
                __RedirectURL = AppendQueryStringToUrl(__RedirectURL, "id", __Id);//2013-11-13 V3.0
                
    Response.Redirect(__RedirectURL,false);

}/// <summary>
/// 添加角色
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void OperationTH_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        var entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_User>(__Id);
                    __RedirectURL = "~/HR_Common/SetRole.aspx";
                           
                EnqueueCurrentUrl();//2013-11-13 V3.0
                __RedirectURL = AppendQueryStringToUrl(__RedirectURL, "parentId", __Id);//2013-11-13 V3.0
            
    Response.Redirect(__RedirectURL,false);

    }
    catch (Exception ex)
    {
        this.AjaxAlert(ex,"EnableButton();");
    }
}                
                protected void cbcPatchAdd_CallBack(object sender, CallBackEventArgs e)
                {
                    try
                    {
                        string value = Convert.ToString(e.Context["Value"]);
                        switch(Convert.ToString(e.Context["ButtonName"]))
                        {
                            default:break;
                        }
                        e.Result = "1";
                    }
                    catch(Exception ex)
                    {
                        e.Result = ex.Message;
                    }
                }
                
/// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}    }
}