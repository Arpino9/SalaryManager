using System;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 時刻
    /// </summary>
    public sealed class TimeValue : ValueObject<TimeValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hour">時</param>
        /// <param name="minute">分</param>
        /// <exception cref="FormatException">書式が不正</exception>
        public TimeValue(int hour, int minute)
        {
            if (hour < 0 || 23 < hour)
            {
                throw new FormatException("時刻(時)が不正です。");
            }

            if (minute < 0 || 59 < minute)
            {
                throw new FormatException("時刻(分)が不正です。");
            }

            this.Hour = hour;
            this.Minute = minute;
        }

        /// <summary> 時 </summary>
        public readonly int Hour;

        /// <summary> 分 </summary>
        public readonly int Minute;

        public override string ToString()
        {
            return (string.Format($"{this.Hour:00}:{this.Minute:00}"));
        }

        protected override bool EqualsCore(TimeValue other)
        {
            return (this.Hour == other.Hour &&
                    this.Minute == other.Minute);
        }
    }
}
