using SalaryManager.Domain.ValueObjects;
using System;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 勤務備考
    /// </summary>
    public sealed class WorkingReferencesEntity
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="yearMonth">年月</param>
        /// <param name="OvertimeTime">時間外時間</param>
        /// <param name="weekendWorktime">休出時間</param>
        /// <param name="midnightWorktime">深夜時間</param>
        /// <param name="lateAbsentH">遅刻早退欠勤H</param>
        /// <param name="insurance">支給額-保険</param>
        /// <param name="norm">標準月額千円</param>
        /// <param name="numberOfDependent">扶養人数</param>
        /// <param name="paidVacation">有給残日数</param>
        /// <param name="workingHours">勤務時間</param>
        /// <param name="workingPlace">勤務先</param>
        /// <param name="remarks">備考</param>
        public WorkingReferencesEntity(
            int id,
            DateTime yearMonth,
            double OvertimeTime,
            double weekendWorktime,
            double midnightWorktime,
            double lateAbsentH,
            double insurance,
            double norm,
            double numberOfDependent,
            double paidVacation,
            double workingHours,
            string workingPlace,
            string remarks)
        {
            this.ID                = id;
            this.YearMonth         = yearMonth;
            this.OvertimeTime      = OvertimeTime;
            this.WeekendWorktime   = weekendWorktime;
            this.MidnightWorktime  = midnightWorktime;
            this.LateAbsentH       = lateAbsentH;
            this.Insurance         = new MoneyValue(insurance);
            this.Norm              = norm;
            this.NumberOfDependent = numberOfDependent;
            this.PaidVacation      = new PaidVacationDaysValue(paidVacation);
            this.WorkingHours      = workingHours;
            this.WorkPlace         = workingPlace;
            this.Remarks           = remarks;
        }

        /// <summary> ID </summary>
        public int ID { get; }

        /// <summary> 年月 </summary>
        public DateTime YearMonth { get; }

        /// <summary> 時間外時間 </summary>
        public double OvertimeTime { get; }

        /// <summary> 休出時間 </summary>
        public double WeekendWorktime { get; }

        /// <summary> 深夜時間 </summary>
        public double MidnightWorktime { get; }

        /// <summary> 遅刻早退欠勤H </summary>
        public double LateAbsentH { get; }

        /// <summary> 支給額-保険 </summary>
        public MoneyValue Insurance { get; }

        /// <summary> 標準月額千円 </summary>
        public double Norm { get; }

        /// <summary> 扶養人数 </summary>
        public double NumberOfDependent { get; }

        /// <summary> 有給残日数 </summary>
        public PaidVacationDaysValue PaidVacation { get; }

        /// <summary> 勤務時間 </summary>
        public double WorkingHours { get; }

        /// <summary> 勤務先 </summary>
        public string WorkPlace { get; set; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }
    }
}
