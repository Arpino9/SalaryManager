using Reactive.Bindings;
using SalaryManager.WPF.Models;
using System.ComponentModel;
using System.Windows.Media;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤務表
    /// </summary>
    public class ViewModel_WorkSchedule_Table : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel_WorkSchedule_Table()
        {
            this.Model.ViewModel_Table = this;

            this.Model.Initialize_Table();
        }

        /// <summary>
        /// Model - 勤務表
        /// </summary>
        private Model_WorkSchedule_Table Model 
            = Model_WorkSchedule_Table.GetInstance();

        #region 1日

        /// <summary> 1日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day1_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 1日 - Text </summary>
        public ReactiveProperty<string> Day1_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day1_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day1_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day1_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day1_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day1_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day1_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day1_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day1_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 1日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day1_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 2日

        /// <summary> 2日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day2_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 2日 - Text </summary>
        public ReactiveProperty<string> Day2_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day2_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day2_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day2_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day2_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day2_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day2_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day2_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day2_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 2日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day2_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 3日

        /// <summary> 3日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day3_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 3日 - Text </summary>
        public ReactiveProperty<string> Day3_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day3_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day3_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day3_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day3_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day3_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day3_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day3_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day3_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 3日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day3_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 4日

        /// <summary> 4日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day4_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 4日 - Text </summary>
        public ReactiveProperty<string> Day4_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day4_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day4_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day4_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day4_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day4_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day4_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day4_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day4_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 4日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day4_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 5日

        /// <summary> 5日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day5_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 5日 - Text </summary>
        public ReactiveProperty<string> Day5_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day5_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day5_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day5_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day5_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day5_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day5_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day5_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day5_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 5日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day5_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 6日

        /// <summary> 6日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day6_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 6日 - Text </summary>
        public ReactiveProperty<string> Day6_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day6_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day6_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day6_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day6_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day6_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day6_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day6_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day6_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 6日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day6_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 7日

        /// <summary> 7日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day7_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 7日 - Text </summary>
        public ReactiveProperty<string> Day7_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day7_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day7_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day7_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day7_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day7_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day7_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day7_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day7_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 7日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day7_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 8日

        /// <summary> 8日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day8_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 8日 - Text </summary>
        public ReactiveProperty<string> Day8_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day8_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day8_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day8_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day8_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day8_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day8_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day8_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day8_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 8日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day8_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 9日

        /// <summary> 9日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day9_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 9日 - Text </summary>
        public ReactiveProperty<string> Day9_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day9_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day9_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day9_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day9_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day9_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day9_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day9_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day9_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 9日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day9_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 10日

        /// <summary> 10日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day10_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 10日 - Text </summary>
        public ReactiveProperty<string> Day10_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day10_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day10_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day10_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day10_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day10_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day10_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day10_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day10_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 10日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day10_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 11日

        /// <summary> 11日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day11_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 11日 - Text </summary>
        public ReactiveProperty<string> Day11_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day11_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day11_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day11_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day11_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day11_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day11_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day11_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day11_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 11日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day11_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 12日

        /// <summary> 12日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day12_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 12日 - Text </summary>
        public ReactiveProperty<string> Day12_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day12_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day12_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day12_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day12_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day12_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day12_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day12_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day12_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 12日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day12_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 13日

        /// <summary> 13日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day13_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 13日 - Text </summary>
        public ReactiveProperty<string> Day13_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day13_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day13_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day13_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day13_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day13_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day13_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day13_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day13_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 13日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day13_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 14日

        /// <summary> 14日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day14_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 14日 - Text </summary>
        public ReactiveProperty<string> Day14_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day14_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day14_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day14_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day14_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day14_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day14_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day14_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day14_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 14日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day14_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 15日

        /// <summary> 15日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day15_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 15日 - Text </summary>
        public ReactiveProperty<string> Day15_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day15_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day15_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day15_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day15_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day15_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day15_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day15_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day15_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 15日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day15_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 16日

        /// <summary> 16日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day16_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 16日 - Text </summary>
        public ReactiveProperty<string> Day16_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day16_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day16_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day16_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day16_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day16_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day16_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day16_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day16_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 16日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day16_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 17日

        /// <summary> 17日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day17_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 17日 - Text </summary>
        public ReactiveProperty<string> Day17_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day17_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day17_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day17_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day17_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day17_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day17_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day17_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day17_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 17日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day17_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 18日

        /// <summary> 18日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day18_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 18日 - Text </summary>
        public ReactiveProperty<string> Day18_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day18_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day18_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day18_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day18_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day18_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day18_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day18_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day18_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 18日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day18_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 19日

        /// <summary> 19日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day19_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 19日 - Text </summary>
        public ReactiveProperty<string> Day19_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day19_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day19_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day19_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day19_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day19_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day19_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day19_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day19_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 19日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day19_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 20日

        /// <summary> 20日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day20_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 20日 - Text </summary>
        public ReactiveProperty<string> Day20_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day20_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day20_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day20_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day20_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day20_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day20_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day20_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day20_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 20日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day20_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 21日

        /// <summary> 21日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day21_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 21日 - Text </summary>
        public ReactiveProperty<string> Day21_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day21_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day21_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day21_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day21_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day21_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day21_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day21_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day21_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 21日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day21_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 22日

        /// <summary> 22日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day22_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 22日 - Text </summary>
        public ReactiveProperty<string> Day22_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day22_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day22_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day22_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day22_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day22_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day22_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day22_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day22_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 22日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day22_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 23日

        /// <summary> 23日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day23_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 23日 - Text </summary>
        public ReactiveProperty<string> Day23_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day23_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day23_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day23_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day23_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day23_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day23_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day23_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day23_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 23日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day23_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 24日

        /// <summary> 24日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day24_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 24日 - Text </summary>
        public ReactiveProperty<string> Day24_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day24_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day24_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day24_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day24_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day24_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day24_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day24_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day24_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 24日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day24_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 25日

        /// <summary> 25日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day25_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 25日 - Text </summary>
        public ReactiveProperty<string> Day25_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day25_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day25_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day25_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day25_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day25_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day25_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day25_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day25_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 25日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day25_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 26日

        /// <summary> 26日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day26_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 26日 - Text </summary>
        public ReactiveProperty<string> Day26_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day26_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day26_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day26_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day26_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day26_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day26_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day26_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day26_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 26日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day26_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 27日

        /// <summary> 27日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day27_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 27日 - Text </summary>
        public ReactiveProperty<string> Day27_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day27_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day27_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day27_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day27_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day27_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day27_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day27_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day27_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 27日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day27_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 28日

        /// <summary> 28日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day28_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 28日 - Text </summary>
        public ReactiveProperty<string> Day28_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day28_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day28_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day28_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day28_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day28_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day28_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day28_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day28_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 28日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day28_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 29日

        /// <summary> 29日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day29_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 29日 - Text </summary>
        public ReactiveProperty<string> Day29_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day29_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day29_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day29_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day29_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day29_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day29_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day29_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day29_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 29日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day29_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 30日

        /// <summary> 30日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day30_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 30日 - Text </summary>
        public ReactiveProperty<string> Day30_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day30_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day30_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day30_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day30_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day30_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day30_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day30_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day30_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 30日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day30_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 31日

        /// <summary> 31日 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Day31_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 31日 - Text </summary>
        public ReactiveProperty<string> Day31_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 始業 - Text </summary>
        public ReactiveProperty<string> Day31_StartTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 終業 - Text </summary>
        public ReactiveProperty<string> Day31_EndTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 昼休憩 - Text </summary>
        public ReactiveProperty<string> Day31_LunchTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 届出 - Text </summary>
        public ReactiveProperty<string> Day31_Notification_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 勤務時間 - Text </summary>
        public ReactiveProperty<string> Day31_WorkingTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 残業時間 - Text </summary>
        public ReactiveProperty<string> Day31_Overtime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 深夜時間 - Text </summary>
        public ReactiveProperty<string> Day31_MidnightTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 欠課時間 - Text </summary>
        public ReactiveProperty<string> Day31_AbsentedTime_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 31日 - 備考 - Text </summary>
        public ReactiveProperty<string> Day31_Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

    }
}
