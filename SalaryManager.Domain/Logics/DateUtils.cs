﻿using System;

namespace SalaryManager.Domain.Logics
{
    /// <summary>
    /// Utility - 日付
    /// </summary>
    public class DateUtils
    {
        /// <summary>
        /// SQLiteの値に変換
        /// </summary>
        /// <param name="dateTime">日付</param>
        /// <returns>日付</returns>
        public static string ConvertToSQLiteValue(DateTime dateTime)
        {
            var year  = dateTime.Year;
            var month = dateTime.Month;

            var date = year + "-" + month.ToString("D2") + "-" + "01";

            return date;
        }
    }
}
