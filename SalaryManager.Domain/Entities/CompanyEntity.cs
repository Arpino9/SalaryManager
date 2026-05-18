namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 会社
/// </summary>
/// <param name="id">ID</param>
/// <param name="middleCategoryNo">業種</param>
/// <param name="companyName">会社名</param>
/// <param name="postCode">郵便番号</param>
/// <param name="address">住所</param>
/// <param name="address_Google">住所(Google Map)</param>
/// <param name="remarks">備考</param>
public class CompanyEntity(
    int id,
    int middleCategoryNo,
    string companyName,
    string postCode,
    string address,
    string address_Google,
    string remarks) : IEntity
{
    /// <summary> ID </summary>
    public int ID => id;

    /// <summary> 業種 </summary>
    public BusinessCategoryValue BusinessCategory => new BusinessCategoryValue(middleCategoryNo);

    /// <summary> 会社名 </summary>
    public string CompanyName => companyName;

    /// <summary> 郵便番号 </summary>
    public string PostCode => postCode;

    /// <summary> 住所 </summary>
    public string Address => address;

    /// <summary> 住所(Google Map) </summary>
    public string Address_Google => address_Google;

    /// <summary> 備考 </summary>
    public string Remarks => remarks;
}
