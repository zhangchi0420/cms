using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Entity;
using Drision.Framework.Enum;
using System.Linq.Expressions;
using Drision.Framework.Common;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class AddressBook : BasePage
    {
        protected int PageSize
        {
            get { return VS<int>("PageSize"); }
            set { VS<int>("PageSize", value); }
        }

        protected int PageIndex
        {
            get { return VS<int>("PageIndex"); }
            set { VS<int>("PageIndex", value); }
        }

        protected string SortField
        {
            get { return VS<string>("SortField"); }
            set { VS<string>("SortField", value); }
        }

        protected int SortDirection
        {
            get { return VS<int>("SortDeirection"); }
            set { VS<int>("SortDeirection", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void Initialize()
        {
            this.ddlDepartment.DataSource = this.DataHelper.FetchAll<T_Department>();
            this.ddlDepartment.DataBind();

            this.txtUserName.SetValue(null);
            this.ddlDepartment.SetValue(null);

            this.PageSize = this.gcUser.PagerSettings.PageSize;
            this.PageIndex = this.gcUser.PagerSettings.PageIndex;

            this.SortDirection = (int)System.Web.UI.WebControls.SortDirection.Ascending;
            this.SortField = "User_Name";

            BindGrid();
        }

        protected void BindGrid()
        {
            //查询列
            var source = from u in this.DataHelper.Set<T_User>()
                         join p in this.DataHelper.Set<T_Department>()
                         on u.Department_ID equals p.Department_ID  
                         into dept
                         from d in dept.DefaultIfEmpty()                                               
                         select new
                         {
                             d.Department_Name,
                             u.User_Code,
                             u.User_Name,
                             u.User_Mobile,
                             u.User_EMail,
                             u.State,
                             u.Department_ID,
                         };

            //条件
            string userName = this.txtUserName.Text.Trim();
            int? deptId = this.ddlDepartment.SelectedValue.ToIntNullable();
            if (!string.IsNullOrEmpty(userName))
            {
                source = source.Where(p => p.User_Name.Contains(userName));
            }
            if (deptId != null)
            {
                source = source.Where(p => p.Department_ID == deptId.Value);
            }

            //排序            
            dynamic orderFunc = CreateOrderFunc(source, this.SortField);
            if (!string.IsNullOrEmpty(this.SortField))
            {
                if (this.SortDirection == (int)System.Web.UI.WebControls.SortDirection.Ascending)
                {
                    source = Queryable.OrderBy(source, orderFunc);
                }
                else
                {
                    source = Queryable.OrderByDescending(source, orderFunc);
                }
            }

            //分页
            int totalCount = source.Count();
            this.gcUser.PagerSettings.DataCount = totalCount;

            if (totalCount > this.PageSize * this.PageIndex)
            {
                this.gcUser.DataSource = source.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
            }
            else
            {
                this.PageIndex = 0;
                this.gcUser.PagerSettings.PageIndex = 0;
                this.gcUser.DataSource = source.Take(this.PageSize).ToList();
            }
            this.gcUser.DataBind();
        }

        protected LambdaExpression CreateOrderFunc(dynamic source, string fieldName)
        {
            Type type = source.GetType().GetGenericArguments()[0];
            ParameterExpression p = Expression.Parameter(type, "p");
            MemberExpression member = Expression.Property(p, fieldName);
            LambdaExpression lambda = Expression.Lambda(member, p);
            return lambda;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void gcUser_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            try
            {
                this.PageIndex = e.PageIndex;
                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void gcUser_HeaderClick(object sender, GridPostBackEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Parameter))
                {
                    this.SortField = e.Parameter;
                }
                else
                {
                    this.SortField = e.FieldName;
                }
                this.SortDirection = (int)e.Direction;

                BindGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}