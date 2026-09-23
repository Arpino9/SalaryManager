namespace SalaryManager.Domain.StaticValues;

public static class Projects
{
    private static List<ProjectEntity> _entities = new List<ProjectEntity>();

    /// <summary>
    /// テーブル取得
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <remarks>
    /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
    /// </remarks>
    public static void Create(IProjectRepository repository)
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
                throw new DatabaseException("プロジェクト一覧の取得に失敗しました。", ex);
            }
        }
    }

    /// <summary>
    /// プロジェクト一覧を取得
    /// </summary>
    /// <param name="companyName">会社名</param>
    /// <returns>プロジェクト一覧</returns>
    public static IReadOnlyList<ProjectEntity> FetchByDescending(string companyName)
        => _entities.Where(x => x.CompanyName == companyName).OrderByDescending(x => x.StartDate.Value).ToList();

    /// <summary>
    /// プロジェクトの合計数を取得
    /// </summary>
    /// <returns>プロジェクトの合計数</returns>
    public static int FetchTotalProjectsCount()
        => _entities.Count();
}
