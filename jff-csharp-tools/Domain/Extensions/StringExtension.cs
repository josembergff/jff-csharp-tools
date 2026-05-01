using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace JffCsharpTools.Domain.Extensions
{
    /// <summary>
    /// Extension methods for string to provide additional text manipulation and validation functionality.
    /// Includes methods for GUID conversion, CPF validation, email parsing, formatting, and text masking.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Converts a string to a GUID. Returns default GUID if conversion fails.
        /// </summary>
        /// <param name="input">The string to convert to GUID</param>
        /// <returns>A valid GUID or default GUID if conversion fails</returns>
        public static Guid ToGuid(this string input)
        {
            Guid newGuid;
            Guid returnGuid = default(Guid);

            if (!string.IsNullOrWhiteSpace(input) && Guid.TryParse(input, out newGuid))
            {
                returnGuid = newGuid;
            }

            return returnGuid;
        }

        /// <summary>
        /// Validates a Brazilian CPF (Cadastro de Pessoa Física) and returns it if valid, empty string if invalid.
        /// </summary>
        /// <param name="cpf">The CPF string to validate</param>
        /// <returns>Valid CPF string without formatting or empty string if invalid</returns>
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

        /// <summary>
        /// Extracts first and last names from an email address by parsing the local part (before @).
        /// Handles separators like dot (.), underscore (_), and hyphen (-).
        /// </summary>
        /// <param name="email">The email address to parse</param>
        /// <returns>A tuple containing the first name and last name extracted from email</returns>
        public static (string first, string last) GetNameFromEmail(this string email)
        {
            var result = (string.Empty, string.Empty);
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
                result.Item1 = parts.First();
                result.Item2 = string.Empty;
            }

            if (result.Item1.Contains("."))
            {
                string[] names = result.Item1.Split('.');
                result.Item1 = names.First();
                result.Item2 = names.Last();
            }
            else if (result.Item1.Contains("_"))
            {
                string[] names = result.Item1.Split('_');
                result.Item1 = names.First();
                result.Item2 = names.Last();
            }
            else if (result.Item1.Contains("-"))
            {
                string[] names = result.Item1.Split('-');
                result.Item1 = names.First();
                result.Item2 = names.Last();
            }

            return result;
        }

        /// <summary>
        /// Extracts only numeric characters from a string.
        /// </summary>
        /// <param name="value">The string to filter</param>
        /// <returns>A string containing only digits, or the original value if null/empty</returns>
        public static string OnlyNumbers(this string value) => string.IsNullOrEmpty(value) ? value : string.Concat(value.Where(c => char.IsDigit(c)));

        /// <summary>
        /// Extracts only letter characters from a string.
        /// </summary>
        /// <param name="value">The string to filter</param>
        /// <returns>A string containing only letters, or the original value if null/empty</returns>
        public static string OnlyLetters(this string value) => string.IsNullOrEmpty(value) ? value : string.Concat(value.Where(c => char.IsLetter(c)));

        /// <summary>
        /// Converts a string to integer by extracting only numeric characters and parsing them.
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>An integer value from the numeric characters, or 0 if null/empty</returns>
        public static int ToInt(this string value) => string.IsNullOrEmpty(value) ? 0 : int.Parse(string.Concat(value.Where(c => char.IsDigit(c))));

        /// <summary>
        /// Formats a string containing CPF or CNPJ numbers with appropriate Brazilian formatting.
        /// CPF (11 digits): XXX.XXX.XXX-XX
        /// CNPJ (14 digits): XX.XXX.XXX/XXXX-XX
        /// </summary>
        /// <param name="value">The string containing CPF or CNPJ numbers to format</param>
        /// <returns>Formatted CPF/CNPJ string or original value if length doesn't match</returns>
        public static string FormatCpfCnpj(this string value)
        {
            var onlyNumbers = value
                .OnlyNumbers();

            if (onlyNumbers.Length == 11)
            {
                return Regex.Replace(
                    onlyNumbers,
                    @"(\d{3})(\d{3})(\d{3})(\d{2})",
                    @"$1.$2.$3-$4");
            }

            if (onlyNumbers.Length == 14)
            {
                return Regex.Replace(
                    onlyNumbers,
                    @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})",
                    @"$1.$2.$3/$4-$5");
            }

            return value;
        }

        /// <summary>
        /// Hides characters at the end of a string by replacing them with a specified character.
        /// Useful for masking sensitive information like passwords or credit card numbers.
        /// </summary>
        /// <param name="value">The string to hide characters from</param>
        /// <param name="amount">The number of characters to hide from the end (default: 3)</param>
        /// <param name="character">The character to use for masking (default: '*')</param>
        /// <returns>String with last characters replaced by mask character, or original if null/empty or length <= amount</returns>
        public static string HideCharacters(this string value, int amount = 3, char character = '*')
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.Length <= amount)
            {
                return value;
            }

            var sb = new StringBuilder();
            sb.Append(value.Substring(0, value.Length - amount));
            sb.Append(new string(character, amount));

            return sb.ToString();
        }

        /// <summary>
        /// Checks if a string is null, empty, or consists only of whitespace characters.
        /// </summary>
        /// <param name="cnpj">The CNPJ string to validate</param>
        /// <returns>True if the CNPJ is valid, false otherwise</returns>
        public static bool IsValidAlphanumericCnpj(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            // Remove máscara
            var clean = new string(cnpj.Where(char.IsLetterOrDigit).ToArray()).ToUpper();

            if (clean.Length != 14)
                return false;

            int[] pesos1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] pesos2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int Valor(char c) => char.IsDigit(c) ? c - '0' : c - 48;

            int CalcularDv(ReadOnlySpan<char> baseCnpj, int[] pesos)
            {
                var soma = 0;
                for (int i = 0; i < pesos.Length; i++)
                    soma += Valor(baseCnpj[i]) * pesos[i];

                var resto = soma % 11;
                return resto < 2 ? 0 : 11 - resto;
            }

            var dv1 = CalcularDv(clean.AsSpan(0, 12), pesos1);
            var dv2 = CalcularDv(clean.AsSpan(0, 13), pesos2);

            return clean[12] - '0' == dv1 && clean[13] - '0' == dv2;
        }

        /// <summary>
        /// Deserializes a JSON string into an object of type T. Returns default(T) if deserialization fails or if the input string is null/empty.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to</typeparam>
        /// <param name="jsonString">The JSON string to deserialize</param>
        /// <returns>An object of type T, or default(T) if deserialization fails</returns>
        public static T JsonStringToObject<T>(this string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return default(T);

            try
            {
                var obj = JsonSerializer.Deserialize<T>(jsonString);
                return obj;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Normalizes a raw URL string by trimming whitespace, unescaping characters, and validating the URL format.
        /// </summary>
        /// <param name="rawUrl">The raw URL string to normalize</param>
        /// <returns>A normalized URL string, or "#" if the URL is invalid</returns>
        private static string NormalizeActionUrl(this string rawUrl)
        {
            if (string.IsNullOrWhiteSpace(rawUrl))
            {
                return "#";
            }

            var url = rawUrl.Trim()
                .Replace("\\/", "/")
                .Replace("\\\"", "\"")
                .Replace("\\u0026", "&")
                .Replace("&amp;", "&")
                .Trim('"');

            while (url.EndsWith("\\", StringComparison.Ordinal))
            {
                url = url[..^1];
            }

            url = url.Replace("%22", string.Empty, StringComparison.OrdinalIgnoreCase);
            url = Uri.UnescapeDataString(url).Trim('"');

            if (Uri.TryCreate(url, UriKind.Absolute, out var parsed) &&
                (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps))
            {
                return parsed.ToString();
            }

            return "#";
        }
    }
}
