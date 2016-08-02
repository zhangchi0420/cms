using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using System.Configuration;
using System.Data;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class ChangeHistory : BasePage
    {
        /// <summary>
        /// 实体名称
        /// </summary>
        private string EntityName
        {
            get 
            {
                return Convert.ToString(ViewState["EntityName"]);
            }
            set 
            {
                ViewState["EntityName"] = value;
            }
        }
        /// <summary>
        /// 实体主键
        /// </summary>
        private int ObjectId
        {
            get
            {
                return Convert.ToInt32(ViewState["ObjectId"]);
            }
            set
            {
                ViewState["ObjectId"] = value;
            }
        }
        /// <summary>
        /// 返回路径
        /// </summary>
        private string ReturnURL
        {
            get
            {
                return Convert.ToString(ViewState["ReturnURL"]);
            }
            set
            {
                ViewState["ReturnURL"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.EntityName = Request.QueryString["EntityName"];
                    if (string.IsNullOrEmpty(this.EntityName))
                    {
                        throw new Exception("必须传入正确的实体名EntityName");
                    }
                    this.ObjectId = Convert.ToInt32(Request.QueryString["ObjectId"]);
                    if (ObjectId <= 0)
                    {
                        throw new Exception("必须传入正确的实体主键ObjectId");
                    }
                    if (this.Request.UrlReferrer != null)
                    {
                        this.ReturnURL = this.Request.UrlReferrer.PathAndQuery;
                    }
                    DbHelperSQL.connectionString = ConfigurationManager.ConnectionStrings["meta_biz"].ConnectionString;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                AjaxAlert(ex);
            }
        }
        /// <summary>
        /// 从哪来回哪去
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.ReturnURL);
        }
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        private void LoadData()
        {
            string strSQL = string.Format(@"Select a.LogId,a.ChangeTime,b.User_Name as ChangeUser
                                            From SysChangeLog a,T_User b
                                            Where a.ChangeUserId = b.User_ID
                                              And a.EntityName = '{0}'
                                              And a.ObjectId = {1} order by a.ChangeTime desc", this.EntityName, this.ObjectId);
            var Result = DbHelperSQL.Query(strSQL);
            this.rpLog.DataSource = Result;
            this.rpLog.DataBind();
        }

        private string GetChangeFieldDisplayText(SysChangeLog log, SysChangeLogDetail logDetail)
        {
            string result = string.Empty;
            SysEntity entity = this.EntityCache.SysEntity.FirstOrDefault(p => p.EntityName == log.EntityName);
            if (entity != null)
            {
                SysField field = entity.Fields.FirstOrDefault(p => p.FieldName == logDetail.FieldName);
                if (field != null)
                {
                    result = field.DisplayText;
                }
            }
            return result;
        }

        /// <summary>
        /// 绑定下一层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpLog_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpLogDetail = e.Item.FindControl("rpLogDetail") as Repeater;
            int LogId = Convert.ToInt32((e.Item.DataItem as DataRowView)["LogId"]);

            
            var source = from logDetail in this.DataHelper.Set<SysChangeLogDetail>()
                         join log in this.DataHelper.Set<SysChangeLog>()
                         on logDetail.LogId equals log.LogId
                         where log.LogId == LogId
                         select new
                         {
                             logDetail.DetailId,
                             logDetail.OriginalValue,
                             logDetail.CurrentValue,
                             DisplayText = GetChangeFieldDisplayText(log,logDetail),
                         };



//            string strSQL = string.Format(@"Select a.DetailId,d.DisplayText,a.OriginalValue,a.CurrentValue
//                                            From SysChangeLogDetail a,SysChangeLog b,SysEntity c,SysField d
//                                            Where a.LogId = b.LogId
//                                              And b.EntityName = c.EntityName
//                                              And c.EntityId = d.EntityId
//                                              And a.FieldName = d.FieldName
//                                              And a.LogId = {0}", LogId);
//            var result = DbHelperSQL.Query(strSQL);

            var result = source.ToList();
            rpLogDetail.DataSource = result;
            rpLogDetail.DataBind();
        }        
    }
}