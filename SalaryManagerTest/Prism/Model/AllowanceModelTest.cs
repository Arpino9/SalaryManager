using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// Model - 支給額 テスト
/// </summary>
/// <remarks>
/// 以下の実装バグが検出されるため、該当テストは現在 FAIL します：
/// - ReCaluculate: DependencyAllowance_Text が2回加算、HousingAllowance_Text が欠落
/// - ChangeColor: ローカル変数への代入のみで ViewModel プロパティが更新されない
/// </remarks>
public class AllowanceModelTest
{
    #region ヘルパー

    /// <summary>モックリポジトリを生成する</summary>
    private static Mock<IAllowanceRepository> CreateRepositoryMock() => new();

    /// <summary>テスト用 AllowanceModel を生成する（シングルトン非使用）</summary>
    private static AllowanceModel CreateModel(Mock<IAllowanceRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        return new AllowanceModel(mock.Object);
    }

    /// <summary>ViewModel を紐付けた AllowanceModel を生成する</summary>
    private static (AllowanceModel model, AllowanceViewModel vm) CreateModelWithViewModel()
    {
        var mock = CreateRepositoryMock();
        var model = CreateModel(mock);
        var vm = new AllowanceViewModel(mock.Object);
        model.ViewModel = vm;
        return (model, vm);
    }

    #endregion

    // ==========================================
    // GetInstance - シングルトン
    // ==========================================

    /// <summary>
    /// GetInstance を複数回呼び出したとき、同じインスタンスが返ることを確認する
    /// </summary>
    [Fact]
    public void GetInstance_複数回呼び出し_同じインスタンスを返す()
    {
        var mock = CreateRepositoryMock();

        var instance1 = AllowanceModel.GetInstance(mock.Object);
        var instance2 = AllowanceModel.GetInstance(mock.Object);

        Assert.Same(instance1, instance2);
    }

    // ==========================================
    // ReCaluculate - 支給総計・差引支給額の再計算
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
    /// ViewModel_Deduction が null のとき、TotalSalary のみ計算され
    /// TotalDeductedSalary は更新されないことを確認する（正常系）
    /// </summary>
    [Fact]
    public void ReCaluculate_ViewModelDeductionがNull_TotalSalaryのみ計算される()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.BasicSalary_Text              = 300000;
        vm.ExecutiveAllowance_Text       = 0;
        vm.DependencyAllowance_Text      = 0;
        vm.OvertimeAllowance_Text        = 0;
        vm.DaysoffIncreased_Text         = 0;
        vm.NightworkIncreased_Text       = 0;
        vm.HousingAllowance_Text         = 0;
        vm.LateAbsent_Text               = 0;
        vm.TransportationExpenses_Text   = 0;
        vm.PrepaidRetirementPayment_Text = 0;
        vm.ElectricityAllowance_Text     = 0;
        vm.SpecialAllowance_Text         = 0;
        vm.SpareAllowance_Text           = 0;
        model.ViewModel_Deduction        = null;

        model.ReCaluculate();

