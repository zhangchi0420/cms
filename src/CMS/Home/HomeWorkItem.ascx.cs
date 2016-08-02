using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;

using Drision.Framework.Manager;
using Drision.Framework.OrgLibrary;
using Drision.Framework.OrgLibrary.InternalEntities;

namespace Drision.Framework.Web.Home
{
    public partial class HomeWorkItem : System.Web.UI.UserControl
    {
        public int LoginUserID
        {
            get
            {
                return (this.Page as BasePage).LoginUserID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            BindWorkItem();
            BindRemind();
        }

        public int WorkItemCount { get; set; }
        public int RemindCount { get; set; }

        private void BindWorkItem()
        {
            using (BizDataContext context = new BizDataContext())
            {
                var source = context
                    .Set<T_WorkItemBase>().Where(p => p.State == (int)WorkItemStatus.Created 
                        && p.OwnerId == this.LoginUserID)
                    .OrderByDescending(p => p.CreateTime);

                var list = source.Take(6).ToList().Select(p => new
                {
                    p.WorkItemId,
                    CreateTime = GetDate(p.CreateTime),
                    DisplayText = GetString(p.Title,35),
                    WorkItemURL = p.CompletePageUrl,
                    EndTime = GetDate(p.EndTime),
                }).ToList();

                this.WorkItemCount = source.Count();

                this.rWorkItem.DataSource = list;
                this.rWorkItem.DataBind();
            }
        }

        private string GetDate(DateTime? datetime)
        {
            if (datetime != null)
            {
                return datetime.Value.ToString("MM/dd");
            }
            return null;
        }


        /// <summary>
        /// 新，站内信
        /// </summary>
        private void BindRemind()
        {
            using (BizDataContext context = new BizDataContext())
            {
                //查询SysInternalMailReceiver表中UserId为当前登录人，并且State为1-已接收
                //并且关联的SysInternalMailSender表的State不为0-草稿的记录
                //并且未读
                var list = from mailReceiver in context.Set<SysInternalMailReceiver>()
                           join mailSender in context.Set<SysInternalMailSender>()
                           on mailReceiver.SenderId equals mailSender.SenderId
                           join mail in context.Set<SysInternalMail>()
                           on mailSender.MailId equals mail.MailId
                           where mailReceiver.UserId == this.LoginUserID && mailReceiver.State == (int)InternalMailReceiverState.IsReceived
                           && mailSender.State != (int)InternalMailSenderState.Draft
                           && mailReceiver.IsRead != true
                           orderby mailReceiver.ReceiveTime descending
                           select new
                           {
                               mailReceiver.ReceiverId,
                               mailReceiver.ReceiveTime,
                               mail.Subject,
                               mailSender.UserId,                               
                               mailReceiver.IsRead,
                           };
                var result = list.Take(6).ToList();
                var source = result.Select(item => new
                {
                    ReceiverId = item.ReceiverId,
                    Subject = GetString(item.Subject, 35),
                    Url = string.Format("../InternalMail/InternalMailDetail.aspx?type=receiver&id={0}",item.ReceiverId),                   
                    ReceiveTime = GetDate(item.ReceiveTime),
                }).ToList();


                this.RemindCount = list.Count();
                this.rRemind.DataSource = source;
                this.rRemind.DataBind();
            }
        }

        /// <summary>
        /// 旧的提醒，已抛弃
        /// </summary>
        private void BindRemind_Old()
        {
            using (BizDataContext context = new BizDataContext())
            {
                var query = context.Set<SysRemind>().Where(item => item.State == (int)RemindStausEnum.New
                            && item.OwnerId == this.LoginUserID).OrderByDescending(x => x.CreateTime);

                var result = query.Take(6).ToList();
                var source = result.Select(item => new
                            {
                                item.RemindId,
                                RemindName = GetString(item.RemindName,35),
                                item.RemindURL,
                                OwnerUserName = GetUserName(context,item.OwnerId),
                                item.CreateTime,
                                item.DeadLine,
                            }).ToList();

                
                this.RemindCount = query.Count();
                this.rRemind.DataSource = source.Select(item => new
                {
                    item.RemindId,
                    item.RemindName,
                    RemindUrl = GetRemindUrl(item.RemindURL, item.RemindId),
                    item.OwnerUserName,
                    CreateTime = GetDate(item.CreateTime),
                    item.DeadLine,
                }).ToList();
                this.rRemind.DataBind();
            }
        }

        private string GetUserName(BizDataContext context,int? id)
        {
            string result = null;
            if (id != null)
            {
                var user = context.FindById<T_User>(id);
                if (user != null)
                {
                    result = user.User_Name;
                }
            }
            return result;
        }

        private string GetString(string str, int length)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length <= length)
            {
                return str;
            }
            return str.Substring(0, length) + "...";
        }

        private string GetRemindUrl(string source, int id)
        {
            string s = source;
            string result = CreateUrl(s, id);
            if (string.IsNullOrEmpty(result))
            {
                result = string.Format("../SystemManagement/SysRemindDetail.aspx?id={0}", id);
            }
            return result;
        }

        private string CreateUrl(string source, int id)
        {
            string result = source;
            if (!string.IsNullOrEmpty(source))
            {
                source = this.ResolveClientUrl(source);
                if (source.Contains("?"))
                {
                    source += string.Format("&remindId={0}", id);
                }
                else
                {
                    source += string.Format("?remindId={0}", id);
                }
                result = source;
            }
            return result;
        }
    }
}