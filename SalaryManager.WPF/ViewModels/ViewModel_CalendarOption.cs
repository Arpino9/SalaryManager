using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - オプション - カレンダー
    /// </summary>
    public class ViewModel_CalendarOption : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_CalendarOption()
        {
            this.Model.CalendarOption = this;

            this.Model.Initialize_Calendar();
        }

        /// <summary> Model - オプション </summary>
        public Model_Option Model = Model_Option.GetInstance();

        #region JSONの保存先パス

        private string _selectPrivateKey_Text;

        /// <summary>
        /// 認証ファイルの保存先パス - Text
        /// </summary>
        public string SelectPrivateKey_Text
        {
            get => this._selectPrivateKey_Text;
            set
            {
                this._selectPrivateKey_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _selectPrivateKey_Command;

        /// <summary>
        /// 認証ファイルの保存先パス - Command
        /// </summary>
        /// <remarks>
        /// 開く
        /// </remarks>
        public RelayCommand SelectPrivateKey_Command
        {
            get
            {
                if (this._selectPrivateKey_Command == null)
                {
                    this._selectPrivateKey_Command = new RelayCommand(this.Model.SelectPrivateKeyPath_Calendar);
                }
                return this._selectPrivateKey_Command;
            }
        }

        #endregion

        #region カレンダーID

        private string _selectCalendarID_Text;

        /// <summary>
        /// カレンダーIDの保存先パス - Text
        /// </summary>
        public string SelectCalendarID_Text
        {
            get => this._selectCalendarID_Text;
            set
            {
                this._selectCalendarID_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
