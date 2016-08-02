using System;
using System.Web.Security;
using System.Web.UI;
using System.Configuration;
using System.Linq;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;

namespace Drision.Framework.Web
{
    /// <summary>
    /// 全局属性方法
    /// </summary>
    public static class GlobalObject
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnString
        {
            // get { return ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString; }
            get { return "Meta_BIZ"; }
        }

        /// <summary>
        /// MD5加密数据
        /// </summary>
        /// <param name="encrypt">密文</param>
        public static string Md5(string encrypt)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(encrypt, "md5");
        }

        /// <summary>
        /// MD5加密数据
        /// </summary>
        /// <param name="encrypt">密文</param>
        public static string Md5(this Page page, string encrypt)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(encrypt, "md5");
        }

        /// <summary>
        /// 判断是否含有权限
        /// </summary>
        /// <param name="roleId">角色ID号</param>
        /// <param name="entityId">实体ID号</param>
        /// <param name="operationEnum">操作方式</param>
        /// <returns></returns>
        public static bool TryCanOperation(int roleId, int entityId, EntityOperationEnum operationEnum)
        {
            using (BizDataContext context = new BizDataContext(GlobalObject.ConnString))
            {
                var query = context.FirstOrDefault<SysDataPrivilege>(x => x.RoleId == roleId && x.EntityId == entityId
                    && x.OperationId == (int)operationEnum);
                if (query == null)
                {
                    return false;
                }
                else if (query.PrivelegeLevel != (int)EntityPrivilegeEnum.NoPermission)
                {
                    return true;
                }
            }
            return false;
        }

    }

    /// <summary>
    /// Session超时异常
    /// </summary>
    public class SessionExpiredException : ApplicationException
    {
        public SessionExpiredException()
            : base("未登录或Session超时")
        {
        }
    }

}