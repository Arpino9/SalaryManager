using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Logics;
using SalaryManager.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.Infrastructure.SQLite
{
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

        public void Save(SideBusinessEntity entity)
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
                new SQLiteParameter("YearMonth", DateUtils.ConvertToSQLiteValue(entity.YearMonth)),
                new SQLiteParameter("SideBusiness", entity.SideBusiness),
                new SQLiteParameter("Perquisite", entity.Perquisite),
                new SQLiteParameter("Others", entity.Others),
                new SQLiteParameter("Remarks", entity.Remarks),
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }
    }
}
