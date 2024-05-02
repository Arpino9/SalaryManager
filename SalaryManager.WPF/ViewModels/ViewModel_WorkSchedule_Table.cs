using SalaryManager.WPF.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤務表
    /// </summary>
    public class ViewModel_WorkSchedule_Table : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_WorkSchedule_Table()
        {
            this.Model.ViewModel_Table = this;

            this.Model.Initialize_Table();
        }

        /// <summary>
        /// Model - 勤務表
        /// </summary>
        private Model_WorkSchedule_Table Model = Model_WorkSchedule_Table.GetInstance();

        #region (M月)1日

        private System.Windows.Media.Brush _background_1;

        /// <summary>
        /// (M月)1日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_1
        {
            get => this._background_1;
            set
            {
                this._background_1 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1;

        /// <summary>
        /// (M月)1日
        /// </summary>
        public string Day_1
        {
            get => this._day_1;
            set
            {
                this._day_1 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_StartTime;

        /// <summary>
        /// (M月)1日 - 始業
        /// </summary>
        public string Day_1_StartTime
        {
            get => this._day_1_StartTime;
            set
            {
                this._day_1_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_EndTime;

        /// <summary>
        /// (M月)1日 - 終業
        /// </summary>
        public string Day_1_EndTime
        {
            get => this._day_1_EndTime;
            set
            {
                this._day_1_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_LunchTime;

        /// <summary>
        /// (M月)1日 - 昼休憩
        /// </summary>
        public string Day_1_LunchTime
        {
            get => this._day_1_LunchTime;
            set
            {
                this._day_1_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_Notification;

        /// <summary>
        /// (M月)1日 - 届出
        /// </summary>
        public string Day_1_Notification
        {
            get => this._day_1_Notification;
            set
            {
                this._day_1_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_WorkingTime;

        /// <summary>
        /// (M月)1日 - 勤務時間
        /// </summary>
        public string Day_1_WorkingTime
        {
            get => this._day_1_WorkingTime;
            set
            {
                this._day_1_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_Overtime;

        /// <summary>
        /// (M月)1日 - 残業時間
        /// </summary>
        public string Day_1_Overtime
        {
            get => this._day_1_Overtime;
            set
            {
                this._day_1_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_MidnightTime;

        /// <summary>
        /// (M月)1日 - 深夜時間
        /// </summary>
        public string Day_1_MidnightTime
        {
            get => this._day_1_MidnightTime;
            set
            {
                this._day_1_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_AbsentedTime;

        /// <summary>
        /// (M月)1日 - 欠課時間
        /// </summary>
        public string Day_1_AbsentedTime
        {
            get => this._day_1_AbsentedTime;
            set
            {
                this._day_1_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_1_Remarks;

        /// <summary>
        /// (M月)1日 - 備考
        /// </summary>
        public string Day_1_Remarks
        {
            get => this._day_1_Remarks;
            set
            {
                this._day_1_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)2日

        private System.Windows.Media.Brush _background_2;

        /// <summary>
        /// (M月)1日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_2
        {
            get => this._background_2;
            set
            {
                this._background_2 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2;

        /// <summary>
        /// (M月)2日
        /// </summary>
        public string Day_2
        {
            get => this._day_2;
            set
            {
                this._day_2 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_StartTime;

        /// <summary>
        /// (M月)2日 - 始業
        /// </summary>
        public string Day_2_StartTime
        {
            get => this._day_2_StartTime;
            set
            {
                this._day_2_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_EndTime;

        /// <summary>
        /// (M月)2日 - 終業
        /// </summary>
        public string Day_2_EndTime
        {
            get => this._day_2_EndTime;
            set
            {
                this._day_2_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_LunchTime;

        /// <summary>
        /// (M月)2日 - 昼休憩
        /// </summary>
        public string Day_2_LunchTime
        {
            get => this._day_2_LunchTime;
            set
            {
                this._day_2_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_Notification;

        /// <summary>
        /// (M月)2日 - 届出
        /// </summary>
        public string Day_2_Notification
        {
            get => this._day_2_Notification;
            set
            {
                this._day_2_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_WorkingTime;

        /// <summary>
        /// (M月)2日 - 勤務時間
        /// </summary>
        public string Day_2_WorkingTime
        {
            get => this._day_2_WorkingTime;
            set
            {
                this._day_2_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_Overtime;

        /// <summary>
        /// (M月)2日 - 残業時間
        /// </summary>
        public string Day_2_Overtime
        {
            get => this._day_2_Overtime;
            set
            {
                this._day_2_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_MidnightTime;

        /// <summary>
        /// (M月)2日 - 深夜時間
        /// </summary>
        public string Day_2_MidnightTime
        {
            get => this._day_2_MidnightTime;
            set
            {
                this._day_2_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_AbsentedTime;

        /// <summary>
        /// (M月)2日 - 欠課時間
        /// </summary>
        public string Day_2_AbsentedTime
        {
            get => this._day_2_AbsentedTime;
            set
            {
                this._day_2_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_2_Remarks;

        /// <summary>
        /// (M月)2日 - 備考
        /// </summary>
        public string Day_2_Remarks
        {
            get => this._day_2_Remarks;
            set
            {
                this._day_2_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)3日

        private System.Windows.Media.Brush _background_3;

        /// <summary>
        /// (M月)3日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_3
        {
            get => this._background_3;
            set
            {
                this._background_3 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3;

        /// <summary>
        /// (M月)3日
        /// </summary>
        public string Day_3
        {
            get => this._day_3;
            set
            {
                this._day_3 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_StartTime;

        /// <summary>
        /// (M月)3日 - 始業
        /// </summary>
        public string Day_3_StartTime
        {
            get => this._day_3_StartTime;
            set
            {
                this._day_3_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_EndTime;

        /// <summary>
        /// (M月)3日 - 終業
        /// </summary>
        public string Day_3_EndTime
        {
            get => this._day_3_EndTime;
            set
            {
                this._day_3_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_LunchTime;

        /// <summary>
        /// (M月)3日 - 昼休憩
        /// </summary>
        public string Day_3_LunchTime
        {
            get => this._day_3_LunchTime;
            set
            {
                this._day_3_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_Notification;

        /// <summary>
        /// (M月)3日 - 届出
        /// </summary>
        public string Day_3_Notification
        {
            get => this._day_3_Notification;
            set
            {
                this._day_3_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_WorkingTime;

        /// <summary>
        /// (M月)3日 - 勤務時間
        /// </summary>
        public string Day_3_WorkingTime
        {
            get => this._day_3_WorkingTime;
            set
            {
                this._day_3_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_Overtime;

        /// <summary>
        /// (M月)3日 - 残業時間
        /// </summary>
        public string Day_3_Overtime
        {
            get => this._day_3_Overtime;
            set
            {
                this._day_3_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_MidnightTime;

        /// <summary>
        /// (M月)3日 - 深夜時間
        /// </summary>
        public string Day_3_MidnightTime
        {
            get => this._day_3_MidnightTime;
            set
            {
                this._day_3_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_AbsentedTime;

        /// <summary>
        /// (M月)3日 - 欠課時間
        /// </summary>
        public string Day_3_AbsentedTime
        {
            get => this._day_3_AbsentedTime;
            set
            {
                this._day_3_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_3_Remarks;

        /// <summary>
        /// (M月)3日 - 備考
        /// </summary>
        public string Day_3_Remarks
        {
            get => this._day_3_Remarks;
            set
            {
                this._day_3_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)4日

        private System.Windows.Media.Brush _background_4;

        /// <summary>
        /// (M月)4日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_4
        {
            get => this._background_4;
            set
            {
                this._background_4 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4;

        /// <summary>
        /// (M月)4日
        /// </summary>
        public string Day_4
        {
            get => this._day_4;
            set
            {
                this._day_4 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_StartTime;

        /// <summary>
        /// (M月)4日 - 始業
        /// </summary>
        public string Day_4_StartTime
        {
            get => this._day_4_StartTime;
            set
            {
                this._day_4_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_EndTime;

        /// <summary>
        /// (M月)4日 - 終業
        /// </summary>
        public string Day_4_EndTime
        {
            get => this._day_4_EndTime;
            set
            {
                this._day_4_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_LunchTime;

        /// <summary>
        /// (M月)4日 - 昼休憩
        /// </summary>
        public string Day_4_LunchTime
        {
            get => this._day_4_LunchTime;
            set
            {
                this._day_4_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_Notification;

        /// <summary>
        /// (M月)4日 - 届出
        /// </summary>
        public string Day_4_Notification
        {
            get => this._day_4_Notification;
            set
            {
                this._day_4_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_WorkingTime;

        /// <summary>
        /// (M月)4日 - 勤務時間
        /// </summary>
        public string Day_4_WorkingTime
        {
            get => this._day_4_WorkingTime;
            set
            {
                this._day_4_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_Overtime;

        /// <summary>
        /// (M月)4日 - 残業時間
        /// </summary>
        public string Day_4_Overtime
        {
            get => this._day_4_Overtime;
            set
            {
                this._day_4_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_MidnightTime;

        /// <summary>
        /// (M月)4日 - 深夜時間
        /// </summary>
        public string Day_4_MidnightTime
        {
            get => this._day_4_MidnightTime;
            set
            {
                this._day_4_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_AbsentedTime;

        /// <summary>
        /// (M月)4日 - 欠課時間
        /// </summary>
        public string Day_4_AbsentedTime
        {
            get => this._day_4_AbsentedTime;
            set
            {
                this._day_4_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_4_Remarks;

        /// <summary>
        /// (M月)4日 - 備考
        /// </summary>
        public string Day_4_Remarks
        {
            get => this._day_4_Remarks;
            set
            {
                this._day_4_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)5日

        private System.Windows.Media.Brush _background_5;

        /// <summary>
        /// (M月)5日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_5
        {
            get => this._background_5;
            set
            {
                this._background_5 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5;

        /// <summary>
        /// (M月)5日
        /// </summary>
        public string Day_5
        {
            get => this._day_5;
            set
            {
                this._day_5 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_StartTime;

        /// <summary>
        /// (M月)5日 - 始業
        /// </summary>
        public string Day_5_StartTime
        {
            get => this._day_5_StartTime;
            set
            {
                this._day_5_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_EndTime;

        /// <summary>
        /// (M月)5日 - 終業
        /// </summary>
        public string Day_5_EndTime
        {
            get => this._day_5_EndTime;
            set
            {
                this._day_5_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_LunchTime;

        /// <summary>
        /// (M月)5日 - 昼休憩
        /// </summary>
        public string Day_5_LunchTime
        {
            get => this._day_5_LunchTime;
            set
            {
                this._day_5_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_Notification;

        /// <summary>
        /// (M月)5日 - 届出
        /// </summary>
        public string Day_5_Notification
        {
            get => this._day_5_Notification;
            set
            {
                this._day_5_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_WorkingTime;

        /// <summary>
        /// (M月)5日 - 勤務時間
        /// </summary>
        public string Day_5_WorkingTime
        {
            get => this._day_5_WorkingTime;
            set
            {
                this._day_5_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_Overtime;

        /// <summary>
        /// (M月)5日 - 残業時間
        /// </summary>
        public string Day_5_Overtime
        {
            get => this._day_5_Overtime;
            set
            {
                this._day_5_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_MidnightTime;

        /// <summary>
        /// (M月)5日 - 深夜時間
        /// </summary>
        public string Day_5_MidnightTime
        {
            get => this._day_5_MidnightTime;
            set
            {
                this._day_5_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_AbsentedTime;

        /// <summary>
        /// (M月)5日 - 欠課時間
        /// </summary>
        public string Day_5_AbsentedTime
        {
            get => this._day_5_AbsentedTime;
            set
            {
                this._day_5_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_5_Remarks;

        /// <summary>
        /// (M月)5日 - 備考
        /// </summary>
        public string Day_5_Remarks
        {
            get => this._day_5_Remarks;
            set
            {
                this._day_5_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)6日

        private System.Windows.Media.Brush _background_6;

        /// <summary>
        /// (M月)6日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_6
        {
            get => this._background_6;
            set
            {
                this._background_6 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6;

        /// <summary>
        /// (M月)6日
        /// </summary>
        public string Day_6
        {
            get => this._day_6;
            set
            {
                this._day_6 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_StartTime;

        /// <summary>
        /// (M月)6日 - 始業
        /// </summary>
        public string Day_6_StartTime
        {
            get => this._day_6_StartTime;
            set
            {
                this._day_6_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_EndTime;

        /// <summary>
        /// (M月)6日 - 終業
        /// </summary>
        public string Day_6_EndTime
        {
            get => this._day_6_EndTime;
            set
            {
                this._day_6_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_LunchTime;

        /// <summary>
        /// (M月)6日 - 昼休憩
        /// </summary>
        public string Day_6_LunchTime
        {
            get => this._day_6_LunchTime;
            set
            {
                this._day_6_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_Notification;

        /// <summary>
        /// (M月)6日 - 届出
        /// </summary>
        public string Day_6_Notification
        {
            get => this._day_6_Notification;
            set
            {
                this._day_6_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_WorkingTime;

        /// <summary>
        /// (M月)6日 - 勤務時間
        /// </summary>
        public string Day_6_WorkingTime
        {
            get => this._day_6_WorkingTime;
            set
            {
                this._day_6_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_Overtime;

        /// <summary>
        /// (M月)6日 - 残業時間
        /// </summary>
        public string Day_6_Overtime
        {
            get => this._day_6_Overtime;
            set
            {
                this._day_6_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_MidnightTime;

        /// <summary>
        /// (M月)6日 - 深夜時間
        /// </summary>
        public string Day_6_MidnightTime
        {
            get => this._day_6_MidnightTime;
            set
            {
                this._day_6_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_AbsentedTime;

        /// <summary>
        /// (M月)6日 - 欠課時間
        /// </summary>
        public string Day_6_AbsentedTime
        {
            get => this._day_6_AbsentedTime;
            set
            {
                this._day_6_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_6_Remarks;

        /// <summary>
        /// (M月)6日 - 備考
        /// </summary>
        public string Day_6_Remarks
        {
            get => this._day_6_Remarks;
            set
            {
                this._day_6_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)7日

        private System.Windows.Media.Brush _background_7;

        /// <summary>
        /// (M月)7日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_7
        {
            get => this._background_7;
            set
            {
                this._background_7 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7;

        /// <summary>
        /// (M月)7日
        /// </summary>
        public string Day_7
        {
            get => this._day_7;
            set
            {
                this._day_7 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_StartTime;

        /// <summary>
        /// (M月)7日 - 始業
        /// </summary>
        public string Day_7_StartTime
        {
            get => this._day_7_StartTime;
            set
            {
                this._day_7_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_EndTime;

        /// <summary>
        /// (M月)7日 - 終業
        /// </summary>
        public string Day_7_EndTime
        {
            get => this._day_7_EndTime;
            set
            {
                this._day_7_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_LunchTime;

        /// <summary>
        /// (M月)7日 - 昼休憩
        /// </summary>
        public string Day_7_LunchTime
        {
            get => this._day_7_LunchTime;
            set
            {
                this._day_7_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_Notification;

        /// <summary>
        /// (M月)7日 - 届出
        /// </summary>
        public string Day_7_Notification
        {
            get => this._day_7_Notification;
            set
            {
                this._day_7_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_WorkingTime;

        /// <summary>
        /// (M月)7日 - 勤務時間
        /// </summary>
        public string Day_7_WorkingTime
        {
            get => this._day_7_WorkingTime;
            set
            {
                this._day_7_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_Overtime;

        /// <summary>
        /// (M月)7日 - 残業時間
        /// </summary>
        public string Day_7_Overtime
        {
            get => this._day_7_Overtime;
            set
            {
                this._day_7_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_MidnightTime;

        /// <summary>
        /// (M月)7日 - 深夜時間
        /// </summary>
        public string Day_7_MidnightTime
        {
            get => this._day_7_MidnightTime;
            set
            {
                this._day_7_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_AbsentedTime;

        /// <summary>
        /// (M月)7日 - 欠課時間
        /// </summary>
        public string Day_7_AbsentedTime
        {
            get => this._day_7_AbsentedTime;
            set
            {
                this._day_7_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_7_Remarks;

        /// <summary>
        /// (M月)7日 - 備考
        /// </summary>
        public string Day_7_Remarks
        {
            get => this._day_7_Remarks;
            set
            {
                this._day_7_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)8日

        private System.Windows.Media.Brush _background_8;

        /// <summary>
        /// (M月)8日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_8
        {
            get => this._background_8;
            set
            {
                this._background_8 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8;

        /// <summary>
        /// (M月)8日
        /// </summary>
        public string Day_8
        {
            get => this._day_8;
            set
            {
                this._day_8 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_StartTime;

        /// <summary>
        /// (M月)8日 - 始業
        /// </summary>
        public string Day_8_StartTime
        {
            get => this._day_8_StartTime;
            set
            {
                this._day_8_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_EndTime;

        /// <summary>
        /// (M月)8日 - 終業
        /// </summary>
        public string Day_8_EndTime
        {
            get => this._day_8_EndTime;
            set
            {
                this._day_8_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_LunchTime;

        /// <summary>
        /// (M月)8日 - 昼休憩
        /// </summary>
        public string Day_8_LunchTime
        {
            get => this._day_8_LunchTime;
            set
            {
                this._day_8_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_Notification;
        
        /// <summary>
        /// (M月)8日 - 届出
        /// </summary>
        public string Day_8_Notification
        {
            get => this._day_8_Notification;
            set
            {
                this._day_8_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_WorkingTime;

        /// <summary>
        /// (M月)8日 - 勤務時間
        /// </summary>
        public string Day_8_WorkingTime
        {
            get => this._day_8_WorkingTime;
            set
            {
                this._day_8_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_Overtime;

        /// <summary>
        /// (M月)8日 - 残業時間
        /// </summary>
        public string Day_8_Overtime
        {
            get => this._day_8_Overtime;
            set
            {
                this._day_8_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_MidnightTime;

        /// <summary>
        /// (M月)8日 - 深夜時間
        /// </summary>
        public string Day_8_MidnightTime
        {
            get => this._day_8_MidnightTime;
            set
            {
                this._day_8_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_AbsentedTime;

        /// <summary>
        /// (M月)8日 - 欠課時間
        /// </summary>
        public string Day_8_AbsentedTime
        {
            get => this._day_8_AbsentedTime;
            set
            {
                this._day_8_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_8_Remarks;

        /// <summary>
        /// (M月)8日 - 備考
        /// </summary>
        public string Day_8_Remarks
        {
            get => this._day_8_Remarks;
            set
            {
                this._day_8_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)9日

        private System.Windows.Media.Brush _background_9;

        /// <summary>
        /// (M月)9日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_9
        {
            get => this._background_9;
            set
            {
                this._background_9 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9;

        /// <summary>
        /// (M月)9日
        /// </summary>
        public string Day_9
        {
            get => this._day_9;
            set
            {
                this._day_9 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_StartTime;

        /// <summary>
        /// (M月)9日 - 始業
        /// </summary>
        public string Day_9_StartTime
        {
            get => this._day_9_StartTime;
            set
            {
                this._day_9_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_EndTime;

        /// <summary>
        /// (M月)9日 - 終業
        /// </summary>
        public string Day_9_EndTime
        {
            get => this._day_9_EndTime;
            set
            {
                this._day_9_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_LunchTime;

        /// <summary>
        /// (M月)9日 - 昼休憩
        /// </summary>
        public string Day_9_LunchTime
        {
            get => this._day_9_LunchTime;
            set
            {
                this._day_9_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_Notification;

        /// <summary>
        /// (M月)9日 - 届出
        /// </summary>
        public string Day_9_Notification
        {
            get => this._day_9_Notification;
            set
            {
                this._day_9_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_WorkingTime;

        /// <summary>
        /// (M月)9日 - 勤務時間
        /// </summary>
        public string Day_9_WorkingTime
        {
            get => this._day_9_WorkingTime;
            set
            {
                this._day_9_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_Overtime;

        /// <summary>
        /// (M月)9日 - 残業時間
        /// </summary>
        public string Day_9_Overtime
        {
            get => this._day_9_Overtime;
            set
            {
                this._day_9_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_MidnightTime;

        /// <summary>
        /// (M月)9日 - 深夜時間
        /// </summary>
        public string Day_9_MidnightTime
        {
            get => this._day_9_MidnightTime;
            set
            {
                this._day_9_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_AbsentedTime;

        /// <summary>
        /// (M月)9日 - 欠課時間
        /// </summary>
        public string Day_9_AbsentedTime
        {
            get => this._day_9_AbsentedTime;
            set
            {
                this._day_9_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_9_Remarks;

        /// <summary>
        /// (M月)9日 - 備考
        /// </summary>
        public string Day_9_Remarks
        {
            get => this._day_9_Remarks;
            set
            {
                this._day_9_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)10日

        private System.Windows.Media.Brush _background_10;

        /// <summary>
        /// (M月)9日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_10
        {
            get => this._background_10;
            set
            {
                this._background_10 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10;

        /// <summary>
        /// (M月)10日
        /// </summary>
        public string Day_10
        {
            get => this._day_10;
            set
            {
                this._day_10 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_StartTime;

        /// <summary>
        /// (M月)10日 - 始業
        /// </summary>
        public string Day_10_StartTime
        {
            get => this._day_10_StartTime;
            set
            {
                this._day_10_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_EndTime;

        /// <summary>
        /// (M月)10日 - 終業
        /// </summary>
        public string Day_10_EndTime
        {
            get => this._day_10_EndTime;
            set
            {
                this._day_10_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_LunchTime;

        /// <summary>
        /// (M月)10日 - 昼休憩
        /// </summary>
        public string Day_10_LunchTime
        {
            get => this._day_10_LunchTime;
            set
            {
                this._day_10_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_Notification;

        /// <summary>
        /// (M月)10日 - 届出
        /// </summary>
        public string Day_10_Notification
        {
            get => this._day_10_Notification;
            set
            {
                this._day_10_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_WorkingTime;

        /// <summary>
        /// (M月)10日 - 勤務時間
        /// </summary>
        public string Day_10_WorkingTime
        {
            get => this._day_10_WorkingTime;
            set
            {
                this._day_10_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_Overtime;

        /// <summary>
        /// (M月)10日 - 残業時間
        /// </summary>
        public string Day_10_Overtime
        {
            get => this._day_10_Overtime;
            set
            {
                this._day_10_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_MidnightTime;

        /// <summary>
        /// (M月)10日 - 深夜時間
        /// </summary>
        public string Day_10_MidnightTime
        {
            get => this._day_10_MidnightTime;
            set
            {
                this._day_10_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_AbsentedTime;

        /// <summary>
        /// (M月)10日 - 欠課時間
        /// </summary>
        public string Day_10_AbsentedTime
        {
            get => this._day_10_AbsentedTime;
            set
            {
                this._day_10_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_10_Remarks;

        /// <summary>
        /// (M月)10日 - 備考
        /// </summary>
        public string Day_10_Remarks
        {
            get => this._day_10_Remarks;
            set
            {
                this._day_10_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)11日

        private System.Windows.Media.Brush _background_11;

        /// <summary>
        /// (M月)11日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_11
        {
            get => this._background_11;
            set
            {
                this._background_11 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11;

        /// <summary>
        /// (M月)11日
        /// </summary>
        public string Day_11
        {
            get => this._day_11;
            set
            {
                this._day_11 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_StartTime;

        /// <summary>
        /// (M月)11日 - 始業
        /// </summary>
        public string Day_11_StartTime
        {
            get => this._day_11_StartTime;
            set
            {
                this._day_11_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_EndTime;

        /// <summary>
        /// (M月)11日 - 終業
        /// </summary>
        public string Day_11_EndTime
        {
            get => this._day_11_EndTime;
            set
            {
                this._day_11_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_LunchTime;

        /// <summary>
        /// (M月)11日 - 昼休憩
        /// </summary>
        public string Day_11_LunchTime
        {
            get => this._day_11_LunchTime;
            set
            {
                this._day_11_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_Notification;

        /// <summary>
        /// (M月)11日 - 届出
        /// </summary>
        public string Day_11_Notification
        {
            get => this._day_11_Notification;
            set
            {
                this._day_11_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_WorkingTime;

        /// <summary>
        /// (M月)11日 - 勤務時間
        /// </summary>
        public string Day_11_WorkingTime
        {
            get => this._day_11_WorkingTime;
            set
            {
                this._day_11_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_Overtime;

        /// <summary>
        /// (M月)11日 - 残業時間
        /// </summary>
        public string Day_11_Overtime
        {
            get => this._day_11_Overtime;
            set
            {
                this._day_11_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_MidnightTime;

        /// <summary>
        /// (M月)11日 - 深夜時間
        /// </summary>
        public string Day_11_MidnightTime
        {
            get => this._day_11_MidnightTime;
            set
            {
                this._day_11_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_AbsentedTime;

        /// <summary>
        /// (M月)11日 - 欠課時間
        /// </summary>
        public string Day_11_AbsentedTime
        {
            get => this._day_11_AbsentedTime;
            set
            {
                this._day_11_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_11_Remarks;

        /// <summary>
        /// (M月)11日 - 備考
        /// </summary>
        public string Day_11_Remarks
        {
            get => this._day_11_Remarks;
            set
            {
                this._day_11_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)12日

        private System.Windows.Media.Brush _background_12;

        /// <summary>
        /// (M月)12日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_12
        {
            get => this._background_12;
            set
            {
                this._background_12 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12;

        /// <summary>
        /// (M月)12日
        /// </summary>
        public string Day_12
        {
            get => this._day_12;
            set
            {
                this._day_12 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_StartTime;

        /// <summary>
        /// (M月)12日 - 始業
        /// </summary>
        public string Day_12_StartTime
        {
            get => this._day_12_StartTime;
            set
            {
                this._day_12_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_EndTime;

        /// <summary>
        /// (M月)12日 - 終業
        /// </summary>
        public string Day_12_EndTime
        {
            get => this._day_12_EndTime;
            set
            {
                this._day_12_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_LunchTime;

        /// <summary>
        /// (M月)12日 - 昼休憩
        /// </summary>
        public string Day_12_LunchTime
        {
            get => this._day_12_LunchTime;
            set
            {
                this._day_12_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_Notification;

        /// <summary>
        /// (M月)12日 - 届出
        /// </summary>
        public string Day_12_Notification
        {
            get => this._day_12_Notification;
            set
            {
                this._day_12_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_WorkingTime;

        /// <summary>
        /// (M月)12日 - 勤務時間
        /// </summary>
        public string Day_12_WorkingTime
        {
            get => this._day_12_WorkingTime;
            set
            {
                this._day_12_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_Overtime;

        /// <summary>
        /// (M月)12日 - 残業時間
        /// </summary>
        public string Day_12_Overtime
        {
            get => this._day_12_Overtime;
            set
            {
                this._day_12_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_MidnightTime;

        /// <summary>
        /// (M月)12日 - 深夜時間
        /// </summary>
        public string Day_12_MidnightTime
        {
            get => this._day_12_MidnightTime;
            set
            {
                this._day_12_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_AbsentedTime;

        /// <summary>
        /// (M月)12日 - 欠課時間
        /// </summary>
        public string Day_12_AbsentedTime
        {
            get => this._day_12_AbsentedTime;
            set
            {
                this._day_12_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_12_Remarks;

        /// <summary>
        /// (M月)12日 - 備考
        /// </summary>
        public string Day_12_Remarks
        {
            get => this._day_12_Remarks;
            set
            {
                this._day_12_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)13日

        private System.Windows.Media.Brush _background_13;

        /// <summary>
        /// (M月)13日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_13
        {
            get => this._background_13;
            set
            {
                this._background_13 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13;

        /// <summary>
        /// (M月)13日
        /// </summary>
        public string Day_13
        {
            get => this._day_13;
            set
            {
                this._day_13 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_StartTime;

        /// <summary>
        /// (M月)13日 - 始業
        /// </summary>
        public string Day_13_StartTime
        {
            get => this._day_13_StartTime;
            set
            {
                this._day_13_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_EndTime;

        /// <summary>
        /// (M月)12日 - 終業
        /// </summary>
        public string Day_13_EndTime
        {
            get => this._day_13_EndTime;
            set
            {
                this._day_13_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_LunchTime;

        /// <summary>
        /// (M月)13日 - 昼休憩
        /// </summary>
        public string Day_13_LunchTime
        {
            get => this._day_13_LunchTime;
            set
            {
                this._day_13_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_Notification;

        /// <summary>
        /// (M月)13日 - 届出
        /// </summary>
        public string Day_13_Notification
        {
            get => this._day_13_Notification;
            set
            {
                this._day_13_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_WorkingTime;

        /// <summary>
        /// (M月)13日 - 勤務時間
        /// </summary>
        public string Day_13_WorkingTime
        {
            get => this._day_13_WorkingTime;
            set
            {
                this._day_13_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_Overtime;

        /// <summary>
        /// (M月)13日 - 残業時間
        /// </summary>
        public string Day_13_Overtime
        {
            get => this._day_13_Overtime;
            set
            {
                this._day_13_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_MidnightTime;

        /// <summary>
        /// (M月)13日 - 深夜時間
        /// </summary>
        public string Day_13_MidnightTime
        {
            get => this._day_13_MidnightTime;
            set
            {
                this._day_13_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_AbsentedTime;

        /// <summary>
        /// (M月)13日 - 欠課時間
        /// </summary>
        public string Day_13_AbsentedTime
        {
            get => this._day_13_AbsentedTime;
            set
            {
                this._day_13_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_13_Remarks;

        /// <summary>
        /// (M月)13日 - 備考
        /// </summary>
        public string Day_13_Remarks
        {
            get => this._day_13_Remarks;
            set
            {
                this._day_13_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)14日

        private System.Windows.Media.Brush _background_14;

        /// <summary>
        /// (M月)14日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_14
        {
            get => this._background_14;
            set
            {
                this._background_14 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14;

        /// <summary>
        /// (M月)14日
        /// </summary>
        public string Day_14
        {
            get => this._day_14;
            set
            {
                this._day_14 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_StartTime;

        /// <summary>
        /// (M月)14日 - 始業
        /// </summary>
        public string Day_14_StartTime
        {
            get => this._day_14_StartTime;
            set
            {
                this._day_14_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_EndTime;

        /// <summary>
        /// (M月)14日 - 終業
        /// </summary>
        public string Day_14_EndTime
        {
            get => this._day_14_EndTime;
            set
            {
                this._day_14_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_LunchTime;

        /// <summary>
        /// (M月)14日 - 昼休憩
        /// </summary>
        public string Day_14_LunchTime
        {
            get => this._day_14_LunchTime;
            set
            {
                this._day_14_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_Notification;

        /// <summary>
        /// (M月)14日 - 届出
        /// </summary>
        public string Day_14_Notification
        {
            get => this._day_14_Notification;
            set
            {
                this._day_14_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_WorkingTime;

        /// <summary>
        /// (M月)14日 - 勤務時間
        /// </summary>
        public string Day_14_WorkingTime
        {
            get => this._day_14_WorkingTime;
            set
            {
                this._day_14_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_Overtime;

        /// <summary>
        /// (M月)14日 - 残業時間
        /// </summary>
        public string Day_14_Overtime
        {
            get => this._day_14_Overtime;
            set
            {
                this._day_14_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_MidnightTime;

        /// <summary>
        /// (M月)14日 - 深夜時間
        /// </summary>
        public string Day_14_MidnightTime
        {
            get => this._day_14_MidnightTime;
            set
            {
                this._day_14_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_AbsentedTime;

        /// <summary>
        /// (M月)14日 - 欠課時間
        /// </summary>
        public string Day_14_AbsentedTime
        {
            get => this._day_14_AbsentedTime;
            set
            {
                this._day_14_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_14_Remarks;

        /// <summary>
        /// (M月)14日 - 備考
        /// </summary>
        public string Day_14_Remarks
        {
            get => this._day_14_Remarks;
            set
            {
                this._day_14_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)15日

        private System.Windows.Media.Brush _background_15;

        /// <summary>
        /// (M月)15日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_15
        {
            get => this._background_15;
            set
            {
                this._background_15 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15;

        /// <summary>
        /// (M月)15日
        /// </summary>
        public string Day_15
        {
            get => this._day_15;
            set
            {
                this._day_15 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_StartTime;

        /// <summary>
        /// (M月)15日 - 始業
        /// </summary>
        public string Day_15_StartTime
        {
            get => this._day_15_StartTime;
            set
            {
                this._day_15_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_EndTime;

        /// <summary>
        /// (M月)15日 - 終業
        /// </summary>
        public string Day_15_EndTime
        {
            get => this._day_15_EndTime;
            set
            {
                this._day_15_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_LunchTime;

        /// <summary>
        /// (M月)15日 - 昼休憩
        /// </summary>
        public string Day_15_LunchTime
        {
            get => this._day_15_LunchTime;
            set
            {
                this._day_15_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_Notification;

        /// <summary>
        /// (M月)15日 - 届出
        /// </summary>
        public string Day_15_Notification
        {
            get => this._day_15_Notification;
            set
            {
                this._day_15_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_WorkingTime;

        /// <summary>
        /// (M月)15日 - 勤務時間
        /// </summary>
        public string Day_15_WorkingTime
        {
            get => this._day_15_WorkingTime;
            set
            {
                this._day_15_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_Overtime;

        /// <summary>
        /// (M月)15日 - 残業時間
        /// </summary>
        public string Day_15_Overtime
        {
            get => this._day_15_Overtime;
            set
            {
                this._day_15_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_MidnightTime;

        /// <summary>
        /// (M月)15日 - 深夜時間
        /// </summary>
        public string Day_15_MidnightTime
        {
            get => this._day_15_MidnightTime;
            set
            {
                this._day_15_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_AbsentedTime;

        /// <summary>
        /// (M月)15日 - 欠課時間
        /// </summary>
        public string Day_15_AbsentedTime
        {
            get => this._day_15_AbsentedTime;
            set
            {
                this._day_15_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_15_Remarks;

        /// <summary>
        /// (M月)15日 - 備考
        /// </summary>
        public string Day_15_Remarks
        {
            get => this._day_15_Remarks;
            set
            {
                this._day_15_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)16日

        private System.Windows.Media.Brush _background_16;

        /// <summary>
        /// (M月)16日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_16
        {
            get => this._background_16;
            set
            {
                this._background_16 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16;

        /// <summary>
        /// (M月)16日
        /// </summary>
        public string Day_16
        {
            get => this._day_16;
            set
            {
                this._day_16 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_StartTime;

        /// <summary>
        /// (M月)16日 - 始業
        /// </summary>
        public string Day_16_StartTime
        {
            get => this._day_16_StartTime;
            set
            {
                this._day_16_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_EndTime;

        /// <summary>
        /// (M月)16日 - 終業
        /// </summary>
        public string Day_16_EndTime
        {
            get => this._day_16_EndTime;
            set
            {
                this._day_16_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_LunchTime;

        /// <summary>
        /// (M月)16日 - 昼休憩
        /// </summary>
        public string Day_16_LunchTime
        {
            get => this._day_16_LunchTime;
            set
            {
                this._day_16_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_Notification;

        /// <summary>
        /// (M月)16日 - 届出
        /// </summary>
        public string Day_16_Notification
        {
            get => this._day_16_Notification;
            set
            {
                this._day_16_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_WorkingTime;

        /// <summary>
        /// (M月)16日 - 勤務時間
        /// </summary>
        public string Day_16_WorkingTime
        {
            get => this._day_16_WorkingTime;
            set
            {
                this._day_16_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_Overtime;

        /// <summary>
        /// (M月)16日 - 残業時間
        /// </summary>
        public string Day_16_Overtime
        {
            get => this._day_16_Overtime;
            set
            {
                this._day_16_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_MidnightTime;

        /// <summary>
        /// (M月)16日 - 深夜時間
        /// </summary>
        public string Day_16_MidnightTime
        {
            get => this._day_16_MidnightTime;
            set
            {
                this._day_16_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_AbsentedTime;

        /// <summary>
        /// (M月)16日 - 欠課時間
        /// </summary>
        public string Day_16_AbsentedTime
        {
            get => this._day_16_AbsentedTime;
            set
            {
                this._day_16_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_16_Remarks;

        /// <summary>
        /// (M月)16日 - 備考
        /// </summary>
        public string Day_16_Remarks
        {
            get => this._day_16_Remarks;
            set
            {
                this._day_16_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)17日

        private System.Windows.Media.Brush _background_17;

        /// <summary>
        /// (M月)17日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_17
        {
            get => this._background_17;
            set
            {
                this._background_17 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17;

        /// <summary>
        /// (M月)17日
        /// </summary>
        public string Day_17
        {
            get => this._day_17;
            set
            {
                this._day_17 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_StartTime;

        /// <summary>
        /// (M月)17日 - 始業
        /// </summary>
        public string Day_17_StartTime
        {
            get => this._day_17_StartTime;
            set
            {
                this._day_17_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_EndTime;

        /// <summary>
        /// (M月)17日 - 終業
        /// </summary>
        public string Day_17_EndTime
        {
            get => this._day_17_EndTime;
            set
            {
                this._day_17_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_LunchTime;

        /// <summary>
        /// (M月)17日 - 昼休憩
        /// </summary>
        public string Day_17_LunchTime
        {
            get => this._day_17_LunchTime;
            set
            {
                this._day_17_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_Notification;

        /// <summary>
        /// (M月)17日 - 届出
        /// </summary>
        public string Day_17_Notification
        {
            get => this._day_17_Notification;
            set
            {
                this._day_17_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_WorkingTime;

        /// <summary>
        /// (M月)17日 - 勤務時間
        /// </summary>
        public string Day_17_WorkingTime
        {
            get => this._day_17_WorkingTime;
            set
            {
                this._day_17_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_Overtime;

        /// <summary>
        /// (M月)17日 - 残業時間
        /// </summary>
        public string Day_17_Overtime
        {
            get => this._day_17_Overtime;
            set
            {
                this._day_17_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_MidnightTime;

        /// <summary>
        /// (M月)17日 - 深夜時間
        /// </summary>
        public string Day_17_MidnightTime
        {
            get => this._day_17_MidnightTime;
            set
            {
                this._day_17_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_AbsentedTime;

        /// <summary>
        /// (M月)17日 - 欠課時間
        /// </summary>
        public string Day_17_AbsentedTime
        {
            get => this._day_17_AbsentedTime;
            set
            {
                this._day_17_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_17_Remarks;

        /// <summary>
        /// (M月)17日 - 備考
        /// </summary>
        public string Day_17_Remarks
        {
            get => this._day_17_Remarks;
            set
            {
                this._day_17_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)18日

        private System.Windows.Media.Brush _background_18;

        /// <summary>
        /// (M月)18日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_18
        {
            get => this._background_18;
            set
            {
                this._background_18 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18;

        /// <summary>
        /// (M月)18日
        /// </summary>
        public string Day_18
        {
            get => this._day_18;
            set
            {
                this._day_18 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_StartTime;

        /// <summary>
        /// (M月)18日 - 始業
        /// </summary>
        public string Day_18_StartTime
        {
            get => this._day_18_StartTime;
            set
            {
                this._day_18_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_EndTime;

        /// <summary>
        /// (M月)18日 - 終業
        /// </summary>
        public string Day_18_EndTime
        {
            get => this._day_18_EndTime;
            set
            {
                this._day_18_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_LunchTime;

        /// <summary>
        /// (M月)18日 - 昼休憩
        /// </summary>
        public string Day_18_LunchTime
        {
            get => this._day_18_LunchTime;
            set
            {
                this._day_18_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_Notification;

        /// <summary>
        /// (M月)18日 - 届出
        /// </summary>
        public string Day_18_Notification
        {
            get => this._day_18_Notification;
            set
            {
                this._day_18_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_WorkingTime;

        /// <summary>
        /// (M月)18日 - 勤務時間
        /// </summary>
        public string Day_18_WorkingTime
        {
            get => this._day_18_WorkingTime;
            set
            {
                this._day_18_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_Overtime;

        /// <summary>
        /// (M月)18日 - 残業時間
        /// </summary>
        public string Day_18_Overtime
        {
            get => this._day_18_Overtime;
            set
            {
                this._day_18_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_MidnightTime;

        /// <summary>
        /// (M月)18日 - 深夜時間
        /// </summary>
        public string Day_18_MidnightTime
        {
            get => this._day_18_MidnightTime;
            set
            {
                this._day_18_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_AbsentedTime;

        /// <summary>
        /// (M月)18日 - 欠課時間
        /// </summary>
        public string Day_18_AbsentedTime
        {
            get => this._day_18_AbsentedTime;
            set
            {
                this._day_18_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_18_Remarks;

        /// <summary>
        /// (M月)18日 - 備考
        /// </summary>
        public string Day_18_Remarks
        {
            get => this._day_18_Remarks;
            set
            {
                this._day_18_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)19日

        private System.Windows.Media.Brush _background_19;

        /// <summary>
        /// (M月)19日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_19
        {
            get => this._background_19;
            set
            {
                this._background_19 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19;

        /// <summary>
        /// (M月)19日
        /// </summary>
        public string Day_19
        {
            get => this._day_19;
            set
            {
                this._day_19 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_StartTime;

        /// <summary>
        /// (M月)19日 - 始業
        /// </summary>
        public string Day_19_StartTime
        {
            get => this._day_19_StartTime;
            set
            {
                this._day_19_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_EndTime;

        /// <summary>
        /// (M月)19日 - 終業
        /// </summary>
        public string Day_19_EndTime
        {
            get => this._day_19_EndTime;
            set
            {
                this._day_19_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_LunchTime;

        /// <summary>
        /// (M月)19日 - 昼休憩
        /// </summary>
        public string Day_19_LunchTime
        {
            get => this._day_19_LunchTime;
            set
            {
                this._day_19_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_Notification;

        /// <summary>
        /// (M月)19日 - 届出
        /// </summary>
        public string Day_19_Notification
        {
            get => this._day_19_Notification;
            set
            {
                this._day_19_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_WorkingTime;

        /// <summary>
        /// (M月)19日 - 勤務時間
        /// </summary>
        public string Day_19_WorkingTime
        {
            get => this._day_19_WorkingTime;
            set
            {
                this._day_19_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_Overtime;

        /// <summary>
        /// (M月)19日 - 残業時間
        /// </summary>
        public string Day_19_Overtime
        {
            get => this._day_19_Overtime;
            set
            {
                this._day_19_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_MidnightTime;

        /// <summary>
        /// (M月)19日 - 深夜時間
        /// </summary>
        public string Day_19_MidnightTime
        {
            get => this._day_19_MidnightTime;
            set
            {
                this._day_19_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_AbsentedTime;

        /// <summary>
        /// (M月)19日 - 欠課時間
        /// </summary>
        public string Day_19_AbsentedTime
        {
            get => this._day_19_AbsentedTime;
            set
            {
                this._day_19_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_19_Remarks;

        /// <summary>
        /// (M月)19日 - 備考
        /// </summary>
        public string Day_19_Remarks
        {
            get => this._day_19_Remarks;
            set
            {
                this._day_19_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)20日

        private System.Windows.Media.Brush _background_20;

        /// <summary>
        /// (M月)20日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_20
        {
            get => this._background_20;
            set
            {
                this._background_20 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20;

        /// <summary>
        /// (M月)20日
        /// </summary>
        public string Day_20
        {
            get => this._day_20;
            set
            {
                this._day_20 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_StartTime;

        /// <summary>
        /// (M月)20日 - 始業
        /// </summary>
        public string Day_20_StartTime
        {
            get => this._day_20_StartTime;
            set
            {
                this._day_20_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_EndTime;

        /// <summary>
        /// (M月)20日 - 終業
        /// </summary>
        public string Day_20_EndTime
        {
            get => this._day_20_EndTime;
            set
            {
                this._day_20_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_LunchTime;

        /// <summary>
        /// (M月)20日 - 昼休憩
        /// </summary>
        public string Day_20_LunchTime
        {
            get => this._day_20_LunchTime;
            set
            {
                this._day_20_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_Notification;

        /// <summary>
        /// (M月)20日 - 届出
        /// </summary>
        public string Day_20_Notification
        {
            get => this._day_20_Notification;
            set
            {
                this._day_20_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_WorkingTime;

        /// <summary>
        /// (M月)20日 - 勤務時間
        /// </summary>
        public string Day_20_WorkingTime
        {
            get => this._day_20_WorkingTime;
            set
            {
                this._day_20_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_Overtime;

        /// <summary>
        /// (M月)20日 - 残業時間
        /// </summary>
        public string Day_20_Overtime
        {
            get => this._day_20_Overtime;
            set
            {
                this._day_20_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_MidnightTime;

        /// <summary>
        /// (M月)20日 - 深夜時間
        /// </summary>
        public string Day_20_MidnightTime
        {
            get => this._day_20_MidnightTime;
            set
            {
                this._day_20_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_AbsentedTime;

        /// <summary>
        /// (M月)20日 - 欠課時間
        /// </summary>
        public string Day_20_AbsentedTime
        {
            get => this._day_20_AbsentedTime;
            set
            {
                this._day_20_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_20_Remarks;

        /// <summary>
        /// (M月)20日 - 備考
        /// </summary>
        public string Day_20_Remarks
        {
            get => this._day_20_Remarks;
            set
            {
                this._day_20_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)21日

        private System.Windows.Media.Brush _background_21;

        /// <summary>
        /// (M月)21日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_21
        {
            get => this._background_21;
            set
            {
                this._background_21 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21;

        /// <summary>
        /// (M月)21日
        /// </summary>
        public string Day_21
        {
            get => this._day_21;
            set
            {
                this._day_21 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_StartTime;

        /// <summary>
        /// (M月)21日 - 始業
        /// </summary>
        public string Day_21_StartTime
        {
            get => this._day_21_StartTime;
            set
            {
                this._day_21_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_EndTime;

        /// <summary>
        /// (M月)21日 - 終業
        /// </summary>
        public string Day_21_EndTime
        {
            get => this._day_21_EndTime;
            set
            {
                this._day_21_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_LunchTime;

        /// <summary>
        /// (M月)21日 - 昼休憩
        /// </summary>
        public string Day_21_LunchTime
        {
            get => this._day_21_LunchTime;
            set
            {
                this._day_21_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_Notification;

        /// <summary>
        /// (M月)21日 - 届出
        /// </summary>
        public string Day_21_Notification
        {
            get => this._day_21_Notification;
            set
            {
                this._day_21_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_WorkingTime;

        /// <summary>
        /// (M月)21日 - 勤務時間
        /// </summary>
        public string Day_21_WorkingTime
        {
            get => this._day_21_WorkingTime;
            set
            {
                this._day_21_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_Overtime;

        /// <summary>
        /// (M月)21日 - 残業時間
        /// </summary>
        public string Day_21_Overtime
        {
            get => this._day_21_Overtime;
            set
            {
                this._day_21_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_MidnightTime;

        /// <summary>
        /// (M月)21日 - 深夜時間
        /// </summary>
        public string Day_21_MidnightTime
        {
            get => this._day_21_MidnightTime;
            set
            {
                this._day_21_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_AbsentedTime;

        /// <summary>
        /// (M月)21日 - 欠課時間
        /// </summary>
        public string Day_21_AbsentedTime
        {
            get => this._day_21_AbsentedTime;
            set
            {
                this._day_21_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_21_Remarks;

        /// <summary>
        /// (M月)21日 - 備考
        /// </summary>
        public string Day_21_Remarks
        {
            get => this._day_21_Remarks;
            set
            {
                this._day_21_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)22日

        private System.Windows.Media.Brush _background_22;

        /// <summary>
        /// (M月)22日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_22
        {
            get => this._background_22;
            set
            {
                this._background_22 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22;

        /// <summary>
        /// (M月)22日
        /// </summary>
        public string Day_22
        {
            get => this._day_22;
            set
            {
                this._day_22 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_StartTime;

        /// <summary>
        /// (M月)22日 - 始業
        /// </summary>
        public string Day_22_StartTime
        {
            get => this._day_22_StartTime;
            set
            {
                this._day_22_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_EndTime;

        /// <summary>
        /// (M月)22日 - 終業
        /// </summary>
        public string Day_22_EndTime
        {
            get => this._day_22_EndTime;
            set
            {
                this._day_22_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_LunchTime;

        /// <summary>
        /// (M月)22日 - 昼休憩
        /// </summary>
        public string Day_22_LunchTime
        {
            get => this._day_22_LunchTime;
            set
            {
                this._day_22_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_Notification;

        /// <summary>
        /// (M月)22日 - 届出
        /// </summary>
        public string Day_22_Notification
        {
            get => this._day_22_Notification;
            set
            {
                this._day_22_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_WorkingTime;

        /// <summary>
        /// (M月)22日 - 勤務時間
        /// </summary>
        public string Day_22_WorkingTime
        {
            get => this._day_22_WorkingTime;
            set
            {
                this._day_22_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_Overtime;

        /// <summary>
        /// (M月)22日 - 残業時間
        /// </summary>
        public string Day_22_Overtime
        {
            get => this._day_22_Overtime;
            set
            {
                this._day_22_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_MidnightTime;

        /// <summary>
        /// (M月)22日 - 深夜時間
        /// </summary>
        public string Day_22_MidnightTime
        {
            get => this._day_22_MidnightTime;
            set
            {
                this._day_22_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_AbsentedTime;

        /// <summary>
        /// (M月)22日 - 欠課時間
        /// </summary>
        public string Day_22_AbsentedTime
        {
            get => this._day_22_AbsentedTime;
            set
            {
                this._day_22_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_22_Remarks;

        /// <summary>
        /// (M月)22日 - 備考
        /// </summary>
        public string Day_22_Remarks
        {
            get => this._day_22_Remarks;
            set
            {
                this._day_22_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)23日

        private System.Windows.Media.Brush _background_23;

        /// <summary>
        /// (M月)23日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_23
        {
            get => this._background_23;
            set
            {
                this._background_23 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23;

        /// <summary>
        /// (M月)23日
        /// </summary>
        public string Day_23
        {
            get => this._day_23;
            set
            {
                this._day_23 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_StartTime;

        /// <summary>
        /// (M月)23日 - 始業
        /// </summary>
        public string Day_23_StartTime
        {
            get => this._day_23_StartTime;
            set
            {
                this._day_23_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_EndTime;

        /// <summary>
        /// (M月)23日 - 終業
        /// </summary>
        public string Day_23_EndTime
        {
            get => this._day_23_EndTime;
            set
            {
                this._day_23_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_LunchTime;

        /// <summary>
        /// (M月)23日 - 昼休憩
        /// </summary>
        public string Day_23_LunchTime
        {
            get => this._day_23_LunchTime;
            set
            {
                this._day_23_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_Notification;

        /// <summary>
        /// (M月)23日 - 届出
        /// </summary>
        public string Day_23_Notification
        {
            get => this._day_23_Notification;
            set
            {
                this._day_23_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_WorkingTime;

        /// <summary>
        /// (M月)23日 - 勤務時間
        /// </summary>
        public string Day_23_WorkingTime
        {
            get => this._day_23_WorkingTime;
            set
            {
                this._day_23_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_Overtime;

        /// <summary>
        /// (M月)23日 - 残業時間
        /// </summary>
        public string Day_23_Overtime
        {
            get => this._day_23_Overtime;
            set
            {
                this._day_23_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_MidnightTime;

        /// <summary>
        /// (M月)23日 - 深夜時間
        /// </summary>
        public string Day_23_MidnightTime
        {
            get => this._day_23_MidnightTime;
            set
            {
                this._day_23_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_AbsentedTime;

        /// <summary>
        /// (M月)23日 - 欠課時間
        /// </summary>
        public string Day_23_AbsentedTime
        {
            get => this._day_23_AbsentedTime;
            set
            {
                this._day_23_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_23_Remarks;

        /// <summary>
        /// (M月)23日 - 備考
        /// </summary>
        public string Day_23_Remarks
        {
            get => this._day_23_Remarks;
            set
            {
                this._day_23_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)24日

        private System.Windows.Media.Brush _background_24;

        /// <summary>
        /// (M月)24日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_24
        {
            get => this._background_24;
            set
            {
                this._background_24 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24;

        /// <summary>
        /// (M月)24日
        /// </summary>
        public string Day_24
        {
            get => this._day_24;
            set
            {
                this._day_24 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_StartTime;

        /// <summary>
        /// (M月)24日 - 始業
        /// </summary>
        public string Day_24_StartTime
        {
            get => this._day_24_StartTime;
            set
            {
                this._day_24_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_EndTime;

        /// <summary>
        /// (M月)24日 - 終業
        /// </summary>
        public string Day_24_EndTime
        {
            get => this._day_24_EndTime;
            set
            {
                this._day_24_EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_LunchTime;

        /// <summary>
        /// (M月)24日 - 昼休憩
        /// </summary>
        public string Day_24_LunchTime
        {
            get => this._day_24_LunchTime;
            set
            {
                this._day_24_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_Notification;

        /// <summary>
        /// (M月)24日 - 届出
        /// </summary>
        public string Day_24_Notification
        {
            get => this._day_24_Notification;
            set
            {
                this._day_24_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_WorkingTime;

        /// <summary>
        /// (M月)24日 - 勤務時間
        /// </summary>
        public string Day_24_WorkingTime
        {
            get => this._day_24_WorkingTime;
            set
            {
                this._day_24_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_Overtime;

        /// <summary>
        /// (M月)24日 - 残業時間
        /// </summary>
        public string Day_24_Overtime
        {
            get => this._day_24_Overtime;
            set
            {
                this._day_24_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_MidnightTime;

        /// <summary>
        /// (M月)24日 - 深夜時間
        /// </summary>
        public string Day_24_MidnightTime
        {
            get => this._day_24_MidnightTime;
            set
            {
                this._day_24_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_AbsentedTime;

        /// <summary>
        /// (M月)24日 - 欠課時間
        /// </summary>
        public string Day_24_AbsentedTime
        {
            get => this._day_24_AbsentedTime;
            set
            {
                this._day_24_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_24_Remarks;

        /// <summary>
        /// (M月)24日 - 備考
        /// </summary>
        public string Day_24_Remarks
        {
            get => this._day_24_Remarks;
            set
            {
                this._day_24_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)25日

        private System.Windows.Media.Brush _background_25;

        /// <summary>
        /// (M月)25日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_25
        {
            get => this._background_25;
            set
            {
                this._background_25 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25;

        /// <summary>
        /// (M月)25日
        /// </summary>
        public string Day_25
        {
            get => this._day_25;
            set
            {
                this._day_25 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25_StartTime;

        /// <summary>
        /// (M月)25日 - 始業
        /// </summary>
        public string Day_25_StartTime
        {
            get => this._day_25_StartTime;
            set
            {
                this._day_25_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25EndTime;

        /// <summary>
        /// (M月)25日 - 終業
        /// </summary>
        public string Day_25_EndTime
        {
            get => this._day_25EndTime;
            set
            {
                this._day_25EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25_LunchTime;

        /// <summary>
        /// (M月)25日 - 昼休憩
        /// </summary>
        public string Day_25_LunchTime
        {
            get => this._day_25_LunchTime;
            set
            {
                this._day_25_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25_Notification;

        /// <summary>
        /// (M月)25日 - 届出
        /// </summary>
        public string Day_25_Notification
        {
            get => this._day_25_Notification;
            set
            {
                this._day_25_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25_WorkingTime;

        /// <summary>
        /// (M月)25日 - 勤務時間
        /// </summary>
        public string Day_25_WorkingTime
        {
            get => this._day_25_WorkingTime;
            set
            {
                this._day_25_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25_Overtime;

        /// <summary>
        /// (M月)25日 - 残業時間
        /// </summary>
        public string Day_25_Overtime
        {
            get => this._day_25_Overtime;
            set
            {
                this._day_25_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25_MidnightTime;

        /// <summary>
        /// (M月)25日 - 深夜時間
        /// </summary>
        public string Day_25_MidnightTime
        {
            get => this._day_25_MidnightTime;
            set
            {
                this._day_25_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25_AbsentedTime;

        /// <summary>
        /// (M月)25日 - 欠課時間
        /// </summary>
        public string Day_25_AbsentedTime
        {
            get => this._day_25_AbsentedTime;
            set
            {
                this._day_25_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_25_Remarks;

        /// <summary>
        /// (M月)25日 - 備考
        /// </summary>
        public string Day_25_Remarks
        {
            get => this._day_25_Remarks;
            set
            {
                this._day_25_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)26日

        private System.Windows.Media.Brush _background_26;

        /// <summary>
        /// (M月)26日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_26
        {
            get => this._background_26;
            set
            {
                this._background_26 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26;

        /// <summary>
        /// (M月)26日
        /// </summary>
        public string Day_26
        {
            get => this._day_26;
            set
            {
                this._day_26 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26_StartTime;

        /// <summary>
        /// (M月)26日 - 始業
        /// </summary>
        public string Day_26_StartTime
        {
            get => this._day_26_StartTime;
            set
            {
                this._day_26_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26EndTime;

        /// <summary>
        /// (M月)26日 - 終業
        /// </summary>
        public string Day_26_EndTime
        {
            get => this._day_26EndTime;
            set
            {
                this._day_26EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26_LunchTime;

        /// <summary>
        /// (M月)26日 - 昼休憩
        /// </summary>
        public string Day_26_LunchTime
        {
            get => this._day_26_LunchTime;
            set
            {
                this._day_26_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26_Notification;

        /// <summary>
        /// (M月)26日 - 届出
        /// </summary>
        public string Day_26_Notification
        {
            get => this._day_26_Notification;
            set
            {
                this._day_26_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26_WorkingTime;

        /// <summary>
        /// (M月)26日 - 勤務時間
        /// </summary>
        public string Day_26_WorkingTime
        {
            get => this._day_26_WorkingTime;
            set
            {
                this._day_26_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26_Overtime;

        /// <summary>
        /// (M月)26日 - 残業時間
        /// </summary>
        public string Day_26_Overtime
        {
            get => this._day_26_Overtime;
            set
            {
                this._day_26_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26_MidnightTime;

        /// <summary>
        /// (M月)26日 - 深夜時間
        /// </summary>
        public string Day_26_MidnightTime
        {
            get => this._day_26_MidnightTime;
            set
            {
                this._day_26_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26_AbsentedTime;

        /// <summary>
        /// (M月)26日 - 欠課時間
        /// </summary>
        public string Day_26_AbsentedTime
        {
            get => this._day_26_AbsentedTime;
            set
            {
                this._day_26_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_26_Remarks;

        /// <summary>
        /// (M月)26日 - 備考
        /// </summary>
        public string Day_26_Remarks
        {
            get => this._day_26_Remarks;
            set
            {
                this._day_26_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)27日

        private System.Windows.Media.Brush _background_27;

        /// <summary>
        /// (M月)27日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_27
        {
            get => this._background_27;
            set
            {
                this._background_27 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27;

        /// <summary>
        /// (M月)27日
        /// </summary>
        public string Day_27
        {
            get => this._day_27;
            set
            {
                this._day_27 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27_StartTime;

        /// <summary>
        /// (M月)27日 - 始業
        /// </summary>
        public string Day_27_StartTime
        {
            get => this._day_27_StartTime;
            set
            {
                this._day_27_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27EndTime;

        /// <summary>
        /// (M月)27日 - 終業
        /// </summary>
        public string Day_27_EndTime
        {
            get => this._day_27EndTime;
            set
            {
                this._day_27EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27_LunchTime;

        /// <summary>
        /// (M月)27日 - 昼休憩
        /// </summary>
        public string Day_27_LunchTime
        {
            get => this._day_27_LunchTime;
            set
            {
                this._day_27_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27_Notification;

        /// <summary>
        /// (M月)27日 - 届出
        /// </summary>
        public string Day_27_Notification
        {
            get => this._day_27_Notification;
            set
            {
                this._day_27_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27_WorkingTime;

        /// <summary>
        /// (M月)27日 - 勤務時間
        /// </summary>
        public string Day_27_WorkingTime
        {
            get => this._day_27_WorkingTime;
            set
            {
                this._day_27_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27_Overtime;

        /// <summary>
        /// (M月)27日 - 残業時間
        /// </summary>
        public string Day_27_Overtime
        {
            get => this._day_27_Overtime;
            set
            {
                this._day_27_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27_MidnightTime;

        /// <summary>
        /// (M月)27日 - 深夜時間
        /// </summary>
        public string Day_27_MidnightTime
        {
            get => this._day_27_MidnightTime;
            set
            {
                this._day_27_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27_AbsentedTime;

        /// <summary>
        /// (M月)27日 - 欠課時間
        /// </summary>
        public string Day_27_AbsentedTime
        {
            get => this._day_27_AbsentedTime;
            set
            {
                this._day_27_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_27_Remarks;

        /// <summary>
        /// (M月)27日 - 備考
        /// </summary>
        public string Day_27_Remarks
        {
            get => this._day_27_Remarks;
            set
            {
                this._day_27_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)28日

        private System.Windows.Media.Brush _background_28;

        /// <summary>
        /// (M月)28日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_28
        {
            get => this._background_28;
            set
            {
                this._background_28 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28;

        /// <summary>
        /// (M月)28日
        /// </summary>
        public string Day_28
        {
            get => this._day_28;
            set
            {
                this._day_28 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28_StartTime;

        /// <summary>
        /// (M月)28日 - 始業
        /// </summ8ary>
        public string Day_28_StartTime
        {
            get => this._day_28_StartTime;
            set
            {
                this._day_28_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28EndTime;

        /// <summary>
        /// (M月)28日 - 終業
        /// </summary>
        public string Day_28_EndTime
        {
            get => this._day_28EndTime;
            set
            {
                this._day_28EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28_LunchTime;

        /// <summary>
        /// (M月)28日 - 昼休憩
        /// </summary>
        public string Day_28_LunchTime
        {
            get => this._day_28_LunchTime;
            set
            {
                this._day_28_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28_Notification;

        /// <summary>
        /// (M月)28日 - 届出
        /// </summary>
        public string Day_28_Notification
        {
            get => this._day_28_Notification;
            set
            {
                this._day_28_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28_WorkingTime;

        /// <summary>
        /// (M月)28日 - 勤務時間
        /// </summary>
        public string Day_28_WorkingTime
        {
            get => this._day_28_WorkingTime;
            set
            {
                this._day_28_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28_Overtime;

        /// <summary>
        /// (M月)28日 - 残業時間
        /// </summary>
        public string Day_28_Overtime
        {
            get => this._day_28_Overtime;
            set
            {
                this._day_28_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28_MidnightTime;

        /// <summary>
        /// (M月)28日 - 深夜時間
        /// </summary>
        public string Day_28_MidnightTime
        {
            get => this._day_28_MidnightTime;
            set
            {
                this._day_28_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28_AbsentedTime;

        /// <summary>
        /// (M月)28日 - 欠課時間
        /// </summary>
        public string Day_28_AbsentedTime
        {
            get => this._day_28_AbsentedTime;
            set
            {
                this._day_28_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_28_Remarks;

        /// <summary>
        /// (M月)28日 - 備考
        /// </summary>
        public string Day_28_Remarks
        {
            get => this._day_28_Remarks;
            set
            {
                this._day_28_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)29日

        private System.Windows.Media.Brush _background_29;

        /// <summary>
        /// (M月)29日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_29
        {
            get => this._background_29;
            set
            {
                this._background_29 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29;

        /// <summary>
        /// (M月)29日
        /// </summary>
        public string Day_29
        {
            get => this._day_29;
            set
            {
                this._day_29 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29_StartTime;

        /// <summary>
        /// (M月)29日 - 始業
        /// </summ8ary>
        public string Day_29_StartTime
        {
            get => this._day_29_StartTime;
            set
            {
                this._day_29_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29EndTime;

        /// <summary>
        /// (M月)29日 - 終業
        /// </summary>
        public string Day_29_EndTime
        {
            get => this._day_29EndTime;
            set
            {
                this._day_29EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29_LunchTime;

        /// <summary>
        /// (M月)29日 - 昼休憩
        /// </summary>
        public string Day_29_LunchTime
        {
            get => this._day_29_LunchTime;
            set
            {
                this._day_29_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29_Notification;

        /// <summary>
        /// (M月)29日 - 届出
        /// </summary>
        public string Day_29_Notification
        {
            get => this._day_29_Notification;
            set
            {
                this._day_29_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29_WorkingTime;

        /// <summary>
        /// (M月)29日 - 勤務時間
        /// </summary>
        public string Day_29_WorkingTime
        {
            get => this._day_29_WorkingTime;
            set
            {
                this._day_29_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29_Overtime;

        /// <summary>
        /// (M月)29日 - 残業時間
        /// </summary>
        public string Day_29_Overtime
        {
            get => this._day_29_Overtime;
            set
            {
                this._day_29_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29_MidnightTime;

        /// <summary>
        /// (M月)29日 - 深夜時間
        /// </summary>
        public string Day_29_MidnightTime
        {
            get => this._day_29_MidnightTime;
            set
            {
                this._day_29_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29_AbsentedTime;

        /// <summary>
        /// (M月)29日 - 欠課時間
        /// </summary>
        public string Day_29_AbsentedTime
        {
            get => this._day_29_AbsentedTime;
            set
            {
                this._day_29_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_29_Remarks;

        /// <summary>
        /// (M月)29日 - 備考
        /// </summary>
        public string Day_29_Remarks
        {
            get => this._day_29_Remarks;
            set
            {
                this._day_29_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)30日

        private System.Windows.Media.Brush _background_30;

        /// <summary>
        /// (M月)30日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_30
        {
            get => this._background_30;
            set
            {
                this._background_30 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30;

        /// <summary>
        /// (M月)30日
        /// </summary>
        public string Day_30
        {
            get => this._day_30;
            set
            {
                this._day_30 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30_StartTime;

        /// <summary>
        /// (M月)30日 - 始業
        /// </summ8ary>
        public string Day_30_StartTime
        {
            get => this._day_30_StartTime;
            set
            {
                this._day_30_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30EndTime;

        /// <summary>
        /// (M月)30日 - 終業
        /// </summary>
        public string Day_30_EndTime
        {
            get => this._day_30EndTime;
            set
            {
                this._day_30EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30_LunchTime;

        /// <summary>
        /// (M月)30日 - 昼休憩
        /// </summary>
        public string Day_30_LunchTime
        {
            get => this._day_30_LunchTime;
            set
            {
                this._day_30_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30_Notification;

        /// <summary>
        /// (M月)30日 - 届出
        /// </summary>
        public string Day_30_Notification
        {
            get => this._day_30_Notification;
            set
            {
                this._day_30_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30_WorkingTime;

        /// <summary>
        /// (M月)30日 - 勤務時間
        /// </summary>
        public string Day_30_WorkingTime
        {
            get => this._day_30_WorkingTime;
            set
            {
                this._day_30_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30_Overtime;

        /// <summary>
        /// (M月)30日 - 残業時間
        /// </summary>
        public string Day_30_Overtime
        {
            get => this._day_30_Overtime;
            set
            {
                this._day_30_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30_MidnightTime;

        /// <summary>
        /// (M月)30日 - 深夜時間
        /// </summary>
        public string Day_30_MidnightTime
        {
            get => this._day_30_MidnightTime;
            set
            {
                this._day_30_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30_AbsentedTime;

        /// <summary>
        /// (M月)30日 - 欠課時間
        /// </summary>
        public string Day_30_AbsentedTime
        {
            get => this._day_30_AbsentedTime;
            set
            {
                this._day_30_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_30_Remarks;

        /// <summary>
        /// (M月)30日 - 備考
        /// </summary>
        public string Day_30_Remarks
        {
            get => this._day_30_Remarks;
            set
            {
                this._day_30_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region (M月)31日

        private System.Windows.Media.Brush _background_31;

        /// <summary>
        /// (M月)31日 - Background
        /// </summary>
        public System.Windows.Media.Brush Background_31
        {
            get => this._background_31;
            set
            {
                this._background_31 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31;

        /// <summary>
        /// (M月)31日
        /// </summary>
        public string Day_31
        {
            get => this._day_31;
            set
            {
                this._day_31 = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31_StartTime;

        /// <summary>
        /// (M月)31日 - 始業
        /// </summ8ary>
        public string Day_31_StartTime
        {
            get => this._day_31_StartTime;
            set
            {
                this._day_31_StartTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31EndTime;

        /// <summary>
        /// (M月)31日 - 終業
        /// </summary>
        public string Day_31_EndTime
        {
            get => this._day_31EndTime;
            set
            {
                this._day_31EndTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31_LunchTime;

        /// <summary>
        /// (M月)30日 - 昼休憩
        /// </summary>
        public string Day_31_LunchTime
        {
            get => this._day_31_LunchTime;
            set
            {
                this._day_31_LunchTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31_Notification;

        /// <summary>
        /// (M月)31日 - 届出
        /// </summary>
        public string Day_31_Notification
        {
            get => this._day_31_Notification;
            set
            {
                this._day_31_Notification = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31_WorkingTime;

        /// <summary>
        /// (M月)31日 - 勤務時間
        /// </summary>
        public string Day_31_WorkingTime
        {
            get => this._day_31_WorkingTime;
            set
            {
                this._day_31_WorkingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31_Overtime;

        /// <summary>
        /// (M月)31日 - 残業時間
        /// </summary>
        public string Day_31_Overtime
        {
            get => this._day_31_Overtime;
            set
            {
                this._day_31_Overtime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31_MidnightTime;

        /// <summary>
        /// (M月)31日 - 深夜時間
        /// </summary>
        public string Day_31_MidnightTime
        {
            get => this._day_31_MidnightTime;
            set
            {
                this._day_31_MidnightTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31_AbsentedTime;

        /// <summary>
        /// (M月)31日 - 欠課時間
        /// </summary>
        public string Day_31_AbsentedTime
        {
            get => this._day_31_AbsentedTime;
            set
            {
                this._day_31_AbsentedTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _day_31_Remarks;

        /// <summary>
        /// (M月)31日 - 備考
        /// </summary>
        public string Day_31_Remarks
        {
            get => this._day_31_Remarks;
            set
            {
                this._day_31_Remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
