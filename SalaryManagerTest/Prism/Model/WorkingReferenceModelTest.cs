using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;

namespace SalaryManagerTest;

/// <summary>
/// Model - 勤務備考 テスト
/// </summary>
/// <remarks>
/// 対象外メソッド:
/// - Initialize: WorkingReferences.Create（静的ドメインクラス）に依存するため除外
/// - Reload: WorkingReferences.Create（静的ドメインクラス）に依存するため除外
/// - EditValidationCheck (false 分岐): Message.ShowErrorMessage（静的UIダイアログ）に依存するため除外
///   ※ PaidVacationDaysValue がコンストラクタで範囲外検証するため false 分岐は到達不能コード
/// </remarks>
public class WorkingReferenceModelTest
{
    #region ヘルパー

    /// <summary>モックリポジトリを生成する</summary>
    private static Mock<IWorkingReferencesRepository> CreateRepositoryMock() => new();

    /// <summary>テスト用 WorkingReferenceModel を生成する（シングルトン非使用）</summary>
    private static WorkingReferenceModel CreateModel(Mock<IWorkingReferencesRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        return new WorkingReferenceModel(mock.Object);
    }

    /// <summary>ViewModel を紐付けた WorkingReferenceModel を生成する</summary>
    private static (WorkingReferenceModel model, WorkingReferenceViewModel vm) CreateModelWithViewModel(
        Mock<IWorkingReferencesRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        var model = CreateModel(mock);
        var vm    = new WorkingReferenceViewModel(mock.Object);
        model.ViewModel = vm;
        return (model, vm);
    }

    /// <summary>Save テスト用に WorkPlace を設定した WorkingReferenceModel を生成する</summary>
    private static (WorkingReferenceModel model, WorkingReferenceViewModel vm, WorkPlaceViewModel workPlace)
        CreateModelWithViewModelAndWorkPlace(Mock<IWorkingReferencesRepository>? mock = null)
    {
        var (model, vm) = CreateModelWithViewModel(mock);
        var workPlace   = new WorkPlaceViewModel(true);
        model.WorkPlace = workPlace;
        return (model, vm, workPlace);
    }

    /// <summary>テスト用 WorkingReferencesEntity を生成する</summary>
    private static WorkingReferencesEntity CreateEntity(
        double overtimeTime      = 8.5,
        double weekendWorktime   = 4.0,
        double midnightWorktime  = 2.0,
        double lateAbsentH       = 1.0,
        double insurance         = 5000,
        double norm              = 220,
        double numberOfDependent = 1,
        double paidVacation      = 10,
        double workingHours      = 160,
        string workingPlace      = "テスト勤務先",
        string remarks           = "テスト備考")
        => new WorkingReferencesEntity(
            1, DateUtils.Today,
            overtimeTime, weekendWorktime, midnightWorktime, lateAbsentH,
            insurance, norm, numberOfDependent, paidVacation, workingHours,
            workingPlace, remarks);

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

        var instance1 = WorkingReferenceModel.GetInstance(mock.Object);
        var instance2 = WorkingReferenceModel.GetInstance(mock.Object);

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
        vm.OvertimeTime_Text      = 8.5;
        vm.WeekendWorktime_Text   = 4.0;
        vm.MidnightWorktime_Text  = 2.0;
        vm.LateAbsentH_Text       = 1.0;
        vm.Insurance_Text         = 5000;
        vm.Norm_Text              = 220;
        vm.NumberOfDependent_Text = 1;
        vm.PaidVacation_Text      = 10;
        vm.WorkingHours_Text      = 160;

        model.Clear();

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
    /// Save を呼び出したとき、IWorkingReferencesRepository.Save が1回呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_正常な値_RepositoryのSaveが1回呼ばれる()
    {
        var mockRepo = CreateRepositoryMock();
        var (model, _, _) = CreateModelWithViewModelAndWorkPlace(mockRepo);
        var mockTransaction = new Mock<ITransactionRepository>();

        model.Save(mockTransaction.Object, 1, DateOnly.FromDateTime(DateTime.Today));

        mockRepo.Verify(
            r => r.Save(mockTransaction.Object, It.IsAny<WorkingReferencesEntity>()),
            Times.Once);
    }

