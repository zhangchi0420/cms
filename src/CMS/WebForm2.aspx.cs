using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Common;
using Drision.Framework.Common.Workflow;
using System.Text;

using Drision.Framework.Enum;
using Drision.Framework.Entity;

namespace Drision.Framework.Web
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Initialize(this);
            if (!IsPostBack)
            {
                
            }
        }

        protected void btnConfirmServer_Click(object sender, EventArgs e)
        {
            MessageBox.Confirm(this, "确认删除吗？", "alert('已删除！');");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            MessageBox.Alert(this, "已删除！");
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            MessageBox.Alert(this, "弹出消息...");
        }        
    }
}