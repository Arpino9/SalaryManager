using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace SalaryManager.Infrastructure.SQLite
{
    public class AnnualChartSQLite : IAnnualChartRepository
    {
        public IReadOnlyList<AnnualChartEntity> GetEntities()
        {
            string sql = @"
SELECT Id, 
YearMonth, 
TotalSalary,
TotalDeductedSalary
FROM Allowance";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new AnnualChartEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToInt32(reader["TotalSalary"]),
                                Convert.ToInt32(reader["TotalDeductedSalary"]));
                });
        }
    }
}
