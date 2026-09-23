namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - 保有スキル
/// </summary>
public class ViewModel_ExperiencedSkill : ViewModelBase<Model_ExperiencedSkill>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    /// <summary> Model - 保有スキル </summary>
    protected override Model_ExperiencedSkill Model { get; }
        = Model_ExperiencedSkill.GetInstance(new ProjectSQLite());

    public ViewModel_ExperiencedSkill()
    {
        this.Model.ViewModel = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    protected override void BindEvents()
    {

    }

    #region Window

    /// <summary> Window - FontFamily </summary>
    public ReactiveProperty<FontFamily> Window_FontFamily { get; set; }
        = new ReactiveProperty<FontFamily>();

    /// <summary> Window - FontSize </summary>
    public ReactiveProperty<decimal> Window_FontSize { get; set; }
        = new ReactiveProperty<decimal>();

    /// <summary> Window - Background </summary>
    public ReactiveProperty<Brush> Window_Background { get; set; }
        = new ReactiveProperty<Brush>();

    /// <summary> Window - Title </summary>
    public ReactiveProperty<string> Window_Title { get; }
        = new ReactiveProperty<string>("保有スキル一覧");

    /// <summary> Window - Activated </summary>
    public ReactiveCommand Window_Activated { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 言語

    /// <summary> 言語 - ItemSource </summary>
    public ReactiveCollection<ExperiencedLanguages> ExperiencedLanguages_ItemSource { get; set; }
        = new ReactiveCollection<ExperiencedLanguages>();

    /// <summary> 言語 - SelectedIndex </summary>
    public ReactiveProperty<int> ExperiencedLanguages_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region DB

    /// <summary> DB - ItemSource </summary>
    public ReactiveCollection<ExperiencedDatabases> ExperiencedDatabases_ItemSource { get; set; }
        = new ReactiveCollection<ExperiencedDatabases>();

    /// <summary> DB - SelectedIndex </summary>
    public ReactiveProperty<int> ExperiencedDatabases_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 開発支援ツール

    /// <summary> 開発支援ツール - ItemSource </summary>
    public ReactiveCollection<ExperiencedDevelopmentSupportingTools> ExperiencedDevelopmentSupportingTools_ItemSource { get; set; }
        = new ReactiveCollection<ExperiencedDevelopmentSupportingTools>();

    /// <summary> 開発支援ツール - SelectedIndex </summary>
    public ReactiveProperty<int> ExperiencedDevelopmentSupportingTools_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region その他のツール

    /// <summary> その他のツール - ItemSource </summary>
    public ReactiveCollection<ExperiencedOtherTools> ExperiencedOtherTools_ItemSource { get; set; }
        = new ReactiveCollection<ExperiencedOtherTools>();

    /// <summary> その他のツール - SelectedIndex </summary>
    public ReactiveProperty<int> ExperiencedOtherTools_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    #endregion

}

public class ExperiencedLanguages(string lauguage, string year)
{
    public string Language { get; set; } = lauguage;
    public string Year { get; set; } = year;
}

public class ExperiencedDatabases(string lauguage, string year)
{
    public string Database { get; set; } = lauguage;
    public string Year { get; set; } = year;
}

public class ExperiencedDevelopmentSupportingTools(string tool, string year)
{
    public string Tool { get; set; } = tool;
    public string Year { get; set; } = year;
}

public class ExperiencedOtherTools(string tool, string year)
{
    public string Tool { get; set; } = tool;
    public string Year { get; set; } = year;
}