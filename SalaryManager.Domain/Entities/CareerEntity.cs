namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 職歴
/// </summary>
/// <param name="id">ID</param>
/// <param name="workingStatus">雇用形態</param>
/// <param name="companyName">会社名</param>
/// <param name="employeeNumber">社員番号</param>
/// <param name="workingStartDate">勤務開始日</param>
/// <param name="workingEndDate">勤務終了日</param>
/// <param name="allowanceExistence">手当</param>
/// <param name="remarks">備考</param>
[Table("Career")]
public sealed record class CareerEntity(
    int id,
    string workingStatus,
    string companyName,
    string employeeNumber,
    DateTime workingStartDate,
    DateTime workingEndDate,
    AllowanceExistenceEntity allowanceExistence,
    string remarks) : ITableEntity
{
    /// <summary> ID </summary>
    [Column("ID")]
    public int ID => id;

    /// <summary> 雇用形態 </summary>
    [Column("WorkingStatus")]
    public string WorkingStatus => workingStatus;

    /// <summary> 会社名 </summary>
    [Column("CompanyName")]
    public CompanyNameValue CompanyName => new CompanyNameValue(companyName);

    /// <summary> 社員番号 </summary>
    [Column("EmployeeNumber")]
    public string EmployeeNumber => employeeNumber;

    /// <summary> 勤務開始日 </summary>
    [Column("WorkingStartDate")]
    public WorkingDateValue WorkingStartDate => new WorkingDateValue(workingStartDate);

    /// <summary> 勤務終了日 </summary>
    [Column("WorkingEndDate")]
    public WorkingDateValue WorkingEndDate => new WorkingDateValue(workingEndDate);

    /// <summary> 手当 </summary>
    [Column("AllowanceExistence")]
    public AllowanceExistenceEntity AllowanceExistence => allowanceExistence;

    /// <summary> 備考 </summary>
    [Column("Remarks")]
    public string Remarks => remarks;
}
