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

    /// <summary>
    /// 指定した言語を使用したプロジェクト一覧を取得
    /// </summary>
    /// <param name="language">言語</param>
    /// <returns>プロジェクト一覧</returns>
    public static IReadOnlyList<ProjectEntity> FetchProjectsWithExperienceLanguage(string language)
        => _entities.Where(x => x.Language.Contains(language)).ToList();

    /// <summary>
    /// 指定したデータベースを使用したプロジェクト一覧を取得
    /// </summary>
    /// <param name="database">データベース</param>
    /// <returns>プロジェクト一覧</returns>
    public static IReadOnlyList<ProjectEntity> FetchProjectsWithExperienceDatabase(string database)
        => _entities.Where(x => x.Database.Contains(database)).ToList();
    
    /// <summary>
    /// 指定した開発支援ツールを使用したプロジェクト一覧を取得
    /// </summary>
    /// <param name="tool">ツール</param>
    /// <returns>プロジェクト一覧</returns>
    public static IReadOnlyList<ProjectEntity> FetchProjectsWithExperienceDevelopmentSupportingTool(string tool)
        => _entities.Where(x => x.DevelopmentSupportingTool.Contains(tool)).ToList();
    
    /// <summary>
    /// 指定した他のツールを使用したプロジェクト一覧を取得
    /// </summary>
    /// <param name="tool">ツール</param>
    /// <returns>プロジェクト一覧</returns>
    public static IReadOnlyList<ProjectEntity> FetchProjectsWithExperienceOtherTool(string tool)
        => _entities.Where(x => x.OtherTools.Contains(tool)).ToList();

    /// <summary>
    /// 使用した言語一覧を取得
    /// </summary>
    /// <returns>言語一覧</returns>
    public static IReadOnlyList<string> FetchExperienceLanguages()
    {
        var languages = new List<string>();

        _entities.Select(x => x.Language)
                 .ToList()
                 .ForEach(x =>
                 {
                     if (x.Contains(','))
                     {
                         x.Split(',').ToList().ForEach(lang => languages.Add(lang));
                     }
                     else
                     {
                         languages.Add(x);
                     }
                 });

        return languages.Distinct().ToList();
    }

    /// <summary>
    /// 使用したデータベース一覧を取得
    /// </summary>
    /// <returns>データベース一覧</returns>
    public static IReadOnlyList<string> FetchExperienceDatabases()
    {
        var databases = new List<string>();

        _entities.Select(x => x.Database)
                 .ToList()
                 .ForEach(x =>
                 {
                     if (x.Contains(','))
                     {
                         x.Split(',').ToList().ForEach(lang => databases.Add(lang));
                     }
                     else
                     {
                         databases.Add(x);
                     }
                 });

        return databases.Distinct().ToList();
    }

    /// <summary>
    /// 使用した開発支援ツール一覧を取得
    /// </summary>
    /// <returns>開発支援ツール一覧</returns>
    public static IReadOnlyList<string> FetchExperienceDevelopmentSupportingTools()
    {
        var tools = new List<string>();

        _entities.Select(x => x.DevelopmentSupportingTool)
                 .ToList()
                 .ForEach(x =>
                 {
                     if (x.Contains(','))
                     {
                         x.Split(',').ToList().ForEach(lang => tools.Add(lang));
                     }
                     else
                     {
                         tools.Add(x);
                     }
                 });

        return tools.Distinct().ToList();
    }

    /// <summary>
    /// 使用したその他のツール一覧を取得
    /// </summary>
    /// <returns>その他のツール一覧</returns>
    public static IReadOnlyList<string> FetchExperienceOtherTools()
    {
        var tools = new List<string>();

        _entities.Select(x => x.OtherTools)
                 .ToList()
                 .ForEach(x =>
                 {
                     if (x.Contains(','))
                     {
                         x.Split(',').ToList().ForEach(lang => tools.Add(lang));
                     }
                     else
                     {
                         tools.Add(x);
                     }
                 });

        return tools.Distinct().ToList();
    }
}
