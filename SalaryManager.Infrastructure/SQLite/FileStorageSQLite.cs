using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SalaryManager.Infrastructure.SQLite
{
    public class FileStorageSQLite : IFileStorageRepository
    {
        public IReadOnlyList<FileStorageEntity> GetEntities()
        {
            string sql = @"
SELECT ID, 
Title, 
FileName, 
Image, 
Remarks, 
CreateDate, 
UpdateDate
FROM FileStorage";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    byte[] image = null;
                    if (!reader["Image"].Equals(DBNull.Value))
                    {
                        image = (byte[])reader["Image"];
                    }

                    return new FileStorageEntity(
                        Convert.ToInt32(reader["ID"]),
                        Convert.ToString(reader["Title"]),
                        Convert.ToString(reader["FileName"]),
                        image,
                        Convert.ToString(reader["Remarks"]),
                        Convert.ToDateTime(reader["CreateDate"]),
                        Convert.ToDateTime(reader["UpdateDate"]));
                });
        }

        public void Save(
            FileStorageEntity entity)
        {
            string insert = @"
insert into FileStorage
(ID,
Title, 
FileName, 
Image, 
Remarks, 
CreateDate, 
UpdateDate)
values
(@ID, 
@Title, 
@FileName, 
@Image, 
@Remarks, 
@CreateDate, 
@UpdateDate)
";

            string update = @"
update FileStorage
set ID                = @ID, 
    Title             = @Title, 
    FileName          = @FileName, 
    Image             = @Image, 
    Remarks           = @Remarks, 
    CreateDate        = @CreateDate, 
    UpdateDate        = @UpdateDate
where ID = @ID
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("ID",         entity.ID),
                new SQLiteParameter("Title",      entity.Title),
                new SQLiteParameter("FileName",   entity.FileName),
                new SQLiteParameter("Image",      entity.Image),
                new SQLiteParameter("Remarks",    entity.Remarks),
                new SQLiteParameter("CreateDate", DateUtils.ConvertToSQLiteValue(entity.CreateDate.Year, entity.CreateDate.Month, entity.CreateDate.Day)),
                new SQLiteParameter("UpdateDate", DateUtils.ConvertToSQLiteValue(entity.UpdateDate.Year, entity.UpdateDate.Month, entity.UpdateDate.Day)),
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }

        public void Delete(
            int id)
        {
            string delete = @"
Delete From FileStorage
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
