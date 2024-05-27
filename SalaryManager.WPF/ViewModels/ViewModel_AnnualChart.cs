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
    }

    protected override void BindEvents()
    {
        throw new NotImplementedException();
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

    #region 1月

    /// <summary> 1月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> January_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 1月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> January_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 1月 - 副業額 - Text </summary>
    public ReactiveProperty<int> January_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 2月

    /// <summary> 2月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> Feburary_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 2月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> Feburary_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 2月 - 副業額 - Text </summary>
    public ReactiveProperty<int> Feburary_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 3月

    /// <summary> 3月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> March_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 3月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> March_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 3月 - 副業額 - Text </summary>
    public ReactiveProperty<int> March_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 4月

    /// <summary> 4月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> April_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 4月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> April_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 4月 - 副業額 - Text </summary>
    public ReactiveProperty<int> April_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 5月

    /// <summary> 5月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> May_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 5月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> May_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 5月 - 副業額 - Text </summary>
    public ReactiveProperty<int> May_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 6月

    /// <summary> 6月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> June_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 6月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> June_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 6月 - 副業額 - Text </summary>
    public ReactiveProperty<int> June_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 7月

    /// <summary> 7月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> July_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 7月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> July_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 7月 - 副業額 - Text </summary>
    public ReactiveProperty<int> July_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 8月

    /// <summary> 8月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> August_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 8月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> August_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 8月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> August_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 9月

    /// <summary> 9月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> September_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 9月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> September_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 9月 - 副業額 - Text </summary>
    public ReactiveProperty<int> September_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 10月

    /// <summary> 10月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> October_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 10月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> October_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 10月 - 副業額 - Text </summary>
    public ReactiveProperty<int> October_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 11月

    /// <summary> 11月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> November_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 11月 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> November_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 11月 - 副業額 - Text </summary>
    public ReactiveProperty<int> November_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 12月

    /// <summary> 12月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> December_TotalSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 12月 - 支給額計 - Text </summary>
    public ReactiveProperty<int> December_TotalDeductedSalary_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 12月 - 副業額 - Text </summary>
    public ReactiveProperty<int> December_TotalSideBusiness_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 合計

    /// <summary> 合計 - 支給額計 - Text </summary>
    public ReactiveProperty<int> TotalSalary_Sum_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 合計 - 差引支給額 - Text </summary>
    public ReactiveProperty<int> TotalDeductedSalary_Sum_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 合計 - 副業額 - Text </summary>
    public ReactiveProperty<int> TotalSideBusiness_Sum_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

}
