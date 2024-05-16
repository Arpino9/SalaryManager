namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 会社
/// </summary>
public class CompanyEntity
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="middleCategoryNo">業種</param>
    /// <param name="companyName">会社名</param>
    /// <param name="postCode">郵便番号</param>
    /// <param name="address">住所</param>
    /// <param name="address_Google">住所(Google Map)</param>
    /// <param name="remarks">備考</param>
    public CompanyEntity(
        int id,
        int middleCategoryNo,
        string companyName,
        string postCode,
        string address,
        string address_Google,
        string remarks)
    {
        this.ID               = id;
        this.BusinessCategory = new BusinessCategoryValue(middleCategoryNo);
        this.CompanyName      = companyName;
        this.PostCode         = postCode;
        this.Address          = address;
        this.Address_Google   = address_Google;
        this.Remarks          = remarks;
    }

    /// <summary> ID </summary>
    public int ID { get; }

    /// <summary> 業種 </summary>
    public BusinessCategoryValue BusinessCategory { get; }

    /// <summary> 会社名 </summary>
    public string CompanyName { get; }

    /// <summary> 郵便番号 </summary>
    public string PostCode { get; }

    /// <summary> 住所 </summary>
    public string Address { get; }

    /// <summary> 住所(Google Map) </summary>
    public string Address_Google { get; }

    /// <summary> 備考 </summary>
    public string Remarks { get; }
}
