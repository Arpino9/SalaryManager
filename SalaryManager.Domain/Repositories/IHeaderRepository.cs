namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - ヘッダ
/// </summary>
public interface IHeaderRepository
{
    /// <summary>
    /// Get - ヘッダ
    /// </summary>
    /// <returns>Entity - 副業</returns>
    IReadOnlyList<HeaderEntity> GetEntities();

    HeaderEntity FetchDefault();

    public void Save(HeaderEntity entity);

    public void Save(ITransactionRepository transaction, HeaderEntity entity);
}
