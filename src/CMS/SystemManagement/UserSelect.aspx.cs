using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Entity;
using Drision.Framework.LiteQueryDef.Internal;
using Drision.Framework.Manager;
using System.Data;
using Drision.Framework.Common;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class UserSelect : BasePage
    {
        public int PageSize
        {
            get { return VS<int>("PageSize"); }
            set { VS<int>("PageSize", value); }
        }

        public int PageIndex
        {
            get { return VS<int>("PageIndex"); }
            set { VS<int>("PageSize", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageIndex = this.gridUser.PagerSettings.PageIndex;
                this.PageSize = this.gridUser.PagerSettings.PageSize;
                BindDept();
                BindGrid();
            }
        }

        private void BindDept()
        {
            this.txtDeptId.DataSource = this.DataHelper.FetchAll<T_Department>();
            this.txtDeptId.DataTextField = "Department_Name";
            this.txtDeptId.DataValueField = "Department_ID";
            this.txtDeptId.DataBind();
        }

        private void BindGrid()
        {
            try
            {
                UserViewQuery query = new UserViewQuery(this.DataHelper, this.LoginUser);

                query.UserName = this.txtUserName.Text.Trim();
                query.UserCode = this.txtUserCode.Text.Trim();
                query.DepartmentId = this.txtDeptId.SelectedValue.ToIntNullable();

                query.PageIndex = this.PageIndex;
                query.PageSize = this.PageSize;

                var dt = query.Query();
                this.gridUser.DataSource = dt;
                this.gridUser.DataBind();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnClearCondition_Click(object sender, EventArgs e)
        {            
            this.txtDeptId.SelectedValue = null;
            this.txtUserCode.Text = null;
            this.txtUserName.Text = null;
            BindGrid();
        }

        protected void gridUser_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageSize = e.PageSize;
            this.PageIndex = e.PageIndex;

            BindGrid();
        }
    }

    public class UserViewQuery : ViewQueryBase
    {
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public int? DepartmentId { get; set; }

        public UserViewQuery(BizDataContext context, T_User currentUser)
            : base(context, currentUser)
        {
            this.KeyName = "User_ID";
            this.EntityId = 1000000002;

            DefaultOperationManager manager = new DefaultOperationManager(context, currentUser);
            this.MaxQueryPrivilege = manager.TryCanOperation(currentUser.User_ID, this.EntityId, EntityOperationEnum.Query);
        }

        public override DataTable Query()
        {
            var q = LiteQuery.New("T_User", "a");

            q.SelectFields.Add(LiteField.NewEntityField("a", "User_ID"));
            q.SelectFields.Add(LiteField.NewEntityField("a", "User_Name"));
            q.SelectFields.Add(LiteField.NewEntityField("a", "User_Code"));
            q.SelectFields.Add(LiteField.NewEntityField("a", "Department_ID"));            
            q.SelectFields.Add(LiteField.NewEntityField("a", "User_Mobile"));

            var subQ = GetRefSubQuery("T_Department", "Department_ID", "Department_Name", "a", "Department_ID");
            q.SelectFields.Add(LiteField.NewSubQueryField(subQ,"Department_ID_V"));

            q.Filter = LiteFilter.True();

            if (!string.IsNullOrEmpty(this.UserName))
            {
                var fUserName = FilterField.New("a", "User_Name").Contains(this.UserName);
                q.Filter = q.Filter.And(fUserName);
            }
            if (!string.IsNullOrEmpty(this.UserCode))
            {
                var fUserCode = FilterField.New("a", "User_Code").Contains(this.UserCode);
                q.Filter = q.Filter.And(fUserCode);
            }
            if (this.DepartmentId != null)
            {
                var fDeptId = FilterField.New("a", "Department_ID").Equal(this.DepartmentId.Value);
                q.Filter = q.Filter.And(fDeptId);
            }

            AddFilterForPrevilege(q);
            AddFilterForShared(q);            

            q.SkipNumber = this.PageIndex * this.PageSize;
            q.TakeNumber = this.PageSize;

            return this.DataHelper.ExecuteLiteQuery(q);
        }

        public override int QueryCount()
        {
            throw new NotImplementedException();
        }
    }
}