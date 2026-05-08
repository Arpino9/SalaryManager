using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - 支給額 テスト
/// </summary>
public class AllowanceTest
{
    #region ヘルパー

    /// <summary>テスト用ViewModelを生成する</summary>
    private static AllowanceViewModel CreateViewModel()
    {
        var mock = new Mock<IAllowanceRepository>();
        return new AllowanceViewModel(mock.Object);
    }

    #endregion

    // ==========================================
    // 初期化 - PropertyChanged通知テスト
    // ==========================================

    /// <summary>
    /// 各プロパティに値を設定したとき PropertyChanged が発火することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 小さな正値（既存ケース）
    /// InlineData 2: 標準的な給与体系＋遅刻早退マイナス＋備考あり（正常系）
    /// InlineData 3: 大きな値＋備考あり（境界値）
    /// </remarks>
    [Theory]
    [InlineData(1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, "", 14000, 15000)]
    [InlineData(500000, 100000, 50000, 80000, 20000, 15000, 60000, -5000, 15000, 10000, 8000, 100000, 5000, "備考テスト", 963000, 800000)]
    [InlineData(9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, "最大値テスト", 9999999, 9999999)]
    public void 初期化(
        double basicSalary,
        double executiveAllowance,
        double dependencyAllowance,
        double OvertimeAllowance,
        double daysoffIncreased,
        double nightworkIncreased,
        double housingAllowance,
        double lateAbsent,
        double transportationExpenses,
        double prepaidRetirementPayment,
        double electricityAllowance,
        double specialAllowance,
        double spareAllowance,
        string remarks,
        double totalSalary,
        double totalDeductedSalary)
    {
        var entity = new AllowanceValueEntity(
            1,
            DateUtils.Today,
            basicSalary,
            executiveAllowance,
            dependencyAllowance,
            OvertimeAllowance,
            daysoffIncreased,
            nightworkIncreased,
            housingAllowance,
            lateAbsent,
            transportationExpenses,
            prepaidRetirementPayment,
            electricityAllowance,
            specialAllowance,
            spareAllowance,
            remarks,
            totalSalary,
            totalDeductedSalary
        );

        var allowanceMock = new Mock<IAllowanceRepository>();
        allowanceMock.Setup(x => x.GetEntity(2026, 1))
                                .Returns(entity);

        var vm = new AllowanceViewModel(allowanceMock.Object);

        vm.BasicSalary_Text = 0;
        vm.ExecutiveAllowance_Text = 0;
        vm.DependencyAllowance_Text = 0;
        vm.OvertimeAllowance_Text = 0;
        vm.DaysoffIncreased_Text = 0;
        vm.NightworkIncreased_Text = 0;
        vm.HousingAllowance_Text = 0;
        vm.LateAbsent_Text = 0;
        vm.TransportationExpenses_Text = 0;
        vm.PrepaidRetirementPayment_Text = 0;
        vm.ElectricityAllowance_Text = 0;
        vm.SpecialAllowance_Text = 0;
        vm.SpareAllowance_Text = 0;
        // Remarks_Text は Model.Clear() により null に設定済みのため再設定不要
        // （"" にリセットすると entity.Remarks = "" のとき SetProperty が変化なしと判定し PropertyChanged が発火しない）
        vm.TotalSalary_Text = 0;
        vm.TotalDeductedSalary_Text = 0;

        イベント通知テスト(vm, entity);
    }

    // ==========================================
    // プロパティ値テスト
    // ==========================================

    /// <summary>
    /// BasicSalary_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系）
    /// </remarks>
    [Theory]
    [InlineData(300000)]
    [InlineData(0)]
    [InlineData(-10000)]
    public void BasicSalary_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.BasicSalary_Text = value;

