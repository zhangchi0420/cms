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
    public partial class T_Role_Detail : BasePage,IPage_T_Role_Detail
    {        
        //实现接口
        
                Label IPage_T_Role_Detail.txtRole_NameEB{get { return this.txtRole_NameEB; }} 
                Label IPage_T_Role_Detail.txtRole_StatusGT{get { return this.txtRole_StatusGT; }} 
                Label IPage_T_Role_Detail.txtRole_CommentDV{get { return this.txtRole_CommentDV; }} 
                    IUserControl IPage_T_Role_Detail.viewcontrol5D{get { return this.viewcontrol5D; }} 

        public override long PageID
        {
            get
            {
                return 1000102457;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Role_Detail";
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
                    InitData();
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
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtRole_NameEB.Text = Server.HtmlEncode(Convert.ToString(entityModel.Role_Name));
                                            }
                                            else
                                            {
                                                txtRole_NameEB.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Role_Name));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtRole_StatusGT.Text = Server.HtmlEncode(EnumUtil.ValueToDescription("StopFlagEnum", entityModel.Role_Status));
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtRole_CommentDV.Text = Server.HtmlEncode(Convert.ToString(entityModel.Role_Comment));
                                            }
                                            else
                                            {
                                                txtRole_CommentDV.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Role_Comment));
                                            }
                                        }
                                        catch(Exception){}
                                        

            InitGrid();
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

       thisGridParma = GridParmaList.FirstOrDefault(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrol5D");
       if(thisGridParma == null)
       {
          thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "viewcontrol5D",QueryParmaList = new List<QueryParma>(),};
          GridParmaList.Add(thisGridParma);
       }
       QueryParma viewcontrol5DQueryParma = thisGridParma.QueryParmaList.FirstOrDefault(p=>p.QueryField.FieldName == "Role_ID");
       if(viewcontrol5DQueryParma == null)
       {
           viewcontrol5DQueryParma = new QueryParma()
           {
               QueryField = new QueryField(){FieldName = "Role_ID",},
               CompareTypeEnum = CompareTypeEnum.Equal,
               Value = __Id,
           };
           thisGridParma.QueryParmaList.Add(viewcontrol5DQueryParma);
       }
       thisGridParma.PageIndex = 0;
       viewcontrol5DQueryParma.Value = __Id;
      viewcontrol5D.InitData();
        }
/// <summary>
/// 添加用户
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnMMRelationAddBC_Click(object sender, EventArgs e)
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
                
/// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}    }
}