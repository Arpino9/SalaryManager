using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 休祝日
/// </summary>
public class Model_Holiday : IMaster
{
    #region Get Instance

    public Model_Holiday()
    {
        
    }

    #endregion

    /// <summary> ViewModel - 職歴 </summary>
    public ViewModel_Holiday ViewModel { get; set; }

    public void Clear_InputForm()
    {
        this.ViewModel.Date_SelectedDate.Value = DateTime.Today;
        this.ViewModel.Name_Text.Value         = string.Empty;
        this.ViewModel.CompanyHoliday_IsChecked.Value = false;
        this.ViewModel.CompanyName_SelectedIndex.Value = 0;
        this.ViewModel.Name_Text.Value = string.Empty;
    }

    public void Initialize()
    {
        this.Clear_InputForm();
        this.Reload();

        Companies.Create(new CompanySQLite());
        this.ViewModel.CompanyName_ItemSource = Companies.FetchByAscending().ToReactiveCollection();

        this.ViewModel.CompanyName_SelectedIndex.Value = 0;

        this.Refresh();
    }

    public void Refresh()
    {
        // TODO: どのタイミングで切り替える？
        // 追加ボタン
        this.ViewModel.Add_IsEnabled.Value    = true;
        // 更新ボタン
        this.ViewModel.Update_IsEnabled.Value = true;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled.Value = true;

        if (!this.ViewModel.Holidays_ItemSource.Any())
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
        this.ViewModel.Name_IsEnabled.Value = (this.ViewModel.CompanyHoliday_IsChecked.Value == false);
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
    }

    /// <summary>
    /// 祝日 - SelectionChanged
    /// </summary>
    public void Holidays_SelectionChanged()
    {
        if (this.ViewModel.Holidays_SelectedIndex.Value.IsUnSelected())
        {
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.Refresh();
        }   
    }

    /// <summary>
    /// 祝日名 - TextChanged
    /// </summary>
    public void Name_TextChanged()
    {
        var inputted = !string.IsNullOrEmpty(this.ViewModel.Name_Text.Value);

        this.ViewModel.Add_IsEnabled.Value    = inputted;
        this.ViewModel.Update_IsEnabled.Value = inputted;
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
        var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

        if (holidays.IsEmpty())
        {
            return;
        }

        var list = new List<HolidayEntity>();

        foreach (var holiday in holidays)
        {
            list.Add(new HolidayEntity(holiday.Date, holiday.Name, holiday.CompanyName, holiday.Remarks));
        }

        this.ViewModel.Holidays_ItemSource = list.ToReactiveCollection(this.ViewModel.Holidays_ItemSource);
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
    /// <param name="id">ID</param>
    /// <returns>職歴</returns>
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

            this.Reload();

            this.ViewModel.Holidays_SelectedIndex.Value = this.ViewModel.Holidays_ItemSource.Count;

            this.Refresh();
            this.Clear_InputForm();
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

            this.Reload();

            this.Refresh();
            this.Clear_InputForm();
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

            this.Reload();

            this.Refresh();
            this.Clear_InputForm();
        }
    }
}
