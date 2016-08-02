using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Entity;
using Drision.Framework.Repository.EF;
using System.Text;
using Drision.Framework.Common;

namespace Drision.Framework.Web.Home
{
    public partial class HomePersonalInfo : BaseUserControl
    {
        public int LoginUserID
        {
            get
            {
                return (this.Page as BasePage).LoginUserID;
            }
        }

        public int CanAdmin
        {
            get
            {
                using (BizDataContext db = new BizDataContext())
                {
                   return  (int)db.FirstOrDefault<T_User_Role>(t => t.User_ID == LoginUserID).Role_ID;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public string RoleName { get; set; }

        private void Initialize()
        {
            //using (BizDataContext context = new BizDataContext())
            {
                var user = DataHelper.FindById<T_User>(this.LoginUserID);
                if (user != null)
                {
                    var orgProxy = OrgLibrary.OrgProxyFactory.GetProxy(this.DataHelper);
                    var roleList = orgProxy.GetUserRoles(user.User_ID).ToList();
                    
                    this.UserName = user.User_Name;
                    if (user.Department_ID != null)
                    {
                        var department = orgProxy.GetDepartmentById(user.Department_ID.Value);
                        if (department != null)
                        {
                            this.DepartmentName = department.Department_Name;
                        }
                    }
                    
                    if (roleList != null && roleList.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder("<span>");
                        sb.Append(string.Join("</span><span>", roleList.Select(p => p.Role_Name)));
                        sb.Append("</span>");

                        this.RoleName = sb.ToString();
                    }
                }
            }
        }
    }
}