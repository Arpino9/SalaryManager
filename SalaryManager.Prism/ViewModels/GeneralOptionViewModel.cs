namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - オプション
/// </summary>
public class GeneralOptionViewModel : ViewModelBase<OptionModel>, IDialogAware
{
    public GeneralOptionViewModel()
    {
        this.Model.ViewModel = this;

        this.Model.Initialize_General();

        this.BindEvents();
    }

    public event Action<IDialogResult> RequestClose;

    protected override void BindEvents()
    {
        // SQLiteの保存先パス
        this.SelectSQLite_Command = new DelegateCommand(() => this.Model.SelectSQLitePath());

        // Excelテンプレートの保存先パス
        this.SelectExcelTemplatePath_Command = new DelegateCommand(() => this.Model.SelectExcelTemplatePath());

        // フォント
        this.FontFamily_SelectionChanged = new DelegateCommand(() => this.Model.FontFamily_SelectionChanged());

        // 背景色
        this.ChangeWindowBackground_Command = new DelegateCommand(() => this.Model.ChangeWindowBackground());

        // DBへの画像の保存方法
        this.HowToSaveImage_Checked = new DelegateCommand(() => this.Model.HowToSaveImage_SelectionChanged());
        this.SelectFolder_Command = new DelegateCommand(() => this.Model.SelectFolder());
        // 保存
        if (Shared.SavingExtension == "XML")
        {
            this.Save_Command = new DelegateCommand(() => this.Model.SaveXML());
        }
        else
        {
            this.Save_Command = new DelegateCommand(() => this.Model.SaveJSON());
        }

        // 初期値に戻す
        this.SetDefault_Command = new DelegateCommand(() => this.Model.SetDefault());
    }

    /// <summary> タイトル </summary>
    public string Title => "オプション";

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
    }

    /// <summary> Model - オプション </summary>
    protected override OptionModel Model { get; } = OptionModel.GetInstance();

    #region SQLite

    /// <summary> SQLite - Text </summary>
    public string SelectSQLite_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> SQLite - Command </summary>
    public DelegateCommand SelectSQLite_Command { get; private set; }

    #endregion

    #region Excel

    /// <summary> Excelテンプレート - Text </summary>
    public string SelectExcelTempletePath_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Excelテンプレート - Command </summary>
    public DelegateCommand SelectExcelTemplatePath_Command { get; private set; }

    #endregion

    #region フォント

    /// <summary> フォントファミリ - ItemSource </summary>
    public ObservableCollection<string> FontFamily_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<string>();

    /// <summary> フォントファミリ - SelectedIndex </summary>
    public int FontFamily_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> フォントファミリ - Text </summary>
    public string FontFamily_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> フォントファミリ - SelectionChanged </summary>
    public DelegateCommand FontFamily_SelectionChanged { get; private set; }

    /// <summary> フォントサイズ - Value </summary>
    public decimal FontSize_Value
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 背景色

    /// <summary> 背景色 - Background </summary>
    public SolidColorBrush Window_Background
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 背景色 - Command </summary>
    public DelegateCommand ChangeWindowBackground_Command { get; private set; }

    #endregion

    #region プレビュー

    /// <summary> プレビュー - FontFamily </summary>
    public FontFamily Preview_FontFamily
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region デフォルト明細

    /// <summary> 初期表示時にデフォルト明細を表示する - IsChecked </summary>
    public bool ShowDefaultPayslip_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

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
    public HowToSaveImage HowToSaveImage_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 画像の保存方法 - Checked </summary>
    public DelegateCommand HowToSaveImage_Checked { get; private set; }

    /// <summary> フォルダを開く - IsEnabled </summary>
    public bool SelectFolder_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> フォルダを開く - Text </summary>
    public string ImageFolderPath_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> フォルダを開く - Command </summary>
    public DelegateCommand SelectFolder_Command { get; private set; }

    #endregion

    #region 保存

    /// <summary> 保存 - Command </summary>
    public DelegateCommand Save_Command { get; private set; }

    #endregion

    #region 初期値に戻す

    /// <summary> 初期値に戻す - Command </summary>
    public DelegateCommand SetDefault_Command { get; private set; }

    #endregion

}
