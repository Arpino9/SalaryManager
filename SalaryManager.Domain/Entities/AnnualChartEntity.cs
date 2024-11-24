namespace SalaryManager.Domain.Entities;

/// <summary>
/// 月収一覧
/// </summary>
/// <param name="id">ID</param>
/// <param name="yearMonth">年月</param>
/// <param name="totalSalary">支給額計</param>
/// <param name="totalDeductedSalary">差引支給額</param>
/// <param name="totalSideBusiness">副業額</param>
public class AnnualChartEntity(
    int id,
    DateTime yearMonth,
    int totalSalary,
    int totalDeductedSalary,
    int totalSideBusiness)
{
    /// <summary> ID </summary>
    public int Id => id;

    /// <summary> 月 </summary>
    public DateTime YearMonth => yearMonth;

    /// <summary> 支給額計 </summary>
    public int TotalSalary => totalSalary;

    /// <summary> 差引支給額 </summary>
    public int TotalDeducetedSalary => totalDeductedSalary;

    /// <summary> 副業額 </summary>
    public int TotalSideBusiness => totalSideBusiness;
}