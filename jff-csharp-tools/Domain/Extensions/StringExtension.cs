using System;
using System.Security.Cryptography;
using System.Text;

namespace JffCsharpTools.Dominio.Extensions
{
    public static class StringExtension
    {
        public static Guid ToGuid(this string aString)
        {
            Guid newGuid;
            Guid returnGuid = default(Guid);

            if (!string.IsNullOrWhiteSpace(aString) && Guid.TryParse(aString, out newGuid))
            {
                returnGuid = newGuid;
            }

            return returnGuid;
        }
    }
}
