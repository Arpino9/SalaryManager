using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - 勤務参考 テスト
/// </summary>
public class WorkingReferenceTest
{
    #region ヘルパー

    /// <summary>テスト用ViewModelを生成する</summary>
    private static WorkingReferenceViewModel CreateViewModel()
    {
        var mock = new Mock<IWorkingReferencesRepository>();
        return new WorkingReferenceViewModel(mock.Object);
    }

    /// <summary>テスト用Entityを生成する</summary>
    private static WorkingReferencesEntity CreateEntity(
        double overtimeTime,
        double weekendWorktime,
        double midnightWorktime,
        double lateAbsentH,
        double insurance,
        double norm,
        double numberOfDependent,
        double paidVacation,
        double workingHours,
        string remarks)
        => new WorkingReferencesEntity(
            1,
            DateOnly.FromDateTime(DateTime.Now),
            overtimeTime,
            weekendWorktime,
            midnightWorktime,
            lateAbsentH,
            insurance,
            norm,
            numberOfDependent,
            paidVacation,
            workingHours,
            "本社",
            remarks);

    #endregion

    // ==========================================
    // 初期化 - PropertyChanged 通知テスト
    // ==========================================

    /// <summary>
    /// 各プロパティに値を設定したとき PropertyChanged が発火することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の勤務データ（正常系）
    /// InlineData 2: 全ゼロ・空文字（境界値）
    /// InlineData 3: 大きな値・最大有給（境界値）
    /// </remarks>
    [Theory]
    [InlineData(8.5,  4.0,  2.0,  1.0,  150000, 280,  1,  10, 160, "テスト備考")]
    [InlineData(0,    0,    0,    0,    0,      0,    0,  0,  0,   "")]
    [InlineData(200,  100,  80,   24,   9999999, 9999, 99, 40, 300, "最大値テスト")]
    public void 初期化(
        double overtimeTime,
        double weekendWorktime,
        double midnightWorktime,
        double lateAbsentH,
        double insurance,
        double norm,
        double numberOfDependent,
        double paidVacation,
        double workingHours,
        string remarks)
    {
        var entity = CreateEntity(
            overtimeTime, weekendWorktime, midnightWorktime, lateAbsentH,
            insurance, norm, numberOfDependent, paidVacation, workingHours, remarks);

        var mock = new Mock<IWorkingReferencesRepository>();
        var vm = new WorkingReferenceViewModel(mock.Object);

        // -1 にリセット（entity 値と重複しないようにする）
        vm.OvertimeTime_Text      = -1;
        vm.WeekendWorktime_Text   = -1;
        vm.MidnightWorktime_Text  = -1;
        vm.LateAbsentH_Text       = -1;
        vm.Insurance_Text         = -1;
        vm.Norm_Text              = -1;
        vm.NumberOfDependent_Text = -1;
        vm.PaidVacation_Text      = -1;
        vm.WorkingHours_Text      = -1;
        // Remarks_Text は Clear() により null 設定済みのため再設定不要
        // （"" にリセットすると entity.Remarks = "" のとき SetProperty が変化なしと判定し PropertyChanged が発火しない）

        イベント通知テスト(vm, entity);
    }

    // ==========================================
    // プロパティ値テスト
    // ==========================================

