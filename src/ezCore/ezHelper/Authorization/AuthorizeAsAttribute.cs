using System;

namespace ez.Core.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAsAttribute : Attribute
    {
        public string Action { get; }
        public string Area { get; set; }
        public string Controller { get; set; }

        public AuthorizeAsAttribute(string action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowUnauthorizedAttribute : Attribute
    {
    }
}
