using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQLite - 会社
    /// </summary>
    public class CompanySQLite : ICompanyRepository
    {
        public IReadOnlyList<CompanyEntity> GetEntities()
        {
            string sql = @"
SELECT ID, 
BusinessCategory, 
CompanyName, 
PostCode, 
Address, 
Address_Google, 
Remarks
FROM Company";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new CompanyEntity(
                                Convert.ToInt32(reader["ID"]),
                                Convert.ToInt32(reader["BusinessCategory"]),
                                Convert.ToString(reader["CompanyName"]),
                                Convert.ToString(reader["PostCode"]),
                                Convert.ToString(reader["Address"]),
                                Convert.ToString(reader["Address_Google"]),
                                Convert.ToString(reader["Remarks"]));
                });
        }

        public CompanyEntity GetEntity(int id)
        {
            string sql = @"
SELECT ID, 
BusinessCategory, 
CompanyName, 
PostCode, 
Address, 
Address_Google, 
Remarks
FROM Company
Where ID = @ID";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("ID", id),
            };

            return SQLiteHelper.QuerySingle<CompanyEntity>(
                sql,
                args.ToArray(),
                reader =>
                {
                    return new CompanyEntity(
                                Convert.ToInt32(reader["ID"]),
                                Convert.ToInt32(reader["BusinessCategory"]),
                                Convert.ToString(reader["CompanyName"]),
                                Convert.ToString(reader["PostCode"]),
                                Convert.ToString(reader["Address"]),
                                Convert.ToString(reader["Address_Google"]),
                                Convert.ToString(reader["Remarks"]));
                },
                null);
        }

        public void Save(CompanyEntity entity)
        {
            string insert = @"
insert into Company
(ID,
BusinessCategory, 
CompanyName, 
PostCode, 
Address, 
Address_Google, 
Remarks)
values
(@ID, 
@BusinessCategory, 
@CompanyName, 
@PostCode, 
@Address, 
@Address_Google, 
@Remarks)
";

            string update = @"
update Company
set ID                = @ID, 
    BusinessCategory  = @BusinessCategory, 
    CompanyName       = @CompanyName, 
    PostCode          = @PostCode, 
    Address           = @Address, 
    Address_Google    = @Address_Google, 
    Remarks           = @Remarks
where ID = @ID
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("ID",                entity.ID),
                new SQLiteParameter("BusinessCategory",  int.Parse(entity.BusinessCategory.MiddleNo)),
                new SQLiteParameter("CompanyName",       entity.CompanyName),
                new SQLiteParameter("PostCode",          entity.PostCode),
                new SQLiteParameter("Address",           entity.Address),
                new SQLiteParameter("Address_Google",    entity.Address_Google),
                new SQLiteParameter("Remarks",           entity.Remarks),
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }

        public void Delete(int id)
        {
            string delete = @"
Delete From Company
where ID = @ID
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("ID", id),
            };

            SQLiteHelper.Execute(delete, args.ToArray());
        }
    }
}
