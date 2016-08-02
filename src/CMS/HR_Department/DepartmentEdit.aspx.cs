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
    public partial class DepartmentEdit : BasePage,IPage_DepartmentEdit
    {     
        //实现接口
        
                TextControl IPage_DepartmentEdit.ctrl_Deportment_Encode{get { return this.ctrl_Deportment_Encode; }} 
                TextControl IPage_DepartmentEdit.ctrl_Department_Name{get { return this.ctrl_Department_Name; }} 
                Dropdown IPage_DepartmentEdit.ctrl_Manager_ID{get { return this.ctrl_Manager_ID; }} 
                Dropdown IPage_DepartmentEdit.ctrl_Parent_ID{get { return this.ctrl_Parent_ID; }} 
                TextControl IPage_DepartmentEdit.ctrl_Department_Comment{get { return this.ctrl_Department_Comment; }} 
                ComboControl IPage_DepartmentEdit.ctrl_Department_Status86{get { return this.ctrl_Department_Status86; }} 
  
        public override long PageID
        {
            get
            {
                return 1000000020;
            }
        }  
        public override string PageName
        {
            get
            {
                return "DepartmentEdit";
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
                            ctrl_Department_Status86.DataSource = typeof(global::Drision.Framework.Enum.StopFlagEnum);
        ctrl_Department_Status86.DataBind();

                    //绑定引用字段下拉 
                                dynamic result1 = null;
            result1 = this.DataHelper.FetchAll<T_User>();
            
            foreach(var m in result1)
            {
                var p = new DataWrapper(m);
ctrl_Manager_ID.Items.Add(new ComboItem() 
                { 
                    Text = p["User_Name"], 
                    Value = p["User_ID"], 
                });
            }
            
            dynamic result2 = null;
            result2 = this.DataHelper.FetchAll<T_Department>();
            
            foreach(var m in result2)
            {
                var p = new DataWrapper(m);
ctrl_Parent_ID.Items.Add(new ComboItem() 
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
            var entityModel = this.GenericHelper.FindById<T_Department>(__Id);
            string FormatString = string.Empty;
                    
                                        try
                                        {
                                            ctrl_Deportment_Encode.SetValue(entityModel.Deportment_Encode);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Department_Name.SetValue(entityModel.Department_Name);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Manager_ID.SetValue(entityModel.Manager_ID);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Parent_ID.SetValue(entityModel.Parent_ID);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Department_Comment.SetValue(entityModel.Department_Comment);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Department_Status86.SetValue(entityModel.Department_Status);
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
protected void ctrl_depteidt_op_save_PreRender(object sender, EventArgs e)
{
    
    if (!IsPostBack)
    {
        if(!string.IsNullOrEmpty(this.ctrl_depteidt_op_save.ValidationGroup))
        {
            this.ctrl_depteidt_op_save.Attributes.Add("onclick", "return ValidateGroup('" + this.ctrl_depteidt_op_save.ValidationGroup + "');");
        }
        else
        {
            this.ctrl_depteidt_op_save.Attributes.Add("onclick", "return Validate();");
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
        global::Drision.Framework.Entity.T_Department entityModel = null;
            entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Department>(__Id);
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
                __Id = entityModel.Department_ID;
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
                
            
         
         private void CreateModel(global::Drision.Framework.Entity.T_Department entityModel)
{
    
                    entityModel.SetPropertyConvertValue("Deportment_Encode",ctrl_Deportment_Encode.GetValue());
                entityModel.SetPropertyConvertValue("Department_Name",ctrl_Department_Name.GetValue());
                entityModel.SetPropertyConvertValue("Manager_ID",ctrl_Manager_ID.GetValue());
                entityModel.SetPropertyConvertValue("Parent_ID",ctrl_Parent_ID.GetValue());
                entityModel.SetPropertyConvertValue("Department_Comment",ctrl_Department_Comment.GetValue());
                entityModel.SetPropertyConvertValue("Department_Status",ctrl_Department_Status86.GetValue());

}
         
         
         /// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}

         
    }
}