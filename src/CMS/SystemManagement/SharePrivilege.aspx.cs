using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using Drision.Framework.OrgLibrary;
using Drision.Framework.Manager;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class SharePrivilege : BasePage
    { 

        private ISharePrivilegeHandler GetSharePrivilegeHandler(BizDataContext ctx)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings[ConstKey.SharePrivilegeInterfacePath];
            if (!string.IsNullOrWhiteSpace(path))
            {
                return CustomHandlerLoader.GetHandlerWithConfiguration<ISharePrivilegeHandler>(path,ctx);
            }
            else
            {
                return new SharePrivilegeManager(ctx);
            }
        }

        private string EntityName
        {
            get
            {
                return ViewState["EntityName"] == null ? "" : ViewState["EntityName"].ToString();
            }
            set
            {
                ViewState["EntityName"] = value;
            }
        }
        private int ObjectId
        {
            get
            {
                return ViewState["ObjectId"] == null ? 0 : ViewState["ObjectId"].ToInt();
            }
            set
            {
                ViewState["ObjectId"] = value;
            }
        }
        
        private BizDataContext context
        {
            get
            {
                return this.DataHelper;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var __EntityName = Request.QueryString["EntityName"];
                var __ObjectId = Request.QueryString["ObjectId"];
                if (string.IsNullOrEmpty(__EntityName) || string.IsNullOrEmpty(__ObjectId))
                {
                    throw new Exception("传入的EntityName或ObjectId不能为空！");
                }
                this.EntityName = __EntityName;
                this.ObjectId = __ObjectId.ToInt();
                InitBinding();
                InitGrid();
            }
        }
        /// <summary>
        /// 初始化绑定下拉
        /// </summary>
        private void InitBinding()
        {
            this.ccShareType.DataSource = typeof(ShareType);
            this.ccShareType.DataBind();

            this.ccShareRoleId.DataSource = this.context.FetchAll<T_Role>();
            this.ccShareRoleId.DataTextField = "Role_Name";
            this.ccShareRoleId.DataValueField = "Role_ID";
            this.ccShareRoleId.DataBind();

            this.ccShareUserId.DataSource = this.context.FetchAll<T_User>();
            this.ccShareUserId.DataTextField = "User_Name";
            this.ccShareUserId.DataValueField = "User_ID";
            this.ccShareUserId.DataBind();

            this.ccPrivilege.DataSource = typeof(EntityOperationEnum);
            this.ccPrivilege.DataBind();
            this.ccPrivilege.Items.RemoveAll(p => p.Value == ((int)EntityOperationEnum.None).ToString() || p.Value == ((int)EntityOperationEnum.Add).ToString());
        }
        /// <summary>
        /// 初始化绑定列表
        /// </summary>
        private void InitGrid()
        {
            var result = this.context.Where<SysSharedPrivilege>(p => p.ObjectId == this.ObjectId).ToList()
                .Select(p => new
                {
                    Id = p.Id,
                    ShareRoleId = p.ShareRoleId == null ? "" : this.context.FindById<T_Role>(p.ShareRoleId).Role_Name,
                    ShareUserId = p.ShareUserId == null ? "" : this.context.FindById<T_User>(p.ShareUserId).User_Name,
                    Privilege = EnumHelper.GetDescription(typeof(EntityOperationEnum), p.Privilege.Value),
                    CreateTime = p.CreateTime,
                }).ToList();
            this.gcProcessInstance.DataSource = result;
            this.gcProcessInstance.DataBind();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        { 
            ShareType sharedType = (ShareType)this.ccShareType.SelectedValue.ToInt();
            var sharedUserId = this.ccShareUserId.SelectedValue.ToIntNullable();
            var sharedRoleId = this.ccShareRoleId.SelectedValue.ToIntNullable();
            //验证下
            if (sharedType == ShareType.ShareRole && sharedRoleId == null)
            {
                this.AjaxAlert("请选择共享角色！");
                return;
            }
            if (sharedType == ShareType.ShareUser && sharedUserId == null)
            {
                this.AjaxAlert("请选择共享人员！");
                return;
            }
            var privilege = (EntityOperationEnum)this.ccPrivilege.SelectedValue.ToInt();

            ISharePrivilegeHandler _sharePrivilege = GetSharePrivilegeHandler(context);
            _sharePrivilege.Share(this.EntityName, this.ObjectId, this.LoginUserID, sharedType, sharedUserId, sharedRoleId, privilege);
            InitGrid();
        }
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            int Id = (sender as LinkButton).CommandArgument.ToInt();
            ISharePrivilegeHandler _sharePrivilege = GetSharePrivilegeHandler(context);
            _sharePrivilege.UnShare(Id);
            InitGrid();
        }
        /// <summary>
        /// 选择不同的共享方式隐藏显示不同的选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccShareType_OnTextChanged(object sender, EventArgs e)
        {
            var shareType = (ShareType)this.ccShareType.SelectedValue.ToInt();
            if (shareType == ShareType.ShareUser)
            {
                this.divShareRoleId.Visible = false;
                this.divShareUserId.Visible = true;
            }
            if (shareType == ShareType.ShareRole)
            {
                this.divShareUserId.Visible = false;
                this.divShareRoleId.Visible = true;
            }
        }
        
    }
}