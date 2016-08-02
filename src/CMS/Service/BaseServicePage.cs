using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drision.Framework.Web.Common;
using Drision.Framework.Manager.Ioc;
using Drision.Framework.Repository.Schemas;
using Drision.Framework.Manager;
using System.Web.Script.Serialization;
using Drision.Framework.Common;

namespace Drision.Framework.Web.Service
{
    public class BaseServicePage : BasePage
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        protected int Id { get { return GetIntParma("id"); } }

        protected string EntityName { get { return GetStringParma("____entityName"); } }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowError(string msg)
        {
            Response.Write("{\"Success\":false,\"Message\":\"" + msg + "\"}");
            Response.End();
        }

        /// <summary>
        /// 获取int型参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected int GetIntParma(string name)
        {
            string val = GetStringParma(name);
            int i;
            if (!int.TryParse(val, out i))
            {
                ShowError(string.Format("获取参数 name：{0} , value：{1} ，转换int 失败", name, val));
            }
            return i;
        }

        protected void Update()
        {
            List<string> fields = new List<string>();
            dynamic entity = CollectEntity(fields);
            this.GenericHelper.Update(entity);
        }

        internal object Add()
        {
            var es = GetEs(EntityName);
            List<string> fields = new List<string>();
            dynamic entity = CollectEntity(fields);

            this.GenericHelper.Save(entity);            

            return es.GetKeyValue(entity);
        }


        private object CollectEntity(List<string> fields)
        {
            var es = GetEs(EntityName);
            object entity = es.CreateInstance();
            foreach (string key in Request.Params.Keys)
            {
                string p = key.Replace("entity[", "").Replace("]", "");
                if (es.PropertyTypes.ContainsKey(p))
                {
                    fields.Add(p);
                    string value = Request.Params[key];
                    if (value.ToUpper() == "NULL")
                    {
                        value = null;
                    }
                    entity.SetPropertyConvertValue(p,value);
                }
            }
            return entity;
        }

        /// <summary>
        /// 获取string参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetStringParma(string name)
        {
            string val = Request.Params[name];

            if (string.IsNullOrEmpty(val))
            {
                ShowError(string.Format("请提供参数 {0}", name));
            }
            return val;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected object Get(int id, string name)
        {
            //return (new OrgManager(this.DataHelper)).GetEntity(name, id); 
            var es = IEntitySchemaHelper.Get(name);
            return this.GenericHelper.FindById(es.EntityType, id);
        }

        /// <summary>
        /// 单实体转json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string ToJson(object obj)
        {            

            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;

            return js.Serialize(obj);

            //IEntitySchema es = GetEs(obj.GetType().Name);
            //List<string> pvalues = new List<string>();
            //foreach (var item in es.PropertyTypes)
            //{
            //    string value = Convert.ToString(obj.GetPropertyValue(item.Key));
            //    value = value.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

            //    pvalues.Add(string.Format(@"""{0}"":""{1}""", item.Key, value));
            //}

            //return "{" + string.Join(",", pvalues) + "}";
        }

        protected class JsonResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public dynamic Result { get; set; }
        }

        protected void WriteJson(JsonResult obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            string str = js.Serialize(obj);
            Response.Write(str);

            //string s = string.Format(@"""Success"":{0},""Message"":""{1}"",""Result"":{2}", obj.Success ? "true" : "false", obj.Message, obj.Result);
            //Response.Write("{" + s + "}");
            //Response.End();
        }

        protected IEntitySchema GetEs(string name)
        {
            IEntitySchema es = IEntitySchemaHelper.Get(name);
            return es;
        }

                


        private string DealException(Exception ex)
        {
            this.Application["StackTrace"] = ex.StackTrace;
            this.Application["MessageError"] = ex.Message;
            this.Application["SourceError"] = ex.Source;
            this.Application["TargetSite"] = ex.TargetSite.ToString();
            string error = string.Format("URl：{0}\n引发异常的方法：{1}\n错误信息：{2}\n错误堆栈：{3}\n",
                this.Request.RawUrl, ex.TargetSite, ex.Message, ex.StackTrace);
            return error;
        }


        protected override void OnError(EventArgs e)
        {
            base.OnError(e);
            Exception ex = this.Server.GetLastError();

            if (ex != null)
            {
                //  string error = this.DealException(ex);
                string error = ex.Message;
                this.Server.ClearError();
                ShowError(error);
            }
        }

        internal void Delete(int id, string entityName)
        {
            IEntitySchema es = IEntitySchemaHelper.Get(entityName);
            object entity = es.CreateInstance();
            entity.SetPropertyValue(es.KeyName, id);
            this.GenericHelper.Delete(entity);
        }
    }
}