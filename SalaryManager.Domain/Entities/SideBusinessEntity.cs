namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 副業
/// </summary>
/// <param name="id">ID</param>
/// <param name="yearMonth">年月</param>
/// <param name="sideBusiness">副業収入</param>
/// <param name="perquisite">臨時収入</param>
/// <param name="other">その他</param>
/// <param name="remarks">備考</param>
public sealed class SideBusinessEntity(
    int id,
    DateOnly yearMonth,
    double sideBusiness,
    double perquisite,
    double other,
    string remarks)
{
    /// <summary> ID </summary>
    public int ID => id;

    /// <summary> 年月 </summary>
    public DateOnly YearMonth => yearMonth;

    /// <summary> 副業収入 </summary>
    public double SideBusiness => sideBusiness;

    /// <summary> 臨時収入 </summary>
    public double Perquisite => perquisite;

    /// <summary> その他 </summary>
    public double Others => other;

    /// <summary> 備考 </summary>
    public string Remarks => remarks;
}
