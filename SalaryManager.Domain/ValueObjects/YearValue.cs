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
    public sealed class YearValue : ValueObject<YearValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public YearValue(int year, int month)
            : this(new DateTime(year, month, 1))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dateTime">日付</param>
        public YearValue(DateTime dateTime)
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

        /// <summary>
        /// 日付
        /// </summary>
        public readonly DateTime Value;

        /// <summary>
        /// 年
        /// </summary>
        /// <remarks>
        /// ex) 2023年
        /// </remarks>
        public string Year
        {
            get
            {
                return (this.Value.Year.ToString() + "年");
            }
        }

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

        /// <summary>
        /// 西暦 + 和暦
        /// </summary>
        /// <remarks>
        /// ex) 2023年(令和5年)
        /// </remarks>
        public string YearWithJapaneseCalendar
        {
            get
            {
                return this.Year + " (" + this.JapaneseCalendar + ")";
            }
        }

        /// <summary>
        /// Equals Core
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override bool EqualsCore(YearValue other)
        {
            return (this.Value == other.Value);
        }
    }
}
