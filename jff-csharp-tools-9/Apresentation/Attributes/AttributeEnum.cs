using System;
using System.Collections.Generic;

namespace JffCsharpTools9.Apresentation.Attributes
{
    /// <summary>
    /// Custom attribute for method-level authorization based on enum roles.
    /// This attribute can be applied to methods to specify which enum-based roles are required for access.
    /// </summary>
    /// <typeparam name="T">The enum type that represents the roles or permissions</typeparam>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AttributeEnum<T> : Attribute where T : Enum
    {
        /// <summary>
        /// Gets the collection of roles/permissions required to access the decorated method
        /// </summary>
        public IEnumerable<T> Roles { get; }

        /// <summary>
        /// Initializes a new instance of the AttributeEnum with the specified roles
        /// </summary>
        /// <param name="roles">Array of enum values representing the required roles for authorization</param>
        public AttributeEnum(params T[] roles)
        {
            Roles = roles;
        }
    }
}