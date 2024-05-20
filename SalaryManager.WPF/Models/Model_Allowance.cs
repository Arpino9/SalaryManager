namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 支給額
/// </summary>
public class Model_Allowance : IInputPayslip
{

    #region Get Instance

    private static Model_Allowance model = null;

    public static Model_Allowance GetInstance(IAllowanceRepository repository)
    {
        if (model == null)
        {
            model = new Model_Allowance(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private IAllowanceRepository _repository;

    public Model_Allowance(IAllowanceRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 支給額 </summary>
    internal ViewModel_Header Header { get; set; }

    /// <summary> ViewModel - 支給額 </summary>
    internal ViewModel_Allowance ViewModel { get; set; }

    /// <summary> ViewModel - 控除額 </summary>
    internal ViewModel_Deduction ViewModel_Deduction { get; set; }

    /// <summary> ViewModel - 勤務先 </summary>
    internal ViewModel_WorkPlace ViewModel_WorkPlace { get; set; }

    /// <summary> Entity - 支給額 </summary>
    public AllowanceValueEntity Entity { get; set; }

    /// <summary> Entity - 支給額 (昨年度) </summary>
    public AllowanceValueEntity Entity_LastYear { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="entityDate">初期化する日付</param>
    /// <remarks>
    /// 画面起動時に、項目を初期化する。
    /// </remarks>
    public void Initialize(DateTime entityDate)
    {
        Allowances.Create(_repository);

        this.Entity          = Allowances.Fetch(entityDate.Year, entityDate.Month);
        this.Entity_LastYear = Allowances.Fetch(entityDate.Year, entityDate.Month - 1);

        var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

        if (this.Entity is null && showDefaultPayslip)
        {
            // デフォルト明細
            this.Entity = Allowances.FetchDefault();
        }

        this.Refresh();
    }

    /// <summary>
    /// 再描画
    /// </summary>
    /// <remarks>
    /// 該当月に支給額と手当有無が存在すれば、各項目を再描画する。
    /// </remarks>
    public void Refresh()
    {
        // 所属会社名
        Careers.Create(new CareerSQLite());
        var company = Careers.FetchCompany(new DateTime(this.Header.Year_Text.Value, this.Header.Month_Text.Value, 1));
        if (company is null)
        {
            return;
        }

        // 手当有無
        var existence = Careers.FetchAllowanceExistence(new CompanyNameValue(company));
        if (existence != null)
        {
            this.ViewModel.ExecutiveAllowance_IsEnabled.Value       = existence.Executive.Value;
            this.ViewModel.DependencyAllowance_IsEnabled.Value      = existence.Dependency.Value;
            this.ViewModel.OvertimeAllowance_IsEnabled.Value        = existence.Overtime.Value;
            this.ViewModel.NightworkIncreased_IsEnabled.Value       = existence.LateNight.Value;
            this.ViewModel.HousingAllowance_IsEnabled.Value         = existence.Housing.Value;
            this.ViewModel.TransportationExpenses_IsEnabled.Value   = existence.Commution.Value;
            this.ViewModel.PrepaidRetirementPayment_IsEnabled.Value = existence.PrepaidRetirement.Value;
            this.ViewModel.ElectricityAllowance_IsEnabled.Value     = existence.Electricity.Value;
            this.ViewModel.SpecialAllowance_IsEnabled.Value         = existence.Special.Value;
        }

        if (this.Entity is null)
        {
            this.Clear();
            return;
        }

        // 基本給
        this.ViewModel.BasicSalary_Text.Value              = this.Entity.BasicSalary.Value;
        // 役職手当
        this.ViewModel.ExecutiveAllowance_Text.Value       = this.Entity.ExecutiveAllowance.Value;
        // 扶養手当
        this.ViewModel.DependencyAllowance_Text.Value      = this.Entity.DependencyAllowance.Value;
        // 時間外手当
        this.ViewModel.OvertimeAllowance_Text.Value        = this.Entity.OvertimeAllowance.Value;
        // 休日割増
        this.ViewModel.DaysoffIncreased_Text.Value         = this.Entity.DaysoffIncreased.Value;
        // 深夜割増
        this.ViewModel.NightworkIncreased_Text.Value       = this.Entity.NightworkIncreased.Value;
        // 住宅手当
        this.ViewModel.HousingAllowance_Text.Value         = this.Entity.HousingAllowance.Value;
        // 遅刻早退欠勤
        this.ViewModel.LateAbsent_Text.Value               = this.Entity.LateAbsent;
        // 交通費
        this.ViewModel.TransportationExpenses_Text.Value   = this.Entity.TransportationExpenses.Value;
        // 前払退職金
        this.ViewModel.PrepaidRetirementPayment_Text.Value = this.Entity.PrepaidRetirementPayment.Value;
        // 在宅手当
        this.ViewModel.ElectricityAllowance_Text.Value     = this.Entity.ElectricityAllowance.Value;
        // 特別手当
        this.ViewModel.SpecialAllowance_Text.Value         = this.Entity.SpecialAllowance;
        // 予備
        this.ViewModel.SpareAllowance_Text.Value           = this.Entity.SpareAllowance;
        // 備考
        this.ViewModel.Remarks_Text.Value                  = this.Entity.Remarks;
        // 支給総計、差引支給額
        this.ReCaluculate();
    }

    /// <summary>
    /// リロード
    /// </summary>
    /// <remarks>
    /// 年月の変更時などに、該当月の項目を取得する。
    /// </remarks>
    public void Reload()
    {
        using (var cursor = new CursorWaiting())
        {
            Allowances.Create(_repository);

            this.Entity          = Allowances.Fetch(this.Header.Year_Text.Value,     this.Header.Month_Text.Value);
            this.Entity_LastYear = Allowances.Fetch(this.Header.Year_Text.Value - 1, this.Header.Month_Text.Value);

            this.Refresh();
        }
    }

    /// <summary>
    /// クリア
    /// </summary>
    /// <remarks>
    /// 各項目を初期化する。
    /// </remarks>
    public void Clear()
    {
        // 基本給
        this.ViewModel.BasicSalary_Text.Value              = default(double);
        // 役職手当
        this.ViewModel.ExecutiveAllowance_Text.Value       = default(double);
        // 扶養手当
        this.ViewModel.DependencyAllowance_Text.Value      = default(double);
        // 時間外手当
        this.ViewModel.OvertimeAllowance_Text.Value        = default(double);
        // 休日割増
        this.ViewModel.DaysoffIncreased_Text.Value         = default(double);
        // 深夜割増
        this.ViewModel.NightworkIncreased_Text.Value       = default(double);
        // 住宅手当
        this.ViewModel.HousingAllowance_Text.Value         = default(double);
        // 遅刻早退欠勤
        this.ViewModel.LateAbsent_Text.Value               = default(double);
        // 交通費
        this.ViewModel.TransportationExpenses_Text.Value   = default(double);
        // 前払退職金
        this.ViewModel.PrepaidRetirementPayment_Text.Value = default(double);
        // 在宅手当
        this.ViewModel.ElectricityAllowance_Text.Value     = default(double);
        // 特別手当
        this.ViewModel.SpecialAllowance_Text.Value         = default(double);
        // 予備
        this.ViewModel.SpareAllowance_Text.Value           = default(double);
        // 備考
        this.ViewModel.Remarks_Text.Value                  = default(string);
        // 支給総計
        this.ViewModel.TotalSalary_Text.Value              = default(double);

        // 差引支給額
        this.ViewModel.TotalDeductedSalary_Foreground.Value = new SolidColorBrush(Colors.Black);
        this.ViewModel.TotalDeductedSalary_Text.Value       = default(double);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="transaction">トランザクション</param>
    /// <remarks>
    /// SQLiteに接続し、入力項目を保存する。
    /// </remarks>
    public void Save(ITransactionRepository transaction)
    {
        var entity = new AllowanceValueEntity(
                          this.Header.Model.ID,
                          this.Header.Model.YearMonth,
                          this.ViewModel.BasicSalary_Text.Value,
                          this.ViewModel.ExecutiveAllowance_Text.Value,
                          this.ViewModel.DependencyAllowance_Text.Value,
                          this.ViewModel.DependencyAllowance_Text.Value,
                          this.ViewModel.DaysoffIncreased_Text.Value,
                          this.ViewModel.NightworkIncreased_Text.Value,
                          this.ViewModel.HousingAllowance_Text.Value,
                          this.ViewModel.LateAbsent_Text.Value,
                          this.ViewModel.TransportationExpenses_Text.Value,
                          this.ViewModel.PrepaidRetirementPayment_Text.Value,
                          this.ViewModel.ElectricityAllowance_Text.Value,
                          this.ViewModel.SpecialAllowance_Text.Value,
                          this.ViewModel.SpareAllowance_Text.Value,
                          this.ViewModel.Remarks_Text.Value,
                          this.ViewModel.TotalSalary_Text.Value,
                          this.ViewModel.TotalDeductedSalary_Text.Value);

        _repository.Save(transaction, entity);
    }

    /// <summary>
    /// 再計算
    /// </summary>
    /// <remarks>
    /// 該当項目の変更時に、支給総計と差引支給額を再計算する。
    /// </remarks>
    internal void ReCaluculate()
    {
        if (this.ViewModel is null)
        {
            return;
        }

        this.ViewModel.TotalSalary_Text.Value = this.ViewModel.BasicSalary_Text.Value
                                              + this.ViewModel.ExecutiveAllowance_Text.Value
                                              + this.ViewModel.DependencyAllowance_Text.Value
                                              + this.ViewModel.DependencyAllowance_Text.Value
                                              + this.ViewModel.DaysoffIncreased_Text.Value
                                              + this.ViewModel.NightworkIncreased_Text.Value
                                              + this.ViewModel.ElectricityAllowance_Text.Value
                                              + this.ViewModel.LateAbsent_Text.Value
                                              + this.ViewModel.OvertimeAllowance_Text.Value
                                              + this.ViewModel.SpecialAllowance_Text.Value
                                              + this.ViewModel.SpareAllowance_Text.Value
                                              + this.ViewModel.TransportationExpenses_Text.Value
                                              + this.ViewModel.PrepaidRetirementPayment_Text.Value;

        if (this.ViewModel_Deduction is null)
        {
            return;
        }

        this.ViewModel.TotalDeductedSalary_Text.Value = this.ViewModel.TotalSalary_Text.Value 
                                                      - this.ViewModel_Deduction.TotalDeduct_Text.Value;

        this.ChangeColor();
    }
    
    /// <summary>
    /// 文字色変更
    /// </summary>
    /// <remarks>
    /// 差引支給額の値の正負によって、文字色を変更する。
    /// </remarks>
    internal void ChangeColor()
    {
        var foreground = this.ViewModel.TotalDeductedSalary_Foreground;

        foreground.Value = this.ViewModel.TotalDeductedSalary_Text.Value >= 0 ?
                           new SolidColorBrush(Colors.Blue) : new SolidColorBrush(Colors.Red);
    }
}
