using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JffCsharpTools.Domain.Extensions
{
    /// <summary>
    /// Extension methods for class objects to provide reflection-based utilities
    /// </summary>
    public static class ClassExtension
    {
        /// <summary>
        /// Gets an array of property names from the specified entity type
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to extract property names from</typeparam>
        /// <param name="type">The entity instance (can be null as only type is used)</param>
        /// <returns>An array of strings containing all property names of the entity</returns>
        public static string[] PropertiesListNames<TEntity>(this TEntity type) where TEntity : class
        {
            PropertyInfo[] propInfos = typeof(TEntity).GetProperties();
            var listNames = propInfos.ToList().Select(s => s.Name).ToArray();
            return listNames;
        }

        /// <summary>
        /// Gets the value of a specified property from an object using reflection
        /// </summary>
        /// <typeparam name="TEntity">The type of the source object</typeparam>
        /// <param name="src">The source object to get the property value from</param>
        /// <param name="propName">The name of the property to retrieve the value from</param>
        /// <returns>The value of the specified property, or null if property doesn't exist</returns>
        public static object PropertiesGetValue<TEntity>(this TEntity src, string propName)
        {
            return src?.GetType()?.GetProperty(propName)?.GetValue(src, null);
        }

        /// <summary>
        /// Retrieves values from multiple properties of an object based on a list of property names
        /// Converts enum values to their integer representation
        /// </summary>
        /// <typeparam name="TEntity">The type of the source object</typeparam>
        /// <param name="src">The source object to extract values from</param>
        /// <param name="listNames">List of property names to retrieve values from</param>
        /// <returns>A list of objects containing the values of the specified properties</returns>
        public static List<object> GetValuesFromListNames<TEntity>(this TEntity src, List<string> listNames)
        {
            var returnListObj = new List<object>();
            foreach (var name in listNames)
            {
                var valueProperty = src.PropertiesGetValue(name);
                if (valueProperty != null && !valueProperty.GetType().IsEnum)
                    returnListObj.Add(valueProperty);
                else if (valueProperty.GetType().IsEnum)
                    returnListObj.Add((int)valueProperty);
            }
            return returnListObj;
        }

        /// <summary>
        /// Adds or updates a property on an ExpandoObject by treating it as a dictionary
        /// Useful for dynamically adding properties to objects at runtime
        /// </summary>
        /// <param name="expando">The ExpandoObject to add the property to (must implement IDictionary&lt;string, object&gt;)</param>
        /// <param name="propertyName">The name of the property to add or update</param>
        /// <param name="propertyValue">The value to assign to the property</param>
        public static void AddProperty(this object expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
    }
}
