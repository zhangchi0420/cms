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
    public partial class RoleEdit : BasePage,IPage_RoleEdit
    {     
        //实现接口
        
                TextControl IPage_RoleEdit.ctrl_Role_Name{get { return this.ctrl_Role_Name; }} 
                TextControl IPage_RoleEdit.ctrl_Role_Comment{get { return this.ctrl_Role_Comment; }} 
                    IUserControl IPage_RoleEdit.viewcontrolWG{get { return this.viewcontrolWG; }} 
  
        public override long PageID
        {
            get
            {
                return 1000000320;
            }
        }  
        public override string PageName
        {
            get
            {
                return "RoleEdit";
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
            var entityModel = this.GenericHelper.FindById<T_Role>(__Id);
            string FormatString = string.Empty;
                    
                                        try
                                        {
                                            ctrl_Role_Name.SetValue(entityModel.Role_Name);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Role_Comment.SetValue(entityModel.Role_Comment);
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
                    //加载视图数据
                    GridParma thisGridParma = null;
                    //IEnumerable<GridParma> thisGridParmaList;

       thisGridParma = GridParmaList.FirstOrDefault(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrolWG");
       if(thisGridParma == null)
       {
          thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "viewcontrolWG",QueryParmaList = new List<QueryParma>(),};
          GridParmaList.Add(thisGridParma);
       }
       QueryParma viewcontrolWGQueryParma = thisGridParma.QueryParmaList.FirstOrDefault(p=>p.QueryField.FieldName == "Role_ID");
       if(viewcontrolWGQueryParma == null)
       {
           viewcontrolWGQueryParma = new QueryParma()
           {
               QueryField = new QueryField(){FieldName = "Role_ID",},
               CompareTypeEnum = CompareTypeEnum.Equal,
               Value = __Id,
           };
           thisGridParma.QueryParmaList.Add(viewcontrolWGQueryParma);
       }
       thisGridParma.PageIndex = 0;
       viewcontrolWGQueryParma.Value = __Id;
      viewcontrolWG.InitData();
        }
/// <summary>
/// 保存按钮添加验证
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void ctrl_roleedit_op_save_PreRender(object sender, EventArgs e)
{
    
    if (!IsPostBack)
    {
        if(!string.IsNullOrEmpty(this.ctrl_roleedit_op_save.ValidationGroup))
        {
            this.ctrl_roleedit_op_save.Attributes.Add("onclick", "return ValidateGroup('" + this.ctrl_roleedit_op_save.ValidationGroup + "');");
        }
        else
        {
            this.ctrl_roleedit_op_save.Attributes.Add("onclick", "return Validate();");
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
        global::Drision.Framework.Entity.T_Role entityModel = null;
            entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Role>(__Id);
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
                __Id = entityModel.Role_ID;
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

}/// <summary>
/// 添加用户
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnMMRelationAddJZ_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        var entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Role>(__Id);
                    __RedirectURL = "~/HR_Common/SetUser.aspx";
                           
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
                
            
         
         private void CreateModel(global::Drision.Framework.Entity.T_Role entityModel)
{
    
                    entityModel.SetPropertyConvertValue("Role_Name",ctrl_Role_Name.GetValue());
                entityModel.SetPropertyConvertValue("Role_Comment",ctrl_Role_Comment.GetValue());

}
         
         
         /// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}

         
    }
}