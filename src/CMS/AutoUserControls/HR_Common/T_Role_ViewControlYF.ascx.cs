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
    public partial class T_Role_ViewControlYF : System.Web.UI.UserControl,IUserControl
    {
        private GridParma _thisGridParma = null;
        private GridParma thisGridParma
        {
            get
            {
                if(_thisGridParma == null)
                {
                    IEnumerable<GridParma> GridParmaList = (this.Page as BasePage).GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "ViewControlYF");
                    if (GridParmaList.Count() == 0)
                    {
                        _thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "ViewControlYF", QueryParmaList = new List<QueryParma>(),};
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
                return ViewControlYF.SelectedValues;
            }
        }
        public GridControl gcList
        {
            get
            {
                return ViewControlYF;
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
                   ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.ViewControlYF.FindControl("btnViewControlYFExport"));
                }
                catch (Exception ex)
                {
                    (this.Page as BasePage).AjaxAlert(ex);
                }
            }
            
        
        /// <summary>
        /// 导出
        /// </summary>
        protected void btnViewControlYFExport_Click(object sender, EventArgs e)
        {
            try
            {
            if(ViewControlYF.IsEmpty == true)
            {
                (this.Page as BasePage).AjaxAlert("无数据导出！");
                return;
            }
            //汇总列
            List<ViewSumField> SumFields = null;
            
            
                var _UserDetail_View_ViewControlYF = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("UserDetail_View_ViewControlYF");                
                //赋条件
                //2013-11-6 V3.0 修改
                (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_UserDetail_View_ViewControlYF, thisGridParma);
                //如果有树作为条件的，也要把树条件加进来
                int DataCount = 0;
                var result = QueryData(_UserDetail_View_ViewControlYF,ref DataCount);
                var fields = new List<HeaderSortField>()
                {
                        new  HeaderSortField() { HeaderText = "角色名称", DataTextField = "a$Role_Name",DataFormatString = _UserDetail_View_ViewControlYF.ViewFieldFormatDict["a$Role_Name"] },
                        new  HeaderSortField() { HeaderText = "是否停用", DataTextField = "a$Role_Status_V",DataFormatString = _UserDetail_View_ViewControlYF.ViewFieldFormatDict["a$Role_Status"] },
                        new  HeaderSortField() { HeaderText = "备注", DataTextField = "a$Role_Comment",DataFormatString = _UserDetail_View_ViewControlYF.ViewFieldFormatDict["a$Role_Comment"] },
                };
                result.Rows.Cast<DataRow>().ToList().ToExcel(fields, "角色列表.xls",SumFields,_UserDetail_View_ViewControlYF.QuerySum());
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
ViewControlYF.PagerSettings.PageIndex = 0;
            }
            thisGridParma.PageSize = ViewControlYF.PagerSettings.PageSize;
            LoadData();            
        }
        /// <summary>
        /// 表头点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ViewControlYF_HeaderClick(object sender, GridPostBackEventArgs e)
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
        protected void ViewControlYF_PageIndexChanging(object sender, GridPostBackEventArgs e)
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
        protected void ViewControlYF_RowDataBound(object sender, GridViewRowEventArgs e)
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
                ViewControlYF.PagerSettings.PageIndex = thisGridParma.PageIndex;
            }
            thisGridParma.PageSize = ViewControlYF.PagerSettings.PageSize;
            var _UserDetail_View_ViewControlYF = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("UserDetail_View_ViewControlYF");
            _UserDetail_View_ViewControlYF.PageSize = thisGridParma.PageSize;
            _UserDetail_View_ViewControlYF.PageIndex = thisGridParma.PageIndex;
            if(!string.IsNullOrEmpty(thisGridParma.SortFieldName))
            {
                _UserDetail_View_ViewControlYF.OrderTableAlias = thisGridParma.SortFieldName.Split('$')[0];
                _UserDetail_View_ViewControlYF.OrderFieldName = thisGridParma.SortFieldName.Split('$')[1];
                _UserDetail_View_ViewControlYF.OrderMethod = thisGridParma.SortDirection == "asc"?Drision.Framework.LiteQueryDef.Internal.OrderMethod.ASC:Drision.Framework.LiteQueryDef.Internal.OrderMethod.DESC;
            }
            //赋条件,如果有条件或者是作为树形两种页面有树作为条件的，都要先定义
            
            //2013-11-6 V3.0 修改
            (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_UserDetail_View_ViewControlYF, thisGridParma);


            //如果有树作为条件的，也要把树条件加进来

            var _dataCount = 0;
            var result = QueryData(_UserDetail_View_ViewControlYF,ref _dataCount);
ViewControlYF.PagerSettings.DataCount = _dataCount;

            //如果当前页无数据，则跳到上一页重查，直到有数据或跳到了第一页就没办法了
            while(result.Rows.Count == 0&&thisGridParma.PageIndex > 0)
            {
                thisGridParma.PageIndex--;
                _UserDetail_View_ViewControlYF.PageIndex--;
                _UserDetail_View_ViewControlYF.PageIndex--;
                result = QueryData(_UserDetail_View_ViewControlYF,ref _dataCount);
            }

ViewControlYF.DataSource = result;
            
            //以下为列统计
            

            //添加格式化字符串
                    ViewControlYF.SetFormatString("a$Role_Name",_UserDetail_View_ViewControlYF.ViewFieldFormatDict["a$Role_Name"]);    
                    ViewControlYF.SetFormatString("a$Role_Comment",_UserDetail_View_ViewControlYF.ViewFieldFormatDict["a$Role_Comment"]);    

ViewControlYF.DataBind();            
        }
        private DataTable QueryData(ViewQueryBase viewQuery,ref int DataCount)
        {
                
                int parentId = -1;
                string parentKeyName = string.Empty;
                if(thisGridParma.QueryParmaList.Count > 0)
                {
                    parentId = Convert.ToInt32(thisGridParma.QueryParmaList[0].Value);
                    parentKeyName = thisGridParma.QueryParmaList[0].QueryField.FieldName;
                }
                if(!string.IsNullOrEmpty(parentKeyName))
                {
                    var subQuery = viewQuery.GetMMSubQuery("T_User_Role", "Role_ID", parentKeyName, parentId);
                    viewQuery.ViewQueryExs.Add(
                        new ViewQueryEx()
                        {
                            TableAlias = "a",
                            FieldName = "Role_ID",
                            SubQuery  = subQuery,
                            CompareType = CompareTypeEnum.IN,
                        }
                    );
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
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowOperationJK_Click(object sender, EventArgs e)
        {
            try
            {
                string __RedirectURL = string.Empty;
                ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
                string id = (sender as LinkButton).CommandArgument;
                var entityModel = (this.Page as BasePage).GenericHelper.FindById<global::Drision.Framework.Entity.T_Role>(id);
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
                                
                                (this.Page as BasePage).GenericHelper.MMDelete(typeof(global::Drision.Framework.Entity.T_Role),Request["id"].ToInt(),"T_User",id.ToInt());
                                
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
                        LoadData();
            }
            catch (Exception ex)
            {
               DrisionLog.Add(ex);
               (this.Page as BasePage).AjaxAlert(ex,"EnableButton();");
            }
        }
        
    }
}