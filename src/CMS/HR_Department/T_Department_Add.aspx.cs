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
    public partial class T_Department_Add : BasePage,IPage_T_Department_Add
    {    
    
        //实现接口
        
                Dropdown IPage_T_Department_Add.ddlManager_IDNM{get { return this.ddlManager_IDNM; }} 
                TextControl IPage_T_Department_Add.txt_Department_NameMZ{get { return this.txt_Department_NameMZ; }} 
                ComboControl IPage_T_Department_Add.txt_Department_StatusBE{get { return this.txt_Department_StatusBE; }} 
                TextControl IPage_T_Department_Add.txt_RemarkZQ{get { return this.txt_RemarkZQ; }} 
                TextControl IPage_T_Department_Add.txt_Department_Full_Code8J{get { return this.txt_Department_Full_Code8J; }} 
                TextControl IPage_T_Department_Add.txt_Organization_EnCodeP3{get { return this.txt_Organization_EnCodeP3; }} 
                TextControl IPage_T_Department_Add.txt_Organization_Code2S{get { return this.txt_Organization_Code2S; }} 
    
        public override long PageID
        {
            get
            {
                return 1000118507;
            }
        }  
        public override string PageName
        {
            get
            {
                return "T_Department_Add";
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
                            txt_Department_StatusBE.DataSource = typeof(global::Drision.Framework.Enum.StopFlagEnum);
        txt_Department_StatusBE.DataBind();

                    //绑定引用字段下拉 
                                dynamic result1 = null;
            result1 = this.DataHelper.FetchAll<T_User>();
            
            foreach(var m in result1)
            {
                var p = new DataWrapper(m);
ddlManager_IDNM.Items.Add(new ComboItem() 
                { 
                    Text = p["User_Name"], 
                    Value = p["User_ID"], 
                });
            }
            

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

/// <summary>
/// 新增
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnAdd_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        ddlManager_IDNM.SetValue(null);
        txt_Department_NameMZ.SetValue(null);
        txt_Department_StatusBE.SetValue(null);
        txt_RemarkZQ.SetValue(null);
        txt_Department_Full_Code8J.SetValue(null);
        txt_Organization_EnCodeP3.SetValue(null);
        txt_Organization_Code2S.SetValue(null);
                __IsAdd = true;
    }
    catch (Exception ex)
    {
        this.AjaxAlert(ex,"EnableButton();");
    }
}/// <summary>
/// 保存按钮添加验证
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnSave_PreRender(object sender, EventArgs e)
{
    
    if (!IsPostBack)
    {
        if(!string.IsNullOrEmpty(this.btnSave.ValidationGroup))
        {
            this.btnSave.Attributes.Add("onclick", "return ValidateGroup('" + this.btnSave.ValidationGroup + "');");
        }
        else
        {
            this.btnSave.Attributes.Add("onclick", "return Validate();");
        }
    }
    
}


/// <summary>
/// 保存
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnDeptTreeSave_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
        global::Drision.Framework.Entity.T_Department entityModel = null;
            if(!__IsAdd)
            {
            entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Department>(__Id);
            if (entityModel == null)
            {
                throw new Exception("保存前请先选择节点然后点击“新增”按钮");
            }
            }
            else
            {
            entityModel = new global::Drision.Framework.Entity.T_Department();
            }
        CreateModel(entityModel);
        PluginEventArgs __PluginEventArg = new PluginEventArgs() { entityModel = entityModel, RedirectURL = __RedirectURL, CurrentUserID = this.LoginUserID };
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
                    if(__IsAdd)
                    {
                      this.GenericHelper.Save(entityModel);
                    }
                    else
                    {
                      this.GenericHelper.Update(entityModel);
                      tree.SelectedNode.Text = entityModel.Department_Name;
                    }
                __Id = entityModel.Department_ID;
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
                    this.AjaxAlert(__ButtonOperationContext.AlertMessage,"EnableButton();");
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
        
            LoadFirstLevel(tree.Nodes,true);
            tree.SelectedValue = __Id.ToString();
            __IsAdd = false;
            this.AjaxAlert("保存成功！", "EnableButton();");
     }
     catch (Exception ex)
     {
        DrisionLog.Add(ex);
        this.AjaxAlert(ex,"EnableButton();");
     }
}


