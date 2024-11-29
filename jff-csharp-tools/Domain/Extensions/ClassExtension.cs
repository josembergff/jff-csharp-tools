using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JffCsharpTools.Dominio.Extensions
{
    public static class ClassExtension
    {
        public static string[] PropertiesListNames<TEntity>(this TEntity type) where TEntity : class
        {
            PropertyInfo[] propInfos = typeof(TEntity).GetProperties();
            var listNames = propInfos.ToList().Select(s => s.Name).ToArray();
            return listNames;
        }

        public static object PropertiesGetValue<TEntity>(this TEntity src, string propName)
        {
            return src?.GetType()?.GetProperty(propName)?.GetValue(src, null);
        }

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
