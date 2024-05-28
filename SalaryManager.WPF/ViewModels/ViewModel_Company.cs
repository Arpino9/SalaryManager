namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - 会社マスタ
/// </summary>
public class ViewModel_Company : ViewModelBase<Model_Company>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_Company()
    {
        this.Model.ViewModel = this;

        this.BindEvents();

        this.Model.Initialize();
    }

    protected override void BindEvents()
    {
        // 会社名
        this.CompanyName_TextChanged.Subscribe(_ => this.Model.EnableAddButton());
        // 住所
        this.Address_Google_TextChanged.Subscribe(_ => this.Model.EnableAddButton());
        // 業種
        this.BusinessCategory_Large_SelectionChanged.Subscribe(_ => this.Model.BusinessCategory_Large_SelectionChanged());

        // 追加
        this.Add_Command.Subscribe(_ => this.Model.Add());
        this.Add_Command.Subscribe(_ => this.Model.AddtionalUpdate());
        this.Add_Command.Subscribe(_ => this.Model.Reload());

        // 更新
        this.Update_Command.Subscribe(_ => this.Model.Update());
        this.Update_Command.Subscribe(_ => this.Model.AddtionalUpdate());
        this.Update_Command.Subscribe(_ => this.Model.Reload());

        // 削除
        this.Delete_Command.Subscribe(_ => this.Model.Delete());
        this.Delete_Command.Subscribe(_ => this.Model.Reload());

        // 会社一覧
        this.Companies_SelectionChanged.Subscribe(_ => this.Model.ListView_SelectionChanged());
    }

    /// <summary> Model - 会社 </summary>
    protected override Model_Company Model { get; } = Model_Company.GetInstance(new CompanySQLite());

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
        = new ReactiveProperty<string>("会社マスタ");

    #endregion

    #region 会社一覧

    /// <summary> 会社一覧 - ItemSource </summary>
    public ReactiveCollection<CompanyEntity> Companies_ItemSource { get; set; }
        = new ReactiveCollection<CompanyEntity>();

    /// <summary> 会社一覧 - SelectedIndex </summary>
    public ReactiveProperty<int> Companies_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 会社一覧 - SelectionChanged </summary>
    public ReactiveCommand Companies_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 業種 (大区分)

    /// <summary>  業種 (大区分) - ItemsSource </summary>
    /// <remarks> 固定なので、値オブジェクトのリストを流用。 </remarks>
    public ReactiveCollection<BusinessCategoryValue> BusinessCategory_Large_ItemsSource { get; set; }
        = new ReactiveCollection<BusinessCategoryValue>();

    /// <summary> 業種 (大区分) - SelectedItem </summary>
    public ReactiveProperty<string> BusinessCategory_Large_SelectedItem { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 業種 (大区分) - Text </summary>
    public ReactiveProperty<string> BusinessCategory_Large_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 業種 (大区分) - SelectionChanged </summary>
    public ReactiveCommand BusinessCategory_Large_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 業種 (中区分)

    /// <summary> 業種 (中区分) - ItemSource </summary>
    public ReactiveCollection<string> BusinessCategory_Middle_ItemSource { get; set; }
        = new ReactiveCollection<string>();

    /// <summary> 業種 (中区分) - Text </summary>
    public ReactiveProperty<string> BusinessCategory_Middle_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 会社名

    /// <summary> 会社名 - Text </summary>
    public ReactiveProperty<string> CompanyName_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 会社名 - TextChanged </summary>
    public ReactiveCommand CompanyName_TextChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 郵便番号

    /// <summary> 郵便番号 - Text </summary>
    public ReactiveProperty<string> PostCode_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 住所

    /// <summary> 住所 - Text </summary>
    public ReactiveProperty<string> Address_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 住所(Googleカレンダー登録用) - Text </summary>
    public ReactiveProperty<string> Address_Google_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 住所(Googleカレンダー登録用) - TextChanged </summary>
    public ReactiveCommand Address_Google_TextChanged { get; private set; }
        = new ReactiveCommand();

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