    /// <summary>
    /// Save を複数回呼び出したとき、その回数分だけ IWorkingReferencesRepository.Save が呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_複数回呼び出し_RepositoryのSaveがその回数だけ呼ばれる()
    {
        var mockRepo = CreateRepositoryMock();
        var (model, _, _) = CreateModelWithViewModelAndWorkPlace(mockRepo);
        var mockTransaction = new Mock<ITransactionRepository>();
        var yearMonth       = DateOnly.FromDateTime(DateTime.Today);

        model.Save(mockTransaction.Object, 1, yearMonth);
        model.Save(mockTransaction.Object, 2, yearMonth);
        model.Save(mockTransaction.Object, 3, yearMonth);

        mockRepo.Verify(
            r => r.Save(mockTransaction.Object, It.IsAny<WorkingReferencesEntity>()),
            Times.Exactly(3));
    }

    /// <summary>
    /// Save を呼び出したとき、ViewModel の値が Entity として渡されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_ViewModel値あり_RepositoryにViewModelの値が渡される()
    {
        var mockRepo = CreateRepositoryMock();
        var (model, vm, workPlace) = CreateModelWithViewModelAndWorkPlace(mockRepo);
        vm.OvertimeTime_Text      = 10.5;
        vm.WeekendWorktime_Text   = 8.0;
        vm.MidnightWorktime_Text  = 3.0;
        vm.LateAbsentH_Text       = 0.5;
        vm.Insurance_Text         = 12000;
        vm.Norm_Text              = 300;
        vm.NumberOfDependent_Text = 2;
        vm.PaidVacation_Text      = 15;
        vm.WorkingHours_Text      = 176;
        vm.Remarks_Text           = "保存テスト備考";
        workPlace.WorkPlace_Text  = "株式会社テスト";

        var mockTransaction = new Mock<ITransactionRepository>();
        model.Save(mockTransaction.Object, 1, DateOnly.FromDateTime(DateTime.Today));

        mockRepo.Verify(r => r.Save(
            mockTransaction.Object,
            It.Is<WorkingReferencesEntity>(e =>
                e.OvertimeTime       == 10.5  &&
                e.WeekendWorktime    == 8.0   &&
                e.MidnightWorktime   == 3.0   &&
                e.LateAbsentH        == 0.5   &&
                e.Insurance.Value    == 12000 &&
                e.Norm               == 300   &&
                e.NumberOfDependent  == 2     &&
                e.PaidVacation.Value == 15    &&
                e.WorkingHours       == 176   &&
                e.WorkPlace          == "株式会社テスト" &&
                e.Remarks            == "保存テスト備考")),
            Times.Once);
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
        vm.OvertimeTime_Text      = 8.5;
        vm.WeekendWorktime_Text   = 4.0;
        vm.MidnightWorktime_Text  = 2.0;
        vm.LateAbsentH_Text       = 1.0;
        vm.Insurance_Text         = 5000;
        vm.Norm_Text              = 220;
        vm.NumberOfDependent_Text = 1;
        vm.PaidVacation_Text      = 10;
        vm.WorkingHours_Text      = 160;
        model.Entity              = null;

        model.Reload_InputForm();

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
    /// 標準的な Entity が設定されているとき、ViewModel に全値が反映されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Reload_InputForm_Entityあり_ViewModelにEntityの全値が反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Entity = CreateEntity(
            overtimeTime:      8.5,
            weekendWorktime:   4.0,
            midnightWorktime:  2.0,
            lateAbsentH:       1.0,
            insurance:         5000,
            norm:              220,
            numberOfDependent: 1,
            paidVacation:      10,
            workingHours:      160,
            remarks:           "テスト備考");

        model.Reload_InputForm();

