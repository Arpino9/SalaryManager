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
public sealed class CareerEntity(
    int id,
    string workingStatus,
    string companyName,
    string employeeNumber,
    DateOnly workingStartDate,
    DateOnly workingEndDate,
    AllowanceExistenceEntity allowanceExistence,
    string remarks)
{
    /// <summary> ID </summary>
    public int ID => id;

    /// <summary> 雇用形態 </summary>
    public string WorkingStatus => workingStatus;

    /// <summary> 会社名 </summary>
    public CompanyNameValue CompanyName => new CompanyNameValue(companyName);

    /// <summary> 社員番号 </summary>
    public string EmployeeNumber => employeeNumber;

    /// <summary> 勤務開始日 </summary>
    public WorkingDateValue WorkingStartDate => new WorkingDateValue(workingStartDate);

    /// <summary> 勤務終了日 </summary>
    public WorkingDateValue WorkingEndDate => new WorkingDateValue(workingEndDate);

    /// <summary> 手当 </summary>
    public AllowanceExistenceEntity AllowanceExistence => allowanceExistence;

    /// <summary> 備考 </summary>
    public string Remarks => remarks;
}
