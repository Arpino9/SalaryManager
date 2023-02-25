using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    public sealed class ViewModel_MainWindow
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
            
        }

        private string _priceUpDown;

        /// <summary>
        /// 基本給
        /// </summary>
        public string PriceUpdown
        {
            get => this._priceUpDown;
            set
            {
                this._priceUpDown = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
