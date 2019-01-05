using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace ezHelper.Authorization
{
    public static class PrincipalExtensions
    {
        public static string Id(this IPrincipal principal)
        {
            var id = principal.Identity.Name;
            if (string.IsNullOrEmpty(id))
                return null;

            return id;
        }
    }
}
