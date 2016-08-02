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
    public partial class ConfigurationQuery : BasePage
    {
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
                if (!IsPostBack)
                {
                    this.PageSize = this.gcConfiguration.PagerSettings.PageSize;
                    this.PageIndex = this.gcConfiguration.PagerSettings.PageIndex;
                    BindGrid();
                }
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
        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            try
            {
                // using (BizDataContext context = new BizDataContext())
                {
                    var query = DataHelper.FetchAll<T_Configuration>().AsQueryable();
                    if (!string.IsNullOrEmpty(tbT_Configuration_Title.Text))
                    {
                        query = query.Where(p => p.Configuration_Title.Contains(tbT_Configuration_Title.Text.Trim()));
                    }
                    if(!string.IsNullOrEmpty(tbT_Configuration_Key.Text))
                    {
                        query = query.Where(p => p.Configuration_Key.Contains(tbT_Configuration_Key.Text.Trim()));
                    }
                    var result = query.OrderByDescending(p => p.Last_Modified).OrderByDescending(p=>p.Last_Modified).ToList();
                    this.gcConfiguration.PagerSettings.DataCount = result.Count();
                    if (result.Count() > this.PageIndex * this.PageSize)
                    {
                        result = result.Skip(this.PageIndex * this.PageSize).Take(this.PageSize).ToList();
                    }
                    else
                    {
                        this.PageIndex = 0;
                        this.gcConfiguration.PagerSettings.PageIndex = 0;

                        result = result.Take(this.PageSize).ToList();
                    }
                    gcConfiguration.DataSource = result;
                    gcConfiguration.DataBind();
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }
        protected void btnClearCondition_Click(object sender, EventArgs e)
        {
            try
            {
                tbT_Configuration_Key.Text = string.Empty;
                tbT_Configuration_Title.Text = string.Empty;
                btnQuery_Click(null, null);
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }
    }
}