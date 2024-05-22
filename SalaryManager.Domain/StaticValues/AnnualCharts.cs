namespace SalaryManager.Domain.StaticValues;

/// <summary>
/// Static Values - 月収一覧
/// </summary>
public static class AnnualCharts
{
    private static List<AnnualChartEntity> _entities = new List<AnnualChartEntity>();

    /// <summary>
    /// テーブル取得
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <remarks>
    /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
    /// </remarks>
    public static void Create(IAnnualChartRepository repository)
    {
        lock (((ICollection)_entities).SyncRoot)
        {
            _entities.Clear();

            try
            {
                _entities.AddRange(repository.GetEntities());
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("月収一覧の算出に失敗しました。", ex);
            }
        }
    }

    /// <summary>
    /// 月収一覧を取得
    /// </summary>
    /// <param name="year">年</param>
    /// <returns>月収一覧</returns>
    public static List<AnnualChartEntity> Fetch(int year)
        => _entities.Where(x => x.YearMonth.Year == year).ToList();

    /// <summary>
    /// 月収一覧を取得
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <returns>月収一覧</returns>
    public static AnnualChartEntity Fetch(int year, int month)
        => _entities.Find(x => x.YearMonth.Year  == year && 
                               x.YearMonth.Month == month);

    /// <summary>
    /// 昇順で取得する
    /// </summary>
    /// <returns>月収一覧</returns>
    public static IReadOnlyList<AnnualChartEntity> FetchByAscending()
        => _entities.OrderBy(x => x.YearMonth).ToList().AsReadOnly();

    /// <summary>
    /// 降順で取得する
    /// </summary>
    /// <returns>月収一覧</returns>
    public static IReadOnlyList<AnnualChartEntity> FetchByDescending()
        => _entities.OrderByDescending(x => x.YearMonth).ToList().AsReadOnly();
}
