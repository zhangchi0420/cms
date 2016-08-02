using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Common;
using Drision.Framework.Repository.EF;
using System.Text;
using Drision.Framework.Entity;
using Drision.Framework.Web.Common;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class PublicTempFunction : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnChangeSystemLevelCode_Click(object sender, EventArgs e)
        {
            using (BizDataContext context = new BizDataContext())
            {
                StringBuilder sbSQL = new StringBuilder();
                //将父部门字段值调整过来并先处理好顶级部门的层级码
                sbSQL.AppendLine("update T_Department set SystemLevelCode = Convert(nvarchar(50),Department_ID)+'-' where Parent_ID is null");
                context.ExecuteNonQuery(sbSQL.ToString());
                var AllDepts = context.FetchAll<T_Department>();
                var Depts = AllDepts.Where(p => !(p.Parent_ID == null)).ToList();
                var DealedDepts = AllDepts.Where(p => p.Parent_ID == null).ToList();
                while (Depts.Count > 0)
                {
                    for (int i = Depts.Count - 1; i >= 0; i--)
                    {
                        var Child = Depts[i];
                        var FindParent = DealedDepts.FirstOrDefault(p => p.Department_ID == Child.Parent_ID);
                        if (FindParent != null)
                        {
                            //当前未处理的这个是某个已处理的子部门，则处理当前这个，并把它从未处理中移走，加到已处理中。
                            Child.SystemLevelCode = FindParent.SystemLevelCode + Child.Department_ID + "-";
                            context.Update(Child);
                            Depts.Remove(Child);
                            DealedDepts.Add(Child);
                        }
                    }
                }                
            }
            this.AjaxAlert("调整成功！");
        }
    }
}