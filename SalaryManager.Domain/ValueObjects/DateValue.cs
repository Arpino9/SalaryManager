using System;
using System.Globalization;
using FormatException = SalaryManager.Domain.Exceptions.FormatException;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 年表示
    /// </summary>
    /// <remarks>
    /// 西暦と和暦の変換用。
    /// </remarks>
    public sealed record class DateValue
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public DateValue(int year, int month)
            : this(new DateTime(year, month, 1))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dateTime">日付</param>
        public DateValue(DateTime dateTime)
        {
            if (dateTime.Year < 1970)
            {
                throw new FormatException("日付書式が不正です。");
            }

            if (dateTime.Month < 1 || 12 < dateTime.Month)
            {
                throw new FormatException("日付書式が不正です。");
            }
            this.Value = dateTime;
        }

        /// <summary> 日付 </summary>
        public readonly DateTime Value;

        /// <summary> 年 </summary>
        /// <remarks> ex) 2023年 </remarks>
        public string Text
            => (this.Value.Year.ToString() + "年");

        /// <summary>
        /// 和暦
        /// </summary>
        /// <remarks>
        /// ex) 令和5年
        /// </remarks>
        public string JapaneseCalendar
        {
            get
            {
                var Japanese = new CultureInfo("ja-JP");
                Japanese.DateTimeFormat.Calendar = new JapaneseCalendar();

                return (this.Value.ToString(Shared.YearFormat + "年", Japanese));
            }
        }

        /// <summary> 西暦 + 和暦 </summary>
        /// <remarks> ex) 2023年(令和5年) </remarks>
        public string YearWithJapaneseCalendar
            => (this.Text + " (" + this.JapaneseCalendar + ")");

        /// <summary> 長い曜日 </summary>
        public string Week_LongName 
            => this.Value.ToString("dddd");

        /// <summary> 短い曜日 </summary>
        public string Week_ShortName 
            => this.Value.ToString("ddd");

        /// <summary> 日付 </summary>
        /// <remarks> YYYYMMDD(月 or 火 or 水 or 木 or 金 or 土 or 日) </remarks>
        public string Date_YYYYMMDDWithWeekName 
            => string.Format($"{this.Value.Year}/{this.Value.Month}/{this.Value.Day}({this.Week_ShortName})");

        /// <summary> 日付 </summary>
        /// <remarks> MMDD(月 or 火 or 水 or 木 or 金 or 土 or 日) </remarks>
        public string Date_MMDDWithWeekName
            => string.Format($"{this.Value.Month}/{this.Value.Day}({this.Week_ShortName})");

        /// <summary> 月の最終日 </summary>
        public int LastMonthDay
            => DateTime.DaysInMonth(this.Value.Year, this.Value.Month);
    }
}
