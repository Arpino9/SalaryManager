namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - 副業
/// </summary>
public interface ISideBusinessRepository
{
    /// <summary>
    /// Get - 副業
    /// </summary>
    /// <returns>Entity - 副業</returns>
    IReadOnlyList<SideBusinessEntity> GetEntities();


    SideBusinessEntity GetEntity(int year, int month);
    SideBusinessEntity GetDefault();

    public void Save(ITransactionRepository transaction, SideBusinessEntity entity);
}
