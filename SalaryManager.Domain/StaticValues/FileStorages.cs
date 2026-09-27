namespace SalaryManager.Domain.StaticValues;

/// <summary>
/// Static Values - 添付ファイル
/// </summary>
public class FileStorages
{
    private static List<FileStorageEntity> _entities = new List<FileStorageEntity>();

    /// <summary>
    /// テーブル取得
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <remarks>
    /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
    /// </remarks>
    public static void Create(IFileStorageRepository repository)
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
                throw new DatabaseException("支給額テーブルの読込に失敗しました。", ex);
            }
        }
    }

    /// <summary>
    /// 支給額を取得
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <returns>支給額</returns>
    public static FileStorageEntity Fetch(int year, int month)
        => _entities.Find(x => x.UpdateDate.Year  == year && 
                               x.UpdateDate.Month == month);

    /// <summary>
    /// 昇順で取得する
    /// </summary>
    /// <returns>支給額</returns>
    public static IReadOnlyList<FileStorageEntity> FetchByAscending()
        => _entities.OrderBy(x => x.UpdateDate).ToList().AsReadOnly();

    /// <summary>
    /// 降順で取得する
    /// </summary>
    /// <returns>支給額</returns>
    public static IReadOnlyList<FileStorageEntity> FetchByDescending()
        => _entities.OrderByDescending(x => x.UpdateDate).ToList().AsReadOnly();
}
