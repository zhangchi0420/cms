using System;
using System.Text;
using Drision.Framework.Enum;
using Drision.Framework.Entity;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
using Drision.Framework.Plugin;
using Drision.Framework.Plugin.Aop;
using Drision.Framework.Plugin.Web;
using Drision.Framework.Manager;
using Drision.Framework.PageInterface;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace Drision.Framework.Web
{
    [PagePlugin("T_Article_Edit", false)]
    public class T_Article_Edit_Plugin : BasePagePlugin<IPage_T_Article_Edit>
    {
        public override void AfterOnLoad()
        {
            HiddenField hf_txtarea = this.Page.GetControlById<HiddenField>("hf_EditorValue");
            HtmlTextArea txtarea = this.Page.GetControlById<HtmlTextArea>("editor");
            if (!Page.IsPostBack)
            {
                int id = 0;
                if (Request["id"] != null)
                {
                    Int32.TryParse(Request["id"].ToString(), out id);
                }
                T_Article model = this.Page.GenericHelper.FindById<T_Article>(id);
                if (model != null)
                {
                    if (hf_txtarea != null && txtarea != null)
                    {
                        txtarea.Value = model.Content;
                        hf_txtarea.Value = model.Content;
                    }
                }
            }

        }
    }
}
