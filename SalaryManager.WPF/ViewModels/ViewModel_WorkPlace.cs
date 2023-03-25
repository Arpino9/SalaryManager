using SalaryManager.Domain.Entities;
using SalaryManager.WPF.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤務先
    /// </summary>
    public class ViewModel_WorkPlace : INotifyPropertyChanged
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
            this.Model.ViewModel               = this;
            this.WorkingReference.WorkPlace    = this;
            this.MainWindow.WorkPlace          = this;
            this.Allowance.ViewModel_WorkPlace = this;

            this.Model.Initialize(DateTime.Today);
        }

        /// <summary> Model </summary>
        public Model_WorkPlace Model { get; set; } = Model_WorkPlace.GetInstance();

        /// <summary> Model - 勤怠備考 </summary>
        public Model_WorkingReference WorkingReference { get; set; } = Model_WorkingReference.GetInstance();

        /// <summary> メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();
        
        /// <summary> 手当 </summary>
        public Model_Allowance Allowance { get; set; } = Model_Allowance.GetInstance();

        /// <summary> Entity - 勤務備考 </summary>
        public WorkingReferencesEntity Entity { get; set; }

        /// <summary> Entity - 勤務備考 (昨年度) </summary>
        public WorkingReferencesEntity Entity_LastYear { get; set; }

        #region 背景色

        private Brush _window_Background;

        /// <summary>
        /// 背景色 - Background
        /// </summary>
        public Brush Window_Background
        {
            get => this._window_Background;
            set
            {
                this._window_Background = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 所属会社名

        private Brush _companyName_Foreground;

        /// <summary>
        /// 所属会社名 - Foreground
        /// </summary>
        public Brush CompanyName_Foreground
        {
            get => _companyName_Foreground;
            set
            {
                this._companyName_Foreground = value;
                this.RaisePropertyChanged();
            }
        }

        private string _companyName;

        /// <summary>
        /// 所属会社名
        /// </summary>
        public string CompanyName
        {
            get => this._companyName;
            set
            {
                this._companyName = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

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
