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
    public partial class UserEdit : BasePage,IPage_UserEdit
    {     
        //实现接口
        
                Dropdown IPage_UserEdit.ctrl_Department_ID{get { return this.ctrl_Department_ID; }} 
                TextControl IPage_UserEdit.ctrl_User_Code{get { return this.ctrl_User_Code; }} 
                TextControl IPage_UserEdit.ctrl_User_Name{get { return this.ctrl_User_Name; }} 
                DateTimeControl IPage_UserEdit.ctrl_EntryDate{get { return this.ctrl_EntryDate; }} 
                TextControl IPage_UserEdit.ctrl_Card_No{get { return this.ctrl_Card_No; }} 
                TextControl IPage_UserEdit.ctrl_User_Mobile{get { return this.ctrl_User_Mobile; }} 
                TextControl IPage_UserEdit.ctrl_User_EMail{get { return this.ctrl_User_EMail; }} 
                ComboControl IPage_UserEdit.ctrl_User_Status{get { return this.ctrl_User_Status; }} 
                ComboControl IPage_UserEdit.ctrl_Is_Prohibit_Web{get { return this.ctrl_Is_Prohibit_Web; }} 
                ComboControl IPage_UserEdit.ctrl_Is_Prohibit_Mobile{get { return this.ctrl_Is_Prohibit_Mobile; }} 
                TextControl IPage_UserEdit.ctrl_User_Comment{get { return this.ctrl_User_Comment; }} 
                ComboControl IPage_UserEdit.txt_User_TypeJW{get { return this.txt_User_TypeJW; }} 
  
        public override long PageID
        {
            get
            {
                return 1000000120;
            }
        }  
        public override string PageName
        {
            get
            {
                return "UserEdit";
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
                            ctrl_User_Status.DataSource = typeof(global::Drision.Framework.Enum.EffectiveFlagEnum);
        ctrl_User_Status.DataBind();
        ctrl_Is_Prohibit_Web.DataSource = typeof(global::Drision.Framework.Enum.ForbiddenWebFlagEmum);
        ctrl_Is_Prohibit_Web.DataBind();
        ctrl_Is_Prohibit_Mobile.DataSource = typeof(global::Drision.Framework.Enum.ForbiddenMobileFlagEmum);
        ctrl_Is_Prohibit_Mobile.DataBind();
        txt_User_TypeJW.DataSource = typeof(global::Drision.Framework.Enum.User_User_TypeEnum);
        txt_User_TypeJW.DataBind();

                    //绑定引用字段下拉 
                                dynamic result1 = null;
            result1 = this.DataHelper.FetchAll<T_Department>();
            
            foreach(var m in result1)
            {
                var p = new DataWrapper(m);
ctrl_Department_ID.Items.Add(new ComboItem() 
                { 
                    Text = p["Department_Name"], 
                    Value = p["Department_ID"], 
                });
            }
            

                    //查询并加载数据
                    __Id = Convert.ToInt32(Request.QueryString["id"]);
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
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
		    if (!this.IsPostBack)
            {
                InitData();
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
                                            ctrl_Department_ID.SetValue(entityModel.Department_ID);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_User_Code.SetValue(entityModel.User_Code);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_User_Name.SetValue(entityModel.User_Name);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_EntryDate.SetValue(entityModel.EntryDate);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Card_No.SetValue(entityModel.Card_No);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_User_Mobile.SetValue(entityModel.User_Mobile);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_User_EMail.SetValue(entityModel.User_EMail);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_User_Status.SetValue(entityModel.User_Status);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Is_Prohibit_Web.SetValue(entityModel.Is_Prohibit_Web);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Is_Prohibit_Mobile.SetValue(entityModel.Is_Prohibit_Mobile);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_User_Comment.SetValue(entityModel.User_Comment);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_User_TypeJW.SetValue(entityModel.User_Type);
                                        }
                                        catch(Exception){}
                                        

            InitGrid();
            //设置字段的隐藏可用属性
            
            //设置按钮的隐藏可用属性
            
            //设置按钮的权限隐藏
            
            //设置批量添加按钮的路径
                string __RedirectURL = string.Empty;

        }
        
        private void InitGrid()
        {
        }
/// <summary>
/// 保存按钮添加验证
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void ctrl_useredit_op_save_PreRender(object sender, EventArgs e)
{
    
    if (!IsPostBack)
    {
        if(!string.IsNullOrEmpty(this.ctrl_useredit_op_save.ValidationGroup))
        {
            this.ctrl_useredit_op_save.Attributes.Add("onclick", "return ValidateGroup('" + this.ctrl_useredit_op_save.ValidationGroup + "');");
        }
        else
        {
            this.ctrl_useredit_op_save.Attributes.Add("onclick", "return Validate();");
        }
    }
    
}


/// <summary>
/// 保存
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnSave_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
        global::Drision.Framework.Entity.T_User entityModel = null;
            entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_User>(__Id);
        CreateModel(entityModel);
        PluginEventArgs __PluginEventArg = new PluginEventArgs() { entityModel = entityModel, RedirectURL = __RedirectURL, CurrentUserID = this.LoginUserID };
        {
            #region 按钮前插件
            if(!__ButtonOperationContext.IsEnd)
            {
                try
                {
                    PagePluginFactory.InvokeBeforePlugins(sender,__PluginEventArg);
                    __ButtonOperationContext.RedirectURL = __PluginEventArg.RedirectURL;
                    if(!string.IsNullOrEmpty(__ButtonOperationContext.RedirectURL))
                    {
                        __ButtonOperationContext.IsEnd = true;
                    }
                }
                catch (MessageException ex)
                {
                    __ButtonOperationContext.IsEnd = true;
                    __ButtonOperationContext.AlertMessage = ex.Message;
                }
            }
            #endregion        
             
            #region 框架自己操作
            if(!__ButtonOperationContext.IsEnd)
            {
                    this.GenericHelper.Update(entityModel);
                __Id = entityModel.User_ID;
            }
            #endregion
            #region 按钮后插件
            if(!__ButtonOperationContext.IsEnd)
            {
                try
                {
                    PagePluginFactory.InvokeAfterPlugins(sender,__PluginEventArg);
                    __ButtonOperationContext.RedirectURL = __PluginEventArg.RedirectURL;
                    if(!string.IsNullOrEmpty(__ButtonOperationContext.RedirectURL))
                    {
                        __ButtonOperationContext.IsEnd = true;
                    }
                }
                catch (MessageException ex)
                {
                    __ButtonOperationContext.IsEnd = true;
                    __ButtonOperationContext.AlertMessage = ex.Message;
                }
            }
            #endregion
            //结束事务
            #region 最后处理
            if(__ButtonOperationContext.IsEnd)
            {
                if(!string.IsNullOrEmpty(__ButtonOperationContext.AlertMessage))
                {
                    this.AjaxAlert(__ButtonOperationContext.AlertMessage,"EnableButton();");
                    return;
                }
                if(!string.IsNullOrEmpty(__ButtonOperationContext.RedirectURL))
                {
                    Response.Redirect(__ButtonOperationContext.RedirectURL);
                    return;
                }
            }
            #endregion
        }
        
                                
                __RedirectURL = DequeueLastUrl("id",__Id); //2013-11-13 V3.0
                
            
            //如果没有id就加上id            
            __RedirectURL = AppendQueryStringToUrl(__RedirectURL, "id", __Id);//2013-11-13 V3.0                          
            
    Response.Redirect(__RedirectURL,false);
     }
     catch (Exception ex)
     {
        DrisionLog.Add(ex);
        this.AjaxAlert(ex,"EnableButton();");
     }
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

}
/// <summary>
/// 重置密码(123)
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btn2H_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
        var entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_User>(Request.QueryString["id"]??"-1");
        PluginEventArgs __PluginEventArg = new PluginEventArgs() { entityModel = entityModel, RedirectURL = __RedirectURL, CurrentUserID = this.LoginUserID };
        {
            #region 按钮前插件
            if(!__ButtonOperationContext.IsEnd)
            {
                try
                {
                    PagePluginFactory.InvokeBeforePlugins(sender,__PluginEventArg);
                    __ButtonOperationContext.RedirectURL = __PluginEventArg.RedirectURL;
                    if(!string.IsNullOrEmpty(__ButtonOperationContext.RedirectURL))
                    {
                        __ButtonOperationContext.IsEnd = true;
                    }
                }
                catch (MessageException ex)
                {
                    __ButtonOperationContext.IsEnd = true;
                    __ButtonOperationContext.AlertMessage = ex.Message;
                }
            }
            #endregion

             
            #region 按钮后插件
            if(!__ButtonOperationContext.IsEnd)
            {
                try
                {
                    PagePluginFactory.InvokeAfterPlugins(sender,__PluginEventArg);
                    __ButtonOperationContext.RedirectURL = __PluginEventArg.RedirectURL;
                    if(!string.IsNullOrEmpty(__ButtonOperationContext.RedirectURL))
                    {
                        __ButtonOperationContext.IsEnd = true;
                    }
                }
                catch (MessageException ex)
                {
                    __ButtonOperationContext.IsEnd = true;
                    __ButtonOperationContext.AlertMessage = ex.Message;
                }
            }
            #endregion
            //结束事务
            #region 最后处理
            if(__ButtonOperationContext.IsEnd)
            {
                if(!string.IsNullOrEmpty(__ButtonOperationContext.AlertMessage))
                {
                    this.AjaxAlert(__ButtonOperationContext.AlertMessage,"EnableButton();");
                    return;
                }
                if(!string.IsNullOrEmpty(__ButtonOperationContext.RedirectURL))
                {
                    Response.Redirect(__ButtonOperationContext.RedirectURL);
                    return;
                }
            }
            #endregion
        }
     }
     catch (Exception ex)
     {
        DrisionLog.Add(ex);
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
                
            
         
         private void CreateModel(global::Drision.Framework.Entity.T_User entityModel)
{
    
                    entityModel.SetPropertyConvertValue("Department_ID",ctrl_Department_ID.GetValue());
                entityModel.SetPropertyConvertValue("User_Code",ctrl_User_Code.GetValue());
                entityModel.SetPropertyConvertValue("User_Name",ctrl_User_Name.GetValue());
                entityModel.SetPropertyConvertValue("EntryDate",ctrl_EntryDate.GetValue());
                entityModel.SetPropertyConvertValue("Card_No",ctrl_Card_No.GetValue());
                entityModel.SetPropertyConvertValue("User_Mobile",ctrl_User_Mobile.GetValue());
                entityModel.SetPropertyConvertValue("User_EMail",ctrl_User_EMail.GetValue());
                entityModel.SetPropertyConvertValue("User_Status",ctrl_User_Status.GetValue());
                entityModel.SetPropertyConvertValue("Is_Prohibit_Web",ctrl_Is_Prohibit_Web.GetValue());
                entityModel.SetPropertyConvertValue("Is_Prohibit_Mobile",ctrl_Is_Prohibit_Mobile.GetValue());
                entityModel.SetPropertyConvertValue("User_Comment",ctrl_User_Comment.GetValue());
                entityModel.SetPropertyConvertValue("User_Type",txt_User_TypeJW.GetValue());

}
         
         
         /// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}

         
    }
}