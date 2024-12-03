using System;
using System.Collections.Generic;

namespace JffCsharpTools6.Apresentation.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AttributeEnum : Attribute
    {
        public IEnumerable<string> Roles { get; }

        public AttributeEnum(params string[] roles)
        {
            Roles = roles;
        }
    }
}