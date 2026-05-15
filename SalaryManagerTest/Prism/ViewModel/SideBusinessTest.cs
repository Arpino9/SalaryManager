using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - 副業 テスト
/// </summary>
public class SideBusinessTest
{
    #region ヘルパー

    /// <summary>テスト用ViewModelを生成する</summary>
    private static SideBusinessViewModel CreateViewModel()
    {
        var mock = new Mock<ISideBusinessRepository>();
        return new SideBusinessViewModel(mock.Object);
    }

    /// <summary>テスト用Entityを生成する</summary>
    private static SideBusinessEntity CreateEntity(
        double sideBusiness,
        double perquisite,
        double others,
        string remarks)
        => new SideBusinessEntity(
            1,
            DateOnly.FromDateTime(DateTime.Now),
            sideBusiness,
            perquisite,
            others,
            remarks);

    #endregion

    // ==========================================
    // 初期化 - PropertyChanged 通知テスト
    // ==========================================

    /// <summary>
    /// 各プロパティに値を設定したとき PropertyChanged が発火することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の副業収入データ（正常系）
    /// InlineData 2: 全ゼロ・空文字（境界値）
    /// InlineData 3: 大きな値（境界値）
    /// </remarks>
    [Theory]
    [InlineData(50000,   30000,  10000,  "テスト備考")]
    [InlineData(0,       0,      0,      "")]
    [InlineData(9999999, 9999999, 9999999, "最大値テスト")]
    public void 初期化(
        double sideBusiness,
        double perquisite,
        double others,
        string remarks)
    {
        var entity = CreateEntity(sideBusiness, perquisite, others, remarks);

        var mock = new Mock<ISideBusinessRepository>();
        var vm = new SideBusinessViewModel(mock.Object);

        // -1 にリセット（entity 値と重複しないようにする）
        vm.SideBusiness_Text = -1;
        vm.Perquisite_Text   = -1;
        vm.Others_Text       = -1;
        // Remarks_Text は Clear() により null 設定済みのため再設定不要
        // （"" にリセットすると entity.Remarks = "" のとき SetProperty が変化なしと判定し PropertyChanged が発火しない）

        イベント通知テスト(vm, entity);
    }

    // ==========================================
    // プロパティ値テスト
    // ==========================================

    /// <summary>
    /// SideBusiness_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の副業収入（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(50000)]
    [InlineData(0)]
    [InlineData(-10000)]
    public void SideBusiness_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.SideBusiness_Text = value;
        Assert.Equal(value, vm.SideBusiness_Text);
    }

    /// <summary>
    /// Perquisite_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の臨時収入（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(30000)]
    [InlineData(0)]
    [InlineData(-5000)]
    public void Perquisite_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.Perquisite_Text = value;
        Assert.Equal(value, vm.Perquisite_Text);
    }

    /// <summary>
    /// Others_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常のその他収入（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(10000)]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Others_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.Others_Text = value;
        Assert.Equal(value, vm.Others_Text);
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
    [InlineData("テスト備考")]
    [InlineData("あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん")]
    public void Remarks_Text_値を設定後に読み取る_設定値と一致する(string value)
    {
        var vm = CreateViewModel();
        vm.Remarks_Text = value;
        Assert.Equal(value, vm.Remarks_Text);
    }

    // ==========================================
    // 初期値テスト
    // ==========================================

    /// <summary>
    /// テスト用コンストラクタ後、各数値プロパティの初期値がゼロであることを確認する
    /// </summary>
    /// <remarks>
    /// テスト用コンストラクタは Model.Clear() を呼び出すため、全数値が 0 に初期化される。
    /// </remarks>
    [Fact]
    public void 各数値プロパティ_コンストラクタ後_初期値はゼロである()
    {
        var vm = CreateViewModel();

        Assert.Equal(0, vm.SideBusiness_Text);
        Assert.Equal(0, vm.Perquisite_Text);
        Assert.Equal(0, vm.Others_Text);
    }

    /// <summary>
    /// テスト用コンストラクタ後、Remarks_Text の初期値が null であることを確認する
    /// </summary>
    /// <remarks>
    /// Model.Clear() は default(string) = null を設定する。
    /// </remarks>
    [Fact]
    public void Remarks_Text_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.Remarks_Text);
    }

    /// <summary>
    /// テスト用コンストラクタ後、Window_Background の初期値が null であることを確認する
    /// </summary>
    /// <remarks>
    /// 本番コンストラクタは Initialize() → Window_Activated() で背景色を設定するが、
    /// テスト用コンストラクタはそれらを呼び出さないため null のままとなる。
    /// </remarks>
    [Fact]
    public void Window_Background_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.Window_Background);
    }

    // ==========================================
    // 同値変更テスト
    // ==========================================

    /// <summary>
    /// SideBusiness_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void SideBusiness_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.SideBusiness_Text = 50000;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.SideBusiness_Text)) fired = true;
        };

        vm.SideBusiness_Text = 50000;

        Assert.False(fired);
    }

    /// <summary>
    /// Remarks_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void Remarks_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.Remarks_Text = "副業備考";

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Remarks_Text)) fired = true;
        };

        vm.Remarks_Text = "副業備考";

        Assert.False(fired);
    }

    // ==========================================
    // PropertyChanged 個別テスト
    // ==========================================

    /// <summary>
    /// SideBusiness_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void SideBusiness_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.SideBusiness_Text),
            () => vm.SideBusiness_Text = 50000);
    }

    /// <summary>
    /// Perquisite_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Perquisite_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Perquisite_Text),
            () => vm.Perquisite_Text = 30000);
    }

    /// <summary>
    /// Others_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Others_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Others_Text),
            () => vm.Others_Text = 10000);
    }

    /// <summary>
    /// Remarks_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Remarks_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Remarks_Text),
            () => vm.Remarks_Text = "テスト備考");
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
    /// Window_FontSize を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Window_FontSize_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Window_FontSize),
            () => vm.Window_FontSize = 12m);
    }

    // ==========================================
    // イベント通知テスト（ヘルパー）
    // ==========================================

    /// <summary>
    /// 全副業プロパティの PropertyChanged イベント発火を検証する
    /// </summary>
    private void イベント通知テスト(SideBusinessViewModel vm, SideBusinessEntity entity)
    {
        Assert.PropertyChanged(vm, nameof(vm.SideBusiness_Text),
            () => vm.SideBusiness_Text = entity.SideBusiness);

        Assert.PropertyChanged(vm, nameof(vm.Perquisite_Text),
            () => vm.Perquisite_Text = entity.Perquisite);

        Assert.PropertyChanged(vm, nameof(vm.Others_Text),
            () => vm.Others_Text = entity.Others);

        Assert.PropertyChanged(vm, nameof(vm.Remarks_Text),
            () => vm.Remarks_Text = entity.Remarks);
    }
}
