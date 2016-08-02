using System;
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
using Drision.Framework.Plugin.Web;
using Drision.Framework.PageInterface;
using Drision.Framework.WebControls.Super;

namespace Drision.Framework.Web
{
    public partial class T_Department_Select : BasePage,IPage_T_Department_Select
    {        
        //实现接口
        

        public override long PageID
        {
            get
            {
                return 1000144024;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Department_Select";
            }
        }
        //页面定义的参数
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //赋值定义的参数
                    
                    //设置按钮的权限隐藏
                    
                    //加载树
                    LoadFirstLevel(tree.Nodes,false);
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        
         
         
         private void LoadFirstLevel(List<TNode> Nodes,bool IsRefresh = true)
{
    Nodes.Clear();

    var _T_Department_View_OnDepartment = this.ViewQueryHelper.GetViewQuery("T_Department_View_OnDepartment");
    _T_Department_View_OnDepartment.ViewQueryExs.Add(new ViewQueryEx(){ TableAlias = "a",FieldName = "Parent_DepartmentID",CompareType = CompareTypeEnum.IsNull});
    var result = _T_Department_View_OnDepartment.Query().Rows.Cast<System.Data.DataRow>().ToList();
    result.ToList().ForEach(m => {
            var p = new DataWrapper(m);
            TNode NewNode = new TNode()
            {
                Text = p["a$Department_Status_V"]+"-"+p["a$Department_Full_Code"]+"-"+p["a$Parent_DepartmentID_V"]+"-"+p["a$Department_Name"],
                Value = p["Department_ID"],
            };
            Nodes.Add(NewNode);
            LoadTree(NewNode.ChildNodes,Convert.ToInt32(p["Department_ID"]),IsRefresh);
        });
    //2013-7-10 zhumin 树控件已经改写,Refresh已经不需要
    //if(IsRefresh == true)
    //{
tree.Refresh();
    //}
}



private void LoadTree(List<TNode> Nodes,int ParentId,bool IsRefresh = true)
{
    Nodes.Clear();

    var _T_Department_View_OnDepartment = this.ViewQueryHelper.GetViewQuery("T_Department_View_OnDepartment");
    _T_Department_View_OnDepartment.ViewQueryExs.Add(new ViewQueryEx(){ TableAlias = "a",FieldName = "Parent_DepartmentID",CompareType = CompareTypeEnum.Equal,ConditionValue = ParentId.ToString()});
    var result = _T_Department_View_OnDepartment.Query().Rows.Cast<System.Data.DataRow>().ToList();
    if(result.Count > 0)
    {
        result.ForEach(m => {
                var p = new DataWrapper(m);
                TNode NewNode = new TNode()
                {
                    Text = p["a$Department_Status_V"]+"-"+p["a$Department_Full_Code"]+"-"+p["a$Parent_DepartmentID_V"]+"-"+p["a$Department_Name"],
                    Value = p["Department_ID"],
                };
                Nodes.Add(NewNode);
                    LoadTree(NewNode.ChildNodes,Convert.ToInt32(p["Department_ID"]),IsRefresh);
            });
    }
    //2013-7-10 zhumin 树控件已经改写,Refresh已经不需要
    //if(IsRefresh == true)
    //{
tree.Refresh();
    //}
}
        /// <summary>
        /// 节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tree_NodeClick(object sender, TNodeEventArgs e)
        {
        }
        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tree_AjaxLoading(object sender, TreeLoadingEventArgs e)
        {
            try
            {
                var entityModel = this.GenericHelper.FindById<T_Department>(e.Value);
                LoadTree(e.Result,Convert.ToInt32(e.Value));
            }
            catch { }
        }

    }
}