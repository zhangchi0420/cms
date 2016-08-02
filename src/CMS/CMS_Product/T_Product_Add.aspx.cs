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
    public partial class T_Product_Add : BasePage,IPage_T_Product_Add
    {   
        //实现接口
        
                TextControl IPage_T_Product_Add.tbProduct_Name{get { return this.tbProduct_Name; }} 
                Uploader IPage_T_Product_Add.txtImgVB{get { return this.txtImgVB; }} 
                TextControl IPage_T_Product_Add.txtUrlAZ{get { return this.txtUrlAZ; }} 
                TextControl IPage_T_Product_Add.txtProductSummary5X{get { return this.txtProductSummary5X; }} 
                TextControl IPage_T_Product_Add.txtDesignStyleJM{get { return this.txtDesignStyleJM; }} 
                TextControl IPage_T_Product_Add.txtDesigners3W{get { return this.txtDesigners3W; }} 
                TextControl IPage_T_Product_Add.txtArea3D{get { return this.txtArea3D; }} 
                Dropdown IPage_T_Product_Add.ddlProductTypeIdYW{get { return this.ddlProductTypeIdYW; }} 

        public override long PageID
        {
            get
            {
                return 2000000308089;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Product_Add";
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
                    
                    //绑定引用字段下拉 
                                dynamic result1 = null;
            result1 = this.DataHelper.FetchAll<T_ProductType>();
            
            foreach(var m in result1)
            {
                var p = new DataWrapper(m);
ddlProductTypeIdYW.Items.Add(new ComboItem() 
                { 
                    Text = p["ProductType_Name"], 
                    Value = p["ProductType_Id"], 
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
                
            }
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
        global::Drision.Framework.Entity.T_Product entityModel = null;
            entityModel = new global::Drision.Framework.Entity.T_Product();
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
                __Id = entityModel.Product_Id;
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
         
         
         
         
         private void CreateModel(global::Drision.Framework.Entity.T_Product entityModel)
{
    
                    entityModel.SetPropertyConvertValue("Product_Name",tbProduct_Name.GetValue());
                entityModel.SetPropertyConvertValue("Img",txtImgVB.GetValue());
                entityModel.SetPropertyConvertValue("Url",txtUrlAZ.GetValue());
                entityModel.SetPropertyConvertValue("ProductSummary",txtProductSummary5X.GetValue());
                entityModel.SetPropertyConvertValue("DesignStyle",txtDesignStyleJM.GetValue());
                entityModel.SetPropertyConvertValue("Designers",txtDesigners3W.GetValue());
                entityModel.SetPropertyConvertValue("Area",txtArea3D.GetValue());
                entityModel.SetPropertyConvertValue("ProductTypeId",ddlProductTypeIdYW.GetValue());

}
         
         /// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}
    }
}