using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Drision.Framework.Web;
using Drision.Framework.Web.Common;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using Drision.Framework.OrgLibrary.InternalEntities;

namespace Drision.Framework.Web.Home
{
    public partial class ChangePwd : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.Form["Type"];
            string value = Request.Form["Value"];            
            if (!string.IsNullOrEmpty(type))
            {
                if (type == "OldPwd")
                {
                    string oldPwdMd5 = this.Md5(value);
                    if (this.LoginUser.User_Password == oldPwdMd5)
                    {
                        Response.Write("true");
                    }
                }
                else if (type == "NewPwd")
                {
                    //复杂性验证
                    if (ValidateComplex(value))
                    {
                        Response.Write("true");
                    }
                }
            }            
        }

        protected void cbc_CallBack(object sender, CallBackEventArgs e)
        {
            try
            {                
                string newPwd = e.Parameter;
                string newPwdMd5 = this.Md5(e.Parameter);
                //using (BizDataContext context = new BizDataContext())
                {
                    var user = DataHelper.FindById<Drision.Framework.Entity.T_User>(this.LoginUserID);
                    if (user != null)
                    {
                        user.User_Password = newPwdMd5;
                        DataHelper.Update(user);

                        this.LoginUser.User_Password = user.User_Password;

                        e.Result = "true";
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }        
    }
}
