namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - 就業場所
/// </summary>
public interface IWorkingPlaceRepository
{
    /// <summary> 
    /// Get - 就業場所
    /// </summary>
    /// <returns>Entity - 就業場所</returns>
    IReadOnlyList<WorkingPlaceEntity> GetEntities();

    /// <summary>
    /// Get - 就業場所
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>就業場所</returns>
    WorkingPlaceEntity GetEntity(int id);

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="entity">就業場所</param>
    public void Save(WorkingPlaceEntity entity);

    /// <summary>
    /// 会社名を更新
    /// </summary>
    /// <param name="transaction">トランザクション</param>
    /// <param name="oldName">更新前の名称</param>
    /// <param name="newName">更新後の名称</param>
    public void UpdateCompanyName(ITransactionRepository transaction, string oldName, string newName);

    /// <summary>
    /// 会社の住所を更新
    /// </summary>
    /// <param name="oldAddress">更新前の住所</param>
    /// <param name="newAddress">更新後の住所</param>
    public void UpdateCompanyAddress(string oldAddress, string newAddress);

    /// <summary>
    /// 削除
    /// </summary>
    /// <param name="id">ID</param>
    public void Delete(int id);
}
