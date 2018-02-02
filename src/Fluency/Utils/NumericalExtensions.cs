using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluency.Utils
{
    public static class NumericalExtensions
    {
        public static DateTime YearsAgo(this int years)
        {
            return DateTime.Now.AddYears(years * -1);
        }


        public static DateTime MonthsAgo(this int months)
        {
            return DateTime.Now.AddMonths(months * -1);
        }


        public static DateTime DaysAgo(this int days)
        {
            return DateTime.Now.AddDays(days * -1);
        }


        public static DateTime YearsFromNow(this int years)
        {
            return DateTime.Now.AddYears(years);
        }


        public static DateTime MonthsFromNow(this int months)
        {
            return DateTime.Now.AddMonths(months);
        }


        public static DateTime DaysFromNow(this int days)
        {
            return DateTime.Now.AddDays(days);
        }


        public static double Cents(this int cents)
        {
            double value = (double)cents / 100;
            return Math.Round(value, 2);
        }


        public static double Percent(this int percent)
        {
            double value = (double)percent / 100;
            return Math.Round(value, 2);
        }


        public static double dollars(this int dollars)
        {
            return dollars;
        }

        public static IEnumerable<int> Times(this int times)
        {
            return Enumerable.Range(0, times);
        }

        /// <summary>
        /// Repeats the given function the specified number of times returning
        /// an IEnumerable containing the specified count of return values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count">The count.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static IEnumerable<T> Of<T>(this int count, Func<T> func)
        {
            for (int i = 0; i < count; i++)
            {
                yield return func.Invoke();
            }
        }
    }
}