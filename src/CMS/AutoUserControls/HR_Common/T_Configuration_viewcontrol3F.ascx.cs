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
    public partial class T_Configuration_viewcontrol3F : System.Web.UI.UserControl,IUserControl
    {
        private GridParma _thisGridParma = null;
        private GridParma thisGridParma
        {
            get
            {
                if(_thisGridParma == null)
                {
                    IEnumerable<GridParma> GridParmaList = (this.Page as BasePage).GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrol3F");
                    if (GridParmaList.Count() == 0)
                    {
                        _thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "viewcontrol3F", QueryParmaList = new List<QueryParma>(),};
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
                return viewcontrol3F.SelectedValues;
            }
        }
        public GridControl gcList
        {
            get
            {
                return viewcontrol3F;
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
                   ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.viewcontrol3F.FindControl("btnviewcontrol3FExport"));
                }
                catch (Exception ex)
                {
                    (this.Page as BasePage).AjaxAlert(ex);
                }
            }
            
        
        /// <summary>
        /// 导出
        /// </summary>
        protected void btnviewcontrol3FExport_Click(object sender, EventArgs e)
        {
            try
            {
            if(viewcontrol3F.IsEmpty == true)
            {
                (this.Page as BasePage).AjaxAlert("无数据导出！");
                return;
            }
            //汇总列
            List<ViewSumField> SumFields = null;
            
            
                var _T_Configuration_Detail_View_viewcontrol3F = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("T_Configuration_Detail_View_viewcontrol3F");                
                //赋条件
                //2013-11-6 V3.0 修改
                (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_T_Configuration_Detail_View_viewcontrol3F, thisGridParma);
                //如果有树作为条件的，也要把树条件加进来
                int DataCount = 0;
                var result = QueryData(_T_Configuration_Detail_View_viewcontrol3F,ref DataCount);
                var fields = new List<HeaderSortField>()
                {
                        new  HeaderSortField() { HeaderText = "标题", DataTextField = "a$Configuration_Title",DataFormatString = _T_Configuration_Detail_View_viewcontrol3F.ViewFieldFormatDict["a$Configuration_Title"] },
                        new  HeaderSortField() { HeaderText = "索引", DataTextField = "a$Configuration_Key",DataFormatString = _T_Configuration_Detail_View_viewcontrol3F.ViewFieldFormatDict["a$Configuration_Key"] },
                        new  HeaderSortField() { HeaderText = "数据值", DataTextField = "a$Configuration_Value",DataFormatString = _T_Configuration_Detail_View_viewcontrol3F.ViewFieldFormatDict["a$Configuration_Value"] },
                        new  HeaderSortField() { HeaderText = "描述", DataTextField = "a$Configuration_Description",DataFormatString = _T_Configuration_Detail_View_viewcontrol3F.ViewFieldFormatDict["a$Configuration_Description"] },
                };
                result.Rows.Cast<DataRow>().ToList().ToExcel(fields, "系统配置.xls",SumFields,_T_Configuration_Detail_View_viewcontrol3F.QuerySum());
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
viewcontrol3F.PagerSettings.PageIndex = 0;
            }
            thisGridParma.PageSize = viewcontrol3F.PagerSettings.PageSize;
            LoadData();            
        }
        /// <summary>
        /// 表头点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void viewcontrol3F_HeaderClick(object sender, GridPostBackEventArgs e)
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
        protected void viewcontrol3F_PageIndexChanging(object sender, GridPostBackEventArgs e)
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
        protected void viewcontrol3F_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //设置行操作的权限隐藏
                            TextControl Configuration_Value = e.Row.FindControl("Configuration_Value") as TextControl;
                    if(Configuration_Value != null)
                    {
                        Configuration_Value.SetValue(DataBinder.Eval(e.Row.DataItem, "a$Configuration_Value"));
                    }
        }
        /// <summary>
        /// 查询绑定数据
        /// </summary>
        public void LoadData()
        {
            //初始化时直接调用此方法，附上此值
            if (thisGridParma.PageIndex != 0)
            {
                viewcontrol3F.PagerSettings.PageIndex = thisGridParma.PageIndex;
            }
            thisGridParma.PageSize = viewcontrol3F.PagerSettings.PageSize;
            var _T_Configuration_Detail_View_viewcontrol3F = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("T_Configuration_Detail_View_viewcontrol3F");
            _T_Configuration_Detail_View_viewcontrol3F.PageSize = thisGridParma.PageSize;
            _T_Configuration_Detail_View_viewcontrol3F.PageIndex = thisGridParma.PageIndex;
            if(!string.IsNullOrEmpty(thisGridParma.SortFieldName))
            {
                _T_Configuration_Detail_View_viewcontrol3F.OrderTableAlias = thisGridParma.SortFieldName.Split('$')[0];
                _T_Configuration_Detail_View_viewcontrol3F.OrderFieldName = thisGridParma.SortFieldName.Split('$')[1];
                _T_Configuration_Detail_View_viewcontrol3F.OrderMethod = thisGridParma.SortDirection == "asc"?Drision.Framework.LiteQueryDef.Internal.OrderMethod.ASC:Drision.Framework.LiteQueryDef.Internal.OrderMethod.DESC;
            }
            //赋条件,如果有条件或者是作为树形两种页面有树作为条件的，都要先定义
            
            //2013-11-6 V3.0 修改
            (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_T_Configuration_Detail_View_viewcontrol3F, thisGridParma);


            //如果有树作为条件的，也要把树条件加进来

            var _dataCount = 0;
            var result = QueryData(_T_Configuration_Detail_View_viewcontrol3F,ref _dataCount);
viewcontrol3F.PagerSettings.DataCount = _dataCount;

            //如果当前页无数据，则跳到上一页重查，直到有数据或跳到了第一页就没办法了
            while(result.Rows.Count == 0&&thisGridParma.PageIndex > 0)
            {
                thisGridParma.PageIndex--;
                _T_Configuration_Detail_View_viewcontrol3F.PageIndex--;
                _T_Configuration_Detail_View_viewcontrol3F.PageIndex--;
                result = QueryData(_T_Configuration_Detail_View_viewcontrol3F,ref _dataCount);
            }

viewcontrol3F.DataSource = result;
            
            //以下为列统计
            

            //添加格式化字符串
                    viewcontrol3F.SetFormatString("a$Configuration_Title",_T_Configuration_Detail_View_viewcontrol3F.ViewFieldFormatDict["a$Configuration_Title"]);    
                    viewcontrol3F.SetFormatString("a$Configuration_Key",_T_Configuration_Detail_View_viewcontrol3F.ViewFieldFormatDict["a$Configuration_Key"]);    
                    viewcontrol3F.SetFormatString("a$Configuration_Description",_T_Configuration_Detail_View_viewcontrol3F.ViewFieldFormatDict["a$Configuration_Description"]);    

viewcontrol3F.DataBind();            
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

    }
}