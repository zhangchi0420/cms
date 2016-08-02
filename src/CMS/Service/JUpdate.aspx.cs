using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Drision.Framework.Web.Service
{
    public partial class JUpdate : BaseServicePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            JsonResult jr = new JsonResult() { Success = true, Message = "更新成功" };
            try
            {
                base.Update();
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