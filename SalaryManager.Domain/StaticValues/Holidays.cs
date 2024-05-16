namespace SalaryManager.Domain.StaticValues;

/// <summary>
/// Static Values - 休祝日
/// </summary>
public static class Holidays
{
    private static List<HolidayEntity> _entities = new List<HolidayEntity>();

    /// <summary>
    /// テーブル取得
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <remarks>
    /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
    /// </remarks>
    public static void Create(IHolidayRepository repository)
    {
        lock (((ICollection)_entities).SyncRoot)
        {
            _entities.Clear();
            _entities.AddRange(repository.GetEntities());
        }
    }

    /// <summary>
    /// 昇順で取得する
    /// </summary>
    /// <returns>職歴</returns>
    public static IReadOnlyList<HolidayEntity> FetchByAscending()
        => _entities?.OrderBy(x => x.Date).ToList().AsReadOnly();

    /// <summary>
    /// 降順で取得する
    /// </summary>
    /// <returns>職歴</returns>
    public static IReadOnlyList<HolidayEntity> FetchByDescending()
        => _entities?.OrderByDescending(x => x.Date).ToList().AsReadOnly();
}
