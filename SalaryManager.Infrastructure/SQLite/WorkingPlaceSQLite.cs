using SalaryManager.Domain.Entities;
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
CompanyName, 
Address, 
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
                               Convert.ToString(reader["CompanyName"]),
                               Convert.ToString(reader["Address"]),
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
CompanyName, 
Address, 
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
                               Convert.ToString(reader["CompanyName"]),
                               Convert.ToString(reader["Address"]),
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
CompanyName, 
Address, 
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
@CompanyName, 
@DispatchingCompany,
@Address, 
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
    CompanyName             = @CompanyName, 
    DispatchingCompany      = @DispatchingCompany,
    Address                 = @Address, 
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
                new SQLiteParameter("CompanyName",             entity.CompanyName.Text),
                new SQLiteParameter("Address",                 entity.Address),
                new SQLiteParameter("WorkingStartTime_Hour",   workingTime.Start.Hour),
                new SQLiteParameter("WorkingStartTime_Minute", workingTime.Start.Minute),
                new SQLiteParameter("WorkingEndTime_Hour",     workingTime.End.Hour),
                new SQLiteParameter("WorkingEndTime_Minute",   workingTime.End.Minute),
                new SQLiteParameter("LunchStartTime_Hour",     lunchTime.Start.Hour),
                new SQLiteParameter("LunchStartTime_Minute",   lunchTime.Start.Minute),
                new SQLiteParameter("LunchEndTime_Hour",       lunchTime.End.Hour),
                new SQLiteParameter("LunchEndTime_Minute",     lunchTime.End.Minute),
                new SQLiteParameter("BreakStartTime_Hour",     breakTime.Start.Hour),
                new SQLiteParameter("BreakStartTime_Minute",   breakTime.Start.Minute),
                new SQLiteParameter("BreakEndTime_Hour",       breakTime.End.Hour),
                new SQLiteParameter("BreakEndTime_Minute",     breakTime.End.Minute),
                new SQLiteParameter("Remarks",                 entity.Remarks),
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
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
