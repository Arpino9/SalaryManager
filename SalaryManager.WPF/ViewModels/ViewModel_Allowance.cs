using SalaryManager.WPF.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            try
            {
                this.Allowance.ViewModel = this;
                this.Allowance.Initialize();
            }
            catch(Exception ex)
            {

            } 
        }

        public Model_Allowance Allowance = Model_Allowance.GetInstance();

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

                this.Allowance.ReCaluculate();
            }
        }

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

                this.Allowance.ReCaluculate();
            }
        }

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

                this.Allowance.ReCaluculate();
            }
        }

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

                this.Allowance.ReCaluculate();
            }
        }

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

                this.Allowance.ReCaluculate();
            }
        }

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

                this.Allowance.ReCaluculate();
            }
        }

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

                this.Allowance.ReCaluculate();
            }
        }

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

                this.Allowance.ReCaluculate();
            }
        }

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

                this.Allowance.ReCaluculate();
            }
        }

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

        #endregion

    }
}
