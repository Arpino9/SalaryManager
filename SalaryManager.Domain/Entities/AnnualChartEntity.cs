namespace SalaryManager.Domain.Entities;

/// <summary>
/// 月収一覧
/// </summary>
public class AnnualChartEntity
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="yearMonth">年月</param>
    /// <param name="totalSalary">支給額計</param>
    /// <param name="totalDeductedSalary">差引支給額</param>
    /// <param name="totalSideBusiness">副業額</param>
    public AnnualChartEntity(
        int id,
        DateTime yearMonth,
        int totalSalary,
        int totalDeductedSalary,
        int totalSideBusiness)
    {
        this.Id                   = id;
        this.YearMonth            = yearMonth;
        this.TotalSalary          = totalSalary;
        this.TotalDeducetedSalary = totalDeductedSalary;
        this.TotalSideBusiness    = totalSideBusiness;
    }

    /// <summary> ID </summary>
    public int Id { get; }
        
    /// <summary> 月 </summary>
    public DateTime YearMonth { get; }

    /// <summary> 支給額計 </summary>
    public int TotalSalary { get; }

    /// <summary> 差引支給額 </summary>
    public int TotalDeducetedSalary { get; }

    /// <summary> 副業額 </summary>
    public int TotalSideBusiness { get; }
}