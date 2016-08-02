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
    public partial class T_User_viewcontrolWG : System.Web.UI.UserControl,IUserControl
    {
        private GridParma _thisGridParma = null;
        private GridParma thisGridParma
        {
            get
            {
                if(_thisGridParma == null)
                {
                    IEnumerable<GridParma> GridParmaList = (this.Page as BasePage).GridParmaList.Where(p => p.PageName == this.Request.Url.AbsolutePath && p.GridName == "viewcontrolWG");
                    if (GridParmaList.Count() == 0)
                    {
                        _thisGridParma = new GridParma() { PageName = this.Request.Url.AbsolutePath, GridName = "viewcontrolWG", QueryParmaList = new List<QueryParma>(),};
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
                return viewcontrolWG.SelectedValues;
            }
        }
        public GridControl gcList
        {
            get
            {
                return viewcontrolWG;
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
                   ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.viewcontrolWG.FindControl("btnviewcontrolWGExport"));
                }
                catch (Exception ex)
                {
                    (this.Page as BasePage).AjaxAlert(ex);
                }
            }
            
        
        /// <summary>
        /// 导出
        /// </summary>
        protected void btnviewcontrolWGExport_Click(object sender, EventArgs e)
        {
            try
            {
            if(viewcontrolWG.IsEmpty == true)
            {
                (this.Page as BasePage).AjaxAlert("无数据导出！");
                return;
            }
            //汇总列
            List<ViewSumField> SumFields = null;
            
            
                var _RoleEdit_View_viewcontrolWG = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("RoleEdit_View_viewcontrolWG");                
                //赋条件
                //2013-11-6 V3.0 修改
                (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_RoleEdit_View_viewcontrolWG, thisGridParma);
                //如果有树作为条件的，也要把树条件加进来
                int DataCount = 0;
                var result = QueryData(_RoleEdit_View_viewcontrolWG,ref DataCount);
                var fields = new List<HeaderSortField>()
                {
                        new  HeaderSortField() { HeaderText = "工号", DataTextField = "a$User_Code",DataFormatString = _RoleEdit_View_viewcontrolWG.ViewFieldFormatDict["a$User_Code"] },
                        new  HeaderSortField() { HeaderText = "用户姓名", DataTextField = "a$User_Name",DataFormatString = _RoleEdit_View_viewcontrolWG.ViewFieldFormatDict["a$User_Name"] },
                        new  HeaderSortField() { HeaderText = "所属部门", DataTextField = "a$Department_ID_V",DataFormatString = _RoleEdit_View_viewcontrolWG.ViewFieldFormatDict["a$Department_ID"] },
                        new  HeaderSortField() { HeaderText = "手机号码", DataTextField = "a$User_Mobile",DataFormatString = _RoleEdit_View_viewcontrolWG.ViewFieldFormatDict["a$User_Mobile"] },
                };
                result.Rows.Cast<DataRow>().ToList().ToExcel(fields, "用户列表.xls",SumFields,_RoleEdit_View_viewcontrolWG.QuerySum());
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
viewcontrolWG.PagerSettings.PageIndex = 0;
            }
            thisGridParma.PageSize = viewcontrolWG.PagerSettings.PageSize;
            LoadData();            
        }
        /// <summary>
        /// 表头点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void viewcontrolWG_HeaderClick(object sender, GridPostBackEventArgs e)
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
        protected void viewcontrolWG_PageIndexChanging(object sender, GridPostBackEventArgs e)
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
        protected void viewcontrolWG_RowDataBound(object sender, GridViewRowEventArgs e)
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
                viewcontrolWG.PagerSettings.PageIndex = thisGridParma.PageIndex;
            }
            thisGridParma.PageSize = viewcontrolWG.PagerSettings.PageSize;
            var _RoleEdit_View_viewcontrolWG = (this.Page as BasePage).ViewQueryHelper.GetViewControlQuery("RoleEdit_View_viewcontrolWG");
            _RoleEdit_View_viewcontrolWG.PageSize = thisGridParma.PageSize;
            _RoleEdit_View_viewcontrolWG.PageIndex = thisGridParma.PageIndex;
            if(!string.IsNullOrEmpty(thisGridParma.SortFieldName))
            {
                _RoleEdit_View_viewcontrolWG.OrderTableAlias = thisGridParma.SortFieldName.Split('$')[0];
                _RoleEdit_View_viewcontrolWG.OrderFieldName = thisGridParma.SortFieldName.Split('$')[1];
                _RoleEdit_View_viewcontrolWG.OrderMethod = thisGridParma.SortDirection == "asc"?Drision.Framework.LiteQueryDef.Internal.OrderMethod.ASC:Drision.Framework.LiteQueryDef.Internal.OrderMethod.DESC;
            }
            //赋条件,如果有条件或者是作为树形两种页面有树作为条件的，都要先定义
            
            //2013-11-6 V3.0 修改
            (this.Page as BasePage).SetQueryCondition_FromGridParmaToViewQuery(_RoleEdit_View_viewcontrolWG, thisGridParma);


            //如果有树作为条件的，也要把树条件加进来

            var _dataCount = 0;
            var result = QueryData(_RoleEdit_View_viewcontrolWG,ref _dataCount);
viewcontrolWG.PagerSettings.DataCount = _dataCount;

            //如果当前页无数据，则跳到上一页重查，直到有数据或跳到了第一页就没办法了
            while(result.Rows.Count == 0&&thisGridParma.PageIndex > 0)
            {
                thisGridParma.PageIndex--;
                _RoleEdit_View_viewcontrolWG.PageIndex--;
                _RoleEdit_View_viewcontrolWG.PageIndex--;
                result = QueryData(_RoleEdit_View_viewcontrolWG,ref _dataCount);
            }

viewcontrolWG.DataSource = result;
            
            //以下为列统计
            

            //添加格式化字符串
                    viewcontrolWG.SetFormatString("a$User_Code",_RoleEdit_View_viewcontrolWG.ViewFieldFormatDict["a$User_Code"]);    
                    viewcontrolWG.SetFormatString("a$User_Name",_RoleEdit_View_viewcontrolWG.ViewFieldFormatDict["a$User_Name"]);    
                    viewcontrolWG.SetFormatString("a$Department_ID_V",_RoleEdit_View_viewcontrolWG.ViewFieldFormatDict["a$Department_ID"]);    
                    viewcontrolWG.SetFormatString("a$User_Mobile",_RoleEdit_View_viewcontrolWG.ViewFieldFormatDict["a$User_Mobile"]);    

viewcontrolWG.DataBind();            
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
                    var subQuery = viewQuery.GetMMSubQuery("T_User_Role", "User_ID", parentKeyName, parentId);
                    viewQuery.ViewQueryExs.Add(
                        new ViewQueryEx()
                        {
                            TableAlias = "a",
                            FieldName = "User_ID",
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

    }
}