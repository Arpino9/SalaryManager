namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - 月収一覧
/// </summary>
public interface IAnnualChartRepository
{
    /// <summary> 
    /// Get - 支給額
    /// </summary>
    /// <returns>Entity - 支給額</returns>
    IReadOnlyList<AnnualChartEntity> GetEntities();
}
