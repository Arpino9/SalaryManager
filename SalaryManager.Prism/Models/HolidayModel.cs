using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 祝日
/// </summary>
public sealed class HolidayModel : ModelBase<HolidayViewModel>, IEditableMaster
{
    #region Get Instance

    public HolidayModel()
    {

    }

    #endregion

    /// <summary> ViewModel - 職歴 </summary>
    internal override HolidayViewModel ViewModel { get; set; }

    public void Initialize()
    {
        this.Reload();

        Companies.Create(new CompanySQLite());

        var companies = Companies.FetchByAscending();

        foreach (var company in companies)
        {
            this.ViewModel.CompanyName_ItemSource.Add(company);
        }

        this.ViewModel.CompanyName_SelectedIndex = 0;

        this.ListView_SelectionChanged();
    }

    /// <summary>
    /// 会社名の変更時イベント
    /// </summary>
    /// <param name="items">変更後の項目</param>
    /// <remarks>
    /// ComboBoxのSelectionChangedイベントが発生した際に、
    /// ViewModelの会社名が更新されない問題を修正するためのメソッド。
    /// </remarks>
    public void CompanyName_SelectionChecked(object[] items)
    {
        if (items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            if (item is not CompanyEntity company)
            {
                continue;
            }

            if (company.CompanyName != this.ViewModel.CompanyName_Text)
            {
                this.ViewModel.CompanyName_Text = company.CompanyName;
            }
        }

        this.EnableControlButton();
    }

    /// <summary>
    /// Enable - 操作ボタン
    /// </summary>
    public void EnableControlButton()
    {
        var date = this.ViewModel.Date_SelectedDate;

        var hasHoliday = this.ViewModel.Holidays_ItemSource.Where(x => x.Date.Year   == date.Year &&
                                                                       x.Date.Month  == date.Month &&
                                                                       x.Date.Day    == date.Day &&
                                                                       x.Name        == this.ViewModel.Name_Text &&
                                                                       x.CompanyName == this.ViewModel.CompanyName_Text &&
                                                                       x.Remarks     == this.ViewModel.Remarks_Text);

        var selected = this.ViewModel.Holidays_SelectedIndex >= 0
                       && this.ViewModel.Holidays_SelectedIndex < this.ViewModel.Holidays_ItemSource.Count;

        // 追加
        this.ViewModel.Add_IsEnabled    = hasHoliday.IsEmpty();
        // 更新
        this.ViewModel.Update_IsEnabled = selected && hasHoliday.IsEmpty();
        // 削除
        this.ViewModel.Delete_IsEnabled = selected;
    }

    /// <summary>
    /// 祝日 - SelectionChanged
    /// </summary>
    public void ListView_SelectionChanged()
    {
        if (this.ViewModel.Holidays_SelectedIndex.IsUnSelected())
        {
            // 未選択
            return;
        }

        if (this.ViewModel.Holidays_ItemSource.IsEmpty())
        {
            // リストが空
            return;
        }

        var entity = this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex];

        // 日付
        this.ViewModel.Date_SelectedDate = entity.Date;
        // 名称
        this.ViewModel.Name_Text = entity.Name;
        // 会社休日
        this.ViewModel.CompanyHoliday_IsChecked = string.IsNullOrEmpty(entity.CompanyName) == false;
        // 会社名
        this.ViewModel.CompanyName_Text = entity.CompanyName;
        this.ViewModel.Name_IsEnabled = (this.ViewModel.CompanyHoliday_IsChecked == false);
        // 備考
        this.ViewModel.Remarks_Text = entity.Remarks;

        this.EnableControlButton();
    }

    /// <summary>
    /// Enable - 会社名
    /// </summary>
    public void EnableCompanyNameComboBox()
    {
        var isChecked = this.ViewModel.CompanyHoliday_IsChecked;

        this.ViewModel.CompanyName_IsEnabled = isChecked;

        if (isChecked)
        {
            this.ViewModel.Name_Text = "会社休日";
        }
        else
        {
            this.ViewModel.CompanyName_Text = string.Empty;
        }

        this.ViewModel.Name_IsEnabled = (isChecked == false);
    }

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
        var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

        if (holidays.IsEmpty())
        {
            return;
        }

        var list = new List<HolidayEntity>();

        this.ViewModel.Holidays_ItemSource.Clear();

        foreach (var holiday in holidays.OrderByDescending(x => x.Date))
        {
            this.ViewModel.Holidays_ItemSource.Add(new HolidayEntity(holiday.Date, holiday.Name, holiday.CompanyName, holiday.Remarks));
        }

        this.ListView_SelectionChanged();
    }

    /// <summary>
    /// 再描画 - 入力用フォーム
    /// </summary>
    private void Reload_InputForm()
    {
        this.Clear_InputForm();

        // 更新、削除ボタン
        this.EnableControlButton();
    }

    public void Clear_InputForm()
    {
        // 日付
        this.ViewModel.Date_SelectedDate = DateTime.Today;
        // 祝日名
        this.ViewModel.Name_Text = string.Empty;
        // 会社休日
        this.ViewModel.CompanyHoliday_IsChecked = false;
        this.ViewModel.CompanyName_SelectedIndex = 0;
        // 備考
        this.ViewModel.Remarks_Text = string.Empty;
    }

    public void Save()
    {
        var list = new List<JSONProperty_Holiday>();

        foreach (var holiday in this.ViewModel.Holidays_ItemSource)
        {
            var json = new JSONProperty_Holiday
            {
                Date = holiday.Date,
                Name = holiday.Name,
                CompanyName = holiday.CompanyName,
                Remarks = holiday.Remarks
            };

            list.Add(json);
        }

        list.ToArray().SerializeToFile(FilePath.GetJSONHolidayDefaultPath());

        this.Reload();
    }

    /// <summary>
    /// Create Entity
    /// </summary>
    /// <returns>祝日</returns>
    private HolidayEntity CreateEntity()
    {
        return new HolidayEntity(
            this.ViewModel.Date_SelectedDate,
            this.ViewModel.Name_Text,
            this.ViewModel.CompanyName_Text,
            this.ViewModel.Remarks_Text);
    }

    /// <summary>
    /// 追加
    /// </summary>
    public async void AddAsync()
    {
        var result = await base.ShowConfirmMsgAsync(this.ViewModel.Title, "入力された祝日を追加しますか？");

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Delete_IsEnabled = true;

            var entity = this.CreateEntity();

            this.ViewModel.Holidays_ItemSource.Add(entity);
            this.Save();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public async void UpdateAsync()
    {
        var result = await base.ShowConfirmMsgAsync(this.ViewModel.Title, "選択中の祝日を更新しますか？");

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var entity = this.CreateEntity();
            this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex] = entity;

            this.Save();
        }
    }

    /// <summary>
    /// 削除
    /// </summary>
    public async void DeleteAsync()
    {
        if (this.ViewModel.Holidays_SelectedIndex.IsUnSelected() ||
            this.ViewModel.Holidays_ItemSource.IsEmpty())
        {
            return;
        }

        var result = await base.ShowConfirmMsgAsync(this.ViewModel.Title, "選択中の祝日を削除しますか？");

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Holidays_ItemSource.RemoveAt(this.ViewModel.Holidays_SelectedIndex);

            this.Save();
        }
    }
}
