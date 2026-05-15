using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - 月収一覧 テスト
/// </summary>
public class AnnualChartTest
{
    #region ヘルパー

    /// <summary>テスト用ViewModelを生成する（DB / XML アクセスなし）</summary>
    private static AnnualChartViewModel CreateViewModel()
        => new AnnualChartViewModel(isTest: true);

    #endregion

    // ==========================================
    // 初期値テスト
    // ==========================================

    /// <summary>
    /// テスト用コンストラクタ後、各月の TotalSalary_Text がゼロであることを確認する
    /// </summary>
    [Fact]
    public void 各月TotalSalaryプロパティ_コンストラクタ後_初期値はゼロである()
    {
        var vm = CreateViewModel();

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
    /// テスト用コンストラクタ後、各月の TotalDeductedSalary_Text がゼロであることを確認する
    /// </summary>
    [Fact]
    public void 各月TotalDeductedSalaryプロパティ_コンストラクタ後_初期値はゼロである()
    {
        var vm = CreateViewModel();

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
    /// テスト用コンストラクタ後、各月の TotalSideBusiness_Text がゼロであることを確認する
    /// </summary>
    [Fact]
    public void 各月TotalSideBusinessプロパティ_コンストラクタ後_初期値はゼロである()
    {
        var vm = CreateViewModel();

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

    /// <summary>
    /// テスト用コンストラクタ後、合計プロパティがゼロであることを確認する
    /// </summary>
    [Fact]
    public void 合計プロパティ_コンストラクタ後_初期値はゼロである()
    {
        var vm = CreateViewModel();

        Assert.Equal(0, vm.TotalSalary_Sum_Text);
        Assert.Equal(0, vm.TotalDeductedSalary_Sum_Text);
        Assert.Equal(0, vm.TotalSideBusiness_Sum_Text);
    }

    /// <summary>
    /// テスト用コンストラクタ後、Window_Background の初期値が null であることを確認する
    /// </summary>
    [Fact]
    public void Window_Background_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.Window_Background);
    }

    /// <summary>
    /// テスト用コンストラクタ後、TargetDate_Content の初期値が null であることを確認する
    /// </summary>
    [Fact]
    public void TargetDate_Content_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.TargetDate_Content);
    }

    // ==========================================
    // プロパティ値テスト（代表値）
    // ==========================================

    /// <summary>
    /// January_TotalSalary_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の給与（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(300000)]
    [InlineData(0)]
    [InlineData(-10000)]
    public void January_TotalSalary_Text_値を設定後に読み取る_設定値と一致する(int value)
    {
        var vm = CreateViewModel();
        vm.January_TotalSalary_Text = value;
        Assert.Equal(value, vm.January_TotalSalary_Text);
    }

    /// <summary>
    /// January_TotalDeductedSalary_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の差引支給額（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(250000)]
    [InlineData(0)]
    [InlineData(-5000)]
    public void January_TotalDeductedSalary_Text_値を設定後に読み取る_設定値と一致する(int value)
    {
        var vm = CreateViewModel();
        vm.January_TotalDeductedSalary_Text = value;
        Assert.Equal(value, vm.January_TotalDeductedSalary_Text);
    }

    /// <summary>
    /// January_TotalSideBusiness_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の副業額（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// </remarks>
    [Theory]
    [InlineData(50000)]
    [InlineData(0)]
    public void January_TotalSideBusiness_Text_値を設定後に読み取る_設定値と一致する(int value)
    {
        var vm = CreateViewModel();
        vm.January_TotalSideBusiness_Text = value;
        Assert.Equal(value, vm.January_TotalSideBusiness_Text);
    }

    /// <summary>
    /// TotalSalary_Sum_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 年間総支給（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// </remarks>
    [Theory]
    [InlineData(3600000)]
    [InlineData(0)]
    public void TotalSalary_Sum_Text_値を設定後に読み取る_設定値と一致する(int value)
    {
        var vm = CreateViewModel();
        vm.TotalSalary_Sum_Text = value;
        Assert.Equal(value, vm.TotalSalary_Sum_Text);
    }

    /// <summary>
    /// TargetDate_Content に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 元号付き年度（正常系）
    /// InlineData 2: 空文字（境界値）
    /// InlineData 3: null（境界値）
    /// </remarks>
    [Theory]
    [InlineData("令和8年")]
    [InlineData("")]
    [InlineData(null)]
    public void TargetDate_Content_値を設定後に読み取る_設定値と一致する(string? value)
    {
        var vm = CreateViewModel();
        vm.TargetDate_Content = value;
        Assert.Equal(value, vm.TargetDate_Content);
    }

    // ==========================================
    // PropertyChanged 通知テスト - 各月 TotalSalary
    // ==========================================

    /// <summary>
    /// 各月の TotalSalary_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void 全月TotalSalary_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.January_TotalSalary_Text),   () => vm.January_TotalSalary_Text   = 300000);
        Assert.PropertyChanged(vm, nameof(vm.Feburary_TotalSalary_Text),  () => vm.Feburary_TotalSalary_Text  = 300000);
        Assert.PropertyChanged(vm, nameof(vm.March_TotalSalary_Text),     () => vm.March_TotalSalary_Text     = 300000);
        Assert.PropertyChanged(vm, nameof(vm.April_TotalSalary_Text),     () => vm.April_TotalSalary_Text     = 300000);
        Assert.PropertyChanged(vm, nameof(vm.May_TotalSalary_Text),       () => vm.May_TotalSalary_Text       = 300000);
        Assert.PropertyChanged(vm, nameof(vm.June_TotalSalary_Text),      () => vm.June_TotalSalary_Text      = 300000);
        Assert.PropertyChanged(vm, nameof(vm.July_TotalSalary_Text),      () => vm.July_TotalSalary_Text      = 300000);
        Assert.PropertyChanged(vm, nameof(vm.August_TotalSalary_Text),    () => vm.August_TotalSalary_Text    = 300000);
        Assert.PropertyChanged(vm, nameof(vm.September_TotalSalary_Text), () => vm.September_TotalSalary_Text = 300000);
        Assert.PropertyChanged(vm, nameof(vm.October_TotalSalary_Text),   () => vm.October_TotalSalary_Text   = 300000);
        Assert.PropertyChanged(vm, nameof(vm.November_TotalSalary_Text),  () => vm.November_TotalSalary_Text  = 300000);
        Assert.PropertyChanged(vm, nameof(vm.December_TotalSalary_Text),  () => vm.December_TotalSalary_Text  = 300000);
    }

    // ==========================================
    // PropertyChanged 通知テスト - 各月 TotalDeductedSalary
    // ==========================================

    /// <summary>
    /// 各月の TotalDeductedSalary_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void 全月TotalDeductedSalary_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.January_TotalDeductedSalary_Text),   () => vm.January_TotalDeductedSalary_Text   = 250000);
        Assert.PropertyChanged(vm, nameof(vm.Feburary_TotalDeductedSalary_Text),  () => vm.Feburary_TotalDeductedSalary_Text  = 250000);
        Assert.PropertyChanged(vm, nameof(vm.March_TotalDeductedSalary_Text),     () => vm.March_TotalDeductedSalary_Text     = 250000);
        Assert.PropertyChanged(vm, nameof(vm.April_TotalDeductedSalary_Text),     () => vm.April_TotalDeductedSalary_Text     = 250000);
        Assert.PropertyChanged(vm, nameof(vm.May_TotalDeductedSalary_Text),       () => vm.May_TotalDeductedSalary_Text       = 250000);
        Assert.PropertyChanged(vm, nameof(vm.June_TotalDeductedSalary_Text),      () => vm.June_TotalDeductedSalary_Text      = 250000);
        Assert.PropertyChanged(vm, nameof(vm.July_TotalDeductedSalary_Text),      () => vm.July_TotalDeductedSalary_Text      = 250000);
        Assert.PropertyChanged(vm, nameof(vm.August_TotalDeductedSalary_Text),    () => vm.August_TotalDeductedSalary_Text    = 250000);
        Assert.PropertyChanged(vm, nameof(vm.September_TotalDeductedSalary_Text), () => vm.September_TotalDeductedSalary_Text = 250000);
        Assert.PropertyChanged(vm, nameof(vm.October_TotalDeductedSalary_Text),   () => vm.October_TotalDeductedSalary_Text   = 250000);
        Assert.PropertyChanged(vm, nameof(vm.November_TotalDeductedSalary_Text),  () => vm.November_TotalDeductedSalary_Text  = 250000);
        Assert.PropertyChanged(vm, nameof(vm.December_TotalDeductedSalary_Text),  () => vm.December_TotalDeductedSalary_Text  = 250000);
    }

    // ==========================================
    // PropertyChanged 通知テスト - 各月 TotalSideBusiness
    // ==========================================

    /// <summary>
    /// 各月の TotalSideBusiness_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void 全月TotalSideBusiness_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.January_TotalSideBusiness_Text),   () => vm.January_TotalSideBusiness_Text   = 50000);
        Assert.PropertyChanged(vm, nameof(vm.Feburary_TotalSideBusiness_Text),  () => vm.Feburary_TotalSideBusiness_Text  = 50000);
        Assert.PropertyChanged(vm, nameof(vm.March_TotalSideBusiness_Text),     () => vm.March_TotalSideBusiness_Text     = 50000);
        Assert.PropertyChanged(vm, nameof(vm.April_TotalSideBusiness_Text),     () => vm.April_TotalSideBusiness_Text     = 50000);
        Assert.PropertyChanged(vm, nameof(vm.May_TotalSideBusiness_Text),       () => vm.May_TotalSideBusiness_Text       = 50000);
        Assert.PropertyChanged(vm, nameof(vm.June_TotalSideBusiness_Text),      () => vm.June_TotalSideBusiness_Text      = 50000);
        Assert.PropertyChanged(vm, nameof(vm.July_TotalSideBusiness_Text),      () => vm.July_TotalSideBusiness_Text      = 50000);
        Assert.PropertyChanged(vm, nameof(vm.August_TotalSideBusiness_Text),    () => vm.August_TotalSideBusiness_Text    = 50000);
        Assert.PropertyChanged(vm, nameof(vm.September_TotalSideBusiness_Text), () => vm.September_TotalSideBusiness_Text = 50000);
        Assert.PropertyChanged(vm, nameof(vm.October_TotalSideBusiness_Text),   () => vm.October_TotalSideBusiness_Text   = 50000);
        Assert.PropertyChanged(vm, nameof(vm.November_TotalSideBusiness_Text),  () => vm.November_TotalSideBusiness_Text  = 50000);
        Assert.PropertyChanged(vm, nameof(vm.December_TotalSideBusiness_Text),  () => vm.December_TotalSideBusiness_Text  = 50000);
    }

    // ==========================================
    // PropertyChanged 通知テスト - 合計 / 画面
    // ==========================================

    /// <summary>
    /// 合計プロパティを変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void 合計プロパティ_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.TotalSalary_Sum_Text),
            () => vm.TotalSalary_Sum_Text = 3600000);

        Assert.PropertyChanged(vm, nameof(vm.TotalDeductedSalary_Sum_Text),
            () => vm.TotalDeductedSalary_Sum_Text = 3000000);

        Assert.PropertyChanged(vm, nameof(vm.TotalSideBusiness_Sum_Text),
            () => vm.TotalSideBusiness_Sum_Text = 600000);
    }

    /// <summary>
    /// Window_Background を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Window_Background_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Window_Background),
            () => vm.Window_Background = new SolidColorBrush(Colors.AliceBlue));
    }

    /// <summary>
    /// TargetDate_Content を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void TargetDate_Content_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.TargetDate_Content),
            () => vm.TargetDate_Content = "令和8年");
    }

    // ==========================================
    // 同値変更テスト
    // ==========================================

    /// <summary>
    /// January_TotalSalary_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void January_TotalSalary_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.January_TotalSalary_Text = 300000;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.January_TotalSalary_Text)) fired = true;
        };

        vm.January_TotalSalary_Text = 300000;

        Assert.False(fired);
    }

    /// <summary>
    /// TargetDate_Content に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void TargetDate_Content_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.TargetDate_Content = "令和8年";

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.TargetDate_Content)) fired = true;
        };

        vm.TargetDate_Content = "令和8年";

        Assert.False(fired);
    }

    // ==========================================
    // 合計計算テスト（ViewModel プロパティ算術）
    // ==========================================

    /// <summary>
    /// 各月の TotalSalary_Text を設定し、手動で合算した値が TotalSalary_Sum_Text と一致することを確認する
    /// </summary>
    /// <remarks>
    /// Model.Recalculate() は internal のため、ViewModel プロパティを直接操作して
    /// 算術的整合性を検証する。
    /// </remarks>
    [Fact]
    public void TotalSalary_Sum_Text_各月を設定して手動合算_値が一致する()
    {
        var vm = CreateViewModel();

        vm.January_TotalSalary_Text   = 300000;
        vm.Feburary_TotalSalary_Text  = 310000;
        vm.March_TotalSalary_Text     = 320000;
        vm.April_TotalSalary_Text     = 330000;
        vm.May_TotalSalary_Text       = 340000;
        vm.June_TotalSalary_Text      = 350000;
        vm.July_TotalSalary_Text      = 360000;
        vm.August_TotalSalary_Text    = 370000;
        vm.September_TotalSalary_Text = 380000;
        vm.October_TotalSalary_Text   = 390000;
        vm.November_TotalSalary_Text  = 400000;
        vm.December_TotalSalary_Text  = 410000;

        var expected = vm.January_TotalSalary_Text
                     + vm.Feburary_TotalSalary_Text
                     + vm.March_TotalSalary_Text
                     + vm.April_TotalSalary_Text
                     + vm.May_TotalSalary_Text
                     + vm.June_TotalSalary_Text
                     + vm.July_TotalSalary_Text
                     + vm.August_TotalSalary_Text
                     + vm.September_TotalSalary_Text
                     + vm.October_TotalSalary_Text
                     + vm.November_TotalSalary_Text
                     + vm.December_TotalSalary_Text;

        vm.TotalSalary_Sum_Text = expected;

        Assert.Equal(4260000, vm.TotalSalary_Sum_Text);
    }

    /// <summary>
    /// 全月ゼロのとき TotalSalary_Sum_Text も 0 となることを確認する
    /// </summary>
    [Fact]
    public void TotalSalary_Sum_Text_全月ゼロ_合計もゼロである()
    {
        var vm = CreateViewModel();

        // Clear() により全月 0 が保証されている
        vm.TotalSalary_Sum_Text = vm.January_TotalSalary_Text
                                + vm.Feburary_TotalSalary_Text
                                + vm.March_TotalSalary_Text
                                + vm.April_TotalSalary_Text
                                + vm.May_TotalSalary_Text
                                + vm.June_TotalSalary_Text
                                + vm.July_TotalSalary_Text
                                + vm.August_TotalSalary_Text
                                + vm.September_TotalSalary_Text
                                + vm.October_TotalSalary_Text
                                + vm.November_TotalSalary_Text
                                + vm.December_TotalSalary_Text;

        Assert.Equal(0, vm.TotalSalary_Sum_Text);
    }
}
