using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;

namespace SalaryManagerTest;

/// <summary>
/// Model - 控除額 テスト
/// </summary>
public class DeductionModelTest
{
    #region ヘルパー

    /// <summary>モックリポジトリを生成する</summary>
    private static Mock<IDeductionRepository> CreateRepositoryMock() => new();

    /// <summary>テスト用 DeductionModel を生成する（シングルトン非使用）</summary>
    private static DeductionModel CreateModel(Mock<IDeductionRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        return new DeductionModel(mock.Object);
    }

    /// <summary>ViewModel を紐付けた DeductionModel を生成する</summary>
    private static (DeductionModel model, DeductionViewModel vm) CreateModelWithViewModel()
    {
        var mock = CreateRepositoryMock();
        var model = CreateModel(mock);
        var vm = new DeductionViewModel(mock.Object);
        model.ViewModel = vm;
        return (model, vm);
    }

    /// <summary>ViewModel と Allowance を紐付けた DeductionModel を生成する</summary>
    /// <remarks>
    /// ReCaluculate や Reload_InputForm では Allowance.ReCaluculate() が呼ばれる。
    /// AllowanceModel.ViewModel を設定しないため Allowance 側は早期リターンするが、
    /// NullReferenceException を防ぐために Allowance インスタンス自体は必須。
    /// </remarks>
    private static (DeductionModel model, DeductionViewModel vm) CreateModelWithViewModelAndAllowance()
    {
        var (model, vm) = CreateModelWithViewModel();
        var allowanceMock = new Mock<IAllowanceRepository>();
        var allowanceModel = new AllowanceModel(allowanceMock.Object);
        model.Allowance = allowanceModel;
        return (model, vm);
    }

    /// <summary>テスト用 DeductionEntity を生成する</summary>
    private static DeductionEntity CreateEntity(
        double healthInsurance       = 10000,
        double nursingInsurance      = 3000,
        double welfareAnnuity        = 20000,
        double employmentInsurance   = 1000,
        double incomeTax             = 15000,
        double municipalTax          = 30000,
        double friendshipAssociation = 2000,
        double yearEndTaxAdjustment  = 500,
        string remarks               = "",
        double totalDeduct           = 81500)
        => new DeductionEntity(
            1, DateUtils.Today,
            healthInsurance, nursingInsurance, welfareAnnuity, employmentInsurance,
            incomeTax, municipalTax, friendshipAssociation, yearEndTaxAdjustment,
            remarks, totalDeduct);

    #endregion

    // ==========================================
    // GetInstance - シングルトン
    // ==========================================

    /// <summary>
    /// GetInstance を複数回呼び出したとき、同じインスタンスが返ることを確認する（正常系）
    /// </summary>
    [Fact]
    public void GetInstance_複数回呼び出し_同じインスタンスを返す()
    {
        var mock = CreateRepositoryMock();

        var instance1 = DeductionModel.GetInstance(mock.Object);
        var instance2 = DeductionModel.GetInstance(mock.Object);

        Assert.Same(instance1, instance2);
    }

    // ==========================================
    // Clear - 全項目を初期値にリセット
    // ==========================================

    /// <summary>
    /// Clear 後、全ての数値プロパティがゼロになることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_全数値プロパティがゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.HealthInsurance_Text       = 10000;
        vm.NursingInsurance_Text      = 3000;
        vm.WelfareAnnuity_Text        = 20000;
        vm.EmploymentInsurance_Text   = 1000;
        vm.IncomeTax_Text             = 15000;
        vm.MunicipalTax_Text          = 30000;
        vm.FriendshipAssociation_Text = 2000;
        vm.YearEndTaxAdjustment_Text  = 500;
        vm.TotalDeduct_Text           = 81500;

        model.Clear();

