namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 休祝日
/// </summary>
/// <param name="date">日付</param>
/// <param name="name">祝日名</param>
/// <param name="companyName">会社名</param>
/// <param name="remarks">備考</param>
public sealed class HolidayEntity(
    DateTime date, 
    string name, 
    string companyName, 
    string remarks) : IEntity
{
    /// <summary> 日付 </summary>
    public DateTime Date => date;

    /// <summary> 祝日名 </summary>
    public string Name => name;

    /// <summary> 会社名 </summary>
    public string CompanyName => companyName;

    /// <summary> 備考 </summary>
    public string Remarks => remarks;
}
