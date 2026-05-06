namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 支給額
/// </summary>
public class AllowanceViewModel : ViewModelBase<AllowanceModel>
{
    public AllowanceViewModel()
    {
        this.MainWindow.Allowance = this.Model;

        this.Model_Deduction.Allowance = this.Model;

        this.Model.ViewModel = this;
        this.Model.Initialize();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        var entity = this.Model.Entity_LastYear;

        // 初期状態
        Default_MouseLeave = new DelegateCommand(() => this.MainWindow.ComparePrice(0, 0));

        // 基本給
        BasicSalary_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.BasicSalary_Text, entity?.BasicSalary.Value));
        BasicSalary_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 役職手当
        ExecutiveAllowance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.ExecutiveAllowance_Text, entity?.ExecutiveAllowance.Value));
        ExecutiveAllowance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());
        
        // 扶養手当
        DependencyAllowance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.DependencyAllowance_Text, entity?.DependencyAllowance.Value));
        DependencyAllowance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 時間外手当
        OvertimeAllowance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.OvertimeAllowance_Text, entity?.OvertimeAllowance.Value));
        OvertimeAllowance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 休日割増
        DaysoffIncreased_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.DaysoffIncreased_Text, entity?.DaysoffIncreased.Value));
        DaysoffIncreased_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 交通費
        TransportationExpenses_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.TransportationExpenses_Text, entity?.TransportationExpenses.Value));
        TransportationExpenses_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 前払退職金
        PrepaidRetirementPayment_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.PrepaidRetirementPayment_Text, entity?.PrepaidRetirementPayment.Value));
        PrepaidRetirementPayment_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 深夜割増
        NightworkIncreased_MouseMove    = new DelegateCommand(() => this.MainWindow.ComparePrice(this.NightworkIncreased_Text, entity?.NightworkIncreased.Value));
        NightworkIncreased_TextChanged  = new DelegateCommand(() => this.Model.ReCaluculate());

        // 住宅手当
        HousingAllowance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.HousingAllowance_Text, entity?.HousingAllowance.Value));
        HousingAllowance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 遅刻早退欠勤
        LateAbsent_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.LateAbsent_Text, entity?.LateAbsent));
        LateAbsent_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 特別手当
        SpecialAllowance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.SpecialAllowance_Text, entity?.SpecialAllowance));
        SpecialAllowance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 在宅手当
        ElectricityAllowance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.ElectricityAllowance_Text, entity?.ElectricityAllowance.Value));
        ElectricityAllowance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 予備
        SpareAllowance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.SpareAllowance_Text, entity?.SpareAllowance));
        SpareAllowance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 支給総計
        TotalSalary_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.TotalSalary_Text, entity?.TotalSalary.Value));

        // 差引支給額
        TotalDeductedSalary_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.TotalDeductedSalary_Text, entity?.TotalDeductedSalary.Value));
    }

    /// <summary> Model - 支給額 </summary>
    protected override AllowanceModel Model { get; }
        = AllowanceModel.GetInstance(new AllowanceSQLite());

    /// <summary> Model - 控除額 </summary>
    public DeductionModel Model_Deduction { get; set; }
        = DeductionModel.GetInstance(new DeductionSQLite());

    /// <summary> Model - メイン画面 </summary>
    public MainWindowModel MainWindow { get; set; }
        = MainWindowModel.GetInstance();

    #region Window

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

    /// <summary> Window - Background </summary>
    public Brush Window_Background
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 初期状態

    /// <summary> 初期状態 - MouseLeave </summary>
    public DelegateCommand Default_MouseLeave { get; private set; }

    #endregion

    #region 基本給

    /// <summary> 基本給 - Text </summary>
    public double BasicSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 基本給 - MouseLeave </summary>
    public DelegateCommand BasicSalary_MouseMove { get; private set; }

    /// <summary> 基本給 - TextChanged </summary>
    public DelegateCommand BasicSalary_TextChanged { get; private set; }

    #endregion

    #region 役職手当

    /// <summary> 役職手当 - IsEnabled </summary>
    public bool ExecutiveAllowance_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 役職手当 - Text </summary>
    public double ExecutiveAllowance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 役職手当 - MouseMove </summary>
    public DelegateCommand ExecutiveAllowance_MouseMove { get; private set; }

    /// <summary> 役職手当 - TextChanged </summary>
    public DelegateCommand ExecutiveAllowance_TextChanged { get; private set; }

    #endregion

    #region 扶養手当

    /// <summary> 扶養手当 - IsEnabled </summary>
    public bool DependencyAllowance_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 扶養手当 - Text </summary>
    public double DependencyAllowance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 扶養手当 - MouseMove </summary>
    public DelegateCommand DependencyAllowance_MouseMove { get; private set; }

    /// <summary> 扶養手当 - TextChanged </summary>
    public DelegateCommand DependencyAllowance_TextChanged { get; private set; }

    #endregion

    #region 時間外手当

    /// <summary> 時間外手当 - IsEnabled </summary>
    public bool OvertimeAllowance_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 時間外手当 - Text </summary>
    public double OvertimeAllowance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 時間外手当 - MouseMove </summary>
    public DelegateCommand OvertimeAllowance_MouseMove { get; private set; }

    /// <summary> 時間外手当 - TextChanged </summary>
    public DelegateCommand OvertimeAllowance_TextChanged { get; private set; }

    #endregion

    #region 休日割増

    /// <summary> 休日割増 - Text </summary>
    public double DaysoffIncreased_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 休日割増 - MouseMove </summary>
    public DelegateCommand DaysoffIncreased_MouseMove { get; private set; }

    /// <summary> 休日割増 - TextChanged </summary>
    public DelegateCommand DaysoffIncreased_TextChanged { get; private set; }

    #endregion

    #region 交通費

    /// <summary> 交通費 - IsEnabled </summary>
    public bool TransportationExpenses_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 交通費 - Text </summary>
    public double TransportationExpenses_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 交通費 - MouseMove </summary>
    public DelegateCommand TransportationExpenses_MouseMove { get; private set; }

    /// <summary> 交通費 - TextChanged </summary>
    public DelegateCommand TransportationExpenses_TextChanged { get; private set; }

    #endregion

    #region 前払退職金

    /// <summary> 前払退職金 - IsEnabled </summary>
    public bool PrepaidRetirementPayment_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 前払退職金 - Text </summary>
    public double PrepaidRetirementPayment_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 前払退職金 - MouseMove </summary>
    public DelegateCommand PrepaidRetirementPayment_MouseMove { get; private set; }

    /// <summary> 前払退職金 - TextChanged </summary>
    public DelegateCommand PrepaidRetirementPayment_TextChanged { get; private set; }

    #endregion

    #region 深夜割増

    /// <summary> 深夜割増 - IsEnabled </summary>
    public bool NightworkIncreased_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 深夜割増 - Text </summary>
    public double NightworkIncreased_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 深夜割増 - MouseMove </summary>
    public DelegateCommand NightworkIncreased_MouseMove { get; private set; }

    /// <summary> 深夜割増 - TextChanged </summary>
    public DelegateCommand NightworkIncreased_TextChanged { get; private set; }

    #endregion

    #region 住宅手当

    /// <summary> 住宅手当 - IsEnabled </summary>
    public bool HousingAllowance_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 住宅手当 - Text </summary>
    public double HousingAllowance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 住宅手当 - MouseMove </summary>
    public DelegateCommand HousingAllowance_MouseMove { get; private set; }

    /// <summary> 住宅手当 - TextChanged </summary>
    public DelegateCommand HousingAllowance_TextChanged { get; private set; }

    #endregion

    #region 遅刻早退欠勤

    /// <summary> 遅刻早退欠勤 - Text </summary>
    public double LateAbsent_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 遅刻早退欠勤 - MouseMove </summary>
    public DelegateCommand LateAbsent_MouseMove { get; private set; }

    /// <summary> 遅刻早退欠勤 - TextChanged </summary>
    public DelegateCommand LateAbsent_TextChanged { get; private set; }

    #endregion

    #region 特別手当

    /// <summary> 特別手当 - IsEnabled </summary>
    public bool SpecialAllowance_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 特別手当 - Text </summary>
    public double SpecialAllowance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 特別手当 - MouseMove </summary>
    public DelegateCommand SpecialAllowance_MouseMove { get; private set; }

    /// <summary> 特別手当 - TextChanged </summary>
    public DelegateCommand SpecialAllowance_TextChanged { get; private set; }

    #endregion

    #region 在宅手当

    /// <summary> 在宅手当 - IsEnabled </summary>
    public bool ElectricityAllowance_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 在宅手当 - Text </summary>
    public double ElectricityAllowance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 在宅手当 - MouseMove </summary>
    public DelegateCommand ElectricityAllowance_MouseMove { get; private set; }

    /// <summary> 在宅手当 - TextChanged </summary>
    public DelegateCommand ElectricityAllowance_TextChanged { get; private set; }

    #endregion

    #region 予備

    /// <summary> 予備 - Text </summary>
    public double SpareAllowance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 予備 - MouseMove </summary>
    public DelegateCommand SpareAllowance_MouseMove { get; private set; }

    /// <summary> 予備 - TextChanged </summary>
    public DelegateCommand SpareAllowance_TextChanged { get; private set; }

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public string Remarks_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 支給総計

    /// <summary> 支給総計 - Foreground </summary>
    public SolidColorBrush TotalSalary_Foreground
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new SolidColorBrush(Colors.Blue);

    /// <summary> 支給総計 - Text </summary>
    public double TotalSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 支給総計 - MouseMove </summary>
    public DelegateCommand TotalSalary_MouseMove { get; private set; }

    #endregion

    #region 差引支給額

    /// <summary> 差引支給額 - Foreground </summary>
    public SolidColorBrush TotalDeductedSalary_Foreground
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 差引支給額 - Text </summary>
    public double TotalDeductedSalary_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 差引支給額 - MouseMove </summary>
    public DelegateCommand TotalDeductedSalary_MouseMove { get; private set; }

    #endregion

}
