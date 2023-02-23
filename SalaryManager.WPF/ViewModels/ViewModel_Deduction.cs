using SalaryManager.WPF.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        /// <exception cref="NotImplementedException">とりあえず未実装例外</exception>
        public ViewModel_Deduction()
        {
            try
            {
                this.Model.ViewModel = this;

                this.Model.Initialize();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        /// <summary>
        /// Model
        /// </summary>
        public Model_Deduction Model { get; set; } = Model_Deduction.GetInstance();

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
            }
        }

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

        #endregion

        #region 雇用保険

        private double _incomeTax;

        /// <summary>
        /// 雇用保険
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

        #endregion

        #region 控除額計

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
