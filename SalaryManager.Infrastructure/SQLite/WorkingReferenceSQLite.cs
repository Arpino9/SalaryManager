using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Logics;
using SalaryManager.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQLite - 勤務備考
    /// </summary>
    public class WorkingReferenceSQLite : IWorkingReferencesRepository
    {
        public IReadOnlyList<WorkingReferencesEntity> GetEntities()
        {
            string sql = @"
SELECT Id, 
YearMonth, 
OvertimeTime, 
WeekendWorktime, 
MidnightWorktime, 
LateAbsentH, 
Insurance, 
Norm, 
NumberOfDependent,
PaidVacation,
WorkingHours,
WorkPlace,
Remarks
from WorkingReference";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new WorkingReferencesEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["OvertimeTime"]),
                                Convert.ToDouble(reader["WeekendWorktime"]),
                                Convert.ToDouble(reader["MidnightWorktime"]),
                                Convert.ToDouble(reader["LateAbsentH"]),
                                Convert.ToDouble(reader["Insurance"]),
                                Convert.ToDouble(reader["Norm"]),
                                Convert.ToDouble(reader["NumberOfDependent"]),
                                Convert.ToDouble(reader["PaidVacation"]),
                                Convert.ToDouble(reader["WorkingHours"]),
                                Convert.ToString(reader["WorkPlace"]),
                                Convert.ToString(reader["Remarks"]));
                });
        }

        public WorkingReferencesEntity GetEntity(int year, int month)
        {
            string sql = @"
SELECT Id, 
YearMonth, 
OvertimeTime, 
WeekendWorktime, 
MidnightWorktime, 
LateAbsentH, 
Insurance, 
Norm, 
NumberOfDependent,
PaidVacation,
WorkingHours,
WorkPlace,
Remarks
from WorkingReference
Where YearMonth = @YearMonth";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("YearMonth", year + "-" + month.ToString("D2") + "-" + "01"),
            };

            return SQLiteHelper.QuerySingle<WorkingReferencesEntity>(
                sql,
                args.ToArray(),
                reader =>
                {
                    return new WorkingReferencesEntity(
                               Convert.ToInt32(reader["Id"]),
                               Convert.ToDateTime(reader["YearMonth"]),
                               Convert.ToDouble(reader["OvertimeTime"]),
                               Convert.ToDouble(reader["WeekendWorktime"]),
                               Convert.ToDouble(reader["MidnightWorktime"]),
                               Convert.ToDouble(reader["LateAbsentH"]),
                               Convert.ToDouble(reader["Insurance"]),
                               Convert.ToDouble(reader["Norm"]),
                               Convert.ToDouble(reader["NumberOfDependent"]),
                               Convert.ToDouble(reader["PaidVacation"]),
                               Convert.ToDouble(reader["WorkingHours"]),
                               Convert.ToString(reader["WorkPlace"]),
                               Convert.ToString(reader["Remarks"]));
                },
                null);
        }

        public void Save(WorkingReferencesEntity entity)
        {
            string insert = @"
insert into WorkingReference
(Id, 
YearMonth, 
OvertimeTime, 
WeekendWorktime, 
MidnightWorktime, 
LateAbsentH, 
Insurance, 
Norm, 
NumberOfDependent,
PaidVacation,
WorkingHours,
WorkPlace,
Remarks)
values
(@Id, 
@YearMonth, 
@OvertimeTime, 
@WeekendWorktime, 
@MidnightWorktime, 
@LateAbsentH, 
@Insurance, 
@Norm, 
@NumberOfDependent,
@PaidVacation,
@WorkingHours,
@WorkPlace,
@Remarks)
";

            string update = @"
update WorkingReference
set YearMonth         = @YearMonth, 
    OvertimeTime      = @OvertimeTime, 
    WeekendWorktime   = @WeekendWorktime, 
    MidnightWorktime  = @MidnightWorktime, 
    LateAbsentH       = @LateAbsentH,  
    Insurance         = @Insurance,  
    Norm              = @Norm,  
    NumberOfDependent = @NumberOfDependent,  
    PaidVacation      = @PaidVacation,  
    WorkingHours      = @WorkingHours,  
    WorkPlace         = @WorkPlace,  
    Remarks           = @Remarks
where Id = @Id
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("Id", entity.ID),
                new SQLiteParameter("YearMonth", DateUtils.ConvertToSQLiteValue(entity.YearMonth)),
                new SQLiteParameter("OvertimeTime", entity.OvertimeTime),
                new SQLiteParameter("WeekendWorktime", entity.WeekendWorktime),
                new SQLiteParameter("MidnightWorktime", entity.MidnightWorktime),
                new SQLiteParameter("LateAbsentH", entity.LateAbsentH),
                new SQLiteParameter("Insurance", entity.Insurance.Value),
                new SQLiteParameter("Norm", entity.Norm),
                new SQLiteParameter("NumberOfDependent", entity.NumberOfDependent),
                new SQLiteParameter("PaidVacation", entity.PaidVacation.Value),
                new SQLiteParameter("WorkingHours", entity.WorkingHours),
                new SQLiteParameter("WorkPlace", entity.WorkPlace),
                new SQLiteParameter("Remarks", entity.Remarks),
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }
    }
}
