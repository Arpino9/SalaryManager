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
        private readonly DateTime Value;

        /// <summary>
        /// 就業中か
        /// </summary>
        public bool IsWorking
        {
            get
            {
                return (this.Value == DateTime.MaxValue);
            }
        }

        public override string ToString()
        {
            return (this.IsWorking ? "就業中" : this.Value.ToString("yyyy/MM/dd"));
        }

        protected override bool EqualsCore(WorkingDateValue other)
        {
            return (this.Value == other.Value);
        }
    }
}
