using System;
using System.ComponentModel;
using System.Windows.Media;
using Reactive.Bindings;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 支給額
    /// </summary>
    public class ViewModel_Allowance : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel_Allowance()
        {
            this.MainWindow.Allowance = this.Model;

            this.Model_Deduction.Allowance = this.Model;

            this.Model.ViewModel = this;
            this.Model.Initialize(DateTime.Today);

            this.BindEvent();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// </remarks>
        private void BindEvent()
        {
            // 項目共通
            this.Default_MouseLeave.Subscribe(_ => this.MainWindow.ComparePrice(0, 0));
            // 基本給
            this.BasicSalary_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.BasicSalary_Text.Value, this.Entity_LastYear?.BasicSalary.Value));
            // 役職手当
            this.ExecutiveAllowance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.ExecutiveAllowance_Text.Value, this.Entity_LastYear?.ExecutiveAllowance.Value));
            // 扶養手当
            this.DependencyAllowance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.DependencyAllowance_Text.Value, this.Entity_LastYear?.DependencyAllowance.Value));
            // 時間外手当
            this.OvertimeAllowance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.OvertimeAllowance_Text.Value, this.Entity_LastYear?.OvertimeAllowance.Value));
            // 休日割増
            this.DaysoffIncreased_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.DaysoffIncreased_Text.Value, this.Entity_LastYear?.DaysoffIncreased.Value));
            // 交通費
            this.TransportationExpenses_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.TransportationExpenses_Text.Value, this.Entity_LastYear?.TransportationExpenses.Value));
            // 深夜割増
            this.NightworkIncreased_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.NightworkIncreased_Text.Value, this.Entity_LastYear?.NightworkIncreased.Value));
            // 住宅手当
            this.HousingAllowance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.HousingAllowance_Text.Value, this.Entity_LastYear?.HousingAllowance.Value));
            // 遅刻早退欠勤
            this.LateAbsent_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.LateAbsent_Text.Value, this.Entity_LastYear?.LateAbsent));
            // 在宅手当
            this.ElectricityAllowance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.ElectricityAllowance_Text.Value, this.Entity_LastYear?.ElectricityAllowance.Value));
            // 特別手当
            this.SpecialAllowance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.SpecialAllowance_Text.Value, this.Entity_LastYear?.SpecialAllowance));
            // 予備
            this.SpareAllowance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.SpareAllowance_Text.Value, this.Entity_LastYear?.SpareAllowance));
            // 支給総計
            this.TotalSalary_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.TotalSalary_Text.Value, this.Entity_LastYear?.TotalSalary.Value));
            // 差引支給額
            this.TotalDeductedSalary_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.TotalDeductedSalary_Text.Value, this.Entity_LastYear?.TotalDeductedSalary.Value));
            // 前払退職金
            this.PrepaidRetirementPayment_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.PrepaidRetirementPayment_Text.Value, this.Entity_LastYear?.PrepaidRetirementPayment.Value));
        }

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Model { get; set; } 
            = Model_Allowance.GetInstance(new AllowanceSQLite());

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Model_Deduction { get; set; } 
            = Model_Deduction.GetInstance(new DeductionSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } 
            = Model_MainWindow.GetInstance();

        /// <summary> Entity - 支給額 </summary>
        public AllowanceValueEntity Entity { get; set; }

        /// <summary> Entity - 支給額 (昨年度) </summary>
        public AllowanceValueEntity Entity_LastYear { get; set; }

        /// <summary> 初期状態 - MouseLeave </summary>
        public ReactiveCommand Default_MouseLeave { get; private set; } 
            = new ReactiveCommand();

        #region 基本給

        /// <summary> 基本給 - Text </summary>
        public ReactiveProperty<double> BasicSalary_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 基本給 - MouseMove </summary>
        public ReactiveCommand BasicSalary_MouseMove { get; private set; } 
            = new ReactiveCommand();

        #endregion

        #region 役職手当

        /// <summary> 役職手当 - IsEnabled </summary>
        public ReactiveProperty<bool> ExecutiveAllowance_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 役職手当 - Text </summary>
        public ReactiveProperty<double> ExecutiveAllowance_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 役職手当 - MouseMove </summary>
        public ReactiveCommand ExecutiveAllowance_MouseMove { get; private set; } 
            = new ReactiveCommand();

        #endregion

        #region 扶養手当

        /// <summary> 扶養手当 - IsEnabled </summary>
        public ReactiveProperty<bool> DependencyAllowance_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 扶養手当 - Text </summary>
        public ReactiveProperty<double> DependencyAllowance_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 扶養手当 - MouseMove </summary>
        public ReactiveCommand DependencyAllowance_MouseMove { get; private set; } 
            = new ReactiveCommand();

        #endregion

        #region 時間外手当

        /// <summary> 時間外手当 - IsEnabled </summary>
        public ReactiveProperty<bool> OvertimeAllowance_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 時間外手当 - Text </summary>
        public ReactiveProperty<double> OvertimeAllowance_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 時間外手当 - MouseMove </summary>
        public ReactiveCommand OvertimeAllowance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 休日割増

        /// <summary> 休日割増 - Text </summary>
        public ReactiveProperty<double> DaysoffIncreased_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 休日割増 - MouseMove </summary>
        public ReactiveCommand DaysoffIncreased_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 交通費

        /// <summary> 交通費 - IsEnabled </summary>
        public ReactiveProperty<bool> TransportationExpenses_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 交通費 - Text </summary>
        public ReactiveProperty<double> TransportationExpenses_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 交通費 - MouseMove </summary>
        public ReactiveCommand TransportationExpenses_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 前払退職金

        /// <summary> 前払退職金 - IsEnabled </summary>
        public ReactiveProperty<bool> PrepaidRetirementPayment_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 前払退職金 - Text </summary>
        public ReactiveProperty<double> PrepaidRetirementPayment_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 前払退職金 - MouseMove </summary>
        public ReactiveCommand PrepaidRetirementPayment_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 深夜割増

        /// <summary> 深夜割増 - IsEnabled </summary>
        public ReactiveProperty<bool> NightworkIncreased_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 深夜割増 - Text </summary>
        public ReactiveProperty<double> NightworkIncreased_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 深夜割増 - MouseMove </summary>
        public ReactiveCommand NightworkIncreased_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 住宅手当

        /// <summary> 住宅手当 - IsEnabled </summary>
        public ReactiveProperty<bool> HousingAllowance_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 住宅手当 - Text </summary>
        public ReactiveProperty<double> HousingAllowance_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 住宅手当 - MouseMove </summary>
        public ReactiveCommand HousingAllowance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 遅刻早退欠勤

        /// <summary> 遅刻早退欠勤 - Text </summary>
        public ReactiveProperty<double> LateAbsent_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 遅刻早退欠勤 - MouseMove </summary>
        public ReactiveCommand LateAbsent_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 特別手当

        /// <summary> 特別手当 - IsEnabled </summary>
        public ReactiveProperty<bool> SpecialAllowance_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 特別手当 - Text </summary>
        public ReactiveProperty<double> SpecialAllowance_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 特別手当 - MouseMove </summary>
        public ReactiveCommand SpecialAllowance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 在宅手当

        /// <summary> 在宅手当 - IsEnabled </summary>
        public ReactiveProperty<bool> ElectricityAllowance_IsEnabled { get; set; } 
            = new ReactiveProperty<bool>();

        /// <summary> 在宅手当 - Text </summary>
        public ReactiveProperty<double> ElectricityAllowance_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 特別手当 - MouseMove </summary>
        public ReactiveCommand ElectricityAllowance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 予備

        /// <summary> 予備 - Text </summary>
        public ReactiveProperty<double> SpareAllowance_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 予備 - MouseMove </summary>
        public ReactiveCommand SpareAllowance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 備考

        /// <summary> 備考 - Text </summary>
        public ReactiveProperty<string> Remarks_Text { get; set; } 
            = new ReactiveProperty<string>();

        #endregion

        #region 支給総計

        /// <summary> 支給総計 - Foreground </summary>
        public ReactiveProperty<SolidColorBrush> TotalSalary_Foreground { get; } 
            = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.Blue));

        /// <summary> 支給総計 - Text </summary>
        public ReactiveProperty<double> TotalSalary_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 支給総計 - MouseMove </summary>
        public ReactiveCommand TotalSalary_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 差引支給額

        /// <summary> 差引支給額 - Foreground </summary>
        public ReactiveProperty<SolidColorBrush> TotalDeductedSalary_Foreground { get; set; } 
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 差引支給額 - Text </summary>
        public ReactiveProperty<double> TotalDeductedSalary_Text { get; set; } 
            = new ReactiveProperty<double>();

        /// <summary> 差引支給額 - MouseMove </summary>
        public ReactiveCommand TotalDeductedSalary_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

    }
}
