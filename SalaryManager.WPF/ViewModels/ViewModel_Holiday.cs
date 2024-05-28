namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - 祝日
/// </summary>
public class ViewModel_Holiday : ViewModelBase<Model_Holiday>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_Holiday()
    {
        this.Model.ViewModel = this;

        this.BindEvents();
        this.Model.Initialize();
    }

    protected override void BindEvents()
    {
        // 日付
        this.Date_SelectedDateChanged.Subscribe(_ => Model.EnableControlButton());
        // 祝日名
        this.Name_TextChanged.Subscribe(_ => Model.EnableControlButton());
        // 会社休日
        this.CompanyHoliday_Checked.Subscribe(_ => Model.EnableCompanyNameComboBox());
        // 祝日一覧
        this.Holidays_SelectionChanged.Subscribe(_ => Model.ListView_SelectionChanged());

        // 追加
        this.Add_Command.Subscribe(_ => Model.Add());
        this.Add_Command.Subscribe(_ => Model.Reload());
        
        // 更新
        this.Update_Command.Subscribe(_ => Model.Update());
        this.Update_Command.Subscribe(_ => Model.Reload());
        
        // 削除
        this.Delete_Command.Subscribe(_ => Model.Delete());
        this.Delete_Command.Subscribe(_ => Model.Reload());
    }

    /// <summary> Model - 自宅 </summary>
    protected override Model_Holiday Model { get; } = new Model_Holiday();

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
        = new ReactiveProperty<string>("祝日マスタ");

    #endregion

    #region 祝日一覧

    /// <summary> 祝日一覧 - ItemSource </summary>
    public ReactiveCollection<HolidayEntity> Holidays_ItemSource { get; set; }
        = new ReactiveCollection<HolidayEntity>();

    /// <summary> 祝日一覧 - SelectedIndex </summary>
    public ReactiveProperty<int> Holidays_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 祝日一覧 - SelectionChanged </summary>
    public ReactiveCommand Holidays_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 日付

    /// <summary> 日付 - SelectedDate </summary>
    public ReactiveProperty<DateTime> Date_SelectedDate { get; set; }
        = new ReactiveProperty<DateTime>();

    /// <summary> 日付 - TextChanged </summary>
    public ReactiveCommand Date_SelectedDateChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 祝日名

    /// <summary> 祝日名 - IsEnabled </summary>
    public ReactiveProperty<bool> Name_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 祝日名 - Text </summary>
    public ReactiveProperty<string> Name_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 祝日名 - TextChanged </summary>
    public ReactiveCommand Name_TextChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 会社休日設定

    /// <summary> 会社休日 - IsChecked </summary>
    public ReactiveProperty<bool> CompanyHoliday_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 会社休日 - Checked </summary>
    public ReactiveCommand CompanyHoliday_Checked { get; private set; }
        = new ReactiveCommand();

    /// <summary> 会社名 - IsEnabled </summary>
    public ReactiveProperty<bool> CompanyName_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 会社名 - ItemSource </summary>
    public ReactiveCollection<CompanyEntity> CompanyName_ItemSource { get; set; }
        = new ReactiveCollection<CompanyEntity>();

    /// <summary> 会社名 - SelectedIndex </summary>
    public ReactiveProperty<int> CompanyName_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 会社名 - Text </summary>
    public ReactiveProperty<string> CompanyName_Text { get; set; }
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
