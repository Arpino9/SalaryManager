namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - 控除額
/// </summary>
public interface IDeductionRepository
{
    /// <summary>
    /// Get - 控除額
    /// </summary>
    /// <returns>Entity - 控除額</returns>
    IReadOnlyList<DeductionEntity> GetEntities();

    /// <summary>
    /// Get - 控除額
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <returns>控除額</returns>
    DeductionEntity GetEntity(int year, int month);

    /// <summary>
    /// デフォルト明細を取得する
    /// </summary>
    /// <returns>デフォルト明細</returns>
    DeductionEntity GetDefault();

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="transaction">トランザクション</param>
    /// <param name="entity">エンティティ</param>
    public void Save(ITransactionRepository transaction, DeductionEntity entity);
}
