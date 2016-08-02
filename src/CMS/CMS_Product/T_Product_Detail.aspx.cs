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
    public partial class T_Product_Detail : BasePage,IPage_T_Product_Detail
    {        
        //实现接口
        
                Attachment IPage_T_Product_Detail.txtImg4P{get { return this.txtImg4P; }} 
                Label IPage_T_Product_Detail.txtUrlXZ{get { return this.txtUrlXZ; }} 
                Label IPage_T_Product_Detail.txtProductSummaryA7{get { return this.txtProductSummaryA7; }} 
                Label IPage_T_Product_Detail.txtState2S{get { return this.txtState2S; }} 
                Label IPage_T_Product_Detail.txtDesignStyleAQ{get { return this.txtDesignStyleAQ; }} 
                Label IPage_T_Product_Detail.txtDesignersQ3{get { return this.txtDesignersQ3; }} 
                Label IPage_T_Product_Detail.txtAreaKE{get { return this.txtAreaKE; }} 
                Label IPage_T_Product_Detail.txtContent8D{get { return this.txtContent8D; }} 
                Label IPage_T_Product_Detail.txtProduct_NameXR{get { return this.txtProduct_NameXR; }} 
                Label IPage_T_Product_Detail.ddlProductTypeId7K{get { return this.ddlProductTypeId7K; }} 

        public override long PageID
        {
            get
            {
                return 2000000308440;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Product_Detail";
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
            var entityModel = this.GenericHelper.FindById<T_Product>(__Id);
            string FormatString = string.Empty;
                    
                                    try
                                    {
                                        txtImg4P.SetValue(entityModel.Img);
                                    }
                                    catch(Exception){}
                                    
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtUrlXZ.Text = Server.HtmlEncode(Convert.ToString(entityModel.Url));
                                            }
                                            else
                                            {
                                                txtUrlXZ.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Url));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtProductSummaryA7.Text = Server.HtmlEncode(Convert.ToString(entityModel.ProductSummary));
                                            }
                                            else
                                            {
                                                txtProductSummaryA7.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.ProductSummary));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtState2S.Text = Server.HtmlEncode(EnumUtil.ValueToDescription("T_ProductStateEnum", entityModel.State));
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtDesignStyleAQ.Text = Server.HtmlEncode(Convert.ToString(entityModel.DesignStyle));
                                            }
                                            else
                                            {
                                                txtDesignStyleAQ.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.DesignStyle));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtDesignersQ3.Text = Server.HtmlEncode(Convert.ToString(entityModel.Designers));
                                            }
                                            else
                                            {
                                                txtDesignersQ3.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Designers));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txtAreaKE.Text = Server.HtmlEncode(Convert.ToString(entityModel.Area));
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtContent8D.Text = Server.HtmlEncode(Convert.ToString(entityModel.Content));
                                            }
                                            else
                                            {
                                                txtContent8D.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Content));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txtProduct_NameXR.Text = Server.HtmlEncode(Convert.ToString(entityModel.Product_Name));
                                            }
                                            else
                                            {
                                                txtProduct_NameXR.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Product_Name));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ddlProductTypeId7K.Text = Server.HtmlEncode(this.DataHelper.FindById<T_ProductType>(entityModel.ProductTypeId).ProductType_Name);
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
protected void btnReturnKP_Click(object sender, EventArgs e)
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