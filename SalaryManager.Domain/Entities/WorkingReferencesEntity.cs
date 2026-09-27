namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 勤務備考
/// </summary>
/// <param name="id">ID</param>
/// <param name="yearMonth">年月</param>
/// <param name="overtimeTime">時間外時間</param>
/// <param name="weekendWorktime">休出時間</param>
/// <param name="midnightWorktime">深夜時間</param>
/// <param name="lateAbsentH">遅刻早退欠勤H</param>
/// <param name="insurance">支給額-保険</param>
/// <param name="norm">標準月額千円</param>
/// <param name="numberOfDependent">扶養人数</param>
/// <param name="paidVacation">有給残日数</param>
/// <param name="workingHours">勤務時間</param>
/// <param name="workingPlace">勤務先</param>
/// <param name="remarks">備考</param>
[Table("WorkingReferences")]
public sealed class WorkingReferencesEntity(
    int id,
    DateOnly yearMonth,
    double overtimeTime,
    double weekendWorktime,
    double midnightWorktime,
    double lateAbsentH,
    double insurance,
    double norm,
    double numberOfDependent,
    double paidVacation,
    double workingHours,
    string workingPlace,
    string remarks) : ITableEntity
{
    /// <summary> ID </summary>
    [Column("ID")]
    public int ID => id;

    /// <summary> 年月 </summary>
    [Column("YearMonth")]
    public DateOnly YearMonth => yearMonth;

    /// <summary> 時間外時間 </summary>
    [Column("OvertimeTime")]
    public double OvertimeTime => overtimeTime;

    /// <summary> 休出時間 </summary>
    [Column("WeekendWorktime")]
    public double WeekendWorktime => weekendWorktime;

    /// <summary> 深夜時間 </summary>
    [Column("MidnightWorktime")]
    public double MidnightWorktime => midnightWorktime;

    /// <summary> 遅刻早退欠勤H </summary>
    [Column("LateAbsentH")]
    public double LateAbsentH => lateAbsentH;

    /// <summary> 支給額-保険 </summary>
    [Column("Insurance")]
    public MoneyValue Insurance => new MoneyValue(insurance);

    /// <summary> 標準月額千円 </summary>
    [Column("Norm")]
    public double Norm => norm;

    /// <summary> 扶養人数 </summary>
    [Column("NumberOfDependent")]
    public double NumberOfDependent => numberOfDependent;

    /// <summary> 有給残日数 </summary>
    [Column("PaidVacation")]
    public PaidVacationDaysValue PaidVacation => new PaidVacationDaysValue(paidVacation);

    /// <summary> 勤務時間 </summary>
    [Column("WorkingHours")]
    public double WorkingHours => workingHours;

    /// <summary> 勤務先 </summary>
    [Column("WorkingPlace")]
    public string WorkPlace => workingPlace;

    /// <summary> 備考 </summary>
    [Column("Remarks")]
    public string Remarks => remarks;
}
