using SalaryManager.Domain.Entities;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 支給額
    /// </summary>
    public sealed class ViewModel_Allowance : INotifyPropertyChanged
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
            try
            {
                this.Model.ViewModel = this;
                this.Model.Initialize();

                this.BindEvent();
            }
            catch(Exception ex)
            {

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
            this.BasicSalary_Action            = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.BasicSalary,            this.Entity_LastYear?.BasicSalary));
            // 役職手当
            this.ExecutiveAllowance_Action     = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.ExecutiveAllowance,     this.Entity_LastYear?.ExecutiveAllowance));
            // 扶養手当
            this.DependencyAllowance_Action    = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.DependencyAllowance,    this.Entity_LastYear?.DependencyAllowance));
            // 時間外手当
            this.OvertimeAllowance_Action      = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.OvertimeAllowance,      this.Entity_LastYear?.OvertimeAllowance));
            // 休日割増
            this.DaysoffIncreased_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.DaysoffIncreased,       this.Entity_LastYear?.DaysoffIncreased));
            // 深夜割増
            this.NightworkIncreased_Action     = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.NightworkIncreased,     this.Entity_LastYear?.NightworkIncreased));
            // 住宅手当
            this.HousingAllowance_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.HousingAllowance,       this.Entity_LastYear?.HousingAllowance));
            // 遅刻早退欠勤
            this.LateAbsent_Action             = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.LateAbsent,             this.Entity_LastYear?.LateAbsent));
            // 交通費
            this.TransportationExpenses_Action = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.TransportationExpenses, this.Entity_LastYear?.TransportationExpenses));
            // 特別手当
            this.SpecialAllowance_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.SpecialAllowance,       this.Entity_LastYear?.SpecialAllowance));
            // 予備
            this.SpareAllowance_Action         = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.SpareAllowance,         this.Entity_LastYear?.SpareAllowance));
            // 支給総計
            this.TotalSalary_Action            = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.TotalSalary,            this.Entity_LastYear?.TotalSalary));
            // 差引支給額
            this.TotalDeductedSalary_Action    = new RelayCommand(() => this.MainWindow.ComparePrice(this.Entity?.TotalDeductedSalary,    this.Entity_LastYear?.TotalDeductedSalary));
        }

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Model { get; set; } = Model_Allowance.GetInstance();

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Entity - 支給額 </summary>
        public AllowanceEntity Entity { get; set; }

        /// <summary> Entity - 支給額 (昨年度) </summary>
        public AllowanceEntity Entity_LastYear { get; set; }

        #region Mouse Leave

        /// <summary>
        /// MouseLeave - Action
        /// </summary>
        public RelayCommand MouseLeave_Action { get; private set; }

        #endregion

        #region 基本給

        private double _basicSalary;

        /// <summary>
        /// 基本給
        /// </summary>
        public double BasicSalary
        {
            get => this._basicSalary;
            set
            {
                this._basicSalary = value;
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

        private double _executiveAllowance;

        /// <summary>
        /// 役職手当
        /// </summary>
        public double ExecutiveAllowance
        {
            get => this._executiveAllowance;
            set
            {
                this._executiveAllowance = value;
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

        private double _dependencyAllowance;

        /// <summary>
        /// 扶養手当
        /// </summary>
        public double DependencyAllowance
        {
            get => this._dependencyAllowance;
            set
            {
                this._dependencyAllowance = value;
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

        private double _overtimeAllowance;

        /// <summary>
        /// 時間外手当
        /// </summary>
        public double OvertimeAllowance
        {
            get => this._overtimeAllowance;
            set
            {
                this._overtimeAllowance = value;
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

        private double _daysoffIncreased;

        /// <summary>
        /// 休日割増
        /// </summary>
        public double DaysoffIncreased
        {
            get => this._daysoffIncreased;
            set
            {
                this._daysoffIncreased = value;
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

        private double _nightworkIncreased;

        /// <summary>
        /// 深夜割増
        /// </summary>
        public double NightworkIncreased
        {
            get => this._nightworkIncreased;
            set
            {
                this._nightworkIncreased = value;
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

        private double _housingAllowance;

        /// <summary>
        /// 住宅手当
        /// </summary>
        public double HousingAllowance
        {
            get
            {
                return this._housingAllowance;
            }
            set
            {
                this._housingAllowance = value;
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

        private double _lateAbsent;

        /// <summary>
        /// 遅刻早退欠勤
        /// </summary>
        public double LateAbsent
        {
            get => this._lateAbsent;
            set
            {
                this._lateAbsent = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 遅刻早退欠勤 - Action
        /// </summary>
        public RelayCommand LateAbsent_Action { get; private set; }

        #endregion

        #region 交通費

        private double _transportationExpenses;

        /// <summary>
        /// 交通費
        /// </summary>
        public double TransportationExpenses
        {
            get => this._transportationExpenses;
            set
            {
                this._transportationExpenses = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 交通費 - Action
        /// </summary>
        public RelayCommand TransportationExpenses_Action { get; private set; }

        #endregion

        #region 特別手当

        private double _specialAllowance;

        /// <summary>
        /// 特別手当
        /// </summary>
        public double SpecialAllowance
        {
            get => this._specialAllowance;
            set
            {
                this._specialAllowance = value;
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

        private double _spareAllowance;

        /// <summary>
        /// 予備
        /// </summary>
        public double SpareAllowance
        {
            get => this._spareAllowance;
            set
            {
                this._spareAllowance = value;
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

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks { get; internal set; }

        #endregion

        #region 支給総計

        private double _totalSalary;

        /// <summary>
        /// 支給総計
        /// </summary>
        public double TotalSalary
        {
            get => this._totalSalary;
            set
            {
                this._totalSalary = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 支給総計 - Action
        /// </summary>
        public RelayCommand TotalSalary_Action { get; private set; }

        #endregion

        #region 差引支給額

        private double _totalDeductedSalary;

        /// <summary>
        /// 差引支給額
        /// </summary>
        public double TotalDeductedSalary
        {
            get => this._totalDeductedSalary;
            set
            {
                this._totalDeductedSalary = value;
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
