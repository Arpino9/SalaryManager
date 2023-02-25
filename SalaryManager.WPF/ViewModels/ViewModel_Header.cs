﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Reactive.Bindings;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_Header : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReactiveProperty<string> OutputText { get; } = new ReactiveProperty<string>("TextBoxに入力した文字をここに表示します\r\nボタンを押すと文字をすべて消去します");

        private static ViewModel_Header model = null;

        public static ViewModel_Header GetInstance()
        {
            if (model == null)
            {
                model = new ViewModel_Header();
            }

            return model;
        }

        public ViewModel_Header()
        {
            this.Allowance.Header        = this;
            this.Header.ViewModel        = this;
            this.AnnualCharts.Header     = this;
            this.Deduction.Header        = this;
            this.WorkingReference.Header = this;
            this.SideBusiness.Header     = this;
            this.OperationButtons.Header = this;

            // ←(戻る)
            this._returnAction += this.Header.Return;
            this._returnAction += this.Allowance.Reload;
            this._returnAction += this.Deduction.Reload;
            this._returnAction += ((Domain.Interface.IInputPayroll)this.WorkingReference).Reload;
            this._returnAction += this.SideBusiness.Reload;
            this._returnAction += this.AnnualCharts.Reload;

            // →(進む)
            this._proceedAction += this.Header.Proceed;
            this._proceedAction += this.Allowance.Reload;
            this._proceedAction += this.Deduction.Reload;
            this._proceedAction += ((Domain.Interface.IInputPayroll)this.WorkingReference).Reload;
            this._proceedAction += this.SideBusiness.Reload;
            this._proceedAction += this.AnnualCharts.Reload;

            this.Header.Initialize();
        }

        /// <summary> Model - ヘッダー </summary>
        public Model_Header Header { get; set; } = Model_Header.GetInstance();

        /// <summary> Model - 月収一覧 </summary>
        public Model_AnnualChart AnnualCharts { get; set; } = Model_AnnualChart.GetInstance();

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Allowance { get; set; } = Model_Allowance.GetInstance();

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Deduction { get; set; } = Model_Deduction.GetInstance();

        /// <summary> Model - 勤務備考 </summary>
        public Model_WorkingReference WorkingReference { get; set; } = Model_WorkingReference.GetInstance();

        /// <summary> Model - 副業 </summary>
        public Model_SideBusiness SideBusiness { get; set; } = Model_SideBusiness.GetInstance();

        /// <summary> Model - 操作ボタン </summary>
        public Model_OperationButtons OperationButtons { get; set; } = Model_OperationButtons.GetInstance();

        #region タイトル

        /// <summary>
        /// 基本給
        /// </summary>
        public string Title
        {
            get => "給与明細管理(仮)";
        }

        #endregion

        #region ID

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; internal set; }

        #endregion

        #region 年月

        /// <summary>
        /// 年月
        /// </summary>
        public DateTime YearMonth { get; set; } = DateTime.Today;

        private int _year = DateTime.Now.Year;

        /// <summary>
        /// 年
        /// </summary>
        public int Year
        {
            get => this._year;
            set
            {
                this._year = value;
                this.RaisePropertyChanged();

                this.Header.Reload();
            }
        }

        private int _month = DateTime.Now.Month;

        /// <summary>
        /// 月
        /// </summary>
        public int Month
        {
            get => this._month;
            set
            {
                this._month = value;
                this.RaisePropertyChanged();

                this.Header.Reload();
            }
        }

        #endregion

        #region デフォルトか

        private bool _isDefault;

        /// <summary>
        /// デフォルトか
        /// </summary>
        public bool IsDefault 
        {
            get => this._isDefault;
            set
            {
                this._isDefault = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 作成日

        private DateTime _createDate;

        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime CreateDate
        {
            get => this._createDate;
            set
            {
                this._createDate = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 更新日

        private DateTime _updateDate;

        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdateDate
        {
            get => this._updateDate;
            set
            {
                this._updateDate = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 戻るボタン

        private RelayCommand _ReturnCommand;

        private Action _returnAction;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public RelayCommand ReturnClick
        {
            get
            {
                if (this._ReturnCommand == null)
                {
                    this._ReturnCommand = new RelayCommand(this._returnAction);

                }
                return this._ReturnCommand;
            }
        }

        #endregion

        #region 進むボタン

        private RelayCommand _ProceedCommand;

        private Action _proceedAction;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public RelayCommand ProceedClick
        {
            get
            {
                if (this._ProceedCommand == null)
                {
                    this._ProceedCommand = new RelayCommand(this._proceedAction);
                }
                return this._ProceedCommand;
            }
        }

        #endregion

    }
}
