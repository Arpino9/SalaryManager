namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 控除額
/// </summary>
public class Model_Deduction : ModelBase<ViewModel_Deduction>
{

    #region Get Instance

    private static Model_Deduction model = null;

    public static Model_Deduction GetInstance(IDeductionRepository repository)
    {
        if (model == null)
        {
            model = new Model_Deduction(repository);
        }

        return model;
    }

    #endregion
    
    /// <summary> Repository </summary>
    private IDeductionRepository _repository;

    public Model_Deduction(IDeductionRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 控除額 </summary>
    internal override ViewModel_Deduction ViewModel { get; set; }

    /// <summary> ViewModel - ヘッダ </summary>
    internal ViewModel_Header Header { get; set; }

    /// <summary> ViewModel - 支給額 </summary>
    internal Model_Allowance Allowance { get; set; }

    /// <summary> Model - ヘッダー </summary>
    private Model_Header Model_Header { get; set; } = Model_Header.GetInstance(new HeaderSQLite());

    /// <summary> Entity - 控除額 </summary>
    public DeductionEntity Entity { get; set; }

    /// <summary> Entity - 控除額 (昨年度) </summary>
    public DeductionEntity Entity_LastYear { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="entityDate">取得する日付</param>
    /// <remarks>
    /// 画面起動時に、項目を初期化する。
    /// </remarks>
    public void Initialize(DateTime entityDate)
    {
        Deductions.Create(_repository);

        this.Entity          = Deductions.Fetch(entityDate.Year, entityDate.Month);
        this.Entity_LastYear = Deductions.Fetch(entityDate.Year, entityDate.Month - 1);

        var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

        if (this.Entity is null && showDefaultPayslip)
        {
            // デフォルト明細
            this.Entity = Deductions.FetchDefault();
        }

        this.Refresh();
    }

    /// <summary>
    /// 再描画
    /// </summary>
    /// <remarks>
    /// 該当月に控除額が存在すれば、各項目を再描画する。
    /// </remarks>
    public void Refresh()
    {
        if (this.Entity is null)
        {
            this.Clear();
            return;
        }

        // 健康保険
        this.ViewModel.HealthInsurance_Text.Value       = this.Entity.HealthInsurance.Value;
        // 介護保険
        this.ViewModel.NursingInsurance_Text.Value      = this.Entity.NursingInsurance.Value;
        // 厚生年金
        this.ViewModel.WelfareAnnuity_Text.Value        = this.Entity.WelfareAnnuity.Value;
        // 雇用保険
        this.ViewModel.EmploymentInsurance_Text.Value   = this.Entity.EmploymentInsurance.Value;
        // 所得税
        this.ViewModel.IncomeTax_Text.Value             = this.Entity.IncomeTax.Value;
        // 市町村税
        this.ViewModel.MunicipalTax_Text.Value          = this.Entity.MunicipalTax.Value;
        // 互助会
        this.ViewModel.FriendshipAssociation_Text.Value = this.Entity.FriendshipAssociation.Value;
        // 年末調整他
        this.ViewModel.YearEndTaxAdjustment_Text.Value  = this.Entity.YearEndTaxAdjustment;
        // 備考
        this.ViewModel.Remarks_Text.Value               = this.Entity.Remarks;
        // 控除額計
        this.ViewModel.TotalDeduct_Text.Value           = this.Entity.TotalDeduct.Value;
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

            this.Entity          = Deductions.Fetch(this.Header.Year_Text.Value,     this.Header.Month_Text.Value);
            this.Entity_LastYear = Deductions.Fetch(this.Header.Year_Text.Value - 1, this.Header.Month_Text.Value);

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
        // 健康保険
        this.ViewModel.HealthInsurance_Text.Value       = default(double);
        // 介護保険
        this.ViewModel.NursingInsurance_Text.Value      = default(double);
        // 厚生年金
        this.ViewModel.WelfareAnnuity_Text.Value        = default(double);
        // 雇用保険
        this.ViewModel.EmploymentInsurance_Text.Value   = default(double);
        // 所得税
        this.ViewModel.IncomeTax_Text.Value             = default(double);
        // 市町村税
        this.ViewModel.MunicipalTax_Text.Value          = default(double);
        // 互助会
        this.ViewModel.FriendshipAssociation_Text.Value = default(double);
        // 年末調整他
        this.ViewModel.YearEndTaxAdjustment_Text.Value  = default(double);
        // 備考
        this.ViewModel.Remarks_Text.Value               = default(string);
        // 控除額計
        this.ViewModel.TotalDeduct_Text.Value           = default(double);
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
        var entity = new DeductionEntity(
                        this.Model_Header.ID,
                        this.Model_Header.YearMonth,
                        this.ViewModel.HealthInsurance_Text.Value,
                        this.ViewModel.NursingInsurance_Text.Value,
                        this.ViewModel.WelfareAnnuity_Text.Value,
                        this.ViewModel.EmploymentInsurance_Text.Value,
                        this.ViewModel.IncomeTax_Text.Value,
                        this.ViewModel.MunicipalTax_Text.Value,
                        this.ViewModel.FriendshipAssociation_Text.Value,
                        this.ViewModel.YearEndTaxAdjustment_Text.Value,
                        this.ViewModel.Remarks_Text.Value,
                        this.ViewModel.TotalDeduct_Text.Value);

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

        this.ViewModel.TotalDeduct_Text.Value = this.ViewModel.HealthInsurance_Text.Value
                                              + this.ViewModel.NursingInsurance_Text.Value
                                              + this.ViewModel.WelfareAnnuity_Text.Value
                                              + this.ViewModel.EmploymentInsurance_Text.Value
                                              + this.ViewModel.IncomeTax_Text.Value
                                              + this.ViewModel.MunicipalTax_Text.Value
                                              + this.ViewModel.FriendshipAssociation_Text.Value
                                              + this.ViewModel.YearEndTaxAdjustment_Text.Value;

        this.Allowance.ReCaluculate();
    }
}
