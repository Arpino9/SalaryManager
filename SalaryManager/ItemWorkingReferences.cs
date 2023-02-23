/// <summary>
/// Salary Manager
/// </summary>
namespace SalaryManager
{
    /// <summary>
    /// 勤務備考
    /// </summary>
    public class ItemWorkingReferences : 
        // 給与明細
        ItemPaySlipAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemWorkingReferences()
        {
        }

        #region 時間外時間

        private double _OvertimeTime;

        /// <summary>
        /// 時間外時間
        /// </summary>
        public double? OvertimeTime
        {
            get
            {
                return this._OvertimeTime;
            }
            set
            {
                this._OvertimeTime = value ?? 0.0;
            }
        }

        #endregion

        #region 休出時間

        private double _WeekendWorktime;

        /// <summary>
        /// 休出時間
        /// </summary>
        public double? WeekendWorktime
        {
            get
            {
                return this._WeekendWorktime;
            }
            set
            {
                this._WeekendWorktime = value ?? 0.0;
            }
        }

        #endregion

        #region 深夜時間

        private double _MidnightWorktime;

        /// <summary>
        /// 深夜時間
        /// </summary>
        public double? MidnightWorktime
        {
            get
            {
                return this._MidnightWorktime;
            }
            set
            {
                this._MidnightWorktime = value ?? 0.0;
            }
        }

        #endregion

        #region 遅刻早退欠勤H

        private int _LateAbsentH;

        /// <summary>
        /// 遅刻早退欠勤H
        /// </summary>
        public int? LateAbsentH
        {
            get
            {
                return this._LateAbsentH;
            }
            set
            {
                this._LateAbsentH = value ?? 0;
            }
        }

        #endregion

        #region 支給額-保険

        private int _Insurance;

        /// <summary>
        /// 支給額-保険
        /// </summary>
        public int? Insurance
        {
            get
            {
                return this._Insurance;
            }
            set
            {
                this._Insurance = value ?? 0;
            }
        }

        #endregion

        #region 扶養人数

        private int _NumberOfDependent;

        /// <summary>
        /// 扶養人数
        /// </summary>
        public int? NumberOfDependent
        {
            get
            {
                return this._NumberOfDependent;
            }
            set
            {
                this._NumberOfDependent = value ?? 0;
            }
        }

        #endregion

        #region 有給残日数

        private int _PaidVacation;

        /// <summary>
        /// 有給残日数
        /// </summary>
        public int? PaidVacation
        {
            get
            {
                return this._PaidVacation;
            }
            set
            {
                this._PaidVacation = value ?? 0;
            }
        }

        #endregion

        #region 勤務時間

        private double _WorkingHours;

        /// <summary>
        /// 勤務時間
        /// </summary>
        public double? WorkingHours
        {
            get
            {
                return this._WorkingHours;
            }
            set
            {
                this._WorkingHours = value ?? 0.0;
            }
        }

        #endregion

        #region 勤務先

        private string _Workplace;

        /// <summary>
        /// 勤務先
        /// </summary>
        public string Workplace
        {
            get
            {
                return this._Workplace;
            }
            set
            {
                this._Workplace = value ?? string.Empty;
            }
        }

        #endregion

        #region Reset

        /// <summary>
        /// Reset
        /// </summary>
        public override void Reset()
        {
            // 時間外時間
            this.OvertimeTime = 0.0;

            // 休出時間
            this.WeekendWorktime = 0.0;

            // 深夜時間
            this.OvertimeTime = 0.0;

            // 遅刻早退欠勤H
            this.LateAbsentH = 0;

            // 支給額-保険
            this.Insurance = 0;

            // 扶養人数
            this.NumberOfDependent = 0;

            // 有給残日数
            this.PaidVacation = 0;

            // 勤務時間
            this.WorkingHours = 0.0;

            // 勤務先
            this.Workplace = string.Empty;
        }

        #endregion

    }
}
