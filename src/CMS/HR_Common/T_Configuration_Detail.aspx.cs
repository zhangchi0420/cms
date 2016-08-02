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
    public partial class T_Configuration_Detail : BasePage,IPage_T_Configuration_Detail
    {        
        //实现接口
        
                Label IPage_T_Configuration_Detail.txtConfiguration_TitleWG{get { return this.txtConfiguration_TitleWG; }} 
                Label IPage_T_Configuration_Detail.txtConfiguration_KeyWW{get { return this.txtConfiguration_KeyWW; }} 
                Label IPage_T_Configuration_Detail.txtConfiguration_ValuePH{get { return this.txtConfiguration_ValuePH; }} 
                Label IPage_T_Configuration_Detail.txtConfiguration_DescriptionWS{get { return this.txtConfiguration_DescriptionWS; }} 
                Label IPage_T_Configuration_Detail.ddlConfiguration_Group_Id7T{get { return this.ddlConfiguration_Group_Id7T; }} 

        public override long PageID
        {
            get
            {
                return 1000074395;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Configuration_Detail";
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
            var entityModel = this.GenericHelper.FindById<T_Configuration>(__Id);
            string FormatString = string.Empty;
                    
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtConfiguration_TitleWG.Text = Server.HtmlEncode(Convert.ToString(entityModel.Configuration_Title));
                                            }
                                            else
                                            {
                                                txtConfiguration_TitleWG.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Configuration_Title));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtConfiguration_KeyWW.Text = Server.HtmlEncode(Convert.ToString(entityModel.Configuration_Key));
                                            }
                                            else
                                            {
                                                txtConfiguration_KeyWW.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Configuration_Key));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtConfiguration_ValuePH.Text = Server.HtmlEncode(Convert.ToString(entityModel.Configuration_Value));
                                            }
                                            else
                                            {
                                                txtConfiguration_ValuePH.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Configuration_Value));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtConfiguration_DescriptionWS.Text = Server.HtmlEncode(Convert.ToString(entityModel.Configuration_Description));
                                            }
                                            else
                                            {
                                                txtConfiguration_DescriptionWS.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Configuration_Description));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ddlConfiguration_Group_Id7T.Text = Server.HtmlEncode(this.DataHelper.FindById<T_ConfigGroup>(entityModel.Configuration_Group_Id).ConfigGroup_Title);
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
        }
/// <summary>
/// 返回
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnReturnVX_Click(object sender, EventArgs e)
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
                
/// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}    }
}