namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - 月収一覧
/// </summary>
public class ViewModel_AnnualChart : ViewModelBase<Model_AnnualChart>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_AnnualChart()
    {
        this.Model.ViewModel = this;
        this.MainWindow.AnnualChart = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        // 項目共通
        this.Default_MouseLeave.Subscribe(_ => this.MainWindow.ComparePrice(0, 0));

        // 1月
        this.January_TotalSalary_MouseMove.Subscribe(_ => 
            this.MainWindow.ComparePrice(this.January_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Jan));

        this.January_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.January_TotalDeductedSalary_Text.Value, 
                                         this.Model.PreviousTotalDeducedSalary_Jan));

        this.January_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.January_TotalSideBusiness_Text.Value, 
                                         this.Model.PreviousSideBusiness_Jan));

        // 2月
        this.Feburary_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.Feburary_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Feb));

        this.Feburary_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.Feburary_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Feb));

        this.Feburary_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.Feburary_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Feb));

        // 3月
        this.March_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.March_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Mar));

        this.March_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.March_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Mar));

        this.March_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.March_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Mar));

        // 4月
        this.April_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.April_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Apr));

        this.April_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.April_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Apr));

        this.April_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.April_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Apr));

        // 5月
        this.May_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.May_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_May));

        this.May_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.May_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_May));

        this.May_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.May_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_May));

        // 6月
        this.June_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.June_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Jun));

        this.June_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.June_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Jun));

        this.June_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.June_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Jun));

        // 7月
        this.July_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.July_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Jul));

        this.July_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.July_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Jul));

        this.July_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.July_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Jul));

        // 8月
        this.August_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.August_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Aug));

        this.August_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.August_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Aug));

        this.August_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.August_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Aug));

        // 9月
        this.September_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.September_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Sep));

        this.September_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.September_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Sep));

        this.September_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.September_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Sep));

        // 10月
        this.October_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.October_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Oct));

        this.October_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.October_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Oct));

        this.October_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.October_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Oct));

        // 11月
        this.November_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.November_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Nov));

        this.November_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.November_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Nov));

        this.November_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.November_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Nov));

        // 12月
        this.December_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.December_TotalSalary_Text.Value,
                                         this.Model.PreviousTotalSalary_Dec));

        this.December_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.December_TotalDeductedSalary_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Dec));

        this.December_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.December_TotalSideBusiness_Text.Value,
                                         this.Model.PreviousSideBusiness_Dec));

        // 合計
        this.Sum_TotalSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.TotalSalary_Sum_Text.Value,
                                         this.Model.PreviousTotalSalary_Sum));

        this.Sum_TotalDeductedSalary_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.TotalDeductedSalary_Sum_Text.Value,
                                         this.Model.PreviousTotalDeducedSalary_Sum));

        this.Sum_TotalSideBusiness_MouseMove.Subscribe(_ =>
            this.MainWindow.ComparePrice(this.TotalSideBusiness_Sum_Text.Value,
                                         this.Model.PreviousSideBusiness_Sum));
    }

    /// <summary> Model - 月収一覧 </summary>
    protected override Model_AnnualChart Model { get; } 
        = Model_AnnualChart.GetInstance();

    /// <summary> Model - メイン画面 </summary>
    public Model_MainWindow MainWindow { get; set; } 
        = Model_MainWindow.GetInstance();

    #region 背景色

    /// <summary> 背景色 - Background </summary>
    public ReactiveProperty<SolidColorBrush> Window_Background { get; set; }
        = new ReactiveProperty<SolidColorBrush>();

    #endregion

    #region 対象日付

    /// <summary> 対象日付 - Content </summary>
    public ReactiveProperty<string> TargetDate_Content { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    /// <summary> 初期状態 - MouseLeave </summary>
    public ReactiveCommand Default_MouseLeave { get; private set; }
        = new ReactiveCommand();

    #region 1月

    /// <summary> 1月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> January_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 1月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand January_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 1月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> January_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 1月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand January_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 1月 - 副業額 - Text </summary>
    public ReactiveProperty<int> January_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 1月 - 副業額 - MouseMove </summary>
    public ReactiveCommand January_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 2月

    /// <summary> 2月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> Feburary_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 2月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand Feburary_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 2月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> Feburary_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 2月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand Feburary_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 2月 - 副業額 - Text </summary>
    public ReactiveProperty<int> Feburary_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 2月 - 副業額 - MouseMove </summary>
    public ReactiveCommand Feburary_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 3月

    /// <summary> 3月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> March_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 3月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand March_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 3月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> March_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 3月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand March_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 3月 - 副業額 - Text </summary>
    public ReactiveProperty<int> March_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 3月 - 副業額 - MouseMove </summary>
    public ReactiveCommand March_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 4月

    /// <summary> 4月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> April_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 4月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand April_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 4月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> April_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 4月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand April_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 4月 - 副業額 - Text </summary>
    public ReactiveProperty<int> April_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 4月 - 副業額 - MouseMove </summary>
    public ReactiveCommand April_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 5月

    /// <summary> 5月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> May_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 5月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand May_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 5月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> May_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 5月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand May_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 5月 - 副業額 - Text </summary>
    public ReactiveProperty<int> May_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 5月 - 副業額 - MouseMove </summary>
    public ReactiveCommand May_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 6月

    /// <summary> 6月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> June_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 6月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand June_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 6月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> June_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 6月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand June_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 6月 - 副業額 - Text </summary>
    public ReactiveProperty<int> June_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 6月 - 副業額 - MouseMove </summary>
    public ReactiveCommand June_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 7月

    /// <summary> 7月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> July_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 7月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand July_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 7月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> July_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 7月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand July_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 7月 - 副業額 - Text </summary>
    public ReactiveProperty<int> July_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 7月 - 副業額 - MouseMove </summary>
    public ReactiveCommand July_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 8月

    /// <summary> 8月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> August_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 8月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand August_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 8月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> August_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 8月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand August_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 8月 - 副業額 - Text </summary>
    public ReactiveProperty<int> August_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 8月 - 副業額 - MouseMove </summary>
    public ReactiveCommand August_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 9月

    /// <summary> 9月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> September_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 9月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand September_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 9月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> September_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 9月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand September_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 9月 - 副業額 - Text </summary>
    public ReactiveProperty<int> September_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 9月 - 副業額 - MouseMove </summary>
    public ReactiveCommand September_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 10月

    /// <summary> 10月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> October_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 10月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand October_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 10月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> October_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 10月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand October_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 10月 - 副業額 - Text </summary>
    public ReactiveProperty<int> October_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 10月 - 副業額 - MouseMove </summary>
    public ReactiveCommand October_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 11月

    /// <summary> 11月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> November_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 10月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand November_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 11月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> November_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 11月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand November_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 11月 - 副業額 - Text </summary>
    public ReactiveProperty<int> November_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 11月 - 副業額 - MouseMove </summary>
    public ReactiveCommand November_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 12月

    /// <summary> 12月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> December_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 12月 - 支給額計 - MouseMove </summary>
    public ReactiveCommand December_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 12月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> December_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 12月 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand December_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 12月 - 副業額 - Text </summary>
    public ReactiveProperty<int> December_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 12月 - 副業額 - MouseMove </summary>
    public ReactiveCommand December_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 合計

    /// <summary> 合計 - 支給額計 - Text </summary>
    public ReactiveProperty<int> TotalSalary_Sum_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 合計 - 支給額計 - MouseMove </summary>
    public ReactiveCommand Sum_TotalSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 合計 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> TotalDeductedSalary_Sum_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 合計 - 差引支給額 - MouseMove </summary>
    public ReactiveCommand Sum_TotalDeductedSalary_MouseMove { get; private set; }
        = new ReactiveCommand();

    /// <summary> 合計 - 副業額 - Text </summary>
    public ReactiveProperty<int> TotalSideBusiness_Sum_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 合計 - 副業額 - MouseMove </summary>
    public ReactiveCommand Sum_TotalSideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

}
