using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQLite - 支給額
    /// </summary>
    public class AllowanceSQLite : IAllowanceRepository
    {
        public IReadOnlyList<AllowanceValueEntity> GetEntities()
        {
            string sql = @"
SELECT A.Id, 
A.YearMonth, 
A.BasicSalary, 
A.ExecutiveAllowance, 
A.DependencyAllowance, 
A.OvertimeAllowance, 
A.DaysoffIncreased, 
A.NightworkIncreased ,
A.HousingAllowance,
A.LateAbsent,
A.TransportationExpenses,
A.PrepaidRetirementPayment,
A.ElectricityAllowance,
A.SpecialAllowance,
A.SpareAllowance,
A.Remarks,
A.BasicSalary
+ A.ExecutiveAllowance
+ A.DependencyAllowance
+ A.OvertimeAllowance
+ A.DaysoffIncreased
+ A.NightworkIncreased
+ A.HousingAllowance
+ A.LateAbsent
+ A.SpecialAllowance
+ A.SpareAllowance AS TotalSalary,
A.BasicSalary
+ A.ExecutiveAllowance
+ A.DependencyAllowance
+ A.OvertimeAllowance
+ A.DaysoffIncreased
+ A.NightworkIncreased
+ A.HousingAllowance
+ A.LateAbsent
+ A.SpecialAllowance
+ A.SpareAllowance
- (
D.HealthInsurance
+ D.NursingInsurance
+ D.WelfareAnnuity
+ D.EmploymentInsurance
+ D.IncomeTax
+ D.MunicipalTax
+ D.FriendshipAssociation
+ D.YearEndTaxAdjustment
) AS TotalDeductedSalary
FROM Allowance A
INNER JOIN Deduction D ON A.YearMonth = D.YearMonth";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new AllowanceValueEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["BasicSalary"]),
                                Convert.ToDouble(reader["ExecutiveAllowance"]),
                                Convert.ToDouble(reader["DependencyAllowance"]),
                                Convert.ToDouble(reader["OvertimeAllowance"]),
                                Convert.ToDouble(reader["DaysoffIncreased"]),
                                Convert.ToDouble(reader["NightworkIncreased"]),
                                Convert.ToDouble(reader["HousingAllowance"]),
                                Convert.ToDouble(reader["LateAbsent"]),
                                Convert.ToDouble(reader["TransportationExpenses"]),
                                Convert.ToDouble(reader["PrepaidRetirementPayment"]),
                                Convert.ToDouble(reader["ElectricityAllowance"]),
                                Convert.ToDouble(reader["SpecialAllowance"]),
                                Convert.ToDouble(reader["SpareAllowance"]),
                                Convert.ToString(reader["Remarks"]),
                                Convert.ToDouble(reader["TotalSalary"]),
                                Convert.ToDouble(reader["TotalDeductedSalary"]));
                });
        }

        /// <summary>
        /// Get - 支給額
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>支給額</returns>
        public AllowanceValueEntity GetEntity(int year, int month)
        {
            string sql = @"
SELECT A.Id, 
A.YearMonth, 
A.BasicSalary, 
A.ExecutiveAllowance, 
A.DependencyAllowance, 
A.OvertimeAllowance, 
A.DaysoffIncreased, 
A.NightworkIncreased ,
A.HousingAllowance,
A.LateAbsent,
A.TransportationExpenses,
A.PrepaidRetirementPayment,
A.ElectricityAllowance,
A.SpecialAllowance,
A.SpareAllowance,
A.Remarks,
A.TotalSalary.
A.TotalDeductedSalary
FROM Allowance A
INNER JOIN Deduction D ON A.YearMonth = D.YearMonth
Where YearMonth = @YearMonth";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("YearMonth", year + "-" + month.ToString("D2") + "-" + "01"),
            };

            return SQLiteHelper.QuerySingle<AllowanceValueEntity>(
                sql,
                args.ToArray(),
                reader =>
                {
                    return new AllowanceValueEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["BasicSalary"]),
                                Convert.ToDouble(reader["ExecutiveAllowance"]),
                                Convert.ToDouble(reader["DependencyAllowance"]),
                                Convert.ToDouble(reader["OvertimeAllowance"]),
                                Convert.ToDouble(reader["DaysoffIncreased"]),
                                Convert.ToDouble(reader["NightworkIncreased"]),
                                Convert.ToDouble(reader["HousingAllowance"]),
                                Convert.ToDouble(reader["LateAbsent"]),
                                Convert.ToDouble(reader["TransportationExpenses"]),
                                Convert.ToDouble(reader["PrepaidRetirementPayment"]),
                                Convert.ToDouble(reader["ElectricityAllowance"]),
                                Convert.ToDouble(reader["SpecialAllowance"]),
                                Convert.ToDouble(reader["SpareAllowance"]),
                                Convert.ToString(reader["Remarks"]),
                                Convert.ToDouble(reader["TotalSalary"]),
                                Convert.ToDouble(reader["TotalDeductedSalary"]));
                },
                null);
        }

        /// <summary>
        /// Get - 支給額(デフォルト)
        /// </summary>
        /// <returns>支給額</returns>
        public AllowanceValueEntity GetDefault()
        {
            string sql = @"
SELECT A.Id, 
A.YearMonth, 
A.BasicSalary, 
A.ExecutiveAllowance, 
A.DependencyAllowance, 
A.OvertimeAllowance, 
A.DaysoffIncreased, 
A.NightworkIncreased ,
A.HousingAllowance,
A.LateAbsent,
A.TransportationExpenses,
A.PrepaidRetirementPayment,
A.ElectricityAllowance,
A.SpecialAllowance,
A.SpareAllowance,
A.Remarks,
A.TotalSalary,
A.TotalDeductedSalary
FROM Allowance A
INNER JOIN YearMonth YM ON A.YearMonth = YM.YearMonth
INNER JOIN Deduction D ON A.YearMonth = D.YearMonth
WHERE YM.IsDefault = True";

            return SQLiteHelper.QuerySingle<AllowanceValueEntity>(
                sql,
                reader =>
                {
                    return new AllowanceValueEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["BasicSalary"]),
                                Convert.ToDouble(reader["ExecutiveAllowance"]),
                                Convert.ToDouble(reader["DependencyAllowance"]),
                                Convert.ToDouble(reader["OvertimeAllowance"]),
                                Convert.ToDouble(reader["DaysoffIncreased"]),
                                Convert.ToDouble(reader["NightworkIncreased"]),
                                Convert.ToDouble(reader["HousingAllowance"]),
                                Convert.ToDouble(reader["LateAbsent"]),
                                Convert.ToDouble(reader["TransportationExpenses"]),
                                Convert.ToDouble(reader["PrepaidRetirementPayment"]),
                                Convert.ToDouble(reader["ElectricityAllowance"]),
                                Convert.ToDouble(reader["SpecialAllowance"]),
                                Convert.ToDouble(reader["SpareAllowance"]),
                                Convert.ToString(reader["Remarks"]),
                                Convert.ToDouble(reader["TotalSalary"]),
                                Convert.ToDouble(reader["TotalDeductedSalary"]));
                },
                null);
        }

        public void Save(
            SQLiteTransaction transaction,
            AllowanceValueEntity entity)
        {
            string insert = @"
insert into Allowance
(Id, 
YearMonth, 
BasicSalary, 
ExecutiveAllowance, 
DependencyAllowance, 
OvertimeAllowance, 
DaysoffIncreased, 
NightworkIncreased, 
HousingAllowance, 
LateAbsent, 
TransportationExpenses, 
PrepaidRetirementPayment, 
ElectricityAllowance, 
SpecialAllowance, 
SpareAllowance, 
TotalSalary,
TotalDeductedSalary,
Remarks)
values
(@Id, 
@YearMonth, 
@BasicSalary, 
@ExecutiveAllowance, 
@DependencyAllowance, 
@OvertimeAllowance, 
@DaysoffIncreased, 
@NightworkIncreased, 
@HousingAllowance,
@LateAbsent, 
@PrepaidRetirementPayment, 
@TransportationExpenses, 
@ElectricityAllowance, 
@SpecialAllowance, 
@SpareAllowance, 
@TotalSalary,
@TotalDeductedSalary,
@Remarks)
";

            string update = @"
update Allowance
set YearMonth              = @YearMonth, 
    BasicSalary            = @BasicSalary, 
    ExecutiveAllowance     = @ExecutiveAllowance, 
    DependencyAllowance    = @DependencyAllowance, 
    OvertimeAllowance      = @OvertimeAllowance,  
    DaysoffIncreased       = @DaysoffIncreased,  
    NightworkIncreased     = @NightworkIncreased,  
    HousingAllowance       = @HousingAllowance,  
    LateAbsent             = @LateAbsent,  
    TransportationExpenses = @TransportationExpenses,  
    PrepaidRetirementPayment = @PrepaidRetirementPayment,  
    ElectricityAllowance   = @ElectricityAllowance,  
    SpecialAllowance       = @SpecialAllowance,  
    SpareAllowance         = @SpareAllowance,  
    TotalSalary            = @TotalSalary,
    TotalDeductedSalary    = @TotalDeductedSalary,
    Remarks                = @Remarks
where Id = @Id
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("Id", entity.ID),
                new SQLiteParameter("YearMonth", DateUtils.ConvertToSQLiteValue(entity.YearMonth)),
                new SQLiteParameter("BasicSalary", entity.BasicSalary.Value),
                new SQLiteParameter("ExecutiveAllowance", entity.ExecutiveAllowance.Value),
                new SQLiteParameter("DependencyAllowance", entity.DependencyAllowance.Value),
                new SQLiteParameter("OvertimeAllowance", entity.OvertimeAllowance.Value),
                new SQLiteParameter("DaysoffIncreased", entity.DaysoffIncreased.Value),
                new SQLiteParameter("NightworkIncreased", entity.NightworkIncreased.Value),
                new SQLiteParameter("HousingAllowance", entity.HousingAllowance),
                new SQLiteParameter("LateAbsent", entity.LateAbsent),
                new SQLiteParameter("TransportationExpenses", entity.TransportationExpenses.Value),
                new SQLiteParameter("PrepaidRetirementPayment", entity.PrepaidRetirementPayment.Value),
                new SQLiteParameter("ElectricityAllowance", entity.ElectricityAllowance.Value),
                new SQLiteParameter("SpecialAllowance", entity.SpecialAllowance),
                new SQLiteParameter("SpareAllowance", entity.SpareAllowance),
                new SQLiteParameter("TotalSalary", entity.TotalSalary.Value),
                new SQLiteParameter("TotalDeductedSalary", entity.TotalDeductedSalary.Value),
                new SQLiteParameter("Remarks", entity.Remarks),
            };

            transaction.Execute(insert, update, args.ToArray());
        }

        public void Save(ITransactionRepository transaction, AllowanceValueEntity entity)
            => this.Save((SQLiteTransaction)transaction, entity);
    }
}
