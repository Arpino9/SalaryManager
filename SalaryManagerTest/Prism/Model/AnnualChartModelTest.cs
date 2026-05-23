using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;

namespace SalaryManagerTest;

/// <summary>
/// Model - 月収一覧 テスト
/// </summary>
/// <remarks>
/// 対象外メソッド:
/// - Reload            : AnnualCharts.Create（静的ドメインクラス）に依存するため除外
/// - Initialize（本体）: Year_Text 4桁 → AnnualCharts.Create に到達するため除外
/// - GetPreviousXxx 系 : AnnualCharts.Fetch（静的ドメインクラス）に依存するため除外
/// - PreviousXxx 系プロパティ : 上記プライベートメソッド経由のため除外
/// </remarks>
public class AnnualChartModelTest
{
    #region ヘルパー

    /// <summary>テスト用 AnnualChartModel を生成する（シングルトン非使用）</summary>
    private static AnnualChartModel CreateModel() => new AnnualChartModel();

    /// <summary>ViewModel を紐付けた AnnualChartModel を生成する</summary>
    private static (AnnualChartModel model, AnnualChartViewModel vm) CreateModelWithViewModel()
    {
        var model = CreateModel();
        var vm    = new AnnualChartViewModel(isTest: true);
        model.ViewModel = vm;
        return (model, vm);
    }

    /// <summary>テスト用 HeaderViewModel を生成する</summary>
    private static HeaderViewModel CreateHeaderViewModel(int year = 2026, int month = 5)
    {
        var mock = new Mock<IHeaderRepository>();
        mock.Setup(r => r.GetEntities()).Returns(new List<HeaderEntity>());
        mock.Setup(r => r.FetchDefault()).Returns((HeaderEntity?)null);
        var vm = new HeaderViewModel(mock.Object);
        vm.Year_Text  = year;
        vm.Month_Text = month;
        return vm;
    }

    /// <summary>
    /// ViewModel の各月支給額プロパティをすべて設定する
    /// </summary>
    private static void SetAllMonthlySalary(AnnualChartViewModel vm, int value)
    {
        vm.January_TotalSalary_Text   = value;
        vm.Feburary_TotalSalary_Text  = value;
        vm.March_TotalSalary_Text     = value;
        vm.April_TotalSalary_Text     = value;
        vm.May_TotalSalary_Text       = value;
        vm.June_TotalSalary_Text      = value;
        vm.July_TotalSalary_Text      = value;
        vm.August_TotalSalary_Text    = value;
        vm.September_TotalSalary_Text = value;
        vm.October_TotalSalary_Text   = value;
        vm.November_TotalSalary_Text  = value;
        vm.December_TotalSalary_Text  = value;
    }

    /// <summary>
    /// ViewModel の各月差引支給額プロパティをすべて設定する
    /// </summary>
    private static void SetAllMonthlyDeductedSalary(AnnualChartViewModel vm, int value)
    {
        vm.January_TotalDeductedSalary_Text   = value;
        vm.Feburary_TotalDeductedSalary_Text  = value;
        vm.March_TotalDeductedSalary_Text     = value;
        vm.April_TotalDeductedSalary_Text     = value;
        vm.May_TotalDeductedSalary_Text       = value;
        vm.June_TotalDeductedSalary_Text      = value;
        vm.July_TotalDeductedSalary_Text      = value;
        vm.August_TotalDeductedSalary_Text    = value;
        vm.September_TotalDeductedSalary_Text = value;
        vm.October_TotalDeductedSalary_Text   = value;
        vm.November_TotalDeductedSalary_Text  = value;
        vm.December_TotalDeductedSalary_Text  = value;
    }

    /// <summary>
    /// ViewModel の各月副業額プロパティをすべて設定する
    /// </summary>
    private static void SetAllMonthlySideBusiness(AnnualChartViewModel vm, int value)
    {
        vm.January_TotalSideBusiness_Text   = value;
        vm.Feburary_TotalSideBusiness_Text  = value;
        vm.March_TotalSideBusiness_Text     = value;
        vm.April_TotalSideBusiness_Text     = value;
        vm.May_TotalSideBusiness_Text       = value;
        vm.June_TotalSideBusiness_Text      = value;
        vm.July_TotalSideBusiness_Text      = value;
        vm.August_TotalSideBusiness_Text    = value;
        vm.September_TotalSideBusiness_Text = value;
        vm.October_TotalSideBusiness_Text   = value;
        vm.November_TotalSideBusiness_Text  = value;
        vm.December_TotalSideBusiness_Text  = value;
    }

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
        var instance1 = AnnualChartModel.GetInstance();
        var instance2 = AnnualChartModel.GetInstance();

