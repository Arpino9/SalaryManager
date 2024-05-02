using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤怠表 (ヘッダ)
    /// </summary>
    public class ViewModel_WorkSchedule_Header : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_WorkSchedule_Header()
        {
            this.Model.ViewModel_Header = this;

            this.Model.Initialize_Header();
        }

        /// <summary>
        /// Model - 勤務表
        /// </summary>
        private Model_WorkSchedule_Table Model = Model_WorkSchedule_Table.GetInstance();

        public DateTime TargetDate;

        

        #region 派遣元

        private string _dispatchingCOmpany;

        /// <summary>
        /// 派遣元
        /// </summary>
        public string DispatchingCompany
        {
            get => this._dispatchingCOmpany;
            set
            {
                this._dispatchingCOmpany = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 派遣先

        private string _dispatchedCOmpany;

        /// <summary>
        /// 派遣先
        /// </summary>
        public string DispatchedCompany
        {
            get => this._dispatchedCOmpany;
            set
            {
                this._dispatchedCOmpany = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        private string _workDaysTotal_Text;

        /// <summary>
        /// 勤務日数 - Text
        /// </summary>
        public string WorkDaysTotal_Text
        {
            get => this._workDaysTotal_Text;
            set
            {
                this._workDaysTotal_Text = value;
                this.RaisePropertyChanged();
            }
        }

        public TimeSpan OvertimeTotal;
        private string _overtimeTotal_Text;

        /// <summary>
        /// 残業時間 - Text
        /// </summary>
        public string OvertimeTotal_Text
        {
            get => this._overtimeTotal_Text;
            set
            {
                this._overtimeTotal_Text = value;
                this.RaisePropertyChanged();
            }
        }

        public TimeSpan WorkingTimeTotal;

        private string _workingtimeTotal_Text;

        /// <summary>
        /// 勤務時間 - Text
        /// </summary>
        public string WorkingTimeTotal_Text
        {
            get => this._workingtimeTotal_Text;
            set
            {
                this._workingtimeTotal_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #region 該当年月

        private int _year;

        /// <summary>
        /// 年
        /// </summary>
        public int Year
        {
            get => this._year;
            set
            {
                this._year = value;
                this.RaisePropertyChanged();
            }
        }

        private int _month;

        /// <summary>
        /// 月
        /// </summary>
        public int Month
        {
            get => this._month;
            set
            {
                this._month = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 戻るボタン

        private RelayCommand _return_Command;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public RelayCommand Return_Command
        {
            get
            {
                if (this._return_Command == null)
                {
                    this._return_Command = new RelayCommand(this.Model.Return_Command);

                }
                return this._return_Command;
            }
        }

        #endregion

        #region 進むボタン

        private RelayCommand _proceed_Command;

        private Action _proceedAction;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public RelayCommand Proceed_Command
        {
            get
            {
                if (this._proceed_Command == null)
                {
                    this._proceed_Command = new RelayCommand(this.Model.Proceed_Command);
                }
                return this._proceed_Command;
            }
        }

        #endregion

    }
}
