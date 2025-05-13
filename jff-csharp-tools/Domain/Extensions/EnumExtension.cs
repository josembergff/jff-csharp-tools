
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JffCsharpTools.Domain.Extensions
{
    public static class EnumExtension
    {
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


        public static int ToInt(this System.Enum value)
        {
            int intAttribute;

            int.TryParse(value.ToString(), out intAttribute);

            return intAttribute;
        }

        public static bool IsObsolete(this System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (ObsoleteAttribute[])
                fi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return attributes != null && attributes.Length > 0;
        }

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
