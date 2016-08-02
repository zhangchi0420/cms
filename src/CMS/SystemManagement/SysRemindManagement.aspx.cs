using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.OrgLibrary.InternalEntities;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class SysRemindManagement : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            this.PageIndex_InBox = this.gcInBox.PagerSettings.PageIndex;
            this.PageSize_InBox = this.gcInBox.PagerSettings.PageSize;

            this.PageIndex_OutBox = this.gcOutBox.PagerSettings.PageIndex;
            this.PageSize_OutBox = this.gcOutBox.PagerSettings.PageSize;

            BindInBox();
            BindOutBox();
            BindDeleteBox();
        }

        #region 收件箱

        public int PageIndex_InBox
        {
            get { return VS<int>("PageIndex_InBox"); }
            set { VS<int>("PageIndex_InBox", value); }
        }

        public int PageSize_InBox
        {
            get { return VS<int>("PageSize_InBox"); }
            set { VS<int>("PageSize_InBox", value); }
        }

        private void BindInBox()
        {
            var list = this.DataHelper.Set<SysRemind>()
                .Where(p => p.OwnerId == this.LoginUserID && p.State != (int)RemindStausEnum.IsDeleted)
                .OrderByDescending(p => p.CreateTime);

            var result = list.Skip(this.PageIndex_InBox * this.PageSize_InBox).Take(this.PageSize_InBox).ToList();            

            var source = result.Select(p => new
            {
                p.RemindId,
                p.RemindName,
                p.RemindURL,                
                CreateUser = GetUserName(p.CreateUserId),
                p.CreateTime,
                p.State,
                StateText = GetState(p.State),
            }).ToList();

            this.gcInBox.DataSource = source;
            this.gcInBox.PagerSettings.DataCount = list.Count();
            this.gcInBox.DataBind();
        }

        protected void gcInBox_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex_InBox = e.PageIndex;
            this.PageSize_InBox = e.PageSize;
            BindInBox();
        }

        #endregion

        #region 发件箱

        public int PageIndex_OutBox
        {
            get { return VS<int>("PageIndex_OutBox"); }
            set { VS<int>("PageIndex_OutBox", value); }
        }

        public int PageSize_OutBox
        {
            get { return VS<int>("PageSize_OutBox"); }
            set { VS<int>("PageSize_OutBox", value); }
        }

        private void BindOutBox()
        {
            var list = this.DataHelper.Set<SysRemind>()
                .Where(p => p.CreateUserId == this.LoginUserID && p.State != (int)RemindStausEnum.IsDeleted)
                .OrderByDescending(p => p.CreateTime);

            var result = list.Skip(this.PageIndex_OutBox * this.PageSize_OutBox).Take(this.PageSize_OutBox).ToList();

            var source = result.Select(p => new
            {
                p.RemindId,
                p.RemindName,
                p.RemindURL,
                Owner = GetUserName(p.CreateUserId),
                p.CreateTime,
                p.State,
                StateText = GetState(p.State),
            }).ToList();

            this.gcOutBox.DataSource = source;
            this.gcOutBox.PagerSettings.DataCount = list.Count();
            this.gcOutBox.DataBind();
        }

        protected void gcOutBox_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex_OutBox = e.PageIndex;
            this.PageSize_OutBox = e.PageSize;
            BindOutBox();
        }

        #endregion

        #region 已删除

        public int PageIndex_IsDelete
        {
            get { return VS<int>("PageIndex_IsDelete"); }
            set { VS<int>("PageIndex_IsDelete", value); }
        }

        public int PageSize_IsDelete
        {
            get { return VS<int>("PageSize_IsDelete"); }
            set { VS<int>("PageSize_IsDelete", value); }
        }

        private void BindDeleteBox()
        {
            var list = this.DataHelper.Set<SysRemind>()
                .Where(p => p.OwnerId == this.LoginUserID && p.State == (int)RemindStausEnum.IsDeleted)
                .OrderByDescending(p => p.CreateTime);

            var result = list.Skip(this.PageIndex_IsDelete * this.PageSize_IsDelete).Take(this.PageSize_IsDelete).ToList();

            var source = result.Select(p => new
            {
                p.RemindId,
                p.RemindName,
                p.RemindURL,
                CreateUser = GetUserName(p.CreateUserId),
                p.CreateTime,
                p.State,
                StateText = GetState(p.State),
            }).ToList();

            this.gcIsDelete.DataSource = source;
            this.gcIsDelete.PagerSettings.DataCount = list.Count();
            this.gcIsDelete.DataBind();
        }

        protected void gcIsDelete_PageIndexChanging(object sender, GridPostBackEventArgs e)
        {
            this.PageIndex_IsDelete = e.PageIndex;
            this.PageSize_IsDelete = e.PageSize;
            BindDeleteBox();
        }

        #endregion

        private string GetUserName(int? userid)
        {
            var user = this.DataHelper.FindById<T_User>(userid);
            return user != null ? user.User_Name : string.Empty;
        }

        private string GetState(int? state)
        {
            string result = string.Empty;
            if (state != null)
            {
                result = EnumHelper.GetDescription(typeof(RemindStausEnum), state.Value);
            }
            return result;
        }

        #region 操作

        private void ChangeState(RemindStausEnum state)
        {
            foreach (string idStr in this.gcInBox.SelectedValues)
            {
                int id = idStr.ToInt();
                var remind = this.DataHelper.FindById<SysRemind>(id);
                if (remind != null)
                {
                    remind.State = (int)state;

                    this.DataHelper.UpdatePartial<SysRemind>(remind, p => new { p.State });
                }
            }

            Initialize();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChangeState(RemindStausEnum.IsDeleted);
        }

        protected void btnIsRead_Click(object sender, EventArgs e)
        {
            ChangeState(RemindStausEnum.Completed);
        }

        protected void btnNotRead_Click(object sender, EventArgs e)
        {
            ChangeState(RemindStausEnum.New);
        }

        protected void btnTrueDelete_Click(object sender, EventArgs e)
        {
            foreach (string idStr in this.gcIsDelete.SelectedValues)
            {
                int id = idStr.ToInt();
                var remind = this.DataHelper.FindById<SysRemind>(id);
                if (remind != null)
                {
                    this.DataHelper.Delete(remind);
                }
            }

            Initialize();
        }

        #endregion

        
    }
}