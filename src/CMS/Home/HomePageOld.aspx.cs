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

namespace Drision.Framework.Web.Home
{
    public partial class HomePageOld : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.PageIndex = this.grid.PagerSettings.PageIndex;
                    this.PageSize = this.grid.PagerSettings.PageSize;                    
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message);
            }
        }

        public int PageIndex
        {
            get { return GetViewStateAsInt("PageIndex").Value; }
            set { VS<int>("PageIndex", value); }
        }

        public int PageSize
        {
            get { return GetViewStateAsInt("PageSize").Value; }
            set { VS<int>("PageSize", value); }
        }

        protected void grid_PageChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex = e.PageIndex;
            this.PageSize = e.PageSize;           
        }
        
    }
}
