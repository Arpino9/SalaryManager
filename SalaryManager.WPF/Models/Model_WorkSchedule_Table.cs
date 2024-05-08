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

            this.GetCompany();

            for (var day = 1; day <= this.LastDayOfMonth; day++)
            {
                var date = new DateTime(this.ViewModel_Header.Year_Text.Value,
                                         this.ViewModel_Header.Month_Text.Value,
                                         day);

                var displayDay = new DateValue(date).Date_MMDDWithWeekName;

                var background = this.GetHoliday(date);

                var entities = Noon.Union(Afternoon).Union(Lunch)
                                   .Where(x => x.StartDate.Day == day).ToList();

                // 届出
                var notification = this.InputNotification(day);

                if (entities.Count < 2)
                {
                    // 休祝日
                    var entity2 = new WorkScheduleRecord(
                                    displayDay,
                                    background,
                                    notification);

                    this.SetSchedule(day, entity2);
                    continue;
                }

                DateTime startDate = entities.Min(x => x.StartDate);
                DateTime endDate   = entities.Max(x => x.EndDate);

                // 始業
                var startTime = $"{startDate.Hour.ToString("00")}:{startDate.Minute.ToString("00")}";

                // 昼休憩
                var lunchTime = this.InputLunchTime(day);

                // 終業
                var endTime = $"{endDate.Hour.ToString("00")}:{endDate.Minute.ToString("00")}";

                // 勤務時間
                var workingTime = this.InputWorkingTime(day, startDate, endDate);

                // 残業時間
                var overtime = this.InputOvertime(day, startDate, endDate);

                // 備考
                var remarks = this.InputRemarks(day, startDate, endDate, entities.First().Place);

                var entity = new WorkScheduleRecord(
                                    displayDay,
                                    background,
                                    startTime,
                                    endTime,
                                    lunchTime,
                                    notification,
                                    workingTime,
                                    overtime,
                                    string.Empty,
                                    string.Empty,
                                    remarks);

                this.SetSchedule(day, entity);
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

        private void SetSchedule(int day, WorkScheduleRecord entity)
        {
            switch (day)
            {
                case 1: this.ViewModel_Table.Day1_Schedule.Value = entity; return;
                case 2: this.ViewModel_Table.Day2_Schedule.Value = entity; return;
                case 3: this.ViewModel_Table.Day3_Schedule.Value = entity; return;
                case 4: this.ViewModel_Table.Day4_Schedule.Value = entity; return;
                case 5: this.ViewModel_Table.Day5_Schedule.Value = entity; return;
                case 6: this.ViewModel_Table.Day6_Schedule.Value = entity; return;
                case 7: this.ViewModel_Table.Day7_Schedule.Value = entity; return;
                case 8: this.ViewModel_Table.Day8_Schedule.Value = entity; return;
                case 9: this.ViewModel_Table.Day9_Schedule.Value = entity; return;
                case 10: this.ViewModel_Table.Day10_Schedule.Value = entity; return;
                case 11: this.ViewModel_Table.Day11_Schedule.Value = entity; return;
                case 12: this.ViewModel_Table.Day12_Schedule.Value = entity; return;
                case 13: this.ViewModel_Table.Day13_Schedule.Value = entity; return;
                case 14: this.ViewModel_Table.Day14_Schedule.Value = entity; return;
                case 15: this.ViewModel_Table.Day15_Schedule.Value = entity; return;
                case 16: this.ViewModel_Table.Day16_Schedule.Value = entity; return;
                case 17: this.ViewModel_Table.Day17_Schedule.Value = entity; return;
                case 18: this.ViewModel_Table.Day18_Schedule.Value = entity; return;
                case 19: this.ViewModel_Table.Day19_Schedule.Value = entity; return;
                case 20: this.ViewModel_Table.Day20_Schedule.Value = entity; return;
                case 21: this.ViewModel_Table.Day21_Schedule.Value = entity; return;
                case 22: this.ViewModel_Table.Day22_Schedule.Value = entity; return;
                case 23: this.ViewModel_Table.Day23_Schedule.Value = entity; return;
                case 24: this.ViewModel_Table.Day24_Schedule.Value = entity; return;
                case 25: this.ViewModel_Table.Day25_Schedule.Value = entity; return;
                case 26: this.ViewModel_Table.Day26_Schedule.Value = entity; return;
                case 27: this.ViewModel_Table.Day27_Schedule.Value = entity; return;
                case 28: this.ViewModel_Table.Day28_Schedule.Value = entity; return;
                case 29: this.ViewModel_Table.Day29_Schedule.Value = entity; return;
                case 30: this.ViewModel_Table.Day30_Schedule.Value = entity; return;
                case 31: this.ViewModel_Table.Day31_Schedule.Value = entity; return;
            }
        }

        /// <summary>
        /// 祝日の取得
        /// </summary>
        /// <param name="date">日付</param>
        private SolidColorBrush GetHoliday(DateTime date)
        {
            var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

            if (holidays.Any() == false)
            {
                return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }

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
            // 勤務時間
            this.ViewModel_Header.WorkingTimeTotal = new TimeSpan();
            this.WorkDaysTotal = 0;

            // 残業時間
            this.ViewModel_Header.OvertimeTotal = new TimeSpan();
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
        /// 入力 - 昼休憩
        /// </summary>
        /// <param name="day">対象日</param>
        private string InputLunchTime(int day)
        {
            var workingPlace = this.SearchWorkingPlace(this.ConvertDayToDate(day));

            if (workingPlace is null)
            {
                return string.Empty;
            }

            return workingPlace.LunchTimeSpan.ToString(@"hh\:mm");
        }

        /// <summary>
        /// 入力 - 届出
        /// </summary>
        /// <param name="day">日</param>
        private string InputNotification(int day)
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

        private TimeSpan WorkingTime_Time;

        public int WorkDaysTotal;

        /// <summary>
        /// 入力 - 勤務時間
        /// </summary>
        /// <param name="day">日</param>
        /// <param name="startTime">始業時間</param>
        /// <param name="endTime">就業時間</param>
        private string InputWorkingTime(int day, DateTime startTime, DateTime endTime)
        {
            var workingPlace = this.SearchWorkingPlace(this.ConvertDayToDate(day));

            if (workingPlace is null)
            {
                return string.Empty;
            }

            this.WorkDaysTotal += 1;

            this.WorkingTime_Time = (endTime - startTime) - workingPlace.LunchTimeSpan;

            if (this.WorkingTime_Time > new TimeSpan(8, 0, 0))
            {
                this.WorkingTime_Time = new TimeSpan(8, 0, 0);
            }

            this.ViewModel_Header.WorkingTimeTotal = this.ViewModel_Header.WorkingTimeTotal.Add(this.WorkingTime_Time);

            return this.WorkingTime_Time.ToString(@"hh\:mm");
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
        private string InputOvertime(int day, DateTime startTime, DateTime endTime)
        {
            var workingPlace = this.SearchWorkingPlace(this.ConvertDayToDate(day));

            if (workingPlace is null)
            {
                return string.Empty;
            }

            var overTime = (endTime - startTime) - workingPlace.LunchTimeSpan - new TimeSpan(8, 0, 0);

            if (overTime.TotalMinutes > 0)
            {
                this.ViewModel_Header.OvertimeTotal = this.ViewModel_Header.OvertimeTotal.Add(overTime);
                return overTime.ToString(@"hh\:mm");
            }

            return "00:00";
        }

        /// <summary>
        /// Input - 備考
        /// </summary>
        /// <param name="day">日</param>
        /// <param name="startTime">始業時間</param>
        /// <param name="endTime">就業時間</param>
        /// <param name="workPlace">勤務場所</param>
        private string InputRemarks(int day, DateTime startTime, DateTime endTime, string workPlace)
        {
            var home = Homes.FetchByDate(this.ConvertDayToDate(day));

            if (home is null)
            {
                return string.Empty;
            }

            var isWorkAtHome = ((endTime.Hour - startTime.Hour) >= 8 && workPlace == home.Address_Google);

            return isWorkAtHome ? "在宅所定時間以上" : string.Empty;
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
