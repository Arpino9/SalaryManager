/// <summary>
/// Salary Manager
/// </summary>
namespace SalaryManager
{
    /// <summary>
    /// 支給額
    /// </summary>
    public class ItemAllowance :
        // 給与明細
        ItemPaySlipAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemAllowance()
        {
        }

        #region 基本給

        private int _BasicSalary;

        /// <summary>
        /// 基本給
        /// </summary>
        public int? BasicSalary
        {
            get
            {
                return this._BasicSalary;
            }
            set
            {
                this._BasicSalary = value ?? 0;
            }
        }

        #endregion

        #region 役職手当

        private int _ExecutiveAllowance;

        /// <summary>
        /// 役職手当
        /// </summary>
        public int? ExecutiveAllowance
        {
            get
            {
                return this._ExecutiveAllowance;
            }
            set
            {
                this._ExecutiveAllowance = value ?? 0;
            }
        }

        #endregion

        #region 扶養手当

        private int _DependencyAllowance;

        /// <summary>
        /// 扶養手当
        /// </summary>
        public int? DependencyAllowance
        {
            get
            {
                return this._DependencyAllowance;
            }
            set
            {
                this._DependencyAllowance = value ?? 0;
            }
        }

        #endregion

        #region 時間外手当

        private int _OvertimeAllowance;

        /// <summary>
        /// 時間外手当
        /// </summary>
        public int? OvertimeAllowance
        {
            get
            {
                return this._OvertimeAllowance;
            }
            set
            {
                this._OvertimeAllowance = value ?? 0;
            }
        }

        #endregion

        #region 休日割増

        private int _DaysoffIncreased;

        /// <summary>
        /// 休日割増
        /// </summary>
        public int? DaysoffIncreased
        {
            get
            {
                return this._DaysoffIncreased;
            }
            set
            {
                this._DaysoffIncreased = value ?? 0;
            }
        }

        #endregion

        #region 深夜割増

        private int _NightworkIncreased;

        /// <summary>
        /// 深夜割増
        /// </summary>
        public int? NightworkIncreased
        {
            get
            {
                return this._NightworkIncreased;
            }
            set
            {
                this._NightworkIncreased = value ?? 0;
            }
        }

        #endregion

        #region 住宅手当

        private int _HousingAllowance;

        /// <summary>
        /// 住宅手当
        /// </summary>
        public int? HousingAllowance
        {
            get
            {
                return this._HousingAllowance;
            }
            set
            {
                this._HousingAllowance = value ?? 0;
            }
        }

        #endregion

        #region 遅刻早退欠勤

        private int _LateAbsent;

        /// <summary>
        /// 遅刻早退欠勤
        /// </summary>
        public int? LateAbsent
        {
            get
            {
                return this._LateAbsent;
            }
            set
            {
                this._LateAbsent = value ?? 0;
            }
        }

        #endregion

        #region 交通費

        private int _TransportationExpenses;

        /// <summary>
        /// 交通費
        /// </summary>
        public int? TransportationExpenses
        {
            get
            {
                return this._TransportationExpenses;
            }
            set
            {
                this._TransportationExpenses = value ?? 0;
            }
        }

        #endregion

        #region 特別手当

        private int _SpecialAllowance;

        /// <summary>
        /// 特別手当
        /// </summary>
        public int? SpecialAllowance
        {
            get
            {
                return this._SpecialAllowance;
            }
            set
            {
                this._SpecialAllowance = value ?? 0;
            }
        }

        #endregion

        #region 予備

        private int _SpareAllowance;

        /// <summary>
        /// 予備
        /// </summary>
        public int? SpareAllowance
        {
            get
            {
                return this._SpareAllowance;
            }
            set
            {
                this._SpareAllowance = value ?? 0;
            }
        }

        #endregion

        #region 支給総計

        private int _TotalSalary;

        /// <summary>
        /// 支給総計
        /// </summary>
        public int? TotalSalary
        {
            get
            {
                return this._TotalSalary;
            }
            set
            {
                this._TotalSalary = value ?? 0;
            }
        }

        #endregion

        #region 差引支給額

        private int _TotalDeductedSalary;

        /// <summary>
        /// 差引支給額
        /// </summary>
        public int? TotalDeductedSalary
        {
            get
            {
                return this._TotalDeductedSalary;
            }
            set
            {
                this._TotalDeductedSalary = value ?? 0;
            }
        }

        #endregion

        #region Reset

        /// <summary>
        /// Reset
        /// </summary>
        public override void Reset()
        {
            // 基本給
            this.BasicSalary = 0;

            // 役職手当
            this.ExecutiveAllowance = 0;

            // 扶養手当
            this.DependencyAllowance = 0;

            // 時間外手当
            this.OvertimeAllowance = 0;

            // 休日手当
            this.DaysoffIncreased = 0;

            // 深夜割増
            this.NightworkIncreased = 0;

            // 住宅手当
            this.HousingAllowance = 0;

            // 遅刻早退欠勤
            this.LateAbsent = 0;

            // 交通費
            this.TransportationExpenses = 0;

            // 特別手当
            this.SpecialAllowance = 0;

            // 予備
            this.SpareAllowance = 0;

            // 支給総計
            this.TotalSalary = 0;

            // 差引支給額
            this.TotalDeductedSalary = 0;
        }

        #endregion

    }
}
