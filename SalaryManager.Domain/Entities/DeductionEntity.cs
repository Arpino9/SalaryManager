using System;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 控除額
    /// </summary>
    public sealed class DeductionEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="yearMonth">年月</param>
        /// <param name="healthInsurance">健康保険</param>
        /// <param name="nursingInsurance">介護保険</param>
        /// <param name="welfareAnnuity">厚生年金</param>
        /// <param name="employmentInsurance">雇用保険</param>
        /// <param name="incomeTax">所得税</param>
        /// <param name="municipalTax">市町村税</param>
        /// <param name="friendshipAssociation">互助会</param>
        /// <param name="yearEndTaxAdjustment">年末調整他</param>
        /// <param name="remarks">備考</param>
        /// <param name="totalDeduct">控除額計</param>
        public DeductionEntity(
            int id,
            DateTime yearMonth,
            double healthInsurance,
            double nursingInsurance,
            double welfareAnnuity,
            double employmentInsurance,
            double incomeTax,
            double municipalTax,
            double friendshipAssociation,
            double yearEndTaxAdjustment,
            string remarks,
            double totalDeduct)
        {
            this.ID                    = id;
            this.YearMonth             = yearMonth;
            this.HealthInsurance       = healthInsurance;
            this.NursingInsurance      = nursingInsurance;
            this.WelfareAnnuity        = welfareAnnuity;
            this.EmploymentInsurance   = employmentInsurance;
            this.IncomeTax             = incomeTax;
            this.MunicipalTax          = municipalTax;
            this.FriendshipAssociation = friendshipAssociation;
            this.YearEndTaxAdjustment  = yearEndTaxAdjustment;
            this.Remarks               = remarks;
            this.TotalDeduct           = totalDeduct;
        }

        /// <summary> ID </summary>
        public int ID { get; set; }

        /// <summary> 年月 </summary>
        public DateTime YearMonth { get; set; }

        /// <summary> 健康保険 </summary>
        public double HealthInsurance { get; }

        /// <summary> 介護保険 </summary>
        public double NursingInsurance { get; }

        /// <summary> 厚生年金 </summary>
        public double WelfareAnnuity { get; }

        /// <summary> 雇用保険 </summary>
        public double EmploymentInsurance { get; }

        /// <summary> 所得税 </summary>
        public double IncomeTax { get; }

        /// <summary> 市町村税 </summary>
        public double MunicipalTax { get; }

        /// <summary> 互助会 </summary>
        public double FriendshipAssociation { get; }

        /// <summary> 年末調整他 </summary>
        public double YearEndTaxAdjustment { get; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }

        /// <summary> 控除額計 </summary>
        public double TotalDeduct { get; }
    }
}
