using System;
using SalaryManager.Domain.ValueObjects;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 支給額
    /// </summary>
    public sealed class AllowanceValueEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="basicSalary">基本給</param>
        /// <param name="executiveAllowance">役職手当</param>
        /// <param name="dependencyAllowance">扶養手当</param>
        /// <param name="OvertimeAllowance">時間外手当</param>
        /// <param name="daysoffIncreased">休日割増</param>
        /// <param name="nightworkIncreased">深夜割増</param>
        /// <param name="housingAllowance">住宅手当</param>
        /// <param name="lateAbsent">遅刻早退欠勤</param>
        /// <param name="transportationExpenses">交通費</param>
        /// <param name="electricityAllowance">在宅手当</param>
        /// <param name="specialAllowance">特別手当</param>
        /// <param name="spareAllowance">予備</param>
        /// <param name="remarks">備考</param>
        /// <param name="totalSalary">支給総計</param>
        /// <param name="totalDeductedSalary">差引支給額</param>
        /// <param name="prepaidRetirementPayment">前払退職金</param>
        public AllowanceValueEntity(
            int id,
            DateTime yearMonth, 
            double basicSalary, 
            double executiveAllowance,
            double dependencyAllowance,
            double OvertimeAllowance,
            double daysoffIncreased,
            double nightworkIncreased,
            double housingAllowance,
            double lateAbsent,
            double transportationExpenses,
            double prepaidRetirementPayment,
            double electricityAllowance,
            double specialAllowance,
            double spareAllowance,
            string remarks,
            double totalSalary,
            double totalDeductedSalary)
        {
            this.ID                       = id;
            this.YearMonth                = yearMonth;
            this.BasicSalary              = new MoneyValue(basicSalary);
            this.ExecutiveAllowance       = new MoneyValue(executiveAllowance);
            this.DependencyAllowance      = new MoneyValue(dependencyAllowance);
            this.OvertimeAllowance        = new MoneyValue(OvertimeAllowance);
            this.DaysoffIncreased         = new MoneyValue(daysoffIncreased);
            this.NightworkIncreased       = new MoneyValue(nightworkIncreased);
            this.HousingAllowance         = new MoneyValue(housingAllowance);
            this.LateAbsent               = lateAbsent;
            this.TransportationExpenses   = new MoneyValue(transportationExpenses);
            this.PrepaidRetirementPayment = new MoneyValue(prepaidRetirementPayment);                           
            this.ElectricityAllowance     = new MoneyValue(electricityAllowance);
            this.SpecialAllowance         = specialAllowance;
            this.SpareAllowance           = spareAllowance;
            this.Remarks                  = remarks;
            this.TotalSalary              = new MoneyValue(totalSalary);
            this.TotalDeductedSalary      = new MoneyValue(totalDeductedSalary);                           
        }

        /// <summary> ID </summary>
        public int ID { get; }

        /// <summary> 年月 </summary>
        public DateTime YearMonth { get; }

        /// <summary> 基本給 </summary>
        public MoneyValue BasicSalary { get; }

        /// <summary> 役職手当 </summary>
        public MoneyValue ExecutiveAllowance { get; }

        /// <summary> 扶養手当 </summary>
        public MoneyValue DependencyAllowance { get; }

        /// <summary> 時間外手当 </summary>
        public MoneyValue OvertimeAllowance { get; }

        /// <summary> 休日割増 </summary>
        public MoneyValue DaysoffIncreased { get; }

        /// <summary> 深夜割増 </summary>
        public MoneyValue NightworkIncreased { get; }

        /// <summary> 住宅手当 </summary>
        public MoneyValue HousingAllowance { get; }

        /// <summary> 遅刻早退欠勤 </summary>
        public double LateAbsent { get; }

        /// <summary> 交通費 </summary>
        public MoneyValue TransportationExpenses { get; }

        /// <summary> 在宅手当 </summary>
        public MoneyValue ElectricityAllowance { get; }

        /// <summary> 前払退職金 </summary>
        public MoneyValue PrepaidRetirementPayment { get; }

        /// <summary> 特別手当 </summary>
        public double SpecialAllowance { get; }

        /// <summary> 予備 </summary>
        public double SpareAllowance { get; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }

        /// <summary> 支給総計 </summary>
        public MoneyValue TotalSalary { get; }

        /// <summary> 差引支給額 </summary>
        public MoneyValue TotalDeductedSalary { get; }

    }
}
