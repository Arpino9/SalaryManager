namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - ヘッダ
/// </summary>
/// <param name="id">ID</param>
/// <param name="yearMonth">年月</param>
/// <param name="isDefault">デフォルト設定か</param>
/// <param name="createDate">作成日</param>
/// <param name="upDateDate">更新日</param>
public sealed class HeaderEntity(
    int id,
    DateOnly yearMonth,
    bool isDefault,
    DateTime createDate,
    DateTime upDateDate) : IEntity
{
    /// <summary> ID </summary>
    public int ID { get; set; } = id;

    /// <summary> 年月 </summary>
    public DateOnly YearMonth { get; set; } = yearMonth;

    /// <summary> デフォルト設定か </summary>
    public bool IsDefault { get; set; } = isDefault;

    /// <summary> 作成日 </summary>
    public DateTime CreateDate => createDate;

    /// <summary> 更新日 </summary>
    public DateTime UpdateDate => upDateDate;
}
