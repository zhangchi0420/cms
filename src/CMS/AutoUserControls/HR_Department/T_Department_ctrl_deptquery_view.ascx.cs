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
    public partial class T_Department_ctrl_deptquery_view : System.Web.UI.UserControl,IUserControl
    {
        private GridParma _thisGridParma = null;
        private GridParma thisGridParma
        {
            get
            {
                if(_thisGridParma == null)
                {
                    IEnumerable<GridParma> GridParmaList = (this.Page as BasePage).GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "ctrl_deptquery_view");
                    if (GridParmaList.Count() == 0)
                    {
                        _thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "ctrl_deptquery_view", QueryParmaList = new List<QueryParma>(),};
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
                return ctrl_deptquery_view.SelectedValues;
            }
        }
        public GridControl gcList
        {
            get
            {
                return ctrl_deptquery_view;
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
                   ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.ctrl_deptquery_view.FindControl("btnctrl_deptquery_viewExport"));
                }
                catch (Exception ex)
                {
                    (this.Page as BasePage).AjaxAlert(ex);
                }
            }
            
        
        /// <summary>
        /// 导出
        /// </summary>
        protected void btnctrl_deptquery_viewExport_Click(object sender, EventArgs e)
        {
            try
            {
            if(ctrl_deptquery_view.IsEmpty == true)
            {
                (this.Page as BasePage).AjaxAlert("无数据导出！");
                return;
            }
            //汇总列
            List<ViewSumField> SumFields = null;
            
            
                var _DepartmentQuery_View_ctrl_deptquery_view = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("DepartmentQuery_View_ctrl_deptquery_view");                
                //赋条件
                //2013-11-6 V3.0 修改
                (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_DepartmentQuery_View_ctrl_deptquery_view, thisGridParma);
                //如果有树作为条件的，也要把树条件加进来
                int DataCount = 0;
                var result = QueryData(_DepartmentQuery_View_ctrl_deptquery_view,ref DataCount);
                var fields = new List<HeaderSortField>()
                {
                        new  HeaderSortField() { HeaderText = "组织名称", DataTextField = "a$Department_Name",DataFormatString = _DepartmentQuery_View_ctrl_deptquery_view.ViewFieldFormatDict["a$Department_Name"] },
                        new  HeaderSortField() { HeaderText = "完整代码", DataTextField = "a$Department_Full_Code",DataFormatString = _DepartmentQuery_View_ctrl_deptquery_view.ViewFieldFormatDict["a$Department_Full_Code"] },
                        new  HeaderSortField() { HeaderText = "父级组织", DataTextField = "a$Parent_DepartmentID_V",DataFormatString = _DepartmentQuery_View_ctrl_deptquery_view.ViewFieldFormatDict["a$Parent_DepartmentID"] },
                        new  HeaderSortField() { HeaderText = "是否停用", DataTextField = "a$Department_Status_V",DataFormatString = _DepartmentQuery_View_ctrl_deptquery_view.ViewFieldFormatDict["a$Department_Status"] },
                };
                result.Rows.Cast<DataRow>().ToList().ToExcel(fields, "停用部门.xls",SumFields,_DepartmentQuery_View_ctrl_deptquery_view.QuerySum());
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
ctrl_deptquery_view.PagerSettings.PageIndex = 0;
            }
            thisGridParma.PageSize = ctrl_deptquery_view.PagerSettings.PageSize;
            LoadData();            
        }
        /// <summary>
        /// 表头点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ctrl_deptquery_view_HeaderClick(object sender, GridPostBackEventArgs e)
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
        protected void ctrl_deptquery_view_PageIndexChanging(object sender, GridPostBackEventArgs e)
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
        protected void ctrl_deptquery_view_RowDataBound(object sender, GridViewRowEventArgs e)
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
                ctrl_deptquery_view.PagerSettings.PageIndex = thisGridParma.PageIndex;
            }
            thisGridParma.PageSize = ctrl_deptquery_view.PagerSettings.PageSize;
            var _DepartmentQuery_View_ctrl_deptquery_view = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("DepartmentQuery_View_ctrl_deptquery_view");
            _DepartmentQuery_View_ctrl_deptquery_view.PageSize = thisGridParma.PageSize;
            _DepartmentQuery_View_ctrl_deptquery_view.PageIndex = thisGridParma.PageIndex;
            if(!string.IsNullOrEmpty(thisGridParma.SortFieldName))
            {
                _DepartmentQuery_View_ctrl_deptquery_view.OrderTableAlias = thisGridParma.SortFieldName.Split('$')[0];
                _DepartmentQuery_View_ctrl_deptquery_view.OrderFieldName = thisGridParma.SortFieldName.Split('$')[1];
                _DepartmentQuery_View_ctrl_deptquery_view.OrderMethod = thisGridParma.SortDirection == "asc"?Drision.Framework.LiteQueryDef.Internal.OrderMethod.ASC:Drision.Framework.LiteQueryDef.Internal.OrderMethod.DESC;
            }
            //赋条件,如果有条件或者是作为树形两种页面有树作为条件的，都要先定义
            
            //2013-11-6 V3.0 修改
            (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_DepartmentQuery_View_ctrl_deptquery_view, thisGridParma);


            //如果有树作为条件的，也要把树条件加进来

            var _dataCount = 0;
            var result = QueryData(_DepartmentQuery_View_ctrl_deptquery_view,ref _dataCount);
ctrl_deptquery_view.PagerSettings.DataCount = _dataCount;

            //如果当前页无数据，则跳到上一页重查，直到有数据或跳到了第一页就没办法了
            while(result.Rows.Count == 0&&thisGridParma.PageIndex > 0)
            {
                thisGridParma.PageIndex--;
                _DepartmentQuery_View_ctrl_deptquery_view.PageIndex--;
                _DepartmentQuery_View_ctrl_deptquery_view.PageIndex--;
                result = QueryData(_DepartmentQuery_View_ctrl_deptquery_view,ref _dataCount);
            }

ctrl_deptquery_view.DataSource = result;
            
            //以下为列统计
            

            //添加格式化字符串
                    ctrl_deptquery_view.SetFormatString("a$Department_Name",_DepartmentQuery_View_ctrl_deptquery_view.ViewFieldFormatDict["a$Department_Name"]);    
                    ctrl_deptquery_view.SetFormatString("a$Department_Full_Code",_DepartmentQuery_View_ctrl_deptquery_view.ViewFieldFormatDict["a$Department_Full_Code"]);    
                    ctrl_deptquery_view.SetFormatString("a$Parent_DepartmentID_V",_DepartmentQuery_View_ctrl_deptquery_view.ViewFieldFormatDict["a$Parent_DepartmentID"]);    

ctrl_deptquery_view.DataBind();            
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
        /// 启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowOperationSU_Click(object sender, EventArgs e)
        {
            try
            {
                string __RedirectURL = string.Empty;
                ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
                string id = (sender as LinkButton).CommandArgument;
                var entityModel = (this.Page as BasePage).GenericHelper.FindById<global::Drision.Framework.Entity.T_Department>(id);
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