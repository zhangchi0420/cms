using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class SubFunctionList : BasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            try
            {
                var fid = Request.Params["fid"].ToLongNullable();
                var roleIdList = this.OrgHelper.GetUserRoles(this.LoginUser.User_ID).Select(p => p.Role_ID).ToList();
                var functionList = (this.Master as Site).RoleFuncManager.GetFunctions(roleIdList).ToList();

                Dictionary<long, SysFunction> dict = new Dictionary<long, SysFunction>();
                foreach (var p in functionList)
                {
                    if (p.Permission_Type == fid && !dict.ContainsKey(p.Function_ID))
                    {
                        dict[p.Function_ID] = p;
                    }
                }
                var source = dict.Values;
                this.repeater.DataSource = source.Select(p => new
                {
                    p.Function_ID,
                    p.Permission_Name,
                    p.Permission_Type,
                    PageURL = (this.Master as Site).GetPageUrl(p),
                    p.OrderIndex,
                    p.Category,
                    p.Function_Comment,
                }).OrderBy(p => p.OrderIndex).ToList();
                this.repeater.DataBind();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}