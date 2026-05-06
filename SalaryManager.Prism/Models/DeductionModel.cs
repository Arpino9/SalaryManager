namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 控除額
/// </summary>
public class DeductionModel : ModelBase<DeductionViewModel>, IParallellyEditable
{

    #region Get Instance

    private static DeductionModel model = null;

    public static DeductionModel GetInstance(IDeductionRepository repository)
    {
        if (model == null)
        {
            model = new DeductionModel(repository);
        }

        return model;
    }

    #endregion
    
    /// <summary> Repository </summary>
    private IDeductionRepository _repository;

    public DeductionModel(IDeductionRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 控除額 </summary>
    internal override DeductionViewModel ViewModel { get; set; }

    /// <summary> ViewModel - ヘッダ </summary>
    internal HeaderViewModel Header { get; set; }

    /// <summary> ViewModel - 支給額 </summary>
    internal AllowanceModel Allowance { get; set; }

    /// <summary> Model - ヘッダー </summary>
    private HeaderModel Model_Header { get; set; }

    /// <summary> Entity - 控除額 </summary>
    public DeductionEntity Entity { get; set; }

    /// <summary> Entity - 控除額 (昨年度) </summary>
    public DeductionEntity Entity_LastYear { get; set; }

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
            this.Entity = Deductions.FetchDefault();
        }
    }

    public void Window_Activated()
    {
        this.ViewModel.Window_FontFamily = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
        this.ViewModel.Window_FontSize   = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
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
            Deductions.Create(_repository);

            this.Entity          = Deductions.Fetch(this.Header.Year_Text,     this.Header.Month_Text);
            this.Entity_LastYear = Deductions.Fetch(this.Header.Year_Text - 1, this.Header.Month_Text);

            this.Reload_InputForm();
        }   
    }

    /// <summary>
    /// 再描画
    /// </summary>
    /// <remarks>
    /// 該当月に控除額が存在すれば、各項目を再描画する。
    /// </remarks>
    public void Reload_InputForm()
    {
        if (this.Entity is null)
        {
            this.Clear();
            return;
        }

        // 健康保険
        this.ViewModel.HealthInsurance_Text       = this.Entity.HealthInsurance.Value;
        // 介護保険
        this.ViewModel.NursingInsurance_Text      = this.Entity.NursingInsurance.Value;
        // 厚生年金
        this.ViewModel.WelfareAnnuity_Text        = this.Entity.WelfareAnnuity.Value;
        // 雇用保険
        this.ViewModel.EmploymentInsurance_Text   = this.Entity.EmploymentInsurance.Value;
        // 所得税
        this.ViewModel.IncomeTax_Text             = this.Entity.IncomeTax.Value;
        // 市町村税
        this.ViewModel.MunicipalTax_Text          = this.Entity.MunicipalTax.Value;
        // 互助会
        this.ViewModel.FriendshipAssociation_Text = this.Entity.FriendshipAssociation.Value;
        // 年末調整他
        this.ViewModel.YearEndTaxAdjustment_Text  = this.Entity.YearEndTaxAdjustment;
        // 備考
        this.ViewModel.Remarks_Text               = this.Entity.Remarks;
        // 控除額計
        this.ViewModel.TotalDeduct_Text           = this.Entity.TotalDeduct.Value;
        // 支給総計、差引支給額
        this.Allowance.ReCaluculate();
    }

    /// <summary>
    /// クリア
    /// </summary>
    /// <remarks>
    /// 各項目を初期化する。
    /// </remarks>
    public void Clear()
    {
        // 健康保険
        this.ViewModel.HealthInsurance_Text       = default(double);
        // 介護保険
        this.ViewModel.NursingInsurance_Text      = default(double);
        // 厚生年金
        this.ViewModel.WelfareAnnuity_Text        = default(double);
        // 雇用保険
        this.ViewModel.EmploymentInsurance_Text   = default(double);
        // 所得税
        this.ViewModel.IncomeTax_Text             = default(double);
        // 市町村税
        this.ViewModel.MunicipalTax_Text          = default(double);
        // 互助会
        this.ViewModel.FriendshipAssociation_Text = default(double);
        // 年末調整他
        this.ViewModel.YearEndTaxAdjustment_Text  = default(double);
        // 備考
        this.ViewModel.Remarks_Text               = default(string);
        // 控除額計
        this.ViewModel.TotalDeduct_Text           = default(double);
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
        var entity = new DeductionEntity(
                        id,
                        yearMonth,
                        this.ViewModel.HealthInsurance_Text,
                        this.ViewModel.NursingInsurance_Text,
                        this.ViewModel.WelfareAnnuity_Text,
                        this.ViewModel.EmploymentInsurance_Text,
                        this.ViewModel.IncomeTax_Text,
                        this.ViewModel.MunicipalTax_Text,
                        this.ViewModel.FriendshipAssociation_Text,
                        this.ViewModel.YearEndTaxAdjustment_Text,
                        this.ViewModel.Remarks_Text,
                        this.ViewModel.TotalDeduct_Text);

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

        this.ViewModel.TotalDeduct_Text = this.ViewModel.HealthInsurance_Text
                                        + this.ViewModel.NursingInsurance_Text
                                        + this.ViewModel.WelfareAnnuity_Text
                                        + this.ViewModel.EmploymentInsurance_Text
                                        + this.ViewModel.IncomeTax_Text
                                        + this.ViewModel.MunicipalTax_Text
                                        + this.ViewModel.FriendshipAssociation_Text
                                        + this.ViewModel.YearEndTaxAdjustment_Text;

        this.Allowance.ReCaluculate();
    }
}
