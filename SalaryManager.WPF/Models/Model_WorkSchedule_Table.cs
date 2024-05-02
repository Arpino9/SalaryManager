using System;
using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Exceptions;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Google_Calendar;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Infrastructure.JSON;
using WorkingPlace = SalaryManager.Domain.StaticValues.WorkingPlace;
using DocumentFormat.OpenXml.Bibliography;

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

            // 該当年月
            this.ViewModel_Header.Year  = this.ViewModel_Header.TargetDate.Year;
            this.ViewModel_Header.Month = this.ViewModel_Header.TargetDate.Month;

            var (Noon, Lunch, Afternoon) = GetScheduleEvents(this.FirstDateOfMonth, this.LastDateOfMonth);

            if (Noon.Any()      == false ||
                Lunch.Any()     == false ||
                Afternoon.Any() == false) 
            {
                throw new DatabaseException("スケジュールの取得に失敗しました。");
            }

            this.Clear();

            var date = this.FirstDateOfMonth;

            for (var i = 1; i <= this.LastDayOfMonth; i++)
            {
                this.InputFormattedDate(date);
                this.GetHoliday(date);

                date = date.AddDays(1);
            }

            this.GetCompany();

            for (var day = 1; day <= this.LastDayOfMonth; day++)
            {
                var entities = Noon.Union(Afternoon).Union(Lunch)
                                   .Where(x => x.StartDate.Day == day).ToList();

                // 届出
                this.InputNotification(day);

                if (entities.Count < 2)
                {
                    // 休祝日
                    continue;
                }

                DateTime startTime = entities.Min(x => x.StartDate);
                DateTime endTime   = entities.Max(x => x.EndDate);
                TimeSpan lunchTime = (entities.Where(x => x.Title.Contains("昼食")).FirstOrDefault().EndDate -
                                      entities.Where(x => x.Title.Contains("昼食")).FirstOrDefault().StartDate);

                // 始業
                this.InputStartTime(day, startTime);

                // 昼休憩
                this.InputLunchTime(day);

                // 終業
                this.InputEndTime(day, endTime);

                // 勤務時間
                this.InputWorkingTime(day, startTime, lunchTime, endTime);

                // 残業時間
                this.InputOvertime(day);

                // 備考
                this.InputRemarks(day, startTime, endTime, entities.First().Place);
            }

            // 合計 - 勤務時間
            this.ViewModel_Header.WorkingTimeTotal_Text = Math.Truncate(this.ViewModel_Header.WorkingTimeTotal.TotalHours) + ":" + 
                                                          this.ViewModel_Header.WorkingTimeTotal.Minutes.ToString("00");

            // 合計 - 残業時間
            this.ViewModel_Header.OvertimeTotal_Text = Math.Truncate(this.ViewModel_Header.OvertimeTotal.TotalHours) + ":" +
                                                       this.ViewModel_Header.OvertimeTotal.Minutes.ToString("00");
        }


        private void GetHoliday(DateTime date)
        {
            var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

            if (holidays.Any() == false)
            {
                return;
            }

            switch (date.Day)
            {
                case 1 : this.ViewModel_Table.Background_1  = Format(); return;
                case 2 : this.ViewModel_Table.Background_2  = Format(); return;
                case 3 : this.ViewModel_Table.Background_3  = Format(); return;
                case 4 : this.ViewModel_Table.Background_4  = Format(); return;
                case 5 : this.ViewModel_Table.Background_5  = Format(); return;
                case 6 : this.ViewModel_Table.Background_6  = Format(); return;
                case 7 : this.ViewModel_Table.Background_7  = Format(); return;
                case 8 : this.ViewModel_Table.Background_8  = Format(); return;
                case 9 : this.ViewModel_Table.Background_9  = Format(); return;
                case 10: this.ViewModel_Table.Background_10 = Format(); return;
                case 11: this.ViewModel_Table.Background_11 = Format(); return;
                case 12: this.ViewModel_Table.Background_12 = Format(); return;
                case 13: this.ViewModel_Table.Background_13 = Format(); return;
                case 14: this.ViewModel_Table.Background_14 = Format(); return;
                case 15: this.ViewModel_Table.Background_15 = Format(); return;
                case 16: this.ViewModel_Table.Background_16 = Format(); return;
                case 17: this.ViewModel_Table.Background_17 = Format(); return;
                case 18: this.ViewModel_Table.Background_18 = Format(); return;
                case 19: this.ViewModel_Table.Background_19 = Format(); return;
                case 20: this.ViewModel_Table.Background_20 = Format(); return;
                case 21: this.ViewModel_Table.Background_21 = Format(); return;
                case 22: this.ViewModel_Table.Background_22 = Format(); return;
                case 23: this.ViewModel_Table.Background_23 = Format(); return;
                case 24: this.ViewModel_Table.Background_24 = Format(); return;
                case 25: this.ViewModel_Table.Background_25 = Format(); return;
                case 26: this.ViewModel_Table.Background_26 = Format(); return;
                case 27: this.ViewModel_Table.Background_27 = Format(); return;
                case 28: this.ViewModel_Table.Background_28 = Format(); return;
                case 29: this.ViewModel_Table.Background_29 = Format(); return;
                case 30: this.ViewModel_Table.Background_30 = Format(); return;
                case 31: this.ViewModel_Table.Background_31 = Format(); return;

                default: return;
            }

            Brush Format()
            {
                var dateValue = new DateValue(date);

                if (dateValue.IsSaturday)
                {
                    return new SolidColorBrush(Color.FromRgb(201, 218, 248));
                }

                if (dateValue.IsSunday)
                {
                    return new SolidColorBrush(Color.FromRgb(252, 229, 205));
                }

                if (this.IsHoliday(date))
                {
                    return new SolidColorBrush(Color.FromRgb(252, 229, 205));
                }

                if (this.IsPaidVacation(date))
                {
                    return new SolidColorBrush(Color.FromRgb(252, 229, 205));
                }

                return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        /// <summary>
        /// 指定した日が祝日か
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>祝日か</returns>
        private bool IsHoliday(DateTime date)
        {
            var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

            return holidays.Where(x => x.Date == date).Any();
        }

        private void Clear()
        {
            // 日付
            this.ViewModel_Table.Day_1  = string.Empty;
            this.ViewModel_Table.Day_2  = string.Empty;
            this.ViewModel_Table.Day_3  = string.Empty;
            this.ViewModel_Table.Day_4  = string.Empty;
            this.ViewModel_Table.Day_5  = string.Empty;
            this.ViewModel_Table.Day_6  = string.Empty;
            this.ViewModel_Table.Day_7  = string.Empty;
            this.ViewModel_Table.Day_8  = string.Empty;
            this.ViewModel_Table.Day_9  = string.Empty;
            this.ViewModel_Table.Day_10 = string.Empty;
            this.ViewModel_Table.Day_11 = string.Empty;
            this.ViewModel_Table.Day_12 = string.Empty;
            this.ViewModel_Table.Day_13 = string.Empty;
            this.ViewModel_Table.Day_14 = string.Empty;
            this.ViewModel_Table.Day_15 = string.Empty;
            this.ViewModel_Table.Day_16 = string.Empty;
            this.ViewModel_Table.Day_17 = string.Empty;
            this.ViewModel_Table.Day_18 = string.Empty;
            this.ViewModel_Table.Day_19 = string.Empty;
            this.ViewModel_Table.Day_20 = string.Empty;
            this.ViewModel_Table.Day_21 = string.Empty;
            this.ViewModel_Table.Day_22 = string.Empty;
            this.ViewModel_Table.Day_23 = string.Empty;
            this.ViewModel_Table.Day_24 = string.Empty;
            this.ViewModel_Table.Day_25 = string.Empty;
            this.ViewModel_Table.Day_26 = string.Empty;
            this.ViewModel_Table.Day_27 = string.Empty;
            this.ViewModel_Table.Day_28 = string.Empty;
            this.ViewModel_Table.Day_29 = string.Empty;
            this.ViewModel_Table.Day_30 = string.Empty;
            this.ViewModel_Table.Day_31 = string.Empty;

            // 始業時間
            this.ViewModel_Table.Day_1_StartTime  = string.Empty;
            this.ViewModel_Table.Day_2_StartTime  = string.Empty;
            this.ViewModel_Table.Day_3_StartTime  = string.Empty;
            this.ViewModel_Table.Day_4_StartTime  = string.Empty;
            this.ViewModel_Table.Day_5_StartTime  = string.Empty;
            this.ViewModel_Table.Day_6_StartTime  = string.Empty;
            this.ViewModel_Table.Day_7_StartTime  = string.Empty;
            this.ViewModel_Table.Day_8_StartTime  = string.Empty;
            this.ViewModel_Table.Day_9_StartTime  = string.Empty;
            this.ViewModel_Table.Day_10_StartTime = string.Empty;
            this.ViewModel_Table.Day_11_StartTime = string.Empty;
            this.ViewModel_Table.Day_12_StartTime = string.Empty;
            this.ViewModel_Table.Day_13_StartTime = string.Empty;
            this.ViewModel_Table.Day_14_StartTime = string.Empty;
            this.ViewModel_Table.Day_15_StartTime = string.Empty;
            this.ViewModel_Table.Day_16_StartTime = string.Empty;
            this.ViewModel_Table.Day_17_StartTime = string.Empty;
            this.ViewModel_Table.Day_18_StartTime = string.Empty;
            this.ViewModel_Table.Day_19_StartTime = string.Empty;
            this.ViewModel_Table.Day_20_StartTime = string.Empty;
            this.ViewModel_Table.Day_21_StartTime = string.Empty;
            this.ViewModel_Table.Day_22_StartTime = string.Empty;
            this.ViewModel_Table.Day_23_StartTime = string.Empty;
            this.ViewModel_Table.Day_24_StartTime = string.Empty;
            this.ViewModel_Table.Day_25_StartTime = string.Empty;
            this.ViewModel_Table.Day_26_StartTime = string.Empty;
            this.ViewModel_Table.Day_27_StartTime = string.Empty;
            this.ViewModel_Table.Day_28_StartTime = string.Empty;
            this.ViewModel_Table.Day_29_StartTime = string.Empty;
            this.ViewModel_Table.Day_30_StartTime = string.Empty;
            this.ViewModel_Table.Day_31_StartTime = string.Empty;

            // 終業時間
            this.ViewModel_Table.Day_1_EndTime  = string.Empty;
            this.ViewModel_Table.Day_2_EndTime  = string.Empty;
            this.ViewModel_Table.Day_3_EndTime  = string.Empty;
            this.ViewModel_Table.Day_4_EndTime  = string.Empty;
            this.ViewModel_Table.Day_5_EndTime  = string.Empty;
            this.ViewModel_Table.Day_6_EndTime  = string.Empty;
            this.ViewModel_Table.Day_7_EndTime  = string.Empty;
            this.ViewModel_Table.Day_8_EndTime  = string.Empty;
            this.ViewModel_Table.Day_9_EndTime  = string.Empty;
            this.ViewModel_Table.Day_10_EndTime = string.Empty;
            this.ViewModel_Table.Day_11_EndTime = string.Empty;
            this.ViewModel_Table.Day_12_EndTime = string.Empty;
            this.ViewModel_Table.Day_13_EndTime = string.Empty;
            this.ViewModel_Table.Day_14_EndTime = string.Empty;
            this.ViewModel_Table.Day_15_EndTime = string.Empty;
            this.ViewModel_Table.Day_16_EndTime = string.Empty;
            this.ViewModel_Table.Day_17_EndTime = string.Empty;
            this.ViewModel_Table.Day_18_EndTime = string.Empty;
            this.ViewModel_Table.Day_19_EndTime = string.Empty;
            this.ViewModel_Table.Day_20_EndTime = string.Empty;
            this.ViewModel_Table.Day_21_EndTime = string.Empty;
            this.ViewModel_Table.Day_22_EndTime = string.Empty;
            this.ViewModel_Table.Day_23_EndTime = string.Empty;
            this.ViewModel_Table.Day_24_EndTime = string.Empty;
            this.ViewModel_Table.Day_25_EndTime = string.Empty;
            this.ViewModel_Table.Day_26_EndTime = string.Empty;
            this.ViewModel_Table.Day_27_EndTime = string.Empty;
            this.ViewModel_Table.Day_28_EndTime = string.Empty;
            this.ViewModel_Table.Day_29_EndTime = string.Empty;
            this.ViewModel_Table.Day_30_EndTime = string.Empty;
            this.ViewModel_Table.Day_31_EndTime = string.Empty;

            // 昼食時間
            this.ViewModel_Table.Day_1_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_2_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_3_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_4_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_5_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_6_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_7_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_8_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_9_LunchTime  = string.Empty;
            this.ViewModel_Table.Day_10_LunchTime = string.Empty;
            this.ViewModel_Table.Day_11_LunchTime = string.Empty;
            this.ViewModel_Table.Day_12_LunchTime = string.Empty;
            this.ViewModel_Table.Day_13_LunchTime = string.Empty;
            this.ViewModel_Table.Day_14_LunchTime = string.Empty;
            this.ViewModel_Table.Day_15_LunchTime = string.Empty;
            this.ViewModel_Table.Day_16_LunchTime = string.Empty;
            this.ViewModel_Table.Day_17_LunchTime = string.Empty;
            this.ViewModel_Table.Day_18_LunchTime = string.Empty;
            this.ViewModel_Table.Day_19_LunchTime = string.Empty;
            this.ViewModel_Table.Day_20_LunchTime = string.Empty;
            this.ViewModel_Table.Day_21_LunchTime = string.Empty;
            this.ViewModel_Table.Day_22_LunchTime = string.Empty;
            this.ViewModel_Table.Day_23_LunchTime = string.Empty;
            this.ViewModel_Table.Day_24_LunchTime = string.Empty;
            this.ViewModel_Table.Day_25_LunchTime = string.Empty;
            this.ViewModel_Table.Day_26_LunchTime = string.Empty;
            this.ViewModel_Table.Day_27_LunchTime = string.Empty;
            this.ViewModel_Table.Day_28_LunchTime = string.Empty;
            this.ViewModel_Table.Day_29_LunchTime = string.Empty;
            this.ViewModel_Table.Day_30_LunchTime = string.Empty;
            this.ViewModel_Table.Day_31_LunchTime = string.Empty;

            // 届出
            this.ViewModel_Table.Day_1_Notification  = string.Empty;
            this.ViewModel_Table.Day_2_Notification  = string.Empty;
            this.ViewModel_Table.Day_3_Notification  = string.Empty;
            this.ViewModel_Table.Day_4_Notification  = string.Empty;
            this.ViewModel_Table.Day_5_Notification  = string.Empty;
            this.ViewModel_Table.Day_6_Notification  = string.Empty;
            this.ViewModel_Table.Day_7_Notification  = string.Empty;
            this.ViewModel_Table.Day_8_Notification  = string.Empty;
            this.ViewModel_Table.Day_9_Notification  = string.Empty;
            this.ViewModel_Table.Day_10_Notification = string.Empty;
            this.ViewModel_Table.Day_11_Notification = string.Empty;
            this.ViewModel_Table.Day_12_Notification = string.Empty;
            this.ViewModel_Table.Day_13_Notification = string.Empty;
            this.ViewModel_Table.Day_14_Notification = string.Empty;
            this.ViewModel_Table.Day_15_Notification = string.Empty;
            this.ViewModel_Table.Day_16_Notification = string.Empty;
            this.ViewModel_Table.Day_17_Notification = string.Empty;
            this.ViewModel_Table.Day_18_Notification = string.Empty;
            this.ViewModel_Table.Day_19_Notification = string.Empty;
            this.ViewModel_Table.Day_20_Notification = string.Empty;
            this.ViewModel_Table.Day_21_Notification = string.Empty;
            this.ViewModel_Table.Day_22_Notification = string.Empty;
            this.ViewModel_Table.Day_23_Notification = string.Empty;
            this.ViewModel_Table.Day_24_Notification = string.Empty;
            this.ViewModel_Table.Day_25_Notification = string.Empty;
            this.ViewModel_Table.Day_26_Notification = string.Empty;
            this.ViewModel_Table.Day_27_Notification = string.Empty;
            this.ViewModel_Table.Day_28_Notification = string.Empty;
            this.ViewModel_Table.Day_29_Notification = string.Empty;
            this.ViewModel_Table.Day_30_Notification = string.Empty;
            this.ViewModel_Table.Day_31_Notification = string.Empty;

            // 勤務時間
            this.ViewModel_Header.WorkingTimeTotal = new TimeSpan();

            this.ViewModel_Table.Day_1_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_2_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_3_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_4_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_5_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_6_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_7_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_8_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_9_WorkingTime  = string.Empty;
            this.ViewModel_Table.Day_10_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_11_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_12_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_13_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_14_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_15_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_16_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_17_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_18_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_19_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_20_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_21_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_22_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_23_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_24_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_25_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_26_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_27_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_28_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_29_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_30_WorkingTime = string.Empty;
            this.ViewModel_Table.Day_31_WorkingTime = string.Empty;

            // 残業時間
            this.ViewModel_Header.OvertimeTotal = new TimeSpan();

            this.ViewModel_Table.Day_1_Overtime  = string.Empty;
            this.ViewModel_Table.Day_2_Overtime  = string.Empty;
            this.ViewModel_Table.Day_3_Overtime  = string.Empty;
            this.ViewModel_Table.Day_4_Overtime  = string.Empty;
            this.ViewModel_Table.Day_5_Overtime  = string.Empty;
            this.ViewModel_Table.Day_6_Overtime  = string.Empty;
            this.ViewModel_Table.Day_7_Overtime  = string.Empty;
            this.ViewModel_Table.Day_8_Overtime  = string.Empty;
            this.ViewModel_Table.Day_9_Overtime  = string.Empty;
            this.ViewModel_Table.Day_10_Overtime = string.Empty;
            this.ViewModel_Table.Day_11_Overtime = string.Empty;
            this.ViewModel_Table.Day_12_Overtime = string.Empty;
            this.ViewModel_Table.Day_13_Overtime = string.Empty;
            this.ViewModel_Table.Day_14_Overtime = string.Empty;
            this.ViewModel_Table.Day_15_Overtime = string.Empty;
            this.ViewModel_Table.Day_16_Overtime = string.Empty;
            this.ViewModel_Table.Day_17_Overtime = string.Empty;
            this.ViewModel_Table.Day_18_Overtime = string.Empty;
            this.ViewModel_Table.Day_19_Overtime = string.Empty;
            this.ViewModel_Table.Day_20_Overtime = string.Empty;
            this.ViewModel_Table.Day_21_Overtime = string.Empty;
            this.ViewModel_Table.Day_22_Overtime = string.Empty;
            this.ViewModel_Table.Day_23_Overtime = string.Empty;
            this.ViewModel_Table.Day_24_Overtime = string.Empty;
            this.ViewModel_Table.Day_25_Overtime = string.Empty;
            this.ViewModel_Table.Day_26_Overtime = string.Empty;
            this.ViewModel_Table.Day_27_Overtime = string.Empty;
            this.ViewModel_Table.Day_28_Overtime = string.Empty;
            this.ViewModel_Table.Day_29_Overtime = string.Empty;
            this.ViewModel_Table.Day_30_Overtime = string.Empty;
            this.ViewModel_Table.Day_31_Overtime = string.Empty;

            // 備考
            this.ViewModel_Table.Day_1_Remarks  = string.Empty;
            this.ViewModel_Table.Day_2_Remarks  = string.Empty;
            this.ViewModel_Table.Day_3_Remarks  = string.Empty;
            this.ViewModel_Table.Day_4_Remarks  = string.Empty;
            this.ViewModel_Table.Day_5_Remarks  = string.Empty;
            this.ViewModel_Table.Day_6_Remarks  = string.Empty;
            this.ViewModel_Table.Day_7_Remarks  = string.Empty;
            this.ViewModel_Table.Day_8_Remarks  = string.Empty;
            this.ViewModel_Table.Day_9_Remarks  = string.Empty;
            this.ViewModel_Table.Day_10_Remarks = string.Empty;
            this.ViewModel_Table.Day_11_Remarks = string.Empty;
            this.ViewModel_Table.Day_12_Remarks = string.Empty;
            this.ViewModel_Table.Day_13_Remarks = string.Empty;
            this.ViewModel_Table.Day_14_Remarks = string.Empty;
            this.ViewModel_Table.Day_15_Remarks = string.Empty;
            this.ViewModel_Table.Day_16_Remarks = string.Empty;
            this.ViewModel_Table.Day_17_Remarks = string.Empty;
            this.ViewModel_Table.Day_18_Remarks = string.Empty;
            this.ViewModel_Table.Day_19_Remarks = string.Empty;
            this.ViewModel_Table.Day_20_Remarks = string.Empty;
            this.ViewModel_Table.Day_21_Remarks = string.Empty;
            this.ViewModel_Table.Day_22_Remarks = string.Empty;
            this.ViewModel_Table.Day_23_Remarks = string.Empty;
            this.ViewModel_Table.Day_24_Remarks = string.Empty;
            this.ViewModel_Table.Day_25_Remarks = string.Empty;
            this.ViewModel_Table.Day_26_Remarks = string.Empty;
            this.ViewModel_Table.Day_27_Remarks = string.Empty;
            this.ViewModel_Table.Day_28_Remarks = string.Empty;
            this.ViewModel_Table.Day_29_Remarks = string.Empty;
            this.ViewModel_Table.Day_30_Remarks = string.Empty;
            this.ViewModel_Table.Day_31_Remarks = string.Empty;
        }

        /// <summary>
        /// 就業場所を取得する
        /// </summary>
        /// <returns>就業場所</returns>
        private void GetCompany()
        {
            var workingPlace = WorkingPlace.FetchByDate(this.FirstDateOfMonth);

            this.ViewModel_Header.DispatchingCompany = workingPlace.Select(x => x.DispatchingCompany).Distinct().FirstOrDefault().Text;
            this.ViewModel_Header.DispatchedCompany  = workingPlace.Select(x => x.DispatchedCompany).Distinct().FirstOrDefault().Text;
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

            var workingPlace = WorkingPlace.FetchByDate(this.FirstDateOfMonth);

            return workingPlace.ToList();
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
        private void InputStartTime(int day, DateTime startTime)
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
        private void InputEndTime(int day, DateTime endTime)
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
        private void InputLunchTime(int day)
        {
            var workingPlace = this.SearchWorkingPlace(this.ConvertDayToDate(day));

            if (workingPlace is null)
            {
                return;
            }

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
                => $"{(workingPlace.LunchTime.End - workingPlace.LunchTime.Start).ToString(@"hh\:mm")}";
        }

        /// <summary>
        /// 入力 - 届出
        /// </summary>
        /// <param name="day">日</param>
        private void InputNotification(int day)
        {
            switch (day)
            {
                case 1:  this.ViewModel_Table.Day_1_Notification  = Format(); return;
                case 2:  this.ViewModel_Table.Day_2_Notification  = Format(); return;
                case 3:  this.ViewModel_Table.Day_3_Notification  = Format(); return;
                case 4:  this.ViewModel_Table.Day_4_Notification  = Format(); return;
                case 5:  this.ViewModel_Table.Day_5_Notification  = Format(); return;
                case 6:  this.ViewModel_Table.Day_6_Notification  = Format(); return;
                case 7:  this.ViewModel_Table.Day_7_Notification  = Format(); return;
                case 8:  this.ViewModel_Table.Day_8_Notification  = Format(); return;
                case 9:  this.ViewModel_Table.Day_9_Notification  = Format(); return;
                case 10: this.ViewModel_Table.Day_10_Notification = Format(); return;
                case 11: this.ViewModel_Table.Day_11_Notification = Format(); return;
                case 12: this.ViewModel_Table.Day_12_Notification = Format(); return;
                case 13: this.ViewModel_Table.Day_13_Notification = Format(); return;
                case 14: this.ViewModel_Table.Day_14_Notification = Format(); return;
                case 15: this.ViewModel_Table.Day_15_Notification = Format(); return;
                case 16: this.ViewModel_Table.Day_16_Notification = Format(); return;
                case 17: this.ViewModel_Table.Day_17_Notification = Format(); return;
                case 18: this.ViewModel_Table.Day_18_Notification = Format(); return;
                case 19: this.ViewModel_Table.Day_19_Notification = Format(); return;
                case 20: this.ViewModel_Table.Day_20_Notification = Format(); return;
                case 21: this.ViewModel_Table.Day_21_Notification = Format(); return;
                case 22: this.ViewModel_Table.Day_22_Notification = Format(); return;
                case 23: this.ViewModel_Table.Day_23_Notification = Format(); return;
                case 24: this.ViewModel_Table.Day_24_Notification = Format(); return;
                case 25: this.ViewModel_Table.Day_25_Notification = Format(); return;
                case 26: this.ViewModel_Table.Day_26_Notification = Format(); return;
                case 27: this.ViewModel_Table.Day_27_Notification = Format(); return;
                case 28: this.ViewModel_Table.Day_28_Notification = Format(); return;
                case 29: this.ViewModel_Table.Day_29_Notification = Format(); return;
                case 30: this.ViewModel_Table.Day_30_Notification = Format(); return;
                case 31: this.ViewModel_Table.Day_31_Notification = Format(); return;
            }

            string Format()
            {
                var date = this.ConvertDayToDate(day);

                if (new DateValue(date).IsWeekend || this.IsHoliday(date))
                {
                    return "休日";
                }

                var workingPlace = this.SearchWorkingPlace(date);

                if (this.IsA_Working(workingPlace))
                {
                    return this.IsPaidVacation(date) ? "Ａ勤務　年次有給休暇（有休）" : "Ａ勤務";
                }

                return string.Empty;
            }
        }

        private TimeSpan WorkingTime_Time;

        /// <summary>
        /// 入力 - 勤務時間
        /// </summary>
        /// <param name="day">日</param>
        /// <param name="startTime">始業時間</param>
        /// <param name="endTime">就業時間</param>
        /// <param name="lunchTime">昼休憩時間</param>
        private void InputWorkingTime(int day, DateTime startTime, TimeSpan lunchTime, DateTime endTime)
        {
            switch (day)
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
                this.ViewModel_Header.WorkingTimeTotal = this.ViewModel_Header.WorkingTimeTotal.Add((endTime - startTime) - lunchTime);

                this.WorkingTime_Time = (endTime - startTime) - lunchTime;
                return this.WorkingTime_Time.ToString(@"hh\:mm");
            }
        }

        /// <summary>
        /// 入力 - 残業時間
        /// </summary>
        /// <param name="day">日付</param>
        /// <remarks>
        /// 必ず勤務時間の算出後に指定すること。
        /// </remarks>
        private void InputOvertime(int day)
        {
            var workingPlace = this.SearchWorkingPlace(this.ConvertDayToDate(day));

            if (workingPlace is null)
            {
                return;
            }

            switch (day)
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
                var overTime  = WorkingTime_Time - workingPlace.WorkTime;

                if (overTime.TotalMinutes > 0)
                {
                    this.ViewModel_Header.OvertimeTotal = this.ViewModel_Header.OvertimeTotal.Add(overTime);
                    return overTime.ToString(@"hh\:mm");
                }

                return "00:00";
            }
        }

        /// <summary>
        /// Input - 備考
        /// </summary>
        /// <param name="day">日</param>
        /// <param name="startTime">始業時間</param>
        /// <param name="endTime">就業時間</param>
        /// <param name="workPlace">勤務場所</param>
        private void InputRemarks(int day, DateTime startTime, DateTime endTime, string workPlace)
        {
            var home = Homes.FetchByDate(this.ConvertDayToDate(day));

            if (home is null)
            {
                return;
            }

            switch (day)
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
                var isWorkAtHome = ((endTime.Hour - startTime.Hour) >= 8 && workPlace == home.Address_Google);

                return isWorkAtHome ? "在宅所定時間以上" : string.Empty;
            }
        }

        /// <summary>
        /// 就業先を検索する
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private WorkingPlaceEntity SearchWorkingPlace(DateTime date)
        {
            var workingPlace = WorkingPlace.FetchByDate(date);

            if (workingPlace.Count == 1 &&
                workingPlace.ToList().Any(x => x.IsWaiting))
            {
                // 待機
                return workingPlace.FirstOrDefault();
            }

            // 常駐先
            return workingPlace.Where(x => x.DispatchedCompany.Text == this.ViewModel_Header.DispatchedCompany).FirstOrDefault();
        }

        /// <summary>
        /// 月初日付を取得
        /// </summary>
        /// <returns>月初日</returns>
        public DateTime FirstDateOfMonth
            => new DateTime(this.ViewModel_Header.Year, this.ViewModel_Header.Month, 1);

        /// <summary>
        /// 月末日
        /// </summary>
        public int LastDayOfMonth
            => new DateValue(this.ViewModel_Header.Year, this.ViewModel_Header.Month).LastDayOfMonth;

        /// <summary>
        /// 月末日付をDateTime形式で取得
        /// </summary>
        /// <returns>月末日</returns>
        public DateTime LastDateOfMonth
            => new DateValue(this.ViewModel_Header.Year, this.ViewModel_Header.Month).LastDateOfMonth;

        /// <summary>
        /// 指定した日のDateTime値を取得
        /// </summary>
        /// <param name="day">日</param>
        /// <returns>DateTime値</returns>
        private DateTime ConvertDayToDate(int day)
            => new DateTime(this.ViewModel_Header.Year, this.ViewModel_Header.Month, day);

        /// <summary>
        /// 指定した日がA勤務か
        /// </summary>
        /// <param name="workingPlace">就業場所</param>
        /// <returns>A勤務か</returns>
        private bool IsA_Working(WorkingPlaceEntity workingPlace)
            => workingPlace?.WorkingTime.Start.Hours == 9 &&
               workingPlace?.WorkingTime.End.Hours   == 18;

        /// <summary>
        /// 指定した日が年休取得日か 
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>年休有無</returns>
        private bool IsPaidVacation(DateTime date)
            => CalendarReader.FindByTitle("年休", date).FirstOrDefault() != null;

        /// <summary> ViewModel - 勤務表 </summary>
        internal ViewModel_WorkSchedule_Table ViewModel_Table { get; set; }

        /// <summary> ViewModel - 勤務表 </summary>
        internal ViewModel_WorkSchedule_Header ViewModel_Header { get; set; }
    }
}
