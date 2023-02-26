using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using System;
using System.Linq;
using System.Windows.Media;

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

        /// <summary> ViewModel - 控除額 </summary>
        internal ViewModel_Deduction ViewModel_Deduction { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="entityDate">初期化する日付</param>
        public void Initialize(DateTime entityDate)
        {
            var sqlite = new AllowanceSQLite();
            var records = sqlite.GetEntities();

            this.ViewModel.Entity = records.Where(record => record.YearMonth.Year  == entityDate.Year
                                                         && record.YearMonth.Month == entityDate.Month)
                                           .FirstOrDefault();

            this.ViewModel.Entity_LastYear = records.Where(record => record.YearMonth.Year  == entityDate.Year - 1
                                                                  && record.YearMonth.Month == entityDate.Month)
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
            var entity = this.ViewModel.Entity;

            if (entity is null)
            {
                this.Clear();
                return;
            }

            // 基本給
            this.ViewModel.BasicSalary            = entity.BasicSalary.Value;
            // 役職手当
            this.ViewModel.ExecutiveAllowance     = entity.ExecutiveAllowance.Value;
            // 扶養手当
            this.ViewModel.DependencyAllowance    = entity.DependencyAllowance.Value;
            // 時間外手当
            this.ViewModel.OvertimeAllowance      = entity.OvertimeAllowance.Value;
            // 休日割増
            this.ViewModel.DaysoffIncreased       = entity.DaysoffIncreased.Value;
            // 深夜割増
            this.ViewModel.NightworkIncreased     = entity.NightworkIncreased.Value;
            // 住宅手当
            this.ViewModel.HousingAllowance       = entity.HousingAllowance.Value;
            // 遅刻早退欠勤
            this.ViewModel.LateAbsent             = entity.LateAbsent;
            // 交通費
            this.ViewModel.TransportationExpenses = entity.TransportationExpenses.Value;
            // 特別手当
            this.ViewModel.SpecialAllowance       = entity.SpecialAllowance.Value;
            // 予備
            this.ViewModel.SpareAllowance         = entity.SpareAllowance.Value;
            // 支給総計
            this.ViewModel.TotalSalary            = entity.TotalSalary.Value;
            // 差引支給額
            this.ViewModel.TotalDeductedSalary    = entity.TotalDeductedSalary;
            this.ChangeColor();
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
            this.ViewModel.TotalDeductedSalary_Foreground = new SolidColorBrush(Colors.Black);
            this.ViewModel.TotalDeductedSalary    = default(double);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public void Save(SQLiteTransaction transaction)
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
            allowance.Save(transaction, entity);
        }

        /// <summary>
        /// 再計算
        /// </summary>
        /// <remarks>
        /// 支給総計を再計算する。
        /// </remarks>
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
                                       + this.ViewModel.LateAbsent
                                       + this.ViewModel.SpecialAllowance
                                       + this.ViewModel.SpareAllowance;

            if (this.ViewModel_Deduction is null)
            {
                return;
            }

            this.ViewModel.TotalDeductedSalary = this.ViewModel.TotalSalary 
                                               - this.ViewModel_Deduction.TotalDeduct;

            this.ChangeColor();
        }
        
        /// <summary>
        /// 色変更
        /// </summary>
        internal void ChangeColor()
        {
            // 差引支給額
            if (this.ViewModel.TotalDeductedSalary >= 0)
            {
                this.ViewModel.TotalDeductedSalary_Foreground = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                this.ViewModel.TotalDeductedSalary_Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
