using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Repository.EF;
using Drision.Framework.Web.Common;
using Drision.Framework.OrgLibrary;
using Drision.Framework.Common;
using Drision.Framework.OrgLibrary.InternalEntities;

namespace Drision.Framework.Web.Home
{
    public partial class HomeNotice : BaseUserControl
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
            // using (BizDataContext context = new BizDataContext())
            {
                var list = DataHelper.Where<T_CC_NoticePeoples>(p => p.UserID == this.LoginUserID 
                    && p.CC_NoticeAndNoteID != null)
                    .Select(p => new
                    {
                        CC_NoticeAndNote = DataHelper.FindById<T_CC_NoticeAndNote>(p.CC_NoticeAndNoteID),
                        p.CC_NoticeAndNoteID,
                        p.CC_NoticePeoples_Id,
                        p.CC_NoticePeoples_Name,
                        p.Is_Viewed,
                        p.UserID,
                    }).ToList();
                var result = list.Where(p => p.CC_NoticeAndNote != null && p.CC_NoticeAndNote.State == 0).Select(p => new
                {
                    p.UserID,
                    p.CC_NoticeAndNoteID,
                    NoticeName = GetNoticeName(p.CC_NoticeAndNote, 8),
                    Is_Viewed = p.Is_Viewed == true ? null : "<div class='icon_new'></div>",
                    PublishTime = GetPublishTime(p.CC_NoticeAndNote),
                    NoticeURL = string.Format("../CallCenter_Common/T_CC_NoticeAndNote_Detail.aspx?id={0}", p.CC_NoticeAndNoteID),
                }).OrderByDescending(p => p.Is_Viewed).ToList();

                this.rNotice.DataSource = result.Take(6).ToList();
                this.rNotice.DataBind();
            }
        }

        private string GetNoticeName(OrgLibrary.InternalEntities.T_CC_NoticeAndNote note,int count)
        {
            if (note != null)
            {
                string str = note.CC_NoticeAndNote_Name;
                if (str.Length > count)
                {
                    return str.Substring(0, count) + "..";
                }
                return str;
            }
            return null;
        }

        private string GetPublishTime(OrgLibrary.InternalEntities.T_CC_NoticeAndNote note)
        {
            if (note != null)
            {
                DateTime? datetime = note.PublishTime;

                if (datetime != null)
                {
                    return datetime.Value.Date.ToString("MM/dd");
                }
            }
            return null;
        }
    }
}