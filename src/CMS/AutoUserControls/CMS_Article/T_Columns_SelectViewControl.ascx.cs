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
    public partial class T_Columns_SelectViewControl : System.Web.UI.UserControl,IUserControl
    {
        private GridParma _thisGridParma = null;
        private GridParma thisGridParma
        {
            get
            {
                if(_thisGridParma == null)
                {
                    IEnumerable<GridParma> GridParmaList = (this.Page as BasePage).GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "SelectViewControl");
                    if (GridParmaList.Count() == 0)
                    {
                        _thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "SelectViewControl", QueryParmaList = new List<QueryParma>(),};
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
                return SelectViewControl.SelectedValues;
            }
        }
        public GridControl gcList
        {
            get
            {
                return SelectViewControl;
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
                   ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.SelectViewControl.FindControl("btnSelectViewControlExport"));
                }
                catch (Exception ex)
                {
                    (this.Page as BasePage).AjaxAlert(ex);
                }
            }
            
        
        /// <summary>
        /// 导出
        /// </summary>
        protected void btnSelectViewControlExport_Click(object sender, EventArgs e)
        {
            try
            {
            if(SelectViewControl.IsEmpty == true)
            {
                (this.Page as BasePage).AjaxAlert("无数据导出！");
                return;
            }
            //汇总列
            List<ViewSumField> SumFields = null;
            
            
                var _T_Columns_Select_View_SelectViewControl = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("T_Columns_Select_View_SelectViewControl");                
                //赋条件
                //2013-11-6 V3.0 修改
                (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_T_Columns_Select_View_SelectViewControl, thisGridParma);
                //如果有树作为条件的，也要把树条件加进来
                int DataCount = 0;
                var result = QueryData(_T_Columns_Select_View_SelectViewControl,ref DataCount);
                var fields = new List<HeaderSortField>()
                {
                        new  HeaderSortField() { HeaderText = "菜单Key", DataTextField = "MenuKey",DataFormatString = _T_Columns_Select_View_SelectViewControl.ViewFieldFormatDict["MenuKey"] },
                        new  HeaderSortField() { HeaderText = "栏目名称", DataTextField = "a$Columns_Name",DataFormatString = _T_Columns_Select_View_SelectViewControl.ViewFieldFormatDict["a$Columns_Name"] },
                };
                result.Rows.Cast<DataRow>().ToList().ToExcel(fields, "选择视图.xls",SumFields,_T_Columns_Select_View_SelectViewControl.QuerySum());
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
SelectViewControl.PagerSettings.PageIndex = 0;
            }
            thisGridParma.PageSize = SelectViewControl.PagerSettings.PageSize;
            LoadData();            
        }
        /// <summary>
        /// 表头点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectViewControl_HeaderClick(object sender, GridPostBackEventArgs e)
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
        protected void SelectViewControl_PageIndexChanging(object sender, GridPostBackEventArgs e)
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
        protected void SelectViewControl_RowDataBound(object sender, GridViewRowEventArgs e)
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
                SelectViewControl.PagerSettings.PageIndex = thisGridParma.PageIndex;
            }
            thisGridParma.PageSize = SelectViewControl.PagerSettings.PageSize;
            var _T_Columns_Select_View_SelectViewControl = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("T_Columns_Select_View_SelectViewControl");
            _T_Columns_Select_View_SelectViewControl.PageSize = thisGridParma.PageSize;
            _T_Columns_Select_View_SelectViewControl.PageIndex = thisGridParma.PageIndex;
            if(!string.IsNullOrEmpty(thisGridParma.SortFieldName))
            {
                _T_Columns_Select_View_SelectViewControl.OrderTableAlias = thisGridParma.SortFieldName.Split('$')[0];
                _T_Columns_Select_View_SelectViewControl.OrderFieldName = thisGridParma.SortFieldName.Split('$')[1];
                _T_Columns_Select_View_SelectViewControl.OrderMethod = thisGridParma.SortDirection == "asc"?Drision.Framework.LiteQueryDef.Internal.OrderMethod.ASC:Drision.Framework.LiteQueryDef.Internal.OrderMethod.DESC;
            }
            //赋条件,如果有条件或者是作为树形两种页面有树作为条件的，都要先定义
            
            //2013-11-6 V3.0 修改
            (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_T_Columns_Select_View_SelectViewControl, thisGridParma);


            //如果有树作为条件的，也要把树条件加进来

            var _dataCount = 0;
            var result = QueryData(_T_Columns_Select_View_SelectViewControl,ref _dataCount);
SelectViewControl.PagerSettings.DataCount = _dataCount;

            //如果当前页无数据，则跳到上一页重查，直到有数据或跳到了第一页就没办法了
            while(result.Rows.Count == 0&&thisGridParma.PageIndex > 0)
            {
                thisGridParma.PageIndex--;
                _T_Columns_Select_View_SelectViewControl.PageIndex--;
                _T_Columns_Select_View_SelectViewControl.PageIndex--;
                result = QueryData(_T_Columns_Select_View_SelectViewControl,ref _dataCount);
            }

SelectViewControl.DataSource = result;
            
            //以下为列统计
            

            //添加格式化字符串
                    SelectViewControl.SetFormatString("MenuKey",_T_Columns_Select_View_SelectViewControl.ViewFieldFormatDict["MenuKey"]);    
                    SelectViewControl.SetFormatString("a$Columns_Name",_T_Columns_Select_View_SelectViewControl.ViewFieldFormatDict["a$Columns_Name"]);    

SelectViewControl.DataBind();            
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