using System;
using System.Collections.Generic;

namespace JffCsharpTools6.Apresentation.Attributes
{
    /// <summary>
    /// Custom attribute for specifying role-based authorization on controller action methods
    /// Allows multiple roles to be assigned to an action for access control
    /// Used in conjunction with TokenEnumFilter to validate user roles from JWT tokens
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AttributeEnum : Attribute
    {
        /// <summary>
        /// Gets the collection of role names that are authorized to access the decorated method
        /// </summary>
        public IEnumerable<string> Roles { get; }

        /// <summary>
        /// Initializes a new instance of the AttributeEnum with specified authorized roles
        /// </summary>
        /// <param name="roles">Variable number of role names that are authorized to access the method</param>
        public AttributeEnum(params string[] roles)
        {
            Roles = roles;
        }
    }
}