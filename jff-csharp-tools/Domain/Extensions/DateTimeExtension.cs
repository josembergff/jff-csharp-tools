using System;

namespace JffCsharpTools.Domain.Extensions
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
            var today = DateTime.Today.Date;
            var age = today.Year - dateBirth.Year;

            if (dateBirth.Date > today.AddYears(-age)) age--;

            return age;
        }

        public static int AgeOfRef(this DateTime dateBirth, DateTime dateRef)
        {
            var today = dateRef.Date;
            var age = today.Year - dateBirth.Year;

            if (dateBirth.Date > today.AddYears(-age)) age--;

            return age;
        }

        public static DateTime NextBusinessDay(this DateTime date)
        {
            var returnDate = DateTime.Now.Date;
            if (date > DateTime.MinValue)
                returnDate = date.Date;

            if (returnDate.DayOfWeek == DayOfWeek.Saturday)
                returnDate = returnDate.AddDays(2);
            else if (returnDate.DayOfWeek == DayOfWeek.Sunday)
                returnDate = returnDate.AddDays(1);

            return returnDate;
        }

        public static DateTime PreviousBusinessDay(this DateTime date)
        {
            var returnDate = DateTime.Now.Date;
            if (date > DateTime.MinValue)
                returnDate = date.Date;

            if (returnDate.DayOfWeek == DayOfWeek.Saturday)
                returnDate = returnDate.AddDays(-1);
            else if (returnDate.DayOfWeek == DayOfWeek.Sunday)
                returnDate = returnDate.AddDays(-2);

            return returnDate;
        }

        public static DateTime LastDayOfMonth(this DateTime date)
        {
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            var lastDayDate = new DateTime(date.Year, date.Month, lastDay);
            return lastDayDate;
        }

        public static DateTime LastBusinessDayOfMonth(this DateTime date)
        {
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            var lastDayDate = new DateTime(date.Year, date.Month, lastDay);
            return PreviousBusinessDay(lastDayDate);
        }
    }
}