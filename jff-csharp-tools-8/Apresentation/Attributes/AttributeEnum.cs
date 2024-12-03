using System;
using System.Collections.Generic;

namespace JffCsharpTools8.Apresentation.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AttributeEnum<T> : Attribute where T : Enum
    {
        public IEnumerable<T> Roles { get; }

        public AttributeEnum(params T[] roles)
        {
            Roles = roles;
        }
    }
}