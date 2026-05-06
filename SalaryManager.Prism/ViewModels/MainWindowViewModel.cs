namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// Model - メイン画面
/// </summary>
public class MainWindowViewModel : ViewModelBase<MainWindowModel>
{
    /// <summary> タイトル </summary>
    public string Title
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = Shared.SystemName;

    private IDialogService _dialogService;

    public MainWindowViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
        this.Model.ViewModel = this;
        this.Header.MainWindow = this;
        this.WorkingReference.MainWindow = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        // 画面遷移時
        this.Window_Activated = new DelegateCommand(this.Window_Activated_Execute);

        // メニュー - 編集
        this.EditCompany_Command      = new DelegateCommand(() => _dialogService.ShowDialog(nameof(Prism.Views.Company), null, null));
        this.EditCareer_Command       = new DelegateCommand(() => _dialogService.ShowDialog(nameof(Prism.Views.Career), null, null));
        this.EditWorkingPlace_Command = new DelegateCommand(this.Model.EditWorkingPlace);
        this.EditHome_Command         = new DelegateCommand(() => _dialogService.ShowDialog(nameof(Prism.Views.Home), null, null));
        this.EditHoliday_Command      = new DelegateCommand(() => _dialogService.ShowDialog(nameof(Prism.Views.Holiday), null, null));
        this.EditFileStorage_Command  = new DelegateCommand(() => _dialogService.ShowDialog(nameof(Prism.Views.FileStorage), null, null));
        this.EditOption_Command       = new DelegateCommand(this.Model.EditOption);

        // 読込
        this.ReadDefaultPayslip_Command = new DelegateCommand(this.Model.ReadDefaultPayslip);
        this.ReadCSV_Command            = new DelegateCommand(this.Model.ReadCSV);

        // 表示
        this.ShowCurrentPayslip_Command = new DelegateCommand(this.Model.ShowCurrentPayslip);

        // 出力
        this.OutputExcel_Command       = new DelegateCommand(this.Model.OutputExcel);
        this.OutputSpreadSheet_Command = new DelegateCommand(this.Model.OutputSpreadSheet);

        // 保存
        this.SavePayslip_Command        = new DelegateCommand(this.SavePayslip_Command_Execute);
        this.SaveDefaultPayslip_Command = new DelegateCommand(this.Header.SetDefaultPayslip);
        this.SaveDBBackup_Command       = new DelegateCommand(this.Model.SaveDBBackup);
    }

    private void Window_Activated_Execute()
    {
        this.Model.Window_Activated();
        this.Header.Window_Activated();
        this.WorkPlace.Window_Activated();
        this.AnnualChart.Window_Activated();
    }

    /// <summary> Model - ヘッダー </summary>
    protected override MainWindowModel Model { get; }
        = MainWindowModel.GetInstance();

    /// <summary> Model - ヘッダ </summary>
    public HeaderModel Header { get; set; }
        = HeaderModel.GetInstance(new HeaderSQLite());

    /// <summary> Model - 勤務場所 </summary>
    private WorkPlaceModel WorkPlace { get; set; }
        = WorkPlaceModel.GetInstance();

    /// <summary> Model - 月収一覧 </summary>
    private AnnualChartModel AnnualChart { get; set; }
        = AnnualChartModel.GetInstance();

    /// <summary> Model - 勤怠備考 </summary>
    private WorkingReferenceModel WorkingReference { get; set; }
        = WorkingReferenceModel.GetInstance(new WorkingReferenceSQLite());

    #region Window

    /// <summary> Window - Background </summary>
    public SolidColorBrush Window_Background { get; set; }

    /// <summary> Window - FontFamily </summary>
    public FontFamily Window_FontFamily
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - FontSize </summary>
    public decimal Window_FontSize
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - Title </summary>
    public string Window_Title
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = Shared.SystemName;

    /// <summary> Window - Activated </summary>
    public DelegateCommand Window_Activated { get; set; }

    #endregion

    #region メニュー - 編集

    /// <summary> 会社マスタ - Command  </summary>
    public DelegateCommand EditCompany_Command { get; private set; }

    /// <summary> 経歴マスタ - Command </summary>
    public DelegateCommand EditCareer_Command { get; private set; }

    /// <summary> 就業時間マスタ - Command </summary>
    public DelegateCommand EditWorkingPlace_Command { get; private set; }

    /// <summary> 自宅マスタ - Command </summary>
    public DelegateCommand EditHome_Command { get; private set; }

    /// <summary> 祝日マスタ - Command </summary>
    public DelegateCommand EditHoliday_Command { get; private set; }

    /// <summary> 添付ファイル - Command </summary>
    public DelegateCommand EditFileStorage_Command { get; private set; }

    /// <summary> オプション - Command </summary>
    public DelegateCommand EditOption_Command { get; private set; }

    #endregion

    #region メニュー - 読込

    /// <summary> デフォルト明細を取得 - Command </summary>
    public DelegateCommand ReadDefaultPayslip_Command { get; private set; }

    /// <summary> CSV読込 - Command </summary>
    public DelegateCommand ReadCSV_Command { get; private set; }

    #endregion

    #region メニュー - 表示

    /// <summary> 今月の明細 - Command </summary>
    public DelegateCommand ShowCurrentPayslip_Command { get; private set; }

    #endregion

    #region メニュー - 出力

    /// <summary> Excel - Command </summary>
    public DelegateCommand OutputExcel_Command { get; private set; }

    /// <summary> SpreadSheet - Command </summary>
    public DelegateCommand OutputSpreadSheet_Command { get; private set; }

    #endregion

    #region メニュー - 保存

    /// <summary> 給与明細 - Command </summary>
    public DelegateCommand SavePayslip_Command { get; private set; }

    /// <summary> デフォルト明細 - Command </summary>
    public DelegateCommand SaveDefaultPayslip_Command { get; private set; }

    /// <summary> DBのバックアップを作成する - Command </summary>
    public DelegateCommand SaveDBBackup_Command { get; private set; }

    /// <summary>
    /// 保存ボタンの実行
    /// </summary>
    private void SavePayslip_Command_Execute()
    {
        this.Model.SavePayslip();
        this.AnnualChart.Initialize();
    }

    #endregion

    #region 金額の比較用

    /// <summary> 金額の比較用 - Content </summary>
    public string PriceUpdown_Content { get; set; } = string.Empty;

    /// <summary> 金額の比較用 - Foreground </summary>
    public SolidColorBrush PriceUpdown_Foreground { get; set; } = new SolidColorBrush();

    #endregion
}
