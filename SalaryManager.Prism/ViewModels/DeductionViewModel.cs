namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 控除額
/// </summary>
public class DeductionViewModel : ViewModelBase<DeductionModel>
{
    public DeductionViewModel()
    {
        this.MainWindow.Deduction = this.Model;

        this.Allowance.ViewModel_Deduction = this;

        this.Model.ViewModel = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    private IDeductionRepository _deductionRepository;

    /// <summary>
    /// 単体テスト用のコンストラクタ
    /// </summary>
    /// <param name="deductionRepository">Repository - 控除額</param>
    public DeductionViewModel(IDeductionRepository deductionRepository)
    {
        _deductionRepository = deductionRepository;
        DeductionModel.GetInstance(_deductionRepository);

        this.MainWindow.Deduction          = this.Model;
        this.Allowance.ViewModel_Deduction = this;
        this.Model.ViewModel               = this;

        this.Model.Clear();
    }

    protected override void BindEvents()
    {
        var entity = this.Model.Entity_LastYear;

        // 初期状態
        this.Default_MouseLeave = new DelegateCommand(() => this.MainWindow.ComparePrice(0, 0));

        // 健康保険
        this.HealthInsurance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.HealthInsurance_Text, entity?.HealthInsurance.Value ?? 0));
        this.HealthInsurance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 介護保険
        this.NursingInsurance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.NursingInsurance_Text, entity?.NursingInsurance.Value ?? 0));
        this.NursingInsurance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 厚生年金
        this.WelfareAnnuity_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.WelfareAnnuity_Text, entity?.WelfareAnnuity.Value ?? 0));
        this.WelfareAnnuity_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 雇用保険
        this.EmploymentInsurance_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.EmploymentInsurance_Text, entity?.EmploymentInsurance.Value ?? 0));
        this.EmploymentInsurance_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 所得税
        this.IncomeTax_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.IncomeTax_Text, entity?.IncomeTax.Value ?? 0));
        this.IncomeTax_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 市町村税
        this.MunicipalTax_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.MunicipalTax_Text, entity?.MunicipalTax.Value ?? 0));
        this.MunicipalTax_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 互助会
        this.FriendshipAssociation_MouseMove   = new DelegateCommand(() => this.MainWindow.ComparePrice(this.FriendshipAssociation_Text, entity?.FriendshipAssociation.Value ?? 0));
        this.FriendshipAssociation_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 年末調整他
        this.YearEndTaxAdjustment_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.YearEndTaxAdjustment_Text, entity?.YearEndTaxAdjustment ?? 0));
        this.YearEndTaxAdjustment_TextChanged = new DelegateCommand(() => this.Model.ReCaluculate());

        // 控除額計
        this.TotalDeduct_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.TotalDeduct_Text, entity?.TotalDeduct.Value ?? 0));
    }

    /// <summary> Model - 控除額 </summary>
    protected override DeductionModel Model { get; } 
        = DeductionModel.GetInstance(new DeductionSQLite());

    /// <summary> Model - 支給額 </summary>
    public AllowanceModel Allowance { get; set; } 
        = AllowanceModel.GetInstance(new AllowanceSQLite());

    /// <summary> Model - メイン画面 </summary>
    public MainWindowModel MainWindow { get; set; } 
        = MainWindowModel.GetInstance();

    #region 初期状態

    /// <summary> 初期状態 - MouseLeave </summary>
    public DelegateCommand Default_MouseLeave { get; private set; }

    #endregion

    #region 健康保険

    /// <summary> 健康保険 - Text </summary>
    public double HealthInsurance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 健康保険 - MouseMove </summary>
    public DelegateCommand HealthInsurance_MouseMove { get; set; }

    /// <summary> 健康保険 - TextChanged </summary>
    public DelegateCommand HealthInsurance_TextChanged { get; set; }

    #endregion

    #region 介護保険

    /// <summary> 介護保険 - Text </summary>
    public double NursingInsurance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 介護保険 - MouseLeave </summary>
    public DelegateCommand NursingInsurance_MouseMove { get; private set; }

    /// <summary> 介護保険 - TextChanged </summary>
    public DelegateCommand NursingInsurance_TextChanged { get; private set; }

    #endregion

    #region 厚生年金

    /// <summary> 厚生年金 - Text </summary>
    public double WelfareAnnuity_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 厚生年金 - MouseMove </summary>
    public DelegateCommand WelfareAnnuity_MouseMove { get; private set; }

    /// <summary> 厚生年金 - TextChanged </summary>
    public DelegateCommand WelfareAnnuity_TextChanged { get; private set; }

    #endregion

    #region 雇用保険

    /// <summary> 雇用保険 - Text </summary>
    public double EmploymentInsurance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 雇用保険 - MouseMove </summary>
    public DelegateCommand EmploymentInsurance_MouseMove { get; set; }

    /// <summary> 雇用保険 - TextChanged </summary>
    public DelegateCommand EmploymentInsurance_TextChanged { get; set; }

    #endregion

    #region 所得税

    /// <summary> 所得税 - Text </summary>
    public double IncomeTax_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 所得税 - MouseMove </summary>
    public DelegateCommand IncomeTax_MouseMove { get; private set; }

    /// <summary> 所得税 - TextChanged </summary>
    public DelegateCommand IncomeTax_TextChanged { get; private set; }

    #endregion

    #region 市町村税

    /// <summary> 市町村税 - Text </summary>
    public double MunicipalTax_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 市町村税 - MouseMove </summary>
    public DelegateCommand MunicipalTax_MouseMove { get; private set; }

    /// <summary> 市町村税 - TextChanged </summary>
    public DelegateCommand MunicipalTax_TextChanged { get; private set; }

    #endregion

    #region 互助会

    /// <summary> 互助会 - Text </summary>
    public double FriendshipAssociation_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 互助会 - MouseMove </summary>
    public DelegateCommand FriendshipAssociation_MouseMove { get; set; }

    /// <summary> 互助会 - TextChanged </summary>
    public DelegateCommand FriendshipAssociation_TextChanged { get; set; }

    #endregion

    #region 年末調整他

    /// <summary> 年末調整他 - Text </summary>
    public double YearEndTaxAdjustment_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 年末調整他 - MouseMove </summary>
    public DelegateCommand YearEndTaxAdjustment_MouseMove { get; set; }

    /// <summary> 年末調整他 - TextChanged </summary>
    public DelegateCommand YearEndTaxAdjustment_TextChanged { get; set; }

    #endregion

    #region 控除額計

    /// <summary> 控除額計 - Foreground </summary>
    public SolidColorBrush TotalDeduct_Foreground
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new SolidColorBrush(Colors.Red);

    /// <summary> 控除額計 - Text </summary>
    public double TotalDeduct_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 控除額計 - MouseMove </summary>
    public DelegateCommand TotalDeduct_MouseMove { get; private set; }

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public string Remarks_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

}
