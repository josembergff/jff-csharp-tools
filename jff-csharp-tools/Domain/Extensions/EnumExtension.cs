
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JffCsharpTools.Domain.Extensions
{
    /// <summary>
    /// Extension methods for System.Enum to provide additional functionality
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the description of an enum value from its custom attributes
        /// </summary>
        /// <param name="value">The enum value to get description from</param>
        /// <returns>The description string or "N/A" if no description attribute is found</returns>
        public static string GetDescription(this System.Enum value)
        {
            dynamic displayAttribute = null;
            var field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attributes = field.GetCustomAttributes(false);

                if (attributes.Any())
                {
                    displayAttribute = attributes.ElementAt(0);
                }
            }

            return displayAttribute?.Description ?? "N/A";
        }

        /// <summary>
        /// Converts an enum value to its integer representation
        /// </summary>
        /// <param name="value">The enum value to convert</param>
        /// <returns>The integer value of the enum</returns>
        public static int ToInt(this System.Enum value)
        {
            int intAttribute;

            int.TryParse(value.ToString(), out intAttribute);

            return intAttribute;
        }

        /// <summary>
        /// Checks if an enum value is marked with the ObsoleteAttribute
        /// </summary>
        /// <param name="value">The enum value to check</param>
        /// <returns>True if the enum value has the ObsoleteAttribute, false otherwise</returns>
        public static bool IsObsolete(this System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (ObsoleteAttribute[])
                fi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return attributes != null && attributes.Length > 0;
        }

        /// <summary>
        /// Filters an enumerable of enum values to exclude those marked as obsolete
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The enumerable of enum values to filter</param>
        /// <returns>An enumerable containing only non-obsolete enum values</returns>
        public static IEnumerable<T> FilterNonObsoleteValues<T>(this IEnumerable<T> value) where T : System.Enum
        {
            var enumType = typeof(T);
            var enumValues = Enum.GetValues(enumType).Cast<T>();
            var nonObsoleteValues = new ArrayList();

            foreach (var enumValue in enumValues)
            {
                if (enumValue.IsObsolete() == false)
                {
                    nonObsoleteValues.Add(enumValue);
                }
            }

            return nonObsoleteValues.Cast<T>();
        }
    }
}