        Assert.Equal(8.5,      vm.OvertimeTime_Text);
        Assert.Equal(4.0,      vm.WeekendWorktime_Text);
        Assert.Equal(2.0,      vm.MidnightWorktime_Text);
        Assert.Equal(1.0,      vm.LateAbsentH_Text);
        Assert.Equal(5000,     vm.Insurance_Text);
        Assert.Equal(220,      vm.Norm_Text);
        Assert.Equal(1,        vm.NumberOfDependent_Text);
        Assert.Equal(10,       vm.PaidVacation_Text);
        Assert.Equal(160,      vm.WorkingHours_Text);
        Assert.Equal("テスト備考", vm.Remarks_Text);
    }

    /// <summary>
    /// 全フィールドがゼロ・空文字の Entity のとき、ViewModel に 0 と空文字が反映されることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Reload_InputForm_全フィールドゼロのEntity_ViewModelにゼロが反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.OvertimeTime_Text      = 99;
        vm.WeekendWorktime_Text   = 99;
        vm.MidnightWorktime_Text  = 99;
        vm.LateAbsentH_Text       = 99;
        vm.Insurance_Text         = 99;
        vm.Norm_Text              = 99;
        vm.NumberOfDependent_Text = 99;
        vm.PaidVacation_Text      = 99;
        vm.WorkingHours_Text      = 99;
        model.Entity = CreateEntity(
            overtimeTime: 0, weekendWorktime: 0, midnightWorktime: 0, lateAbsentH: 0,
            insurance: 0, norm: 0, numberOfDependent: 0, paidVacation: 0, workingHours: 0,
            workingPlace: "", remarks: "");

        model.Reload_InputForm();

        Assert.Equal(0,  vm.OvertimeTime_Text);
        Assert.Equal(0,  vm.WeekendWorktime_Text);
        Assert.Equal(0,  vm.MidnightWorktime_Text);
        Assert.Equal(0,  vm.LateAbsentH_Text);
        Assert.Equal(0,  vm.Insurance_Text);
        Assert.Equal(0,  vm.Norm_Text);
        Assert.Equal(0,  vm.NumberOfDependent_Text);
        Assert.Equal(0,  vm.PaidVacation_Text);
        Assert.Equal(0,  vm.WorkingHours_Text);
        Assert.Equal("", vm.Remarks_Text);
    }

    /// <summary>
    /// 大きな値を持つ Entity のとき、ViewModel に正しく反映されることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Reload_InputForm_大きな値のEntity_ViewModelに正しく反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Entity = CreateEntity(
            overtimeTime:      999.9,
            weekendWorktime:   999.9,
            midnightWorktime:  999.9,
            lateAbsentH:       999.9,
            insurance:         9999999,
            norm:              9999999,
            numberOfDependent: 99,
            paidVacation:      40,
            workingHours:      9999,
            remarks:           "最大値テスト");

        model.Reload_InputForm();

        Assert.Equal(999.9,    vm.OvertimeTime_Text);
        Assert.Equal(999.9,    vm.WeekendWorktime_Text);
        Assert.Equal(999.9,    vm.MidnightWorktime_Text);
        Assert.Equal(999.9,    vm.LateAbsentH_Text);
        Assert.Equal(9999999,  vm.Insurance_Text);
        Assert.Equal(9999999,  vm.Norm_Text);
        Assert.Equal(99,       vm.NumberOfDependent_Text);
        Assert.Equal(40,       vm.PaidVacation_Text);
        Assert.Equal(9999,     vm.WorkingHours_Text);
        Assert.Equal("最大値テスト", vm.Remarks_Text);
    }

    /// <summary>
    /// 負の値を持つ Entity のとき（保険・有給残日数を除く）、ViewModel にそのまま反映されることを確認する（異常系）
    /// </summary>
    /// <remarks>
    /// MoneyValue（保険）は 0 未満で例外をスローするため insurance は 0 を使用する。
    /// PaidVacationDaysValue（有給残日数）は 0 未満で例外をスローするため paidVacation は 0 を使用する。
    /// </remarks>
    [Fact]
    public void Reload_InputForm_負値のEntity_ViewModelにそのまま反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Entity = CreateEntity(
            overtimeTime:      -8.5,
            weekendWorktime:   -4.0,
            midnightWorktime:  -2.0,
            lateAbsentH:       -1.0,
            insurance:         0,
            norm:              -220,
            numberOfDependent: -1,
            paidVacation:      0,
            workingHours:      -160,
            remarks:           "マイナステスト");

        model.Reload_InputForm();

        Assert.Equal(-8.5,       vm.OvertimeTime_Text);
        Assert.Equal(-4.0,       vm.WeekendWorktime_Text);
        Assert.Equal(-2.0,       vm.MidnightWorktime_Text);
        Assert.Equal(-1.0,       vm.LateAbsentH_Text);
        Assert.Equal(0,          vm.Insurance_Text);
        Assert.Equal(-220,       vm.Norm_Text);
        Assert.Equal(-1,         vm.NumberOfDependent_Text);
        Assert.Equal(0,          vm.PaidVacation_Text);
        Assert.Equal(-160,       vm.WorkingHours_Text);
        Assert.Equal("マイナステスト", vm.Remarks_Text);
    }

    /// <summary>
    /// Entity を null → 非null → null と切り替えたとき、ViewModel が正しく更新されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Reload_InputForm_Entity切り替え_ViewModelが正しく更新される()
    {
        var (model, vm) = CreateModelWithViewModel();

        // 1回目: Entity = null → Clear
        model.Entity = null;
        model.Reload_InputForm();
        Assert.Equal(0, vm.OvertimeTime_Text);
        Assert.Null(vm.Remarks_Text);

        // 2回目: Entity あり → 値反映
        model.Entity = CreateEntity(overtimeTime: 5.0, paidVacation: 20, remarks: "切替テスト");
        model.Reload_InputForm();
        Assert.Equal(5.0,     vm.OvertimeTime_Text);
        Assert.Equal(20,      vm.PaidVacation_Text);
        Assert.Equal("切替テスト", vm.Remarks_Text);

        // 3回目: Entity = null → Clear
        model.Entity = null;
        model.Reload_InputForm();
        Assert.Equal(0, vm.OvertimeTime_Text);
        Assert.Null(vm.Remarks_Text);
    }

    // ==========================================
    // EditValidationCheck - エディットバリデーションチェック
    // ==========================================

    /// <summary>
    /// Entity が null のとき、true を返すことを確認する（境界値）
    /// </summary>
    [Fact]
    public void EditValidationCheck_Entityがnull_trueを返す()
    {
        var (model, _) = CreateModelWithViewModel();
        model.Entity = null;

        var result = model.EditValidationCheck();

        Assert.True(result);
    }

    /// <summary>
    /// 有効な有給残日数（下限値・中間値・上限値）を持つ Entity のとき、true を返すことを確認する（正常系・境界値）
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(20)]
    [InlineData(40)]
    public void EditValidationCheck_有効な有給残日数_trueを返す(double paidVacation)
    {
        var (model, _) = CreateModelWithViewModel();
        model.Entity = CreateEntity(paidVacation: paidVacation);

        var result = model.EditValidationCheck();

        Assert.True(result);
    }

    /// <summary>
    /// 有給残日数が負値のとき、PaidVacationDaysValue が例外をスローすることを確認する（異常系）
    /// </summary>
    /// <remarks>
    /// EditValidationCheck の false 分岐は到達不能。
    /// entity.PaidVacation アクセス時に PaidVacationDaysValue コンストラクタが先に例外をスローする。
    /// </remarks>
    [Fact]
    public void EditValidationCheck_有給残日数が負値_ArgumentOutOfRangeExceptionがスローされる()
    {
        var (model, _) = CreateModelWithViewModel();
        model.Entity = new WorkingReferencesEntity(
            1, DateUtils.Today,
            0, 0, 0, 0, 0, 0, 0,
            -1,
            0, "テスト", "");

        Assert.Throws<ArgumentOutOfRangeException>(() => model.EditValidationCheck());
    }

    /// <summary>
    /// 有給残日数が上限超過のとき、PaidVacationDaysValue が例外をスローすることを確認する（異常系）
    /// </summary>
    /// <remarks>
    /// EditValidationCheck の false 分岐は到達不能。
    /// entity.PaidVacation アクセス時に PaidVacationDaysValue コンストラクタが先に例外をスローする。
    /// </remarks>
    [Fact]
    public void EditValidationCheck_有給残日数が上限超過_ArgumentOutOfRangeExceptionがスローされる()
    {
        var (model, _) = CreateModelWithViewModel();
        model.Entity = new WorkingReferencesEntity(
            1, DateUtils.Today,
            0, 0, 0, 0, 0, 0, 0,
            41,
            0, "テスト", "");

        Assert.Throws<ArgumentOutOfRangeException>(() => model.EditValidationCheck());
    }
}
