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

        internal void Return()
        {
            this.ViewModel_Header.TargetDate = this.ViewModel_Header.TargetDate.AddMonths(-1);
            Initialize_Table();
        }

        internal void Proceed()
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
            this.ViewModel_Header.Year_Text.Value  = this.ViewModel_Header.TargetDate.Year;
            this.ViewModel_Header.Month_Text.Value = this.ViewModel_Header.TargetDate.Month;

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
                this.InputWorkingTime(day, startTime, endTime);

                // 残業時間
                this.InputOvertime(day, startTime, endTime);

                // 備考
                this.InputRemarks(day, startTime, endTime, entities.First().Place);
            }

            // 勤務日数
            this.ViewModel_Header.WorkDaysTotal_Text.Value = this.WorkDaysTotal.ToString();

            // 合計 - 勤務時間
            this.ViewModel_Header.WorkingTimeTotal_Text.Value = Math.Truncate(this.ViewModel_Header.WorkingTimeTotal.TotalHours) + ":" + 
                                                          this.ViewModel_Header.WorkingTimeTotal.Minutes.ToString("00");

            // 合計 - 残業時間
            this.ViewModel_Header.OvertimeTotal_Text.Value = Math.Truncate(this.ViewModel_Header.OvertimeTotal.TotalHours) + ":" +
                                                       this.ViewModel_Header.OvertimeTotal.Minutes.ToString("00");
        }

        /// <summary>
        /// 祝日の取得
        /// </summary>
        /// <param name="date">日付</param>
        private void GetHoliday(DateTime date)
        {
            var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

            if (holidays.Any() == false)
            {
                return;
            }

            switch (date.Day)
            {
                case 1 : this.ViewModel_Table.Day1_Background.Value  = Format(); return;
                case 2 : this.ViewModel_Table.Day2_Background.Value  = Format(); return;
                case 3 : this.ViewModel_Table.Day3_Background.Value  = Format(); return;
                case 4 : this.ViewModel_Table.Day4_Background.Value  = Format(); return;
                case 5 : this.ViewModel_Table.Day5_Background.Value  = Format(); return;
                case 6 : this.ViewModel_Table.Day6_Background.Value  = Format(); return;
                case 7 : this.ViewModel_Table.Day7_Background.Value  = Format(); return;
                case 8 : this.ViewModel_Table.Day8_Background.Value  = Format(); return;
                case 9 : this.ViewModel_Table.Day9_Background.Value  = Format(); return;
                case 10: this.ViewModel_Table.Day10_Background.Value = Format(); return;
                case 11: this.ViewModel_Table.Day11_Background.Value = Format(); return;
                case 12: this.ViewModel_Table.Day12_Background.Value = Format(); return;
                case 13: this.ViewModel_Table.Day13_Background.Value = Format(); return;
                case 14: this.ViewModel_Table.Day14_Background.Value = Format(); return;
                case 15: this.ViewModel_Table.Day15_Background.Value = Format(); return;
                case 16: this.ViewModel_Table.Day16_Background.Value = Format(); return;
                case 17: this.ViewModel_Table.Day17_Background.Value = Format(); return;
                case 18: this.ViewModel_Table.Day18_Background.Value = Format(); return;
                case 19: this.ViewModel_Table.Day19_Background.Value = Format(); return;
                case 20: this.ViewModel_Table.Day20_Background.Value = Format(); return;
                case 21: this.ViewModel_Table.Day21_Background.Value = Format(); return;
                case 22: this.ViewModel_Table.Day22_Background.Value = Format(); return;
                case 23: this.ViewModel_Table.Day23_Background.Value = Format(); return;
                case 24: this.ViewModel_Table.Day24_Background.Value = Format(); return;
                case 25: this.ViewModel_Table.Day25_Background.Value = Format(); return;
                case 26: this.ViewModel_Table.Day26_Background.Value = Format(); return;
                case 27: this.ViewModel_Table.Day27_Background.Value = Format(); return;
                case 28: this.ViewModel_Table.Day28_Background.Value = Format(); return;
                case 29: this.ViewModel_Table.Day29_Background.Value = Format(); return;
                case 30: this.ViewModel_Table.Day30_Background.Value = Format(); return;
                case 31: this.ViewModel_Table.Day31_Background.Value = Format(); return;

                default: return;
            }

            SolidColorBrush Format()
            {
                if (this.IsHoliday(date))
                {
                    return new SolidColorBrush(Color.FromRgb(252, 229, 205));
                }

                var dateValue = new DateValue(date);

                if (dateValue.IsSaturday)
                {
                    return new SolidColorBrush(Color.FromRgb(201, 218, 248));
                }

                if (dateValue.IsSunday)
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

        /// <summary>
        /// 祝日の名称を取得
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>祝日名</returns>
        private string GetHolidayName(DateTime date)
        {
            var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

            var holiday = holidays.Where(x => x.Date == date).FirstOrDefault();

            if (string.IsNullOrEmpty(holiday.CompanyName) == false)
            {
                // 会社休日
                if (holiday.CompanyName == this.ViewModel_Header.DispatchingCompany_Text.Value ||
                    holiday.CompanyName == this.ViewModel_Header.DispatchedCompany_Text.Value)
                {
                    return $"会社休日：{holiday.CompanyName}";
                }
            }

            return holiday.Name;
        }

        /// <summary>
        /// クリア
        /// </summary>
        private void Clear()
        {
            // 日付
            this.ViewModel_Table.Day1_Text.Value  = string.Empty;
            this.ViewModel_Table.Day2_Text.Value  = string.Empty;
            this.ViewModel_Table.Day3_Text.Value  = string.Empty;
            this.ViewModel_Table.Day4_Text.Value  = string.Empty;
            this.ViewModel_Table.Day5_Text.Value  = string.Empty;
            this.ViewModel_Table.Day6_Text.Value  = string.Empty;
            this.ViewModel_Table.Day7_Text.Value  = string.Empty;
            this.ViewModel_Table.Day8_Text.Value  = string.Empty;
            this.ViewModel_Table.Day9_Text.Value  = string.Empty;
            this.ViewModel_Table.Day10_Text.Value = string.Empty;
            this.ViewModel_Table.Day11_Text.Value = string.Empty;
            this.ViewModel_Table.Day12_Text.Value = string.Empty;
            this.ViewModel_Table.Day13_Text.Value = string.Empty;
            this.ViewModel_Table.Day14_Text.Value = string.Empty;
            this.ViewModel_Table.Day15_Text.Value = string.Empty;
            this.ViewModel_Table.Day16_Text.Value = string.Empty;
            this.ViewModel_Table.Day17_Text.Value = string.Empty;
            this.ViewModel_Table.Day18_Text.Value = string.Empty;
            this.ViewModel_Table.Day19_Text.Value = string.Empty;
            this.ViewModel_Table.Day20_Text.Value = string.Empty;
            this.ViewModel_Table.Day21_Text.Value = string.Empty;
            this.ViewModel_Table.Day22_Text.Value = string.Empty;
            this.ViewModel_Table.Day23_Text.Value = string.Empty;
            this.ViewModel_Table.Day24_Text.Value = string.Empty;
            this.ViewModel_Table.Day25_Text.Value = string.Empty;
            this.ViewModel_Table.Day26_Text.Value = string.Empty;
            this.ViewModel_Table.Day27_Text.Value = string.Empty;
            this.ViewModel_Table.Day28_Text.Value = string.Empty;
            this.ViewModel_Table.Day29_Text.Value = string.Empty;
            this.ViewModel_Table.Day30_Text.Value = string.Empty;
            this.ViewModel_Table.Day31_Text.Value = string.Empty;

            // 始業時間
            this.ViewModel_Table.Day1_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day2_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day3_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day4_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day5_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day6_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day7_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day8_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day9_StartTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day10_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day11_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day12_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day13_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day14_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day15_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day16_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day17_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day18_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day19_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day20_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day21_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day22_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day23_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day24_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day25_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day26_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day27_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day28_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day29_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day30_StartTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day31_StartTime_Text.Value = string.Empty;

            // 終業時間
            this.ViewModel_Table.Day1_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day2_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day3_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day4_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day5_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day6_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day7_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day8_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day9_EndTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day10_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day11_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day12_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day13_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day14_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day15_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day16_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day17_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day18_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day19_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day20_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day21_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day22_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day23_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day24_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day25_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day26_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day27_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day28_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day29_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day30_EndTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day31_EndTime_Text.Value = string.Empty;

            // 昼食時間
            this.ViewModel_Table.Day1_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day2_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day3_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day4_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day5_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day6_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day7_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day8_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day9_LunchTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day10_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day11_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day12_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day13_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day14_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day15_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day16_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day17_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day18_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day19_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day20_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day21_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day22_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day23_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day24_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day25_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day26_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day27_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day28_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day29_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day30_LunchTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day31_LunchTime_Text.Value = string.Empty;

            // 届出
            this.ViewModel_Table.Day1_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day2_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day3_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day4_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day5_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day6_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day7_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day8_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day9_Notification_Text.Value  = string.Empty;
            this.ViewModel_Table.Day10_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day11_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day12_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day13_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day14_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day15_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day16_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day17_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day18_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day19_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day20_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day21_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day22_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day23_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day24_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day25_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day26_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day27_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day28_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day29_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day30_Notification_Text.Value = string.Empty;
            this.ViewModel_Table.Day31_Notification_Text.Value = string.Empty;

            // 勤務時間
            this.ViewModel_Header.WorkingTimeTotal = new TimeSpan();
            this.WorkDaysTotal = 0;

            this.ViewModel_Table.Day1_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day2_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day3_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day4_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day5_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day6_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day7_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day8_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day9_WorkingTime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day10_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day11_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day12_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day13_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day14_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day15_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day16_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day17_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day18_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day19_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day20_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day21_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day22_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day23_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day24_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day25_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day26_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day27_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day28_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day29_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day30_WorkingTime_Text.Value = string.Empty;
            this.ViewModel_Table.Day31_WorkingTime_Text.Value = string.Empty;

            // 残業時間
            this.ViewModel_Header.OvertimeTotal = new TimeSpan();

            this.ViewModel_Table.Day1_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day2_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day3_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day4_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day5_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day6_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day7_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day8_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day9_Overtime_Text.Value  = string.Empty;
            this.ViewModel_Table.Day10_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day11_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day12_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day13_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day14_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day15_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day16_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day17_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day18_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day19_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day20_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day21_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day22_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day23_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day24_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day25_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day26_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day27_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day28_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day29_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day30_Overtime_Text.Value = string.Empty;
            this.ViewModel_Table.Day31_Overtime_Text.Value = string.Empty;

            // 備考
            this.ViewModel_Table.Day1_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day2_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day3_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day4_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day5_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day6_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day7_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day8_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day9_Remarks_Text.Value  = string.Empty;
            this.ViewModel_Table.Day10_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day11_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day12_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day13_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day14_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day15_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day16_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day17_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day18_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day19_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day20_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day21_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day22_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day23_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day24_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day25_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day26_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day27_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day28_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day29_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day30_Remarks_Text.Value = string.Empty;
            this.ViewModel_Table.Day31_Remarks_Text.Value = string.Empty;
        }

        /// <summary>
        /// 就業場所を取得する
        /// </summary>
        /// <returns>就業場所</returns>
        private void GetCompany()
        {
            var workingPlace = WorkingPlace.FetchByDate(this.FirstDateOfMonth);

            this.ViewModel_Header.DispatchingCompany_Text.Value = workingPlace.Select(x => x.DispatchingCompany).Distinct().FirstOrDefault().Text;
            this.ViewModel_Header.DispatchedCompany_Text.Value  = workingPlace.Select(x => x.DispatchedCompany).Distinct().FirstOrDefault().Text;
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
                case 1  : this.ViewModel_Table.Day1_Text.Value  = Format(); return;
                case 2  : this.ViewModel_Table.Day2_Text.Value  = Format(); return;
                case 3  : this.ViewModel_Table.Day3_Text.Value  = Format(); return;
                case 4  : this.ViewModel_Table.Day4_Text.Value  = Format(); return;
                case 5  : this.ViewModel_Table.Day5_Text.Value  = Format(); return;
                case 6  : this.ViewModel_Table.Day6_Text.Value  = Format(); return;
                case 7  : this.ViewModel_Table.Day7_Text.Value  = Format(); return;
                case 8  : this.ViewModel_Table.Day8_Text.Value  = Format(); return;
                case 9  : this.ViewModel_Table.Day9_Text.Value  = Format(); return;
                case 10 : this.ViewModel_Table.Day10_Text.Value = Format(); return;
                case 11 : this.ViewModel_Table.Day11_Text.Value = Format(); return;
                case 12 : this.ViewModel_Table.Day12_Text.Value = Format(); return;
                case 13 : this.ViewModel_Table.Day13_Text.Value = Format(); return;
                case 14 : this.ViewModel_Table.Day14_Text.Value = Format(); return;
                case 15 : this.ViewModel_Table.Day15_Text.Value = Format(); return;
                case 16 : this.ViewModel_Table.Day16_Text.Value = Format(); return;
                case 17 : this.ViewModel_Table.Day17_Text.Value = Format(); return;
                case 18 : this.ViewModel_Table.Day18_Text.Value = Format(); return;
                case 19 : this.ViewModel_Table.Day19_Text.Value = Format(); return;
                case 20 : this.ViewModel_Table.Day20_Text.Value = Format(); return;
                case 21 : this.ViewModel_Table.Day21_Text.Value = Format(); return;
                case 22 : this.ViewModel_Table.Day22_Text.Value = Format(); return;
                case 23 : this.ViewModel_Table.Day23_Text.Value = Format(); return;
                case 24 : this.ViewModel_Table.Day24_Text.Value = Format(); return;
                case 25 : this.ViewModel_Table.Day25_Text.Value = Format(); return;
                case 26 : this.ViewModel_Table.Day26_Text.Value = Format(); return;
                case 27 : this.ViewModel_Table.Day27_Text.Value = Format(); return;
                case 28 : this.ViewModel_Table.Day28_Text.Value = Format(); return;
                case 29 : this.ViewModel_Table.Day29_Text.Value = Format(); return;
                case 30 : this.ViewModel_Table.Day30_Text.Value = Format(); return;
                case 31 : this.ViewModel_Table.Day31_Text.Value = Format(); return;

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
        /// <remarks>
        /// 「hh:mm」形式で始業時間を求める
        /// </remarks>
        private void InputStartTime(int day, DateTime startTime)
        {
            switch (day)
            {
                case 1 : this.ViewModel_Table.Day1_StartTime_Text.Value  = Format(); return;
                case 2 : this.ViewModel_Table.Day2_StartTime_Text.Value  = Format(); return;
                case 3 : this.ViewModel_Table.Day3_StartTime_Text.Value  = Format(); return;
                case 4 : this.ViewModel_Table.Day4_StartTime_Text.Value  = Format(); return;
                case 5 : this.ViewModel_Table.Day5_StartTime_Text.Value  = Format(); return;
                case 6 : this.ViewModel_Table.Day6_StartTime_Text.Value  = Format(); return;
                case 7 : this.ViewModel_Table.Day7_StartTime_Text.Value  = Format(); return;
                case 8 : this.ViewModel_Table.Day8_StartTime_Text.Value  = Format(); return;
                case 9 : this.ViewModel_Table.Day9_StartTime_Text.Value  = Format(); return;
                case 10: this.ViewModel_Table.Day10_StartTime_Text.Value = Format(); return;
                case 11: this.ViewModel_Table.Day11_StartTime_Text.Value = Format(); return;
                case 12: this.ViewModel_Table.Day12_StartTime_Text.Value = Format(); return;
                case 13: this.ViewModel_Table.Day13_StartTime_Text.Value = Format(); return;
                case 14: this.ViewModel_Table.Day14_StartTime_Text.Value = Format(); return;
                case 15: this.ViewModel_Table.Day15_StartTime_Text.Value = Format(); return;
                case 16: this.ViewModel_Table.Day16_StartTime_Text.Value = Format(); return;
                case 17: this.ViewModel_Table.Day17_StartTime_Text.Value = Format(); return;
                case 18: this.ViewModel_Table.Day18_StartTime_Text.Value = Format(); return;
                case 19: this.ViewModel_Table.Day19_StartTime_Text.Value = Format(); return;
                case 20: this.ViewModel_Table.Day20_StartTime_Text.Value = Format(); return;
                case 21: this.ViewModel_Table.Day21_StartTime_Text.Value = Format(); return;
                case 22: this.ViewModel_Table.Day22_StartTime_Text.Value = Format(); return;
                case 23: this.ViewModel_Table.Day23_StartTime_Text.Value = Format(); return;
                case 24: this.ViewModel_Table.Day24_StartTime_Text.Value = Format(); return;
                case 25: this.ViewModel_Table.Day25_StartTime_Text.Value = Format(); return;
                case 26: this.ViewModel_Table.Day26_StartTime_Text.Value = Format(); return;
                case 27: this.ViewModel_Table.Day27_StartTime_Text.Value = Format(); return;
                case 28: this.ViewModel_Table.Day28_StartTime_Text.Value = Format(); return;
                case 29: this.ViewModel_Table.Day29_StartTime_Text.Value = Format(); return;
                case 30: this.ViewModel_Table.Day30_StartTime_Text.Value = Format(); return;
                case 31: this.ViewModel_Table.Day31_StartTime_Text.Value = Format(); return;
            }

            string Format()
                => $"{startTime.Hour.ToString("00")}:{startTime.Minute.ToString("00")}";
        }

        /// <summary>
        /// 入力 - 終業時間
        /// </summary>
        /// <param name="day">対象日</param>
        /// <param name="endTime">終業時刻</param>
        /// <remarks>
        /// 「hh:mm」形式で終業時間を求める
        /// </remarks>
        private void InputEndTime(int day, DateTime endTime)
        {
            switch (day)
            {
                case 1:  this.ViewModel_Table.Day1_EndTime_Text.Value  = Format(); return;
                case 2:  this.ViewModel_Table.Day2_EndTime_Text.Value  = Format(); return;
                case 3:  this.ViewModel_Table.Day3_EndTime_Text.Value  = Format(); return;
                case 4:  this.ViewModel_Table.Day4_EndTime_Text.Value  = Format(); return;
                case 5:  this.ViewModel_Table.Day5_EndTime_Text.Value  = Format(); return;
                case 6:  this.ViewModel_Table.Day6_EndTime_Text.Value  = Format(); return;
                case 7:  this.ViewModel_Table.Day7_EndTime_Text.Value  = Format(); return;
                case 8:  this.ViewModel_Table.Day8_EndTime_Text.Value  = Format(); return;
                case 9:  this.ViewModel_Table.Day9_EndTime_Text.Value  = Format(); return;
                case 10: this.ViewModel_Table.Day10_EndTime_Text.Value = Format(); return;
                case 11: this.ViewModel_Table.Day11_EndTime_Text.Value = Format(); return;
                case 12: this.ViewModel_Table.Day12_EndTime_Text.Value = Format(); return;
                case 13: this.ViewModel_Table.Day13_EndTime_Text.Value = Format(); return;
                case 14: this.ViewModel_Table.Day14_EndTime_Text.Value = Format(); return;
                case 15: this.ViewModel_Table.Day15_EndTime_Text.Value = Format(); return;
                case 16: this.ViewModel_Table.Day16_EndTime_Text.Value = Format(); return;
                case 17: this.ViewModel_Table.Day17_EndTime_Text.Value = Format(); return;
                case 18: this.ViewModel_Table.Day18_EndTime_Text.Value = Format(); return;
                case 19: this.ViewModel_Table.Day19_EndTime_Text.Value = Format(); return;
                case 20: this.ViewModel_Table.Day20_EndTime_Text.Value = Format(); return;
                case 21: this.ViewModel_Table.Day21_EndTime_Text.Value = Format(); return;
                case 22: this.ViewModel_Table.Day22_EndTime_Text.Value = Format(); return;
                case 23: this.ViewModel_Table.Day23_EndTime_Text.Value = Format(); return;
                case 24: this.ViewModel_Table.Day24_EndTime_Text.Value = Format(); return;
                case 25: this.ViewModel_Table.Day25_EndTime_Text.Value = Format(); return;
                case 26: this.ViewModel_Table.Day26_EndTime_Text.Value = Format(); return;
                case 27: this.ViewModel_Table.Day27_EndTime_Text.Value = Format(); return;
                case 28: this.ViewModel_Table.Day28_EndTime_Text.Value = Format(); return;
                case 29: this.ViewModel_Table.Day29_EndTime_Text.Value = Format(); return;
                case 30: this.ViewModel_Table.Day30_EndTime_Text.Value = Format(); return;
                case 31: this.ViewModel_Table.Day31_EndTime_Text.Value = Format(); return;
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
                case 1:  this.ViewModel_Table.Day1_LunchTime_Text.Value  = Format(); return;
                case 2:  this.ViewModel_Table.Day2_LunchTime_Text.Value  = Format(); return;
                case 3:  this.ViewModel_Table.Day3_LunchTime_Text.Value  = Format(); return;
                case 4:  this.ViewModel_Table.Day4_LunchTime_Text.Value  = Format(); return;
                case 5:  this.ViewModel_Table.Day5_LunchTime_Text.Value  = Format(); return;
                case 6:  this.ViewModel_Table.Day6_LunchTime_Text.Value  = Format(); return;
                case 7:  this.ViewModel_Table.Day7_LunchTime_Text.Value  = Format(); return;
                case 8:  this.ViewModel_Table.Day8_LunchTime_Text.Value  = Format(); return;
                case 9:  this.ViewModel_Table.Day9_LunchTime_Text.Value  = Format(); return;
                case 10: this.ViewModel_Table.Day10_LunchTime_Text.Value = Format(); return;
                case 11: this.ViewModel_Table.Day11_LunchTime_Text.Value = Format(); return;
                case 12: this.ViewModel_Table.Day12_LunchTime_Text.Value = Format(); return;
                case 13: this.ViewModel_Table.Day13_LunchTime_Text.Value = Format(); return;
                case 14: this.ViewModel_Table.Day14_LunchTime_Text.Value = Format(); return;
                case 15: this.ViewModel_Table.Day15_LunchTime_Text.Value = Format(); return;
                case 16: this.ViewModel_Table.Day16_LunchTime_Text.Value = Format(); return;
                case 17: this.ViewModel_Table.Day17_LunchTime_Text.Value = Format(); return;
                case 18: this.ViewModel_Table.Day18_LunchTime_Text.Value = Format(); return;
                case 19: this.ViewModel_Table.Day19_LunchTime_Text.Value = Format(); return;
                case 20: this.ViewModel_Table.Day20_LunchTime_Text.Value = Format(); return;
                case 21: this.ViewModel_Table.Day21_LunchTime_Text.Value = Format(); return;
                case 22: this.ViewModel_Table.Day22_LunchTime_Text.Value = Format(); return;
                case 23: this.ViewModel_Table.Day23_LunchTime_Text.Value = Format(); return;
                case 24: this.ViewModel_Table.Day24_LunchTime_Text.Value = Format(); return;
                case 25: this.ViewModel_Table.Day25_LunchTime_Text.Value = Format(); return;
                case 26: this.ViewModel_Table.Day26_LunchTime_Text.Value = Format(); return;
                case 27: this.ViewModel_Table.Day27_LunchTime_Text.Value = Format(); return;
                case 28: this.ViewModel_Table.Day28_LunchTime_Text.Value = Format(); return;
                case 29: this.ViewModel_Table.Day29_LunchTime_Text.Value = Format(); return;
                case 30: this.ViewModel_Table.Day30_LunchTime_Text.Value = Format(); return;
                case 31: this.ViewModel_Table.Day31_LunchTime_Text.Value = Format(); return;
            }

            string Format()
                => workingPlace.LunchTimeSpan.ToString(@"hh\:mm");
        }

        /// <summary>
        /// 入力 - 届出
        /// </summary>
        /// <param name="day">日</param>
        private void InputNotification(int day)
        {
            switch (day)
            {
                case 1:  this.ViewModel_Table.Day1_Notification_Text.Value  = Format(); return;
                case 2:  this.ViewModel_Table.Day2_Notification_Text.Value  = Format(); return;
                case 3:  this.ViewModel_Table.Day3_Notification_Text.Value  = Format(); return;
                case 4:  this.ViewModel_Table.Day4_Notification_Text.Value  = Format(); return;
                case 5:  this.ViewModel_Table.Day5_Notification_Text.Value  = Format(); return;
                case 6:  this.ViewModel_Table.Day6_Notification_Text.Value  = Format(); return;
                case 7:  this.ViewModel_Table.Day7_Notification_Text.Value  = Format(); return;
                case 8:  this.ViewModel_Table.Day8_Notification_Text.Value  = Format(); return;
                case 9:  this.ViewModel_Table.Day9_Notification_Text.Value  = Format(); return;
                case 10: this.ViewModel_Table.Day10_Notification_Text.Value = Format(); return;
                case 11: this.ViewModel_Table.Day11_Notification_Text.Value = Format(); return;
                case 12: this.ViewModel_Table.Day12_Notification_Text.Value = Format(); return;
                case 13: this.ViewModel_Table.Day13_Notification_Text.Value = Format(); return;
                case 14: this.ViewModel_Table.Day14_Notification_Text.Value = Format(); return;
                case 15: this.ViewModel_Table.Day15_Notification_Text.Value = Format(); return;
                case 16: this.ViewModel_Table.Day16_Notification_Text.Value = Format(); return;
                case 17: this.ViewModel_Table.Day17_Notification_Text.Value = Format(); return;
                case 18: this.ViewModel_Table.Day18_Notification_Text.Value = Format(); return;
                case 19: this.ViewModel_Table.Day19_Notification_Text.Value = Format(); return;
                case 20: this.ViewModel_Table.Day20_Notification_Text.Value = Format(); return;
                case 21: this.ViewModel_Table.Day21_Notification_Text.Value = Format(); return;
                case 22: this.ViewModel_Table.Day22_Notification_Text.Value = Format(); return;
                case 23: this.ViewModel_Table.Day23_Notification_Text.Value = Format(); return;
                case 24: this.ViewModel_Table.Day24_Notification_Text.Value = Format(); return;
                case 25: this.ViewModel_Table.Day25_Notification_Text.Value = Format(); return;
                case 26: this.ViewModel_Table.Day26_Notification_Text.Value = Format(); return;
                case 27: this.ViewModel_Table.Day27_Notification_Text.Value = Format(); return;
                case 28: this.ViewModel_Table.Day28_Notification_Text.Value = Format(); return;
                case 29: this.ViewModel_Table.Day29_Notification_Text.Value = Format(); return;
                case 30: this.ViewModel_Table.Day30_Notification_Text.Value = Format(); return;
                case 31: this.ViewModel_Table.Day31_Notification_Text.Value = Format(); return;
            }

            string Format()
            {
                var date = this.ConvertDayToDate(day);

                if (this.IsHoliday(date))
                {
                    // 祝日マスタに登録あり
                    var holidayName = this.GetHolidayName(date);
                    if (holidayName.Contains("会社休日"))
                    {
                        holidayName = holidayName.Replace("会社休日：", string.Empty);
                        return $"会社休日（{holidayName}）";
                    }
                    else
                    {
                        return $"祝日（{holidayName}）";
                    }
                }

                if (new DateValue(date).IsWeekend)
                {
                    return "休日";
                }

                var workingPlace = this.SearchWorkingPlace(date);

                if (this.IsA_Working(workingPlace))
                {
                    return this.IsPaidVacation(date) ? "A勤務　年次有給休暇（有休）" : "A勤務";
                }
                else if (this.IsB_Working(workingPlace))
                {
                    return this.IsPaidVacation(date) ? "B勤務　年次有給休暇（有休）" : "B勤務";
                }
                else if (this.IsC_Working(workingPlace))
                {
                    return this.IsPaidVacation(date) ? "C勤務　年次有給休暇（有休）" : "C勤務";
                }

                return string.Empty;
            }
        }

        private TimeSpan WorkingTime_Time;

        public int WorkDaysTotal;

        /// <summary>
        /// 入力 - 勤務時間
        /// </summary>
        /// <param name="day">日</param>
        /// <param name="startTime">始業時間</param>
        /// <param name="endTime">就業時間</param>
        private void InputWorkingTime(int day, DateTime startTime, DateTime endTime)
        {
            var workingPlace = this.SearchWorkingPlace(this.ConvertDayToDate(day));

            if (workingPlace is null)
            {
                return;
            }

            switch (day)
            {
                case 1:  this.ViewModel_Table.Day1_WorkingTime_Text.Value  = Format(); return;
                case 2:  this.ViewModel_Table.Day2_WorkingTime_Text.Value  = Format(); return;
                case 3:  this.ViewModel_Table.Day3_WorkingTime_Text.Value  = Format(); return;
                case 4:  this.ViewModel_Table.Day4_WorkingTime_Text.Value  = Format(); return;
                case 5:  this.ViewModel_Table.Day5_WorkingTime_Text.Value  = Format(); return;
                case 6:  this.ViewModel_Table.Day6_WorkingTime_Text.Value  = Format(); return;
                case 7:  this.ViewModel_Table.Day7_WorkingTime_Text.Value  = Format(); return;
                case 8:  this.ViewModel_Table.Day8_WorkingTime_Text.Value  = Format(); return;
                case 9:  this.ViewModel_Table.Day9_WorkingTime_Text.Value  = Format(); return;
                case 10: this.ViewModel_Table.Day10_WorkingTime_Text.Value = Format(); return;
                case 11: this.ViewModel_Table.Day11_WorkingTime_Text.Value = Format(); return;
                case 12: this.ViewModel_Table.Day12_WorkingTime_Text.Value = Format(); return;
                case 13: this.ViewModel_Table.Day13_WorkingTime_Text.Value = Format(); return;
                case 14: this.ViewModel_Table.Day14_WorkingTime_Text.Value = Format(); return;
                case 15: this.ViewModel_Table.Day15_WorkingTime_Text.Value = Format(); return;
                case 16: this.ViewModel_Table.Day16_WorkingTime_Text.Value = Format(); return;
                case 17: this.ViewModel_Table.Day17_WorkingTime_Text.Value = Format(); return;
                case 18: this.ViewModel_Table.Day18_WorkingTime_Text.Value = Format(); return;
                case 19: this.ViewModel_Table.Day19_WorkingTime_Text.Value = Format(); return;
                case 20: this.ViewModel_Table.Day20_WorkingTime_Text.Value = Format(); return;
                case 21: this.ViewModel_Table.Day21_WorkingTime_Text.Value = Format(); return;
                case 22: this.ViewModel_Table.Day22_WorkingTime_Text.Value = Format(); return;
                case 23: this.ViewModel_Table.Day23_WorkingTime_Text.Value = Format(); return;
                case 24: this.ViewModel_Table.Day24_WorkingTime_Text.Value = Format(); return;
                case 25: this.ViewModel_Table.Day25_WorkingTime_Text.Value = Format(); return;
                case 26: this.ViewModel_Table.Day26_WorkingTime_Text.Value = Format(); return;
                case 27: this.ViewModel_Table.Day27_WorkingTime_Text.Value = Format(); return;
                case 28: this.ViewModel_Table.Day28_WorkingTime_Text.Value = Format(); return;
                case 29: this.ViewModel_Table.Day29_WorkingTime_Text.Value = Format(); return;
                case 30: this.ViewModel_Table.Day30_WorkingTime_Text.Value = Format(); return;
                case 31: this.ViewModel_Table.Day31_WorkingTime_Text.Value = Format(); return;
            }

            string Format()
            {
                this.WorkDaysTotal += 1;

                this.WorkingTime_Time = (endTime - startTime) - workingPlace.LunchTimeSpan;

                if (this.WorkingTime_Time > new TimeSpan(8, 0, 0))
                {
                    this.WorkingTime_Time = new TimeSpan(8, 0, 0);
                }

                this.ViewModel_Header.WorkingTimeTotal = this.ViewModel_Header.WorkingTimeTotal.Add(this.WorkingTime_Time);

                return this.WorkingTime_Time.ToString(@"hh\:mm");
            }
        }

        /// <summary>
        /// 入力 - 残業時間
        /// </summary>
        /// <param name="day">日付</param>
        /// <param name="startTime">昼休憩</param>
        /// <param name="endTime">昼休憩</param>
        /// <remarks>
        /// 必ず勤務時間の算出後に指定すること。
        /// </remarks>
        private void InputOvertime(int day, DateTime startTime, DateTime endTime)
        {
            var workingPlace = this.SearchWorkingPlace(this.ConvertDayToDate(day));

            if (workingPlace is null)
            {
                return;
            }

            switch (day)
            {
                case 1:  this.ViewModel_Table.Day1_Overtime_Text.Value  = Format(); return;
                case 2:  this.ViewModel_Table.Day2_Overtime_Text.Value  = Format(); return;
                case 3:  this.ViewModel_Table.Day3_Overtime_Text.Value  = Format(); return;
                case 4:  this.ViewModel_Table.Day4_Overtime_Text.Value  = Format(); return;
                case 5:  this.ViewModel_Table.Day5_Overtime_Text.Value  = Format(); return;
                case 6:  this.ViewModel_Table.Day6_Overtime_Text.Value  = Format(); return;
                case 7:  this.ViewModel_Table.Day7_Overtime_Text.Value  = Format(); return;
                case 8:  this.ViewModel_Table.Day8_Overtime_Text.Value  = Format(); return;
                case 9:  this.ViewModel_Table.Day9_Overtime_Text.Value  = Format(); return;
                case 10: this.ViewModel_Table.Day10_Overtime_Text.Value = Format(); return;
                case 11: this.ViewModel_Table.Day11_Overtime_Text.Value = Format(); return;
                case 12: this.ViewModel_Table.Day12_Overtime_Text.Value = Format(); return;
                case 13: this.ViewModel_Table.Day13_Overtime_Text.Value = Format(); return;
                case 14: this.ViewModel_Table.Day14_Overtime_Text.Value = Format(); return;
                case 15: this.ViewModel_Table.Day15_Overtime_Text.Value = Format(); return;
                case 16: this.ViewModel_Table.Day16_Overtime_Text.Value = Format(); return;
                case 17: this.ViewModel_Table.Day17_Overtime_Text.Value = Format(); return;
                case 18: this.ViewModel_Table.Day18_Overtime_Text.Value = Format(); return;
                case 19: this.ViewModel_Table.Day19_Overtime_Text.Value = Format(); return;
                case 20: this.ViewModel_Table.Day20_Overtime_Text.Value = Format(); return;
                case 21: this.ViewModel_Table.Day21_Overtime_Text.Value = Format(); return;
                case 22: this.ViewModel_Table.Day22_Overtime_Text.Value = Format(); return;
                case 23: this.ViewModel_Table.Day23_Overtime_Text.Value = Format(); return;
                case 24: this.ViewModel_Table.Day24_Overtime_Text.Value = Format(); return;
                case 25: this.ViewModel_Table.Day25_Overtime_Text.Value = Format(); return;
                case 26: this.ViewModel_Table.Day26_Overtime_Text.Value = Format(); return;
                case 27: this.ViewModel_Table.Day27_Overtime_Text.Value = Format(); return;
                case 28: this.ViewModel_Table.Day28_Overtime_Text.Value = Format(); return;
                case 29: this.ViewModel_Table.Day29_Overtime_Text.Value = Format(); return;
                case 30: this.ViewModel_Table.Day30_Overtime_Text.Value = Format(); return;
                case 31: this.ViewModel_Table.Day31_Overtime_Text.Value = Format(); return;
            }

            string Format()
            {
                var overTime  = (endTime - startTime) - workingPlace.LunchTimeSpan - new TimeSpan(8, 0, 0);

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
                case 1:  this.ViewModel_Table.Day1_Remarks_Text.Value  = Format(); return;
                case 2:  this.ViewModel_Table.Day2_Remarks_Text.Value  = Format(); return;
                case 3:  this.ViewModel_Table.Day3_Remarks_Text.Value  = Format(); return;
                case 4:  this.ViewModel_Table.Day4_Remarks_Text.Value  = Format(); return;
                case 5:  this.ViewModel_Table.Day5_Remarks_Text.Value  = Format(); return;
                case 6:  this.ViewModel_Table.Day6_Remarks_Text.Value  = Format(); return;
                case 7:  this.ViewModel_Table.Day7_Remarks_Text.Value  = Format(); return;
                case 8:  this.ViewModel_Table.Day8_Remarks_Text.Value  = Format(); return;
                case 9:  this.ViewModel_Table.Day9_Remarks_Text.Value  = Format(); return;
                case 10: this.ViewModel_Table.Day10_Remarks_Text.Value = Format(); return;
                case 11: this.ViewModel_Table.Day11_Remarks_Text.Value = Format(); return;
                case 12: this.ViewModel_Table.Day12_Remarks_Text.Value = Format(); return;
                case 13: this.ViewModel_Table.Day13_Remarks_Text.Value = Format(); return;
                case 14: this.ViewModel_Table.Day14_Remarks_Text.Value = Format(); return;
                case 15: this.ViewModel_Table.Day15_Remarks_Text.Value = Format(); return;
                case 16: this.ViewModel_Table.Day16_Remarks_Text.Value = Format(); return;
                case 17: this.ViewModel_Table.Day17_Remarks_Text.Value = Format(); return;
                case 18: this.ViewModel_Table.Day18_Remarks_Text.Value = Format(); return;
                case 19: this.ViewModel_Table.Day19_Remarks_Text.Value = Format(); return;
                case 20: this.ViewModel_Table.Day20_Remarks_Text.Value = Format(); return;
                case 21: this.ViewModel_Table.Day21_Remarks_Text.Value = Format(); return;
                case 22: this.ViewModel_Table.Day22_Remarks_Text.Value = Format(); return;
                case 23: this.ViewModel_Table.Day23_Remarks_Text.Value = Format(); return;
                case 24: this.ViewModel_Table.Day24_Remarks_Text.Value = Format(); return;
                case 25: this.ViewModel_Table.Day25_Remarks_Text.Value = Format(); return;
                case 26: this.ViewModel_Table.Day26_Remarks_Text.Value = Format(); return;
                case 27: this.ViewModel_Table.Day27_Remarks_Text.Value = Format(); return;
                case 28: this.ViewModel_Table.Day28_Remarks_Text.Value = Format(); return;
                case 29: this.ViewModel_Table.Day29_Remarks_Text.Value = Format(); return;
                case 30: this.ViewModel_Table.Day30_Remarks_Text.Value = Format(); return;
                case 31: this.ViewModel_Table.Day31_Remarks_Text.Value = Format(); return;
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
            return workingPlace.Where(x => x.DispatchedCompany.Text == this.ViewModel_Header.DispatchedCompany_Text.Value).FirstOrDefault();
        }

        /// <summary>
        /// 月初日付を取得
        /// </summary>
        /// <returns>月初日</returns>
        public DateTime FirstDateOfMonth
            => new DateTime(this.ViewModel_Header.Year_Text.Value, this.ViewModel_Header.Month_Text.Value, 1);

        /// <summary>
        /// 月末日
        /// </summary>
        public int LastDayOfMonth
            => new DateValue(this.ViewModel_Header.Year_Text.Value, this.ViewModel_Header.Month_Text.Value).LastDayOfMonth;

        /// <summary>
        /// 月末日付をDateTime形式で取得
        /// </summary>
        /// <returns>月末日</returns>
        public DateTime LastDateOfMonth
            => new DateValue(this.ViewModel_Header.Year_Text.Value, this.ViewModel_Header.Month_Text.Value).LastDateOfMonth;

        /// <summary>
        /// 指定した日のDateTime値を取得
        /// </summary>
        /// <param name="day">日</param>
        /// <returns>DateTime値</returns>
        private DateTime ConvertDayToDate(int day)
            => new DateTime(this.ViewModel_Header.Year_Text.Value, this.ViewModel_Header.Month_Text.Value, day);

        /// <summary>
        /// 指定した日がA勤務か
        /// </summary>
        /// <param name="workingPlace">就業場所</param>
        /// <returns>A勤務か</returns>
        private bool IsA_Working(WorkingPlaceEntity workingPlace)
            => workingPlace?.WorkingTime.Start.Hours == 9 &&
               workingPlace?.WorkingTime.End.Hours   <= 18;

        /// <summary>
        /// 指定した日がB勤務か
        /// </summary>
        /// <param name="workingPlace">就業場所</param>
        /// <returns>B勤務か</returns>
        private bool IsB_Working(WorkingPlaceEntity workingPlace)
            => workingPlace?.WorkingTime.Start.Hours == 10 &&
               workingPlace?.WorkingTime.End.Hours   <= 19;

        /// <summary>
        /// 指定した日がC勤務か
        /// </summary>
        /// <param name="workingPlace">就業場所</param>
        /// <returns>B勤務か</returns>
        private bool IsC_Working(WorkingPlaceEntity workingPlace)
            => workingPlace?.WorkingTime.Start.Hours == 11 &&
               workingPlace?.WorkingTime.End.Hours   <= 20;

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
