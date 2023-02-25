using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Interface;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using System;
using System.Linq;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 支給額
    /// </summary>
    public class Model_Allowance : IInputPayroll
    {

        #region Get Instance

        private static Model_Allowance model = null;

        public static Model_Allowance GetInstance()
        {
            if (model == null)
            {
                model = new Model_Allowance();
            }

            return model;
        }

        #endregion

        /// <summary> ViewModel - ヘッダ </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary> ViewModel - 支給額 </summary>
        internal ViewModel_Allowance ViewModel { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            var sqlite = new AllowanceSQLite();
            var records = sqlite.GetEntities();

            this.ViewModel.Entity = records.Where(record => record.YearMonth.Year  == DateTime.Today.Year
                                               && record.YearMonth.Month == DateTime.Today.Month)
                                 .FirstOrDefault();

            this.ViewModel.Entity_LastYear = records.Where(record => record.YearMonth.Year  == DateTime.Today.Year - 1
                                                        && record.YearMonth.Month == DateTime.Today.Month)
                                 .FirstOrDefault();

            if (this.ViewModel.Entity is null)
            {
                // レコードなし
                var header = new HeaderSQLite();
                var defaultEntity = header.GetDefaultEntity();

                if (defaultEntity != null)
                {
                    this.ViewModel.Entity = records.Where(record => record.ID == defaultEntity.ID)
                                         .FirstOrDefault();
                }
            }

            this.Refresh();
        }

        /// <summary>
        /// 再描画
        /// </summary>
        public void Refresh()
        {
            if (this.ViewModel.Entity is null)
            {
                this.Clear();
                return;
            }

            // 基本給
            this.ViewModel.BasicSalary            = this.ViewModel.Entity.BasicSalary;
            // 役職手当
            this.ViewModel.ExecutiveAllowance     = this.ViewModel.Entity.ExecutiveAllowance;
            // 扶養手当
            this.ViewModel.DependencyAllowance    = this.ViewModel.Entity.DependencyAllowance;
            // 時間外手当
            this.ViewModel.OvertimeAllowance      = this.ViewModel.Entity.OvertimeAllowance;
            // 休日割増
            this.ViewModel.DaysoffIncreased       = this.ViewModel.Entity.DaysoffIncreased;
            // 深夜割増
            this.ViewModel.NightworkIncreased     = this.ViewModel.Entity.NightworkIncreased;
            // 住宅手当
            this.ViewModel.HousingAllowance       = this.ViewModel.Entity.HousingAllowance;
            // 遅刻早退欠勤
            this.ViewModel.LateAbsent             = this.ViewModel.Entity.LateAbsent;
            // 交通費
            this.ViewModel.TransportationExpenses = this.ViewModel.Entity.TransportationExpenses;
            // 特別手当
            this.ViewModel.SpecialAllowance       = this.ViewModel.Entity.SpecialAllowance;
            // 予備
            this.ViewModel.SpareAllowance         = this.ViewModel.Entity.SpareAllowance;
            // 支給総計
            this.ViewModel.TotalSalary            = this.ViewModel.Entity.TotalSalary;
            // 差引支給額
            this.ViewModel.TotalDeductedSalary    = this.ViewModel.Entity.TotalDeductedSalary;
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            var allowanceTable = new AllowanceSQLite();

            this.ViewModel.Entity          = allowanceTable.GetEntiity(this.Header.Year,     this.Header.Month);
            this.ViewModel.Entity_LastYear = allowanceTable.GetEntiity(this.Header.Year - 1, this.Header.Month);

            this.Refresh();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 基本給
            this.ViewModel.BasicSalary            = default(double);
            // 役職手当
            this.ViewModel.ExecutiveAllowance     = default(double);
            // 扶養手当
            this.ViewModel.DependencyAllowance    = default(double);
            // 時間外手当
            this.ViewModel.OvertimeAllowance      = default(double);
            // 休日割増
            this.ViewModel.DaysoffIncreased       = default(double);
            // 深夜割増
            this.ViewModel.NightworkIncreased     = default(double);
            // 住宅手当
            this.ViewModel.HousingAllowance       = default(double);
            // 遅刻早退欠勤
            this.ViewModel.LateAbsent             = default(double);
            // 交通費
            this.ViewModel.TransportationExpenses = default(double);
            // 特別手当
            this.ViewModel.SpecialAllowance       = default(double);
            // 予備
            this.ViewModel.SpareAllowance         = default(double);
            // 支給総計
            this.ViewModel.TotalSalary            = default(double);
            // 差引支給額
            this.ViewModel.TotalDeductedSalary    = default(double);
        }

        /// <summary>
        /// 登録
        /// </summary>
        public void Register()
        {
            var entity = new AllowanceEntity(
                              this.Header.ID,
                              this.Header.YearMonth,
                              this.ViewModel.BasicSalary,
                              this.ViewModel.ExecutiveAllowance,
                              this.ViewModel.DependencyAllowance,
                              this.ViewModel.OvertimeAllowance,
                              this.ViewModel.DaysoffIncreased,
                              this.ViewModel.NightworkIncreased,
                              this.ViewModel.HousingAllowance,
                              this.ViewModel.LateAbsent,
                              this.ViewModel.TransportationExpenses,
                              this.ViewModel.SpecialAllowance,
                              this.ViewModel.SpareAllowance,
                              this.ViewModel.Remarks,
                              this.ViewModel.TotalSalary,
                              this.ViewModel.TotalDeductedSalary);

            var allowance = new AllowanceSQLite();
            allowance.Save(entity);
        }

        /// <summary>
        /// 再計算
        /// </summary>
        internal void ReCaluculate()
        {
            if (this.ViewModel is null)
            {
                return;
            }

            this.ViewModel.TotalSalary = this.ViewModel.BasicSalary
                                       + this.ViewModel.ExecutiveAllowance
                                       + this.ViewModel.DependencyAllowance
                                       + this.ViewModel.OvertimeAllowance
                                       + this.ViewModel.DaysoffIncreased
                                       + this.ViewModel.NightworkIncreased
                                       + this.ViewModel.HousingAllowance
                                       + this.ViewModel.SpecialAllowance
                                       + this.ViewModel.SpareAllowance;
        }
    }
}
