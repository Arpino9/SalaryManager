using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQLite - 副業
    /// </summary>
    public class SideBusinessSQLite : ISideBusinessRepository
    {
        public IReadOnlyList<SideBusinessEntity> GetEntities()
        {
            string sql = @"
SELECT Id, 
YearMonth, 
SideBusiness, 
Perquisite, 
Others, 
Remarks
from SideBusiness";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new SideBusinessEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["SideBusiness"]),
                                Convert.ToDouble(reader["Perquisite"]),
                                Convert.ToDouble(reader["Others"]),
                                Convert.ToString(reader["Remarks"]));
                });
        }

        public SideBusinessEntity GetEntity(int year, int month)
        {
            string sql = @"
SELECT Id, 
YearMonth, 
SideBusiness, 
Perquisite, 
Others, 
Remarks
from SideBusiness
Where YearMonth = @YearMonth";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("YearMonth", year + "-" + month.ToString("D2") + "-" + "01"),
            };

            return SQLiteHelper.QuerySingle<SideBusinessEntity>(
                sql,
                args.ToArray(),
                reader =>
                {
                    return new SideBusinessEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["SideBusiness"]),
                                Convert.ToDouble(reader["Perquisite"]),
                                Convert.ToDouble(reader["Others"]),
                                Convert.ToString(reader["Remarks"]));
                },
                null);
        }

        /// <summary>
        /// Get - 勤務備考(デフォルト)
        /// </summary>
        /// <returns>勤務備考</returns>
        public SideBusinessEntity GetDefault()
        {
            string sql = @"
SELECT S.Id, 
S.YearMonth, 
S.SideBusiness, 
S.Perquisite, 
S.Others, 
S.Remarks
FROM SideBusiness S
INNER JOIN YearMonth YM ON S.YearMonth = YM.YearMonth
WHERE YM.IsDefault = True";

            return SQLiteHelper.QuerySingle<SideBusinessEntity>(
                sql,
                reader =>
                {
                    return new SideBusinessEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["SideBusiness"]),
                                Convert.ToDouble(reader["Perquisite"]),
                                Convert.ToDouble(reader["Others"]),
                                Convert.ToString(reader["Remarks"]));
                },
                null);
        }

        public void Save(SQLiteTransaction transaction, SideBusinessEntity entity)
        {
            string insert = @"
insert into SideBusiness
(Id, YearMonth, SideBusiness, Perquisite, Others, Remarks)
values
(@Id, @YearMonth, @SideBusiness, @Perquisite, @Others, @Remarks)
";

            string update = @"
update SideBusiness
set YearMonth    = @YearMonth, 
    SideBusiness = @SideBusiness, 
    Perquisite   = @Perquisite, 
    Others       = @Others, 
    Remarks      = @Remarks
where Id = @Id
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("Id", entity.ID),
                new SQLiteParameter("YearMonth", DateHelpers.ConvertToSQLiteValue(entity.YearMonth)),
                new SQLiteParameter("SideBusiness", entity.SideBusiness),
                new SQLiteParameter("Perquisite", entity.Perquisite),
                new SQLiteParameter("Others", entity.Others),
                new SQLiteParameter("Remarks", entity.Remarks),
            };

            transaction.Execute(insert, update, args.ToArray());
        }
    }
}