        Assert.Equal(0, vm.HealthInsurance_Text);
        Assert.Equal(0, vm.NursingInsurance_Text);
        Assert.Equal(0, vm.WelfareAnnuity_Text);
        Assert.Equal(0, vm.EmploymentInsurance_Text);
        Assert.Equal(0, vm.IncomeTax_Text);
        Assert.Equal(0, vm.MunicipalTax_Text);
        Assert.Equal(0, vm.FriendshipAssociation_Text);
        Assert.Equal(0, vm.YearEndTaxAdjustment_Text);
        Assert.Equal(0, vm.TotalDeduct_Text);
    }

    /// <summary>
    /// Clear 後、備考が null になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_備考がNullになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Remarks_Text = "テスト備考";

        model.Clear();

        Assert.Null(vm.Remarks_Text);
    }

    // ==========================================
    // Save - リポジトリへの保存
    // ==========================================

    /// <summary>
    /// Save を呼び出したとき、IDeductionRepository.Save が1回呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_正常な値_RepositoryのSaveが1回呼ばれる()
    {
        var mockRepo = CreateRepositoryMock();
        var model = CreateModel(mockRepo);
        var vm = new DeductionViewModel(mockRepo.Object);
        model.ViewModel = vm;

        var mockTransaction = new Mock<ITransactionRepository>();

        model.Save(mockTransaction.Object, 1, DateOnly.FromDateTime(DateTime.Today));

        mockRepo.Verify(
            r => r.Save(mockTransaction.Object, It.IsAny<DeductionEntity>()),
            Times.Once);
    }

    /// <summary>
    /// Save を複数回呼び出したとき、その回数分だけ IDeductionRepository.Save が呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_複数回呼び出し_RepositoryのSaveがその回数だけ呼ばれる()
    {
        var mockRepo = CreateRepositoryMock();
        var model = CreateModel(mockRepo);
        var vm = new DeductionViewModel(mockRepo.Object);
        model.ViewModel = vm;

        var mockTransaction = new Mock<ITransactionRepository>();
        var yearMonth = DateOnly.FromDateTime(DateTime.Today);

        model.Save(mockTransaction.Object, 1, yearMonth);
        model.Save(mockTransaction.Object, 2, yearMonth);
        model.Save(mockTransaction.Object, 3, yearMonth);

        mockRepo.Verify(
            r => r.Save(mockTransaction.Object, It.IsAny<DeductionEntity>()),
            Times.Exactly(3));
    }

    // ==========================================
    // ReCaluculate - 控除額合計の再計算
    // ==========================================

    /// <summary>
    /// ViewModel が null のとき、例外なく早期リターンすることを確認する（境界値）
    /// </summary>
    [Fact]
    public void ReCaluculate_ViewModelがNull_例外なく処理される()
    {
        var model = CreateModel();
        // ViewModel を設定しない

        var ex = Record.Exception(() => model.ReCaluculate());

        Assert.Null(ex);
    }

    /// <summary>
    /// 全項目がゼロのとき、TotalDeduct がゼロになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void ReCaluculate_全項目がゼロ_TotalDeductはゼロになる()
    {
        var (model, vm) = CreateModelWithViewModelAndAllowance();
        vm.HealthInsurance_Text       = 0;
        vm.NursingInsurance_Text      = 0;
        vm.WelfareAnnuity_Text        = 0;
        vm.EmploymentInsurance_Text   = 0;
        vm.IncomeTax_Text             = 0;
        vm.MunicipalTax_Text          = 0;
        vm.FriendshipAssociation_Text = 0;
        vm.YearEndTaxAdjustment_Text  = 0;

        model.ReCaluculate();

        Assert.Equal(0, vm.TotalDeduct_Text);
    }

    /// <summary>
    /// 標準的な控除体系で TotalDeduct が全控除項目の合計になることを確認する（正常系）
    /// </summary>
    /// <remarks>
    /// 期待値: 10000 + 3000 + 20000 + 1000 + 15000 + 30000 + 2000 + 500 = 81500
    /// </remarks>
    [Fact]
    public void ReCaluculate_標準的な控除体系_TotalDeductが全項目の合計になる()
    {
        var (model, vm) = CreateModelWithViewModelAndAllowance();
        vm.HealthInsurance_Text       = 10000;
        vm.NursingInsurance_Text      = 3000;
        vm.WelfareAnnuity_Text        = 20000;
        vm.EmploymentInsurance_Text   = 1000;
        vm.IncomeTax_Text             = 15000;
        vm.MunicipalTax_Text          = 30000;
        vm.FriendshipAssociation_Text = 2000;
        vm.YearEndTaxAdjustment_Text  = 500;

        model.ReCaluculate();

        Assert.Equal(81500, vm.TotalDeduct_Text);
    }

    /// <summary>
    /// 年末調整がマイナス（還付）のとき、TotalDeduct に減算が反映されることを確認する（正常系）
    /// </summary>
    /// <remarks>
    /// 期待値: 10000 + 3000 + 20000 + 1000 + 15000 + 30000 + 2000 + (-5000) = 76000
    /// </remarks>
    [Fact]
    public void ReCaluculate_年末調整がマイナス_TotalDeductに減算が反映される()
    {
        var (model, vm) = CreateModelWithViewModelAndAllowance();
        vm.HealthInsurance_Text       = 10000;
        vm.NursingInsurance_Text      = 3000;
        vm.WelfareAnnuity_Text        = 20000;
        vm.EmploymentInsurance_Text   = 1000;
        vm.IncomeTax_Text             = 15000;
        vm.MunicipalTax_Text          = 30000;
        vm.FriendshipAssociation_Text = 2000;
        vm.YearEndTaxAdjustment_Text  = -5000;

        model.ReCaluculate();

        Assert.Equal(76000, vm.TotalDeduct_Text);
    }

    /// <summary>
    /// 健康保険のみに値を設定したとき、TotalDeduct がその値と一致することを確認する（正常系）
    /// </summary>
    [Fact]
    public void ReCaluculate_健康保険のみ設定_TotalDeductがその値になる()
    {
        var (model, vm) = CreateModelWithViewModelAndAllowance();
        vm.HealthInsurance_Text       = 25000;
        vm.NursingInsurance_Text      = 0;
        vm.WelfareAnnuity_Text        = 0;
        vm.EmploymentInsurance_Text   = 0;
        vm.IncomeTax_Text             = 0;
        vm.MunicipalTax_Text          = 0;
        vm.FriendshipAssociation_Text = 0;
        vm.YearEndTaxAdjustment_Text  = 0;

        model.ReCaluculate();

        Assert.Equal(25000, vm.TotalDeduct_Text);
    }

    /// <summary>
    /// 全項目に大きな値を設定したとき、TotalDeduct がオーバーフローせず正しく計算されることを確認する（境界値）
    /// </summary>
    /// <remarks>
    /// 期待値: 9999999 * 8 = 79999992
    /// </remarks>
    [Fact]
    public void ReCaluculate_全項目が大きな値_TotalDeductが正しく計算される()
    {
        var (model, vm) = CreateModelWithViewModelAndAllowance();
        vm.HealthInsurance_Text       = 9999999;
        vm.NursingInsurance_Text      = 9999999;
        vm.WelfareAnnuity_Text        = 9999999;
        vm.EmploymentInsurance_Text   = 9999999;
        vm.IncomeTax_Text             = 9999999;
        vm.MunicipalTax_Text          = 9999999;
        vm.FriendshipAssociation_Text = 9999999;
        vm.YearEndTaxAdjustment_Text  = 9999999;

        model.ReCaluculate();

        Assert.Equal(79999992, vm.TotalDeduct_Text);
    }

    // ==========================================
    // Reload_InputForm - 入力フォームの再描画
    // ==========================================

    /// <summary>
    /// Entity が null のとき、全ての数値プロパティがゼロになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Reload_InputForm_Entityがnull_全数値プロパティがゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.HealthInsurance_Text       = 10000;
        vm.NursingInsurance_Text      = 3000;
        vm.WelfareAnnuity_Text        = 20000;
        vm.EmploymentInsurance_Text   = 1000;
        vm.IncomeTax_Text             = 15000;
        vm.MunicipalTax_Text          = 30000;
        vm.FriendshipAssociation_Text = 2000;
        vm.YearEndTaxAdjustment_Text  = 500;
        vm.TotalDeduct_Text           = 81500;
        model.Entity                  = null;

        model.Reload_InputForm();

        Assert.Equal(0, vm.HealthInsurance_Text);
        Assert.Equal(0, vm.NursingInsurance_Text);
        Assert.Equal(0, vm.WelfareAnnuity_Text);
        Assert.Equal(0, vm.EmploymentInsurance_Text);
        Assert.Equal(0, vm.IncomeTax_Text);
        Assert.Equal(0, vm.MunicipalTax_Text);
        Assert.Equal(0, vm.FriendshipAssociation_Text);
        Assert.Equal(0, vm.YearEndTaxAdjustment_Text);
        Assert.Equal(0, vm.TotalDeduct_Text);
    }

    /// <summary>
    /// Entity が null のとき、備考が null になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Reload_InputForm_Entityがnull_備考がNullになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Remarks_Text = "テスト備考";
        model.Entity    = null;

        model.Reload_InputForm();

        Assert.Null(vm.Remarks_Text);
    }

    /// <summary>
    /// Entity が設定されているとき、ViewModel に Entity の全項目値が反映されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Reload_InputForm_Entityあり_ViewModelにEntityの値が反映される()
    {
        var (model, vm) = CreateModelWithViewModelAndAllowance();
        model.Entity = CreateEntity(
            healthInsurance:       10000,
            nursingInsurance:      3000,
            welfareAnnuity:        20000,
            employmentInsurance:   1000,
            incomeTax:             15000,
            municipalTax:          30000,
            friendshipAssociation: 2000,
            yearEndTaxAdjustment:  500,
            remarks:               "テスト備考",
            totalDeduct:           81500);

        model.Reload_InputForm();

        Assert.Equal(10000, vm.HealthInsurance_Text);
        Assert.Equal(3000,  vm.NursingInsurance_Text);
        Assert.Equal(20000, vm.WelfareAnnuity_Text);
        Assert.Equal(1000,  vm.EmploymentInsurance_Text);
        Assert.Equal(15000, vm.IncomeTax_Text);
        Assert.Equal(30000, vm.MunicipalTax_Text);
        Assert.Equal(2000,  vm.FriendshipAssociation_Text);
        Assert.Equal(500,   vm.YearEndTaxAdjustment_Text);
        Assert.Equal("テスト備考", vm.Remarks_Text);
        Assert.Equal(81500, vm.TotalDeduct_Text);
    }

    /// <summary>
    /// Entity が設定されていて年末調整がマイナスのとき、ViewModel に正しく反映されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Reload_InputForm_年末調整がマイナスのEntity_ViewModelに正しく反映される()
    {
        var (model, vm) = CreateModelWithViewModelAndAllowance();
        model.Entity = CreateEntity(yearEndTaxAdjustment: -30000, totalDeduct: 51500);

        model.Reload_InputForm();

        Assert.Equal(-30000, vm.YearEndTaxAdjustment_Text);
    }
}
