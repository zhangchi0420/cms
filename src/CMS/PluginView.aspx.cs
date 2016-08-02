using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Plugin;
using Drision.Framework.Plugin.Aop;
using Drision.Framework.Plugin.Web;

namespace Drision.Framework.Web
{
    public partial class PluginView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Repeater1.DataSource = PagePluginFactory.pluginHandlers;
            this.Repeater1.DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem == null) return;
            if (e.Item.DataItem.GetType() != typeof(KeyValuePair<string, PagePluginHandler>)) return;

            var labelPageName = e.Item.FindControl("labelPageName") as Label;
            var labelTypeName = e.Item.FindControl("labelTypeName") as Label;

            //foreach(var a in PagePluginFactory.pluginHandlers)
            //{
                
                
            //}

            var pageHandler = (KeyValuePair<string, PagePluginHandler>)e.Item.DataItem;
            labelPageName.Text = pageHandler.Key;
            labelTypeName.Text = pageHandler.Value.pluginType.ToString();


            var Repeater2 = e.Item.FindControl("Repeater2") as Repeater;
            var cmd_plugin_list = new List<System.Web.UI.Triplet>();
            foreach (var cmd_handler in pageHandler.Value.CommandHandlers)
            {
                foreach (var cmd_plugin in cmd_handler.Value.beforeActions)
                {
                    System.Web.UI.Triplet triplet = new Triplet(
                        cmd_handler.Key, cmd_plugin.pluginAttribute.InvokeType, cmd_plugin.methodInfo);
                    cmd_plugin_list.Add(triplet);
                }
                foreach (var pi in cmd_handler.Value.afterActions)
                {
                    System.Web.UI.Triplet triplet = new Triplet(
                        cmd_handler.Key, pi.pluginAttribute.InvokeType, pi.methodInfo);
                    cmd_plugin_list.Add(triplet);
                }
            }
            Repeater2.DataSource = cmd_plugin_list;
            Repeater2.DataBind();

        }


        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem == null) return;
            if (e.Item.DataItem.GetType() != typeof(Triplet)) return;


            var labelButtonName = e.Item.FindControl("labelButtonName") as Label;
            var labelInvokeType = e.Item.FindControl("labelInvokeType") as Label;
            var labelMethod = e.Item.FindControl("labelMethod") as Label;

            Triplet t = (Triplet)e.Item.DataItem;

            labelButtonName.Text = t.First.ToString();
            labelInvokeType.Text = t.Second.ToString();
            labelMethod.Text = t.Third.ToString();

        }

    }
}