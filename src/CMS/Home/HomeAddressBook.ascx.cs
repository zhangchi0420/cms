using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using System.Text;
using System.Web.Script.Serialization;
using Drision.Framework.OrgLibrary.InternalEntities;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.Home
{
    public partial class HomeAddressBook : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        public int TotalPage { get; set; }

        private void Initialize()
        {
            //using (BizDataContext context = new BizDataContext())
            {
                var query = DataHelper.Where<T_User>(p => p.User_Status == (int)Enum.EffectiveFlagEnum.Yes)
                    .Select(p => new
                    {
                        p.User_ID,
                        p.User_Name,
                        p.User_Code,
                        p.User_Mobile,
                    });

                var result = query.ToList();
                this.TotalPage = result.Count % 3 == 0 ? result.Count / 3 : result.Count / 3 + 1;
                var list = query.Take(3).ToList();
                this.rUserAddress.DataSource = list;
                this.rUserAddress.DataBind();
            }
        }

        protected void cbcPage_CallBack(object sender, CallBackEventArgs e)
        {
            int index = e.Parameter.ToInt();
            //using (BizDataContext context = new BizDataContext())
            {
                var query = DataHelper.Where<T_User>(p => p.User_Status == (int)Enum.EffectiveFlagEnum.Yes)
                    .Select(p => new
                    {
                        p.User_ID,
                        p.User_Name,
                        p.User_Code,
                        p.User_Mobile,
                    });
                var list = query.ToList().Skip(3 * index).Take(3).ToList();

                StringBuilder sb = new StringBuilder();
                foreach (var p in list)
                {
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", p.User_Name, p.User_Code, p.User_Mobile);
                }
                e.Result = sb.ToString();
            }
        }

        protected void cbcQuery_CallBack(object sender, CallBackEventArgs e)
        {
            string param = e.Parameter;

            // using (BizDataContext context = new BizDataContext())
            {
                var query = DataHelper.Where<T_User>(p => p.User_Status == (int)Enum.EffectiveFlagEnum.Yes)
                    .Select(p => new
                    {
                        p.User_ID,
                        p.User_Name,
                        p.User_Code,
                        p.User_Mobile,
                    });
                var list = query.ToList();
                int index = 0;
                int count = 0;
                int temp = 0;                
                foreach (var p in list)
                {
                    count++;
                    temp++;
                    if (temp == 3)
                    {
                        index++;
                        temp = 0;
                    }

                    if (p.User_Name.Contains(param) || p.User_Code.Contains(param))
                    {
                        break;
                    }
                }

                if (temp == 0 && count != list.Count)
                {
                    index--;
                }

                list = list.Skip(3 * index).Take(3).ToList();

                StringBuilder sb = new StringBuilder();
                foreach (var p in list)
                {
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", p.User_Name, p.User_Code, p.User_Mobile);
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                e.Result = js.Serialize(new
                {
                    Content = sb.ToString(),
                    Index = index + 1,
                });                
            }
        }
    }
}