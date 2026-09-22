using MahApps.Metro.Controls.Dialogs;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - プロジェクト
/// </summary>
internal class Model_Project : ModelBase<ViewModel_Project>, IEditableMaster
{
    #region Get Instance

    private static Model_Project model = null;

    public static Model_Project GetInstance(IProjectRepository repository)
    {
        if (model == null)
        {
            model = new Model_Project(repository);
        }

        return model;
    }

    #endregion

    /// <summary> ViewModel - プロジェクト </summary>
    internal override ViewModel_Project ViewModel { get; set; }

    public IProjectRepository _repository { get; }

    public Model_Project(IProjectRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        this.Window_Activated();

        this.Reload();

        this.ListView_SelectionChanged();
    }

    public void Window_Activated()
    {
        this.ViewModel.Window_FontFamily.Value = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
        this.ViewModel.Window_FontSize.Value = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background.Value = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
    }

    /// <summary>
    /// ListView - SelectionChanged
    /// </summary>
    public void ListView_SelectionChanged()
    {
        if (this.ViewModel.Projects_SelectedIndex.Value.IsUnSelected())
        {
            return;
        }

        this.EnableControlButton();
        this.ViewModel.Window_Title.Value = Model_WorkingPlace.SelectedCompanyName;
        this.ViewModel.CompanyName_Text.Value = Model_WorkingPlace.SelectedCompanyName;

        if (!this.ViewModel.Projects_ItemSource.Any())
        {
            return;
        }

        var entity = this.ViewModel.Projects_ItemSource[this.ViewModel.Projects_SelectedIndex.Value];

        // 名称
        this.ViewModel.ProjectName_Text.Value = entity.ProjectName;
        // プロジェクト期間
        this.ViewModel.StartDate_SelectedDate.Value = entity.StartDate.Value;
        this.ViewModel.EndDate_SelectedDate.Value = entity.EndDate.Value;
        // 進行中か
        this.ViewModel.Ongoing_IsChecked.Value = entity.EndDate.IsWorking;
        this.IsOngoing_Checked();
        // システム名
        this.ViewModel.SystemName_Text.Value = entity.SystemName;
        // 言語
        this.ViewModel.Language_Text.Value = entity.Language;
        // データベース
        this.ViewModel.Database_Text.Value = entity.Database;
        // 開発支援ツール
        this.ViewModel.DevelopmentSupporting_Text.Value = entity.DevelopmentSupportingTool;
        // その他ツール
        this.ViewModel.OtherTools_Text.Value = entity.OtherTools;
        // 担当
        this.ViewModel.Role_Text.Value = entity.Role;
        // メンバー
        this.ViewModel.Member_Text.Value = entity.Member;

        this.ViewModel.AssignedPhrase_RequireDefinition_IsChecked.Value = entity.AssignedPhrase_RequireDefinition.Value;
        this.ViewModel.AssignedPhrase_BasicDesign_IsChecked.Value = entity.AssignedPhrase_BasicDesign.Value;
        this.ViewModel.AssignedPhrase_DetailDesign_IsChecked.Value = entity.AssignedPhrase_DetailDesign.Value;
        this.ViewModel.AssignedPhrase_Development_IsChecked.Value = entity.AssignedPhrase_Development.Value;
        this.ViewModel.AssignedPhrase_UnitTest_IsChecked.Value = entity.AssignedPhrase_UnitTest.Value;
        this.ViewModel.AssignedPhrase_IntegrationTest_IsChecked.Value = entity.AssignedPhrase_IntegrationTest.Value;
        this.ViewModel.AssignedPhrase_SystemTest_IsChecked.Value = entity.AssignedPhrase_SystemTest.Value;
        this.ViewModel.AssignedPhrase_OperationTest_IsChecked.Value = entity.AssignedPhrase_OperationTest.Value;
        this.ViewModel.AssignedPhrase_Other_IsChecked.Value = entity.AssignedPhrase_Other.Value;
        // 業務内容
        this.ViewModel.Contents_Text.Value = entity.Contents;
        // 備考
        this.ViewModel.Remarks_Text.Value = entity.Remarks;
    }

    /// <summary>
    /// Enable - 追加ボタン
    /// </summary>
    public void EnableAddButton()
        => this.ViewModel.Add_IsEnabled.Value = !string.IsNullOrEmpty(this.ViewModel.ProjectName_Text.Value);

