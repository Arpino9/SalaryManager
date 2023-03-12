﻿using SalaryManager.WPF.Models;
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
            this.Model.WorkPlace = this;
            this.MainWindow.WorkPlace = this;
            this.Allowance.ViewModel_WorkPlace = this;
        }

        /// <summary> Model </summary>
        public Model_WorkingReference Model { get; set; } = Model_WorkingReference.GetInstance();

        /// <summary> メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();
        
        /// <summary> 手当 </summary>
        public Model_Allowance Allowance { get; set; } = Model_Allowance.GetInstance();

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
        /// 勤務先
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
