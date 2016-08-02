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
using Tension;
using Drision.Framework.PageInterface;
using Drision.Framework.WebControls.Super;

namespace Drision.Framework.Web
{
    public partial class T_Department_Detail : BasePage,IPage_T_Department_Detail
    {    
    
        //实现接口
        
    
        public override long PageID
        {
            get
            {
                return 1000143652;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Department_Detail";
            }
        }
        private int __Id
        {
            get
            {
                return Convert.ToInt32(ViewState["__Id"]);
            }
            set
            {
                ViewState["__Id"] = value;
            }
        }
        private bool __IsAdd
        {
            get
            {
                if(ViewState["__IsAdd"] == null)
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean(ViewState["__IsAdd"]);
                }
            }
            set
            {
                ViewState["__IsAdd"] = value;
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
                    
                    //绑定启动流程下拉选择框
                    
                     //绑定枚举 
                    
                    //绑定引用字段下拉 
                    
                    //设置按钮的权限隐藏
                    
                    //加载树
                    LoadFirstLevel(tree.Nodes,false);
                    __IsAdd = false;
                    if (!string.IsNullOrEmpty(this.Request.Params["id"]))
                    {
                        __Id = int.Parse(this.Request.Params["id"]);
                        tree.SelectedValue = __Id.ToString();
                    }
                    else
                    {
                        __Id = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
		    if (!this.IsPostBack)
            {
                BindDetail();
		    }
        }
         
         
         
         
         
         private void LoadFirstLevel(List<TNode> Nodes,bool IsRefresh = true)
{
    Nodes.Clear();

    var _T_Department_View_AllDeptTree = this.ViewQueryHelper.GetViewQuery("T_Department_View_AllDeptTree");
    _T_Department_View_AllDeptTree.ViewQueryExs.Add(new ViewQueryEx(){ TableAlias = "a",FieldName = "Parent_DepartmentID",CompareType = CompareTypeEnum.IsNull});
    var result = _T_Department_View_AllDeptTree.Query().Rows.Cast<System.Data.DataRow>().ToList();
    result.ToList().ForEach(m => {
            var p = new DataWrapper(m);
            TNode NewNode = new TNode()
            {
                Text = p["a$Department_Name"]+"-"+p["a$Department_Full_Code"],
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

    var _T_Department_View_AllDeptTree = this.ViewQueryHelper.GetViewQuery("T_Department_View_AllDeptTree");
    _T_Department_View_AllDeptTree.ViewQueryExs.Add(new ViewQueryEx(){ TableAlias = "a",FieldName = "Parent_DepartmentID",CompareType = CompareTypeEnum.Equal,ConditionValue = ParentId.ToString()});
    var result = _T_Department_View_AllDeptTree.Query().Rows.Cast<System.Data.DataRow>().ToList();
    if(result.Count > 0)
    {
        result.ForEach(m => {
                var p = new DataWrapper(m);
                TNode NewNode = new TNode()
                {
                    Text = p["a$Department_Name"]+"-"+p["a$Department_Full_Code"],
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
        
        protected void BindDetail()
        {
            var entityModel = this.GenericHelper.FindById<T_Department>(__Id);
            
        }
        
        
        /// <summary>
        /// 节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tree_NodeClick(object sender, TNodeEventArgs e)
        {         
            __IsAdd = false;
            __Id = Convert.ToInt32(e.Node.Value);
            BindDetail();
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
                LoadTree(e.Result,Convert.ToInt32(e.Value),false);
            }
            catch { }
        }
        private void CreateModel(global::Drision.Framework.Entity.T_Department entityModel)
{
    
        
        if(__Id > 0)
        {
            if(__IsAdd)
            {
                //这个外键不好找啊
                entityModel.Parent_DepartmentID = __Id;
            }
            else
            {
                entityModel.Department_ID = __Id;
            }
        }
        
    
}
    }
}