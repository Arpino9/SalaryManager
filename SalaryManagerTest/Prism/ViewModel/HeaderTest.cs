using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - ヘッダ テスト
/// </summary>
public class HeaderTest
{
    #region ヘルパー

    /// <summary>テスト用ViewModelを生成する</summary>
    private static HeaderViewModel CreateViewModel()
    {
        var mock = new Mock<IHeaderRepository>();
        return new HeaderViewModel(mock.Object);
    }

    #endregion

    // ==========================================
    // 初期化 - PropertyChanged 通知テスト
    // ==========================================

    /// <summary>
    /// 年・月プロパティに値を設定したとき PropertyChanged が発火することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 現在年・1月（正常系）
    /// InlineData 2: 過去年・12月（境界月）
    /// InlineData 3: 遠い過去年・6月（中間月）
    /// </remarks>
    [Theory]
    [InlineData(2026, 1)]
    [InlineData(2000, 12)]
    [InlineData(1990, 6)]
    public void 初期化(int year, int month)
    {
        var mock = new Mock<IHeaderRepository>();
        var vm = new HeaderViewModel(mock.Object);

        // フィールド初期化値（DateTime.Now.Year/Month）と衝突しないよう 0 にリセット
        vm.Year_Text  = 0;
        vm.Month_Text = 0;

        Assert.PropertyChanged(vm, nameof(vm.Year_Text),
                               () => vm.Year_Text = year);

        Assert.PropertyChanged(vm, nameof(vm.Month_Text),
                               () => vm.Month_Text = month);
    }

    // ==========================================
    // プロパティ値テスト
    // ==========================================

    /// <summary>
    /// Year_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の西暦年（正常系）
    /// InlineData 2: 過去年（正常系）
    /// InlineData 3: 最小の4桁年（境界値）
    /// InlineData 4: 最大の4桁年（境界値）
    /// InlineData 5: ゼロ（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(2026)]
    [InlineData(1990)]
    [InlineData(1000)]
    [InlineData(9999)]
    [InlineData(0)]
    public void Year_Text_値を設定後に読み取る_設定値と一致する(int value)
    {
        var vm = CreateViewModel();

        vm.Year_Text = value;

        Assert.Equal(value, vm.Year_Text);
    }

    /// <summary>
    /// Month_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 最小有効月（境界値）
    /// InlineData 2: 最大有効月（境界値）
    /// InlineData 3: 中間月（正常系）
    /// InlineData 4: ゼロ（異常系 / IsValid_Month で 1 に修正されるが ViewModel 単体では素通り）
    /// InlineData 5: 13（異常系 / IsValid_Month で 12 に修正されるが ViewModel 単体では素通り）
    /// </remarks>
    [Theory]
    [InlineData(1)]
    [InlineData(12)]
    [InlineData(6)]
    [InlineData(0)]
    [InlineData(13)]
    public void Month_Text_値を設定後に読み取る_設定値と一致する(int value)
    {
        var vm = CreateViewModel();

        vm.Month_Text = value;

        Assert.Equal(value, vm.Month_Text);
    }

    /// <summary>
    /// Window_Background に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    [Fact]
    public void Window_Background_値を設定後に読み取る_設定値と一致する()
    {
        var vm = CreateViewModel();
        var brush = new SolidColorBrush(Colors.LightBlue);

        vm.Window_Background = brush;

        Assert.Equal(brush, vm.Window_Background);
    }

    // ==========================================
    // 初期値テスト
    // ==========================================

    /// <summary>
    /// コンストラクタ後、Year_Text の初期値が現在年であることを確認する
    /// </summary>
    [Fact]
    public void Year_Text_コンストラクタ後_現在年が設定されている()
    {
        var vm = CreateViewModel();

        Assert.Equal(DateTime.Now.Year, vm.Year_Text);
    }

    /// <summary>
    /// コンストラクタ後、Month_Text の初期値が現在月であることを確認する
    /// </summary>
    [Fact]
    public void Month_Text_コンストラクタ後_現在月が設定されている()
    {
        var vm = CreateViewModel();

        Assert.Equal(DateTime.Now.Month, vm.Month_Text);
    }

    /// <summary>
    /// テスト用コンストラクタ後、Window_Background の初期値が null であることを確認する
    /// </summary>
    /// <remarks>
    /// 本番コンストラクタは Initialize() → Window_Activated() で背景色を設定するが、
    /// テスト用コンストラクタはそれらを呼び出さないため null のままとなる。
    /// </remarks>
    [Fact]
    public void Window_Background_テスト用コンストラクタ後_nullである()
    {
        var vm = CreateViewModel();

        Assert.Null(vm.Window_Background);
    }

    // ==========================================
    // 同値変更テスト
    // ==========================================

    /// <summary>
    /// Year_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void Year_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.Year_Text = 2026;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Year_Text))
                fired = true;
        };

        vm.Year_Text = 2026;

        Assert.False(fired);
    }

    /// <summary>
    /// Month_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void Month_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.Month_Text = 4;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Month_Text))
                fired = true;
        };

        vm.Month_Text = 4;

        Assert.False(fired);
    }

    // ==========================================
    // PropertyChanged 個別テスト
    // ==========================================

    /// <summary>
    /// Year_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Year_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.Year_Text),
                               () => vm.Year_Text = 2000);
    }

    /// <summary>
    /// Month_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Month_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        vm.Month_Text = 0; // 初期値（現在月）と衝突しないようリセット

        Assert.PropertyChanged(vm, nameof(vm.Month_Text),
                               () => vm.Month_Text = 3);
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
}
