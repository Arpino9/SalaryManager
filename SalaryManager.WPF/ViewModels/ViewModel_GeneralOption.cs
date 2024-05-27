namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - オプション(一般)
/// </summary>
public class ViewModel_GeneralOption : ViewModelBase<Model_Option>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_GeneralOption()
    {
        this.Model.ViewModel = this;
        this.Model.Initialize_General();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        // SQLiteの保存先パス
        this.SelectSQLite_Command.Subscribe(_ => this.Model.SelectSQLitePath());

        // Excelテンプレートの保存先パス
        this.SelectExcelTemplatePath_Command.Subscribe(_ => this.Model.SelectExcelTemplatePath());

        // フォント
        this.FontFamily_SelectionChanged.Subscribe(_ => this.Model.FontFamily_SelectionChanged());

        // 背景色
        this.ChangeWindowBackground_Command.Subscribe(_ => this.Model.ChangeWindowBackground());

        // DBへの画像の保存方法
        this.HowToSaveImage_Checked.Subscribe(_ => this.Model.HowToSaveImage_SelectionChanged());
        this.SelectFolder_Command.Subscribe(_ => this.Model.SelectFolder());

        // 保存
        if (Shared.SavingExtension == "XML")
        {
            this.Save_Command.Subscribe(_ => this.Model.SaveXML());
        }
        else
        {
            this.Save_Command.Subscribe(_ => this.Model.SaveJSON());
        }

        // 初期値に戻す
        this.SetDefault_Command.Subscribe(_ => this.Model.SetDefault());
    }

    /// <summary> Model - オプション </summary>
    protected override Model_Option Model { get; } = Model_Option.GetInstance();

    #region Window

    /// <summary> Window - Title </summary>
    public ReactiveProperty<string> Window_Title { get; }
        = new ReactiveProperty<string>("オプション");

    /// <summary> Window - FontFamily </summary>
    public ReactiveProperty<FontFamily> Window_FontFamily { get; set; }
        = new ReactiveProperty<FontFamily>();

    #endregion

    #region SQLite

    /// <summary> SQLite - Text </summary>
    public ReactiveProperty<string> SelectSQLite_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> SQLite - Command </summary>
    public ReactiveCommand SelectSQLite_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region Excel

    /// <summary> Excelテンプレート - Text </summary>
    public ReactiveProperty<string> SelectExcelTempletePath_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> Excelテンプレート - Command </summary>
    public ReactiveCommand SelectExcelTemplatePath_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region フォント

    /// <summary> フォントファミリ - ItemSource </summary>
    public ReactiveProperty<ObservableCollection<string>> FontFamily_ItemSource { get; set; }
        = new ReactiveProperty<ObservableCollection<string>>();

    /// <summary> フォントファミリ - SelectedIndex </summary>
    public ReactiveProperty<int> FontFamily_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> フォントファミリ - Text </summary>
    public ReactiveProperty<string> FontFamily_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> フォントファミリ - SelectionChanged </summary>
    public ReactiveCommand FontFamily_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    /// <summary> フォントサイズ - Value </summary>
    public ReactiveProperty<decimal> FontSize_Value { get; set; }
        = new ReactiveProperty<decimal>();

    #endregion

    #region 背景色

    /// <summary> 背景色 - Background </summary>
    public ReactiveProperty<SolidColorBrush> Window_Background { get; set; }
        = new ReactiveProperty<SolidColorBrush>();

    /// <summary> 背景色 - Command </summary>
    public ReactiveCommand ChangeWindowBackground_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region プレビュー

    /// <summary> プレビュー - FontFamily </summary>
    public ReactiveProperty<FontFamily> Preview_FontFamily { get; set; }
        = new ReactiveProperty<FontFamily>();

    #endregion

    #region デフォルト明細

    /// <summary> 初期表示時にデフォルト明細を表示する - IsChecked </summary>
    public ReactiveProperty<bool> ShowDefaultPayslip_IsChecked { get; set; }
        = new ReactiveProperty<bool>();

    #endregion

    #region DBへの画像の保存方法

    /// <summary>
    /// 画像の保存方法
    /// </summary>
    public enum HowToSaveImage
    {
        /// <summary> 画像パス </summary>
        SavePath,

        /// <summary> 画像データ </summary>
        SaveImage,
    }

    /// <summary> 画像の保存方法 - IsChecked </summary>
    public ReactiveProperty<HowToSaveImage> HowToSaveImage_IsChecked { get; set; }
        = new ReactiveProperty<HowToSaveImage>();

    /// <summary> 画像の保存方法 - Checked </summary>
    public ReactiveCommand HowToSaveImage_Checked { get; private set; }
        = new ReactiveCommand();

    /// <summary> フォルダを開く - IsEnabled </summary>
    public ReactiveProperty<bool> SelectFolder_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> フォルダを開く - Text </summary>
    public ReactiveProperty<string> ImageFolderPath_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> フォルダを開く - Command </summary>
    public ReactiveCommand SelectFolder_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 保存

    /// <summary> 保存 - Command </summary>
    public ReactiveCommand Save_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 初期値に戻す

    /// <summary> 初期値に戻す - Command </summary>
    public ReactiveCommand SetDefault_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

}
