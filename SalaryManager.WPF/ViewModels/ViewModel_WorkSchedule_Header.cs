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

        private string _year;

        /// <summary>
        /// 年
        /// </summary>
        public string Year
        {
            get => this._year;
            set
            {
                this._year = value;
                this.RaisePropertyChanged();
            }
        }

        private string _month;

        /// <summary>
        /// 月
        /// </summary>
        public string Month
        {
            get => this._month;
            set
            {
                this._month = value;
                this.RaisePropertyChanged();
            }
        }

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
