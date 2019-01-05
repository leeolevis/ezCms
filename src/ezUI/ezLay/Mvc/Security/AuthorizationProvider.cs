using DapperExtensions;
using ez.Core.Authorization;
using ezModel.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ezLay.Mvc.Security
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private IEnumerable<Type> Controllers { get; }
        private IServiceProvider Services { get; }
        private Dictionary<string, string> Required { get; } //需要验证的Action
        private Dictionary<string, HashSet<string>> Permissions { get; set; }

        private IHttpContextAccessor AccessorHttpContext { get; }
        private IDatabase Database { get; }

        public AuthorizationProvider(Assembly controllers, IServiceProvider services, IHttpContextAccessor accessorHttpContext, IDatabase database)
        {
            AccessorHttpContext = accessorHttpContext;
            Database = database;
            Permissions = new Dictionary<string, HashSet<string>>();
            Required = new Dictionary<string, string>();
            Controllers = GetValid(controllers);
            Services = services;
            string permission = string.Empty;
            foreach (var type in Controllers)
            {
                foreach (var method in GetValidMethods(type))
                {
                    if (string.IsNullOrEmpty(GetArea(type)))
                        permission = (GetController(type) + "/" + GetAction(method)).ToLower();
                    else
                        permission = (GetArea(type) + "/" + GetController(type) + "/" + GetAction(method)).ToLower();

                    var requiredPermission = GetRequiredPermission(type, method);

                    if (requiredPermission != null && !Required.ContainsKey(permission))
                        Required[permission] = requiredPermission;
                }
            }

            RefreshAll();
        }

        public bool IsAuthorizedFor(string roleId, string area, string controller, string action)
        {
            var _config = Services.GetRequiredService<IConfiguration>();
            var superRole = _config["WebOption:SuperRole"].ToString().Split(',');
            if (superRole.Contains(roleId)) return true;//如果是超管角色则不校验
            //if (AccessorHttpContext.HttpContext.User.Identities.First(u => u.IsAuthenticated).FindFirst(MyClaimTypes.PrivilegeManage.ToString()).Value == "True") return true;

            string permission = string.Empty;
            if (string.IsNullOrEmpty(area))
                permission = (controller + "/" + action).ToLower();
            else
                permission = (area + "/" + controller + "/" + action).ToLower();

            if (!Required.ContainsKey(permission))
                return true;
            var result = Permissions.ContainsKey(roleId) && Permissions[roleId].Contains(Required[permission]);
            return result;
        }

        public bool IsDataAuthorizedFor(string roleId, string resource)
        {
            var _config = Services.GetRequiredService<IConfiguration>();
            var superRole = _config["WebOption:SuperRole"].ToString().Split(',');
            if (superRole.Contains(roleId)) return true;//如果是超管角色则不校验
            //if (AccessorHttpContext.HttpContext.User.Identities.First(u => u.IsAuthenticated).FindFirst(MyClaimTypes.PrivilegeManage.ToString()).Value == "True") return true;

            var result = Permissions.ContainsKey(roleId) && Permissions[roleId].Contains(resource);
            return result;
        }

        public bool IsSuperAdmin(string roleId)
        {
            var _config = Services.GetRequiredService<IConfiguration>();
            var superRole = _config["WebOption:SuperRole"].ToString().Split(',');
            if (superRole.Contains(roleId)) return true;//如果是超管角色则不校验
            //if (AccessorHttpContext.HttpContext.User.Identities.First(u => u.IsAuthenticated).FindFirst(MyClaimTypes.PrivilegeManage.ToString()).Value == "True") return true;
            return false;
        }


        /// <summary>
        /// 第一次启动加载全部权限
        /// </summary>
        private void RefreshAll()
        {
            var list = Database.GetListSQL<rolePrivilegeModel>("select privilege.id,privilege.type,privilege.`name`,privilege.resource from roleprivilege inner join privilege on roleprivilege.privilegeid=privilege.id");

            ////多表关联写法
            //var joinList = _database.GetList<RoleList>(new List<IJoinPredicate>() {
            //                                                    Predicates.Join<通用_角色_权限点, 通用_权限点>(t => t.权限点, Operator.Eq, t1 => t1.序号,JoinOperator.Inner,"通用_角色_权限点","通用_权限点")
            //                                            },
            //                                            new List<IJoinAliasPredicate>()
            //                                            {
            //                                                 Predicates.JoinAlia<RoleList,通用_权限点>(t=>t.id,t1=>t1.序号),
            //                                                    Predicates.JoinAlia<RoleList,通用_权限点>(t=>t.area,t1=>t1.区域),
            //                                                    Predicates.JoinAlia<RoleList,通用_权限点>(t=>t.controller,t1=>t1.控制器),
            //                                                    Predicates.JoinAlia<RoleList,通用_权限点>(t=>t.action,t1=>t1.动作)
            //                                            });

            foreach (var item in list)
            {
                if (!Permissions.ContainsKey(item.id))
                {
                    Permissions.Add(item.id, new HashSet<string>(list.Where(t => t.id == item.id & t.resource != "").Select(t => t.resource?.ToLower())));
                }
            }

        }

        /// <summary>
        /// 有权限变动时刷新相应角色权限
        /// </summary>
        /// <param name="accountId">角色Id</param>
        public void Refresh(string roleId)
        {
            //先删除角色权限
            if (Permissions.ContainsKey(roleId))
                Permissions.Remove(roleId);

            //重新加载权限
            var list = Database.GetListSQL<rolePrivilegeModel>("select privilege.id,privilege.type,privilege.`name`,privilege.resource from roleprivilege inner join privilege on roleprivilege.privilegeid=privilege.id WHERE roleid=@roleid", new { roleid = roleId });

            ////多表关联写法
            //var joinList = _database.GetList<RoleList>(
            //                                            //关联
            //                                            new List<IJoinPredicate>() {
            //                                                    Predicates.Join<通用_角色_权限点, 通用_权限点>(t => t.权限点, Operator.Eq, t1 => t1.序号,JoinOperator.Inner,"通用_角色_权限点","通用_权限点")
            //                                            },
            //                                            //查询映射字段
            //                                            new List<IJoinAliasPredicate>()
            //                                            {
            //                                                    Predicates.JoinAlia<RoleList,通用_权限点>(t=>t.id,t1=>t1.序号),
            //                                                    Predicates.JoinAlia<RoleList,通用_权限点>(t=>t.area,t1=>t1.区域),
            //                                                    Predicates.JoinAlia<RoleList,通用_权限点>(t=>t.controller,t1=>t1.控制器),
            //                                                    Predicates.JoinAlia<RoleList,通用_权限点>(t=>t.action,t1=>t1.动作)
            //                                            }
            //                                            //查询
            //                                            , Predicates.Field<通用_角色_权限点>(p => p.角色, Operator.Eq, roleId)
            //                                            //排序
            //                                            , new List<ISort>() {Predicates.Sort<通用_角色_权限点>(p=>p.角色)
            //                                            });

            foreach (var item in list)
            {
                if (!Permissions.ContainsKey(item.id))
                {
                    Permissions.Add(item.id, new HashSet<string>(list.Where(t => t.id == item.id & t.resource != "").Select(t => t.resource?.ToLower())));
                }
            }

        }

        private IEnumerable<MethodInfo> GetValidMethods(Type controller)
        {
            return controller
                    .GetMethods(
                        BindingFlags.DeclaredOnly |
                        BindingFlags.InvokeMethod |
                        BindingFlags.Instance |
                        BindingFlags.Public)
                    .Where(method =>
                        !method.IsDefined(typeof(NonActionAttribute)) &&
                        !method.IsSpecialName)
                    .OrderByDescending(method =>
                        method.IsDefined(typeof(HttpGetAttribute), false));
        }
        private IEnumerable<Type> GetValid(Assembly controllers)
        {
            return controllers
                .GetTypes()
                .Where(type =>
                    type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
                    typeof(Controller).IsAssignableFrom(type) &&
                    !type.IsAbstract &&
                    type.IsPublic);
        }

        private string GetRequiredPermission(Type type, MethodInfo method)
        {
            var authorizeAs = method.GetCustomAttribute<AuthorizeAsAttribute>(false);
            var controller = GetController(type);
            var action = GetAction(method);
            var area = GetArea(type);

            if (authorizeAs != null)
            {
                type = GetControllerType(authorizeAs.Area ?? area, authorizeAs.Controller ?? controller);
                method = GetMethod(type, authorizeAs.Action);

                return GetRequiredPermission(type, method);
            }
            if (string.IsNullOrEmpty(area))
                return AllowsUnauthorized(type, method) ? null : (controller + "/" + action).ToLower();
            else
                return AllowsUnauthorized(type, method) ? null : (area + "/" + controller + "/" + action).ToLower();
        }
        private string GetAction(MethodInfo method)
        {
            return method.GetCustomAttribute<ActionNameAttribute>(false)?.Name ?? method.Name;
        }
        private string GetController(Type type)
        {
            return type.Name.Substring(0, type.Name.Length - 10);
        }
        private string GetArea(Type type)
        {
            return type.GetCustomAttribute<AreaAttribute>(false)?.RouteValue;
        }

        private bool AllowsUnauthorized(Type controller, MethodInfo method)
        {
            if (method.IsDefined(typeof(AuthorizeAttribute), false)) return false;
            if (method.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
            if (method.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

            while (controller != typeof(Controller))
            {
                if (controller.IsDefined(typeof(AuthorizeAttribute), false)) return false;
                if (controller.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
                if (controller.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

                controller = controller.BaseType;
            }

            return true;
        }
        private Type GetControllerType(string area, string controller)
        {
            var controllers = Controllers
                .Where(type => type.Name.Equals(controller + "Controller", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(area))
                controllers = controllers.Where(type =>
                    !type.IsDefined(typeof(AreaAttribute), false));
            else
                controllers = controllers.Where(type =>
                    type.IsDefined(typeof(AreaAttribute), false) &&
                    string.Equals(type.GetCustomAttribute<AreaAttribute>(false).RouteValue, area, StringComparison.OrdinalIgnoreCase));

            return controllers.Single();
        }
        private MethodInfo GetMethod(Type controller, string action)
        {
            return controller
                .GetMethods()
                .Where(method =>
                    (
                        !method.IsDefined(typeof(ActionNameAttribute), false) &&
                        method.Name.ToLowerInvariant() == action.ToLowerInvariant()
                    )
                    ||
                    (
                        method.IsDefined(typeof(ActionNameAttribute), false) &&
                        method.GetCustomAttribute<ActionNameAttribute>(false).Name.ToLowerInvariant() == action.ToLowerInvariant()
                    ))
                .OrderByDescending(method =>
                    method.IsDefined(typeof(HttpGetAttribute), false))
                .First();
        }

        public bool IsAdminRole(string roleId)
        {
            throw new NotImplementedException();
        }
    }
}
