using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_AnnualChart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public ViewModel_AnnualChart()
        {
            this.Model = Model_AnnualChart.GetInstance();
            this.Model.ViewModel = this;

            this.Model.Initialize(DateTime.Today.Year);
        }

        private Model_AnnualChart Model { get; set; }

        #region 対象日付

        private string _targetDate;

        /// <summary>
        /// 対象日付
        /// </summary>
        public string TargetDate
        {
            get => this._targetDate;
            set
            {
                this._targetDate = value;
                this.RaisePropertyChanged();
            }
        }


        #endregion

        #region 1月

        private int _totalSalary_January;

        /// <summary>
        /// 1月 - 支給額計
        /// </summary>
        public int TotalSalary_January
        {
            get => this._totalSalary_January;
            set
            {
                this._totalSalary_January = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_January;

        /// <summary>
        /// 1月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_January
        {
            get => this._totalDeductedSalary_January;
            set
            {
                this._totalDeductedSalary_January = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_January;

        /// <summary>
        /// 1月 = 副業額
        /// </summary>
        public int TotalSideBusiness_January
        {
            get => this._totalSideBusiness_January;
            set
            {
                this._totalSideBusiness_January = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 2月

        private int _totalSalary_Feburary;

        /// <summary>
        /// 2月 - 支給額計
        /// </summary>
        public int TotalSalary_Feburary
        {
            get => this._totalSalary_Feburary;
            set
            {
                this._totalSalary_Feburary = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_Feburary;

        /// <summary>
        /// 2月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_Feburary
        {
            get => this._totalDeductedSalary_Feburary;
            set
            {
                this._totalDeductedSalary_Feburary = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_Feburary;

        /// <summary>
        /// 2月 = 副業額
        /// </summary>
        public int TotalSideBusiness_Feburary
        {
            get => this._totalSideBusiness_Feburary;
            set
            {
                this._totalSideBusiness_Feburary = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 3月

        private int _totalSalary_March;

        /// <summary>
        /// 3月 - 支給額計
        /// </summary>
        public int TotalSalary_March
        {
            get => this._totalSalary_March;
            set
            {
                this._totalSalary_March = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_March;

        /// <summary>
        /// 3月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_March
        {
            get => this._totalDeductedSalary_March;
            set
            {
                this._totalDeductedSalary_March = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_March;

        /// <summary>
        /// 3月 = 副業額
        /// </summary>
        public int TotalSideBusiness_March
        {
            get => this._totalSideBusiness_March;
            set
            {
                this._totalSideBusiness_March = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 4月

        private int _totalSalary_April;

        /// <summary>
        /// 4月 - 支給額計
        /// </summary>
        public int TotalSalary_April
        {
            get => this._totalSalary_April;
            set
            {
                this._totalSalary_April = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_April;

        /// <summary>
        /// 4月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_April
        {
            get => this._totalDeductedSalary_April;
            set
            {
                this._totalDeductedSalary_April = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_April;

        /// <summary>
        /// 4月 = 副業額
        /// </summary>
        public int TotalSideBusiness_April
        {
            get => this._totalSideBusiness_April;
            set
            {
                this._totalSideBusiness_April = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 5月

        private int _totalSalary_May;

        /// <summary>
        /// 5月 - 支給額計
        /// </summary>
        public int TotalSalary_May
        {
            get => this._totalSalary_May;
            set
            {
                this._totalSalary_May = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_May;

        /// <summary>
        /// 5月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_May
        {
            get => this._totalDeductedSalary_May;
            set
            {
                this._totalDeductedSalary_May = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_May;

        /// <summary>
        /// 5月 = 副業額
        /// </summary>
        public int TotalSideBusiness_May
        {
            get => this._totalSideBusiness_May;
            set
            {
                this._totalSideBusiness_May = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 6月

        private int _totalSalary_June;

        /// <summary>
        /// 6月 - 支給額計
        /// </summary>
        public int TotalSalary_June
        {
            get => this._totalSalary_June;
            set
            {
                this._totalSalary_June = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_June;

        /// <summary>
        /// 6月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_June
        {
            get => this._totalDeductedSalary_June;
            set
            {
                this._totalDeductedSalary_June = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_June;

        /// <summary>
        /// 6月 = 副業額
        /// </summary>
        public int TotalSideBusiness_June
        {
            get => this._totalSideBusiness_June;
            set
            {
                this._totalSideBusiness_June = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 7月

        private int _totalSalary_July;

        /// <summary>
        /// 7月 - 支給額計
        /// </summary>
        public int TotalSalary_July
        {
            get => this._totalSalary_July;
            set
            {
                this._totalSalary_July = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_July;

        /// <summary>
        /// 7月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_July
        {
            get => this._totalDeductedSalary_July;
            set
            {
                this._totalDeductedSalary_July = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_July;

        /// <summary>
        /// 7月 = 副業額
        /// </summary>
        public int TotalSideBusiness_July
        {
            get => this._totalSideBusiness_July;
            set
            {
                this._totalSideBusiness_July = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 8月

        private int _totalSalary_August;

        /// <summary>
        /// 8月 - 支給額計
        /// </summary>
        public int TotalSalary_August
        {
            get => this._totalSalary_August;
            set
            {
                this._totalSalary_August = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_August;

        /// <summary>
        /// 8月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_August
        {
            get => this._totalDeductedSalary_August;
            set
            {
                this._totalDeductedSalary_August = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_August;

        /// <summary>
        /// 8月 = 副業額
        /// </summary>
        public int TotalSideBusiness_August
        {
            get => this._totalSideBusiness_August;
            set
            {
                this._totalSideBusiness_August = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 9月

        private int _totalSalary_September;

        /// <summary>
        /// 9月 - 支給額計
        /// </summary>
        public int TotalSalary_September
        {
            get => this._totalSalary_September;
            set
            {
                this._totalSalary_September = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_September;

        /// <summary>
        /// 9月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_September
        {
            get => this._totalDeductedSalary_September;
            set
            {
                this._totalDeductedSalary_September = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_September;

        /// <summary>
        /// 9月 = 副業額
        /// </summary>
        public int TotalSideBusiness_September
        {
            get => this._totalSideBusiness_September;
            set
            {
                this._totalSideBusiness_September = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 10月

        private int _totalSalary_October;

        /// <summary>
        /// 10月 - 支給額計
        /// </summary>
        public int TotalSalary_October
        {
            get => this._totalSalary_October;
            set
            {
                this._totalSalary_October = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_October;

        /// <summary>
        /// 10月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_October
        {
            get => this._totalDeductedSalary_October;
            set
            {
                this._totalDeductedSalary_October = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_October;

        /// <summary>
        /// 10月 = 副業額
        /// </summary>
        public int TotalSideBusiness_October
        {
            get => this._totalSideBusiness_October;
            set
            {
                this._totalSideBusiness_October = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 11月

        private int _totalSalary_November;

        /// <summary>
        /// 11月 - 支給額計
        /// </summary>
        public int TotalSalary_November
        {
            get => this._totalSalary_November;
            set
            {
                this._totalSalary_November = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_November;

        /// <summary>
        /// 11月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_November
        {
            get => this._totalDeductedSalary_November;
            set
            {
                this._totalDeductedSalary_November = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_November;

        /// <summary>
        /// 11月 = 副業額
        /// </summary>
        public int TotalSideBusiness_November
        {
            get => this._totalSideBusiness_November;
            set
            {
                this._totalSideBusiness_November = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 12月

        private int _totalSalary_December;

        /// <summary>
        /// 12月 - 支給額計
        /// </summary>
        public int TotalSalary_December
        {
            get => this._totalSalary_December;
            set
            {
                this._totalSalary_December = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_December;

        /// <summary>
        /// 12月 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_December
        {
            get => this._totalDeductedSalary_December;
            set
            {
                this._totalDeductedSalary_December = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_December;

        /// <summary>
        /// 12月 = 副業額
        /// </summary>
        public int TotalSideBusiness_December
        {
            get => this._totalSideBusiness_December;
            set
            {
                this._totalSideBusiness_December = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 合計

        private int _totalSalary_Sum;

        /// <summary>
        /// 合計 - 支給額計
        /// </summary>
        public int TotalSalary_Sum
        {
            get => this._totalSalary_Sum;
            set
            {
                this._totalSalary_Sum = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalDeductedSalary_Sum;

        /// <summary>
        /// 合計 - 差引支給額
        /// </summary>
        public int TotalDeductedSalary_Sum
        {
            get => this._totalDeductedSalary_Sum;
            set
            {
                this._totalDeductedSalary_Sum = value;
                this.RaisePropertyChanged();
            }
        }

        private int _totalSideBusiness_Sum;

        /// <summary>
        /// 合計 = 副業額
        /// </summary>
        public int TotalSideBusiness_Sum
        {
            get => this._totalSideBusiness_Sum;
            set
            {
                this._totalSideBusiness_Sum = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
