using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SalaryManager.Domain.Entities;
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
        /// <exception cref="Exception">読込失敗時</exception>
        public ViewModel_Allowance()
        {
            try
            {
                this.MainWindow.Allowance = this.Model;

                this.Model_Deduction.Allowance = this.Model;

                this.Model.ViewModel = this;
                this.Model.Initialize(DateTime.Today);

                this.BindEvent();
            }
            catch(Exception ex)
            {
                throw new Exception("支給額テーブルの読込に失敗しました。", ex);
            } 
        }

        /// <summary>
        /// Bind Event
        /// </summary>
        private void BindEvent()
        {
            // Mouse Leave
            this.MouseLeave_Action             = new RelayCommand(() => this.MainWindow.ComparePrice(0, 0));
            // 基本給
            this.BasicSalary_Action            = new RelayCommand(() => this.MainWindow.ComparePrice(this.BasicSalary_Value,            this.Entity_LastYear?.BasicSalary.Value));
            // 役職手当
            this.ExecutiveAllowance_Action     = new RelayCommand(() => this.MainWindow.ComparePrice(this.ExecutiveAllowance_Value,     this.Entity_LastYear?.ExecutiveAllowance.Value));
            // 扶養手当
            this.DependencyAllowance_Action    = new RelayCommand(() => this.MainWindow.ComparePrice(this.DependencyAllowance_Value,    this.Entity_LastYear?.DependencyAllowance.Value));
            // 時間外手当
            this.OvertimeAllowance_Action      = new RelayCommand(() => this.MainWindow.ComparePrice(this.OvertimeAllowance_Value,      this.Entity_LastYear?.OvertimeAllowance.Value));
            // 休日割増
            this.DaysoffIncreased_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.DaysoffIncreased_Value,       this.Entity_LastYear?.DaysoffIncreased.Value));
            // 深夜割増
            this.NightworkIncreased_Action     = new RelayCommand(() => this.MainWindow.ComparePrice(this.NightworkIncreased_Value,     this.Entity_LastYear?.NightworkIncreased.Value));
            // 住宅手当
            this.HousingAllowance_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.HousingAllowance_Value,       this.Entity_LastYear?.HousingAllowance.Value));
            // 遅刻早退欠勤
            this.LateAbsent_Action             = new RelayCommand(() => this.MainWindow.ComparePrice(this.LateAbsent_Value,             this.Entity_LastYear?.LateAbsent));
            // 交通費
            this.TransportationExpenses_Action = new RelayCommand(() => this.MainWindow.ComparePrice(this.TransportationExpenses_Value, this.Entity_LastYear?.TransportationExpenses.Value));
            // 在宅手当
            this.ElectricityAllowance_Action   = new RelayCommand(() => this.MainWindow.ComparePrice(this.ElectricityAllowance_Value,   this.Entity_LastYear?.ElectricityAllowance.Value));
            // 特別手当
            this.SpecialAllowance_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.SpecialAllowance_Value,       this.Entity_LastYear?.SpecialAllowance.Value));
            // 予備
            this.SpareAllowance_Action         = new RelayCommand(() => this.MainWindow.ComparePrice(this.SpareAllowance_Value,         this.Entity_LastYear?.SpareAllowance.Value));
            // 支給総計
            this.TotalSalary_Action            = new RelayCommand(() => this.MainWindow.ComparePrice(this.TotalSalary_Value,            this.Entity_LastYear?.TotalSalary.Value));
            // 差引支給額
            this.TotalDeductedSalary_Action    = new RelayCommand(() => this.MainWindow.ComparePrice(this.TotalDeductedSalary_Value,    this.Entity_LastYear?.TotalDeductedSalary.Value));
        }

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Model { get; set; } = Model_Allowance.GetInstance();

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Model_Deduction { get; set; } = Model_Deduction.GetInstance();

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Entity - 支給額 </summary>
        public AllowanceValueEntity Entity { get; set; }

        /// <summary> Entity - 支給額 (昨年度) </summary>
        public AllowanceValueEntity Entity_LastYear { get; set; }

        #region Mouse Leave

        /// <summary>
        /// MouseLeave - Action
        /// </summary>
        public RelayCommand MouseLeave_Action { get; private set; }

        #endregion

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
        /// 基本給 - Action
        /// </summary>
        public RelayCommand BasicSalary_Action { get; private set; }

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
        /// 役職手当 - Action
        /// </summary>
        public RelayCommand ExecutiveAllowance_Action { get; private set; }

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
        /// 扶養手当 - Action
        /// </summary>
        public RelayCommand DependencyAllowance_Action { get; private set; }

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
        /// 時間外手当 - Action
        /// </summary>
        public RelayCommand OvertimeAllowance_Action { get; private set; }

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
        /// 休日割増 - Action
        /// </summary>
        public RelayCommand DaysoffIncreased_Action { get; private set; }

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
        /// 深夜割増 - Action
        /// </summary>
        public RelayCommand NightworkIncreased_Action { get; private set; }

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
        /// 住宅手当 - Action
        /// </summary>
        public RelayCommand HousingAllowance_Action { get; private set; }

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
        /// 遅刻早退欠勤 - Action
        /// </summary>
        public RelayCommand LateAbsent_Action { get; private set; }

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
        /// 交通費 - Action
        /// </summary>
        public RelayCommand TransportationExpenses_Action { get; private set; }

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
            }
        }

        /// <summary>
        /// 在宅手当 - Action
        /// </summary>
        public RelayCommand ElectricityAllowance_Action { get; private set; }

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
        /// 特別手当 - Action
        /// </summary>
        public RelayCommand SpecialAllowance_Action { get; private set; }

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
        /// 予備 - Action
        /// </summary>
        public RelayCommand SpareAllowance_Action { get; private set; }

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
        /// 支給総計 - Action
        /// </summary>
        public RelayCommand TotalSalary_Action { get; private set; }

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
        /// 差引支給額 - Action
        /// </summary>
        public RelayCommand TotalDeductedSalary_Action { get; private set; }

        #endregion

    }
}
