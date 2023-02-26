using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

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
            // 登録
            this._registerAction += this.Model.Register;
            this._registerAction += this.AnnualChart.Reload;

            // デフォルトに設定
            this._setDefaultAction += this.Header.SetDefault;
            this._setDefaultAction += this.Header.Register;

            this.Model.MainWindow = this;
        }

        /// <summary> Model - ヘッダー </summary>
        public Model_MainWindow Model { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Model - ヘッダ </summary>
        public Model_Header Header { get; set; } = Model_Header.GetInstance();

        /// <summary> Model - 月収一覧 </summary>
        private Model_AnnualChart AnnualChart { get; set; } = Model_AnnualChart.GetInstance();

        #region タイトル

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get => "給与明細管理(仮)";
        }

        #endregion

        #region CSV読込

        private RelayCommand _readCSV;

        /// <summary>
        /// CSV読込ボタン
        /// </summary>
        public RelayCommand ReadCSVClick
        {
            get
            {
                if (this._readCSV == null)
                {
                    this._readCSV = new RelayCommand(this.Model.ReadCSV);
                }
                return this._readCSV;
            }
        }

        #endregion

        #region デフォルトに設定

        private RelayCommand _setDefault;

        private Action _setDefaultAction;

        /// <summary>
        /// デフォルトに設定ボタン
        /// </summary>
        public RelayCommand SetDefaultClick
        {
            get
            {
                if (this._setDefault == null)
                {
                    this._setDefault = new RelayCommand(this._setDefaultAction);
                }
                return this._setDefault;
            }
        }

        #endregion

        #region デフォルトから取得

        private RelayCommand _getDefault;

        /// <summary>
        /// デフォルトに設定ボタン
        /// </summary>
        public RelayCommand GetDefaultClick
        {
            get
            {
                if (this._getDefault == null)
                {
                    this._getDefault = new RelayCommand(this.Model.GetDefault);
                }
                return this._getDefault;
            }
        }

        #endregion

        #region 登録

        private RelayCommand _register;

        private Action _registerAction;

        /// <summary>
        /// 登録ボタン
        /// </summary>
        public RelayCommand RegisterClick
        {
            get
            {
                if (this._register == null)
                {
                    this._register = new RelayCommand(this._registerAction);
                }
                return this._register;
            }
        }

        #endregion

        #region 金額の比較用

        private Brush _priceUpDown_Foreground;

        /// <summary>
        /// 金額の比較用
        /// </summary>
        public Brush PriceUpdown_Foreground
        {
            get => this._priceUpDown_Foreground;
            set
            {
                this._priceUpDown_Foreground = value;
                this.RaisePropertyChanged();
            }
        }

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
