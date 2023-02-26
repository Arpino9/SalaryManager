using SalaryManager.Domain.Entities;
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
            try
            {
                this.MainWindow.Deduction = this.Model;

                this.Allowance.ViewModel_Deduction = this;

                this.Model.ViewModel = this;
                this.Model.Initialize();

                this.BindEvent();
            }
            catch (Exception ex)
            {
                throw new Exception("控除額テーブルの読込に失敗しました。");
            }
        }

        /// <summary>
        /// Bind Event
        /// </summary>
        private void BindEvent()
        {
            // Mouse Leave
            this.MouseLeave_Action            = new RelayCommand(() => this.MainWindow.ComparePrice(0, 0));
            // 健康保険
            this.HealthInsurance_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.HealthInsurance,       this.Entity_LastYear.HealthInsurance.Value));
            // 介護保険
            this.NursingInsurance_Action      = new RelayCommand(() => this.MainWindow.ComparePrice(this.NursingInsurance,      this.Entity_LastYear.NursingInsurance.Value));
            // 厚生年金
            this.WelfareAnnuity_Action        = new RelayCommand(() => this.MainWindow.ComparePrice(this.WelfareAnnuity,        this.Entity_LastYear.WelfareAnnuity.Value));
            // 雇用保険
            this.EmploymentInsurance_Action   = new RelayCommand(() => this.MainWindow.ComparePrice(this.EmploymentInsurance,   this.Entity_LastYear.EmploymentInsurance.Value));
            // 所得税
            this.IncomeTax_Action             = new RelayCommand(() => this.MainWindow.ComparePrice(this.IncomeTax,             this.Entity_LastYear.IncomeTax.Value));
            // 市町村税
            this.MunicipalTax_Action          = new RelayCommand(() => this.MainWindow.ComparePrice(this.MunicipalTax,          this.Entity_LastYear.MunicipalTax.Value));
            // 互助会
            this.FriendshipAssociation_Action = new RelayCommand(() => this.MainWindow.ComparePrice(this.FriendshipAssociation, this.Entity_LastYear.FriendshipAssociation.Value));
            // 年末調整他
            this.YearEndTaxAdjustment_Action  = new RelayCommand(() => this.MainWindow.ComparePrice(this.YearEndTaxAdjustment,  this.Entity_LastYear.YearEndTaxAdjustment));
            // 控除額計
            this.TotalDeduct_Action           = new RelayCommand(() => this.MainWindow.ComparePrice(this.TotalDeduct,           this.Entity_LastYear.TotalDeduct.Value));
        }

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Model { get; set; } = Model_Deduction.GetInstance();

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Allowance { get; set; } = Model_Allowance.GetInstance();

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

        private double _healthhnsurance;

        /// <summary>
        /// 健康保険
        /// </summary>
        public double HealthInsurance
        {
            get => this._healthhnsurance;
            set
            {
                this._healthhnsurance = value;
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

        private double _nursingInsurance;

        /// <summary>
        /// 介護保険
        /// </summary>
        public double NursingInsurance
        {
            get => this._nursingInsurance;
            set
            {
                this._nursingInsurance = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 介護保険 - Action
        /// </summary>
        public RelayCommand NursingInsurance_Action { get; private set; }

        #endregion

        #region 厚生年金

        private double _welfareAnnuity;

        /// <summary>
        /// 厚生年金
        /// </summary>
        public double WelfareAnnuity
        {
            get => this._welfareAnnuity;
            set
            {
                this._welfareAnnuity = value;
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

        private double _employmentInsurance;

        /// <summary>
        /// 雇用保険
        /// </summary>
        public double EmploymentInsurance
        {
            get => this._employmentInsurance;
            set
            {
                this._employmentInsurance = value;
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

        private double _incomeTax;

        /// <summary>
        /// 所得税
        /// </summary>
        public double IncomeTax
        {
            get => this._incomeTax;
            set
            {
                this._incomeTax = value;
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

        private double _municipalTax;

        /// <summary>
        /// 市町村税
        /// </summary>
        public double MunicipalTax
        {
            get => this._municipalTax;
            set
            {
                this._municipalTax = value;
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

        private double _friendshipAssociation;

        /// <summary>
        /// 互助会
        /// </summary>
        public double FriendshipAssociation
        {
            get => this._friendshipAssociation;
            set
            {
                this._friendshipAssociation = value;
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

        private double _yearEndTaxAdjustment;

        /// <summary>
        /// 年末調整他
        /// </summary>
        public double YearEndTaxAdjustment
        {
            get => this._yearEndTaxAdjustment;
            set
            {
                this._yearEndTaxAdjustment = value;
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

        private double _totalDeduct;

        /// <summary>
        /// 控除額計
        /// </summary>
        public double TotalDeduct
        {
            get => this._totalDeduct;
            set
            {
                this._totalDeduct = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 年末調整他 - Action
        /// </summary>
        public RelayCommand TotalDeduct_Action { get; private set; }

        #endregion

        #region 備考

        private string _remarks;

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks
        {
            get => this._remarks;
            set
            {
                this._remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