        Assert.Equal(value, vm.BasicSalary_Text);
    }

    /// <summary>
    /// TotalSalary_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// </remarks>
    [Theory]
    [InlineData(300000)]
    [InlineData(0)]
    public void TotalSalary_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.TotalSalary_Text = value;

        Assert.Equal(value, vm.TotalSalary_Text);
    }

    /// <summary>
    /// TotalDeductedSalary_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 控除超過によるマイナス（異常系）
    /// </remarks>
    [Theory]
    [InlineData(250000)]
    [InlineData(0)]
    [InlineData(-50000)]
    public void TotalDeductedSalary_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.TotalDeductedSalary_Text = value;

        Assert.Equal(value, vm.TotalDeductedSalary_Text);
    }

    /// <summary>
    /// Remarks_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 空文字（境界値）
    /// InlineData 2: 通常の文字列（正常系）
    /// InlineData 3: 長い文字列（境界値）
    /// </remarks>
    [Theory]
    [InlineData("")]
    [InlineData("備考テスト")]
    [InlineData("あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん")]
    public void Remarks_Text_値を設定後に読み取る_設定値と一致する(string value)
    {
        var vm = CreateViewModel();

        vm.Remarks_Text = value;

        Assert.Equal(value, vm.Remarks_Text);
    }

    // ==========================================
    // IsEnabled プロパティ - PropertyChanged通知テスト
    // ==========================================

    /// <summary>
    /// IsEnabled 各プロパティを変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    /// <remarks>
    /// Model.Clear() は IsEnabled を初期化しないため、
    /// 構築直後の既定値 false から true への変更でイベント発火を確認する。
    /// </remarks>
    [Fact]
    public void IsEnabled_各プロパティ_値変更時にPropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.ExecutiveAllowance_IsEnabled),
            () => vm.ExecutiveAllowance_IsEnabled = true);

        Assert.PropertyChanged(vm, nameof(vm.DependencyAllowance_IsEnabled),
            () => vm.DependencyAllowance_IsEnabled = true);

        Assert.PropertyChanged(vm, nameof(vm.OvertimeAllowance_IsEnabled),
            () => vm.OvertimeAllowance_IsEnabled = true);

        Assert.PropertyChanged(vm, nameof(vm.NightworkIncreased_IsEnabled),
            () => vm.NightworkIncreased_IsEnabled = true);

        Assert.PropertyChanged(vm, nameof(vm.HousingAllowance_IsEnabled),
            () => vm.HousingAllowance_IsEnabled = true);

        Assert.PropertyChanged(vm, nameof(vm.TransportationExpenses_IsEnabled),
            () => vm.TransportationExpenses_IsEnabled = true);

        Assert.PropertyChanged(vm, nameof(vm.PrepaidRetirementPayment_IsEnabled),
            () => vm.PrepaidRetirementPayment_IsEnabled = true);

        Assert.PropertyChanged(vm, nameof(vm.ElectricityAllowance_IsEnabled),
            () => vm.ElectricityAllowance_IsEnabled = true);

        Assert.PropertyChanged(vm, nameof(vm.SpecialAllowance_IsEnabled),
            () => vm.SpecialAllowance_IsEnabled = true);
    }

    // ==========================================
    // 同値変更テスト
    // ==========================================

    /// <summary>
    /// BasicSalary_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void BasicSalary_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.BasicSalary_Text = 300000;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.BasicSalary_Text))
                fired = true;
        };

        vm.BasicSalary_Text = 300000;

        Assert.False(fired);
    }

    // ==========================================
    // 初期値テスト
    // ==========================================

    /// <summary>
    /// コンストラクタ後、TotalSalary_Foreground の初期値が Blue であることを確認する
    /// </summary>
    [Fact]
    public void TotalSalary_Foreground_コンストラクタ後_初期値はBlueである()
    {
        var vm = CreateViewModel();

        Assert.Equal(Colors.Blue, vm.TotalSalary_Foreground?.Color);
    }

    /// <summary>
    /// コンストラクタ後（Model.Clear() 呼び出し後）、TotalDeductedSalary_Foreground の初期値が Black であることを確認する
    /// </summary>
    [Fact]
    public void TotalDeductedSalary_Foreground_Clear後_初期値はBlackである()
    {
        var vm = CreateViewModel();

        Assert.Equal(Colors.Black, vm.TotalDeductedSalary_Foreground?.Color);
    }

    // ==========================================
    // イベント通知テスト（ヘルパー）
    // ==========================================

    /// <summary>
    /// 全支給額プロパティの PropertyChanged イベント発火を検証する
    /// </summary>
    private void イベント通知テスト(AllowanceViewModel vm, AllowanceValueEntity entity)
    {
        Assert.PropertyChanged(vm, nameof(vm.BasicSalary_Text),
                               () => vm.BasicSalary_Text = entity.BasicSalary.Value);

        Assert.PropertyChanged(vm, nameof(vm.ExecutiveAllowance_Text),
                               () => vm.ExecutiveAllowance_Text = entity.ExecutiveAllowance.Value);

        Assert.PropertyChanged(vm, nameof(vm.DependencyAllowance_Text),
                               () => vm.DependencyAllowance_Text = entity.DependencyAllowance.Value);

        Assert.PropertyChanged(vm, nameof(vm.OvertimeAllowance_Text),
                               () => vm.OvertimeAllowance_Text = entity.OvertimeAllowance.Value);

        Assert.PropertyChanged(vm, nameof(vm.DaysoffIncreased_Text),
                               () => vm.DaysoffIncreased_Text = entity.DaysoffIncreased.Value);

        Assert.PropertyChanged(vm, nameof(vm.NightworkIncreased_Text),
                               () => vm.NightworkIncreased_Text = entity.NightworkIncreased.Value);

        Assert.PropertyChanged(vm, nameof(vm.HousingAllowance_Text),
                               () => vm.HousingAllowance_Text = entity.HousingAllowance.Value);

        Assert.PropertyChanged(vm, nameof(vm.LateAbsent_Text),
                               () => vm.LateAbsent_Text = entity.LateAbsent);

        Assert.PropertyChanged(vm, nameof(vm.TransportationExpenses_Text),
                               () => vm.TransportationExpenses_Text = entity.TransportationExpenses.Value);

        Assert.PropertyChanged(vm, nameof(vm.PrepaidRetirementPayment_Text),
                               () => vm.PrepaidRetirementPayment_Text = entity.PrepaidRetirementPayment.Value);

        Assert.PropertyChanged(vm, nameof(vm.ElectricityAllowance_Text),
                               () => vm.ElectricityAllowance_Text = entity.ElectricityAllowance.Value);

        Assert.PropertyChanged(vm, nameof(vm.SpecialAllowance_Text),
                               () => vm.SpecialAllowance_Text = entity.SpecialAllowance);

        Assert.PropertyChanged(vm, nameof(vm.SpareAllowance_Text),
                               () => vm.SpareAllowance_Text = entity.SpareAllowance);

        Assert.PropertyChanged(vm, nameof(vm.Remarks_Text),
                               () => vm.Remarks_Text = entity.Remarks);

        Assert.PropertyChanged(vm, nameof(vm.TotalSalary_Text),
                               () => vm.TotalSalary_Text = entity.TotalSalary.Value);

        Assert.PropertyChanged(vm, nameof(vm.TotalDeductedSalary_Text),
                               () => vm.TotalDeductedSalary_Text = entity.TotalDeductedSalary.Value);
    }
}
