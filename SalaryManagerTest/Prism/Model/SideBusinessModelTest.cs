using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;

namespace SalaryManagerTest;

/// <summary>
/// Model - 副業 テスト
/// </summary>
public class SideBusinessModelTest
{
    #region ヘルパー

    /// <summary>モックリポジトリを生成する</summary>
    private static Mock<ISideBusinessRepository> CreateRepositoryMock() => new();

    /// <summary>テスト用 SideBusinessModel を生成する（シングルトン非使用）</summary>
    private static SideBusinessModel CreateModel(Mock<ISideBusinessRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        return new SideBusinessModel(mock.Object);
    }

    /// <summary>ViewModel を紐付けた SideBusinessModel を生成する</summary>
    private static (SideBusinessModel model, SideBusinessViewModel vm) CreateModelWithViewModel(
        Mock<ISideBusinessRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        var model = CreateModel(mock);
        var vm    = new SideBusinessViewModel(mock.Object);
        model.ViewModel = vm;
        return (model, vm);
    }

    /// <summary>テスト用 SideBusinessEntity を生成する</summary>
    private static SideBusinessEntity CreateEntity(
        double sideBusiness = 50000,
        double perquisite   = 30000,
        double others       = 10000,
        string remarks      = "テスト備考")
        => new SideBusinessEntity(
            1, DateUtils.Today,
            sideBusiness, perquisite, others, remarks);

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

        var instance1 = SideBusinessModel.GetInstance(mock.Object);
        var instance2 = SideBusinessModel.GetInstance(mock.Object);

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
        vm.SideBusiness_Text = 50000;
        vm.Perquisite_Text   = 30000;
        vm.Others_Text       = 10000;

        model.Clear();

        Assert.Equal(0, vm.SideBusiness_Text);
        Assert.Equal(0, vm.Perquisite_Text);
        Assert.Equal(0, vm.Others_Text);
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
    /// Save を呼び出したとき、ISideBusinessRepository.Save が1回呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_正常な値_RepositoryのSaveが1回呼ばれる()
    {
        var mockRepo = CreateRepositoryMock();
        var (model, _) = CreateModelWithViewModel(mockRepo);
        var mockTransaction = new Mock<ITransactionRepository>();

        model.Save(mockTransaction.Object, 1, DateOnly.FromDateTime(DateTime.Today));

        mockRepo.Verify(
            r => r.Save(mockTransaction.Object, It.IsAny<SideBusinessEntity>()),
            Times.Once);
    }

    /// <summary>
    /// Save を複数回呼び出したとき、その回数分だけ ISideBusinessRepository.Save が呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_複数回呼び出し_RepositoryのSaveがその回数だけ呼ばれる()
    {
        var mockRepo = CreateRepositoryMock();
        var (model, _) = CreateModelWithViewModel(mockRepo);
        var mockTransaction = new Mock<ITransactionRepository>();
        var yearMonth       = DateOnly.FromDateTime(DateTime.Today);

        model.Save(mockTransaction.Object, 1, yearMonth);
        model.Save(mockTransaction.Object, 2, yearMonth);
        model.Save(mockTransaction.Object, 3, yearMonth);

        mockRepo.Verify(
            r => r.Save(mockTransaction.Object, It.IsAny<SideBusinessEntity>()),
            Times.Exactly(3));
    }

