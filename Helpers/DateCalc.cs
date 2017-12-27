using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Microsoft.VisualBasic;

namespace Nigeria_Reg.Helpers
{
    public class DateCalc
    {
        public static int GetWeekNo(DateTime date)
        {

            var cultureInfo = CultureInfo.CurrentCulture;
            var calendar = cultureInfo.Calendar;

            var calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            var lastDayOfWeek = cultureInfo.LCID == 1033 //En-us
                                ? DayOfWeek.Saturday
                                : DayOfWeek.Sunday;

            var lastDayOfYear = new DateTime(date.Year, 12, 31);

            var weekNumber = calendar.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);

            //Check if this is the last week in the year and it doesn`t occupy the whole week
            return weekNumber == 53 && lastDayOfYear.DayOfWeek != lastDayOfWeek
                   ? 1
                   : weekNumber;
        }

        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;

            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }


        /// <summary>
        /// Returns the first day of the week that the specified
        /// date is in using the current culture. 
        /// </summary>
        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDateOfWeek(dayInWeek, defaultCultureInfo);
        }

        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        public static int FirstDayOfWeek(DateTime date)
        {
            DateTime dt = GetFirstDateOfWeek(date);
            return dt.Day;
        }

        public static int LastDayOfWeek(DateTime date)
        {
            DateTime dt = GetFirstDateOfWeek(date).AddDays(6);
            return dt.Day;
        }


        public static string GetMonthName(DateTime date)
        {
            return date.ToString("MMMM");
        }

        public static string GetMonthName(int number)
        {
            return number > 0 ? DateAndTime.MonthName(number) : "";
        }
    }
}