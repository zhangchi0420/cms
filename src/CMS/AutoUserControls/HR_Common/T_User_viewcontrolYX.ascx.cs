using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tension;
using Drision.Framework.Manager;
using System.Data.Common;
using Drision.Framework.Entity;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
using Drision.Framework.WebControls.Super;
//using Drision.Framework.Repository.EF;
using Drision.Framework.Plugin.Web;
using System.Data;
using System.Text;

namespace Drision.Framework.Web
{
    public partial class T_User_viewcontrolYX : System.Web.UI.UserControl,IUserControl
    {
        private GridParma _thisGridParma = null;
        private GridParma thisGridParma
        {
            get
            {
                if(_thisGridParma == null)
                {
                    IEnumerable<GridParma> GridParmaList = (this.Page as BasePage).GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrolYX");
                    if (GridParmaList.Count() == 0)
                    {
                        _thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "viewcontrolYX", QueryParmaList = new List<QueryParma>(),};
                        (this.Page as BasePage).GridParmaList.Add(_thisGridParma);
                    }
                    else
                    {
                        _thisGridParma = GridParmaList.First();
                    }
                }
                return _thisGridParma;
            }
        }
        public List<string> SelectedValues
        {
            get
            {
                return viewcontrolYX.SelectedValues;
            }
        }
        public GridControl gcList
        {
            get
            {
                return viewcontrolYX;
            }
        }

        public void SetMultiSelect()
        {
            if(this.gcList != null)
            {
                this.gcList.SingleSelect = false;    
            }
        }

        
            
