namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - 支給額
/// </summary>
public interface IAllowanceRepository
{
    /// <summary> 
    /// Get - 支給額
    /// </summary>
    /// <returns>Entity - 支給額</returns>
    IReadOnlyList<AllowanceValueEntity> GetEntities();

    /// <summary>
    /// Get - 支給額
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <returns>支給額</returns>
    AllowanceValueEntity GetEntity(int year, int month);

    /// <summary>
    /// Get - デフォルト明細
    /// </summary>
    /// <returns></returns>
    AllowanceValueEntity GetDefault();

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="transaction">トランザクション</param>
    /// <param name="entity">エンティティ</param>
    public void Save(ITransactionRepository transaction, AllowanceValueEntity entity);
}
