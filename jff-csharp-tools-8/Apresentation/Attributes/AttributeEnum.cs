using System;
using System.Collections.Generic;

namespace JffCsharpTools8.Apresentation.Attributes
{
    /// <summary>
    /// Generic attribute class that can be applied to methods to specify required roles/permissions.
    /// This attribute works with any enum type that defines roles or permissions in the system.
    /// Can only be applied once per method and is used for authorization purposes.
    /// </summary>
    /// <typeparam name="T">The enum type that defines the roles or permissions</typeparam>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AttributeEnum<T> : Attribute where T : Enum
    {
        /// <summary>
        /// Gets the collection of roles/permissions required to access the decorated method.
        /// This property is read-only and contains all the enum values passed during attribute construction.
        /// </summary>
        public IEnumerable<T> Roles { get; }

        /// <summary>
        /// Initializes a new instance of the AttributeEnum class with the specified roles.
        /// </summary>
        /// <param name="roles">Variable number of enum values representing the required roles/permissions to access the method</param>
        public AttributeEnum(params T[] roles)
        {
            Roles = roles;
        }
    }
}