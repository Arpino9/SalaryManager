namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 就業場所
/// </summary>
/// <param name="id">ID</param>
/// <param name="dispatchingCompany">派遣元会社</param>
/// <param name="dispatchedCompany"></param>
/// <param name="workingPlace"></param>
/// <param name="workingAddress"></param>
/// <param name="WorkingStart"></param>
/// <param name="WorkingEnd"></param>
/// <param name="isWaiting"></param>
/// <param name="isWorking"></param>
/// <param name="workingStartTime"></param>
/// <param name="workingEndTime"></param>
/// <param name="lunchStartTime"></param>
/// <param name="lunchEndTime"></param>
/// <param name="breakStartTime"></param>
/// <param name="breakEndTime"></param>
/// <param name="remarks"></param>
public sealed class WorkingPlaceEntity(
    int id,
    string dispatchingCompany,
    string dispatchedCompany,
    string workingPlace,
    string workingAddress,
    DateOnly workingStart,
    DateOnly workingEnd,
    bool isWaiting,
    bool isWorking,
    TimeOnly workingStartTime,
    TimeOnly workingEndTime,
    TimeOnly lunchStartTime,
    TimeOnly lunchEndTime,
    TimeOnly breakStartTime,
    TimeOnly breakEndTime,
    string remarks) : IEntity
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="dispatchingCompany">派遣元会社</param>
    /// <param name="workingCompanyAddress">住所</param>
    /// <param name="WorkingPlace">就業先名</param>
    /// <param name="workingCompanyAddress">就業先住所</param>
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
        string dispatchingCompany,
        string dispatchedCompany,
        string WorkingPlace,
        string workingCompanyAddress,
        DateOnly WorkingStart,
        DateOnly WorkingEnd,
        bool isWaiting,
        bool isWorking,
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
        string remarks) : this(id, dispatchingCompany, dispatchedCompany, WorkingPlace, workingCompanyAddress,
                               WorkingStart, WorkingEnd, isWaiting, isWorking,
                              new TimeOnly(working_Start_Hour, working_Start_Minute),
                              new TimeOnly(working_End_Hour, working_End_Minute),
                              new TimeOnly(lunch_Start_Hour, lunch_Start_Minute),
                              new TimeOnly(lunch_End_Hour, lunch_End_Minute),
                              new TimeOnly(break_Start_Hour, break_Start_Minute),
                              new TimeOnly(break_End_Hour, break_End_Minute),
                              remarks)
    {
        
    }

    /// <summary> ID </summary>
    public int ID => id;

    /// <summary> 派遣元会社 </summary>
    public CompanyNameValue DispatchingCompany => new CompanyNameValue(dispatchingCompany);

    /// <summary> 派遣先会社 </summary>
    public CompanyNameValue DispatchedCompany => new CompanyNameValue(dispatchedCompany);

    /// <summary> 就業先(名称) </summary>
    public CompanyNameValue WorkingPlace_Name => new CompanyNameValue(workingPlace);

    /// <summary> 就業先(住所) </summary>
    public string WorkingPlace_Address => workingAddress;

    /// <summary> 勤務開始 </summary>
    public DateOnly WorkingStart => workingStart;

    /// <summary> 勤務終了 </summary>
    public DateOnly WorkingEnd => this.IsWorking ? DateUtils.Today : workingEnd;

    /// <summary> 待機中か </summary>
    public bool IsWaiting => isWaiting;

    /// <summary> 就業中か </summary>
    public bool IsWorking => isWorking;

    /// <summary> 労働時間 </summary>
    /// <remarks> (始業時刻, 終業時刻) </remarks>
    public (TimeSpan Start, TimeSpan End) WorkingTime => 
        (new TimeSpan(workingStartTime.Hour, workingStartTime.Minute, 0),
         new TimeSpan(workingEndTime.Hour,   workingEndTime.Minute, 0));

    /// <summary> 昼休憩 </summary>
    /// <remarks> (開始時刻, 終了時刻) </remarks>
    public (TimeSpan Start, TimeSpan End) LunchTime => 
        (new TimeSpan(lunchStartTime.Hour, lunchStartTime.Minute, 0),
         new TimeSpan(lunchEndTime.Hour,   lunchEndTime.Minute, 0));

    /// <summary> 休憩 </summary>
    /// <remarks> (開始時刻, 終了時刻) </remarks>
    public (TimeSpan Start, TimeSpan End) BreakTime => 
        (new TimeSpan(breakStartTime.Hour, breakStartTime.Minute, 0),
         new TimeSpan(breakEndTime.Hour,   breakEndTime.Minute, 0));

    /// <summary> 備考 </summary>
    public string Remarks => remarks;

    /// <summary> 名目労働時間 </summary>
    public TimeSpan NominalWorkTimeSpan
        => this.WorkingTime.End - this.WorkingTime.Start;

    /// <summary> 実働労働時間 </summary>
    public TimeSpan ActualWorkTimeSpan
        => this.NominalWorkTimeSpan - this.LunchTimeSpan;

    /// <summary> 昼休憩時間 </summary>
    public TimeSpan LunchTimeSpan
        => (this.LunchTime.End - this.LunchTime.Start);
}
