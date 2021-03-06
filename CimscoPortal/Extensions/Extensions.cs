﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Extensions
{
    public static class Extensions
    {
        public static int RoundOff(this int i)
        {
            return ((int)Math.Round(i / 10.0)) * 10;
        }

        //public static int RoundOff_(this Decimal i)
        //{
        //    return ((int)Math.Round(i / 10.0)) * 10;
        //}

        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }

        public static List<int> IntegersFromString(this string input, char delim)
        {
            string[] _stringValues = input.Split(delim);
            List<int> _result = new List<int>();
            try
            {
                foreach (string _value in _stringValues)
                {
                    _result.Add(Int32.Parse(_value));
                }
            }
            catch { }

            return _result;
        }

        //public static bool IsNullOrValue(this int? value, int valueToCheck)
        //{
        //    return (value ?? valueToCheck) == valueToCheck;
        //}
    }

    public static class DateExtensions
    {
        public static DateTime EndOfTheMonth(this DateTime date)
        {
            var endOfTheMonth = new DateTime(date.Year, date.Month, 1)
                .AddMonths(1)
                .AddDays(-1);

            return endOfTheMonth;
        }

        public static DateTime EndOfLastMonth(this DateTime date)
        {
            var endOfLastMonth = new DateTime(date.Year, date.Month, 1)
                .AddDays(-1);

            return endOfLastMonth;
        }

        public static DateTime StartOfThisMonth(this DateTime date)
        {
            var startOfThisMonth = new DateTime(date.Year, date.Month, 1);

            return startOfThisMonth;
        }
    }

    public static class NumericExtensions
    {
        static public decimal SafeDivision(this decimal Numerator, decimal Denominator)
        {
            return (Denominator == 0) ? 0 : Numerator / Denominator;
        }
    }

    public static class StringExtensions
    {
        //public static string[] ToStringArray(this string values)
        //{

        //    return new string[] { "test", "test2" };
        //}

        public static string RemoveEmptyRecord(this string value, string emptyText)
        {
                return (value == emptyText) ? "" : value;
        }
    }
}
