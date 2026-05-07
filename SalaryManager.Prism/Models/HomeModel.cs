using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 自宅
/// </summary>
public sealed class HomeModel : ModelBase<HomeViewModel>, IEditableMaster
{
    #region Get Instance

    private static HomeModel model = null;

    public HomeModel(IHomeRepository repository)
    {
        _repository = repository;
    }

    #endregion

    public IHomeRepository _repository { get; }

    public static HomeModel GetInstance(IHomeRepository repository)
    {
        if (model == null)
        {
            model = new HomeModel(repository);
        }

        return model;
    }

    /// <summary> ViewModel - 職歴 </summary>
    internal override HomeViewModel ViewModel { get; set; }

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
        this.ViewModel.Window_FontFamily = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
        this.ViewModel.Window_FontSize   = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
    }

    /// <summary>
    /// 在住中 - Checked
    /// </summary>
    public void IsLiving_Checked()
    {
        if (this.ViewModel.IsLiving_IsChecked)
        {
            this.ViewModel.LivingEnd_SelectedDate = DateUtils.Today;
        }
    }

    public void Clear_InputForm()
    {
        // 名称
        this.ViewModel.DisplayName_Text = string.Empty;

        // 住所
        this.ViewModel.Address_Text = string.Empty;

        // 在住期間
        this.ViewModel.LivingStart_SelectedDate = DateUtils.Today;
        this.ViewModel.LivingEnd_SelectedDate = DateUtils.Today;
        this.ViewModel.IsLiving_IsChecked = false;

        // 備考
        this.ViewModel.Remarks_Text = string.Empty;
    }

    /// <summary>
    /// ListView - SelectionChanged
    /// </summary>
    public void ListView_SelectionChanged()
    {
        if (this.ViewModel.Homes_SelectedIndex.IsUnSelected())
        {
            return;
        }

        this.EnableControlButton();

        if (!this.ViewModel.Title.Any())
        {
            return;
        }

        var entity = this.ViewModel.Homes_ItemSource[this.ViewModel.Homes_SelectedIndex];

        // 名称
        this.ViewModel.DisplayName_Text = entity.DisplayName;
        // 在住期間
        this.ViewModel.LivingStart_SelectedDate = entity.LivingStart;
        this.ViewModel.LivingEnd_SelectedDate = entity.LivingEnd;
        // 在住中か
        this.ViewModel.IsLiving_IsChecked = entity.IsLiving;
        // 住所
        this.ViewModel.Address_Text = entity.Address;
        // 会社名
        this.ViewModel.Address_Google_Text = entity.Address_Google;
        // 郵便番号
        this.ViewModel.PostCode_Text = entity.PostCode;
        // 備考
        this.ViewModel.Remarks_Text = entity.Remarks;
    }

    /// <summary>
    /// 自宅 - TextChanged
    /// </summary>
    public void EnableAddButton()
    {
        var inputted = !string.IsNullOrEmpty(this.ViewModel.Address_Text);

        this.ViewModel.Add_IsEnabled = inputted;
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
        Homes.Create(_repository);

        var entities = Homes.FetchByDescending();

        if (entities.IsEmpty())
        {
            return;
        }

        this.ViewModel.Homes_ItemSource.Clear();

        foreach (var entity in entities)
        {
            this.ViewModel.Homes_ItemSource.Add(entity);
        }
    }

    /// <summary>
    /// 再描画 - 入力用フォーム
    /// </summary>
    private void Reload_InputForm()
    {
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
    private HomeEntity CreateEntity(int id)
    {
        return new HomeEntity(
            id,
            this.ViewModel.DisplayName_Text,
            this.ViewModel.LivingStart_SelectedDate,
            this.ViewModel.LivingEnd_SelectedDate,
            this.ViewModel.IsLiving_IsChecked,
            this.ViewModel.PostCode_Text,
            this.ViewModel.Address_Text,
            this.ViewModel.Address_Google_Text,
            this.ViewModel.Remarks_Text);
    }

    /// <summary>
    /// Enable - 操作ボタン
    /// </summary>
    /// <remarks>
    /// 追加ボタンは「会社名」に値があれば押下可能。
    /// </remarks>
    private void EnableControlButton()
    {
        var selected = this.ViewModel.Homes_ItemSource.Any()
                    && this.ViewModel.Homes_SelectedIndex >= 0;

        // 更新ボタン
        this.ViewModel.Update_IsEnabled = selected;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled = selected;
    }

    /// <summary>
    /// 追加
    /// </summary>
    public async void AddAsync()
    {
        var result = await base.MetroWindow.ShowMessageAsync(
                                this.ViewModel.Title,
                                "入力された自宅情報を追加しますか？",
                                MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Delete_IsEnabled = true;

            var id = this.ViewModel.Homes_ItemSource.Any() ?
                     this.ViewModel.Homes_ItemSource.Max(x => x.ID) + 1 : 1;

            var entity = this.CreateEntity(id);

            this.ViewModel.Homes_ItemSource.Add(entity);
            this.Save();

            this.ViewModel.Homes_SelectedIndex = this.ViewModel.Homes_ItemSource.Count;
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public async void UpdateAsync()
    {
        var result = await base.MetroWindow.ShowMessageAsync(
                                this.ViewModel.Title,
                                "選択中の自宅情報を更新しますか？",
                                MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var id = this.ViewModel.Homes_ItemSource[this.ViewModel.Homes_SelectedIndex].ID;

            var entity = this.CreateEntity(id);
            this.ViewModel.Homes_ItemSource[this.ViewModel.Homes_SelectedIndex] = entity;

            this.Save();
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
        foreach (var entity in this.ViewModel.Homes_ItemSource)
        {
            _repository.Save(entity);
        }
    }

    /// <summary>
    /// 削除
    /// </summary>
    public async void DeleteAsync()
    {
        if (this.ViewModel.Homes_SelectedIndex.IsUnSelected() ||
            !this.ViewModel.Homes_ItemSource.Any())
        {
            return;
        }

        var result = await base.MetroWindow.ShowMessageAsync(
                                this.ViewModel.Title,
                                "選択中の職歴を削除しますか？",
                                MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            _repository.Delete(this.ViewModel.Homes_SelectedIndex + 1);

            this.ViewModel.Homes_ItemSource.RemoveAt(this.ViewModel.Homes_SelectedIndex);
        }
    }
}
