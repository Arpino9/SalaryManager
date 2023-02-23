using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤務備考
    /// </summary>
    public class ViewModel_WorkingReference : INotifyPropertyChanged
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
        /// Constructure
        /// </summary>
        /// <exception cref="NotImplementedException">未実装</exception>
        public ViewModel_WorkingReference()
        {
            try
            {
                this.Model.ViewModel = this;
                this.Model.Initialize();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        /// <summary>
        /// Model
        /// </summary>
        public Model_WorkingReference Model { get; set; } = Model_WorkingReference.GetInstance();

        #region 時間外時間

        private double _overtimeTime;

        /// <summary>
        /// 時間外時間
        /// </summary>
        public double OvertimeTime
        {
            get => this._overtimeTime;
            set
            {
                this._overtimeTime = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 休出時間

        private double _weekendWorktime;

        /// <summary>
        /// 休出時間
        /// </summary>
        public double WeekendWorktime
        {
            get => this._weekendWorktime;
            set
            {
                this._weekendWorktime = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 深夜時間

        private double _midnightWorktime;

        /// <summary>
        /// 深夜時間
        /// </summary>
        public double MidnightWorktime
        {
            get => this._midnightWorktime;
            set
            {
                this._midnightWorktime = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 遅刻早退欠勤H

        private double _lateAbsentH;

        /// <summary>
        /// 遅刻早退欠勤H
        /// </summary>
        public double LateAbsentH
        {
            get => this._lateAbsentH;
            set
            {
                this._lateAbsentH = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 支給額-保険

        private double _insurance;

        /// <summary>
        /// 支給額-保険
        /// </summary>
        public double Insurance
        {
            get => this._insurance;
            set
            {
                this._insurance = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 標準月額千円

        private double _norm;

        /// <summary>
        /// 標準月額千円
        /// </summary>
        public double Norm
        {
            get => this._norm;
            set
            {
                this._norm = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 扶養人数

        private double _numberOfDependent;

        /// <summary>
        /// 扶養人数
        /// </summary>
        public double NumberOfDependent
        {
            get => this._numberOfDependent;
            set
            {
                this._numberOfDependent = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 有給残日数

        private double _paidVacation;

        /// <summary>
        /// 有給残日数
        /// </summary>
        public double PaidVacation
        {
            get => this._paidVacation;
            set
            {
                this._paidVacation = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 勤務時間

        private double _workingHours;

        /// <summary>
        /// 勤務時間
        /// </summary>
        public double WorkingHours
        {
            get => this._workingHours;
            set
            {
                this._workingHours = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 勤務先

        /// <summary>
        /// 勤務先
        /// </summary>
        public string WorkingPlace { get; set; }

        #endregion

        #region 備考

        private string _remarks;

        /// <summary>
        /// 勤務時間
        /// </summary>
        public string Remarks
        {
            get => this._remarks;
            set
            {
                this._remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
