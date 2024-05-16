namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - 会社
/// </summary>
public interface ICompanyRepository
{
    /// <summary> 
    /// Get - 会社
    /// </summary>
    /// <returns>Entity - 会社</returns>
    IReadOnlyList<CompanyEntity> GetEntities();

    /// <summary>
    /// Get - 会社
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>支給額</returns>
    CompanyEntity GetEntity(int id);

    public void Save(
        ITransactionRepository transaction,
        CompanyEntity entity);

    public void SaveAddress(
        ITransactionRepository transaction,
        CompanyEntity entity);

    /// <summary>
    /// 削除
    /// </summary>
    /// <param name="id">ID</param>
    public void Delete(int id);
}