    /// <summary>
    /// OvertimeTime_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(8.5)]
    [InlineData(0)]
    [InlineData(-1.0)]
    public void OvertimeTime_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.OvertimeTime_Text = value;
        Assert.Equal(value, vm.OvertimeTime_Text);
    }

    /// <summary>
    /// WeekendWorktime_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(4.0)]
    [InlineData(0)]
    [InlineData(-1.0)]
    public void WeekendWorktime_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.WeekendWorktime_Text = value;
        Assert.Equal(value, vm.WeekendWorktime_Text);
    }

    /// <summary>
    /// MidnightWorktime_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(2.0)]
    [InlineData(0)]
    [InlineData(-1.0)]
    public void MidnightWorktime_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.MidnightWorktime_Text = value;
        Assert.Equal(value, vm.MidnightWorktime_Text);
    }

    /// <summary>
    /// LateAbsentH_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(1.0)]
    [InlineData(0)]
    [InlineData(-1.0)]
    public void LateAbsentH_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.LateAbsentH_Text = value;
        Assert.Equal(value, vm.LateAbsentH_Text);
    }

    /// <summary>
    /// Insurance_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の保険額（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// </remarks>
    [Theory]
    [InlineData(150000)]
    [InlineData(0)]
    public void Insurance_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.Insurance_Text = value;
        Assert.Equal(value, vm.Insurance_Text);
    }

    /// <summary>
    /// Norm_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の標準月額（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// </remarks>
    [Theory]
    [InlineData(280)]
    [InlineData(0)]
    public void Norm_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.Norm_Text = value;
        Assert.Equal(value, vm.Norm_Text);
    }

    /// <summary>
    /// NumberOfDependent_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 扶養1人（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 多人数（正常系）
    /// </remarks>
    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(99)]
    public void NumberOfDependent_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.NumberOfDependent_Text = value;
        Assert.Equal(value, vm.NumberOfDependent_Text);
    }

    /// <summary>
    /// PaidVacation_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常値（正常系）
    /// InlineData 2: 下限値 0（境界値）
    /// InlineData 3: 上限値 40（境界値 / PaidVacationDaysValue.Maximum）
    /// </remarks>
    [Theory]
    [InlineData(10)]
    [InlineData(0)]
    [InlineData(40)]
    public void PaidVacation_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.PaidVacation_Text = value;
        Assert.Equal(value, vm.PaidVacation_Text);
    }

    /// <summary>
    /// WorkingHours_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の月間勤務時間（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(160)]
    [InlineData(0)]
    [InlineData(-1)]
    public void WorkingHours_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();
        vm.WorkingHours_Text = value;
        Assert.Equal(value, vm.WorkingHours_Text);
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

        Assert.Equal(0, vm.OvertimeTime_Text);
        Assert.Equal(0, vm.WeekendWorktime_Text);
        Assert.Equal(0, vm.MidnightWorktime_Text);
        Assert.Equal(0, vm.LateAbsentH_Text);
        Assert.Equal(0, vm.Insurance_Text);
        Assert.Equal(0, vm.Norm_Text);
        Assert.Equal(0, vm.NumberOfDependent_Text);
        Assert.Equal(0, vm.PaidVacation_Text);
        Assert.Equal(0, vm.WorkingHours_Text);
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
    /// OvertimeTime_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void OvertimeTime_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.OvertimeTime_Text = 8.5;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.OvertimeTime_Text)) fired = true;
        };

        vm.OvertimeTime_Text = 8.5;

        Assert.False(fired);
    }

    /// <summary>
    /// Remarks_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void Remarks_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.Remarks_Text = "備考";

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Remarks_Text)) fired = true;
        };

        vm.Remarks_Text = "備考";

        Assert.False(fired);
    }

    // ==========================================
    // PropertyChanged 個別テスト
    // ==========================================

    /// <summary>
    /// OvertimeTime_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void OvertimeTime_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.OvertimeTime_Text),
            () => vm.OvertimeTime_Text = 8.5);
    }

    /// <summary>
    /// WeekendWorktime_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void WeekendWorktime_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.WeekendWorktime_Text),
            () => vm.WeekendWorktime_Text = 4.0);
    }

    /// <summary>
    /// MidnightWorktime_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void MidnightWorktime_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.MidnightWorktime_Text),
            () => vm.MidnightWorktime_Text = 2.0);
    }

    /// <summary>
    /// LateAbsentH_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void LateAbsentH_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.LateAbsentH_Text),
            () => vm.LateAbsentH_Text = 1.0);
    }

    /// <summary>
    /// Insurance_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Insurance_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Insurance_Text),
            () => vm.Insurance_Text = 150000);
    }

    /// <summary>
    /// Norm_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Norm_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.Norm_Text),
            () => vm.Norm_Text = 280);
    }

    /// <summary>
    /// NumberOfDependent_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void NumberOfDependent_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.NumberOfDependent_Text),
            () => vm.NumberOfDependent_Text = 1);
    }

    /// <summary>
    /// PaidVacation_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void PaidVacation_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.PaidVacation_Text),
            () => vm.PaidVacation_Text = 10);
    }

    /// <summary>
    /// WorkingHours_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void WorkingHours_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();
        Assert.PropertyChanged(vm, nameof(vm.WorkingHours_Text),
            () => vm.WorkingHours_Text = 160);
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

    // ==========================================
    // イベント通知テスト（ヘルパー）
    // ==========================================

    /// <summary>
    /// 全勤務参考プロパティの PropertyChanged イベント発火を検証する
    /// </summary>
    private void イベント通知テスト(WorkingReferenceViewModel vm, WorkingReferencesEntity entity)
    {
        Assert.PropertyChanged(vm, nameof(vm.OvertimeTime_Text),
            () => vm.OvertimeTime_Text = entity.OvertimeTime);

        Assert.PropertyChanged(vm, nameof(vm.WeekendWorktime_Text),
            () => vm.WeekendWorktime_Text = entity.WeekendWorktime);

        Assert.PropertyChanged(vm, nameof(vm.MidnightWorktime_Text),
            () => vm.MidnightWorktime_Text = entity.MidnightWorktime);

        Assert.PropertyChanged(vm, nameof(vm.LateAbsentH_Text),
            () => vm.LateAbsentH_Text = entity.LateAbsentH);

        Assert.PropertyChanged(vm, nameof(vm.Insurance_Text),
            () => vm.Insurance_Text = entity.Insurance.Value);

        Assert.PropertyChanged(vm, nameof(vm.Norm_Text),
            () => vm.Norm_Text = entity.Norm);

        Assert.PropertyChanged(vm, nameof(vm.NumberOfDependent_Text),
            () => vm.NumberOfDependent_Text = entity.NumberOfDependent);

        Assert.PropertyChanged(vm, nameof(vm.PaidVacation_Text),
            () => vm.PaidVacation_Text = entity.PaidVacation.Value);

        Assert.PropertyChanged(vm, nameof(vm.WorkingHours_Text),
            () => vm.WorkingHours_Text = entity.WorkingHours);

        Assert.PropertyChanged(vm, nameof(vm.Remarks_Text),
            () => vm.Remarks_Text = entity.Remarks);
    }
}
