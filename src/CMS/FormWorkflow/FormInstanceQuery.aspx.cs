using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.Workflow;
using System.Linq.Expressions;


namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormInstanceQuery : BasePage
    {
        #region 属性

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

        #endregion

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

        /// <summary>
        /// 初始化
        /// </summary>
        protected void Initialize()
        {
            this.ccState.DataSource = typeof(FormInstanceState);
            this.ccState.DataBind();

            var formList = this.DataHelper.Set<SysForm>().Where(p => p.State == (int)FormState.StartUsed).ToList();
            this.ccForm.DataSource = formList;
            this.ccForm.DataBind();

            this.ccForm.SetValue(null);
            this.ccState.SetValue(null);
            this.txtCode.SetValue(null);
            
            this.PageSize = this.gcFormInstance.PagerSettings.PageSize;
            this.PageIndex = this.gcFormInstance.PagerSettings.PageIndex;

            this.SortDirection = (int)System.Web.UI.WebControls.SortDirection.Descending;
            this.SortField = "CreateTime";

            BindGrid();
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void BindGrid()
        {
            //查询列
            var source = from p in this.DataHelper.Set<SysFormInstance>()
                         join f in this.DataHelper.Set<SysForm>()
                         on p.FormId equals f.FormId                         
                         select new
                         {
                             p.FormInstanceId,
                             f.FormId,
                             f.FormName,
                             p.FormTitle,
                             p.FormCode,
                             p.FormDate,
                             p.CreateTime,
                             FormInstanceState = EnumHelper.GetDescription((FormInstanceState)p.State),
                             p.State,
                             CanEdit = p.State != (int)FormInstanceState.Approving,
                         };

            //条件
            long? formId = this.ccForm.SelectedValue.ToLongNullable();
            int? state = this.ccState.SelectedValue.ToIntNullable();
            string code = this.txtCode.Text.Trim();

            if (formId != null)
            {
                source = source.Where(p => p.FormId == formId);
            }
            if (state != null)
            {
                source = source.Where(p => p.State == state);
            }
            if (!string.IsNullOrWhiteSpace(code))
            {
                source = source.Where(p => p.FormCode.Contains(code));
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
            this.gcFormInstance.PagerSettings.DataCount = totalCount;

            if (totalCount > this.PageSize * this.PageIndex)
            {
                this.gcFormInstance.DataSource = source.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
            }
            else
            {
                this.PageIndex = 0;
                this.gcFormInstance.PagerSettings.PageIndex = 0;
                this.gcFormInstance.DataSource = source.Take(this.PageSize).ToList();
            }
            this.gcFormInstance.DataBind();
        }

        /// <summary>
        /// 排序Lambda
        /// </summary>        
        protected LambdaExpression CreateOrderFunc(dynamic source, string fieldName)
        {
            Type type = source.GetType().GetGenericArguments()[0];
            ParameterExpression p = Expression.Parameter(type, "p");
            MemberExpression member = Expression.Property(p, fieldName);
            LambdaExpression lambda = Expression.Lambda(member, p);
            return lambda;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gcFormInstance_PageIndexChanging(object sender, GridPostBackEventArgs e)
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

        /// <summary>
        /// 点击表头排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gcFormInstance_HeaderClick(object sender, GridPostBackEventArgs e)
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