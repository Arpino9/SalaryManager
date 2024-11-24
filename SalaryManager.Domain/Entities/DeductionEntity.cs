namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 控除額
/// </summary>
/// <param name="id">ID</param>
/// <param name="yearMonth">年月</param>
/// <param name="healthInsurance">健康保険</param>
/// <param name="nursingInsurance">介護保険</param>
/// <param name="welfareAnnuity">厚生年金</param>
/// <param name="employmentInsurance">雇用保険</param>
/// <param name="incomeTax">所得税</param>
/// <param name="municipalTax">市町村税</param>
/// <param name="friendshipAssociation">互助会</param>
/// <param name="yearEndTaxAdjustment">年末調整他</param>
/// <param name="remarks">備考</param>
/// <param name="totalDeduct">控除額計</param>
public sealed class DeductionEntity(
    int id,
    DateTime yearMonth,
    double healthInsurance,
    double nursingInsurance,
    double welfareAnnuity,
    double employmentInsurance,
    double incomeTax,
    double municipalTax,
    double friendshipAssociation,
    double yearEndTaxAdjustment,
    string remarks,
    double totalDeduct)
{
    /// <summary> ID </summary>
    public int ID => id;

    /// <summary> 年月 </summary>
    public DateTime YearMonth => yearMonth;

    /// <summary> 健康保険 </summary>
    public MoneyValue HealthInsurance => new MoneyValue(healthInsurance);

    /// <summary> 介護保険 </summary>
    public MoneyValue NursingInsurance => new MoneyValue(nursingInsurance);

    /// <summary> 厚生年金 </summary>
    public MoneyValue WelfareAnnuity => new MoneyValue(welfareAnnuity);

    /// <summary> 雇用保険 </summary>
    public MoneyValue EmploymentInsurance => new MoneyValue(employmentInsurance);

    /// <summary> 所得税 </summary>
    public MoneyValue IncomeTax => new MoneyValue(incomeTax);

    /// <summary> 市町村税 </summary>
    public MoneyValue MunicipalTax => new MoneyValue(municipalTax);

    /// <summary> 互助会 </summary>
    public MoneyValue FriendshipAssociation => new MoneyValue(friendshipAssociation);

    /// <summary> 年末調整他 </summary>
    public double YearEndTaxAdjustment => yearEndTaxAdjustment;

    /// <summary> 備考 </summary>
    public string Remarks => remarks;

    /// <summary> 控除額計 </summary>
    public MoneyValue TotalDeduct => new MoneyValue(totalDeduct);
}
