using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Drision.Framework.Web
{
    /// <summary>
    /// UploadControl 的摘要说明
    /// </summary>
    public class UploadControl1 : IHttpHandler,IRequiresSessionState
    {
        private System.Web.UI.WebControls.UploadControl uploader = new System.Web.UI.WebControls.UploadControl();

        public void ProcessRequest(HttpContext context)
        {
            uploader.ProcessRequest(context);
        }

        public bool IsReusable
        {
            get
            {
                return uploader.IsReusable;
            }
        }
    }
}