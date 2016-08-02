
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
using Drision.Framework.Entity;
using Drision.Framework.Web.Common;
//using Drision.Framework.Repository.EF;
using Drision.Framework.Plugin.Web;
using Drision.Framework.PageInterface;
using Drision.Framework.WebControls.Super;

namespace Drision.Framework.Web
{
    public partial class RoleQuery : BasePage,IPage_RoleQuery
    {
        //实现接口
        
                SText IPage_RoleQuery.ctrl_Role_Name{get { return this.ctrl_Role_Name; }} 

        public override long PageID
        {
            get
            {
                return 1000000360;
            }
        }  
        public override string PageName
        {
            get
            {
                return "RoleQuery";
            }
        }
        /// <summary>
        /// 当前选择的视图控件主键
        /// </summary>
        private long __SelectViewControlID
        {
            get;
            set;
        }
        /// <summary>
        /// 视图控件的名称(找条件或创建条件时用)
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
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                if (!IsPostBack)
                {
                    //赋值定义的参数
                    
                    //绑定枚举 
                    
                    //绑定引用字段下拉 
                    

                    
                    if(Request.QueryString["ViewID"] != null)
                    {
                        __SelectViewControlID = Convert.ToInt64(Request.QueryString["ViewID"]);
                    }
                    else
                    {
                        __SelectViewControlID = 1000000363;
                    }
                    
                }
                else
                {
                    __SelectViewControlID = Convert.ToInt64(Request[Hidden_SelectViewControlID.UniqueID]);
                }
                ReLoadControl();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        //页面定义的参数
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //加入查询条件列表，设置默认值-----2013-11-6 V3.0 修改
                    //暂时只考虑二级联动，后期考虑多级的，反正要保证最后一级联动的最后赋值
                this.AddOrUpdateQueryCondition(this.ctrl_Role_Name, null);   

                if (!this.IsPostBack)
                {
                    //从菜单进入要清空保存的所有查询条件，不过菜单中URL的Reload=true可能暂时没加上
                    if (Request.QueryString["Reload"] != null)
                    {
                        GridParmaList.Clear();
                    }

                    
                    if(Request.QueryString["ViewID"] == null)
                    {
                        Hidden_SelectViewControlID.Value = "1000000363";
                    }
                    else
                    {
                        Hidden_SelectViewControlID.Value = Request.QueryString["ViewID"];
                    }
                    
                    //设置按钮的权限隐藏
                    

                    //父子视图名称固定
                    
                    //手风琴名称固定
                }                
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            try
            {
		        if (!this.IsPostBack)
                {
                    //加载保存的条件
if(GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == __ViewControlName).Count() > 0)
{
    GridParma thisGridParma = GridParmaList.First(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == __ViewControlName);
    if(thisGridParma != null)
    {
        
        //2013-11-6 V3.0 修改
        this.SetQueryCondition_FromGridParmaToControl(thisGridParma);
    }
}
                    ReLoadData();                    
		        }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        
        /// <summary>
        /// 加载控件
        /// </summary>
        private void ReLoadControl()
        {
            Control uc = null;
            
ctrl_rolequery_viewlistitem.CssClass = "";
            if(__SelectViewControlID == 1000000363)
            {
ctrl_rolequery_viewlistitem.CssClass = "hot_line";
                __ViewControlName = "ctrl_rolequery_view";
                uc = this.LoadControl("~/AutoUserControls/HR_Common/T_Role_ctrl_rolequery_view.ascx");
                uc.ID = "ctrl_rolequery_view";
                this.lblViewControlTitle.Text =  "所有角色";
            }
            
            
ViewItem5C.CssClass = "";
            if(__SelectViewControlID == 1000000383)
            {
ViewItem5C.CssClass = "hot_line";
                __ViewControlName = "viewcontrolB6";
                uc = this.LoadControl("~/AutoUserControls/HR_Common/T_Role_viewcontrolB6.ascx");
                uc.ID = "ctrl_rolequery_view";
                this.lblViewControlTitle.Text =  "启用角色";
            }
            
            
ViewItemR4.CssClass = "";
            if(__SelectViewControlID == 1000000384)
            {
ViewItemR4.CssClass = "hot_line";
                __ViewControlName = "viewcontrolVX";
                uc = this.LoadControl("~/AutoUserControls/HR_Common/T_Role_viewcontrolVX.ascx");
                uc.ID = "ctrl_rolequery_view";
                this.lblViewControlTitle.Text =  "停用角色";
            }
            
            this.divctrl_rolequery_view.Controls.Clear();
            this.divctrl_rolequery_view.Controls.Add(uc);
        }
        
        


protected void btnLoadView_Click(object sender, EventArgs e)
{
    try
    {
        Hidden_SelectViewControlID.Value = (sender as LinkButton).CommandArgument;
        __SelectViewControlID = Convert.ToInt64(Hidden_SelectViewControlID.Value);
        string __RedirectURL = this.Request.Url.PathAndQuery;
        if (__RedirectURL.Contains("ViewID="))
        {
            __RedirectURL = __RedirectURL.Remove(__RedirectURL.IndexOf("ViewID="));
            __RedirectURL = string.Format("{0}ViewID={1}", __RedirectURL, __SelectViewControlID);
        }
        else if (__RedirectURL.Contains("?"))
        {
            __RedirectURL = string.Format("{0}&ViewID={1}", __RedirectURL, __SelectViewControlID);
        }
        else
        {
            __RedirectURL = string.Format("{0}?ViewID={1}", __RedirectURL, __SelectViewControlID);
        }
        Response.Redirect(__RedirectURL);
    }
    catch (Exception ex)
    {
        this.AjaxAlert(ex);
    }
}


        


