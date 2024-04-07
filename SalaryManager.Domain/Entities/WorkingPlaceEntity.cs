using SalaryManager.Domain.ValueObjects;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 就業場所
    /// </summary>
    public sealed class WorkingPlaceEntity
    {
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">会社名</param>
        /// <param name="address">住所</param>
        /// <param name="workingStartTime">労働時間(始業)</param>
        /// <param name="workingEndTime">労働時間(終業)</param>
        /// <param name="lunchStartTime">昼休憩(開始)</param>
        /// <param name="lunchEndTime">昼休憩(終了)</param>
        /// <param name="breakStartTime">休憩(開始)</param>
        /// <param name="breakEndTime">休憩(終了)</param>
        /// <param name="remarks">備考</param>
        public WorkingPlaceEntity(
            int id,
            string name,
            string address,
            (int Hour, int Minute) workingStartTime,
            (int Hour, int Minute) workingEndTime,
            (int Hour, int Minute) lunchStartTime,
            (int Hour, int Minute) lunchEndTime,
            (int Hour, int Minute) breakStartTime,
            (int Hour, int Minute) breakEndTime,
            string remarks)
        {
            this.ID = id;
            this.CompanyName = new CompanyNameValue(name);
            this.Address = address;

            this.WorkingTime = (new TimeValue(workingStartTime.Hour, workingStartTime.Minute),
                                new TimeValue(workingEndTime.Hour,   workingEndTime.Minute));

            this.LunchTime   = (new TimeValue(lunchStartTime.Hour, lunchStartTime.Minute),
                                new TimeValue(lunchEndTime.Hour,   lunchEndTime.Minute));

            this.BreakTime = (new TimeValue(breakStartTime.Hour, breakStartTime.Minute),
                                new TimeValue(breakEndTime.Hour, breakEndTime.Minute));
            this.Remarks = remarks;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">会社名</param>
        /// <param name="address">住所</param>
        /// <param name="working_Start_Hour">労働 - 開始 - 時</param>
        /// <param name="working_Start_Minute">労働 - 開始 - 分</param>
        /// <param name="working_End_Hour">労働 - 終了 - 時</param>
        /// <param name="working_End_Minute">労働 - 終了 - 分</param>
        /// <param name="lunch_Start_Hour">昼休憩 - 開始 - 時</param>
        /// <param name="lunch_Start_Minute">昼休憩 - 開始 - 分</param>
        /// <param name="lunch_End_Hour">昼休憩 - 終了 - 時</param>
        /// <param name="lunch_End_Minute">昼休憩 - 終了 - 分</param>
        /// <param name="break_Start_Hour">休憩 - 開始 - 時</param>
        /// <param name="break_Start_Minute">休憩 - 開始 - 分</param>
        /// <param name="break_End_Hour">休憩 - 終了 - 時</param>
        /// <param name="break_End_Minute">休憩 - 終了 - 分</param>
        /// <param name="remarks">備考</param>
        public WorkingPlaceEntity(
            int id,
            string name,
            string address,
            int working_Start_Hour,
            int working_Start_Minute,
            int working_End_Hour,
            int working_End_Minute,
            int lunch_Start_Hour,
            int lunch_Start_Minute,
            int lunch_End_Hour,
            int lunch_End_Minute,
            int break_Start_Hour,
            int break_Start_Minute,
            int break_End_Hour,
            int break_End_Minute,
            string remarks)
        {
            this.ID = id;
            this.CompanyName = new CompanyNameValue(name);
            this.Address = address;

            this.WorkingTime = (new TimeValue(working_Start_Hour, working_Start_Minute),
                                new TimeValue(working_End_Hour, working_End_Minute));

            this.LunchTime = (new TimeValue(lunch_Start_Hour, lunch_Start_Minute),
                                new TimeValue(lunch_End_Hour, lunch_End_Minute));

            this.BreakTime = (new TimeValue(break_Start_Hour, break_Start_Minute),
                                new TimeValue(break_End_Hour, break_End_Minute));

            this.Remarks = remarks;
        }

        /// <summary> ID </summary>
        public int ID { get; }

        /// <summary> 会社名 </summary>
        public CompanyNameValue CompanyName { get; }

        /// <summary> 住所 </summary>
        public string Address { get; }

        /// <summary> 労働時間 </summary>
        /// <remarks> (始業時刻, 終業時刻) </remarks>
        public (TimeValue Start, TimeValue End) WorkingTime { get; }

        /// <summary> 昼休憩 </summary>
        /// <remarks> (開始時刻, 終了時刻) </remarks>
        public (TimeValue Start, TimeValue End) LunchTime { get; }

        /// <summary> 休憩 </summary>
        /// <remarks> (開始時刻, 終了時刻) </remarks>
        public (TimeValue Start, TimeValue End) BreakTime { get; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }
    }
}
