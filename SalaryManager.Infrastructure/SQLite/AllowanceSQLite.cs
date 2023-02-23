using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Logics;
using SalaryManager.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SalaryManager.Infrastructure.SQLite
{
    public class AllowanceSQLite : IAllowanceRepository
    {
        public IReadOnlyList<AllowanceEntity> GetEntities()
        {
            string sql = @"
SELECT Id, 
YearMonth, 
BasicSalary, 
ExecutiveAllowance, 
DependencyAllowance, 
OvertimeAllowance, 
DaysoffIncreased, 
NightworkIncreased ,
HousingAllowance,
LateAbsent,
TransportationExpenses,
SpecialAllowance,
SpareAllowance,
Remarks,
TotalSalary,
TotalDeductedSalary
FROM Allowance";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new AllowanceEntity(
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
        public AllowanceEntity GetEntiity(int year, int month)
        {
            string sql = @"
SELECT Id, 
YearMonth, 
BasicSalary, 
ExecutiveAllowance, 
DependencyAllowance, 
OvertimeAllowance, 
DaysoffIncreased, 
NightworkIncreased ,
HousingAllowance,
LateAbsent,
TransportationExpenses,
SpecialAllowance,
SpareAllowance,
Remarks,
TotalSalary,
TotalDeductedSalary
FROM Allowance
Where YearMonth = @YearMonth";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("YearMonth", year + "-" + month.ToString("D2") + "-" + "01"),
            };

            return SQLiteHelper.QuerySingle<AllowanceEntity>(
                sql,
                args.ToArray(),
                reader =>
                {
                    return new AllowanceEntity(
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
                                Convert.ToDouble(reader["SpecialAllowance"]),
                                Convert.ToDouble(reader["SpareAllowance"]),
                                Convert.ToString(reader["Remarks"]),
                                Convert.ToDouble(reader["TotalSalary"]),
                                Convert.ToDouble(reader["TotalDeductedSalary"]));
                },
                null);
        }

        public void Save(AllowanceEntity entity)
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
SpecialAllowance, 
SpareAllowance, 
Remarks, 
TotalSalary, 
TotalDeductedSalary)
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
@TransportationExpenses, 
@SpecialAllowance, 
@SpareAllowance, 
@Remarks, 
@TotalSalary, 
@TotalDeductedSalary)
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
    SpecialAllowance       = @SpecialAllowance,  
    SpareAllowance         = @SpareAllowance,  
    Remarks                = @Remarks,  
    TotalSalary            = @TotalSalary,  
    TotalDeductedSalary    = @TotalDeductedSalary
where Id = @Id
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("Id", entity.ID),
                new SQLiteParameter("YearMonth", DateUtils.ConvertToSQLiteValue(entity.YearMonth)),
                new SQLiteParameter("BasicSalary", entity.BasicSalary),
                new SQLiteParameter("ExecutiveAllowance", entity.ExecutiveAllowance),
                new SQLiteParameter("DependencyAllowance", entity.DependencyAllowance),
                new SQLiteParameter("OvertimeAllowance", entity.OvertimeAllowance),
                new SQLiteParameter("DaysoffIncreased", entity.DaysoffIncreased),
                new SQLiteParameter("NightworkIncreased", entity.NightworkIncreased),
                new SQLiteParameter("HousingAllowance", entity.HousingAllowance),
                new SQLiteParameter("LateAbsent", entity.LateAbsent),
                new SQLiteParameter("TransportationExpenses", entity.TransportationExpenses),
                new SQLiteParameter("SpecialAllowance", entity.SpecialAllowance),
                new SQLiteParameter("SpareAllowance", entity.SpareAllowance),
                new SQLiteParameter("Remarks", entity.Remarks),
                new SQLiteParameter("TotalSalary", entity.TotalSalary),
                new SQLiteParameter("TotalDeductedSalary", entity.TotalDeductedSalary),
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }
    }
}