        Assert.Same(instance1, instance2);
    }

    // ==========================================
    // Clear - 全項目を初期値にリセット
    // ==========================================

    /// <summary>
    /// Clear 後、全ての支給額プロパティ（12ヶ月）がゼロになることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_全支給額プロパティがゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlySalary(vm, 300000);

        model.Clear();

        Assert.Equal(0, vm.January_TotalSalary_Text);
        Assert.Equal(0, vm.Feburary_TotalSalary_Text);
        Assert.Equal(0, vm.March_TotalSalary_Text);
        Assert.Equal(0, vm.April_TotalSalary_Text);
        Assert.Equal(0, vm.May_TotalSalary_Text);
        Assert.Equal(0, vm.June_TotalSalary_Text);
        Assert.Equal(0, vm.July_TotalSalary_Text);
        Assert.Equal(0, vm.August_TotalSalary_Text);
        Assert.Equal(0, vm.September_TotalSalary_Text);
        Assert.Equal(0, vm.October_TotalSalary_Text);
        Assert.Equal(0, vm.November_TotalSalary_Text);
        Assert.Equal(0, vm.December_TotalSalary_Text);
    }

    /// <summary>
    /// Clear 後、全ての差引支給額プロパティ（12ヶ月）がゼロになることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_全差引支給額プロパティがゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlyDeductedSalary(vm, 250000);

        model.Clear();

        Assert.Equal(0, vm.January_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.Feburary_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.March_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.April_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.May_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.June_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.July_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.August_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.September_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.October_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.November_TotalDeductedSalary_Text);
        Assert.Equal(0, vm.December_TotalDeductedSalary_Text);
    }

    /// <summary>
    /// Clear 後、全ての副業額プロパティ（12ヶ月）がゼロになることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_全副業額プロパティがゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlySideBusiness(vm, 50000);

        model.Clear();

        Assert.Equal(0, vm.January_TotalSideBusiness_Text);
        Assert.Equal(0, vm.Feburary_TotalSideBusiness_Text);
        Assert.Equal(0, vm.March_TotalSideBusiness_Text);
        Assert.Equal(0, vm.April_TotalSideBusiness_Text);
        Assert.Equal(0, vm.May_TotalSideBusiness_Text);
        Assert.Equal(0, vm.June_TotalSideBusiness_Text);
        Assert.Equal(0, vm.July_TotalSideBusiness_Text);
        Assert.Equal(0, vm.August_TotalSideBusiness_Text);
        Assert.Equal(0, vm.September_TotalSideBusiness_Text);
        Assert.Equal(0, vm.October_TotalSideBusiness_Text);
        Assert.Equal(0, vm.November_TotalSideBusiness_Text);
        Assert.Equal(0, vm.December_TotalSideBusiness_Text);
    }

    // ==========================================
    // Recalculate - 集計再計算
    // ==========================================

    /// <summary>
    /// 全ての月がゼロのとき、支給額計がゼロになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Recalculate_全月ゼロ_支給額計がゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlySalary(vm, 0);

        model.Recalculate();

        Assert.Equal(0, vm.TotalSalary_Sum_Text);
    }

    /// <summary>
    /// 全ての月がゼロのとき、差引支給額計がゼロになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Recalculate_全月ゼロ_差引支給額計がゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlyDeductedSalary(vm, 0);

        model.Recalculate();

        Assert.Equal(0, vm.TotalDeductedSalary_Sum_Text);
    }

    /// <summary>
    /// 全ての月がゼロのとき、副業額計がゼロになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Recalculate_全月ゼロ_副業額計がゼロになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlySideBusiness(vm, 0);

        model.Recalculate();

        Assert.Equal(0, vm.TotalSideBusiness_Sum_Text);
    }

    /// <summary>
    /// 各月の支給額が 100,000 のとき、年間支給額計が 1,200,000 になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Recalculate_各月100000_支給額計が1200000になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlySalary(vm, 100000);

        model.Recalculate();

        Assert.Equal(1200000, vm.TotalSalary_Sum_Text);
    }

    /// <summary>
    /// 各月の差引支給額が 80,000 のとき、年間差引支給額計が 960,000 になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Recalculate_各月80000_差引支給額計が960000になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlyDeductedSalary(vm, 80000);

        model.Recalculate();

        Assert.Equal(960000, vm.TotalDeductedSalary_Sum_Text);
    }

    /// <summary>
    /// 各月の副業額が 30,000 のとき、年間副業額計が 360,000 になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Recalculate_各月30000_副業額計が360000になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlySideBusiness(vm, 30000);

        model.Recalculate();

        Assert.Equal(360000, vm.TotalSideBusiness_Sum_Text);
    }

    /// <summary>
    /// 一部の月のみ支給額がある場合、その合計が正しく算出されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Recalculate_一部月のみ値あり_支給額計が正しく算出される()
    {
        var (model, vm) = CreateModelWithViewModel();
        // 1月・7月・12月のみ設定、他は 0
        vm.January_TotalSalary_Text  = 300000;
        vm.July_TotalSalary_Text     = 320000;
        vm.December_TotalSalary_Text = 350000;

        model.Recalculate();

        Assert.Equal(970000, vm.TotalSalary_Sum_Text);
    }

    /// <summary>
    /// 一部の月のみ差引支給額がある場合、その合計が正しく算出されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Recalculate_一部月のみ値あり_差引支給額計が正しく算出される()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.January_TotalDeductedSalary_Text  = 250000;
        vm.December_TotalDeductedSalary_Text = 270000;

        model.Recalculate();

        Assert.Equal(520000, vm.TotalDeductedSalary_Sum_Text);
    }

    /// <summary>
    /// 一部の月のみ副業額がある場合、その合計が正しく算出されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Recalculate_一部月のみ値あり_副業額計が正しく算出される()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.June_TotalSideBusiness_Text      = 50000;
        vm.September_TotalSideBusiness_Text = 60000;

        model.Recalculate();

        Assert.Equal(110000, vm.TotalSideBusiness_Sum_Text);
    }

    /// <summary>
    /// 各月の支給額が大きな値のとき、int の範囲内で正しく合計されることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Recalculate_大きな値_支給額計が正しく算出される()
    {
        var (model, vm) = CreateModelWithViewModel();
        // 12 ヶ月 × 100,000,000 = 1,200,000,000 (int.MaxValue ≈ 2,147,483,647 の範囲内)
        SetAllMonthlySalary(vm, 100000000);

        model.Recalculate();

        Assert.Equal(1200000000, vm.TotalSalary_Sum_Text);
    }

    /// <summary>
    /// 月の値が異なるとき、支給額計・差引支給額計・副業額計が独立して算出されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Recalculate_各カテゴリに異なる値_それぞれ独立して算出される()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlySalary(vm, 300000);
        SetAllMonthlyDeductedSalary(vm, 240000);
        SetAllMonthlySideBusiness(vm, 50000);

        model.Recalculate();

        Assert.Equal(3600000, vm.TotalSalary_Sum_Text);
        Assert.Equal(2880000, vm.TotalDeductedSalary_Sum_Text);
        Assert.Equal(600000,  vm.TotalSideBusiness_Sum_Text);
    }

    /// <summary>
    /// Recalculate 後に値を変更して再度 Recalculate したとき、合計が正しく更新されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Recalculate_再計算後に値変更_合計が正しく更新される()
    {
        var (model, vm) = CreateModelWithViewModel();
        SetAllMonthlySalary(vm, 100000);
        model.Recalculate();
        Assert.Equal(1200000, vm.TotalSalary_Sum_Text);

        // 1月の支給額を変更して再計算
        vm.January_TotalSalary_Text = 200000;
        model.Recalculate();

        Assert.Equal(1300000, vm.TotalSalary_Sum_Text);
    }

    // ==========================================
    // Initialize - 早期リターン（年桁数チェック）
    // ==========================================

    /// <summary>
    /// Year_Text が 3 桁のとき、Initialize が早期リターンし TargetDate_Content が null のままになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Initialize_年が3桁_早期リターンしTargetDate_ContentがNullのまま()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Header = CreateHeaderViewModel(year: 999, month: 5);

        model.Initialize();

        Assert.Null(vm.TargetDate_Content);
    }

    /// <summary>
    /// Year_Text が 5 桁のとき、Initialize が早期リターンし TargetDate_Content が null のままになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Initialize_年が5桁_早期リターンしTargetDate_ContentがNullのまま()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Header = CreateHeaderViewModel(year: 10000, month: 5);

        model.Initialize();

        Assert.Null(vm.TargetDate_Content);
    }

    /// <summary>
    /// Year_Text が 0 のとき、Initialize が早期リターンし TargetDate_Content が null のままになることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Initialize_年がゼロ_早期リターンしTargetDate_ContentがNullのまま()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Header = CreateHeaderViewModel(year: 0, month: 5);

        model.Initialize();

        Assert.Null(vm.TargetDate_Content);
    }

    /// <summary>
    /// Year_Text が 2 桁のとき、Initialize が早期リターンし TargetDate_Content が null のままになることを確認する（異常系）
    /// </summary>
    [Fact]
    public void Initialize_年が2桁_早期リターンしTargetDate_ContentがNullのまま()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Header = CreateHeaderViewModel(year: 99, month: 5);

        model.Initialize();

        Assert.Null(vm.TargetDate_Content);
    }

    /// <summary>
    /// Year_Text が 1 桁のとき、Initialize が早期リターンし TargetDate_Content が null のままになることを確認する（異常系）
    /// </summary>
    [Fact]
    public void Initialize_年が1桁_早期リターンしTargetDate_ContentがNullのまま()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Header = CreateHeaderViewModel(year: 9, month: 5);

        model.Initialize();

        Assert.Null(vm.TargetDate_Content);
    }
}
