
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
    public partial class UserSelect : BasePage,IPage_UserSelect
    {
        //实现接口
        
                Dropdown IPage_UserSelect.ctrl_Department_ID{get { return this.ctrl_Department_ID; }} 
                SText IPage_UserSelect.ctrl_User_Code{get { return this.ctrl_User_Code; }} 
                SText IPage_UserSelect.ctrl_User_Name{get { return this.ctrl_User_Name; }} 
                    IUserControl IPage_UserSelect.ctrl_userselect_view{get { return this.ctrl_userselect_view; }} 

        public override long PageID
        {
            get
            {
                return 1000000250;
            }
        }  
        public override string PageName
        {
            get
            {
                return "UserSelect";
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
                this.AddOrUpdateQueryCondition(this.ctrl_Department_ID, null);   
                this.AddOrUpdateQueryCondition(this.ctrl_User_Code, null);   
                this.AddOrUpdateQueryCondition(this.ctrl_User_Name, null);   

                if (!this.IsPostBack)
                {
                    //赋值定义的参数
                    
                    //绑定枚举 
                    
                    //绑定引用字段下拉 
                                dynamic result1 = null;
                result1 = this.DataHelper.FetchAll<T_Department>();
            
            foreach(var m in result1)
            {
                var p = new DataWrapper(m);
ctrl_Department_ID.Items.Add(new ComboItem() 
                { 
                    Text = p["Department_Name"], 
                    Value = p["Department_ID"], 
                });
            }
            

                    //给两属性赋值
                    __ViewControlName = "ctrl_userselect_view";
                            
                    if(this.Request.Params["multiselect"] == "1")
                    {
                        ctrl_userselect_view.SetMultiSelect();
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
            
    
    GridParmaList.RemoveAll(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "ctrl_userselect_view");
    CopyGridParma = new GridParma() 
    { 
        GridName = "ctrl_userselect_view", 
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
                ctrl_userselect_view.InitData();
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