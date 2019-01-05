using ez.Core.Authorization;
using ezModel.BaseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;


namespace ezLay.Mvc.TagsHelpers
{
    [HtmlTargetElement("a", Attributes = "permission-for")]
    [HtmlTargetElement("dd", Attributes = "permission-for")]
    [HtmlTargetElement("input", Attributes = "permission-for")]
    [HtmlTargetElement("div", Attributes = "permission-for")]
    [HtmlTargetElement("td", Attributes = "permission-for")]
    [HtmlTargetElement("li", Attributes = "permission-for")]
    [HtmlTargetElement("img", Attributes = "permission-for")]
    [HtmlTargetElement("button", Attributes = "permission-for")]
    [HtmlTargetElement("span", Attributes = "permission-for")]
    public class PermissionTagHelper : TagHelper
    {
        private readonly IAuthorizationProvider Authorization;

        private readonly IHttpContextAccessor _accessor;

        public PermissionTagHelper(IAuthorizationProvider authorization, IHttpContextAccessor accessor)
        {
            Authorization = authorization;
            _accessor = accessor;
        }

        [HtmlAttributeName("permission-for")]
        public string For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated)
                return;

            if (For.Contains(','))
            {
                List<bool?> groupPermission = new List<bool?>();
                string[] mores = For.Split(',');
                foreach (var item in mores)
                {
                    if (For.Contains('/'))
                    {
                        var url = item.ToString().Split('/');
                        var roleId = _accessor.HttpContext.User.Identities.First(u => u.IsAuthenticated).FindFirst(MyClaimTypes.Role.ToString()).Value;
                        if (url.Length > 2)
                            groupPermission.Add(Authorization?.IsAuthorizedFor(roleId, url[0], url[1], url[2]));
                        else
                            groupPermission.Add(Authorization?.IsAuthorizedFor(roleId, "", url[0], url[1]));
                    }
                    else
                    {
                        var roleId = _accessor.HttpContext.User.Identities.First(u => u.IsAuthenticated).FindFirst(MyClaimTypes.Role.ToString()).Value;
                        groupPermission.Add(Authorization?.IsDataAuthorizedFor(roleId, For));
                    }
                }
                if (!groupPermission.Any(t => t.Value == true))
                    output.Attributes.SetAttribute("class", (output.Attributes["class"]?.Value + " hide").Trim());
            }
            else
            {
                //页面权限和资源权限
                if (For.Contains('/'))
                {
                    var url = For.ToString().Split('/');
                    var roleId = _accessor.HttpContext.User.Identities.First(u => u.IsAuthenticated).FindFirst(MyClaimTypes.Role.ToString()).Value;
                    bool? hasPermission = false;
                    if (url.Length > 2)
                        hasPermission = Authorization?.IsAuthorizedFor(roleId, url[0], url[1], url[2]);
                    else
                        hasPermission = Authorization?.IsAuthorizedFor(roleId, "", url[0], url[1]);

                    if (!hasPermission.Value)
                        output.Attributes.SetAttribute("class", (output.Attributes["class"]?.Value + " hide").Trim());
                }
                else
                {
                    var roleId = _accessor.HttpContext.User.Identities.First(u => u.IsAuthenticated).FindFirst(MyClaimTypes.Role.ToString()).Value;
                    var hasPermission = Authorization?.IsDataAuthorizedFor(roleId, For);
                    if (!hasPermission.Value)
                        output.Attributes.SetAttribute("class", (output.Attributes["class"]?.Value + " hide").Trim());
                }
            }
        }
    }
}
