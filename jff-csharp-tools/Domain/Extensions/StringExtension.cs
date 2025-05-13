using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JffCsharpTools.Domain.Extensions
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

        public static string ToValidCPF(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return string.Empty;
            }

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
            {
                return string.Empty;
            }

            if (cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999")
            {
                return string.Empty;
            }

            int[] numbers = new int[11];
            for (int i = 0; i < 11; i++)
            {
                numbers[i] = int.Parse(cpf[i].ToString());
            }

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += numbers[i] * (10 - i);
            }

            int firstDigit = 11 - (sum % 11);
            if (firstDigit >= 10)
            {
                firstDigit = 0;
            }

            if (firstDigit != numbers[9])
            {
                return string.Empty;
            }

            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += numbers[i] * (11 - i);
            }

            int secondDigit = 11 - (sum % 11);
            if (secondDigit >= 10)
            {
                secondDigit = 0;
            }

            if (secondDigit != numbers[10])
            {
                return string.Empty;
            }

            return cpf;
        }

        public static (string first, string last) GetNameFromEmail(this string email)
        {
            var returnString = (string.Empty, string.Empty);
            if (string.IsNullOrWhiteSpace(email))
            {
                return (string.Empty, string.Empty);
            }

            string[] parts = email.Split('@');
            if (parts.Length != 2)
            {
                return (string.Empty, string.Empty);
            }
            else
            {
                returnString.Item1 = parts.First();
                returnString.Item2 = string.Empty;
            }

            if (returnString.Item1.Contains("."))
            {
                string[] names = returnString.Item1.Split('.');
                returnString.Item1 = names.First();
                returnString.Item2 = names.Last();
            }
            else if (returnString.Item1.Contains("_"))
            {
                string[] names = returnString.Item1.Split('_');
                returnString.Item1 = names.First();
                returnString.Item2 = names.Last();
            }
            else if (returnString.Item1.Contains("-"))
            {
                string[] names = returnString.Item1.Split('-');
                returnString.Item1 = names.First();
                returnString.Item2 = names.Last();
            }

            return returnString;
        }
    }
}
