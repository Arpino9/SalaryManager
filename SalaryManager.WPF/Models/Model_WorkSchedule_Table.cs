using System;
using System.Collections.Generic;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Exceptions;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Google_Calendar;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 勤怠表
    /// </summary>
    public class Model_WorkSchedule_Table
    {
        public Model_WorkSchedule_Table()
        {
            
        }

        #region Get Instance

        private static Model_WorkSchedule_Table model = null;

        public static Model_WorkSchedule_Table GetInstance()
        {
            if (model == null)
            {
                model = new Model_WorkSchedule_Table();
            }

            return model;
        }

        #endregion


        public void Initialize_Header()
        {
            this.ViewModel_Header.TargetDate = DateTime.Now;
        }

        internal void Return_Command()
        {
            this.ViewModel_Header.TargetDate = this.ViewModel_Header.TargetDate.AddMonths(-1);
            Initialize_Table();
        }

        internal void Proceed_Command()
        {
            this.ViewModel_Header.TargetDate = this.ViewModel_Header.TargetDate.AddMonths(1);
            Initialize_Table();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// データの読込が終わるまで再帰させる。
        /// </remarks>
        public async void Initialize_Table()
        {
            if (CalendarReader.Loading)
            {
                System.Threading.Thread.Sleep(3000);
                this.Initialize_Table();
            }

            this.ViewModel_Header.Year  = this.ViewModel_Header.TargetDate.Year.ToString();
            this.ViewModel_Header.Month = this.ViewModel_Header.TargetDate.Month.ToString();

            var year         = int.Parse(this.ViewModel_Header.Year);
            var month        = int.Parse(this.ViewModel_Header.Month);
            var lastMonthDay = DateTime.DaysInMonth(year, month);

            var (Noon, Lunch, Afternoon) = GetScheduleEvents(new DateTime(year, month, 1), new DateTime(year, month, lastMonthDay));

            if (Noon.Any()      == false || 
                Afternoon.Any() == false) 
            {
                throw new DatabaseException("スケジュールの取得に失敗しました。");
            }

            var date = new DateTime(year, month, 1);

            for (var i = 1; i <= lastMonthDay; i++)
            {
                this.InputFormattedDate(date);

                date = date.AddDays(1);
            }

            for (var day = 1; day <= lastMonthDay; day++)
            {
                var entities = Noon.Union(Afternoon).Union(Lunch)
                                   .Where(x => x.StartDate.Day == day).ToList();

                if (entities.Count < 2)
                {
                    // 休祝日
                    continue;
                }

                var inputDay = entities.First().StartDate.Day;

                // 始業
                this.InputStartTime(inputDay, this.GetStartTime(entities));

                // 昼休憩
                this.InputLunchTime(inputDay, this.GetLunchTime(entities));
                
                // 終業
                this.InputEndTime(inputDay, this.GetEndTime(entities));

                // 勤務時間
                this.InputWorkingTime(entities);

                // 残業時間
                this.InputOvertime(entities, day);

                // 備考
                this.InputRemarks(entities, day);
            }
        }

        /// <summary>
        /// 就業場所を取得する
        /// </summary>
        /// <returns>就業場所</returns>
        private List<WorkingPlaceEntity> GetWorkPlaces()
        {
            WorkingPlace.Create(new WorkingPlaceSQLite());
            Careers.Create(new CareerSQLite());
            Homes.Create(new HomeSQLite());

            var workingPlace = WorkingPlace.FetchByDescending();
            var company      = Careers.FetchBelongingCompany(new DateTime(2024, 4, 1));

            var list = new List<WorkingPlaceEntity>();

            foreach(var entity in workingPlace)
            {
                if (company.Where(x => x.CompanyName == entity.DispatchingCompany).Any())
                {
                    list.Add(entity);
                }
            }

            return list;
        }

        /// <summary>
        /// スケジュールのイベントを取得する
        /// </summary>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <returns>(午前, 昼休憩, 午後)</returns>
        /// <remarks>
        /// 登録された就業場所の住所、始業時刻、昼休憩、終業時刻を元にイベントを取得する。
        /// </remarks>
        private (List<CalendarEventEntity> Noon, List<CalendarEventEntity> Lunch, List<CalendarEventEntity> Afternoon) GetScheduleEvents(DateTime startDate, DateTime endDate)
        {
            var workingPlaces = this.GetWorkPlaces();

            var noon      = new List<CalendarEventEntity>();
            var lunch     = new List<CalendarEventEntity>();
            var afternoon = new List<CalendarEventEntity>();

            foreach(var entity in workingPlaces)
            {
                // 午前
                noon.AddRange(CalendarReader.FindByAddress(entity.WorkingPlace_Address, startDate, endDate,
                                                           entity.WorkingTime.Start, entity.LunchTime.Start));

                // 昼休憩
                lunch.AddRange(CalendarReader.FindByTitle("昼食", startDate, endDate));

                // 午後
                afternoon.AddRange(CalendarReader.FindByAddress(entity.WorkingPlace_Address, startDate, endDate,
                                                                entity.LunchTime.End));
            }

            return (noon, lunch, afternoon);
        }

        /// <summary>
        /// 入力 - 日付
        /// </summary>
        /// <param name="date">日付</param>
        private void InputFormattedDate(DateTime date)
        {
            switch (date.Day) 
            {
                case 1  : this.ViewModel_Table.Day_1  = Format(); return;
                case 2  : this.ViewModel_Table.Day_2  = Format(); return;
                case 3  : this.ViewModel_Table.Day_3  = Format(); return;
                case 4  : this.ViewModel_Table.Day_4  = Format(); return;
                case 5  : this.ViewModel_Table.Day_5  = Format(); return;
                case 6  : this.ViewModel_Table.Day_6  = Format(); return;
                case 7  : this.ViewModel_Table.Day_7  = Format(); return;
                case 8  : this.ViewModel_Table.Day_8  = Format(); return;
                case 9  : this.ViewModel_Table.Day_9  = Format(); return;
                case 10 : this.ViewModel_Table.Day_10 = Format(); return;
                case 11 : this.ViewModel_Table.Day_11 = Format(); return;
                case 12 : this.ViewModel_Table.Day_12 = Format(); return;
                case 13 : this.ViewModel_Table.Day_13 = Format(); return;
                case 14 : this.ViewModel_Table.Day_14 = Format(); return;
                case 15 : this.ViewModel_Table.Day_15 = Format(); return;
                case 16 : this.ViewModel_Table.Day_16 = Format(); return;
                case 17 : this.ViewModel_Table.Day_17 = Format(); return;
                case 18 : this.ViewModel_Table.Day_18 = Format(); return;
                case 19 : this.ViewModel_Table.Day_19 = Format(); return;
                case 20 : this.ViewModel_Table.Day_20 = Format(); return;
                case 21 : this.ViewModel_Table.Day_21 = Format(); return;
                case 22 : this.ViewModel_Table.Day_22 = Format(); return;
                case 23 : this.ViewModel_Table.Day_23 = Format(); return;
                case 24 : this.ViewModel_Table.Day_24 = Format(); return;
                case 25 : this.ViewModel_Table.Day_25 = Format(); return;
                case 26 : this.ViewModel_Table.Day_26 = Format(); return;
                case 27 : this.ViewModel_Table.Day_27 = Format(); return;
                case 28 : this.ViewModel_Table.Day_28 = Format(); return;
                case 29 : this.ViewModel_Table.Day_29 = Format(); return;
                case 30 : this.ViewModel_Table.Day_30 = Format(); return;
                case 31 : this.ViewModel_Table.Day_31 = Format(); return;

                default: return;
            }

            string Format()
                => new DateValue(date).Date_MMDDWithWeekName;
        }

        /// <summary>
        /// 入力 - 始業時間
        /// </summary>
        /// <param name="day">対象日</param>
        /// <param name="startTime">始業時刻</param>
        private void InputStartTime(int day, (int Hour, int Minute) startTime)
        {
            switch (day)
            {
                case 1 : this.ViewModel_Table.Day_1_StartTime  = Format(); return;
                case 2 : this.ViewModel_Table.Day_2_StartTime  = Format(); return;
                case 3 : this.ViewModel_Table.Day_3_StartTime  = Format(); return;
                case 4 : this.ViewModel_Table.Day_4_StartTime  = Format(); return;
                case 5 : this.ViewModel_Table.Day_5_StartTime  = Format(); return;
                case 6 : this.ViewModel_Table.Day_6_StartTime  = Format(); return;
                case 7 : this.ViewModel_Table.Day_7_StartTime  = Format(); return;
                case 8 : this.ViewModel_Table.Day_8_StartTime  = Format(); return;
                case 9 : this.ViewModel_Table.Day_9_StartTime  = Format(); return;
                case 10: this.ViewModel_Table.Day_10_StartTime = Format(); return;
                case 11: this.ViewModel_Table.Day_11_StartTime = Format(); return;
                case 12: this.ViewModel_Table.Day_12_StartTime = Format(); return;
                case 13: this.ViewModel_Table.Day_13_StartTime = Format(); return;
                case 14: this.ViewModel_Table.Day_14_StartTime = Format(); return;
                case 15: this.ViewModel_Table.Day_15_StartTime = Format(); return;
                case 16: this.ViewModel_Table.Day_16_StartTime = Format(); return;
                case 17: this.ViewModel_Table.Day_17_StartTime = Format(); return;
                case 18: this.ViewModel_Table.Day_18_StartTime = Format(); return;
                case 19: this.ViewModel_Table.Day_19_StartTime = Format(); return;
                case 20: this.ViewModel_Table.Day_20_StartTime = Format(); return;
                case 21: this.ViewModel_Table.Day_21_StartTime = Format(); return;
                case 22: this.ViewModel_Table.Day_22_StartTime = Format(); return;
                case 23: this.ViewModel_Table.Day_23_StartTime = Format(); return;
                case 24: this.ViewModel_Table.Day_24_StartTime = Format(); return;
                case 25: this.ViewModel_Table.Day_25_StartTime = Format(); return;
                case 26: this.ViewModel_Table.Day_26_StartTime = Format(); return;
                case 27: this.ViewModel_Table.Day_27_StartTime = Format(); return;
                case 28: this.ViewModel_Table.Day_28_StartTime = Format(); return;
                case 29: this.ViewModel_Table.Day_29_StartTime = Format(); return;
                case 30: this.ViewModel_Table.Day_30_StartTime = Format(); return;
                case 31: this.ViewModel_Table.Day_31_StartTime = Format(); return;
            }

            string Format()
                => $"{startTime.Hour.ToString("00")}:{startTime.Minute.ToString("00")}";
        }

        /// <summary>
        /// 入力 - 終業時間
        /// </summary>
        /// <param name="day">対象日</param>
        /// <param name="endTime">終業時刻</param>
        private void InputEndTime(int day, (int Hour, int Minute) endTime)
        {
            switch (day)
            {
                case 1:  this.ViewModel_Table.Day_1_EndTime  = Format(); return;
                case 2:  this.ViewModel_Table.Day_2_EndTime  = Format(); return;
                case 3:  this.ViewModel_Table.Day_3_EndTime  = Format(); return;
                case 4:  this.ViewModel_Table.Day_4_EndTime  = Format(); return;
                case 5:  this.ViewModel_Table.Day_5_EndTime  = Format(); return;
                case 6:  this.ViewModel_Table.Day_6_EndTime  = Format(); return;
                case 7:  this.ViewModel_Table.Day_7_EndTime  = Format(); return;
                case 8:  this.ViewModel_Table.Day_8_EndTime  = Format(); return;
                case 9:  this.ViewModel_Table.Day_9_EndTime  = Format(); return;
                case 10: this.ViewModel_Table.Day_10_EndTime = Format(); return;
                case 11: this.ViewModel_Table.Day_11_EndTime = Format(); return;
                case 12: this.ViewModel_Table.Day_12_EndTime = Format(); return;
                case 13: this.ViewModel_Table.Day_13_EndTime = Format(); return;
                case 14: this.ViewModel_Table.Day_14_EndTime = Format(); return;
                case 15: this.ViewModel_Table.Day_15_EndTime = Format(); return;
                case 16: this.ViewModel_Table.Day_16_EndTime = Format(); return;
                case 17: this.ViewModel_Table.Day_17_EndTime = Format(); return;
                case 18: this.ViewModel_Table.Day_18_EndTime = Format(); return;
                case 19: this.ViewModel_Table.Day_19_EndTime = Format(); return;
                case 20: this.ViewModel_Table.Day_20_EndTime = Format(); return;
                case 21: this.ViewModel_Table.Day_21_EndTime = Format(); return;
                case 22: this.ViewModel_Table.Day_22_EndTime = Format(); return;
                case 23: this.ViewModel_Table.Day_23_EndTime = Format(); return;
                case 24: this.ViewModel_Table.Day_24_EndTime = Format(); return;
                case 25: this.ViewModel_Table.Day_25_EndTime = Format(); return;
                case 26: this.ViewModel_Table.Day_26_EndTime = Format(); return;
                case 27: this.ViewModel_Table.Day_27_EndTime = Format(); return;
                case 28: this.ViewModel_Table.Day_28_EndTime = Format(); return;
                case 29: this.ViewModel_Table.Day_29_EndTime = Format(); return;
                case 30: this.ViewModel_Table.Day_30_EndTime = Format(); return;
                case 31: this.ViewModel_Table.Day_31_EndTime = Format(); return;
            }
            string Format()
                => $"{endTime.Hour.ToString("00")}:{endTime.Minute.ToString("00")}";
        }

        /// <summary>
        /// 入力 - 昼休憩
        /// </summary>
        /// <param name="day">対象日</param>
        /// <param name="endTime">終業時刻</param>
        private void InputLunchTime(int day, (int Hour, int Minute) lunchTime)
        {
            switch (day)
            {
                case 1:  this.ViewModel_Table.Day_1_LunchTime  = Format(); return;
                case 2:  this.ViewModel_Table.Day_2_LunchTime  = Format(); return;
                case 3:  this.ViewModel_Table.Day_3_LunchTime  = Format(); return;
                case 4:  this.ViewModel_Table.Day_4_LunchTime  = Format(); return;
                case 5:  this.ViewModel_Table.Day_5_LunchTime  = Format(); return;
                case 6:  this.ViewModel_Table.Day_6_LunchTime  = Format(); return;
                case 7:  this.ViewModel_Table.Day_7_LunchTime  = Format(); return;
                case 8:  this.ViewModel_Table.Day_8_LunchTime  = Format(); return;
                case 9:  this.ViewModel_Table.Day_9_LunchTime  = Format(); return;
                case 10: this.ViewModel_Table.Day_10_LunchTime = Format(); return;
                case 11: this.ViewModel_Table.Day_11_LunchTime = Format(); return;
                case 12: this.ViewModel_Table.Day_12_LunchTime = Format(); return;
                case 13: this.ViewModel_Table.Day_13_LunchTime = Format(); return;
                case 14: this.ViewModel_Table.Day_14_LunchTime = Format(); return;
                case 15: this.ViewModel_Table.Day_15_LunchTime = Format(); return;
                case 16: this.ViewModel_Table.Day_16_LunchTime = Format(); return;
                case 17: this.ViewModel_Table.Day_17_LunchTime = Format(); return;
                case 18: this.ViewModel_Table.Day_18_LunchTime = Format(); return;
                case 19: this.ViewModel_Table.Day_19_LunchTime = Format(); return;
                case 20: this.ViewModel_Table.Day_20_LunchTime = Format(); return;
                case 21: this.ViewModel_Table.Day_21_LunchTime = Format(); return;
                case 22: this.ViewModel_Table.Day_22_LunchTime = Format(); return;
                case 23: this.ViewModel_Table.Day_23_LunchTime = Format(); return;
                case 24: this.ViewModel_Table.Day_24_LunchTime = Format(); return;
                case 25: this.ViewModel_Table.Day_25_LunchTime = Format(); return;
                case 26: this.ViewModel_Table.Day_26_LunchTime = Format(); return;
                case 27: this.ViewModel_Table.Day_27_LunchTime = Format(); return;
                case 28: this.ViewModel_Table.Day_28_LunchTime = Format(); return;
                case 29: this.ViewModel_Table.Day_29_LunchTime = Format(); return;
                case 30: this.ViewModel_Table.Day_30_LunchTime = Format(); return;
                case 31: this.ViewModel_Table.Day_31_LunchTime = Format(); return;
            }

            string Format()
                => $"{lunchTime.Hour.ToString("00")}:{lunchTime.Minute.ToString("00")}";
        }

        /// <summary>
        /// 入力 - 勤務時間
        /// </summary>
        /// <param name="entities">エンティティ</param>
        private void InputWorkingTime(List<CalendarEventEntity> entities)
        {
            switch (entities.First().StartDate.Day)
            {
                case 1:  this.ViewModel_Table.Day_1_WorkingTime  = Format(); return;
                case 2:  this.ViewModel_Table.Day_2_WorkingTime  = Format(); return;
                case 3:  this.ViewModel_Table.Day_3_WorkingTime  = Format(); return;
                case 4:  this.ViewModel_Table.Day_4_WorkingTime  = Format(); return;
                case 5:  this.ViewModel_Table.Day_5_WorkingTime  = Format(); return;
                case 6:  this.ViewModel_Table.Day_6_WorkingTime  = Format(); return;
                case 7:  this.ViewModel_Table.Day_7_WorkingTime  = Format(); return;
                case 8:  this.ViewModel_Table.Day_8_WorkingTime  = Format(); return;
                case 9:  this.ViewModel_Table.Day_9_WorkingTime  = Format(); return;
                case 10: this.ViewModel_Table.Day_10_WorkingTime = Format(); return;
                case 11: this.ViewModel_Table.Day_11_WorkingTime = Format(); return;
                case 12: this.ViewModel_Table.Day_12_WorkingTime = Format(); return;
                case 13: this.ViewModel_Table.Day_13_WorkingTime = Format(); return;
                case 14: this.ViewModel_Table.Day_14_WorkingTime = Format(); return;
                case 15: this.ViewModel_Table.Day_15_WorkingTime = Format(); return;
                case 16: this.ViewModel_Table.Day_16_WorkingTime = Format(); return;
                case 17: this.ViewModel_Table.Day_17_WorkingTime = Format(); return;
                case 18: this.ViewModel_Table.Day_18_WorkingTime = Format(); return;
                case 19: this.ViewModel_Table.Day_19_WorkingTime = Format(); return;
                case 20: this.ViewModel_Table.Day_20_WorkingTime = Format(); return;
                case 21: this.ViewModel_Table.Day_21_WorkingTime = Format(); return;
                case 22: this.ViewModel_Table.Day_22_WorkingTime = Format(); return;
                case 23: this.ViewModel_Table.Day_23_WorkingTime = Format(); return;
                case 24: this.ViewModel_Table.Day_24_WorkingTime = Format(); return;
                case 25: this.ViewModel_Table.Day_25_WorkingTime = Format(); return;
                case 26: this.ViewModel_Table.Day_26_WorkingTime = Format(); return;
                case 27: this.ViewModel_Table.Day_27_WorkingTime = Format(); return;
                case 28: this.ViewModel_Table.Day_28_WorkingTime = Format(); return;
                case 29: this.ViewModel_Table.Day_29_WorkingTime = Format(); return;
                case 30: this.ViewModel_Table.Day_30_WorkingTime = Format(); return;
                case 31: this.ViewModel_Table.Day_31_WorkingTime = Format(); return;
            }

            string Format()
            {
                (int Hour, int Minute) start = GetStartTime(entities);
                (int Hour, int Minute) end   = GetEndTime(entities);
                (int Hour, int Minute) lunch = GetLunchTime(entities);

                return $"{(end.Hour   - start.Hour   - lunch.Hour).ToString("00")}:" +
                       $"{(end.Minute - start.Minute - lunch.Minute).ToString("00")}";
            }
        }

        /// <summary>
        /// 入力 - 残業時間
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <param name="day">日付</param>
        private void InputOvertime(List<CalendarEventEntity> entities, int day)
        {
            var startDate = entities.First().StartDate;
            var workingPlace = this.SearchWorkingPlace(entities, new DateTime(startDate.Year, startDate.Month, day));

            switch (startDate.Day)
            {
                case 1:  this.ViewModel_Table.Day_1_Overtime  = Format(); return;
                case 2:  this.ViewModel_Table.Day_2_Overtime  = Format(); return;
                case 3:  this.ViewModel_Table.Day_3_Overtime  = Format(); return;
                case 4:  this.ViewModel_Table.Day_4_Overtime  = Format(); return;
                case 5:  this.ViewModel_Table.Day_5_Overtime  = Format(); return;
                case 6:  this.ViewModel_Table.Day_6_Overtime  = Format(); return;
                case 7:  this.ViewModel_Table.Day_7_Overtime  = Format(); return;
                case 8:  this.ViewModel_Table.Day_8_Overtime  = Format(); return;
                case 9:  this.ViewModel_Table.Day_9_Overtime  = Format(); return;
                case 10: this.ViewModel_Table.Day_10_Overtime = Format(); return;
                case 11: this.ViewModel_Table.Day_11_Overtime = Format(); return;
                case 12: this.ViewModel_Table.Day_12_Overtime = Format(); return;
                case 13: this.ViewModel_Table.Day_13_Overtime = Format(); return;
                case 14: this.ViewModel_Table.Day_14_Overtime = Format(); return;
                case 15: this.ViewModel_Table.Day_15_Overtime = Format(); return;
                case 16: this.ViewModel_Table.Day_16_Overtime = Format(); return;
                case 17: this.ViewModel_Table.Day_17_Overtime = Format(); return;
                case 18: this.ViewModel_Table.Day_18_Overtime = Format(); return;
                case 19: this.ViewModel_Table.Day_19_Overtime = Format(); return;
                case 20: this.ViewModel_Table.Day_20_Overtime = Format(); return;
                case 21: this.ViewModel_Table.Day_21_Overtime = Format(); return;
                case 22: this.ViewModel_Table.Day_22_Overtime = Format(); return;
                case 23: this.ViewModel_Table.Day_23_Overtime = Format(); return;
                case 24: this.ViewModel_Table.Day_24_Overtime = Format(); return;
                case 25: this.ViewModel_Table.Day_25_Overtime = Format(); return;
                case 26: this.ViewModel_Table.Day_26_Overtime = Format(); return;
                case 27: this.ViewModel_Table.Day_27_Overtime = Format(); return;
                case 28: this.ViewModel_Table.Day_28_Overtime = Format(); return;
                case 29: this.ViewModel_Table.Day_29_Overtime = Format(); return;
                case 30: this.ViewModel_Table.Day_30_Overtime = Format(); return;
                case 31: this.ViewModel_Table.Day_31_Overtime = Format(); return;
            }

            string Format()
            {
                (int Hour, int Minute) start = GetStartTime(entities);
                (int Hour, int Minute) end   = GetEndTime(entities);

                return $"{(end.Hour   - start.Hour   - workingPlace.WorkingTime.Start.Hour).ToString("00")}:" +
                       $"{(end.Minute - start.Minute - workingPlace.WorkingTime.End.Minute).ToString("00")}";
            }
        }

        /// <summary>
        /// Input - 備考
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <param name="day">日</param>
        private void InputRemarks(List<CalendarEventEntity> entities, int day)
        {
            var startDate = entities.First().StartDate;
            //var workingPlace = this.SearchWorkingPlace(entities, new DateTime(startDate.Year, startDate.Month, day));
            var home = Homes.FetchByDate(new DateTime(startDate.Year, startDate.Month, day));

            switch (startDate.Day)
            {
                case 1:  this.ViewModel_Table.Day_1_Remarks  = Format(); return;
                case 2:  this.ViewModel_Table.Day_2_Remarks  = Format(); return;
                case 3:  this.ViewModel_Table.Day_3_Remarks  = Format(); return;
                case 4:  this.ViewModel_Table.Day_4_Remarks  = Format(); return;
                case 5:  this.ViewModel_Table.Day_5_Remarks  = Format(); return;
                case 6:  this.ViewModel_Table.Day_6_Remarks  = Format(); return;
                case 7:  this.ViewModel_Table.Day_7_Remarks  = Format(); return;
                case 8:  this.ViewModel_Table.Day_8_Remarks  = Format(); return;
                case 9:  this.ViewModel_Table.Day_9_Remarks  = Format(); return;
                case 10: this.ViewModel_Table.Day_10_Remarks = Format(); return;
                case 11: this.ViewModel_Table.Day_11_Remarks = Format(); return;
                case 12: this.ViewModel_Table.Day_12_Remarks = Format(); return;
                case 13: this.ViewModel_Table.Day_13_Remarks = Format(); return;
                case 14: this.ViewModel_Table.Day_14_Remarks = Format(); return;
                case 15: this.ViewModel_Table.Day_15_Remarks = Format(); return;
                case 16: this.ViewModel_Table.Day_16_Remarks = Format(); return;
                case 17: this.ViewModel_Table.Day_17_Remarks = Format(); return;
                case 18: this.ViewModel_Table.Day_18_Remarks = Format(); return;
                case 19: this.ViewModel_Table.Day_19_Remarks = Format(); return;
                case 20: this.ViewModel_Table.Day_20_Remarks = Format(); return;
                case 21: this.ViewModel_Table.Day_21_Remarks = Format(); return;
                case 22: this.ViewModel_Table.Day_22_Remarks = Format(); return;
                case 23: this.ViewModel_Table.Day_23_Remarks = Format(); return;
                case 24: this.ViewModel_Table.Day_24_Remarks = Format(); return;
                case 25: this.ViewModel_Table.Day_25_Remarks = Format(); return;
                case 26: this.ViewModel_Table.Day_26_Remarks = Format(); return;
                case 27: this.ViewModel_Table.Day_27_Remarks = Format(); return;
                case 28: this.ViewModel_Table.Day_28_Remarks = Format(); return;
                case 29: this.ViewModel_Table.Day_29_Remarks = Format(); return;
                case 30: this.ViewModel_Table.Day_30_Remarks = Format(); return;
                case 31: this.ViewModel_Table.Day_31_Remarks = Format(); return;
            }

            string Format()
            {
                (int Hour, int Minute) start = GetStartTime(entities);
                (int Hour, int Minute) end   = GetEndTime(entities);

                var isWorkAtHome = (end.Hour - start.Hour >= 8 && entities.First().Place == home.Address_Google);

                return isWorkAtHome ? "在宅所定時間以上" : string.Empty;
            }
        }

        /// <summary>
        /// 就業先を検索する
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private WorkingPlaceEntity SearchWorkingPlace(List<CalendarEventEntity> entities, DateTime date)
        {
            var workingPlace = WorkingPlace.FetchByDate(date);
            //var home = Homes.FetchByDate(date);

            if (workingPlace.Count == 1 &&
                workingPlace.ToList().Any(x => x.IsWaiting))
            {
                // 待機
                return workingPlace.FirstOrDefault();
            }

            // TODO: 現場対応の場合はどうなる？
            // 常駐先
            return workingPlace.Where(x => x.IsWaiting == false).FirstOrDefault();
        }

        /// <summary>
        /// 始業時間の取得
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>始業時間</returns>
        private (int Hours, int Minutes) GetStartTime(List<CalendarEventEntity> entities)
            => (entities.Min(x => x.StartDate.Hour), entities.Min(x => x.StartDate.Minute));

        /// <summary>
        /// 終業時間の取得
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>終業時間</returns>
        private (int Hours, int Minutes) GetEndTime(List<CalendarEventEntity> entities)
            => (entities.Max(x => x.EndDate.Hour), entities.Max(x => x.EndDate.Minute));

        /// <summary>
        /// 昼休憩時間の取得
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>昼休憩時間</returns>
        private (int Hours, int Minutes) GetLunchTime(List<CalendarEventEntity> entities)
            => (entities.Where(x => x.Title.Contains("昼食"))
                        .Select(x => (x.EndDate.Hour - x.StartDate.Hour,
                                                       x.EndDate.Minute - x.StartDate.Minute)).FirstOrDefault());

        

        /// <summary> ViewModel - 勤務表 </summary>
        internal ViewModel_WorkSchedule_Table ViewModel_Table { get; set; }

        /// <summary> ViewModel - 勤務表 </summary>
        internal ViewModel_WorkSchedule_Header ViewModel_Header { get; set; }
    }
}
