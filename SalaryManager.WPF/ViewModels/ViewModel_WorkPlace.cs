using SalaryManager.WPF.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤務先
    /// </summary>
    public sealed class ViewModel_WorkPlace : INotifyPropertyChanged
    {
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_WorkPlace()
        {
            this.Model.WorkPlace = this;
        }

        /// <summary> Model </summary>
        public Model_WorkingReference Model { get; set; } = Model_WorkingReference.GetInstance();

        #region 勤務先

        private string _workPlace;

        /// <summary>
        /// 勤務先
        /// </summary>
        public string WorkPlace
        {
            get => this._workPlace;
            set
            {
                this._workPlace = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
