using System;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 勤務日
    /// </summary>
    public class WorkingDateValue : ValueObject<WorkingDateValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">勤務日</param>
        public WorkingDateValue(DateTime value)
        {
            this.Value = value;
        }

        /// <summary> 値 </summary>
        public readonly DateTime Value;

        /// <summary> 不明 </summary>
        public static readonly WorkingDateValue Unknown = new WorkingDateValue(DateTime.MinValue);

        /// <summary> 就業中 </summary>
        public static readonly WorkingDateValue Working = new WorkingDateValue(DateTime.MaxValue);

        /// <summary>
        /// 不明か
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return (this.Value.ToString("yyyy/MM/dd") == WorkingDateValue.Unknown.Value.ToString("yyyy/MM/dd"));
            }
        }

        /// <summary>
        /// 就業中か
        /// </summary>
        public bool IsWorking
        {
            get
            {
                return (this.Value.ToString("yyyy/MM/dd") == WorkingDateValue.Working.Value.ToString("yyyy/MM/dd"));
            }
        }

        public override string ToString()
        {
            return (this.IsWorking || this == Working ? "就業中" : this.Value.ToString("yyyy/MM/dd"));
        }

        protected override bool EqualsCore(WorkingDateValue other)
        {
            return (this.Value == other.Value);
        }
    }
}
