using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Drision.Framework.Manager.Ioc;
using System.IO;
using Drision.Framework.Manager;
using System.Web.UI.WebControls;
using Drision.Framework.WebControls.Super;
using Drision.Framework.Entity;

namespace Drision.Framework.Web
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Log4Net 日志配置文件处理办法 读取数据库中的配置写入根目录
            //if (File.Exists(Server.MapPath("~/log4net.xml")))
            //{
            //    using (Stream configStream = new StreamReader(Server.MapPath("~/log4net.xml")).BaseStream)
            //    {
            //        log4net.Config.XmlConfigurator.Configure(configStream);
            //    }
            //}

            //初始化对象容器
            UnityObjectFactory ObjectFactory = new UnityObjectFactory();

            EntityCache cache = EntityCache.New();

            //ScheduleTimerManager stm = ScheduleTimerManager.CreateInstance();
            //stm.Start();

            //额外的一些处理，跟在原UploadControl的内部处理之后            
            UploadControl.OnProcessRequest = new Action<HttpContext>(p => AttachHandler.ProcessRequest(p));
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["ModuleId"] = 1;
            Session["FunctionId"] = 1;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}