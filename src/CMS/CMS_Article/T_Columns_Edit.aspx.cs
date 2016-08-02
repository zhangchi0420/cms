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
    public partial class T_Columns_Edit : BasePage,IPage_T_Columns_Edit
    {     
        //实现接口
        
                TextControl IPage_T_Columns_Edit.tbColumns_Name{get { return this.tbColumns_Name; }} 
                TextControl IPage_T_Columns_Edit.txtMenuKey2E{get { return this.txtMenuKey2E; }} 
                ComboControl IPage_T_Columns_Edit.txtStateGR{get { return this.txtStateGR; }} 
  
        public override long PageID
        {
            get
            {
                return 2000000308219;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Columns_Edit";
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
                            txtStateGR.DataSource = typeof(global::Drision.Framework.Enum.T_ColumnsStateEnum);
        txtStateGR.DataBind();

                    //绑定引用字段下拉 
                    
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
            var entityModel = this.GenericHelper.FindById<T_Columns>(__Id);
            string FormatString = string.Empty;
                    
                                        try
                                        {
                                            tbColumns_Name.SetValue(entityModel.Columns_Name);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtMenuKey2E.SetValue(entityModel.MenuKey);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtStateGR.SetValue(entityModel.State);
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
protected void btnSave_PreRender(object sender, EventArgs e)
{
    
    if (!IsPostBack)
    {
        if(!string.IsNullOrEmpty(this.btnSave.ValidationGroup))
        {
            this.btnSave.Attributes.Add("onclick", "return ValidateGroup('" + this.btnSave.ValidationGroup + "');");
        }
        else
        {
            this.btnSave.Attributes.Add("onclick", "return Validate();");
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
        global::Drision.Framework.Entity.T_Columns entityModel = null;
            entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Columns>(__Id);
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
                __Id = entityModel.Columns_Id;
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
protected void btnReturn_Click(object sender, EventArgs e)
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
                
            
         
         private void CreateModel(global::Drision.Framework.Entity.T_Columns entityModel)
{
    
                    entityModel.SetPropertyConvertValue("Columns_Name",tbColumns_Name.GetValue());
                entityModel.SetPropertyConvertValue("MenuKey",txtMenuKey2E.GetValue());
                entityModel.SetPropertyConvertValue("State",txtStateGR.GetValue());

}
         
         
         /// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}

         
    }
}