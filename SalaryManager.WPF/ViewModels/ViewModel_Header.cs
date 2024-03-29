﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - ヘッダ
    /// </summary>
    public class ViewModel_Header : INotifyPropertyChanged
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

        public ViewModel_Header()
        {
            this.MainWindow.Header = this.Model;

            this.Model.ViewModel         = this;
            this.Allowance.Header        = this;
            this.Deduction.Header        = this;
            this.WorkingReference.Header = this;
            this.SideBusiness.Header     = this;
            this.WorkPlace.Header        = this;
            this.AnnualCharts.Header     = this;

            this.BindEvents();

            this.Model.Initialize(DateTime.Today);
        }

        /// <summary>
        /// Bind Events
        /// </summary>
        private void BindEvents()
        {
            // ←(戻る)
            this._returnAction += this.Model.Return;
            this._returnAction += this.Allowance.Reload;
            this._returnAction += this.Deduction.Reload;
            this._returnAction += this.WorkingReference.Reload;
            this._returnAction += this.SideBusiness.Reload;
            this._returnAction += this.WorkPlace.Reload;
            this._returnAction += this.AnnualCharts.Reload;

            // →(進む)
            this._proceedAction += this.Model.Proceed;
            this._proceedAction += this.Allowance.Reload;
            this._proceedAction += this.Deduction.Reload;
            this._proceedAction += this.WorkingReference.Reload;
            this._proceedAction += this.SideBusiness.Reload;
            this._proceedAction += this.WorkPlace.Reload;
            this._proceedAction += this.AnnualCharts.Reload;

            // 年
            this._yearAction += this.Model.Reload;
            this._yearAction += this.Allowance.Reload;
            this._yearAction += this.Deduction.Reload;
            this._yearAction += this.WorkingReference.Reload;
            this._yearAction += this.SideBusiness.Reload;
            this._yearAction += this.WorkPlace.Reload;
            this._yearAction += this.AnnualCharts.Reload;
            this.Year_TextChanged = new RelayCommand(_yearAction);

            // 月
            this._monthAction += this.Model.Reload;
            this._monthAction += this.Allowance.Reload;
            this._monthAction += this.Deduction.Reload;
            this._monthAction += this.WorkingReference.Reload;
            this._monthAction += this.SideBusiness.Reload;
            this._monthAction += this.WorkPlace.Reload;
            this._monthAction += this.AnnualCharts.Reload;
            this.Month_TextChanged = new RelayCommand(_monthAction);
        }

        /// <summary> Model - ヘッダー </summary>
        public Model_Header Model { get; set; } = Model_Header.GetInstance(new HeaderSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Model - 月収一覧 </summary>
        public Model_AnnualChart AnnualCharts { get; set; } = Model_AnnualChart.GetInstance();

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Allowance { get; set; } = Model_Allowance.GetInstance(new AllowanceSQLite());

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Deduction { get; set; } = Model_Deduction.GetInstance(new DeductionSQLite());

        /// <summary> Model - 勤務備考 </summary>
        public Model_WorkingReference WorkingReference { get; set; } = Model_WorkingReference.GetInstance(new WorkingReferenceSQLite());

        /// <summary> Model - 勤務場所 </summary>
        public Model_WorkPlace WorkPlace { get; set; } = Model_WorkPlace.GetInstance();

        /// <summary> Model - 副業 </summary>
        public Model_SideBusiness SideBusiness { get; set; } = Model_SideBusiness.GetInstance(new SideBusinessSQLite());

        #region 背景色

        private System.Windows.Media.Brush _window_Background;

        /// <summary>
        /// 背景色 - Background
        /// </summary>
        public System.Windows.Media.Brush Window_Background
        {
            get => this._window_Background;
            set
            {
                this._window_Background = value;
                this.RaisePropertyChanged();
            }
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

        private int _year_Value = DateTime.Now.Year;

        /// <summary>
        /// 年 - Value
        /// </summary>
        public int Year_Value
        {
            get => this._year_Value;
            set
            {
                if (value.ToString().Length != 4)
                {
                    return;
                }

                this._year_Value = value;
                this.RaisePropertyChanged();
            }
        }

        private event Action _yearAction;

        /// <summary>
        /// 年 - TextChanged
        /// </summary>
        public RelayCommand Year_TextChanged { get; private set; }

        private int _month_Value = DateTime.Now.Month;

        /// <summary>
        /// 月 - Value
        /// </summary>
        public int Month_Value
        {
            get => this._month_Value;
            set
            {
                if (value < 1)
                {
                    this._month_Value = 1;
                } 
                else if (value > 12)
                {
                    this._month_Value = 12;
                }
                else
                {
                    this._month_Value = value;
                }
                
                this.RaisePropertyChanged();
            }
        }

        private event Action _monthAction;

        /// <summary>
        /// 月 - TextChanged
        /// </summary>
        public RelayCommand Month_TextChanged { get; private set; }

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

        private RelayCommand _return_Command;

        private Action _returnAction;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public RelayCommand Return_Command
        {
            get
            {
                if (this._return_Command == null)
                {
                    this._return_Command = new RelayCommand(this._returnAction);

                }
                return this._return_Command;
            }
        }

        #endregion

        #region 進むボタン

        private RelayCommand _proceed_Command;

        private Action _proceedAction;

        /// <summary>
        /// 戻るボタン
        /// </summary>
        public RelayCommand Proceed_Command
        {
            get
            {
                if (this._proceed_Command == null)
                {
                    this._proceed_Command = new RelayCommand(this._proceedAction);
                }
                return this._proceed_Command;
            }
        }

        #endregion

    }
}
