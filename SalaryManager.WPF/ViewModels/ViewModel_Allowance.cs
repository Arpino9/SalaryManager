using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 支給額
    /// </summary>
    public class ViewModel_Allowance : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModel_Allowance()
        {
            this.MainWindow.Allowance = this.Model;

            this.Model_Deduction.Allowance = this.Model;

            this.Model.ViewModel = this;
            this.Model.Initialize(DateTime.Today);

            this.BindEvent();
        }

        /// <summary>
        /// 単体テスト用
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public ViewModel_Allowance(AllowanceValueEntity entity)
        {
            if (entity is null)
            {
                return;
            }

            this.BasicSalary_Value            = entity.BasicSalary.Value;
            this.ExecutiveAllowance_Value     = entity.ExecutiveAllowance.Value;
            this.DependencyAllowance_Value    = entity.DependencyAllowance.Value;
            this.OvertimeAllowance_Value      = entity.OvertimeAllowance.Value;
            this.DaysoffIncreased_Value       = entity.DaysoffIncreased.Value;
            this.TransportationExpenses_Value = entity.TransportationExpenses.Value;
            this.NightworkIncreased_Value     = entity.NightworkIncreased.Value;
            this.HousingAllowance_Value       = entity.HousingAllowance.Value;
            this.LateAbsent_Value             = entity.LateAbsent;
            this.SpecialAllowance_Value       = entity.SpecialAllowance;
            this.ElectricityAllowance_Value   = entity.ElectricityAllowance.Value;
            this.SpareAllowance_Value         = entity.SpareAllowance;
            this.Remarks_Text                 = entity.Remarks;
            this.TotalSalary_Value            = entity.TotalSalary.Value;
            this.TotalDeductedSalary_Value    = entity.TotalDeductedSalary.Value;
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvent()
        {
            // 項目共通
            this.Default_MouseLeave                 = new RelayCommand(() => this.MainWindow.ComparePrice(0, 0));
            // 基本給
            this.BasicSalary_MouseMove              = new RelayCommand(() => this.MainWindow.ComparePrice(this.BasicSalary_Value,              this.Entity_LastYear?.BasicSalary.Value));
            // 役職手当
            this.ExecutiveAllowance_MouseMove       = new RelayCommand(() => this.MainWindow.ComparePrice(this.ExecutiveAllowance_Value,       this.Entity_LastYear?.ExecutiveAllowance.Value));
            // 扶養手当
            this.DependencyAllowance_MouseMove      = new RelayCommand(() => this.MainWindow.ComparePrice(this.DependencyAllowance_Value,      this.Entity_LastYear?.DependencyAllowance.Value));
            // 時間外手当
            this.OvertimeAllowance_MouseMove        = new RelayCommand(() => this.MainWindow.ComparePrice(this.OvertimeAllowance_Value,        this.Entity_LastYear?.OvertimeAllowance.Value));
            // 休日割増
            this.DaysoffIncreased_MouseMove         = new RelayCommand(() => this.MainWindow.ComparePrice(this.DaysoffIncreased_Value,         this.Entity_LastYear?.DaysoffIncreased.Value));
            // 交通費
            this.TransportationExpenses_MouseMove   = new RelayCommand(() => this.MainWindow.ComparePrice(this.TransportationExpenses_Value,   this.Entity_LastYear?.TransportationExpenses.Value));
            // 深夜割増
            this.NightworkIncreased_MouseMove       = new RelayCommand(() => this.MainWindow.ComparePrice(this.NightworkIncreased_Value,       this.Entity_LastYear?.NightworkIncreased.Value));
            // 住宅手当
            this.HousingAllowance_MouseMove         = new RelayCommand(() => this.MainWindow.ComparePrice(this.HousingAllowance_Value,         this.Entity_LastYear?.HousingAllowance.Value));
            // 遅刻早退欠勤
            this.LateAbsent_MouseMove               = new RelayCommand(() => this.MainWindow.ComparePrice(this.LateAbsent_Value,               this.Entity_LastYear?.LateAbsent));
            // 在宅手当
            this.ElectricityAllowance_MouseMove     = new RelayCommand(() => this.MainWindow.ComparePrice(this.ElectricityAllowance_Value,     this.Entity_LastYear?.ElectricityAllowance.Value));
            // 特別手当
            this.SpecialAllowance_MouseMove         = new RelayCommand(() => this.MainWindow.ComparePrice(this.SpecialAllowance_Value,         this.Entity_LastYear?.SpecialAllowance));
            // 予備
            this.SpareAllowance_MouseMove           = new RelayCommand(() => this.MainWindow.ComparePrice(this.SpareAllowance_Value,           this.Entity_LastYear?.SpareAllowance));
            // 支給総計
            this.TotalSalary_MouseMove              = new RelayCommand(() => this.MainWindow.ComparePrice(this.TotalSalary_Value,              this.Entity_LastYear?.TotalSalary.Value));
            // 差引支給額
            this.TotalDeductedSalary_MouseMove      = new RelayCommand(() => this.MainWindow.ComparePrice(this.TotalDeductedSalary_Value,      this.Entity_LastYear?.TotalDeductedSalary.Value));
            // 前払退職金
            this.PrepaidRetirementPayment_MouseMove = new RelayCommand(() => this.MainWindow.ComparePrice(this.PrepaidRetirementPayment_Value, this.Entity_LastYear?.PrepaidRetirementPayment.Value));
        }

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Model { get; set; } = Model_Allowance.GetInstance(new AllowanceSQLite());

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Model_Deduction { get; set; } = Model_Deduction.GetInstance(new DeductionSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Entity - 支給額 </summary>
        public AllowanceValueEntity Entity { get; set; }

        /// <summary> Entity - 支給額 (昨年度) </summary>
        public AllowanceValueEntity Entity_LastYear { get; set; }

        /// <summary>
        /// 初期状態 - MouseLeave
        /// </summary>
        public RelayCommand Default_MouseLeave { get; private set; }

        #region 基本給

        private double _basicSalary_Value;

        /// <summary>
        /// 基本給 - Value
        /// </summary>
        public double BasicSalary_Value
        {
            get => this._basicSalary_Value;
            set
            {
                this._basicSalary_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 基本給 - MouseMove
        /// </summary>
        public RelayCommand BasicSalary_MouseMove { get; private set; }

        #endregion

        #region 役職手当

        private bool _executiveAllowance_IsEnabled;

        /// <summary>
        /// 役職手当 - IsEnabled
        /// </summary>
        public bool ExecutiveAllowance_IsEnabled
        {
            get => this._executiveAllowance_IsEnabled;
            set
            {
                this._executiveAllowance_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _executiveAllowance_Value;

        /// <summary>
        /// 役職手当 - Value
        /// </summary>
        public double ExecutiveAllowance_Value
        {
            get => this._executiveAllowance_Value;
            set
            {
                this._executiveAllowance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 役職手当 - MouseMove
        /// </summary>
        public RelayCommand ExecutiveAllowance_MouseMove { get; private set; }

        #endregion

        #region 扶養手当

        private bool _dependencyAllowance_IsEnabled;

        /// <summary>
        /// 扶養手当 - IsEnabled
        /// </summary>
        public bool DependencyAllowance_IsEnabled
        {
            get => this._dependencyAllowance_IsEnabled;
            set
            {
                this._dependencyAllowance_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _dependencyAllowance_Value;

        /// <summary>
        /// 扶養手当 - Value
        /// </summary>
        public double DependencyAllowance_Value
        {
            get => this._dependencyAllowance_Value;
            set
            {
                this._dependencyAllowance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 扶養手当 - MouseMove
        /// </summary>
        public RelayCommand DependencyAllowance_MouseMove { get; private set; }

        #endregion

        #region 時間外手当

        private bool _overtimeAllowance_IsEnabled;

        /// <summary>
        /// 時間外手当 - IsEnabled
        /// </summary>
        public bool OvertimeAllowance_IsEnabled
        {
            get => this._overtimeAllowance_IsEnabled;
            set
            {
                this._overtimeAllowance_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _overtimeAllowance_Value;

        /// <summary>
        /// 時間外手当 - Value
        /// </summary>
        public double OvertimeAllowance_Value
        {
            get => this._overtimeAllowance_Value;
            set
            {
                this._overtimeAllowance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 時間外手当 - MouseMove
        /// </summary>
        public RelayCommand OvertimeAllowance_MouseMove { get; private set; }

        #endregion

        #region 休日割増

        private double _daysoffIncreased_Value;

        /// <summary>
        /// 休日割増 - Value
        /// </summary>
        public double DaysoffIncreased_Value
        {
            get => this._daysoffIncreased_Value;
            set
            {
                this._daysoffIncreased_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 休日割増 - MouseMove
        /// </summary>
        public RelayCommand DaysoffIncreased_MouseMove { get; private set; }

        #endregion

        #region 交通費

        private bool _transportationExpenses_IsEnabled;

        /// <summary>
        /// 交通費 - IsEnabled
        /// </summary>
        public bool TransportationExpenses_IsEnabled
        {
            get => this._transportationExpenses_IsEnabled;
            set
            {
                this._transportationExpenses_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _transportationExpenses_Value;

        /// <summary>
        /// 交通費 - Value
        /// </summary>
        public double TransportationExpenses_Value
        {
            get => this._transportationExpenses_Value;
            set
            {
                this._transportationExpenses_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 交通費 - MouseMove
        /// </summary>
        public RelayCommand TransportationExpenses_MouseMove { get; private set; }

        #endregion

        #region 前払退職金

        private bool _prepaidRetirementPayment_IsEnabled;

        /// <summary>
        /// 前払退職金 - IsEnabled
        /// </summary>
        public bool PrepaidRetirementPayment_IsEnabled
        {
            get => this._prepaidRetirementPayment_IsEnabled;
            set
            {
                this._prepaidRetirementPayment_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _prepaidRetirementPayment_Value;

        /// <summary>
        /// 前払退職金 - Value
        /// </summary>
        public double PrepaidRetirementPayment_Value
        {
            get => this._prepaidRetirementPayment_Value;
            set
            {
                this._prepaidRetirementPayment_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 前払退職金 - MouseMove
        /// </summary>
        public RelayCommand PrepaidRetirementPayment_MouseMove { get; private set; }

        #endregion

        #region 深夜割増

        private bool _nightworkIncreased_IsEnabled;

        /// <summary>
        /// 深夜割増 - IsEnabled
        /// </summary>
        public bool NightworkIncreased_IsEnabled
        {
            get => this._nightworkIncreased_IsEnabled;
            set
            {
                this._nightworkIncreased_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _nightworkIncreased_Value;

        /// <summary>
        /// 深夜割増 - Value
        /// </summary>
        public double NightworkIncreased_Value
        {
            get => this._nightworkIncreased_Value;
            set
            {
                this._nightworkIncreased_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 深夜割増 - MouseMove
        /// </summary>
        public RelayCommand NightworkIncreased_MouseMove { get; private set; }

        #endregion

        #region 住宅手当

        private bool _housingAllowance_IsEnabled;

        /// <summary>
        /// 住宅手当 - IsEnabled
        /// </summary>
        public bool HousingAllowance_IsEnabled
        {
            get => this._housingAllowance_IsEnabled;
            set
            {
                this._housingAllowance_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _housingAllowance_Value;

        /// <summary>
        /// 住宅手当 - Value
        /// </summary>
        public double HousingAllowance_Value
        {
            get
            {
                return this._housingAllowance_Value;
            }
            set
            {
                this._housingAllowance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 住宅手当 - MouseMove
        /// </summary>
        public RelayCommand HousingAllowance_MouseMove { get; private set; }

        #endregion

        #region 遅刻早退欠勤

        private double _lateAbsent_Value;

        /// <summary>
        /// 遅刻早退欠勤 - Value
        /// </summary>
        public double LateAbsent_Value
        {
            get => this._lateAbsent_Value;
            set
            {
                this._lateAbsent_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 遅刻早退欠勤 - MouseMove
        /// </summary>
        public RelayCommand LateAbsent_MouseMove { get; private set; }

        #endregion

        #region 特別手当

        private bool _specialAllowance_IsEnabled;

        /// <summary>
        /// 特別手当 - IsEnabled
        /// </summary>
        public bool SpecialAllowance_IsEnabled
        {
            get => this._specialAllowance_IsEnabled;
            set
            {
                this._specialAllowance_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _specialAllowance_Value;

        /// <summary>
        /// 特別手当 - Value
        /// </summary>
        public double SpecialAllowance_Value
        {
            get => this._specialAllowance_Value;
            set
            {
                this._specialAllowance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 特別手当 - MouseMove
        /// </summary>
        public RelayCommand SpecialAllowance_MouseMove { get; private set; }

        #endregion

        #region 在宅手当

        private bool _electricityAllowance_IsEnabled;

        /// <summary>
        /// 在宅手当 - IsEnabled
        /// </summary>
        public bool ElectricityAllowance_IsEnabled
        {
            get => this._electricityAllowance_IsEnabled;
            set
            {
                this._electricityAllowance_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private double _electricityAllowance_Value;

        /// <summary>
        /// 在宅手当 - Value
        /// </summary>
        public double ElectricityAllowance_Value
        {
            get => this._electricityAllowance_Value;
            set
            {
                this._electricityAllowance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 在宅手当 - MouseMove
        /// </summary>
        public RelayCommand ElectricityAllowance_MouseMove { get; private set; }

        #endregion

        #region 予備

        private double _spareAllowance_Value;

        /// <summary>
        /// 予備 - Value
        /// </summary>
        public double SpareAllowance_Value
        {
            get => this._spareAllowance_Value;
            set
            {
                this._spareAllowance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 予備 - MouseMove
        /// </summary>
        public RelayCommand SpareAllowance_MouseMove { get; private set; }

        #endregion

        #region 備考

        private string _remarks_Text;

        /// <summary>
        /// 備考 - Text
        /// </summary>
        public string Remarks_Text
        {
            get => this._remarks_Text;
            set
            {
                this._remarks_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 支給総計

        /// <summary>
        /// 支給総計 - Foreground
        /// </summary>
        public Brush TotalSalary_Foreground
        {
            get => new SolidColorBrush(Colors.Blue);
        }

        private double _totalSalary_Value;

        /// <summary>
        /// 支給総計 - Value
        /// </summary>
        public double TotalSalary_Value
        {
            get => this._totalSalary_Value;
            set
            {
                this._totalSalary_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 支給総計 - MouseMove
        /// </summary>
        public RelayCommand TotalSalary_MouseMove { get; private set; }

        #endregion

        #region 差引支給額

        private Brush _totalDeductedSalary_Foreground;

        /// <summary>
        /// 差引支給額 - Foreground
        /// </summary>
        public Brush TotalDeductedSalary_Foreground
        {
            get => _totalDeductedSalary_Foreground;
            set
            {
                this._totalDeductedSalary_Foreground = value;
                this.RaisePropertyChanged();
            }
        }

        private double _totalDeductedSalary_Value;

        /// <summary>
        /// 差引支給額 - Value
        /// </summary>
        public double TotalDeductedSalary_Value
        {
            get => this._totalDeductedSalary_Value;
            set
            {
                this._totalDeductedSalary_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 差引支給額 - MouseMove
        /// </summary>
        public RelayCommand TotalDeductedSalary_MouseMove { get; private set; }

        #endregion

    }
}
