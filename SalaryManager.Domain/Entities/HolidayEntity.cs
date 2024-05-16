namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 休祝日
/// </summary>
public sealed class HolidayEntity
{
    public HolidayEntity(DateTime date, string name, string companyName, string remarks)
    {
        this.Date        = date;
        this.Name        = name;
        this.CompanyName = companyName;
        this.Remarks     = remarks;
    }

    /// <summary> 日付 </summary>
    public DateTime Date { get; }

    /// <summary> 祝日名 </summary>
    public string Name { get; }

    /// <summary> 会社名 </summary>
    public string CompanyName { get; }

    /// <summary> 備考 </summary>
    public string Remarks { get; }
}
