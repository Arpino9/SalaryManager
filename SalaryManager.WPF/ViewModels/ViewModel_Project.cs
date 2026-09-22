namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - プロジェクト
/// </summary>
internal class ViewModel_Project : ViewModelBase<Model_Project>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    protected override Model_Project Model => Model_Project.GetInstance(new ProjectSQLite());

    public ViewModel_Project()
    {
        this.Model.ViewModel = this;
        this.Model.Initialize();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        this.Ongoing_Checked.Subscribe(_ => this.Model.IsOngoing_Checked());

        // プロジェクト一覧
        this.Projects_SelectionChanged.Subscribe(_ => this.Model.ListView_SelectionChanged());
        this.ProjectName_TextChanged.Subscribe(_ => this.Model.EnableAddButton());

        // 追加
        this.Add_Command.Subscribe(_ => this.Model.AddAsync());

        // 更新
        this.Update_Command.Subscribe(_ => this.Model.UpdateAsync());

        // 削除
        this.Delete_Command.Subscribe(_ => this.Model.DeleteAsync());
    }

    internal ViewModel_Career ViewModel_Career { get; set; }

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
        = new ReactiveProperty<string>("プロジェクト編集");

    /// <summary> Window - Activated </summary>
    public ReactiveCommand Window_Activated { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region プロジェクト一覧

    /// <summary> プロジェクト一覧 - ItemSource </summary>
    public ReactiveCollection<ProjectEntity> Projects_ItemSource { get; set; }
        = new ReactiveCollection<ProjectEntity>();

    /// <summary> プロジェクト一覧 - SelectionChanged </summary>
    public ReactiveCommand Projects_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    /// <summary> プロジェクト - SelectedIndex </summary>
    public ReactiveProperty<int> Projects_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> プロジェクト名 - TextChanged </summary>
    public ReactiveCommand ProjectName_TextChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 会社名

    /// <summary> 会社名 - Text </summary>
    public ReactiveProperty<string> CompanyName_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> プロジェクト名 - Text </summary>
    public ReactiveProperty<string> ProjectName_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 担当期間

    /// <summary> 担当期間 - 開始日 - Text </summary>
    public ReactiveProperty<DateTime?> StartDate_SelectedDate { get; set; }
        = new ReactiveProperty<DateTime?>();

    /// <summary> 担当期間 - 終了日 - Text </summary>
    public ReactiveProperty<DateTime?> EndDate_SelectedDate { get; set; }
        = new ReactiveProperty<DateTime?>();

    /// <summary> 担当期間 - 終了日 - IsEnabled </summary>
    public ReactiveProperty<bool> EndDate_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    #endregion

    #region 進行中

    /// <summary> 進行中 - IsEnabled </summary>
    public ReactiveProperty<bool> Ongoing_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 進行中 - Checked </summary>
    public ReactiveCommand Ongoing_Checked { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region システム概要

    /// <summary> システム名 - Text </summary>
    public ReactiveProperty<string> SystemName_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 言語 - Text </summary>
    public ReactiveProperty<string> Language_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> データベース - Text </summary>
    public ReactiveProperty<string> Database_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 開発支援ツール - Text </summary>
    public ReactiveProperty<string> DevelopmentSupporting_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> その他ツール - Text </summary>
    public ReactiveProperty<string> OtherTools_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region メンバー

    /// <summary> 担当 - Text </summary>
    public ReactiveProperty<string> Role_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> メンバー数 - Text </summary>
    public ReactiveProperty<string> Member_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 担当工程

    /// <summary> 担当工程 - 要件定義 - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_RequireDefinition_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 担当工程 - 基本設計 - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_BasicDesign_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 担当工程 - 詳細設計 - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_DetailDesign_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 担当工程 - 開発 - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_Development_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 担当工程 - 単体テスト - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_UnitTest_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 担当工程 - 結合テスト - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_IntegrationTest_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 担当工程 - システムテスト - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_SystemTest_IsChecked { get; set; }
        = new ReactiveProperty<bool>();
    
    /// <summary> 担当工程 - 運用テスト - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_OperationTest_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 担当工程 - その他 - IsChecked </summary>
    public ReactiveProperty<bool> AssignedPhrase_Other_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    #endregion

    #region 業務詳細

    /// <summary> 業務内容 - Text </summary>
    public ReactiveProperty<string> Contents_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public ReactiveProperty<string> Remarks_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 追加

    /// <summary> 追加 - IsEnabled </summary>
    public ReactiveProperty<bool> Add_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 追加 - Command </summary>
    public ReactiveCommand Add_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 更新

    /// <summary> 更新 - IsEnabled </summary>
    public ReactiveProperty<bool> Update_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 更新 - Command </summary>
    public ReactiveCommand Update_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 削除

    /// <summary> 削除 - IsEnabled </summary>
    public ReactiveProperty<bool> Delete_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();
    
    /// <summary> 削除 - Command </summary>
    public ReactiveCommand Delete_Command { get; private set; }
        = new ReactiveCommand();

    #endregion
}
