using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQLite - 就業場所
    /// </summary>
    public class WorkingPlaceSQLite : IWorkingPlaceRepository
    {
        public IReadOnlyList<WorkingPlaceEntity> GetEntities()
        {
            string sql = @"
SELECT ID, 
DispatchingCompany, 
DispatchedCompany, 
WorkingPlace_Name, 
WorkingPlace_Address, 
WorkingStart, 
WorkingEnd, 
IsWaiting, 
IsWorking, 
WorkingStartTime_Hour, 
WorkingStartTime_Minute, 
WorkingEndTime_Hour, 
WorkingEndTime_Minute, 
LunchStartTime_Hour, 
LunchStartTime_Minute, 
LunchEndTime_Hour, 
LunchEndTime_Minute, 
BreakStartTime_Hour, 
BreakStartTime_Minute, 
BreakEndTime_Hour, 
BreakEndTime_Minute, 
Remarks
FROM WorkingPlace";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new WorkingPlaceEntity(
                               Convert.ToInt32(reader["ID"]),
                               Convert.ToString(reader["DispatchingCompany"]),
                               Convert.ToString(reader["DispatchedCompany"]),
                               Convert.ToString(reader["WorkingPlace_Name"]),
                               Convert.ToString(reader["WorkingPlace_Address"]),
                               Convert.ToDateTime(reader["WorkingStart"]),
                               Convert.ToDateTime(reader["WorkingEnd"]),
                               Convert.ToBoolean(reader["IsWaiting"]),
                               Convert.ToBoolean(reader["IsWorking"]),
                               Convert.ToInt32(reader["WorkingStartTime_Hour"]), 
                               Convert.ToInt32(reader["WorkingStartTime_Minute"]),
                               Convert.ToInt32(reader["WorkingEndTime_Hour"]), 
                               Convert.ToInt32(reader["WorkingEndTime_Minute"]),
                               Convert.ToInt32(reader["LunchStartTime_Hour"]), 
                               Convert.ToInt32(reader["LunchStartTime_Minute"]),
                               Convert.ToInt32(reader["LunchEndTime_Hour"]), 
                               Convert.ToInt32(reader["LunchEndTime_Minute"]),
                               Convert.ToInt32(reader["BreakStartTime_Hour"]), 
                               Convert.ToInt32(reader["BreakStartTime_Minute"]),
                               Convert.ToInt32(reader["BreakEndTime_Hour"]), 
                               Convert.ToInt32(reader["BreakEndTime_Minute"]),
                               Convert.ToString(reader["Remarks"]));
                });
        }

        /// <summary>
        /// Get - 就業場所
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>就業場所</returns>
        public WorkingPlaceEntity GetEntity(int id)
        {
            string sql = @"
SELECT ID, 
DispatchingCompany, 
WorkingPlace_Name, 
WorkingPlace_Address, 
WorkingStart, 
WorkingEnd, 
IsWaiting, 
IsWorking, 
WorkingStartTime_Hour, 
WorkingStartTime_Minute, 
WorkingEndTime_Hour, 
WorkingEndTime_Minute, 
LunchStartTime_Hour, 
LunchStartTime_Minute, 
LunchEndTime_Hour, 
LunchEndTime_Minute, 
BreakStartTime_Hour, 
BreakStartTime_Minute, 
BreakEndTime_Hour, 
BreakEndTime_Minute, 
Remarks
FROM WorkingPlace
Where ID = @ID";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("ID", id),
            };

            return SQLiteHelper.QuerySingle<WorkingPlaceEntity>(
                sql,
                args.ToArray(),
                reader =>
                {
                    return new WorkingPlaceEntity(
                               Convert.ToInt32(reader["ID"]),
                               Convert.ToString(reader["DispatchingCompany"]),
                               Convert.ToString(reader["DispatchedCompany"]),
                               Convert.ToString(reader["WorkingPlace_Name"]),
                               Convert.ToString(reader["WorkingPlace_Address"]),
                               Convert.ToDateTime(reader["WorkingStart"]),
                               Convert.ToDateTime(reader["WorkingEnd"]),
                               Convert.ToBoolean(reader["IsWaiting"]),
                               Convert.ToBoolean(reader["IsWorking"]),
                               Convert.ToInt32(reader["WorkingStartTime_Hour"]), 
                               Convert.ToInt32(reader["WorkingEndTime_Minute"]),
                               Convert.ToInt32(reader["WorkingEndTime_Hour"]),
                               Convert.ToInt32(reader["WorkingEndTime_Minute"]),
                               Convert.ToInt32(reader["LunchStartTime_Hour"]),
                               Convert.ToInt32(reader["LunchStartTime_Minute"]),
                               Convert.ToInt32(reader["LunchEndTime_Hour"]),
                               Convert.ToInt32(reader["LunchEndTime_Minute"]),
                               Convert.ToInt32(reader["BreakStartTime_Hour"]), 
                               Convert.ToInt32(reader["BreakStartTime_Minute"]),
                               Convert.ToInt32(reader["BreakEndTime_Hour"]), 
                               Convert.ToInt32(reader["BreakEndTime_Minute"]),
                               Convert.ToString(reader["Remarks"]));
                },
                null);
        }

        public void Save(
            WorkingPlaceEntity entity)
        {
            string insert = @"
insert into WorkingPlace
(ID,
DispatchingCompany,
DispatchedCompany,
WorkingPlace_Name, 
WorkingPlace_Address, 
WorkingStart, 
WorkingEnd, 
IsWaiting, 
IsWorking, 
WorkingStartTime_Hour, 
WorkingStartTime_Minute, 
WorkingEndTime_Hour, 
WorkingEndTime_Minute, 
LunchStartTime_Hour, 
LunchStartTime_Minute, 
LunchEndTime_Hour, 
LunchEndTime_Minute, 
BreakStartTime_Hour, 
BreakStartTime_Minute, 
BreakEndTime_Hour, 
BreakEndTime_Minute, 
Remarks)
values
(@ID, 
@DispatchingCompany, 
@DispatchedCompany,
@WorkingPlace_Name, 
@WorkingPlace_Address, 
@WorkingStart, 
@WorkingEnd, 
@IsWaiting, 
@IsWorking, 
@WorkingStartTime_Hour, 
@WorkingStartTime_Minute, 
@WorkingEndTime_Hour, 
@WorkingEndTime_Minute, 
@LunchStartTime_Hour, 
@LunchStartTime_Minute, 
@LunchEndTime_Hour, 
@LunchEndTime_Minute, 
@BreakStartTime_Hour, 
@BreakStartTime_Minute, 
@BreakEndTime_Hour, 
@BreakEndTime_Minute, 
@Remarks)
";

            string update = @"
update WorkingPlace
set ID                      = @ID, 
    DispatchingCompany      = @DispatchingCompany,
    DispatchedCompany       = @DispatchedCompany,
    WorkingPlace_Name       = @WorkingPlace_Name, 
    WorkingPlace_Address    = @WorkingPlace_Address, 
    WorkingStart            = @WorkingStart, 
    WorkingEnd              = @WorkingEnd, 
    IsWaiting               = @IsWaiting, 
    IsWorking               = @IsWorking, 
    WorkingStartTime_Hour   = @WorkingStartTime_Hour, 
    WorkingStartTime_Minute = @WorkingStartTime_Minute, 
    WorkingEndTime_Hour     = @WorkingEndTime_Hour, 
    WorkingEndTime_Minute   = @WorkingEndTime_Minute, 
    LunchStartTime_Hour     = @LunchStartTime_Hour, 
    LunchStartTime_Minute   = @LunchStartTime_Minute,  
    LunchEndTime_Hour       = @LunchEndTime_Hour,  
    LunchEndTime_Minute     = @LunchEndTime_Minute,  
    BreakStartTime_Hour     = @BreakStartTime_Hour,  
    BreakStartTime_Minute   = @BreakStartTime_Minute,  
    BreakEndTime_Hour       = @BreakEndTime_Hour,  
    BreakEndTime_Minute     = @BreakEndTime_Minute,  
    Remarks                 = @Remarks
where ID = @ID
";
            var workingTime = entity.WorkingTime;
            var lunchTime   = entity.LunchTime;
            var breakTime   = entity.BreakTime;

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("ID",                      entity.ID),
                new SQLiteParameter("DispatchingCompany",      entity.DispatchingCompany.Text),
                new SQLiteParameter("DispatchedCompany",       entity.DispatchedCompany.Text),
                new SQLiteParameter("WorkingPlace_Name",       entity.WorkingPlace_Name.Text),
                new SQLiteParameter("WorkingPlace_Address",    entity.WorkingPlace_Address),
                new SQLiteParameter("WorkingStart",            entity.WorkingStart.ConvertToSQLiteDate()),
                new SQLiteParameter("WorkingEnd",              entity.WorkingEnd.ConvertToSQLiteDate()),
                new SQLiteParameter("IsWaiting",               entity.IsWaiting),
                new SQLiteParameter("IsWorking",               entity.IsWorking),
                new SQLiteParameter("WorkingStartTime_Hour",   workingTime.Start.Hours),
                new SQLiteParameter("WorkingStartTime_Minute", workingTime.Start.Minutes),
                new SQLiteParameter("WorkingEndTime_Hour",     workingTime.End.Hours),
                new SQLiteParameter("WorkingEndTime_Minute",   workingTime.End.Minutes),
                new SQLiteParameter("LunchStartTime_Hour",     lunchTime.Start.Hours),
                new SQLiteParameter("LunchStartTime_Minute",   lunchTime.Start.Minutes),
                new SQLiteParameter("LunchEndTime_Hour",       lunchTime.End.Hours),
                new SQLiteParameter("LunchEndTime_Minute",     lunchTime.End.Minutes),
                new SQLiteParameter("BreakStartTime_Hour",     breakTime.Start.Hours),
                new SQLiteParameter("BreakStartTime_Minute",   breakTime.Start.Minutes),
                new SQLiteParameter("BreakEndTime_Hour",       breakTime.End.Hours),
                new SQLiteParameter("BreakEndTime_Minute",     breakTime.End.Minutes),
                new SQLiteParameter("Remarks",                 entity.Remarks),
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }

        /// <summary>
        /// 派遣元会社名の更新
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <param name="oldName">更新前の名称</param>
        /// <param name="newName">更新後の名称</param>
        public void UpdateDispatchingCompanyName(SQLiteTransaction transaction, string oldName, string newName)
        {
            string update = @"
update WorkingPlace
set DispatchingCompany   = @NewCompanyName
where DispatchingCompany = @OldCompanyName
";

            this.UpdateCompanyName(transaction, update, oldName, newName);
        }

        /// <summary>
        /// 派遣先会社名の更新
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <param name="oldName">更新前の名称</param>
        /// <param name="newName">更新後の名称</param>
        public void UpdateDispatchedCompanyName(SQLiteTransaction transaction, string oldName, string newName)
        {
            string update = @"
update WorkingPlace
set DispatchedCompany   = @NewCompanyName
where DispatchedCompany = @OldCompanyName
";

            this.UpdateCompanyName(transaction, update, oldName, newName);
        }

        /// <summary>
        /// 就業先(名称)の更新
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <param name="oldName">更新前の名称</param>
        /// <param name="newName">更新後の名称</param>
        public void UpdateWorkingPlaceName(SQLiteTransaction transaction, string oldName, string newName)
        {
            string update = @"
update WorkingPlace
set WorkingPlace_Name   = @NewCompanyName
where WorkingPlace_Name = @OldCompanyName
";

            this.UpdateCompanyName(transaction, update, oldName, newName);
        }

        /// <summary>
        /// 会社名の更新
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <param name="updateSQL">UPDATE文</param>
        /// <param name="oldName">更新前の名称</param>
        /// <param name="newName">更新後の名称</param>
        public void UpdateCompanyName(
            SQLiteTransaction transaction, string updateSQL, string oldName, string newName)
        {
            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("OldCompanyName", oldName),
                new SQLiteParameter("NewCompanyName", newName),
            };

            transaction.Execute(updateSQL, args.ToArray());
        }

        /// <summary>
        /// 会社名の更新
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <param name="oldName">更新前の名称</param>
        /// <param name="newName">更新後の名称</param>
        public void UpdateCompanyName(ITransactionRepository transaction, string oldName, string newName)
        {
            this.UpdateDispatchingCompanyName((SQLiteTransaction)transaction, oldName, newName);
            this.UpdateDispatchedCompanyName((SQLiteTransaction)transaction, oldName, newName);
            this.UpdateWorkingPlaceName((SQLiteTransaction)transaction, oldName, newName);
        }

        /// <summary>
        /// 会社の住所の更新
        /// </summary>
        /// <param name="oldAddress">更新前の名称</param>
        /// <param name="newAddress">更新後の名称</param>
        public void UpdateCompanyAddress(string oldAddress, string newAddress)
        {
            string update = @"
update WorkingPlace
set WorkingPlace_Address   = @NewCompanyAddress
where WorkingPlace_Address = @OldCompanyAddress
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("OldCompanyAddress", oldAddress),
                new SQLiteParameter("NewCompanyAddress", newAddress),
            };

            SQLiteHelper.Execute(update, args.ToArray());
        }

        public void Delete(
            int id)
        {
            string delete = @"
Delete From WorkingPlace
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