/// <summary>
/// 配置岗位
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnCustom6M_Click(object sender, EventArgs e)
{
    try
    {
        string __RedirectURL = string.Empty;
        ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
        var entityModel = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Department>(Request.QueryString["id"]??"-1");
        PluginEventArgs __PluginEventArg = new PluginEventArgs() { entityModel = entityModel, RedirectURL = __RedirectURL, CurrentUserID = this.LoginUserID };
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
                    this.AjaxAlert(__ButtonOperationContext.AlertMessage,"EnableButton();");
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
     }
     catch (Exception ex)
     {
        DrisionLog.Add(ex);
        this.AjaxAlert(ex,"EnableButton();");
     }
}
/// <summary>
/// 删除
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnDeleteH6_Click(object sender, EventArgs e)
{
    try
    {
        ButtonOperationContext __ButtonOperationContext = new ButtonOperationContext();
            
            if(__Id != 0)
            {
                PluginEventArgs __PluginEventArg = new PluginEventArgs() { entityModel = null, RedirectURL = string.Empty, CurrentUserID = this.LoginUserID };
                {

                    #region 按钮前插件
                    if(!__ButtonOperationContext.IsEnd)
                    {
                        try
                        {
                            PagePluginFactory.InvokeBeforePlugins(sender,__PluginEventArg);
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
                        var item = this.GenericHelper.FindById<global::Drision.Framework.Entity.T_Department>(__Id);
                        if(item != null)
                        {
                            this.GenericHelper.Delete(item);
                        }
                    }
                    #endregion

                    #region 按钮后插件
                    if(!__ButtonOperationContext.IsEnd)
                    {
                        try
                        {
                            PagePluginFactory.InvokeAfterPlugins(sender,__PluginEventArg);
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
                            this.AjaxAlert(__ButtonOperationContext.AlertMessage,"EnableButton();");
                            return;
                        }
                    }
                    #endregion
                }
tree.SelectedValue= null;
tree.RemoveNode(__Id.ToString());
                __Id = 0;
tree.Refresh();
                        ddlManager_IDNM.SetValue(null);
        txt_Department_NameMZ.SetValue(null);
        txt_Department_StatusBE.SetValue(null);
        txt_RemarkZQ.SetValue(null);
        txt_Department_Full_Code8J.SetValue(null);
        txt_Organization_EnCodeP3.SetValue(null);
        txt_Organization_Code2S.SetValue(null);

                this.AjaxAlert("删除成功！");
            }
             
     }
     catch (Exception ex)
     {
        DrisionLog.Add(ex);
        this.AjaxAlert(ex,"EnableButton();");
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
                    
                                        try
                                        {
                                            ddlManager_IDNM.SetValue(entityModel.Manager_ID);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_Department_NameMZ.SetValue(entityModel.Department_Name);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_Department_StatusBE.SetValue(entityModel.Department_Status);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_RemarkZQ.SetValue(entityModel.Remark);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_Department_Full_Code8J.SetValue(entityModel.Department_Full_Code);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_Organization_EnCodeP3.SetValue(entityModel.Organization_EnCode);
                                        }
                                        catch(Exception){}
                                        
        
                                        try
                                        {
                                            txt_Organization_Code2S.SetValue(entityModel.Organization_Code);
                                        }
                                        catch(Exception){}
                                        

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
        
                    entityModel.SetPropertyConvertValue("Manager_ID",ddlManager_IDNM.GetValue());
                entityModel.SetPropertyConvertValue("Department_Name",txt_Department_NameMZ.GetValue());
                entityModel.SetPropertyConvertValue("Department_Status",txt_Department_StatusBE.GetValue());
                entityModel.SetPropertyConvertValue("Remark",txt_RemarkZQ.GetValue());
                entityModel.SetPropertyConvertValue("Department_Full_Code",txt_Department_Full_Code8J.GetValue());
                entityModel.SetPropertyConvertValue("Organization_EnCode",txt_Organization_EnCodeP3.GetValue());
                entityModel.SetPropertyConvertValue("Organization_Code",txt_Organization_Code2S.GetValue());

}
    }
}