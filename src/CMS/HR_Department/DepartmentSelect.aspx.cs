﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Drision.Framework.Manager;
using Drision.Framework.Entity;
using System.Data.Common;
using Tension;
using Drision.Framework.Enum;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
//using Drision.Framework.Repository.EF;
using Drision.Framework.Plugin.Web;
using Drision.Framework.PageInterface;
using Drision.Framework.WebControls.Super;

namespace Drision.Framework.Web
{
    public partial class DepartmentSelect : BasePage,IPage_DepartmentSelect
    {
        //实现接口
        
                SText IPage_DepartmentSelect.ctrl_Deportment_Encode{get { return this.ctrl_Deportment_Encode; }} 
                SText IPage_DepartmentSelect.ctrl_Department_Name{get { return this.ctrl_Department_Name; }} 
                    IUserControl IPage_DepartmentSelect.ctrl_deptselect_view{get { return this.ctrl_deptselect_view; }} 

        public override long PageID
        {
            get
            {
                return 1000000270;
            }
        }  
        public override string PageName
        {
            get
            {
                return "DepartmentSelect";
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
        //页面定义的参数
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //加入查询条件列表，设置默认值-----2013-11-6 V3.0 修改
                    //暂时只考虑二级联动，后期考虑多级的，反正要保证最后一级联动的最后赋值
                this.AddOrUpdateQueryCondition(this.ctrl_Deportment_Encode, null);   
                this.AddOrUpdateQueryCondition(this.ctrl_Department_Name, null);   

                if (!this.IsPostBack)
                {
                    //赋值定义的参数
                    
                    //绑定枚举 
                    
                    //绑定引用字段下拉 
                    
                    //给两属性赋值
                    __ViewControlName = "ctrl_deptselect_view";
                            
                    if(this.Request.Params["multiselect"] == "1")
                    {
                        ctrl_deptselect_view.SetMultiSelect();
                    }
                    //设置按钮的权限隐藏
                    
                    //空条件查询（相当于重置）
                    btnQuery_Click(null,null);
                }
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
            
    
    GridParmaList.RemoveAll(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "ctrl_deptselect_view");
    CopyGridParma = new GridParma() 
    { 
        GridName = "ctrl_deptselect_view", 
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
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                
                //2013-11-6 V3.0 修改
GridParma thisGridParma = GetNewGridParma();
AddGridParma(thisGridParma); 

                


                //查询数据
                ctrl_deptselect_view.InitData();
                this.AjaxAlert(string.Empty,"EnableButton();");
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
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

                //查询数据
                btnQuery_Click(null,null);
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
         }
         
         
         
         
     }
}