    /// <summary>
    /// Save を呼び出したとき、ViewModel の値が Entity として渡されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_ViewModel値あり_RepositoryにViewModelの値が渡される()
    {
        var mockRepo = CreateRepositoryMock();
        var (model, vm) = CreateModelWithViewModel(mockRepo);
        vm.SideBusiness_Text = 80000;
        vm.Perquisite_Text   = 20000;
        vm.Others_Text       = 5000;
        vm.Remarks_Text      = "副業保存テスト";

        var mockTransaction = new Mock<ITransactionRepository>();
        model.Save(mockTransaction.Object, 1, DateOnly.FromDateTime(DateTime.Today));

        mockRepo.Verify(r => r.Save(
            mockTransaction.Object,
            It.Is<SideBusinessEntity>(e =>
                e.SideBusiness == 80000 &&
                e.Perquisite   == 20000 &&
                e.Others       == 5000  &&
                e.Remarks      == "副業保存テスト")),
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
        vm.SideBusiness_Text = 50000;
        vm.Perquisite_Text   = 30000;
        vm.Others_Text       = 10000;
        model.Entity         = null;

        model.Reload_InputForm();

        Assert.Equal(0, vm.SideBusiness_Text);
        Assert.Equal(0, vm.Perquisite_Text);
        Assert.Equal(0, vm.Others_Text);
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
            sideBusiness: 50000,
            perquisite:   30000,
            others:       10000,
            remarks:      "テスト備考");

        model.Reload_InputForm();

        Assert.Equal(50000,    vm.SideBusiness_Text);
        Assert.Equal(30000,    vm.Perquisite_Text);
        Assert.Equal(10000,    vm.Others_Text);
        Assert.Equal("テスト備考", vm.Remarks_Text);
    }

    /// <summary>
    /// 全フィールドがゼロ・空文字の Entity のとき、ViewModel に 0 と空文字が反映されることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Reload_InputForm_全フィールドゼロのEntity_ViewModelにゼロが反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        // 事前に非ゼロをセットしておく
        vm.SideBusiness_Text = 99999;
        vm.Perquisite_Text   = 99999;
        vm.Others_Text       = 99999;
        model.Entity = CreateEntity(
            sideBusiness: 0,
            perquisite:   0,
            others:       0,
            remarks:      "");

        model.Reload_InputForm();

        Assert.Equal(0,  vm.SideBusiness_Text);
        Assert.Equal(0,  vm.Perquisite_Text);
        Assert.Equal(0,  vm.Others_Text);
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
            sideBusiness: 9999999,
            perquisite:   9999999,
            others:       9999999,
            remarks:      "最大値テスト");

        model.Reload_InputForm();

        Assert.Equal(9999999, vm.SideBusiness_Text);
        Assert.Equal(9999999, vm.Perquisite_Text);
        Assert.Equal(9999999, vm.Others_Text);
        Assert.Equal("最大値テスト", vm.Remarks_Text);
    }

    /// <summary>
    /// 負の値を持つ Entity のとき、ViewModel にそのまま負値が反映されることを確認する（異常系）
    /// </summary>
    [Fact]
    public void Reload_InputForm_負値のEntity_ViewModelにそのまま反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.Entity = CreateEntity(
            sideBusiness: -5000,
            perquisite:   -2000,
            others:       -1000,
            remarks:      "マイナステスト");

        model.Reload_InputForm();

        Assert.Equal(-5000, vm.SideBusiness_Text);
        Assert.Equal(-2000, vm.Perquisite_Text);
        Assert.Equal(-1000, vm.Others_Text);
        Assert.Equal("マイナステスト", vm.Remarks_Text);
    }

    /// <summary>
    /// Entity を null → 非null → null と切り替えたとき、
    /// Clear → 値反映 → Clear の順に ViewModel が更新されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Reload_InputForm_Entity切り替え_ViewModelが正しく更新される()
    {
        var (model, vm) = CreateModelWithViewModel();

        // 1回目: Entity = null → Clear
        model.Entity = null;
        model.Reload_InputForm();
        Assert.Equal(0,    vm.SideBusiness_Text);
        Assert.Null(vm.Remarks_Text);

        // 2回目: Entity あり → 値反映
        model.Entity = CreateEntity(sideBusiness: 60000, perquisite: 0, others: 0, remarks: "切替テスト");
        model.Reload_InputForm();
        Assert.Equal(60000,   vm.SideBusiness_Text);
        Assert.Equal("切替テスト", vm.Remarks_Text);

        // 3回目: Entity = null → Clear
        model.Entity = null;
        model.Reload_InputForm();
        Assert.Equal(0, vm.SideBusiness_Text);
        Assert.Null(vm.Remarks_Text);
    }
}
