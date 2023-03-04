using System;
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
            this.MainWindow.Header = this.Model;

            this.Model.ViewModel         = this;
            this.Allowance.Header        = this;
            this.Deduction.Header        = this;
            this.WorkingReference.Header = this;
            this.SideBusiness.Header     = this;
            this.AnnualCharts.Header     = this;

            // ←(戻る)
            this._returnAction += this.Model.Return;
            this._returnAction += this.Allowance.Reload;
            this._returnAction += this.Deduction.Reload;
            this._returnAction += this.WorkingReference.Reload;
            this._returnAction += this.SideBusiness.Reload;
            this._returnAction += this.AnnualCharts.Reload;

            // →(進む)
            this._proceedAction += this.Model.Proceed;
            this._proceedAction += this.Allowance.Reload;
            this._proceedAction += this.Deduction.Reload;
            this._proceedAction += this.WorkingReference.Reload;
            this._proceedAction += this.SideBusiness.Reload;
            this._proceedAction += this.AnnualCharts.Reload;

            // 年
            this._yearAction += this.Allowance.Reload;
            this._yearAction += this.Deduction.Reload;
            this._yearAction += this.WorkingReference.Reload;
            this._yearAction += this.SideBusiness.Reload;
            this._yearAction += this.AnnualCharts.Reload;
            this.Year_TextChanged = new RelayCommand(_yearAction);

            // 月
            this._monthAction += this.Allowance.Reload;
            this._monthAction += this.Deduction.Reload;
            this._monthAction += this.WorkingReference.Reload;
            this._monthAction += this.SideBusiness.Reload;
            this._monthAction += this.AnnualCharts.Reload;
            this.Month_TextChanged = new RelayCommand(_monthAction);

            this.Model.Initialize(DateTime.Today);
        }

        /// <summary> Model - ヘッダー </summary>
        public Model_Header Model { get; set; } = Model_Header.GetInstance();

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();

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
                if (value.ToString().Length != 4)
                {
                    return;
                }

                this._year = value;
                this.RaisePropertyChanged();

                this.Model.Reload();
            }
        }

        private Action _yearAction;

        /// <summary>
        /// 年 - TextChanged
        /// </summary>
        public RelayCommand Year_TextChanged { get; private set; }

        private int _month = DateTime.Now.Month;

        /// <summary>
        /// 月
        /// </summary>
        public int Month
        {
            get => this._month;
            set
            {
                if (value < 1)
                {
                    this._month = 1;
                } 
                else if (value > 12)
                {
                    this._month = 12;
                }
                else
                {
                    this._month = value;
                }
                
                this.RaisePropertyChanged();

                this.Model.Reload();
            }
        }

        private Action _monthAction;

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
