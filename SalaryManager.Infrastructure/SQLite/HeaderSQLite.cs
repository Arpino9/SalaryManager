﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Logics;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQlite - ヘッダ
    /// </summary>
    public class HeaderSQLite : IHeaderRepository
    {
        /// <summary>
        /// Get - ヘッダ
        /// </summary>
        /// <returns>ヘッダ</returns>
        public IReadOnlyList<HeaderEntity> GetEntities()
        {
            string sql = @"
SELECT Id, 
YearMonth, 
IsDefault, 
CreateDate, 
UpdateDate
FROM YearMonth";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new HeaderEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToBoolean(reader["IsDefault"]),
                                Convert.ToDateTime(reader["CreateDate"]),
                                Convert.ToDateTime(reader["UpdateDate"]));
                });
        }

        public HeaderEntity GetDefaultEntity()
        {
            string sql = @"
SELECT Id, 
YearMonth, 
IsDefault, 
CreateDate, 
UpdateDate
FROM YearMonth
Where IsDefault = True";

            return SQLiteHelper.QuerySingle(
                sql,
                reader =>
                {
                    return new HeaderEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToBoolean(reader["IsDefault"]),
                                Convert.ToDateTime(reader["CreateDate"]),
                                Convert.ToDateTime(reader["UpdateDate"]));
                }, 
                null);
        }

        public void Save(HeaderEntity entity)
        {
            string insert = @"
insert into YearMonth
(Id, YearMonth, IsDefault, CreateDate, UpdateDate)
values
(@Id, @YearMonth, @IsDefault, @CreateDate, @UpdateDate)
";

            string update = @"
update YearMonth
set YearMonth  = @YearMonth, 
    IsDefault  = @IsDefault, 
    CreateDate = @CreateDate, 
    UpdateDate = @UpdateDate
where Id = @Id
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("Id", entity.ID),
                new SQLiteParameter("YearMonth", DateUtils.ConvertToSQLiteValue(entity.YearMonth)),
                new SQLiteParameter("IsDefault", entity.IsDefault),
                new SQLiteParameter("CreateDate", entity.CreateDate),
                new SQLiteParameter("UpdateDate", entity.UpdateDate),
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }
    }
}
