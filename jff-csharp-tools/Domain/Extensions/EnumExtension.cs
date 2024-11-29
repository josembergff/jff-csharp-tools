
using System.Linq;

namespace JffCsharpTools.Dominio.Extensions
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
    }
}
