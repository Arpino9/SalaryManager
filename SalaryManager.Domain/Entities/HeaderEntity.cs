namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - ヘッダ
/// </summary>
public sealed class HeaderEntity
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="yearMonth">年月</param>
    /// <param name="isDefault">デフォルト設定か</param>
    /// <param name="createDate">作成日</param>
    /// <param name="upDateDate">更新日</param>
    public HeaderEntity(
        int id,
        DateTime yearMonth,
        bool isDefault,
        DateTime createDate,
        DateTime upDateDate)
    {
        this.ID         = id;
        this.YearMonth  = yearMonth;
        this.IsDefault  = isDefault;
        this.CreateDate = createDate;
        this.UpdateDate = upDateDate;
    }

    /// <summary> ID </summary>
    public int ID { get; set; }

    /// <summary> 年月 </summary>
    public DateTime YearMonth { get; set; }

    /// <summary> デフォルト設定か </summary>
    public bool IsDefault { get; set; }

    /// <summary> 作成日 </summary>
    public DateTime CreateDate { get; }

    /// <summary> 更新日 </summary>
    public DateTime UpdateDate { get; }
}
