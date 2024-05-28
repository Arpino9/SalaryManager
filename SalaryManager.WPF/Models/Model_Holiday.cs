using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 休祝日
/// </summary>
public class Model_Holiday : ModelBase<ViewModel_Holiday>, IEditableMaster
{
    #region Get Instance

    public Model_Holiday()
    {
        
    }

    #endregion

    /// <summary> ViewModel - 職歴 </summary>
    internal override ViewModel_Holiday ViewModel { get; set; }

    public void Initialize()
    {
        this.Window_Activated();

        this.Reload();

        Companies.Create(new CompanySQLite());

        var companies = Companies.FetchByAscending();

        foreach (var company in companies)
        {
            this.ViewModel.CompanyName_ItemSource.Add(company);
        }

        this.ViewModel.CompanyName_SelectedIndex.Value = 0;

        this.ListView_SelectionChanged();
    }

    public void Window_Activated()
    {
        this.ViewModel.Window_FontFamily.Value = XMLLoader.FetchFontFamily();
        this.ViewModel.Window_FontSize.Value   = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
    }

    public void EnableControlButton()
    {
        var date = this.ViewModel.Date_SelectedDate.Value;
        var name = this.ViewModel.Name_Text.Value;

        var hasHoliday = this.ViewModel.Holidays_ItemSource.Where(x => x.Date.Year  == date.Year &&
                                                                       x.Date.Month == date.Month &&
                                                                       x.Date.Day   == date.Day &&
                                                                       x.Name       == name);

        // 追加ボタン
        this.ViewModel.Add_IsEnabled.Value    = hasHoliday.IsEmpty();
        // 更新ボタン
        this.ViewModel.Update_IsEnabled.Value = hasHoliday.IsEmpty();
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled.Value = true;
    }

    /// <summary>
    /// 祝日 - SelectionChanged
    /// </summary>
    public void ListView_SelectionChanged()
    {
        if (this.ViewModel.Holidays_SelectedIndex.Value.IsUnSelected())
        {
            return;
        }

        if (this.ViewModel.Holidays_ItemSource.IsEmpty())
        {
            return;
        }

        var entity = this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex.Value];

        // 日付
        this.ViewModel.Date_SelectedDate.Value = entity.Date;
        // 名称
        this.ViewModel.Name_Text.Value         = entity.Name;
        // 会社休日
        this.ViewModel.CompanyHoliday_IsChecked.Value = string.IsNullOrEmpty(entity.CompanyName) == false;
        // 会社名
        this.ViewModel.CompanyName_Text.Value  = entity.CompanyName;
        this.ViewModel.Name_IsEnabled.Value    = (this.ViewModel.CompanyHoliday_IsChecked.Value == false);
        // 備考
        this.ViewModel.Remarks_Text.Value      = entity.Remarks;

        if (this.ViewModel.CompanyName_ItemSource.IsEmpty())
        {
            this.ViewModel.CompanyName_Text.Value = string.Empty;
            return;
        }

        if (this.ViewModel.CompanyHoliday_IsChecked.Value is false)
        {
            this.ViewModel.CompanyName_Text.Value = this.ViewModel.CompanyName_ItemSource.First().CompanyName;
        }

        this.EnableControlButton();
    }

    /// <summary>
    /// 会社休日 - Checked
    /// </summary>
    public void EnableCompanyNameComboBox()
    {
        var isChecked = this.ViewModel.CompanyHoliday_IsChecked.Value;

        this.ViewModel.CompanyName_IsEnabled.Value = isChecked;

        this.ViewModel.Name_Text.Value = "会社休日";
        this.ViewModel.Name_IsEnabled.Value        = (isChecked == false);
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
        using (var cursor = new CursorWaiting())
        {
            var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

            if (holidays.IsEmpty())
            {
                return;
            }

            var list = new List<HolidayEntity>();

            foreach (var holiday in holidays.OrderByDescending(x => x.Date))
            {
                this.ViewModel.Holidays_ItemSource.Add(new HolidayEntity(holiday.Date, holiday.Name, holiday.CompanyName, holiday.Remarks));
            }
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
        this.ViewModel.Date_SelectedDate.Value         = DateTime.Today;
        // 祝日名
        this.ViewModel.Name_Text.Value                 = string.Empty;
        // 会社休日
        this.ViewModel.CompanyHoliday_IsChecked.Value  = false;
        this.ViewModel.CompanyName_SelectedIndex.Value = 0;
        // 備考
        this.ViewModel.Remarks_Text.Value = string.Empty;
    }

    public void Save()
    {
        var list = new List<JSONProperty_Holiday>();

        foreach (var holiday in this.ViewModel.Holidays_ItemSource)
        {
            var json = new JSONProperty_Holiday { 
                                                    Date        = holiday.Date, 
                                                    Name        = holiday.Name, 
                                                    CompanyName = holiday.CompanyName,
                                                    Remarks     = holiday.Remarks 
                                                };

            list.Add(json);
        }

        list.ToArray().SerializeToFile(FilePath.GetJSONHolidayDefaultPath());
    }

    /// <summary>
    /// Create Entity
    /// </summary>
    /// <returns>祝日</returns>
    private HolidayEntity CreateEntity()
    {
        return new HolidayEntity(
            this.ViewModel.Date_SelectedDate.Value,
            this.ViewModel.Name_Text.Value,
            this.ViewModel.CompanyName_Text.Value,
            this.ViewModel.Remarks_Text.Value);
    }

    /// <summary>
    /// 追加
    /// </summary>
    public void Add()
    {
        if (!Message.ShowConfirmingMessage($"入力された祝日を追加しますか？", this.ViewModel.Window_Title.Value))
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Delete_IsEnabled.Value = true;

            var entity = this.CreateEntity();

            this.ViewModel.Holidays_ItemSource.Add(entity);
            this.Save();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        if (!Message.ShowConfirmingMessage($"選択中の祝日を更新しますか？", this.ViewModel.Window_Title.Value))
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var entity = this.CreateEntity();
            this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex.Value] = entity;

            this.Save();
        }
    }

    /// <summary>
    /// 削除
    /// </summary>
    public void Delete()
    {
        if (this.ViewModel.Holidays_SelectedIndex.Value.IsUnSelected() ||
            this.ViewModel.Holidays_ItemSource.IsEmpty())
        {
            return;
        }

        if (!Message.ShowConfirmingMessage($"選択中の祝日を削除しますか？", this.ViewModel.Window_Title.Value))
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Holidays_ItemSource.RemoveAt(this.ViewModel.Holidays_SelectedIndex.Value);

            this.Save();
        }
    }
}
