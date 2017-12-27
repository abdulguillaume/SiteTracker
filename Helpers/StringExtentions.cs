using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nigeria_Reg.Helpers
{
    public static class StringExtensions
    {
        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }

        public static string Left(this string str, int length)
        {
            return str.Substring(0, length);
        }

        public static string InitialCapital(this string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1).ToLower();
        }
    }
}