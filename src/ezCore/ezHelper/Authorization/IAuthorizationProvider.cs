namespace ez.Core.Authorization
{
    public interface IAuthorizationProvider
    {
        //普通页面权限校验
        bool IsAuthorizedFor(string roleId, string area, string controller, string action);

        //资源数据权限校验
        bool IsDataAuthorizedFor(string roleId, string resource);

        //是否管理员
        bool IsSuperAdmin(string roleId);

        void Refresh(string roleId);
    }
}
