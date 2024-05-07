using System;
using System.ComponentModel;
using Reactive.Bindings;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤務備考
    /// </summary>
    public class ViewModel_WorkingReference : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// Subscribe()メソッドのオーバーロードが正しく呼ばれないので、
        /// 名前空間に「using System;」を必ず入れること。
        /// </remarks>
        private void BindEvent()
        {
            // 初期状態
            this.Default_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(0, 0));
            // 支給額-保険
            this.Insurance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.Insurance_Text.Value, this.Entity_LastYear?.Insurance.Value));
            // 標準月額千円
            this.Norm_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.Norm_Text.Value, this.Entity_LastYear?.Norm));
        }

        /// <summary> Model </summary>
        public Model_WorkingReference Model { get; set; } 
            = Model_WorkingReference.GetInstance(new WorkingReferenceSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; }  
            = Model_MainWindow.GetInstance();

        /// <summary> Entity - 勤務備考 </summary>
        public WorkingReferencesEntity Entity { get; set; }

        /// <summary> Entity - 勤務備考 (昨年度) </summary>
        public WorkingReferencesEntity Entity_LastYear { get; set; }

        /// <summary> 初期状態 - MouseMove </summary>
        public ReactiveCommand Default_MouseMove { get; private set; }
            = new ReactiveCommand();

        #region 時間外時間

        /// <summary> 時間外時間 - Text </summary>
        public ReactiveProperty<double> OvertimeTime_Text { get; set; }
            = new ReactiveProperty<double>();

        #endregion

        #region 休出時間

        /// <summary> 休出時間 - Text </summary>
        public ReactiveProperty<double> WeekendWorktime_Text { get; set; }
            = new ReactiveProperty<double>();

        #endregion

        #region 深夜時間

        /// <summary> 深夜時間 - Text </summary>
        public ReactiveProperty<double> MidnightWorktime_Text { get; set; }
            = new ReactiveProperty<double>();

        #endregion

        #region 遅刻早退欠勤H

        /// <summary> 遅刻早退欠勤H - Text </summary>
        public ReactiveProperty<double> LateAbsentH_Text { get; set; }
            = new ReactiveProperty<double>();

        #endregion

        #region 支給額-保険

        /// <summary> 支給額-保険 - Text </summary>
        public ReactiveProperty<double> Insurance_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 支給額-保険 - MouseMove </summary>
        public ReactiveCommand Insurance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 標準月額千円

        /// <summary> 標準月額千円 - Text </summary>
        public ReactiveProperty<double> Norm_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 標準月額千円 - MouseMove </summary>
        public ReactiveCommand Norm_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 扶養人数

        /// <summary> 扶養人数 - Text </summary>
        public ReactiveProperty<double> NumberOfDependent_Text { get; set; }
            = new ReactiveProperty<double>();

        #endregion

        #region 有給残日数

        /// <summary> 有給残日数 - Text </summary>
        public ReactiveProperty<double> PaidVacation_Text { get; set; }
            = new ReactiveProperty<double>();

        #endregion

        #region 勤務時間

        /// <summary> 勤務時間 - Text </summary>
        public ReactiveProperty<double> WorkingHours_Text { get; set; }
            = new ReactiveProperty<double>();

        #endregion

        #region 備考

        /// <summary> 勤務時間 - Text </summary>
        public ReactiveProperty<string> Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

    }
}
