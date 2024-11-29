using System;

namespace JffCsharpTools.Dominio.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime StartWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static int Age(this DateTime dateBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateBirth.Year;

            if (dateBirth.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}