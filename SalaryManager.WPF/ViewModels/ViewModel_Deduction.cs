using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 控除額
    /// </summary>
    public class ViewModel_Deduction : INotifyPropertyChanged
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

        /// <summary>
        /// Constructure
        /// </summary>
        /// <exception cref="Exception">読込失敗時</exception>
        public ViewModel_Deduction()
        {
            this.MainWindow.Deduction = this.Model;

            this.Allowance.ViewModel_Deduction = this;

            this.Model.ViewModel = this;
            this.Model.Initialize(DateTime.Today);

            this.BindEvent();
        }

        /// <summary>
        /// Bind Event
        /// </summary>
        private void BindEvent()
        {
            // Mouse Leave
            this.MouseLeave_Action            = new RelayCommand(() => this.MainWindow.ComparePrice(0, 0));
            // 健康保険
            this.HealthInsurance_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.HealthInsurance_Value,       this.Entity_LastYear?.HealthInsurance.Value));
            // 介護保険
            this.NursingInsurance_Action      = new RelayCommand(() => this.MainWindow.ComparePrice(this.NursingInsurance_Value,      this.Entity_LastYear?.NursingInsurance.Value));
            // 厚生年金
            this.WelfareAnnuity_Action        = new RelayCommand(() => this.MainWindow.ComparePrice(this.WelfareAnnuity_Value,        this.Entity_LastYear?.WelfareAnnuity.Value));
            // 雇用保険
            this.EmploymentInsurance_Action   = new RelayCommand(() => this.MainWindow.ComparePrice(this.EmploymentInsurance_Value,   this.Entity_LastYear?.EmploymentInsurance.Value));
            // 所得税
            this.IncomeTax_Action             = new RelayCommand(() => this.MainWindow.ComparePrice(this.IncomeTax_Value,             this.Entity_LastYear?.IncomeTax.Value));
            // 市町村税
            this.MunicipalTax_Action          = new RelayCommand(() => this.MainWindow.ComparePrice(this.MunicipalTax_Value,          this.Entity_LastYear?.MunicipalTax.Value));
            // 互助会
            this.FriendshipAssociation_Action = new RelayCommand(() => this.MainWindow.ComparePrice(this.FriendshipAssociation_Value, this.Entity_LastYear?.FriendshipAssociation.Value));
            // 年末調整他
            this.YearEndTaxAdjustment_Action  = new RelayCommand(() => this.MainWindow.ComparePrice(this.YearEndTaxAdjustment_Value,  this.Entity_LastYear?.YearEndTaxAdjustment));
            // 控除額計
            this.TotalDeduct_Action           = new RelayCommand(() => this.MainWindow.ComparePrice(this.TotalDeduct_Value,           this.Entity_LastYear?.TotalDeduct.Value));
        }

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Model { get; set; } = Model_Deduction.GetInstance(new DeductionSQLite());

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Allowance { get; set; } = Model_Allowance.GetInstance(new AllowanceSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Entity - 控除額 </summary>
        public DeductionEntity Entity { get; set; }

        /// <summary> Entity - 控除額 (昨年度) </summary>
        public DeductionEntity Entity_LastYear { get; set; }

        #region Mouse Leave

        /// <summary>
        /// MouseLeave - Action
        /// </summary>
        public RelayCommand MouseLeave_Action { get; private set; }

        #endregion

        #region 健康保険

        private double _healthhnsurance_Value;

        /// <summary>
        /// 健康保険 - Value
        /// </summary>
        public double HealthInsurance_Value
        {
            get => this._healthhnsurance_Value;
            set
            {
                this._healthhnsurance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 健康保険 - Action
        /// </summary>
        public RelayCommand HealthInsurance_Action { get; private set; }

        #endregion

        #region 介護保険

        private double _nursingInsurance_Value;

        /// <summary>
        /// 介護保険 = Value
        /// </summary>
        public double NursingInsurance_Value
        {
            get => this._nursingInsurance_Value;
            set
            {
                this._nursingInsurance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 介護保険 - Action
        /// </summary>
        public RelayCommand NursingInsurance_Action { get; private set; }

        #endregion

        #region 厚生年金

        private double _welfareAnnuity_Value;

        /// <summary>
        /// 厚生年金 - Value
        /// </summary>
        public double WelfareAnnuity_Value
        {
            get => this._welfareAnnuity_Value;
            set
            {
                this._welfareAnnuity_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 厚生年金 - Action
        /// </summary>
        public RelayCommand WelfareAnnuity_Action { get; private set; }

        #endregion

        #region 雇用保険

        private double _employmentInsurance_Value;

        /// <summary>
        /// 雇用保険 - Value
        /// </summary>
        public double EmploymentInsurance_Value
        {
            get => this._employmentInsurance_Value;
            set
            {
                this._employmentInsurance_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 雇用保険 - Action
        /// </summary>
        public RelayCommand EmploymentInsurance_Action { get; private set; }

        #endregion

        #region 所得税

        private double _incomeTax_Value;

        /// <summary>
        /// 所得税 - Value
        /// </summary>
        public double IncomeTax_Value
        {
            get => this._incomeTax_Value;
            set
            {
                this._incomeTax_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 所得税 - Action
        /// </summary>
        public RelayCommand IncomeTax_Action { get; private set; }

        #endregion

        #region 市町村税

        private double _municipalTax_Value;

        /// <summary>
        /// 市町村税 - Value
        /// </summary>
        public double MunicipalTax_Value
        {
            get => this._municipalTax_Value;
            set
            {
                this._municipalTax_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 市町村税 - Action
        /// </summary>
        public RelayCommand MunicipalTax_Action { get; private set; }

        #endregion

        #region 互助会

        private double _friendshipAssociation_Value;

        /// <summary>
        /// 互助会 - Value
        /// </summary>
        public double FriendshipAssociation_Value
        {
            get => this._friendshipAssociation_Value;
            set
            {
                this._friendshipAssociation_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 互助会 - Action
        /// </summary>
        public RelayCommand FriendshipAssociation_Action { get; private set; }

        #endregion

        #region 年末調整他

        private double _yearEndTaxAdjustment_Value;

        /// <summary>
        /// 年末調整他 - Value
        /// </summary>
        public double YearEndTaxAdjustment_Value
        {
            get => this._yearEndTaxAdjustment_Value;
            set
            {
                this._yearEndTaxAdjustment_Value = value;
                this.RaisePropertyChanged();

                this.Model.ReCaluculate();
            }
        }

        /// <summary>
        /// 年末調整他 - Action
        /// </summary>
        public RelayCommand YearEndTaxAdjustment_Action { get; private set; }

        #endregion

        #region 控除額計

        /// <summary>
        /// 控除額計 - Foreground
        /// </summary>
        public Brush TotalDeduct_Foreground
        {
            get => new SolidColorBrush(Colors.Red);
        }

        private double _totalDeduct_Value;

        /// <summary>
        /// 控除額計 - Value
        /// </summary>
        public double TotalDeduct_Value
        {
            get => this._totalDeduct_Value;
            set
            {
                this._totalDeduct_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 年末調整他 - Action
        /// </summary>
        public RelayCommand TotalDeduct_Action { get; private set; }

        #endregion

        #region 備考

        private string _remarks_Text;

        /// <summary>
        /// 備考 - Text
        /// </summary>
        public string Remarks_Text
        {
            get => this._remarks_Text;
            set
            {
                this._remarks_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
