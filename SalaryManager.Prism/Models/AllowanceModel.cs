using SalaryManager.Prism.ViewModels;
using System.Reflection;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 支給額
/// </summary>
public sealed class AllowanceModel : ModelBase<AllowanceViewModel>, IParallellyEditable
{
    private static readonly log4net.ILog _logger =
      log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    #region Get Instance

    private static AllowanceModel model = null;

    public static AllowanceModel GetInstance(IAllowanceRepository repository)
    {
        if (model == null)
        {
            model = new AllowanceModel(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private IAllowanceRepository _repository;

    public AllowanceModel(IAllowanceRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 支給額 </summary>
    internal override AllowanceViewModel ViewModel { get; set; }

    /// <summary> ViewModel - 支給額 </summary>
    internal HeaderViewModel Header { get; set; }

    /// <summary> ViewModel - 控除額 </summary>
    internal DeductionViewModel ViewModel_Deduction { get; set; }

    /// <summary> ViewModel - 勤務先 </summary>
    internal WorkPlaceViewModel ViewModel_WorkPlace { get; set; }

    /// <summary> Entity - 支給額 </summary>
    public AllowanceValueEntity Entity { get; set; }

    /// <summary> Entity - 支給額 (昨年度) </summary>
    public AllowanceValueEntity Entity_LastYear { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 画面起動時に、項目を初期化する。
    /// </remarks>
    public void Initialize()
    {
        this.Window_Activated();

        this.Reload();

        var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

        if (this.Entity is null && showDefaultPayslip)
        {
            // デフォルト明細
            this.Entity = Allowances.FetchDefault();
        }
    }

    public void Window_Activated()
    {
        try
        {
            this.ViewModel.Window_FontFamily = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
            this.ViewModel.Window_FontSize   = XMLLoader.FetchFontSize();
            this.ViewModel.Window_Background = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
        } 
        catch (FileReaderException ex)
        {
            _logger.Error("XMLの読み込みに失敗しました。", ex);
        }
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

            this.Entity          = Allowances.Fetch(this.Header.Year_Text,     this.Header.Month_Text);
            this.Entity_LastYear = Allowances.Fetch(this.Header.Year_Text - 1, this.Header.Month_Text);

            this.Reload_InputForm();
        }
    }

    /// <summary>
    /// 再描画 - 入力フォーム
    /// </summary>
    /// <remarks>
    /// 該当月に支給額と手当有無が存在すれば、各項目を再描画する。
    /// </remarks>
    public void Reload_InputForm()
    {
        // 所属会社名
        Careers.Create(new CareerSQLite());

        var company = Careers.FetchCompany(new DateTime(this.Header.Year_Text, this.Header.Month_Text, 1));
        if (company is null)
        {
            return;
        }

        // 手当有無
        var existence = Careers.FetchAllowanceExistence(new CompanyNameValue(company));
        if (existence != null)
        {
            this.ViewModel.ExecutiveAllowance_IsEnabled       = existence.Executive.Value;
            this.ViewModel.DependencyAllowance_IsEnabled      = existence.Dependency.Value;
            this.ViewModel.OvertimeAllowance_IsEnabled        = existence.Overtime.Value;
            this.ViewModel.NightworkIncreased_IsEnabled       = existence.LateNight.Value;
            this.ViewModel.HousingAllowance_IsEnabled         = existence.Housing.Value;
            this.ViewModel.TransportationExpenses_IsEnabled   = existence.Commution.Value;
            this.ViewModel.PrepaidRetirementPayment_IsEnabled = existence.PrepaidRetirement.Value;
            this.ViewModel.ElectricityAllowance_IsEnabled     = existence.Electricity.Value;
            this.ViewModel.SpecialAllowance_IsEnabled         = existence.Special.Value;
        }

        if (this.Entity is null)
        {
            this.Clear();
            return;
        }

        // 基本給
        this.ViewModel.BasicSalary_Text              = this.Entity.BasicSalary.Value;
        // 役職手当
        this.ViewModel.ExecutiveAllowance_Text       = this.Entity.ExecutiveAllowance.Value;
        // 扶養手当
        this.ViewModel.DependencyAllowance_Text      = this.Entity.DependencyAllowance.Value;
        // 時間外手当
        this.ViewModel.OvertimeAllowance_Text        = this.Entity.OvertimeAllowance.Value;
        // 休日割増
        this.ViewModel.DaysoffIncreased_Text         = this.Entity.DaysoffIncreased.Value;
        // 深夜割増
        this.ViewModel.NightworkIncreased_Text       = this.Entity.NightworkIncreased.Value;
        // 住宅手当
        this.ViewModel.HousingAllowance_Text         = this.Entity.HousingAllowance.Value;
        // 遅刻早退欠勤
        this.ViewModel.LateAbsent_Text               = this.Entity.LateAbsent;
        // 交通費
        this.ViewModel.TransportationExpenses_Text   = this.Entity.TransportationExpenses.Value;
        // 前払退職金
        this.ViewModel.PrepaidRetirementPayment_Text = this.Entity.PrepaidRetirementPayment.Value;
        // 在宅手当
        this.ViewModel.ElectricityAllowance_Text     = this.Entity.ElectricityAllowance.Value;
        // 特別手当
        this.ViewModel.SpecialAllowance_Text         = this.Entity.SpecialAllowance;
        // 予備
        this.ViewModel.SpareAllowance_Text           = this.Entity.SpareAllowance;
        // 備考
        this.ViewModel.Remarks_Text                  = this.Entity.Remarks;
        // 支給総計、差引支給額
        this.ReCaluculate();
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
        this.ViewModel.BasicSalary_Text              = default(double);
        // 役職手当
        this.ViewModel.ExecutiveAllowance_Text       = default(double);
        // 扶養手当
        this.ViewModel.DependencyAllowance_Text      = default(double);
        // 時間外手当
        this.ViewModel.OvertimeAllowance_Text        = default(double);
        // 休日割増
        this.ViewModel.DaysoffIncreased_Text         = default(double);
        // 深夜割増
        this.ViewModel.NightworkIncreased_Text       = default(double);
        // 住宅手当
        this.ViewModel.HousingAllowance_Text         = default(double);
        // 遅刻早退欠勤
        this.ViewModel.LateAbsent_Text               = default(double);
        // 交通費
        this.ViewModel.TransportationExpenses_Text   = default(double);
        // 前払退職金
        this.ViewModel.PrepaidRetirementPayment_Text = default(double);
        // 在宅手当
        this.ViewModel.ElectricityAllowance_Text     = default(double);
        // 特別手当
        this.ViewModel.SpecialAllowance_Text         = default(double);
        // 予備
        this.ViewModel.SpareAllowance_Text           = default(double);
        // 備考
        this.ViewModel.Remarks_Text                  = default(string);
        // 支給総計
        this.ViewModel.TotalSalary_Text              = default(double);

        // 差引支給額
        this.ViewModel.TotalDeductedSalary_Foreground = new SolidColorBrush(Colors.Black);
        this.ViewModel.TotalDeductedSalary_Text       = default(double);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="yearMonth">年月</param>
    /// <param name="transaction">トランザクション</param>
    /// <remarks>
    /// SQLiteに接続し、入力項目を保存する。
    /// </remarks>
    public void Save(ITransactionRepository transaction, int id, DateOnly yearMonth)
    {
        var entity = new AllowanceValueEntity(
                          id,
                          yearMonth,
                          this.ViewModel.BasicSalary_Text,
                          this.ViewModel.ExecutiveAllowance_Text,
                          this.ViewModel.DependencyAllowance_Text,
                          this.ViewModel.OvertimeAllowance_Text,
                          this.ViewModel.DaysoffIncreased_Text,
                          this.ViewModel.NightworkIncreased_Text,
                          this.ViewModel.HousingAllowance_Text,
                          this.ViewModel.LateAbsent_Text,
                          this.ViewModel.TransportationExpenses_Text,
                          this.ViewModel.PrepaidRetirementPayment_Text,
                          this.ViewModel.ElectricityAllowance_Text,
                          this.ViewModel.SpecialAllowance_Text,
                          this.ViewModel.SpareAllowance_Text,
                          this.ViewModel.Remarks_Text,
                          this.ViewModel.TotalSalary_Text,
                          this.ViewModel.TotalDeductedSalary_Text);

        _repository.Save(transaction, entity);
    }

    /// <summary>
    /// 再計算
    /// </summary>
    /// <remarks>
    /// 該当項目の変更時に、支給総計と差引支給額を再計算する。
    /// </remarks>
    public void ReCaluculate()
    {
        if (this.ViewModel is null)
        {
            return;
        }

        this.ViewModel.TotalSalary_Text = this.ViewModel.BasicSalary_Text
                                              + this.ViewModel.ExecutiveAllowance_Text
                                              + this.ViewModel.DependencyAllowance_Text
                                              + this.ViewModel.DependencyAllowance_Text
                                              + this.ViewModel.DaysoffIncreased_Text
                                              + this.ViewModel.NightworkIncreased_Text
                                              + this.ViewModel.ElectricityAllowance_Text
                                              + this.ViewModel.LateAbsent_Text
                                              + this.ViewModel.OvertimeAllowance_Text
                                              + this.ViewModel.SpecialAllowance_Text
                                              + this.ViewModel.SpareAllowance_Text
                                              + this.ViewModel.TransportationExpenses_Text
                                              + this.ViewModel.PrepaidRetirementPayment_Text;

        if (this.ViewModel_Deduction is null)
        {
            return;
        }

        this.ViewModel.TotalDeductedSalary_Text = this.ViewModel.TotalSalary_Text - this.ViewModel_Deduction.TotalDeduct_Text;

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

        foreground = this.ViewModel.TotalDeductedSalary_Text >= 0 ?
                           new SolidColorBrush(Colors.Blue) : new SolidColorBrush(Colors.Red);
    }
}
