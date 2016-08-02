
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Drision.Framework.Manager;
using System.Data.Common;
using Tension;
using Drision.Framework.Enum;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
using Drision.Framework.Entity;
//using Drision.Framework.Repository.EF;
using Drision.Framework.Plugin.Web;
using Drision.Framework.PageInterface;
using Drision.Framework.WebControls.Super;

namespace Drision.Framework.Web
{
    public partial class SetUser : BasePage,IPage_SetUser
    {
        //实现接口
        
                    IUserControl IPage_SetUser.viewcontrolXS{get { return this.viewcontrolXS; }} 

        public override long PageID
        {
            get
            {
                return 1000305524;
            }
        }  
        public override string PageName
        {
            get
            {
                return "SetUser";
            }
        }
        /// <summary>
        /// 视图控件的名称
        /// </summary>
        private string __ViewControlName
        {
            get
            {
                return ViewState["__ViewControlName"] == null ? "" : ViewState["__ViewControlName"].ToString();
            }
            set
            {
                ViewState["__ViewControlName"] = value;
            }
        }
        private int __Id
        {
            get
            {
                return Convert.ToInt32(ViewState["__Id"] ?? "0");
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
                    
                    __Id = Convert.ToInt32(Request["parentId"]);
                    //给两属性赋值
                    __ViewControlName = "viewcontrolXS";
                    //设置按钮的权限隐藏
                    
                    //初始化列表，到时看，如果数据量大可能初始不加载
                    viewcontrolXS.InitData();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        
                
/// <summary>
/// 2013-11-6 V3.0 修改
/// </summary>
private GridParma GetNewGridParma()
{
            GridParma thisGridParma = this.SetQueryCondition_FromControlToGridParma();
            thisGridParma.GridName = this.__ViewControlName;
            return thisGridParma;
        }

        /// <summary>
        /// 2013-11-6 V3.0 修改
        /// </summary>
        private void AddGridParma(GridParma thisGridParma)
        {
            GridParma CopyGridParma;
            
    
    GridParmaList.RemoveAll(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrolXS");
    CopyGridParma = new GridParma() 
    { 
        GridName = "viewcontrolXS", 
        PageIndex = thisGridParma.PageIndex,
        PageName = thisGridParma.PageName,
        PageSize = thisGridParma.PageSize,
        QueryParmaList = thisGridParma.QueryParmaList,
        SortFieldName = thisGridParma.SortFieldName,
        SortDirection = thisGridParma.SortDirection,
    };
    GridParmaList.Add(CopyGridParma);
     
        }

        
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {

            //2013-11-6 V3.0 修改
GridParma thisGridParma = GetNewGridParma();
AddGridParma(thisGridParma);
                
                //查询数据
                    viewcontrolXS.InitData();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
         }

/// <summary>
/// 确定
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnMMRelationSaveJ6_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
        PluginEventArgs __PluginEventArg = new PluginEventArgs() { entityModel = null, RedirectURL = __RedirectURL, CurrentUserID = this.LoginUserID };
        List<string> childIds = viewcontrolXS.SelectedValues;
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
                this.GenericHelper.MMSave(typeof(global::Drision.Framework.Entity.T_User),__Id,"T_Role",childIds);
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
}/// <summary>
/// 取消
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnReturnWA_Click(object sender, EventArgs e)
{
    string __RedirectURL = string.Empty;
                                    
                __RedirectURL = DequeueLastUrl("id",__Id); //2013-11-13 V3.0
                
                
                //如果没有id就加上id                
                __RedirectURL = AppendQueryStringToUrl(__RedirectURL, "id", __Id);//2013-11-13 V3.0
                
    Response.Redirect(__RedirectURL,false);

}         
         
         
         
     }
}
