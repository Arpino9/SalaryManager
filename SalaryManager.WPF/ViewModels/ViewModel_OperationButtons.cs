﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 操作ボタン
    /// </summary>
    public class ViewModel_OperationButtons : INotifyPropertyChanged
    {
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_OperationButtons()
        {
            // 登録
            this._registerAction += this.MainWindow.Register;
            this._registerAction += this.AnnualChart.Fetch;

            // デフォルトに設定
            this._setDefaultAction += this.OperationButtons.SetDefault;
            this._setDefaultAction += this.Header.Register;
        }

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

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Model - ヘッダ </summary>
        public Model_Header Header { get; set; } = Model_Header.GetInstance();

        /// <summary> Model - 月収一覧 </summary>
        private Model_AnnualChart AnnualChart { get; set; } = Model_AnnualChart.GetInstance();

        /// <summary> Model - 操作ボタン </summary>
        private Model_OperationButtons OperationButtons { get; set; } = Model_OperationButtons.GetInstance();
    }
}
