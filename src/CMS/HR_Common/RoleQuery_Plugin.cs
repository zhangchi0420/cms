using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using Drision.Framework.PageInterface;
using Drision.Framework.Plugin.Aop;
using Drision.Framework.Plugin.Web;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.HR_Common
{
    [PagePlugin("RoleQuery", false)]
    public class RoleQuery_Plugin : BasePagePlugin<IPage_RoleQuery>
    {
        [CommandPlugin("lbtn32", InvokeType.Before)]
        public void lbtn8B_Click_BeforePlugin(object sender, PluginEventArgs e)
        {
            var model = e.entityModel as T_Role;
            if (model.Role_ID == 305589 || model.Role_ID == 305590)
            {
                throw new MessageException("系统内置角色,禁止删除！");

            }


        }

    }
}