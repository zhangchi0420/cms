﻿using System;
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
    public partial class DepartmentDetail : BasePage,IPage_DepartmentDetail
    {        
        //实现接口
        
                Label IPage_DepartmentDetail.ctrl_Deportment_Encode{get { return this.ctrl_Deportment_Encode; }} 
                Label IPage_DepartmentDetail.ctrl_Department_Code{get { return this.ctrl_Department_Code; }} 
                Label IPage_DepartmentDetail.ctrl_Department_Name{get { return this.ctrl_Department_Name; }} 
                Label IPage_DepartmentDetail.ctrl_Department_Status{get { return this.ctrl_Department_Status; }} 
                Label IPage_DepartmentDetail.ctrl_Department_Comment{get { return this.ctrl_Department_Comment; }} 
                Label IPage_DepartmentDetail.txt_Department_Full_CodeWW{get { return this.txt_Department_Full_CodeWW; }} 
                Label IPage_DepartmentDetail.txt_Parent_DepartmentIDJM{get { return this.txt_Parent_DepartmentIDJM; }} 

        public override long PageID
        {
            get
            {
                return 1000000040;
            }
        }  
        public override string PageName
        {
            get
            {
                return "DepartmentDetail";
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
            var entityModel = this.GenericHelper.FindById<T_Department>(__Id);
            string FormatString = string.Empty;
                    
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_Deportment_Encode.Text = Server.HtmlEncode(Convert.ToString(entityModel.Deportment_Encode));
                                            }
                                            else
                                            {
                                                ctrl_Deportment_Encode.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Deportment_Encode));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_Department_Code.Text = Server.HtmlEncode(Convert.ToString(entityModel.Department_Code));
                                            }
                                            else
                                            {
                                                ctrl_Department_Code.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Department_Code));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_Department_Name.Text = Server.HtmlEncode(Convert.ToString(entityModel.Department_Name));
                                            }
                                            else
                                            {
                                                ctrl_Department_Name.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Department_Name));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            ctrl_Department_Status.Text = Server.HtmlEncode(EnumUtil.ValueToDescription("StopFlagEnum", entityModel.Department_Status));
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                ctrl_Department_Comment.Text = Server.HtmlEncode(Convert.ToString(entityModel.Department_Comment));
                                            }
                                            else
                                            {
                                                ctrl_Department_Comment.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Department_Comment));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            FormatString = string.Empty;
                                            if(string.IsNullOrEmpty(FormatString))
                                            {
                                                txt_Department_Full_CodeWW.Text = Server.HtmlEncode(Convert.ToString(entityModel.Department_Full_Code));
                                            }
                                            else
                                            {
                                                txt_Department_Full_CodeWW.Text = Server.HtmlEncode(string.Format(FormatString, entityModel.Department_Full_Code));
                                            }
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_Parent_DepartmentIDJM.Text = Server.HtmlEncode(this.DataHelper.FindById<T_Department>(entityModel.Parent_DepartmentID).Department_Name);
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
                
/// <summary>
/// 供初始化时设置选中的标签
/// </summary>
private void SetTabSelectedIndex()
{
}    }
}