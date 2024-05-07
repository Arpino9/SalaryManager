using System;
using System.ComponentModel;
using Reactive.Bindings;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤怠表 (ヘッダ)
    /// </summary>
    public class ViewModel_WorkSchedule_Header : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel_WorkSchedule_Header()
        {
            this.Model.ViewModel_Header = this;

            this.Model.Initialize_Header();

            this.BindEvents();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// Subscribe()メソッドのオーバーロードが正しく呼ばれないので、
        /// 名前空間に「using System;」を必ず入れること。
        /// </remarks>
        private void BindEvents()
        {
            this.Return_Command.Subscribe(_ => this.Model.Return());
            this.Proceed_Command.Subscribe(_ => this.Model.Proceed());
        }

        /// <summary>
        /// Model - 勤務表
        /// </summary>
        private Model_WorkSchedule_Table Model 
            = Model_WorkSchedule_Table.GetInstance();

        public DateTime TargetDate;

        #region 派遣元

        /// <summary> 派遣元 - Text </summary>
        public ReactiveProperty<string> DispatchingCompany_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 派遣先

        /// <summary> 派遣先 - Text </summary>
        public ReactiveProperty<string> DispatchedCompany_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 勤務日数

        /// <summary> 勤務日数 - Text </summary>
        public ReactiveProperty<string> WorkDaysTotal_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        public TimeSpan OvertimeTotal;

        #region 残業時間

        /// <summary> 残業時間 - Text </summary>
        public ReactiveProperty<string> OvertimeTotal_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        public TimeSpan WorkingTimeTotal;

        #region 勤務時間

        /// <summary> 勤務時間 - Text </summary>
        public ReactiveProperty<string> WorkingTimeTotal_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 該当年月

        /// <summary> 年 - Text </summary>
        public ReactiveProperty<int> Year_Text { get; set; }
            = new ReactiveProperty<int>();

        /// <summary> 月 - Text </summary>
        public ReactiveProperty<int> Month_Text { get; set; }
            = new ReactiveProperty<int>();

        #endregion

        #region 戻る

        /// <summary> 戻る - Command </summary>
        public ReactiveCommand Return_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 進む

        /// <summary> 進む - Command </summary>
        public ReactiveCommand Proceed_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

    }
}