        /// <summary>
        /// 查询按钮也要验证查询条件有没有必输
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if(!string.IsNullOrEmpty(this.btnQuery.ValidationGroup))
                {
                    this.btnQuery.Attributes.Add("onclick", "return ValidateGroup('" + this.btnQuery.ValidationGroup + "');");
                }
                else
                {
                    this.btnQuery.Attributes.Add("onclick", "return Validate();");
                }
                             
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Refresh();
                this.AjaxAlert(string.Empty,"EnableButton();");
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex,"EnableButton();");
            }
        }
        /// <summary>
        /// 清空条件
        /// </summary>
        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            try
            {
                    //2013-11-6 V3.0 修改
    this.ClearQueryConditions();

                Refresh();
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
            
    
    GridParmaList.RemoveAll(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "ctrl_rolequery_view");
    CopyGridParma = new GridParma() 
    { 
        GridName = "ctrl_rolequery_view", 
        PageIndex = thisGridParma.PageIndex,
        PageName = thisGridParma.PageName,
        PageSize = thisGridParma.PageSize,
        QueryParmaList = thisGridParma.QueryParmaList,
        SortFieldName = thisGridParma.SortFieldName,
        SortDirection = thisGridParma.SortDirection,
    };
    GridParmaList.Add(CopyGridParma);
     
    
    GridParmaList.RemoveAll(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrolB6");
    CopyGridParma = new GridParma() 
    { 
        GridName = "viewcontrolB6", 
        PageIndex = thisGridParma.PageIndex,
        PageName = thisGridParma.PageName,
        PageSize = thisGridParma.PageSize,
        QueryParmaList = thisGridParma.QueryParmaList,
        SortFieldName = thisGridParma.SortFieldName,
        SortDirection = thisGridParma.SortDirection,
    };
    GridParmaList.Add(CopyGridParma);
     
    
    GridParmaList.RemoveAll(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrolVX");
    CopyGridParma = new GridParma() 
    { 
        GridName = "viewcontrolVX", 
        PageIndex = thisGridParma.PageIndex,
        PageName = thisGridParma.PageName,
        PageSize = thisGridParma.PageSize,
        QueryParmaList = thisGridParma.QueryParmaList,
        SortFieldName = thisGridParma.SortFieldName,
        SortDirection = thisGridParma.SortDirection,
    };
    GridParmaList.Add(CopyGridParma);
     
        }


        /// <summary>
        /// 重载BasePage的刷新方法(完全初始化，包括页码置为0)
        /// </summary>
        public override void Refresh()
        {
            //2013-11-6 V3.0 修改
GridParma thisGridParma = GetNewGridParma();
AddGridParma(thisGridParma);  

                

            //重新加载数据
            

                
            (this.divctrl_rolequery_view.FindControl("ctrl_rolequery_view") as IUserControl).InitData();
            

            InitAccordionAndParent();
        }       


        /// <summary>
        /// 重新查询，但页码等信息保留
        /// </summary>
        public void ReLoadData()
        {
            GridParma thisGridParma = GetNewGridParma();            
            if (GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == __ViewControlName).Count() > 0)
            {
                GridParma oldParma = GridParmaList.First(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == __ViewControlName);
                if (oldParma != null)
                {
                    oldParma.QueryParmaList = thisGridParma.QueryParmaList;
                }
            }
            else
            {
                AddGridParma(thisGridParma);
            }

            (this.divctrl_rolequery_view.FindControl("ctrl_rolequery_view") as IUserControl).LoadData();
            InitAccordionAndParent(false);
        }
        /// <summary>
        /// 初始化手风琴和父子视图
        /// </summary>
        private void InitAccordionAndParent(bool isInit = true)
        {            
            
            
            
        }

/// <summary>
/// 新增
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnAdd_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
            __RedirectURL = "~/HR_Common/RoleAdd.aspx";
                            
                EnqueueCurrentUrl();//2013-11-13 V3.0
            
    Response.Redirect(__RedirectURL,false);
    }
    catch (Exception ex)
    {
        this.AjaxAlert(ex,"EnableButton();");
    }
}        
         
         
         
         
     }
}