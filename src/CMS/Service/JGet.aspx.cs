using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Manager.Ioc;
using Drision.Framework.Entity;
using Drision.Framework.Common;

namespace Drision.Framework.Web.Service
{
    public partial class JGet : BaseServicePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            object result = Get(Id, EntityName);


            JsonResult jr = new JsonResult() { Success = true };
            if (result == null)
            {
                jr.Success = false;
                jr.Message = string.Format("未获取到 {0} ,id {1}", EntityName, Id);
                //jr.Result = "{}";
            }
            else
            {
                //jr.Result = ToJson(result);
                jr.Result = result;
            }
            WriteJson(jr);

        }
    }
}