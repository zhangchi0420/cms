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
    public partial class T_Article_Edit : BasePage,IPage_T_Article_Edit
    {     
        //实现接口
        
                TextControl IPage_T_Article_Edit.tbArticle_Name{get { return this.tbArticle_Name; }} 
                ComboControl IPage_T_Article_Edit.txtStateK6{get { return this.txtStateK6; }} 
                Dropdown IPage_T_Article_Edit.ddlColumnsIdRX{get { return this.ddlColumnsIdRX; }} 
                Uploader IPage_T_Article_Edit.txtImgEJ{get { return this.txtImgEJ; }} 
  
        public override long PageID
        {
            get
            {
                return 2000000308157;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Article_Edit";
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
                            txtStateK6.DataSource = typeof(global::Drision.Framework.Enum.T_ArticleStateEnum);
        txtStateK6.DataBind();

                    //绑定引用字段下拉 
                                dynamic result1 = null;
            result1 = this.DataHelper.FetchAll<T_Columns>();
            
            foreach(var m in result1)
            {
                var p = new DataWrapper(m);
ddlColumnsIdRX.Items.Add(new ComboItem() 
                { 
                    Text = p["Columns_Name"], 
                    Value = p["Columns_Id"], 
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
            var entityModel = this.GenericHelper.FindById<T_Article>(__Id);
            string FormatString = string.Empty;
                    
                                        try
                                        {
                                            tbArticle_Name.SetValue(entityModel.Article_Name);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtStateK6.SetValue(entityModel.State);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ddlColumnsIdRX.SetValue(entityModel.ColumnsId);
                                        }
                                        catch(Exception){}
                                        
        
                                    try
                                    {
                                        txtImgEJ.SetValue(entityModel.Img);
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
        global::Drision.Framework.Entity.T_Article entityModel = null;
            entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Article>(__Id);
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
                __Id = entityModel.Article_Id;
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
                
            
         
         private void CreateModel(global::Drision.Framework.Entity.T_Article entityModel)
{
    
                    entityModel.SetPropertyConvertValue("Article_Name",tbArticle_Name.GetValue());
                entityModel.SetPropertyConvertValue("State",txtStateK6.GetValue());
                entityModel.SetPropertyConvertValue("ColumnsId",ddlColumnsIdRX.GetValue());
                entityModel.SetPropertyConvertValue("Img",txtImgEJ.GetValue());

}
         
         
         /// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}

         
    }
}