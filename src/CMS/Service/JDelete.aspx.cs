using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Drision.Framework.Web.Service
{
    public partial class JDelete : BaseServicePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            JsonResult jr = new JsonResult() { Success = true, Message = "删除成功"};
            try
            {
                base.Delete(Id, EntityName);
            }
            catch (Exception ex)
            {
                jr.Success = false;
                jr.Message = ex.Message;
            }
            WriteJson(jr);
        }
    }
}