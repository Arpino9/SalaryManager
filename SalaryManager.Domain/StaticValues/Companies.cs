namespace SalaryManager.Domain.StaticValues;

/// <summary>
/// Static Values - 会社
/// </summary>
public static class Companies
{
    private static List<CompanyEntity> _entities = new List<CompanyEntity>();

    /// <summary>
    /// テーブル取得
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <remarks>
    /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
    /// </remarks>
    public static void Create(ICompanyRepository repository)
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
                throw new DatabaseException("会社テーブルの読込に失敗しました。", ex);
            }
        }
    }

    /// <summary>
    /// 会社を取得
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>職歴</returns>
    public static CompanyEntity Fetch(int id)
        => _entities?.Find(x => x.ID == id);

    /// <summary>
    /// 昇順で取得する
    /// </summary>
    /// <returns>職歴</returns>
    public static IReadOnlyList<CompanyEntity> FetchByAscending()
        => _entities?.OrderBy(x => x.ID).ToList().AsReadOnly();

    /// <summary>
    /// 降順で取得する
    /// </summary>
    /// <returns>職歴</returns>
    public static IReadOnlyList<CompanyEntity> FetchByDescending()
        => _entities?.OrderByDescending(x => x.ID).ToList().AsReadOnly();
}
