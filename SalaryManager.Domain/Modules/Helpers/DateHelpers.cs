using System;

namespace SalaryManager.Domain.Modules.Helpers
{
    //TODO: これは値オブジェクトでいい
    /// <summary>
    /// Helpers - 日付関連
    /// </summary>
    public sealed class DateHelpers
    {
        /// <summary>
        /// SQLiteの値に変換
        /// </summary>
        /// <param name="dateTime">日付</param>
        /// <returns>日付</returns>
        public static string ConvertToSQLiteValue(DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month;

            var date = year + "-" + month.ToString("D2") + "-" + "01";

            return date;
        }

        /// <summary>
        /// SQLiteの値に変換
        /// </summary>
        /// <param name="dateTime">日付</param>
        /// <returns>日付</returns>
        public static string ConvertToSQLiteValue(int year, int month, int day)
        {
            var date = year + "-" + month.ToString("D2") + "-" + day.ToString("D2");

            return date;
        }
    }
}
