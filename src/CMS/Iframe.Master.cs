using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Drision.Framework.Web
{
    public partial class Iframe : MasterPage
    {
        /// <summary>
        /// 页面背景色样式
        /// </summary>
        public string LinkTemplateStyle
        {
            get { return Session["LinkTemplate"] == null ? "/template_apple/template.css" : Session["LinkTemplate"] as string; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.linkTemplate.Href = this.LinkTemplateStyle;
        }
    }
}