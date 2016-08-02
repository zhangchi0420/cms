using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Entity;
using Drision.Framework.Manager;
using Drision.Framework.Web.Common;
//=================Created by zhongyan.tao at 2011-9-16 11:10.am for the first version=====================
namespace Drision.Framework.Web
{
    public partial class DownLoadPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LoginUser != null)
                {
                    FileId = int.Parse(Request.QueryString["id"]);
                    ToDownLoadFile();
                }
                else
                {
                    RedirectToLoginPage();
                }
            }
        }

        /// <summary>
        /// 文件ID
        /// </summary>
        private int FileId
        {
            get { return (int)ViewState["fileId"]; }
            set { ViewState["fileId"] = value; }
        }

        public void ToDownLoadFile()
        { 
            T_Attachment atta = this.DataHelper.FindById<T_Attachment>(FileId);

            if (atta != null)
            {
                byte[] fileByte = atta.FileData;//二进制字段内容
                string fileName = atta.DisplayName;
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));//设置编码，解决下载文件名乱码
                Response.AppendHeader("Content-Length", fileByte.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.OutputStream.Write(fileByte, 0, fileByte.Length);
                Response.End();
            }
            else
            {
                this.AjaxAlert("不存在");
            }
        }

        //protected void download_Click(object sender, EventArgs e)
        //{
        //    toDownLoadFile();
        //}

    }
}