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
using Drision.Framework.Plugin.Web;
using System.Text;
using System.Data;

namespace Drision.Framework.Web
{
    public partial class T_Configuration_viewcontrolSB : System.Web.UI.UserControl,IUserControl
    {
        private GridParma _thisGridParma = null;
        private GridParma thisGridParma
        {
            get
            {
                if(_thisGridParma == null)
                {
                    IEnumerable<GridParma> GridParmaList = (this.Page as BasePage).GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrolSB");
                    if (GridParmaList.Count() == 0)
                    {
                        _thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "viewcontrolSB", QueryParmaList = new List<QueryParma>(),};
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
                return null;
            }
        }
        public GridControl gcList
        {
            get
            {
                return null;
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
                PagerControl1.PageIndex = 0;
                thisGridParma.PageSize = PagerControl1.PageSize;
            }
            LoadData();
        }
        /// <summary>
        /// 分页点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PagerControl1_PageIndexChanged(object sender, PageChangeEventArgs e)
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
        /// <summary>
        /// 绑定视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            GridControl gc = e.Item.FindControl("viewcontrolSB") as GridControl;
            if (gc != null)
            {
                DataTable result = DataBinder.Eval(e.Item.DataItem, "GridViewSource") as DataTable;
                
                gc.DataSource = result;
                //为了获取格式化字符串，只好在这新建了一个查询类
                var _T_Configuration_Query_View_viewcontrolSB = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("T_Configuration_Query_View_viewcontrolSB");
                    gc.SetFormatString("Configuration_Title",_T_Configuration_Query_View_viewcontrolSB.ViewFieldFormatDict["Configuration_Title"]); 
                    gc.SetFormatString("Configuration_Key",_T_Configuration_Query_View_viewcontrolSB.ViewFieldFormatDict["Configuration_Key"]); 
                    gc.SetFormatString("Configuration_Value",_T_Configuration_Query_View_viewcontrolSB.ViewFieldFormatDict["Configuration_Value"]); 
                    gc.SetFormatString("Configuration_Description",_T_Configuration_Query_View_viewcontrolSB.ViewFieldFormatDict["Configuration_Description"]); 
                    gc.SetFormatString("Sort_Order",_T_Configuration_Query_View_viewcontrolSB.ViewFieldFormatDict["Sort_Order"]); 
                    gc.SetFormatString("Configuration_Group_Id_V",_T_Configuration_Query_View_viewcontrolSB.ViewFieldFormatDict["Configuration_Group_Id"]); 
                gc.DataBind();
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
                PagerControl1.PageIndex = thisGridParma.PageIndex;
            }
            thisGridParma.PageSize = PagerControl1.PageSize;
            var _T_Configuration_Query_View_viewcontrolSB = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("T_Configuration_Query_View_viewcontrolSB");
            _T_Configuration_Query_View_viewcontrolSB.PageSize = thisGridParma.PageSize;
            _T_Configuration_Query_View_viewcontrolSB.PageIndex = thisGridParma.PageIndex;

            //2013-11-8 V3.0 先按分组字段排序后查询数据
            _T_Configuration_Query_View_viewcontrolSB.OrderTableAlias = "a";
            _T_Configuration_Query_View_viewcontrolSB.OrderFieldName = "Configuration_Group_Id";
            _T_Configuration_Query_View_viewcontrolSB.OrderMethod = Drision.Framework.LiteQueryDef.Internal.OrderMethod.ASC; 

            //赋条件
            
            //2013-11-6 V3.0 修改
            (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_T_Configuration_Query_View_viewcontrolSB, thisGridParma);


            var _dataCount = 0;
            var result = QueryData(_T_Configuration_Query_View_viewcontrolSB,ref _dataCount);
            PagerControl1.DataCount = _dataCount;

            //处理是否显示空数据和页脚
            divFooter.Visible = _dataCount != 0;
            divEmptyData.Visible = _dataCount == 0;

            //如果当前页无数据，则跳到上一页重查，直到有数据或跳到了第一页就没办法了
            while(result.Rows.Count == 0&&thisGridParma.PageIndex > 0)
            {
                thisGridParma.PageIndex--;
                PagerControl1.PageIndex--;
                _T_Configuration_Query_View_viewcontrolSB.PageIndex--;
                result = QueryData(_T_Configuration_Query_View_viewcontrolSB,ref _dataCount);
            }
            //处理数据源
            List<EntityGroupViewSource> source = new List<EntityGroupViewSource>();
            result.Rows.Cast<DataRow>().ToList().ForEach(m => {
                var p = new DataWrapper(m);
                var GroupSource = source.FirstOrDefault(q => q.GroupFieldName == p["Configuration_Group_Id_V"]);
                //处理数据源，将多引用的转换过来
                if (GroupSource == null)
                {
                    GroupSource = new EntityGroupViewSource() 
                    {
                        GroupFieldName = p["Configuration_Group_Id_V"], 
                            GroupFieldValue = p["Configuration_Group_Id_V"], 
                        GridViewSource = m.Table.Clone(), 
                    };
                    source.Add(GroupSource);
                }
                
                
                 GroupSource.GridViewSource.ImportRow(m);
            });

            Repeater1.DataSource = source;
            Repeater1.DataBind();
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
    }
}