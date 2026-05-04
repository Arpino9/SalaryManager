namespace SalaryManager.Domain.Modules.Helpers;

/// <summary>
/// Helpers - 日付関連
/// </summary>
public static class DateUtils
{
    /// <summary> 今日 </summary>
    public static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);

    /// <param name="dateTime">日付</param>
    extension(DateOnly dateTime)
    {
        /// <summary>
        /// SQLiteの値に変換
        /// </summary>
        /// <returns>日付</returns>
        public string ConvertToSQLiteYearMonth()
            => dateTime.Year + "-" + dateTime.Month.ToString("D2") + "-" + "01";

        /// <summary>
        /// SQLiteの値に変換
        /// </summary>
        /// <returns>SQLite日付</returns>
        public string ConvertToSQLiteDate()
           => dateTime.Year + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2");
    }

    extension(DateTime dateTime)
    {
        /// <summary>
        /// SQLiteの値に変換
        /// </summary>
        /// <returns>SQLite日付</returns>
        public string ConvertToSQLiteDate()
           => dateTime.Year + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2");
    }
}
