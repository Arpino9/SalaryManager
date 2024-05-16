namespace SalaryManager.Domain.Repositories;

/// <summary>
/// Repository - 休祝日
/// </summary>
public interface IHolidayRepository
{
    /// <summary> 
    /// Get - 休祝日
    /// </summary>
    /// <returns>Entity - 休祝日</returns>
    IReadOnlyList<HolidayEntity> GetEntities();

    /// <summary>
    /// Get - 休祝日
    /// </summary>
    /// <returns>休祝日</returns>
    HolidayEntity GetEntity(int id);

    public void Save(HolidayEntity entity);

    /// <summary>
    /// 削除
    /// </summary>
    /// <param name="id">ID</param>
    public void Delete(int id);
}
