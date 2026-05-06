namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - ヘッダ
/// </summary>
public class HeaderViewModel : ViewModelBase<HeaderModel>
{
    public HeaderViewModel()
    {
        this.MainWindow.Header = this.Model;

        this.Model.ViewModel         = this;
        this.Allowance.Header        = this;
        this.Deduction.Header        = this;
        this.WorkingReference.Header = this;
        this.SideBusiness.Header     = this;
        this.WorkPlace.Header        = this;
        this.AnnualCharts.Header     = this;

        this.BindEvents();

        this.Model.Initialize();
    }

    protected override void BindEvents()
    {
        // ←(戻る)
        this.Return_Command = new DelegateCommand(() =>
        {
            this.Model.Return();
            this.Reload();
        });

        // →(進む)
        this.Proceed_Command = new DelegateCommand(() =>
        {
            this.Model.Proceed();
            this.Reload();
        });

        // 年
        this.Year_TextChanged = new DelegateCommand(() =>
        {
            this.Model.IsValid_Year();
            this.Reload();
        });

        // 月
        this.Month_TextChanged = new DelegateCommand(() =>
        {
            this.Model.IsValid_Month();
            this.Reload();
        });
    }

    /// <summary> Model - ヘッダー </summary>
    protected override HeaderModel Model { get; }
        = HeaderModel.GetInstance(new HeaderSQLite());

    /// <summary> Model - メイン画面 </summary>
    public MainWindowModel MainWindow { get; set; }
        = MainWindowModel.GetInstance();

    /// <summary> Model - 月収一覧 </summary>
    public AnnualChartModel AnnualCharts { get; set; }
        = AnnualChartModel.GetInstance();

    /// <summary> Model - 支給額 </summary>
    public AllowanceModel Allowance { get; set; }
        = AllowanceModel.GetInstance(new AllowanceSQLite());

    /// <summary> Model - 控除額 </summary>
    public DeductionModel Deduction { get; set; }
        = DeductionModel.GetInstance(new DeductionSQLite());

    /// <summary> Model - 勤務備考 </summary>
    public WorkingReferenceModel WorkingReference { get; set; }
        = WorkingReferenceModel.GetInstance(new WorkingReferenceSQLite());

    /// <summary> Model - 勤務場所 </summary>
    public WorkPlaceModel WorkPlace { get; set; }
        = WorkPlaceModel.GetInstance();

    /// <summary> Model - 副業 </summary>
    public SideBusinessModel SideBusiness { get; set; }
        = SideBusinessModel.GetInstance(new SideBusinessSQLite());

    /// <summary>
    /// リロード
    /// </summary>
    public void Reload()
    {
        this.Allowance.Reload();
        this.Deduction.Reload();
        this.WorkingReference.Reload();
        this.SideBusiness.Reload();
        this.WorkPlace.Reload();
        this.AnnualCharts.Initialize();
    }

    #region 背景色

    /// <summary> 背景色 - Background </summary>
    public SolidColorBrush Window_Background
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 年

    /// <summary> 年 - Text </summary>
    public int Year_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = DateTime.Now.Year;

    /// <summary> 年 - TextChanged </summary>
    public DelegateCommand Year_TextChanged { get; private set; }

    #endregion

    #region 月

    /// <summary> 月 - Text </summary>
    public int Month_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = DateTime.Now.Month;

    /// <summary> 月 - TextChanged </summary>
    public DelegateCommand Month_TextChanged { get; private set; }

    #endregion

    #region 戻るボタン

    /// <summary> 戻る - Command </summary>
    public DelegateCommand Return_Command { get; private set; }

    #endregion

    #region 進むボタン

    /// <summary> 進む - Command </summary>
    public DelegateCommand Proceed_Command { get; private set; }

    #endregion

}
