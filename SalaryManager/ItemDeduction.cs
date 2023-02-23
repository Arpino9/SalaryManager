/// <summary>
/// Salary Manager
/// </summary>
namespace SalaryManager
{
    /// <summary>
    /// 控除額
    /// </summary>
    public class ItemDeduction :
        // 給与明細
        ItemPaySlipAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemDeduction()
        {
        }

        #region 健康保険

        private int _HealthInsurance;

        /// <summary>
        /// 健康保険
        /// </summary>
        public int? HealthInsurance
        {
            get
            {
                return this._HealthInsurance;
            }
            set
            {
                this._HealthInsurance = value ?? 0;
            }
        }

        #endregion

        #region 介護保険

        private int _NursingInsurance;

        /// <summary>
        /// 介護保険
        /// </summary>
        public int? NursingInsurance
        {
            get
            {
                return this._NursingInsurance;
            }
            set
            {
                this._NursingInsurance = value ?? 0;
            }
        }

        #endregion

        #region 厚生年金

        private int _WelfareAnnuity;

        /// <summary>
        /// 厚生年金
        /// </summary>
        public int? WelfareAnnuity
        {
            get
            {
                return this._WelfareAnnuity;
            }
            set
            {
                this._WelfareAnnuity = value ?? 0;
            }
        }

        #endregion

        #region 雇用保険

        private int _EmploymentInsurance;

        /// <summary>
        /// 雇用保険
        /// </summary>
        public int? EmploymentInsurance
        {
            get
            {
                return this._EmploymentInsurance;
            }
            set
            {
                this._EmploymentInsurance = value ?? 0;
            }
        }

        #endregion

        #region 所得税

        private int _IncomeTax;

        /// <summary>
        /// 所得税
        /// </summary>
        public int? IncomeTax
        {
            get
            {
                return this._IncomeTax;
            }
            set
            {
                this._IncomeTax = value ?? 0;
            }
        }

        #endregion

        #region 市町村税

        private int _MunicipalTax;

        /// <summary>
        /// 市町村税
        /// </summary>
        public int? MunicipalTax
        {
            get
            {
                return this._MunicipalTax;
            }
            set
            {
                this._MunicipalTax = value ?? 0;
            }
        }

        #endregion

        #region 互助会

        private int _FriendshipAssociation;

        /// <summary>
        /// 互助会
        /// </summary>
        public int? FriendshipAssociation
        {
            get
            {
                return this._FriendshipAssociation;
            }
            set
            {
                this._FriendshipAssociation = value ?? 0;
            }
        }

        #endregion

        #region 年末調整他

        private int _YearEndTaxAdjustment;

        /// <summary>
        /// 年末調整他
        /// </summary>
        public int? YearEndTaxAdjustment
        {
            get
            {
                return this._YearEndTaxAdjustment;
            }
            set
            {
                this._YearEndTaxAdjustment = value ?? 0;
            }
        }

        #endregion

        #region 控除額計

        private int _TotalDeduct;

        /// <summary>
        /// 控除額計
        /// </summary>
        public int? TotalDeduct
        {
            get
            {
                return this._TotalDeduct;
            }
            set
            {
                this._TotalDeduct = value ?? 0;
            }
        }

        #endregion

        #region Reset

        /// <summary>
        /// Reset
        /// </summary>
        public override void Reset()
        {
            // 健康保険
            this.HealthInsurance = 0;

            // 介護保険
            this.NursingInsurance = 0;

            // 厚生年金
            this.WelfareAnnuity = 0;

            // 所得税
            this.IncomeTax = 0;

            // 市町村税
            this.MunicipalTax = 0;

            // 互助会
            this.FriendshipAssociation = 0;

            // 年末調整他
            this.YearEndTaxAdjustment = 0;

            // 控除額計
            this.TotalDeduct = 0;
        }

        #endregion

    }
}
