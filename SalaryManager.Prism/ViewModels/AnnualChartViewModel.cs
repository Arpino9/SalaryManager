namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 月収一覧
/// </summary>
public class AnnualChartViewModel : ViewModelBase<AnnualChartModel>
{
    public AnnualChartViewModel()
    {
        this.Model.ViewModel = this;
        this.MainWindow.AnnualChart = this;

        base.Window_Activated();

        this.Model.Initialize();

        this.BindEvents();
    }

    /// <summary>
    /// 単体テスト用のコンストラクタ
    /// </summary>
    /// <remarks>
    /// Initialize() を呼ばず DB / XML アクセスを回避する。
    /// </remarks>
    public AnnualChartViewModel(bool isTest)
    {
        this.Model.ViewModel        = this;
        this.MainWindow.AnnualChart = this;
        this.Model.Clear();
    }

    protected override void BindEvents()
    {
        // 項目共通
        this.Default_MouseLeave = new DelegateCommand(() => this.MainWindow.ComparePrice(0, 0));

        // 1月
        this.January_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.January_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Jan));

        this.January_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.January_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Jan));

        this.January_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.January_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Jan));

        // 2月
        this.Feburary_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.Feburary_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Feb));

        this.Feburary_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.Feburary_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Feb));

        this.Feburary_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.Feburary_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Feb));

        // 3月
        this.March_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.March_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Mar));

        this.March_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.March_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Mar));

        this.March_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.March_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Mar));

        // 4月
        this.April_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.April_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Apr));

        this.April_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.April_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Apr));

        this.April_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.April_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Apr));

        // 5月
        this.May_TotalSalary_MouseMove = new DelegateCommand(() =>
             this.MainWindow.ComparePrice(this.March_TotalSalary_Text,
                                          this.Model.PreviousTotalSalary_Mar));

        this.May_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.May_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_May));

        this.May_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
             this.MainWindow.ComparePrice(this.May_TotalSideBusiness_Text,
                                          this.Model.PreviousSideBusiness_May));

        // 6月
        this.June_TotalSalary_MouseMove = new DelegateCommand(() =>
             this.MainWindow.ComparePrice(this.June_TotalSalary_Text,
                                          this.Model.PreviousTotalSalary_Jun));

        this.June_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
             this.MainWindow.ComparePrice(this.June_TotalDeductedSalary_Text,
                                          this.Model.PreviousTotalDeducedSalary_Jun));

        this.June_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
             this.MainWindow.ComparePrice(this.June_TotalSideBusiness_Text,
                                          this.Model.PreviousSideBusiness_Jun));

        // 7月
        this.July_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.July_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Jul));

        this.July_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.July_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Jul));

        this.July_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.July_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Jul));

        // 8月
        this.August_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.August_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Aug));

        this.August_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.August_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Aug));

        this.August_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.August_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Aug));

        // 9月
        this.September_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.September_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Sep));

        this.September_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.September_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Sep));

        this.September_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.September_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Sep));

        // 10月
        this.October_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.October_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Oct));

        this.October_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.October_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Oct));

        this.October_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.October_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Oct));

        // 11月
        this.November_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.November_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Nov));

        this.November_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.November_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Nov));

        this.November_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.November_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Nov));

        // 12月
        this.December_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.December_TotalSalary_Text,
                                         this.Model.PreviousTotalSalary_Dec));

        this.December_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.December_TotalDeductedSalary_Text,
                                         this.Model.PreviousTotalDeducedSalary_Dec));

        this.December_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.December_TotalSideBusiness_Text,
                                         this.Model.PreviousSideBusiness_Dec));

        // 合計
        this.Sum_TotalSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.TotalSalary_Sum_Text,
                                         this.Model.PreviousTotalSalary_Sum));

        this.Sum_TotalDeductedSalary_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.TotalDeductedSalary_Sum_Text,
                                         this.Model.PreviousTotalDeducedSalary_Sum));

        this.Sum_TotalSideBusiness_MouseMove = new DelegateCommand(() =>
            this.MainWindow.ComparePrice(this.TotalSideBusiness_Sum_Text,
                                         this.Model.PreviousSideBusiness_Sum));
    }

    /// <summary> Model - 月収一覧 </summary>
    protected override AnnualChartModel Model { get; }
        = AnnualChartModel.GetInstance();

    /// <summary> Model - メイン画面 </summary>
    public MainWindowModel MainWindow { get; set; }
        = MainWindowModel.GetInstance();

    #region 対象日付

    /// <summary> 対象日付 - Content </summary>
    public string TargetDate_Content
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    /// <summary> 初期状態 - MouseLeave </summary>
    public DelegateCommand Default_MouseLeave { get; private set; }

    #region 1月

    /// <summary> 1月 - 支給額計 - Text </summary>
    public int January_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 1月 - 支給額計 - MouseMove </summary>
    public DelegateCommand January_TotalSalary_MouseMove { get; private set; }

    /// <summary> 1月 - 差引支給額 - Text </summary>
    public int January_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 1月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand January_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 1月 - 副業額 - Text </summary>
    public int January_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 1月 - 副業額 - MouseMove </summary>
    public DelegateCommand January_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 2月

    /// <summary> 2月 - 支給額計 - Text </summary>
    public int Feburary_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 2月 - 支給額計 - MouseMove </summary>
    public DelegateCommand Feburary_TotalSalary_MouseMove { get; private set; }

    /// <summary> 2月 - 差引支給額 - Text </summary>
    public int Feburary_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 2月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand Feburary_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 2月 - 副業額 - Text </summary>
    public int Feburary_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 2月 - 副業額 - MouseMove </summary>
    public DelegateCommand Feburary_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 3月

    /// <summary> 3月 - 支給額計 - Text </summary>
    public int March_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 3月 - 支給額計 - MouseMove </summary>
    public DelegateCommand March_TotalSalary_MouseMove { get; private set; }

    /// <summary> 3月 - 差引支給額 - Text </summary>
    public int March_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 3月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand March_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 3月 - 副業額 - Text </summary>
    public int March_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 3月 - 副業額 - MouseMove </summary>
    public DelegateCommand March_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 4月

    /// <summary> 4月 - 支給額計 - Text </summary>
    public int April_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 4月 - 支給額計 - MouseMove </summary>
    public DelegateCommand April_TotalSalary_MouseMove { get; private set; }

    /// <summary> 4月 - 差引支給額 - Text </summary>
    public int April_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 4月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand April_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 4月 - 副業額 - Text </summary>
    public int April_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 4月 - 副業額 - MouseMove </summary>
    public DelegateCommand April_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 5月

    /// <summary> 5月 - 支給額計 - Text </summary>
    public int May_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 5月 - 支給額計 - MouseMove </summary>
    public DelegateCommand May_TotalSalary_MouseMove { get; private set; }

    /// <summary> 5月 - 差引支給額 - Text </summary>
    public int May_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 5月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand May_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 5月 - 副業額 - Text </summary>
    public int May_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 5月 - 副業額 - MouseMove </summary>
    public DelegateCommand May_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 6月

    /// <summary> 6月 - 支給額計 - Text </summary>
    public int June_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 6月 - 支給額計 - MouseMove </summary>
    public DelegateCommand June_TotalSalary_MouseMove { get; private set; }

    /// <summary> 6月 - 支給額計 - Text </summary>
    public int June_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 6月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand June_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 6月 - 副業額 - Text </summary>
    public int June_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 6月 - 副業額 - MouseMove </summary>
    public DelegateCommand June_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 7月

    /// <summary> 7月 - 支給額計 - Text </summary>
    public int July_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 7月 - 支給額計 - MouseMove </summary>
    public DelegateCommand July_TotalSalary_MouseMove { get; private set; }

    /// <summary> 7月 - 差引支給額 - Text </summary>
    public int July_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 7月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand July_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 7月 - 副業額 - Text </summary>
    public int July_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 7月 - 副業額 - MouseMove </summary>
    public DelegateCommand July_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 8月

    /// <summary> 8月 - 支給額計 - Text </summary>
    public int August_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 8月 - 支給額計 - MouseMove </summary>
    public DelegateCommand August_TotalSalary_MouseMove { get; private set; }

    /// <summary> 8月 - 差引支給額 - Text </summary>
    public int August_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 8月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand August_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 8月 - 副業額 - Text </summary>
    public int August_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 8月 - 副業額 - MouseMove </summary>
    public DelegateCommand August_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 9月

    /// <summary> 9月 - 支給額計 - Text </summary>
    public int September_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 9月 - 支給額計 - MouseMove </summary>
    public DelegateCommand September_TotalSalary_MouseMove { get; private set; }

    /// <summary> 9月 - 差引支給額 - Text </summary>
    public int September_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 9月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand September_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 9月 - 副業額 - Text </summary>
    public int September_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 9月 - 副業額 - MouseMove </summary>
    public DelegateCommand September_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 10月

    /// <summary> 10月 - 支給額計 - Text </summary>
    public int October_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 10月 - 支給額計 - MouseMove </summary>
    public DelegateCommand October_TotalSalary_MouseMove { get; private set; }

    /// <summary> 10月 - 差引支給額 - Text </summary>
    public int October_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 10月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand October_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 10月 - 副業額 - Text </summary>
    public int October_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 10月 - 副業額 - MouseMove </summary>
    public DelegateCommand October_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 11月

    /// <summary> 11月 - 支給額計 - Text </summary>
    public int November_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 10月 - 支給額計 - MouseMove </summary>
    public DelegateCommand November_TotalSalary_MouseMove { get; private set; }

    /// <summary> 11月 - 差引支給額 - Text </summary>
    public int November_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 11月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand November_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 11月 - 副業額 - Text </summary>
    public int November_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 11月 - 副業額 - MouseMove </summary>
    public DelegateCommand November_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 12月

    /// <summary> 12月 - 支給額計 - Text </summary>
    public int December_TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 12月 - 支給額計 - MouseMove </summary>
    public DelegateCommand December_TotalSalary_MouseMove { get; private set; }

    /// <summary> 12月 - 支給額計 - Text </summary>
    public int December_TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 12月 - 差引支給額 - MouseMove </summary>
    public DelegateCommand December_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 12月 - 副業額 - Text </summary>
    public int December_TotalSideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 12月 - 副業額 - MouseMove </summary>
    public DelegateCommand December_TotalSideBusiness_MouseMove { get; private set; }

    #endregion

    #region 合計

    /// <summary> 合計 - 支給額計 - Text </summary>
    public int TotalSalary_Sum_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 合計 - 支給額計 - MouseMove </summary>
    public DelegateCommand Sum_TotalSalary_MouseMove { get; private set; }

    /// <summary> 合計 - 差引支給額 - Text </summary>
    public int TotalDeductedSalary_Sum_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 合計 - 差引支給額 - MouseMove </summary>
    public DelegateCommand Sum_TotalDeductedSalary_MouseMove { get; private set; }

    /// <summary> 合計 - 副業額 - Text </summary>
    public int TotalSideBusiness_Sum_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 合計 - 副業額 - MouseMove </summary>
    public DelegateCommand Sum_TotalSideBusiness_MouseMove { get; private set; }

    #endregion
}
