namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 自宅
/// </summary>
/// <param name="id">ID</param>
/// <param name="displayName">名称</param>
/// <param name="livingStart">在住開始日</param>
/// <param name="livingEnd">在住終了日</param>
/// <param name="isLiving">在住中か</param>
/// <param name="postCode">郵便番号</param>
/// <param name="address">住所</param>
/// <param name="address_google">住所</param>
/// <param name="remarks">備考</param>
[Table("Home")]
public sealed record class HomeEntity(
    int id,
    string displayName,
    DateTime? livingStart,
    DateTime? livingEnd,
    bool isLiving,
    string postCode,
    string address,
    string address_google,
    string remarks) : IEntity
{
    /// <summary> ID </summary>
    [Column("ID")]
    public int ID => id;

    /// <summary> 名称 </summary>
    [Column("DisplayName")]
    public string DisplayName => displayName;

    /// <summary> 郵便番号 </summary>
    [Column("PostCode")]
    public string PostCode => postCode;

    /// <summary> 在住開始日 </summary>
    [Column("LivingStart")]
    public DateTime? LivingStart => livingStart;

    /// <summary> 在住終了日 </summary>
    [Column("LivingEnd")]
    public DateTime? LivingEnd => this.IsLiving ? DateTime.Today : livingEnd;

    /// <summary> 在住中か </summary>
    [Column("IsLiving")]
    public bool IsLiving => isLiving;

    /// <summary> 住所 </summary>
    [Column("Address")]
    public string Address => address;

    /// <summary> 住所 </summary>
    [Column("Address_Google")]
    public string Address_Google => address_google;

    /// <summary> 備考 </summary>
    [Column("Remarks")]
    public string Remarks => remarks;
}
