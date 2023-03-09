using System;
using SalaryManager.Domain.ValueObjects;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 職歴
    /// </summary>
    public sealed class CareerEntity
    {
        public CareerEntity(
            string workingStatus,
            string companyName,
            DateTime workingStartDate,
            DateTime workingEndDate,
            AllowanceExistenceEntity allowanceExistence,
            string remarks)
        {
            this.WorkingStatus      = workingStatus;
            this.CompanyName        = companyName;
            this.WorkingStartDate   = new WorkingDateValue(workingStartDate);
            this.WorkingEndDate     = new WorkingDateValue(workingEndDate);
            this.AllowanceExistence = allowanceExistence;
            this.Remarks            = remarks;
        }

        /// <summary> 雇用形態 </summary>
        public string WorkingStatus { get; }

        /// <summary> 会社名 </summary>
        public string CompanyName { get; }

        /// <summary> 勤務開始日 </summary>
        public WorkingDateValue WorkingStartDate { get; }
        
        /// <summary> 勤務終了日 </summary>
        public WorkingDateValue WorkingEndDate { get; }

        /// <summary> 手当 </summary>
        public AllowanceExistenceEntity AllowanceExistence { get; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }
    }
}
