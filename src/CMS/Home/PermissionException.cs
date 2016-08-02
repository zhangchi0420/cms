using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drision.Framework.Web.Home
{
    public class PermissionException:System.Exception
    {
        public PermissionException(string strMessage):base(strMessage)
        { }

        public PermissionException(string strMessage, Exception innerException)
            : base(strMessage, innerException)
        { }
    }
}
