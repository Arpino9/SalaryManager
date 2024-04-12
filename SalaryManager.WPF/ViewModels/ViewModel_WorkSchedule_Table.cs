using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_WorkSchedule_Table : INotifyPropertyChanged
    {
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_WorkSchedule_Table()
        {
            
        }

        #region 1日

        private string _day_1;

        /// <summary>
        /// 1日
        /// </summary>
        public string Day_1
        {
            get => this._day_1;
            set
            {
                this._day_1 = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
