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
    [PagePlugin("T_Product_Add", false)]
    public class T_Product_Add_Plugin : BasePagePlugin<IPage_T_Product_Add>
    {
        public override void AfterOnLoad()
        {
            HiddenField hf_txtarea = this.Page.GetControlById<HiddenField>("hf_EditorValue");
            HtmlTextArea txtarea = this.Page.GetControlById<HtmlTextArea>("editor");
            if (hf_txtarea != null && txtarea != null)
            {
                if (!string.IsNullOrEmpty(hf_txtarea.Value))
                {
                    txtarea.Value = hf_txtarea.Value;
                }
            }

        }

        /// <summary>
        /// 保存-之前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [CommandPlugin("btnSave", InvokeType.Before)]
        public void btnSave_Before(object sender, PluginEventArgs e)
        {
            var model = e.entityModel as T_Article;
            if (model != null)
            {
                model.Content = GetEditorValue(this.Page);
            }
        }

        /// <summary>
        /// 获取html编辑器的值
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private string GetEditorValue(BasePage page)
        {
            string res = string.Empty;
            HtmlTextArea txtarea = page.GetControlById<HtmlTextArea>("editor");
            if (txtarea != null)
            {
                res = txtarea.Value;
            }
            return res;
        }
    }
}
