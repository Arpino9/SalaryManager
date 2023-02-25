using System;
using SalaryManager.Domain.ValueObjects;

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
            this.HealthInsurance       = new MoneyValue(healthInsurance);
            this.NursingInsurance      = new MoneyValue(nursingInsurance);
            this.WelfareAnnuity        = new MoneyValue(welfareAnnuity);
            this.EmploymentInsurance   = new MoneyValue(employmentInsurance);
            this.IncomeTax             = new MoneyValue(incomeTax);
            this.MunicipalTax          = new MoneyValue(municipalTax);
            this.FriendshipAssociation = new MoneyValue(friendshipAssociation);
            this.YearEndTaxAdjustment  = yearEndTaxAdjustment;
            this.Remarks               = remarks;
            this.TotalDeduct           = new MoneyValue(totalDeduct);
        }

        /// <summary> ID </summary>
        public int ID { get; set; }

        /// <summary> 年月 </summary>
        public DateTime YearMonth { get; set; }

        /// <summary> 健康保険 </summary>
        public MoneyValue HealthInsurance { get; }

        /// <summary> 介護保険 </summary>
        public MoneyValue NursingInsurance { get; }

        /// <summary> 厚生年金 </summary>
        public MoneyValue WelfareAnnuity { get; }

        /// <summary> 雇用保険 </summary>
        public MoneyValue EmploymentInsurance { get; }

        /// <summary> 所得税 </summary>
        public MoneyValue IncomeTax { get; }

        /// <summary> 市町村税 </summary>
        public MoneyValue MunicipalTax { get; }

        /// <summary> 互助会 </summary>
        public MoneyValue FriendshipAssociation { get; }

        /// <summary> 年末調整他 </summary>
        public double YearEndTaxAdjustment { get; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }

        /// <summary> 控除額計 </summary>
        public MoneyValue TotalDeduct { get; }
    }
}
