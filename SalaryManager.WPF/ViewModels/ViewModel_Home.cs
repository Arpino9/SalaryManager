namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - 自宅
/// </summary>
public class ViewModel_Home : ViewModelBase<Model_Home>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_Home()
    {
        this.Model.ViewModel = this;

        this.Model.Initialize();
        
        this.BindEvents();
    }

    protected override void BindEvents()
    {
        // 自宅一覧
        this.Homes_SelectionChanged.Subscribe(_ => this.Model.ListView_SelectionChanged());

        // 在住中
        this.IsLiving_Checked.Subscribe(_ => this.Model.IsLiving_Checked());

        // 住所
        this.Address_Google_TextChanged.Subscribe(_ => this.Model.EnableAddButton());

        this.Add_Command.Subscribe(_ => this.Model.Add());
        this.Add_Command.Subscribe(_ => this.Model.Reload());
        this.Update_Command.Subscribe(_ => this.Model.Update());
        this.Update_Command.Subscribe(_ => this.Model.Reload());
        this.Delete_Command.Subscribe(_ => this.Model.Delete());
        this.Delete_Command.Subscribe(_ => this.Model.Reload());
    }

    /// <summary> Model - 自宅 </summary>
    protected override Model_Home Model { get; } = Model_Home.GetInstance(new HomeSQLite());

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
        = new ReactiveProperty<string>("自宅マスタ");

    #endregion

    #region 自宅一覧

    /// <summary> 自宅一覧 - ItemSource </summary>
    public ReactiveCollection<HomeEntity> Homes_ItemSource { get; set; }
        = new ReactiveCollection<HomeEntity>();

    /// <summary> 自宅一覧 - SelectionChanged </summary>
    public ReactiveCommand Homes_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    /// <summary> 自宅 - SelectedIndex </summary>
    public ReactiveProperty<int> Homes_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 名称

    /// <summary> 名称 - Text </summary>
    public ReactiveProperty<string> DisplayName_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 郵便番号

    /// <summary> 郵便番号 - Text </summary>
    public ReactiveProperty<string> PostCode_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 在住期間

    /// <summary> 在住期間 - 開始日 - Text </summary>
    public ReactiveProperty<DateTime> LivingStart_SelectedDate { get; set; }
        = new ReactiveProperty<DateTime>();

    /// <summary> 在住期間 - 終了日 - Text </summary>
    public ReactiveProperty<DateTime> LivingEnd_SelectedDate { get; set; }
        = new ReactiveProperty<DateTime>();

    #endregion

    #region 在住中

    /// <summary> 在住中 - IsChecked </summary>
    public ReactiveProperty<bool> IsLiving_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 在住中 - Checked </summary>
    public ReactiveCommand IsLiving_Checked { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 住所

    /// <summary> 住所 - Text </summary>
    public ReactiveProperty<string> Address_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 住所 (Google) - Text </summary>
    public ReactiveProperty<string> Address_Google_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 住所 (Google) - TextChanged </summary>
    public ReactiveProperty<bool> Address_Google_TextChanged { get; set; }
        = new ReactiveProperty<bool>();

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