    /// <summary>
    /// Enable - 操作ボタン
    /// </summary>
    /// <remarks>
    /// 追加ボタンは「会社名」に値があれば押下可能。
    /// </remarks>
    private void EnableControlButton()
    {
        var selected = this.ViewModel.Projects_ItemSource.Any()
                    && this.ViewModel.Projects_SelectedIndex.Value >= 0;

        // 更新ボタン
        this.ViewModel.Update_IsEnabled.Value = selected;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled.Value = selected;
    }

    /// <summary>
    /// 進行中か - Checked
    /// </summary>
    public void IsOngoing_Checked()
    {
        this.ViewModel.EndDate_IsEnabled.Value = (this.ViewModel.Ongoing_IsChecked.Value == false);

        if (this.ViewModel.Ongoing_IsChecked.Value)
        {
            this.ViewModel.EndDate_SelectedDate.Value = DateTime.Today;
        }
    }

    /// <summary>
    /// 再描画
    /// </summary>
    public void Reload()
    {
        using (var cursor = new CursorWaiting())
        {
            // ListView
            this.Reload_ListView();

            // 入力用フォーム
            this.Reload_InputForm();
        }
    }

    /// <summary>
    /// 再描画 - ListView
    /// </summary>
    private void Reload_ListView()
    {
        Projects.Create(_repository);

        var entities = Projects.FetchByDescending(Model_WorkingPlace.SelectedCompanyName);

        if (entities.IsEmpty())
        {
            return;
        }

        this.ViewModel.Projects_ItemSource.Clear();

        foreach (var entity in entities)
        {
            this.ViewModel.Projects_ItemSource.Add(entity);
        }
    }

    /// <summary>
    /// 再描画 - 入力用フォーム
    /// </summary>
    private void Reload_InputForm()
    {
        this.Clear_InputForm();

        // 進行中フラグ
        this.IsOngoing_Checked();

        // 追加ボタン
        this.EnableAddButton();
        // 更新、削除ボタン
        this.EnableControlButton();
    }

    /// <summary>
    /// Create Entity
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>職歴</returns>
    private ProjectEntity CreateEntity(int id)
    {
        var workingEndDate = this.ViewModel.Ongoing_IsChecked.Value ? DateTime.MaxValue : Convert.ToDateTime(this.ViewModel.EndDate_SelectedDate.Value);

        return new ProjectEntity(
            id,
            this.ViewModel.CompanyName_Text.Value,
            this.ViewModel.ProjectName_Text.Value,
            this.ViewModel.StartDate_SelectedDate.Value.Value,
            workingEndDate,
            this.ViewModel.SystemName_Text.Value,
            this.ViewModel.Language_Text.Value,
            this.ViewModel.Database_Text.Value,
            this.ViewModel.DevelopmentSupporting_Text.Value,
            this.ViewModel.OtherTools_Text.Value,
            this.ViewModel.Role_Text.Value,
            this.ViewModel.Member_Text.Value,
            this.ViewModel.AssignedPhrase_RequireDefinition_IsChecked.Value,
            this.ViewModel.AssignedPhrase_BasicDesign_IsChecked.Value,
            this.ViewModel.AssignedPhrase_DetailDesign_IsChecked.Value,
            this.ViewModel.AssignedPhrase_Development_IsChecked.Value,
            this.ViewModel.AssignedPhrase_UnitTest_IsChecked.Value,
            this.ViewModel.AssignedPhrase_IntegrationTest_IsChecked.Value,
            this.ViewModel.AssignedPhrase_SystemTest_IsChecked.Value,
            this.ViewModel.AssignedPhrase_OperationTest_IsChecked.Value,
            this.ViewModel.AssignedPhrase_Other_IsChecked.Value,
            this.ViewModel.Contents_Text.Value,
            this.ViewModel.Remarks_Text.Value
            );
    }

