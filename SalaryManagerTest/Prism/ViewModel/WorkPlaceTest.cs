using Moq;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - 勤務場所 テスト
/// </summary>
public class WorkPlaceTest
{
    #region ヘルパー

    /// <summary>テスト用ViewModelを生成する</summary>
    private static WorkPlaceViewModel CreateViewModel()
        => new WorkPlaceViewModel(forTest: true);

    #endregion

    // ==========================================
    // 初期値テスト
    // ==========================================

    /// <summary>
    /// テスト用コンストラクタ後、CompanyName_Text の初期値が null であることを確認する
    /// </summary>
    /// <remarks>
    /// Initialize() を呼ばないため、文字列プロパティはすべて null のままとなる。
    /// </remarks>
    [Fact]
    public void CompanyName_Text_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.CompanyName_Text);
    }

    /// <summary>
    /// テスト用コンストラクタ後、WorkPlace_Text の初期値が null であることを確認する
    /// </summary>
    [Fact]
    public void WorkPlace_Text_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.WorkPlace_Text);
    }

    /// <summary>
    /// テスト用コンストラクタ後、CompanyName_Foreground の初期値が null であることを確認する
    /// </summary>
    [Fact]
    public void CompanyName_Foreground_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.CompanyName_Foreground);
    }

    /// <summary>
    /// テスト用コンストラクタ後、WorkPlace_Foreground の初期値が null であることを確認する
    /// </summary>
    [Fact]
    public void WorkPlace_Foreground_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.WorkPlace_Foreground);
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

    /// <summary>
    /// テスト用コンストラクタ後、Window_FontFamily の初期値が null であることを確認する
    /// </summary>
    [Fact]
    public void Window_FontFamily_コンストラクタ後_初期値はnullである()
    {
        var vm = CreateViewModel();
        Assert.Null(vm.Window_FontFamily);
    }

    /// <summary>
    /// テスト用コンストラクタ後、Window_FontSize の初期値がゼロであることを確認する
    /// </summary>
    [Fact]
    public void Window_FontSize_コンストラクタ後_初期値はゼロである()
    {
        var vm = CreateViewModel();
        Assert.Equal(0m, vm.Window_FontSize);
    }

    // ==========================================
    // プロパティ値テスト
    // ==========================================

    /// <summary>
    /// CompanyName_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 空文字（境界値）
    /// InlineData 2: 通常の会社名（正常系）
    /// InlineData 3: 長い会社名（境界値）
    /// </remarks>
    [Theory]
    [InlineData("")]
    [InlineData("株式会社テスト")]
    [InlineData("あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん")]
    public void CompanyName_Text_値を設定後に読み取る_設定値と一致する(string value)
    {
        var vm = CreateViewModel();
        vm.CompanyName_Text = value;
        Assert.Equal(value, vm.CompanyName_Text);
    }

    /// <summary>
    /// WorkPlace_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 空文字（境界値）
    /// InlineData 2: 通常の勤務先名（正常系）
    /// InlineData 3: 長い勤務先名（境界値）
    /// </remarks>
    [Theory]
    [InlineData("")]
    [InlineData("本社ビル 4F")]
    [InlineData("あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん")]
    public void WorkPlace_Text_値を設定後に読み取る_設定値と一致する(string value)
    {
        var vm = CreateViewModel();
        vm.WorkPlace_Text = value;
        Assert.Equal(value, vm.WorkPlace_Text);
    }

    /// <summary>
    /// Window_FontSize に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: ゼロ（境界値）
    /// InlineData 2: 通常のフォントサイズ（正常系）
    /// InlineData 3: 大きなフォントサイズ（境界値）
    /// </remarks>
    [Theory]
    [InlineData(0)]
    [InlineData(12)]
    [InlineData(999)]
    public void Window_FontSize_値を設定後に読み取る_設定値と一致する(decimal value)
    {
        var vm = CreateViewModel();
        vm.Window_FontSize = value;
        Assert.Equal(value, vm.Window_FontSize);
    }

    // ==========================================
    // PropertyChanged テスト
    // ==========================================

    /// <summary>
    /// CompanyName_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void CompanyName_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.CompanyName_Text),
            () => vm.CompanyName_Text = "株式会社テスト");
    }

    /// <summary>
    /// WorkPlace_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void WorkPlace_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.WorkPlace_Text),
            () => vm.WorkPlace_Text = "本社 4F");
    }

    /// <summary>
    /// CompanyName_Foreground を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void CompanyName_Foreground_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.CompanyName_Foreground),
            () => vm.CompanyName_Foreground = new SolidColorBrush(Colors.Black));
    }

    /// <summary>
    /// WorkPlace_Foreground を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void WorkPlace_Foreground_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.WorkPlace_Foreground),
            () => vm.WorkPlace_Foreground = new SolidColorBrush(Colors.Gray));
    }

    /// <summary>
    /// Window_Background を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Window_Background_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Window_Background),
            () => vm.Window_Background = new SolidColorBrush(Colors.White));
    }

    /// <summary>
    /// Window_FontFamily を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Window_FontFamily_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Window_FontFamily),
            () => vm.Window_FontFamily = new FontFamily("Meiryo"));
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
    // 同値変更テスト
    // ==========================================

    /// <summary>
    /// CompanyName_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void CompanyName_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.CompanyName_Text = "株式会社テスト";

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.CompanyName_Text)) fired = true;
        };

        vm.CompanyName_Text = "株式会社テスト";

        Assert.False(fired);
    }

    /// <summary>
    /// WorkPlace_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void WorkPlace_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.WorkPlace_Text = "本社 4F";

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.WorkPlace_Text)) fired = true;
        };

        vm.WorkPlace_Text = "本社 4F";

        Assert.False(fired);
    }

    // ==========================================
    // WorkPlaceModel テスト
    // ==========================================

    /// <summary>
    /// GetInstance を複数回呼び出したとき、同じインスタンスが返ることを確認する
    /// </summary>
    [Fact]
    public void GetInstance_複数回呼び出し_同じインスタンスを返す()
    {
        var instance1 = WorkPlaceModel.GetInstance();
        var instance2 = WorkPlaceModel.GetInstance();

        Assert.Same(instance1, instance2);
    }

    /// <summary>
    /// Save を呼び出したとき NotImplementedException が投げられることを確認する
    /// </summary>
    /// <remarks>
    /// 保存先が勤怠備考テーブルであるため、WorkPlaceModel.Save は実装されていない。
    /// </remarks>
    [Fact]
    public void Save_呼び出し時_NotImplementedExceptionを投げる()
    {
        var model = WorkPlaceModel.GetInstance();
        var mockTransaction = new Mock<ITransactionRepository>();

        Assert.Throws<NotImplementedException>(() =>
            model.Save(mockTransaction.Object, 1, DateOnly.FromDateTime(DateTime.Now)));
    }
}
