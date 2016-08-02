using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drision.Framework.Common;

namespace Drision.Framework.Web.FormWorkflow
{
    /// <summary>
    /// ConvertPinYin 的摘要说明
    /// </summary>
    public class ConvertPinYin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string str = context.Request.Form["Value"];
            string result = str.ToPingYin();
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}