        Assert.Equal(300000, vm.TotalSalary_Text);
        Assert.Equal(0, vm.TotalDeductedSalary_Text);
    }

    /// <summary>
    /// 全項目にゼロを設定したとき、TotalSalary がゼロになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void ReCaluculate_全項目がゼロ_TotalSalaryはゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.BasicSalary_Text              = 0;
        vm.ExecutiveAllowance_Text       = 0;
        vm.DependencyAllowance_Text      = 0;
        vm.OvertimeAllowance_Text        = 0;
        vm.DaysoffIncreased_Text         = 0;
        vm.NightworkIncreased_Text       = 0;
        vm.HousingAllowance_Text         = 0;
        vm.LateAbsent_Text               = 0;
        vm.TransportationExpenses_Text   = 0;
        vm.PrepaidRetirementPayment_Text = 0;
        vm.ElectricityAllowance_Text     = 0;
        vm.SpecialAllowance_Text         = 0;
        vm.SpareAllowance_Text           = 0;
        model.ViewModel_Deduction        = null;

        model.ReCaluculate();

        Assert.Equal(0, vm.TotalSalary_Text);
    }

    /// <summary>
    /// 標準的な給与体系で TotalSalary が全手当の合計になることを確認する（正常系）
    /// </summary>
    /// <remarks>
    /// 期待値: 100000 + 10000 + 5000 + 8000 + 2000 + 3000 + 15000 + (-1000) + 6000 + 2000 + 1000 + 4000 + 500 = 155500
    ///
    /// *** このテストは以下のバグにより FAIL します ***
    ///   - DependencyAllowance_Text が2回加算される（152行目, 153行目が重複）
    ///   - HousingAllowance_Text が合計に含まれない
    /// 実際の計算結果: 145500（正しい値より 10000 少ない）
    /// </remarks>
    [Fact]
    public void ReCaluculate_標準給与体系_TotalSalaryが全手当合計と一致する()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.BasicSalary_Text              = 100000;
        vm.ExecutiveAllowance_Text       = 10000;
        vm.DependencyAllowance_Text      = 5000;
        vm.OvertimeAllowance_Text        = 8000;
        vm.DaysoffIncreased_Text         = 2000;
        vm.NightworkIncreased_Text       = 3000;
        vm.HousingAllowance_Text         = 15000;
        vm.LateAbsent_Text               = -1000;
        vm.TransportationExpenses_Text   = 6000;
        vm.PrepaidRetirementPayment_Text = 2000;
        vm.ElectricityAllowance_Text     = 1000;
        vm.SpecialAllowance_Text         = 4000;
        vm.SpareAllowance_Text           = 500;
        model.ViewModel_Deduction        = null;

        model.ReCaluculate();

        Assert.Equal(155500, vm.TotalSalary_Text);
    }

    /// <summary>
    /// 遅刻早退欠勤がマイナス値のとき、TotalSalary が正しく減算されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void ReCaluculate_遅刻早退欠勤がマイナス_TotalSalaryに減算が反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.BasicSalary_Text              = 300000;
        vm.ExecutiveAllowance_Text       = 0;
        vm.DependencyAllowance_Text      = 0;
        vm.OvertimeAllowance_Text        = 0;
        vm.DaysoffIncreased_Text         = 0;
        vm.NightworkIncreased_Text       = 0;
        vm.HousingAllowance_Text         = 0;
        vm.LateAbsent_Text               = -5000;
        vm.TransportationExpenses_Text   = 0;
        vm.PrepaidRetirementPayment_Text = 0;
        vm.ElectricityAllowance_Text     = 0;
        vm.SpecialAllowance_Text         = 0;
        vm.SpareAllowance_Text           = 0;
        model.ViewModel_Deduction        = null;

        model.ReCaluculate();

        Assert.Equal(295000, vm.TotalSalary_Text);
    }

    /// <summary>
    /// ViewModel_Deduction が設定されているとき、TotalDeductedSalary が
    /// TotalSalary - TotalDeduct になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void ReCaluculate_ViewModel_Deductionあり_TotalDeductedSalaryが差額になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.BasicSalary_Text              = 300000;
        vm.ExecutiveAllowance_Text       = 0;
        vm.DependencyAllowance_Text      = 0;
        vm.OvertimeAllowance_Text        = 0;
        vm.DaysoffIncreased_Text         = 0;
        vm.NightworkIncreased_Text       = 0;
        vm.HousingAllowance_Text         = 0;
        vm.LateAbsent_Text               = 0;
        vm.TransportationExpenses_Text   = 0;
        vm.PrepaidRetirementPayment_Text = 0;
        vm.ElectricityAllowance_Text     = 0;
        vm.SpecialAllowance_Text         = 0;
        vm.SpareAllowance_Text           = 0;

        var deductMock = new Mock<IDeductionRepository>();
        var deductVm = new DeductionViewModel(deductMock.Object);
        deductVm.TotalDeduct_Text = 50000;
        model.ViewModel_Deduction = deductVm;

        model.ReCaluculate();

        Assert.Equal(250000, vm.TotalDeductedSalary_Text);
    }

    /// <summary>
    /// 控除合計が支給総計を超えるとき、TotalDeductedSalary がマイナスになることを確認する（異常系）
    /// </summary>
    [Fact]
    public void ReCaluculate_控除超過_TotalDeductedSalaryがマイナスになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.BasicSalary_Text              = 100000;
        vm.ExecutiveAllowance_Text       = 0;
        vm.DependencyAllowance_Text      = 0;
        vm.OvertimeAllowance_Text        = 0;
        vm.DaysoffIncreased_Text         = 0;
        vm.NightworkIncreased_Text       = 0;
        vm.HousingAllowance_Text         = 0;
        vm.LateAbsent_Text               = 0;
        vm.TransportationExpenses_Text   = 0;
        vm.PrepaidRetirementPayment_Text = 0;
        vm.ElectricityAllowance_Text     = 0;
        vm.SpecialAllowance_Text         = 0;
        vm.SpareAllowance_Text           = 0;

        var deductMock = new Mock<IDeductionRepository>();
        var deductVm = new DeductionViewModel(deductMock.Object);
        deductVm.TotalDeduct_Text = 150000;
        model.ViewModel_Deduction = deductVm;

        model.ReCaluculate();

        Assert.Equal(-50000, vm.TotalDeductedSalary_Text);
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
        vm.BasicSalary_Text              = 300000;
        vm.ExecutiveAllowance_Text       = 10000;
        vm.DependencyAllowance_Text      = 5000;
        vm.OvertimeAllowance_Text        = 8000;
        vm.DaysoffIncreased_Text         = 2000;
        vm.NightworkIncreased_Text       = 3000;
        vm.HousingAllowance_Text         = 15000;
        vm.LateAbsent_Text               = -1000;
        vm.TransportationExpenses_Text   = 6000;
        vm.PrepaidRetirementPayment_Text = 2000;
        vm.ElectricityAllowance_Text     = 1000;
        vm.SpecialAllowance_Text         = 4000;
        vm.SpareAllowance_Text           = 500;
        vm.TotalSalary_Text              = 155500;
        vm.TotalDeductedSalary_Text      = 105500;

        model.Clear();

        Assert.Equal(0, vm.BasicSalary_Text);
        Assert.Equal(0, vm.ExecutiveAllowance_Text);
        Assert.Equal(0, vm.DependencyAllowance_Text);
        Assert.Equal(0, vm.OvertimeAllowance_Text);
        Assert.Equal(0, vm.DaysoffIncreased_Text);
        Assert.Equal(0, vm.NightworkIncreased_Text);
        Assert.Equal(0, vm.HousingAllowance_Text);
        Assert.Equal(0, vm.LateAbsent_Text);
        Assert.Equal(0, vm.TransportationExpenses_Text);
        Assert.Equal(0, vm.PrepaidRetirementPayment_Text);
        Assert.Equal(0, vm.ElectricityAllowance_Text);
        Assert.Equal(0, vm.SpecialAllowance_Text);
        Assert.Equal(0, vm.SpareAllowance_Text);
        Assert.Equal(0, vm.TotalSalary_Text);
        Assert.Equal(0, vm.TotalDeductedSalary_Text);
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

    /// <summary>
    /// Clear 後、TotalDeductedSalary_Foreground が Black になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_TotalDeductedSalaryForegroundがBlackになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.TotalDeductedSalary_Foreground = new SolidColorBrush(Colors.Red);

        model.Clear();

        Assert.Equal(Colors.Black, vm.TotalDeductedSalary_Foreground?.Color);
    }

    // ==========================================
    // Save - リポジトリへの保存
    // ==========================================

    /// <summary>
    /// Save を呼び出したとき、IAllowanceRepository.Save が1回呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_正常な値_RepositoryのSaveが1回呼ばれる()
    {
        var mockRepo = CreateRepositoryMock();
        var model = CreateModel(mockRepo);
        var vm = new AllowanceViewModel(mockRepo.Object);
        model.ViewModel = vm;

        var mockTransaction = new Mock<ITransactionRepository>();

        model.Save(mockTransaction.Object, 1, DateOnly.FromDateTime(DateTime.Today));

        mockRepo.Verify(
            r => r.Save(mockTransaction.Object, It.IsAny<AllowanceValueEntity>()),
            Times.Once);
    }

    /// <summary>
    /// Save を複数回呼び出したとき、その回数分だけ IAllowanceRepository.Save が呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_複数回呼び出し_RepositoryのSaveがその回数だけ呼ばれる()
    {
        var mockRepo = CreateRepositoryMock();
        var model = CreateModel(mockRepo);
        var vm = new AllowanceViewModel(mockRepo.Object);
        model.ViewModel = vm;

        var mockTransaction = new Mock<ITransactionRepository>();
        var yearMonth = DateOnly.FromDateTime(DateTime.Today);

        model.Save(mockTransaction.Object, 1, yearMonth);
        model.Save(mockTransaction.Object, 2, yearMonth);
        model.Save(mockTransaction.Object, 3, yearMonth);

        mockRepo.Verify(
            r => r.Save(mockTransaction.Object, It.IsAny<AllowanceValueEntity>()),
            Times.Exactly(3));
    }

    // ==========================================
    // ChangeColor - 差引支給額による前景色の変更
    // ==========================================

    /// <summary>
    /// 差引支給額が正値のとき、TotalDeductedSalary_Foreground が Blue になることを確認する（正常系）
    /// </summary>
    /// <remarks>
    /// *** このテストは実装バグにより FAIL します ***
    /// ChangeColor 内でローカル変数 foreground に代入しているだけで
    /// ViewModel.TotalDeductedSalary_Foreground プロパティが更新されない。
    /// </remarks>
    [Fact]
    public void ChangeColor_差引支給額が正値_前景色がBlueになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.TotalDeductedSalary_Foreground = new SolidColorBrush(Colors.Black);
        vm.TotalDeductedSalary_Text = 100000;

        model.ChangeColor();

        Assert.Equal(Colors.Blue, vm.TotalDeductedSalary_Foreground?.Color);
    }

    /// <summary>
    /// 差引支給額がゼロのとき、TotalDeductedSalary_Foreground が Blue になることを確認する（境界値）
    /// </summary>
    /// <remarks>
    /// *** このテストは実装バグにより FAIL します ***
    /// ChangeColor 内でローカル変数 foreground に代入しているだけで
    /// ViewModel.TotalDeductedSalary_Foreground プロパティが更新されない。
    /// </remarks>
    [Fact]
    public void ChangeColor_差引支給額がゼロ_前景色がBlueになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.TotalDeductedSalary_Foreground = new SolidColorBrush(Colors.Black);
        vm.TotalDeductedSalary_Text = 0;

        model.ChangeColor();

        Assert.Equal(Colors.Blue, vm.TotalDeductedSalary_Foreground?.Color);
    }

    /// <summary>
    /// 差引支給額が負値のとき、TotalDeductedSalary_Foreground が Red になることを確認する（異常系）
    /// </summary>
    /// <remarks>
    /// *** このテストは実装バグにより FAIL します ***
    /// ChangeColor 内でローカル変数 foreground に代入しているだけで
    /// ViewModel.TotalDeductedSalary_Foreground プロパティが更新されない。
    /// </remarks>
    [Fact]
    public void ChangeColor_差引支給額が負値_前景色がRedになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.TotalDeductedSalary_Foreground = new SolidColorBrush(Colors.Black);
        vm.TotalDeductedSalary_Text = -10000;

        model.ChangeColor();

        Assert.Equal(Colors.Red, vm.TotalDeductedSalary_Foreground?.Color);
    }
}
