using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Entity;
using System.Linq.Expressions;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormQuery : BasePage
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
            this.ddlFormState.DataSource = typeof(FormState);
            this.ddlFormState.DataBind();
            
            this.txtFormName.SetValue(null);
            this.ddlFormState.SetValue(null);

            this.PageSize = this.gcForm.PagerSettings.PageSize;
            this.PageIndex = this.gcForm.PagerSettings.PageIndex;

            this.SortDirection = (int)System.Web.UI.WebControls.SortDirection.Ascending;
            this.SortField = "FormName";

            BindGrid();
        }
        

        private string GetDisplayText(SysForm p)
        {
            SysEntity entity = GetEntity(p.EntityId);
            if (entity != null)
            {
                return entity.DisplayText;
            }
            return string.Empty;
        }

        private string GetEntityName(SysForm p)
        {
            SysEntity entity = GetEntity(p.EntityId);
            if (entity != null)
            {
                return entity.EntityName;
            }
            return string.Empty;
        }

        private string GetUserName(int? userId)
        {
            T_User user = this.DataHelper.FindById<T_User>(userId);
            if (user != null)
            {
                return user.User_Name;
            }
            return string.Empty;
        }            

        protected void BindGrid()
        {
            //查询列
            var source = from p in this.DataHelper.Set<SysForm>()
                         join u in this.DataHelper.Set<T_User>()
                         on p.CreateUserId equals u.User_ID
                         select new
                         {
                             p.FormId,
                             p.FormName,
                             p.State,
                             FormState = EnumHelper.GetDescription((FormState)p.State),                             
                             p.CreateTime,
                             p.EntityId,                             
                             EntityName = GetEntityName(p),
                             DisplayText = GetDisplayText(p),
                             p.CreateUserId,
                             CreateUser = u.User_Name,
                             IsStartUsed = p.State == (int)FormState.StartUsed,
                         };

            //条件
            string formName = this.txtFormName.Text.Trim();
            int? formState = this.ddlFormState.SelectedValue.ToIntNullable();
            if (!string.IsNullOrEmpty(formName))
            {
                source = source.Where(p => p.FormName.Contains(formName));
            }
            if (formState != null)
            {
                source = source.Where(p => p.State == formState);
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
            this.gcForm.PagerSettings.DataCount = totalCount;            

            if (totalCount > this.PageSize * this.PageIndex)
            {
                this.gcForm.DataSource = source.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
            }
            else
            {
                this.PageIndex = 0;
                this.gcForm.PagerSettings.PageIndex = 0;
                this.gcForm.DataSource = source.Take(this.PageSize).ToList();
            }            
            this.gcForm.DataBind();
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

        protected void gcForm_PageIndexChanging(object sender, GridPostBackEventArgs e)
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

        protected void gcForm_HeaderClick(object sender, GridPostBackEventArgs e)
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

        protected void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                long? formId = (sender as LinkButton).CommandArgument.ToLongNullable();
                if (formId != null)
                {
                    SysForm form = this.DataHelper.FindById<SysForm>(formId);
                    if (form != null)
                    {
                        form.State = (int)FormState.StopUsed;
                        form.UpdateTime = DateTime.Now;
                        form.UpdateUserId = this.LoginUserID;

                        this.DataHelper.UpdatePartial(form, p => new { p.State, p.UpdateUserId, p.UpdateTime });

                        BindGrid();
                    }
                    else
                    {
                        throw new Exception("表单不存在");
                    }
                }
                else
                {
                    throw new Exception("表单不存在");
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}