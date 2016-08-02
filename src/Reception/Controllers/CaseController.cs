using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using Drision.Framework.Enum;
using Reception.Models;

namespace Reception.Controllers
{
    public class CaseController : Controller
    {
        //
        // GET: /Case/
        [AllowAnonymous]
        public ActionResult Index()
        {
            Case model = new Case();
            using (BizDataContext db = new BizDataContext())
            {
                var prot = db.Where<T_Product>(p => p.State == (int)T_ProductStateEnum.Enable).OrderBy(p => p.Product_Id).FirstOrDefault();
                if (prot != null)
                {
                    model.Id = prot.Product_Id;
                    model.Title = prot.Product_Name;
                    model.Content = prot.Content;
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult CaseList()
        {
            return View();
        }

    }
}
