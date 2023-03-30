using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Converter;
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
        /// <exception cref="Exception">読込失敗時</exception>
        public ViewModel_WorkingReference()
        {
            this.MainWindow.WorkingReference = this.Model;
            this.Model.ViewModel = this;

            this.Model.Initialize(DateTime.Today);

            this.BindEvent();
        }

        /// <summary>
        /// Bind Event
        /// </summary>
        private void BindEvent()
        {
            // Mouse Leave
            this.MouseLeave_Action = new RelayCommand(() => this.MainWindow.ComparePrice(0, 0));
            // 支給額-保険
            this.Insurance_Action  = new RelayCommand(() => this.MainWindow.ComparePrice(this.Insurance_Value, this.Entity_LastYear?.Insurance.Value));
            // 標準月額千円
            this.Norm_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.Norm_Value,      this.Entity_LastYear?.Norm));
        }

        /// <summary> Model </summary>
        public Model_WorkingReference Model { get; set; } = Model_WorkingReference.GetInstance(new WorkingReferenceSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; }  = Model_MainWindow.GetInstance();

        /// <summary> Entity - 勤務備考 </summary>
        public WorkingReferencesEntity Entity { get; set; }

        /// <summary> Entity - 勤務備考 (昨年度) </summary>
        public WorkingReferencesEntity Entity_LastYear { get; set; }

        #region Mouse Leave

        /// <summary>
        /// MouseLeave - Action
        /// </summary>
        public RelayCommand MouseLeave_Action { get; private set; }

        #endregion

        #region 時間外時間

        private double _overtimeTime_Value;

        /// <summary>
        /// 時間外時間 - Value
        /// </summary>
        public double OvertimeTime_Value
        {
            get => this._overtimeTime_Value;
            set
            {
                this._overtimeTime_Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 休出時間

        private double _weekendWorktime_Value;

        /// <summary>
        /// 休出時間 - Value
        /// </summary>
        public double WeekendWorktime_Value
        {
            get => this._weekendWorktime_Value;
            set
            {
                this._weekendWorktime_Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 深夜時間

        private double _midnightWorktime_Value;

        /// <summary>
        /// 深夜時間 - Value
        /// </summary>
        public double MidnightWorktime_Value
        {
            get => this._midnightWorktime_Value;
            set
            {
                this._midnightWorktime_Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 遅刻早退欠勤H

        private double _lateAbsentH_Value;

        /// <summary>
        /// 遅刻早退欠勤H - Value
        /// </summary>
        public double LateAbsentH_Value
        {
            get => this._lateAbsentH_Value;
            set
            {
                this._lateAbsentH_Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 支給額-保険

        private double _insurance_Value;

        /// <summary>
        /// 支給額-保険 - Value
        /// </summary>
        public double Insurance_Value
        {
            get => this._insurance_Value;
            set
            {
                this._insurance_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 支給額-保険 - Action
        /// </summary>
        public RelayCommand Insurance_Action { get; private set; }

        #endregion

        #region 標準月額千円

        private double _norm_Value;

        /// <summary>
        /// 標準月額千円 - Value
        /// </summary>
        public double Norm_Value
        {
            get => this._norm_Value;
            set
            {
                this._norm_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 標準月額千円 - Action
        /// </summary>
        public RelayCommand Norm_Action { get; private set; }

        #endregion

        #region 扶養人数

        private double _numberOfDependent_Value;

        /// <summary>
        /// 扶養人数 - Value
        /// </summary>
        public double NumberOfDependent_Value
        {
            get => this._numberOfDependent_Value;
            set
            {
                this._numberOfDependent_Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 有給残日数

        private double _paidVacation_Value;

        /// <summary>
        /// 有給残日数 - Value
        /// </summary>
        public double PaidVacation_Value
        {
            get => this._paidVacation_Value;
            set
            {
                this._paidVacation_Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 勤務時間

        private double _workingHours_Value;

        /// <summary>
        /// 勤務時間 - Value
        /// </summary>
        public double WorkingHours_Value
        {
            get => this._workingHours_Value;
            set
            {
                this._workingHours_Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 備考

        private string _remarks_Text;

        /// <summary>
        /// 勤務時間 - Text
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

    }
}
