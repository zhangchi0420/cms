using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Drision.Framework.Manager;
using System.Data.Common;
using Tension;
using Drision.Framework.Enum;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using System.Linq.Expressions;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using Drision.Framework.OrgLibrary;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class SharePrivilegeQuery : BasePage
    {
        private ISharePrivilegeHandler GetSharePrivilegeHandler(BizDataContext ctx)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings[ConstKey.SharePrivilegeInterfacePath];
            if (!string.IsNullOrWhiteSpace(path))
            {
                return CustomHandlerLoader.GetHandlerWithConfiguration<ISharePrivilegeHandler>(path, ctx);
            }
            else
            {
                return new SharePrivilegeManager(ctx);
            }
        }

        public int PageIndex
        {
            get { return VS<int>("PageIndex"); }
            set { VS<int>("PageIndex", value); }
        }

        public int PageSize
        {
            get { return VS<int>("PageSize"); }
            set { VS<int>("PageSize", value); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    LoadDropDown();
                    //空条件查询（相当于重置）



                    this.PageSize = this.gcProcessInstance.PagerSettings.PageSize;
                    this.PageIndex = this.gcProcessInstance.PagerSettings.PageIndex;
                    BindGrid();
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        void LoadDropDown()
        {
            try
            {
                this.ccPrivilege.DataSource = typeof(EntityOperationEnum);
                this.ccPrivilege.DataBind();
                this.ccPrivilege.Items.RemoveAll(p => p.Value == ((int)EntityOperationEnum.None).ToString() || p.Value == ((int)EntityOperationEnum.Add).ToString());
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        protected void grid_PageChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex = e.PageIndex;
            this.PageSize = e.PageSize;
            BindGrid();
        }

        private void BindGrid()
        {
            try
            {
                

                    var list = this.DataHelper.Where<SysSharedPrivilege>(p => p.OwnerId == LoginUserID).AsQueryable();
                    if (ccPrivilege.SelectedValue != null)
                    {
                        int stateId = Int32.Parse(ccPrivilege.SelectedValue);
                        list = list.Where(p => p.Privilege == stateId);
                    }
                    if (dtStartTime1.Text != "")
                    {
                        DateTime st1 = DateTime.Parse(dtStartTime1.Text);
                        list = list.Where(p => p.CreateTime > st1);
                    }
                    if (dtStartTime2.Text != "")
                    {
                        DateTime st2 = DateTime.Parse(dtStartTime2.Text);
                        list = list.Where(p => p.CreateTime < st2);
                    }

                    var result = list.OrderByDescending(p => p.CreateTime).ToList();
                    this.gcProcessInstance.PagerSettings.DataCount = result.Count();
                    //绑定
                    if (result.Count() > this.PageIndex * this.PageSize)
                    {
                        result = result.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
                    }
                    else
                    {
                        this.PageIndex = 0;
                        this.gcProcessInstance.PagerSettings.PageIndex = 0;

                        result = result.Take(this.PageSize).ToList();
                    }
                    gcProcessInstance.DataSource = result;
                    gcProcessInstance.DataBind();
                
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 清空条件
        /// </summary>
        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            try
            {
                ccPrivilege.SelectedValue = null;
                dtStartTime1.Text = "";
                dtStartTime2.Text = "";
                //查询数据
                btnQuery_Click(null, null);
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gcProcessInstance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        string entityName =DataBinder.Eval(e.Row.DataItem, "EntityName")==null?string.Empty:DataBinder.Eval(e.Row.DataItem, "EntityName").ToString();
                        int objectId =DataBinder.Eval(e.Row.DataItem, "ObjectId")==null?-1:Int32.Parse(DataBinder.Eval(e.Row.DataItem, "ObjectId").ToString());
                        int shareUserId =DataBinder.Eval(e.Row.DataItem, "ShareUserId")==null?-1:Int32.Parse(DataBinder.Eval(e.Row.DataItem, "ShareUserId").ToString());
                        int shareRoleId = DataBinder.Eval(e.Row.DataItem, "ShareRoleId") == null ? -1 : Int32.Parse(DataBinder.Eval(e.Row.DataItem, "ShareRoleId").ToString());
                        int privilege =DataBinder.Eval(e.Row.DataItem, "Privilege")==null?-1: Int32.Parse(DataBinder.Eval(e.Row.DataItem, "Privilege").ToString());
                        if (objectId > 0)
                        {
                            gcProcessInstance.SetRowText(e.Row, "ObjectId", (new OrgManager(this.DataHelper)).GetDisplayValue(entityName, objectId));
                        }
                        if (shareUserId>0)
                        {
                            gcProcessInstance.SetRowText(e.Row, "ShareUserId", (new OrgManager(this.DataHelper)).GetDisplayValue("T_User", shareUserId));
                        }
                        if (shareRoleId > 0)
                        {
                            gcProcessInstance.SetRowText(e.Row, "ShareRoleId", (new OrgManager(this.DataHelper)).GetDisplayValue("T_Role", shareRoleId));
                        }
                        if (privilege > 0)
                        {
                            gcProcessInstance.SetRowText(e.Row, "Privilege", EnumHelper.GetDescription(typeof(EntityOperationEnum), privilege));
                        }
                    }
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        /// <summary>
        /// 取消共享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (sender as LinkButton).CommandArgument.ToInt();
                ISharePrivilegeHandler _sharePrivilege = GetSharePrivilegeHandler(this.DataHelper);            
                _sharePrivilege.UnShare(id);
                BindGrid();
                this.AjaxAlert("共享已取消！");
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }
    }
}