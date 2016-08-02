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
    public partial class T_Configuration_Edit : BasePage,IPage_T_Configuration_Edit
    {     
        //实现接口
        
                TextControl IPage_T_Configuration_Edit.txtConfiguration_TitleNB{get { return this.txtConfiguration_TitleNB; }} 
                TextControl IPage_T_Configuration_Edit.txtConfiguration_KeyAY{get { return this.txtConfiguration_KeyAY; }} 
                TextControl IPage_T_Configuration_Edit.txtConfiguration_Value3N{get { return this.txtConfiguration_Value3N; }} 
                TextControl IPage_T_Configuration_Edit.txtConfiguration_DescriptionKD{get { return this.txtConfiguration_DescriptionKD; }} 
                Dropdown IPage_T_Configuration_Edit.ddlConfiguration_Group_IdA7{get { return this.ddlConfiguration_Group_IdA7; }} 
  
        public override long PageID
        {
            get
            {
                return 2000000307736;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Configuration_Edit";
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
                                dynamic result1 = null;
            result1 = this.DataHelper.FetchAll<T_ConfigGroup>();
            
            foreach(var m in result1)
            {
                var p = new DataWrapper(m);
ddlConfiguration_Group_IdA7.Items.Add(new ComboItem() 
                { 
                    Text = p["ConfigGroup_Title"], 
                    Value = p["Configuration_Group_Id"], 
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
            var entityModel = this.GenericHelper.FindById<T_Configuration>(__Id);
            string FormatString = string.Empty;
                    
                                        try
                                        {
                                            txtConfiguration_TitleNB.SetValue(entityModel.Configuration_Title);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtConfiguration_KeyAY.SetValue(entityModel.Configuration_Key);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtConfiguration_Value3N.SetValue(entityModel.Configuration_Value);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtConfiguration_DescriptionKD.SetValue(entityModel.Configuration_Description);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ddlConfiguration_Group_IdA7.SetValue(entityModel.Configuration_Group_Id);
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
protected void btnSaveFQ_PreRender(object sender, EventArgs e)
{
    
    if (!IsPostBack)
    {
        if(!string.IsNullOrEmpty(this.btnSaveFQ.ValidationGroup))
        {
            this.btnSaveFQ.Attributes.Add("onclick", "return ValidateGroup('" + this.btnSaveFQ.ValidationGroup + "');");
        }
        else
        {
            this.btnSaveFQ.Attributes.Add("onclick", "return Validate();");
        }
    }
    
}


/// <summary>
/// 保存
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnSaveFQ_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
        global::Drision.Framework.Entity.T_Configuration entityModel = null;
            entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Configuration>(__Id);
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
                __Id = entityModel.Configuration_Id;
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
protected void btnReturn7W_Click(object sender, EventArgs e)
{
    string __RedirectURL = string.Empty;
                                    
                __RedirectURL = DequeueLastUrl("id",__Id); //2013-11-13 V3.0
                
                
                //如果没有id就加上id                
                __RedirectURL = AppendQueryStringToUrl(__RedirectURL, "id", __Id);//2013-11-13 V3.0
                
    Response.Redirect(__RedirectURL,false);

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
                
            
         
         private void CreateModel(global::Drision.Framework.Entity.T_Configuration entityModel)
{
    
                    entityModel.SetPropertyConvertValue("Configuration_Title",txtConfiguration_TitleNB.GetValue());
                entityModel.SetPropertyConvertValue("Configuration_Key",txtConfiguration_KeyAY.GetValue());
                entityModel.SetPropertyConvertValue("Configuration_Value",txtConfiguration_Value3N.GetValue());
                entityModel.SetPropertyConvertValue("Configuration_Description",txtConfiguration_DescriptionKD.GetValue());
                entityModel.SetPropertyConvertValue("Configuration_Group_Id",ddlConfiguration_Group_IdA7.GetValue());

}
         
         
         /// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}

         
    }
}