using System;

namespace JffCsharpTools.Domain.Extensions
{
    /// <summary>
    /// Extension methods for DateTime to provide additional date manipulation functionality
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Gets the first day of the week for a given date based on the specified start of week
        /// </summary>
        /// <param name="dt">The date to find the start of week for</param>
        /// <param name="startOfWeek">The day that represents the start of the week</param>
        /// <returns>The DateTime representing the start of the week</returns>
        public static DateTime StartWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Calculates the age in years from a birth date to today's date
        /// </summary>
        /// <param name="dateBirth">The birth date</param>
        /// <returns>The age in complete years</returns>
        public static int Age(this DateTime dateBirth)
        {
            var today = DateTime.Today.Date;
            var age = today.Year - dateBirth.Year;

            if (dateBirth.Date > today.AddYears(-age)) age--;

            return age;
        }

        /// <summary>
        /// Calculates the age in years from a birth date to a specific reference date
        /// </summary>
        /// <param name="dateBirth">The birth date</param>
        /// <param name="dateRef">The reference date to calculate age from</param>
        /// <returns>The age in complete years as of the reference date</returns>
        public static int AgeOfRef(this DateTime dateBirth, DateTime dateRef)
        {
            var today = dateRef.Date;
            var age = today.Year - dateBirth.Year;

            if (dateBirth.Date > today.AddYears(-age)) age--;

            return age;
        }

        /// <summary>
        /// Gets the next business day (Monday-Friday) from the given date
        /// If the date falls on a weekend, returns the following Monday
        /// </summary>
        /// <param name="date">The date to find the next business day from</param>
        /// <returns>The next business day</returns>
        public static DateTime NextBusinessDay(this DateTime date)
        {
            var returnDate = DateTime.UtcNow.Date;
            if (date > DateTime.MinValue)
                returnDate = date.Date;

            if (returnDate.DayOfWeek == DayOfWeek.Saturday)
                returnDate = returnDate.AddDays(2);
            else if (returnDate.DayOfWeek == DayOfWeek.Sunday)
                returnDate = returnDate.AddDays(1);

            return returnDate;
        }

        /// <summary>
        /// Gets the previous business day (Monday-Friday) from the given date
        /// If the date falls on a weekend, returns the previous Friday
        /// </summary>
        /// <param name="date">The date to find the previous business day from</param>
        /// <returns>The previous business day</returns>
        public static DateTime PreviousBusinessDay(this DateTime date)
        {
            var returnDate = DateTime.UtcNow.Date;
            if (date > DateTime.MinValue)
                returnDate = date.Date;

            if (returnDate.DayOfWeek == DayOfWeek.Saturday)
                returnDate = returnDate.AddDays(-1);
            else if (returnDate.DayOfWeek == DayOfWeek.Sunday)
                returnDate = returnDate.AddDays(-2);

            return returnDate;
        }

        /// <summary>
        /// Gets the last day of the month for the given date
        /// </summary>
        /// <param name="date">The date to find the last day of the month for</param>
        /// <returns>The last day of the month</returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            var lastDayDate = new DateTime(date.Year, date.Month, lastDay);
            return lastDayDate;
        }

        /// <summary>
        /// Gets the last business day of the month for the given date
        /// If the last day falls on a weekend, returns the previous Friday
        /// </summary>
        /// <param name="date">The date to find the last business day of the month for</param>
        /// <returns>The last business day of the month</returns>
        public static DateTime LastBusinessDayOfMonth(this DateTime date)
        {
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            var lastDayDate = new DateTime(date.Year, date.Month, lastDay);
            return PreviousBusinessDay(lastDayDate);
        }
    }
}