    /// <summary>
    /// クリア
    /// </summary>
    /// <remarks>
    /// 各項目を初期化する。
    /// </remarks>
    public void Clear_InputForm()
    {
        // プロジェクト名
        this.ViewModel.ProjectName_Text.Value = default(string);
        // プロジェクト開始日
        this.ViewModel.StartDate_SelectedDate.Value = DateTime.Today;
        // プロジェクト終了日
        this.ViewModel.EndDate_SelectedDate.Value = DateTime.Today;
        this.IsOngoing_Checked();
        // システム名
        this.ViewModel.SystemName_Text.Value = default(string);
        // 言語
        this.ViewModel.Language_Text.Value = default(string);
        // データベース
        this.ViewModel.Database_Text.Value = default(string);
        // 開発支援ツール
        this.ViewModel.DevelopmentSupporting_Text.Value = default(string);
        // その他ツール
        this.ViewModel.OtherTools_Text.Value = default(string);
        // 担当役割
        this.ViewModel.Role_Text.Value = default(string);
        // メンバー数
        this.ViewModel.Member_Text.Value = default(string);

        // 担当工程 - 要件定義
        this.ViewModel.AssignedPhrase_RequireDefinition_IsChecked.Value = default(bool);
        // 担当工程 - 基本設計
        this.ViewModel.AssignedPhrase_BasicDesign_IsChecked.Value = default(bool);
        // 担当工程 - 詳細設計
        this.ViewModel.AssignedPhrase_DetailDesign_IsChecked.Value = default(bool);
        // 担当工程 - 開発
        this.ViewModel.AssignedPhrase_Development_IsChecked.Value = default(bool);
        // 担当工程 - 単体テスト
        this.ViewModel.AssignedPhrase_UnitTest_IsChecked.Value = default(bool);
        // 担当工程 - 結合テスト
        this.ViewModel.AssignedPhrase_IntegrationTest_IsChecked.Value = default(bool);
        // 担当工程 - システムテスト
        this.ViewModel.AssignedPhrase_SystemTest_IsChecked.Value = default(bool);
        // 担当工程 - 運用テスト
        this.ViewModel.AssignedPhrase_OperationTest_IsChecked.Value = default(bool);
        // 担当工程 - その他
        this.ViewModel.AssignedPhrase_Other_IsChecked.Value = default(bool);
        // プロジェクト内容
        this.ViewModel.Contents_Text.Value = default(string);
        // 備考
        this.ViewModel.Remarks_Text.Value = default(string);

        // 追加ボタン
        this.ViewModel.Add_IsEnabled.Value = false;
        // 更新ボタン
        this.ViewModel.Update_IsEnabled.Value = false;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled.Value = false;
    }

    /// <summary>
    /// 追加
    /// </summary>
    public async Task AddAsync()
    {
        var result = await base.ShowConfirmMsgAsync(this.ViewModel.Window_Title.Value, "入力されたプロジェクト情報を追加しますか？");

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Delete_IsEnabled.Value = true;

            var id = Projects.FetchTotalProjectsCount() + 1;

            var entity = this.CreateEntity(id);

            this.ViewModel.Projects_ItemSource.Add(entity);
            this.Save();

            this.ViewModel.Projects_SelectedIndex.Value = this.ViewModel.Projects_ItemSource.Count - 1;

            this.Reload();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public async Task UpdateAsync()
    {
        var result = await base.ShowConfirmMsgAsync(this.ViewModel.Window_Title.Value, "選択中のプロジェクト情報を更新しますか？");

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var id = this.ViewModel.Projects_ItemSource[this.ViewModel.Projects_SelectedIndex.Value].ID;

            var entity = this.CreateEntity(id);
            this.ViewModel.Projects_ItemSource[this.ViewModel.Projects_SelectedIndex.Value] = entity;

            this.Save();

            this.Reload();
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
        foreach (var entity in this.ViewModel.Projects_ItemSource)
        {
            _repository.Save(entity);
        }
    }

    /// <summary>
    /// 削除
    /// </summary>
    public async Task DeleteAsync()
    {
        if (this.ViewModel.Projects_SelectedIndex.Value.IsUnSelected() ||
            !this.ViewModel.Projects_ItemSource.Any())
        {
            return;
        }

        var result = await base.ShowConfirmMsgAsync(this.ViewModel.Window_Title.Value, "選択中の職歴を削除しますか？");

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            _repository.Delete(this.ViewModel.Projects_ItemSource[this.ViewModel.Projects_SelectedIndex.Value].ID);

            this.ViewModel.Projects_ItemSource.RemoveAt(this.ViewModel.Projects_SelectedIndex.Value);

            this.Reload();
        }
    }
}
