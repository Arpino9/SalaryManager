namespace SalaryManager.Domain.StaticValues;

/// <summary>
/// Static Values - 副業
/// </summary>
public static class SideBusinesses
{
    private static List<SideBusinessEntity> _entities = new List<SideBusinessEntity>();

    private static SideBusinessEntity _default;

    /// <summary>
    /// テーブル取得
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <remarks>
    /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
    /// </remarks>
    public static void Create(ISideBusinessRepository repository)
    {
        lock (((ICollection)_entities).SyncRoot)
        {
            _entities.Clear();

            try
            {
                _entities.AddRange(repository.GetEntities());

                _default = repository.GetDefault();
            } 
            catch(SqlException ex) 
            {
                throw new DatabaseException("副業テーブルの読込に失敗しました。", ex);
            }
        }
    }

    /// <summary>
    /// 副業額を取得
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <returns>副業額</returns>
    public static SideBusinessEntity Fetch(int year, int month)
        => _entities.Find(x => x.YearMonth.Year == year && x.YearMonth.Month == month);

    /// <summary>
    /// 昇順で取得する
    /// </summary>
    /// <returns>副業額</returns>
    public static IReadOnlyList<SideBusinessEntity> FetchByAscending()
        => _entities.OrderBy(x => x.YearMonth).ToList().AsReadOnly();

    /// <summary>
    /// 降順で取得する
    /// </summary>
    /// <returns>副業額</returns>
    public static IReadOnlyList<SideBusinessEntity> FetchByDescending()
        => _entities.OrderByDescending(x => x.YearMonth).ToList().AsReadOnly();

    /// <summary>
    /// デフォルト明細を取得する
    /// </summary>
    /// <returns>デフォルト明細</returns>
    public static SideBusinessEntity FetchDefault()
        => _default;
}
