using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drision.Framework.Web
{
    public class UserApproveComment
    {
        public int? WorkItemId { get; set; }
        public string User_Name { get; set; }
        public string ApproveComment { get; set; }
        public string AddingUser { get; set; }
        public string ProxyUser { get; set; }
        public string ApproveResult { get; set; }
        public string ApproveDate { get; set; }
    }
}