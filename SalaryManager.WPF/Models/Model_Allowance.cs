using System;
using System.Windows.Media;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Repositories;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.XML;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 支給額
    /// </summary>
    public class Model_Allowance : IInputPayslip
    {

        #region Get Instance

        private static Model_Allowance model = null;

        public static Model_Allowance GetInstance(IAllowanceRepository repository)
        {
            if (model == null)
            {
                model = new Model_Allowance(repository);
            }

            return model;
        }

        #endregion

        /// <summary> Repository </summary>
        private IAllowanceRepository _repository;

        public Model_Allowance(IAllowanceRepository repository)
        {
            _repository = repository;
        }

        /// <summary> ViewModel - 支給額 </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary> ViewModel - 支給額 </summary>
        internal ViewModel_Allowance ViewModel { get; set; }

        /// <summary> ViewModel - 控除額 </summary>
        internal ViewModel_Deduction ViewModel_Deduction { get; set; }

        /// <summary> ViewModel - 勤務先 </summary>
        internal ViewModel_WorkPlace ViewModel_WorkPlace { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="entityDate">初期化する日付</param>
        /// <remarks>
        /// 画面起動時に、項目を初期化する。
        /// </remarks>
        public void Initialize(DateTime entityDate)
        {
            Allowances.Create(_repository);

            this.ViewModel.Entity          = Allowances.Fetch(entityDate.Year, entityDate.Month);
            this.ViewModel.Entity_LastYear = Allowances.Fetch(entityDate.Year, entityDate.Month - 1);

            var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

            if (this.ViewModel.Entity is null && showDefaultPayslip)
            {
                // デフォルト明細
                this.ViewModel.Entity = Allowances.FetchDefault();
            }
        }

        /// <summary>
        /// 再描画
        /// </summary>
        /// <remarks>
        /// 該当月に支給額と手当有無が存在すれば、各項目を再描画する。
        /// </remarks>
        public void Refresh()
        {
            var entity = this.ViewModel.Entity;

            if (entity is null)
            {
                this.Clear();
                return;
            }

            // 基本給
            this.ViewModel.BasicSalary_Text.Value              = entity.BasicSalary.Value;
            // 役職手当
            this.ViewModel.ExecutiveAllowance_Text.Value       = entity.ExecutiveAllowance.Value;
            // 扶養手当
            this.ViewModel.DependencyAllowance_Text.Value      = entity.DependencyAllowance.Value;
            // 時間外手当
            this.ViewModel.OvertimeAllowance_Text.Value        = entity.OvertimeAllowance.Value;
            // 休日割増
            this.ViewModel.DaysoffIncreased_Text.Value         = entity.DaysoffIncreased.Value;
            // 深夜割増
            this.ViewModel.NightworkIncreased_Text.Value       = entity.NightworkIncreased.Value;
            // 住宅手当
            this.ViewModel.HousingAllowance_Text.Value         = entity.HousingAllowance.Value;
            // 遅刻早退欠勤
            this.ViewModel.LateAbsent_Text.Value               = entity.LateAbsent;
            // 交通費
            this.ViewModel.TransportationExpenses_Text.Value   = entity.TransportationExpenses.Value;
            // 前払退職金
            this.ViewModel.PrepaidRetirementPayment_Text.Value = entity.PrepaidRetirementPayment.Value;
            // 在宅手当
            this.ViewModel.ElectricityAllowance_Text.Value     = entity.ElectricityAllowance.Value;
            // 特別手当
            this.ViewModel.SpecialAllowance_Text.Value         = entity.SpecialAllowance;
            // 予備
            this.ViewModel.SpareAllowance_Text.Value           = entity.SpareAllowance;
            // 備考
            this.ViewModel.Remarks_Text.Value                  = entity.Remarks;
            // 支給総計、差引支給額
            this.ReCaluculate();

            // 所属会社名
            Careers.Create(new CareerSQLite());
            var company = Careers.FetchCompany(new DateTime(this.Header.Year_Text.Value, this.Header.Month_Text.Value, 1));
            if (company is null)
            {
                return;
            }

            // 手当有無
            var existence = Careers.FetchAllowanceExistence(new CompanyNameValue(company));
            if (existence != null) 
            {
                this.ViewModel.ExecutiveAllowance_IsEnabled.Value       = existence.Executive.Value;
                this.ViewModel.DependencyAllowance_IsEnabled.Value      = existence.Dependency.Value;
                this.ViewModel.OvertimeAllowance_IsEnabled.Value        = existence.Overtime.Value;
                this.ViewModel.NightworkIncreased_IsEnabled.Value       = existence.LateNight.Value;
                this.ViewModel.HousingAllowance_IsEnabled.Value         = existence.Housing.Value;
                this.ViewModel.TransportationExpenses_IsEnabled.Value   = existence.Commution.Value;
                this.ViewModel.PrepaidRetirementPayment_IsEnabled.Value = existence.PrepaidRetirement.Value;
                this.ViewModel.ElectricityAllowance_IsEnabled.Value     = existence.Electricity.Value;
                this.ViewModel.SpecialAllowance_IsEnabled.Value         = existence.Special.Value;
            }
        }

        /// <summary>
        /// リロード
        /// </summary>
        /// <remarks>
        /// 年月の変更時などに、該当月の項目を取得する。
        /// </remarks>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                Allowances.Create(_repository);

                this.ViewModel.Entity          = Allowances.Fetch(this.Header.Year_Text.Value,     this.Header.Month_Text.Value);
                this.ViewModel.Entity_LastYear = Allowances.Fetch(this.Header.Year_Text.Value - 1, this.Header.Month_Text.Value);

                this.Refresh();
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        /// <remarks>
        /// 各項目を初期化する。
        /// </remarks>
        public void Clear()
        {
            // 基本給
            this.ViewModel.BasicSalary_Text.Value = default(double);

            // 役職手当
            this.ViewModel.ExecutiveAllowance_Text.Value      = default(double);
            this.ViewModel.ExecutiveAllowance_IsEnabled.Value = true;

            // 扶養手当
            this.ViewModel.DependencyAllowance_Text.Value      = default(double);
            this.ViewModel.DependencyAllowance_IsEnabled.Value = true;

            // 時間外手当
            this.ViewModel.OvertimeAllowance_Text.Value      = default(double);
            this.ViewModel.OvertimeAllowance_IsEnabled.Value = true;

            // 休日割増
            this.ViewModel.DaysoffIncreased_Text.Value           = default(double);

            // 深夜割増
            this.ViewModel.NightworkIncreased_Text.Value      = default(double);
            this.ViewModel.NightworkIncreased_IsEnabled.Value = true;

            // 住宅手当
            this.ViewModel.HousingAllowance_Text.Value      = default(double);
            this.ViewModel.HousingAllowance_IsEnabled.Value = true;

            // 遅刻早退欠勤
            this.ViewModel.LateAbsent_Text.Value = default(double);

            // 交通費
            this.ViewModel.TransportationExpenses_Text.Value      = default(double);
            this.ViewModel.TransportationExpenses_IsEnabled.Value = true;

            // 前払退職金
            this.ViewModel.PrepaidRetirementPayment_Text.Value      = default(double);
            this.ViewModel.PrepaidRetirementPayment_IsEnabled.Value = true;

            // 在宅手当
            this.ViewModel.ElectricityAllowance_Text.Value      = default(double);
            this.ViewModel.ElectricityAllowance_IsEnabled.Value = true;

            // 特別手当
            this.ViewModel.SpecialAllowance_Text.Value      = default(double);
            this.ViewModel.SpecialAllowance_IsEnabled.Value = true;

            // 予備
            this.ViewModel.SpareAllowance_Text.Value = default(double);

            // 備考
            this.ViewModel.Remarks_Text.Value = default(string);

            // 支給総計
            this.ViewModel.TotalSalary_Text.Value = default(double);

            // 差引支給額
            this.ViewModel.TotalDeductedSalary_Foreground.Value = new SolidColorBrush(Colors.Black);
            this.ViewModel.TotalDeductedSalary_Text.Value       = default(double);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <remarks>
        /// SQLiteに接続し、入力項目を保存する。
        /// </remarks>
        public void Save(ITransactionRepository transaction)
        {
            var entity = new AllowanceValueEntity(
                              this.Header.ID,
                              this.Header.YearMonth,
                              this.ViewModel.BasicSalary_Text.Value,
                              this.ViewModel.ExecutiveAllowance_Text.Value,
                              this.ViewModel.DependencyAllowance_Text.Value,
                              this.ViewModel.DependencyAllowance_Text.Value,
                              this.ViewModel.DaysoffIncreased_Text.Value,
                              this.ViewModel.NightworkIncreased_Text.Value,
                              this.ViewModel.HousingAllowance_Text.Value,
                              this.ViewModel.LateAbsent_Text.Value,
                              this.ViewModel.TransportationExpenses_Text.Value,
                              this.ViewModel.PrepaidRetirementPayment_Text.Value,
                              this.ViewModel.ElectricityAllowance_Text.Value,
                              this.ViewModel.SpecialAllowance_Text.Value,
                              this.ViewModel.SpareAllowance_Text.Value,
                              this.ViewModel.Remarks_Text.Value,
                              this.ViewModel.TotalSalary_Text.Value,
                              this.ViewModel.TotalDeductedSalary_Text.Value);

            _repository.Save(transaction, entity);
        }

        /// <summary>
        /// 再計算
        /// </summary>
        /// <remarks>
        /// 該当項目の変更時に、支給総計と差引支給額を再計算する。
        /// </remarks>
        internal void ReCaluculate()
        {
            if (this.ViewModel is null)
            {
                return;
            }

            this.ViewModel.TotalSalary_Text.Value = this.ViewModel.BasicSalary_Text.Value
                                                  + this.ViewModel.ExecutiveAllowance_Text.Value
                                                  + this.ViewModel.DependencyAllowance_Text.Value
                                                  + this.ViewModel.DependencyAllowance_Text.Value
                                                  + this.ViewModel.DaysoffIncreased_Text.Value
                                                  + this.ViewModel.NightworkIncreased_Text.Value
                                                  + this.ViewModel.ElectricityAllowance_Text.Value
                                                  + this.ViewModel.LateAbsent_Text.Value
                                                  + this.ViewModel.SpecialAllowance_Text.Value
                                                  + this.ViewModel.SpareAllowance_Text.Value
                                                  + this.ViewModel.TransportationExpenses_Text.Value
                                                  + this.ViewModel.PrepaidRetirementPayment_Text.Value;

            if (this.ViewModel_Deduction is null)
            {
                return;
            }

            this.ViewModel.TotalDeductedSalary_Text.Value = this.ViewModel.TotalSalary_Text.Value 
                                                          - this.ViewModel_Deduction.TotalDeduct_Text.Value;

            this.ChangeColor();
        }
        
        /// <summary>
        /// 文字色変更
        /// </summary>
        /// <remarks>
        /// 差引支給額の値の正負によって、文字色を変更する。
        /// </remarks>
        internal void ChangeColor()
        {
            var foreground = this.ViewModel.TotalDeductedSalary_Foreground;

            foreground.Value = this.ViewModel.TotalDeductedSalary_Text.Value >= 0 ?
                               new SolidColorBrush(Colors.Blue) : new SolidColorBrush(Colors.Red);
        }
    }
}
