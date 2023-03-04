using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQLite - 副業額
    /// </summary>
    public class AnnualChartSQLite : IAnnualChartRepository
    {
        public IReadOnlyList<AnnualChartEntity> GetEntities()
        {
            string sql = @"
SELECT A.Id, 
A.YearMonth, 
A.TotalSalary,
A.TotalDeductedSalary,
S.SideBusiness + S.Perquisite + S.Others AS TotalSideBusiness
FROM Allowance A
INNER JOIN SideBusiness S ON A.YearMonth = S.YearMonth";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new AnnualChartEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToInt32(reader["TotalSalary"]),
                                Convert.ToInt32(reader["TotalDeductedSalary"]),
                                Convert.ToInt32(reader["TotalSideBusiness"]));
                });
        }
    }
}
