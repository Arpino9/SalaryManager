using SalaryManager.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    public sealed class ViewModel_MainWindow : INotifyPropertyChanged
    {
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_MainWindow()
        {
            this.Model.MainWindow = this;
        }

        /// <summary> Model - ヘッダー </summary>
        public Model_MainWindow Model { get; set; } = Model_MainWindow.GetInstance();

        #region 金額の比較用

        private string _priceUpDown;

        /// <summary>
        /// 金額の比較用
        /// </summary>
        public string PriceUpdown_Content
        {
            get => this._priceUpDown;
            set
            {
                this._priceUpDown = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
