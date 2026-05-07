using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 会社マスタ
/// </summary>
public sealed class CompanyModel : ModelBase<CompanyViewModel>, IEditableMaster
{
    #region Get Instance

    private static CompanyModel model = null;

    public static CompanyModel GetInstance(ICompanyRepository repository)
    {
        if (model == null)
        {
            model = new CompanyModel(repository);
        }

        return model;
    }

    #endregion

    private ICompanyRepository _repository;

    public CompanyModel(ICompanyRepository repository)
    {
        this._repository = repository;
    }

    /// <summary> ViewModel - 職歴 </summary>
    internal override CompanyViewModel ViewModel { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 未選択状態、かつ新規登録が可能な状態にする。
    /// </remarks>
    public void Initialize()
    {
        this.Window_Activated();

        this.Reload();

        this.ListView_SelectionChanged();

        var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_Text);

        this.ViewModel.BusinessCategory_Middle_ItemSource.Clear();

        foreach (var value in item.MiddleList.Values)
        {
            this.ViewModel.BusinessCategory_Middle_ItemSource.Add(value);
        }

        var entity = this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex];
        this.ViewModel.BusinessCategory_Middle_Text = entity.BusinessCategory.MiddleName;
    }

    public void Window_Activated()
    {
        this.ViewModel.Window_FontFamily = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
        this.ViewModel.Window_FontSize = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
    }

    /// <summary>
    /// 業種 (大区分) - SelectionChanged
    /// </summary>
    public void BusinessCategory_Large_SelectionChanged()
    {
        if (this.ViewModel.BusinessCategory_Large_SelectedItem is null ||
            this.ViewModel.Companies_SelectedIndex.IsUnSelected())
        {
            // 無効
            return;
        }

        var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_SelectedItem);

        this.ViewModel.BusinessCategory_Middle_ItemSource.Clear();

        foreach (var value in item.MiddleList.Values)
        {
            this.ViewModel.BusinessCategory_Middle_ItemSource.Add(value);
        }

        if (this.ViewModel.BusinessCategory_Large_SelectedItem !=
            this.ViewModel.BusinessCategory_Large_Text)
        {
            // リスト変更時
            this.ViewModel.BusinessCategory_Middle_Text = this.ViewModel.BusinessCategory_Middle_ItemSource.FirstOrDefault();
        }
        else
        {
            // 一覧変更時
            var entity = this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex];
            this.ViewModel.BusinessCategory_Middle_Text = entity.BusinessCategory.MiddleName;
        }
    }

    /// <summary>
    /// クリア
    /// </summary>
    /// <remarks>
    /// 各項目を初期化する。
    /// </remarks>
    public void Clear_InputForm()
    {
        // 会社名
        this.ViewModel.CompanyName_Text = string.Empty;

        // 郵便番号
        this.ViewModel.PostCode_Text = string.Empty;

        // 住所
        this.ViewModel.Address_Text = string.Empty;
        this.ViewModel.Address_Google_Text = string.Empty;

        // 備考
        this.ViewModel.Remarks_Text = string.Empty;

        // 追加ボタン
        this.ViewModel.Add_IsEnabled = false;
        // 更新ボタン
        this.ViewModel.Update_IsEnabled = false;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled = false;
    }

    /// <summary>
    /// 会社名 - TextChanged
    /// </summary>
    public void EnableAddButton()
    {
        var inputted = !string.IsNullOrEmpty(this.ViewModel.CompanyName_Text) &&
                       !string.IsNullOrEmpty(this.ViewModel.Address_Google_Text);

        this.ViewModel.Add_IsEnabled = inputted;
    }

    /// <summary> 会社名(更新前) </summary>
    private string CompanyName_Prev;

    /// <summary> 会社の住所(更新前) </summary>
    private string CompanyAddress_Prev;

    /// <summary>
    /// 会社一覧 - SelectionChanged
    /// </summary>
    public void ListView_SelectionChanged()
    {
        if (this.ViewModel.Companies_SelectedIndex.IsUnSelected())
        {
            return;
        }

        this.EnableControlButton();

        if (this.ViewModel.Companies_ItemSource.IsEmpty())
        {
            return;
        }

        var entity = this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex];

        // 業種(大区分)
        foreach (var category in BusinessCategoryValue.LargeCategory)
        {
            this.ViewModel.BusinessCategory_Large_ItemsSource.Add(category);
        }

        this.ViewModel.BusinessCategory_Large_Text = entity.BusinessCategory.LargeName;

        this.BusinessCategory_Large_SelectionChanged();

        // 会社名
        this.ViewModel.CompanyName_Text = entity.CompanyName;
        this.CompanyName_Prev = entity.CompanyName;
        this.CompanyAddress_Prev = entity.Address_Google;

        // 郵便番号
        this.ViewModel.PostCode_Text = entity.PostCode;
        // 住所
        this.ViewModel.Address_Text = entity.Address;
        // 住所(Google)
        this.ViewModel.Address_Google_Text = entity.Address_Google;
        // 備考
        this.ViewModel.Remarks_Text = entity.Remarks;
    }

    /// <summary>
    /// Enable - 操作ボタン
    /// </summary>
    /// <remarks>
    /// 追加ボタンは「会社名」に値があれば押下可能。
    /// </remarks>
    private void EnableControlButton()
    {
        var selected = this.ViewModel.Companies_ItemSource.Any()
                    && this.ViewModel.Companies_SelectedIndex >= 0;

        // 更新ボタン
        this.ViewModel.Update_IsEnabled = selected;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled = selected;
    }

    /// <summary>
    /// リロード
    /// </summary>
    public void Reload()
    {
        using (var cursor = new CursorWaiting())
        {
            Companies.Create(_repository);

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
        var entities = Companies.FetchByDescending();

        if (entities.IsEmpty())
        {
            return;
        }

        this.ViewModel.Companies_ItemSource.Clear();

        foreach (var entity in entities)
        {
            this.ViewModel.Companies_ItemSource.Add(entity);
        }

        this.ListView_SelectionChanged();
    }

    /// <summary>
    /// 再描画 - 入力用フォーム
    /// </summary>
    private void Reload_InputForm()
    {
        this.Clear_InputForm();

        // 追加ボタン
        this.EnableAddButton();
        // 更新、削除ボタン
        this.EnableControlButton();
    }

    /// <summary> Model </summary>
    public Model_WorkingPlace Model_WorkingPlace { get; set; } = Model_WorkingPlace.GetInstance(new WorkingPlaceSQLite());

    /// <summary>
    /// 追加更新
    /// </summary>
    public void AddtionalUpdate()
    {
        IWorkingPlaceRepository workingPlaceRepository = new WorkingPlaceSQLite();

        if (string.IsNullOrEmpty(this.CompanyName_Prev) == false &&
            string.IsNullOrEmpty(this.ViewModel.CompanyName_Text) == false &&
            this.CompanyName_Prev != this.ViewModel.CompanyName_Text)
        {
            // 会社名が変更された場合
            this.Model_WorkingPlace.UpdateCompanyName(this.CompanyName_Prev, this.ViewModel.CompanyName_Text);

            using (var transaction = new SQLiteTransaction())
            {
                workingPlaceRepository.UpdateCompanyName(transaction, CompanyName_Prev, this.ViewModel.CompanyName_Text);

                transaction.Commit();
            }
        }

        if (string.IsNullOrEmpty(this.CompanyAddress_Prev) == false &&
            string.IsNullOrEmpty(this.ViewModel.Address_Google_Text) == false &&
            this.CompanyAddress_Prev != this.ViewModel.Address_Google_Text)
        {
            // 会社の住所が変更された場合
            workingPlaceRepository.UpdateCompanyAddress(this.CompanyAddress_Prev, this.ViewModel.Address_Google_Text);
        }
    }

    /// <summary>
    /// 追加
    /// </summary>
    public async void AddAsync()
    {
        var result = await base.MetroWindow.ShowMessageAsync(
                                this.ViewModel.Title,
                                "入力された会社情報を追加しますか？",
                                MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Delete_IsEnabled = true;

            var id = this.ViewModel.Companies_ItemSource.Any() ?
                     this.ViewModel.Companies_ItemSource.Max(x => x.ID) + 1 : 1;

            var entity = this.CreateEntity(id);

            this.ViewModel.Companies_ItemSource.Add(entity);
            this.Save();
        }

        this.ViewModel.Companies_SelectedIndex += 1;
    }

    /// <summary>
    /// Create Entity
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>職歴</returns>
    private CompanyEntity CreateEntity(int id)
    {
        var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_Text);
        var middleNo = item.GetMiddleCategoryKey(this.ViewModel.BusinessCategory_Middle_Text);

        return new CompanyEntity(
            id,
            int.Parse(middleNo),
            this.ViewModel.CompanyName_Text,
            this.ViewModel.PostCode_Text,
            this.ViewModel.Address_Text,
            this.ViewModel.Address_Google_Text,
            this.ViewModel.Remarks_Text);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public async void UpdateAsync()
    {
        var result = await base.MetroWindow.ShowMessageAsync(
                                this.ViewModel.Title,
                                "選択中の会社情報を更新しますか？",
                                MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var id = this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex].ID;

            var entity = this.CreateEntity(id);
            this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex] = entity;

            this.Save();
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    private void Save()
    {
        using (var transaction = new SQLiteTransaction())
        {
            foreach (var entity in this.ViewModel.Companies_ItemSource)
            {
                _repository.Save(transaction, entity);
                _repository.SaveAddress(transaction, entity);
            }

            transaction.Commit();
        }
    }

    /// <summary>
    /// 削除
    /// </summary>
    public async void DeleteAsync()
    {
        if (this.ViewModel.Companies_SelectedIndex.IsUnSelected() ||
            this.ViewModel.Companies_ItemSource.IsEmpty())
        {
            return;
        }

        var result = await base.MetroWindow.ShowMessageAsync(
                                this.ViewModel.Title,
                                "選択中の会社情報を削除しますか？",
                                MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var id = this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex].ID;

            _repository.Delete(id);

            this.ViewModel.Companies_ItemSource.RemoveAt(this.ViewModel.Companies_SelectedIndex);

            if (this.ViewModel.Companies_SelectedIndex >= this.ViewModel.Companies_ItemSource.Count)
            {
                this.ViewModel.Companies_SelectedIndex -= 1;
            }
        }
    }
}
