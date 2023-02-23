﻿using System;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 支給額
    /// </summary>
    public sealed class AllowanceEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="basicSalary">基本給</param>
        /// <param name="executiveAllowance">役職手当</param>
        /// <param name="dependencyAllowance">扶養手当</param>
        /// <param name="overtimeAllowance">時間外手当</param>
        /// <param name="daysoffIncreased">休日割増</param>
        /// <param name="nightworkIncreased">深夜割増</param>
        /// <param name="housingAllowance">住宅手当</param>
        /// <param name="lateAbsent">遅刻早退欠勤</param>
        /// <param name="transportationExpenses">交通費</param>
        /// <param name="specialAllowance">特別手当</param>
        /// <param name="spareAllowance">予備</param>
        /// <param name="remarks">備考</param>
        /// <param name="totalSalary">支給総計</param>
        /// <param name="totalDeductedSalary">差引支給額</param>
        public AllowanceEntity(
            int id,
            DateTime yearMonth, 
            double basicSalary, 
            double executiveAllowance,
            double dependencyAllowance,
            double overtimeAllowance,
            double daysoffIncreased,
            double nightworkIncreased,
            double housingAllowance,
            double lateAbsent,
            double transportationExpenses,
            double specialAllowance,
            double spareAllowance,
            string remarks,
            double totalSalary,
            double totalDeductedSalary)
        {
            this.ID                     = id;
            this.YearMonth              = yearMonth;
            this.BasicSalary            = basicSalary;
            this.ExecutiveAllowance     = executiveAllowance;
            this.DependencyAllowance    = dependencyAllowance;
            this.OvertimeAllowance      = overtimeAllowance;
            this.DaysoffIncreased       = daysoffIncreased;
            this.NightworkIncreased     = nightworkIncreased;
            this.HousingAllowance       = housingAllowance;
            this.LateAbsent             = lateAbsent;
            this.TransportationExpenses = transportationExpenses;
            this.SpecialAllowance       = specialAllowance;
            this.SpareAllowance         = spareAllowance;
            this.Remarks                = remarks;
            this.TotalSalary            = totalSalary;
            this.TotalDeductedSalary    = totalDeductedSalary;                
        }

        /// <summary> ID </summary>
        public int ID { get; set; }

        /// <summary> 年月 </summary>
        public DateTime YearMonth { get; }

        /// <summary> 基本給 </summary>
        public double BasicSalary { get; }

        /// <summary> 役職手当 </summary>
        public double ExecutiveAllowance { get; }

        /// <summary> 扶養手当 </summary>
        public double DependencyAllowance { get; }

        /// <summary> 時間外手当 </summary>
        public double OvertimeAllowance { get; }

        /// <summary> 休日割増 </summary>
        public double DaysoffIncreased { get; }

        /// <summary> 深夜割増 </summary>
        public double NightworkIncreased { get; }

        /// <summary> 住宅手当 </summary>
        public double HousingAllowance { get; }

        /// <summary> 遅刻早退欠勤 </summary>
        public double LateAbsent { get; }

        /// <summary> 交通費 </summary>
        public double TransportationExpenses { get; }

        /// <summary> 特別手当 </summary>
        public double SpecialAllowance { get; }

        /// <summary> 予備 </summary>
        public double SpareAllowance { get; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }

        /// <summary> 支給総計 </summary>
        public double TotalSalary { get; }

        /// <summary> 差引支給額 </summary>
        public double TotalDeductedSalary { get; }
    }
}