            protected void Page_Load(object sender, EventArgs e)
            {
                try
                {
                   ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.viewcontrolYX.FindControl("btnviewcontrolYXExport"));
                }
                catch (Exception ex)
                {
                    (this.Page as BasePage).AjaxAlert(ex);
                }
            }
            
        
        /// <summary>
        /// 导出
        /// </summary>
        protected void btnviewcontrolYXExport_Click(object sender, EventArgs e)
        {
            try
            {
            if(viewcontrolYX.IsEmpty == true)
            {
                (this.Page as BasePage).AjaxAlert("无数据导出！");
                return;
            }
            //汇总列
            List<ViewSumField> SumFields = null;
            
            
                var _T_Attend_User_Query_Set_View_viewcontrolYX = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("T_Attend_User_Query_Set_View_viewcontrolYX");                
                //赋条件
                //2013-11-6 V3.0 修改
                (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_T_Attend_User_Query_Set_View_viewcontrolYX, thisGridParma);
                //如果有树作为条件的，也要把树条件加进来
                int DataCount = 0;
                var result = QueryData(_T_Attend_User_Query_Set_View_viewcontrolYX,ref DataCount);
                var fields = new List<HeaderSortField>()
                {
                        new  HeaderSortField() { HeaderText = "所属部门", DataTextField = "a$Department_ID_V",DataFormatString = _T_Attend_User_Query_Set_View_viewcontrolYX.ViewFieldFormatDict["a$Department_ID"] },
                        new  HeaderSortField() { HeaderText = "工号", DataTextField = "a$User_Code",DataFormatString = _T_Attend_User_Query_Set_View_viewcontrolYX.ViewFieldFormatDict["a$User_Code"] },
                        new  HeaderSortField() { HeaderText = "用户姓名", DataTextField = "a$User_Name",DataFormatString = _T_Attend_User_Query_Set_View_viewcontrolYX.ViewFieldFormatDict["a$User_Name"] },
                        new  HeaderSortField() { HeaderText = "是否考勤", DataTextField = "a$Is_Attend_V",DataFormatString = _T_Attend_User_Query_Set_View_viewcontrolYX.ViewFieldFormatDict["a$Is_Attend"] },
                };
                result.Rows.Cast<DataRow>().ToList().ToExcel(fields, "考勤用户列表.xls",SumFields,_T_Attend_User_Query_Set_View_viewcontrolYX.QuerySum());
            }
            catch (Exception ex)
            {
                (this.Page as BasePage).AjaxAlert(ex);
            }
        }
        /// <summary>
        /// 初始化查询绑定数据
        /// </summary>
        public void InitData()
        {
            //if (!this.IsPostBack)
            {
                thisGridParma.PageIndex = 0;
viewcontrolYX.PagerSettings.PageIndex = 0;
            }
            thisGridParma.PageSize = viewcontrolYX.PagerSettings.PageSize;
            LoadData();            
        }
        /// <summary>
        /// 表头点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void viewcontrolYX_HeaderClick(object sender, GridPostBackEventArgs e)
        {
            try
            {
                thisGridParma.SortFieldName = e.Parameter;
                if(thisGridParma.SortFieldName.EndsWith("_V"))
                {
                    thisGridParma.SortFieldName = thisGridParma.SortFieldName.Replace("_V","");
                }
                thisGridParma.SortDirection = e.Direction == SortDirection.Ascending ? "asc" : "desc";
                LoadData();
            }
            catch (Exception ex)
            {
                (this.Page as BasePage).AjaxAlert(ex);
            }
        }
        /// <summary>
        /// 分页点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void viewcontrolYX_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            try
            {
                thisGridParma.PageIndex = e.PageIndex;
                thisGridParma.PageSize = e.PageSize;
                LoadData();
            }
            catch (Exception ex)
            {
                (this.Page as BasePage).AjaxAlert(ex);
            }
        }
        protected void viewcontrolYX_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //设置行操作的权限隐藏
        }
        /// <summary>
        /// 查询绑定数据
        /// </summary>
        public void LoadData()
        {
            //初始化时直接调用此方法，附上此值
            if (thisGridParma.PageIndex != 0)
            {
                viewcontrolYX.PagerSettings.PageIndex = thisGridParma.PageIndex;
            }
            thisGridParma.PageSize = viewcontrolYX.PagerSettings.PageSize;
            var _T_Attend_User_Query_Set_View_viewcontrolYX = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("T_Attend_User_Query_Set_View_viewcontrolYX");
            _T_Attend_User_Query_Set_View_viewcontrolYX.PageSize = thisGridParma.PageSize;
            _T_Attend_User_Query_Set_View_viewcontrolYX.PageIndex = thisGridParma.PageIndex;
            if(!string.IsNullOrEmpty(thisGridParma.SortFieldName))
            {
                _T_Attend_User_Query_Set_View_viewcontrolYX.OrderTableAlias = thisGridParma.SortFieldName.Split('$')[0];
                _T_Attend_User_Query_Set_View_viewcontrolYX.OrderFieldName = thisGridParma.SortFieldName.Split('$')[1];
                _T_Attend_User_Query_Set_View_viewcontrolYX.OrderMethod = thisGridParma.SortDirection == "asc"?Drision.Framework.LiteQueryDef.Internal.OrderMethod.ASC:Drision.Framework.LiteQueryDef.Internal.OrderMethod.DESC;
            }
            //赋条件,如果有条件或者是作为树形两种页面有树作为条件的，都要先定义
            
            //2013-11-6 V3.0 修改
            (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_T_Attend_User_Query_Set_View_viewcontrolYX, thisGridParma);


            //如果有树作为条件的，也要把树条件加进来

            var _dataCount = 0;
            var result = QueryData(_T_Attend_User_Query_Set_View_viewcontrolYX,ref _dataCount);
viewcontrolYX.PagerSettings.DataCount = _dataCount;

            //如果当前页无数据，则跳到上一页重查，直到有数据或跳到了第一页就没办法了
            while(result.Rows.Count == 0&&thisGridParma.PageIndex > 0)
            {
                thisGridParma.PageIndex--;
                _T_Attend_User_Query_Set_View_viewcontrolYX.PageIndex--;
                _T_Attend_User_Query_Set_View_viewcontrolYX.PageIndex--;
                result = QueryData(_T_Attend_User_Query_Set_View_viewcontrolYX,ref _dataCount);
            }

viewcontrolYX.DataSource = result;
            
            //以下为列统计
            

            //添加格式化字符串
                    viewcontrolYX.SetFormatString("a$Department_ID_V",_T_Attend_User_Query_Set_View_viewcontrolYX.ViewFieldFormatDict["a$Department_ID"]);    
                    viewcontrolYX.SetFormatString("a$User_Code",_T_Attend_User_Query_Set_View_viewcontrolYX.ViewFieldFormatDict["a$User_Code"]);    
                    viewcontrolYX.SetFormatString("a$User_Name",_T_Attend_User_Query_Set_View_viewcontrolYX.ViewFieldFormatDict["a$User_Name"]);    

viewcontrolYX.DataBind();            
        }
        private DataTable QueryData(ViewQueryBase viewQuery,ref int DataCount)
        {
                
                //附加过来的条件，基本是级联查询
                string ParaName = Request["ParaName"];
                string ParaValue = Request["ParaValue"];
                if (ParaName != null)
                {
                    viewQuery.ViewQueryExs.Add(new ViewQueryEx(){ TableAlias = "a",FieldName = ParaName,CompareType = CompareTypeEnum.Equal,ConditionValue = ParaValue});
                }
                
                 
            DataCount = viewQuery.QueryCount();
            return viewQuery.Query();
        }

        private DataTable ExportData(ViewQueryBase viewQuery,ref int DataCount)
        {
            //附加过来的条件，基本是级联查询
            string ParaName = Request["ParaName"];
            string ParaValue = Request["ParaValue"];
            if (ParaName != null)
            {
                viewQuery.ViewQueryExs.Add(new ViewQueryEx(){ TableAlias = "a",FieldName = ParaName,CompareType = CompareTypeEnum.Equal,ConditionValue = ParaValue});
            }
            

            
            
            DataCount = viewQuery.QueryCount();
            return viewQuery.Query();
        }

        
        /// <summary>
        /// 设置考勤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRowEditHE_Click(object sender, EventArgs e)
        {
            try
            {
                string __RedirectURL = string.Empty;
                ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
                string id = (sender as LinkButton).CommandArgument;
                var entityModel = (this.Page as BasePage).GenericHelper.FindById<global::Drision.Framework.Entity.T_User>(id);
                PluginEventArgs __PluginEventArg = new PluginEventArgs() { entityModel = entityModel, RedirectURL = __RedirectURL, CurrentUserID = (this.Page as BasePage).LoginUserID };
                
                
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
                                
                                //(this.Page as BasePage).ListURL.Add(this.Request.RawUrl);
                                (this.Page as BasePage).EnqueueCurrentUrl(); //2013-11-13 V3.0
                                Response.Redirect("~/HR_Common/T_Attend_User_Set.aspx?id="+(sender as LinkButton).CommandArgument);
                                
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
                            (this.Page as BasePage).AjaxAlert(__ButtonOperationContext.AlertMessage,"EnableButton();");
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
                //按钮所有完成后的完善操作，如刷新数据等
            }
            catch (Exception ex)
            {
               DrisionLog.Add(ex);
               (this.Page as BasePage).AjaxAlert(ex,"EnableButton();");
            }
        }
        
    }
}