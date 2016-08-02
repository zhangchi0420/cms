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
using Drision.Framework.Plugin.Web;
using Drision.Framework.PageInterface;
using Drision.Framework.WebControls.Super;

namespace Drision.Framework.Web
{
    public partial class UserAdd : BasePage,IPage_UserAdd
    {   
        //实现接口
        
                Dropdown IPage_UserAdd.ctrl_User_Code{get { return this.ctrl_User_Code; }} 
                TextControl IPage_UserAdd.ctrl_Department_ID{get { return this.ctrl_Department_ID; }} 
                TextControl IPage_UserAdd.ctrl_User_Password{get { return this.ctrl_User_Password; }} 
                TextControl IPage_UserAdd.ctrl_User_Name{get { return this.ctrl_User_Name; }} 
                DateTimeControl IPage_UserAdd.ctrl_EntryDate{get { return this.ctrl_EntryDate; }} 
                TextControl IPage_UserAdd.ctrl_Card_No{get { return this.ctrl_Card_No; }} 
                TextControl IPage_UserAdd.ctrl_User_Mobile{get { return this.ctrl_User_Mobile; }} 
                TextControl IPage_UserAdd.ctrl_User_EMail{get { return this.ctrl_User_EMail; }} 
                ComboControl IPage_UserAdd.ctrl_User_Status{get { return this.ctrl_User_Status; }} 
                ComboControl IPage_UserAdd.ctrl_Is_Prohibit_Web{get { return this.ctrl_Is_Prohibit_Web; }} 
                ComboControl IPage_UserAdd.ctrl_Is_Prohibit_Mobile{get { return this.ctrl_Is_Prohibit_Mobile; }} 
                TextControl IPage_UserAdd.ctrl_User_Comment{get { return this.ctrl_User_Comment; }} 
                ComboControl IPage_UserAdd.txt_User_TypeVN{get { return this.txt_User_TypeVN; }} 

        public override long PageID
        {
            get
            {
                return 1000000090;
            }
        }  
        public override string PageName
        {
            get
            {
                return "UserAdd";
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
                    //赋值定义的参数
                    
                    //绑定启动流程下拉选择框
                    
                    //绑定枚举 
                            ctrl_User_Status.DataSource = typeof(global::Drision.Framework.Enum.EffectiveFlagEnum);
        ctrl_User_Status.DataBind();
        ctrl_Is_Prohibit_Web.DataSource = typeof(global::Drision.Framework.Enum.ForbiddenWebFlagEmum);
        ctrl_Is_Prohibit_Web.DataBind();
        ctrl_Is_Prohibit_Mobile.DataSource = typeof(global::Drision.Framework.Enum.ForbiddenMobileFlagEmum);
        ctrl_Is_Prohibit_Mobile.DataBind();
        txt_User_TypeVN.DataSource = typeof(global::Drision.Framework.Enum.User_User_TypeEnum);
        txt_User_TypeVN.DataBind();

                    //绑定引用字段下拉 
                                dynamic result1 = null;
            result1 = this.DataHelper.FetchAll<T_Department>();
            
            foreach(var m in result1)
            {
                var p = new DataWrapper(m);
ctrl_User_Code.Items.Add(new ComboItem() 
                { 
                    Text = p["Department_Name"], 
                    Value = p["Department_ID"], 
                });
            }
            

                    //设置按钮的权限隐藏
                    
                    SetTabSelectedIndex();
                }
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
		        //赋默认值
                                
                if (ctrl_User_Status.Items.Where(p => p.Value == "1").Count() > 0)
                {
ctrl_User_Status.SetValue("1");
                }
                

            }
	    }
/// <summary>
/// 保存按钮添加验证
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void ctrl_useradd_op_save_PreRender(object sender, EventArgs e)
{
    
    if (!IsPostBack)
    {
        if(!string.IsNullOrEmpty(this.ctrl_useradd_op_save.ValidationGroup))
        {
            this.ctrl_useradd_op_save.Attributes.Add("onclick", "return ValidateGroup('" + this.ctrl_useradd_op_save.ValidationGroup + "');");
        }
        else
        {
            this.ctrl_useradd_op_save.Attributes.Add("onclick", "return Validate();");
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
            entityModel = new global::Drision.Framework.Entity.T_User();
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
                    this.GenericHelper.Save(entityModel);
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
         
         
         
         
         private void CreateModel(global::Drision.Framework.Entity.T_User entityModel)
{
    
                    entityModel.SetPropertyConvertValue("Department_ID",ctrl_User_Code.GetValue());
                entityModel.SetPropertyConvertValue("User_Code",ctrl_Department_ID.GetValue());
                entityModel.SetPropertyConvertValue("User_Password",ctrl_User_Password.GetValue());
                entityModel.SetPropertyConvertValue("User_Name",ctrl_User_Name.GetValue());
                entityModel.SetPropertyConvertValue("EntryDate",ctrl_EntryDate.GetValue());
                entityModel.SetPropertyConvertValue("Card_No",ctrl_Card_No.GetValue());
                entityModel.SetPropertyConvertValue("User_Mobile",ctrl_User_Mobile.GetValue());
                entityModel.SetPropertyConvertValue("User_EMail",ctrl_User_EMail.GetValue());
                entityModel.SetPropertyConvertValue("User_Status",ctrl_User_Status.GetValue());
                entityModel.SetPropertyConvertValue("Is_Prohibit_Web",ctrl_Is_Prohibit_Web.GetValue());
                entityModel.SetPropertyConvertValue("Is_Prohibit_Mobile",ctrl_Is_Prohibit_Mobile.GetValue());
                entityModel.SetPropertyConvertValue("User_Comment",ctrl_User_Comment.GetValue());
                entityModel.SetPropertyConvertValue("User_Type",txt_User_TypeVN.GetValue());

}
         
         /// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}
    }
}