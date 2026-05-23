using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;

namespace SalaryManagerTest;

/// <summary>
/// Model - ヘッダー テスト
/// </summary>
/// <remarks>
/// 対象外メソッド:
/// - SetDefaultPayslip: Message.ShowConfirmingMessage（静的UIダイアログ）に依存するため除外
/// </remarks>
public class HeaderModelTest
{
    #region ヘルパー

    /// <summary>
    /// テスト用リポジトリモックを生成する
    /// </summary>
    /// <param name="entities">GetEntities() の戻り値（省略時は空リスト）</param>
    private static Mock<IHeaderRepository> CreateRepositoryMock(
        IReadOnlyList<HeaderEntity>? entities = null)
    {
        var mock = new Mock<IHeaderRepository>();
        mock.Setup(r => r.GetEntities())
            .Returns((entities ?? new List<HeaderEntity>()).ToList());
        mock.Setup(r => r.FetchDefault())
            .Returns((HeaderEntity?)null);
        return mock;
    }

    /// <summary>テスト用 HeaderModel を生成する（シングルトン非使用）</summary>
    private static HeaderModel CreateModel(Mock<IHeaderRepository> mock)
        => new HeaderModel(mock.Object);

    /// <summary>ViewModel を紐付けた HeaderModel を生成する</summary>
    private static (HeaderModel model, HeaderViewModel vm) CreateModelWithViewModel(
        Mock<IHeaderRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        var model = CreateModel(mock);
        var vm = new HeaderViewModel(mock.Object);
        model.ViewModel = vm;
        return (model, vm);
    }

    /// <summary>
    /// Return/Proceed テスト用に、Reload() 内で entities.Any() = true かつ
    /// 年月不一致（entity is null）となるダミーエンティティ付きリポジトリを生成する
    /// </summary>
    /// <remarks>
    /// entities.Any() = true により Clear() ではなく ID 更新パスが通り、
    /// Reload() 末尾で YearMonth が ViewModel の年月に更新される。
    /// </remarks>
    private static Mock<IHeaderRepository> CreateRepositoryMockWithDummyEntity()
    {
        var dummy = new HeaderEntity(10, new DateOnly(2000, 1, 1), false, DateTime.Today, DateTime.Today);
        return CreateRepositoryMock(new List<HeaderEntity> { dummy });
    }

    /// <summary>テスト用 HeaderEntity を生成する</summary>
    private static HeaderEntity CreateEntity(
        int id                 = 1,
        int year               = 2026,
        int month              = 5,
        bool isDefault         = false,
        DateTime createDate    = default,
        DateTime updateDate    = default)
        => new HeaderEntity(
            id,
            new DateOnly(year, month, 1),
            isDefault,
            createDate == default ? DateTime.Today : createDate,
            updateDate == default ? DateTime.Today : updateDate);

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

        var instance1 = HeaderModel.GetInstance(mock.Object);
        var instance2 = HeaderModel.GetInstance(mock.Object);

        Assert.Same(instance1, instance2);
    }

    // ==========================================
    // Clear - 各項目を初期値にリセット
    // ==========================================

    /// <summary>
    /// Clear 後、ViewModel.Year_Text が YearMonth の年と一致することを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_ViewModelYearTextがYearMonthの年になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.YearMonth = new DateOnly(2023, 8, 1);
        vm.Year_Text    = 9999; // 別の値を設定しておく

        model.Clear();

        Assert.Equal(2023, vm.Year_Text);
    }

    /// <summary>
    /// Clear 後、ViewModel.Month_Text が YearMonth の月と一致することを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_ViewModelMonthTextがYearMonthの月になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        model.YearMonth = new DateOnly(2023, 8, 1);
        vm.Month_Text   = 1; // 別の値を設定しておく

        model.Clear();

        Assert.Equal(8, vm.Month_Text);
    }

    /// <summary>
    /// Clear 後、CreateDate が今日の日付になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_CreateDateが今日になる()
    {
        var (model, _) = CreateModelWithViewModel();
        model.CreateDate = new DateTime(2000, 1, 1);

        model.Clear();

        Assert.Equal(DateTime.Today, model.CreateDate);
    }

    /// <summary>
    /// Clear 後、UpdateDate が今日の日付になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_実行後_UpdateDateが今日になる()
    {
        var (model, _) = CreateModelWithViewModel();
        model.UpdateDate = new DateTime(2000, 1, 1);

        model.Clear();

        Assert.Equal(DateTime.Today, model.UpdateDate);
    }

    // ==========================================
    // SaveDefaultPayslip - トランザクションなし保存
    // ==========================================

    /// <summary>
    /// SaveDefaultPayslip を呼び出したとき、IHeaderRepository.Save(entity) が1回呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void SaveDefaultPayslip_正常な値_RepositoryのSaveが1回呼ばれる()
    {
        var mock = CreateRepositoryMock();
        var (model, _) = CreateModelWithViewModel(mock);

        model.SaveDefaultPayslip();

        mock.Verify(r => r.Save(It.IsAny<HeaderEntity>()), Times.Once);
    }

    // ==========================================
    // Save(transaction) - トランザクションあり保存
    // ==========================================

    /// <summary>
    /// Save を呼び出したとき、IHeaderRepository.Save(transaction, entity) が1回呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_正常な値_RepositoryのSave_transactionありが1回呼ばれる()
    {
        var mock = CreateRepositoryMock();
        var (model, _) = CreateModelWithViewModel(mock);
        var mockTransaction = new Mock<ITransactionRepository>();

        model.Save(mockTransaction.Object);

        mock.Verify(r => r.Save(mockTransaction.Object, It.IsAny<HeaderEntity>()), Times.Once);
    }

    /// <summary>
    /// Save を複数回呼び出したとき、その回数分だけ Save が呼ばれることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Save_複数回呼び出し_RepositoryのSaveがその回数だけ呼ばれる()
    {
        var mock = CreateRepositoryMock();
        var (model, _) = CreateModelWithViewModel(mock);
        var mockTransaction = new Mock<ITransactionRepository>();

        model.Save(mockTransaction.Object);
        model.Save(mockTransaction.Object);
        model.Save(mockTransaction.Object);

        mock.Verify(r => r.Save(mockTransaction.Object, It.IsAny<HeaderEntity>()), Times.Exactly(3));
    }

    // ==========================================
    // Return - 1ヶ月戻る
    // ==========================================

    /// <summary>
    /// 5月から Return を呼び出したとき、ViewModel の年月が4月になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Return_5月から呼び出し_ViewModel年月が4月になる()
    {
        var mock = CreateRepositoryMockWithDummyEntity();
        var (model, vm) = CreateModelWithViewModel(mock);
        model.YearMonth  = new DateOnly(2026, 5, 1);
        vm.Year_Text     = 2026;
        vm.Month_Text    = 5;

        model.Return();

        Assert.Equal(2026, vm.Year_Text);
        Assert.Equal(4, vm.Month_Text);
        Assert.Equal(new DateOnly(2026, 4, 1), model.YearMonth);
    }

    /// <summary>
    /// 1月から Return を呼び出したとき、ViewModel の年月が前年12月になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Return_1月から呼び出し_ViewModel年月が前年12月になる()
    {
        var mock = CreateRepositoryMockWithDummyEntity();
        var (model, vm) = CreateModelWithViewModel(mock);
        model.YearMonth  = new DateOnly(2026, 1, 1);
        vm.Year_Text     = 2026;
        vm.Month_Text    = 1;

        model.Return();

        Assert.Equal(2025, vm.Year_Text);
        Assert.Equal(12, vm.Month_Text);
        Assert.Equal(new DateOnly(2025, 12, 1), model.YearMonth);
    }

    // ==========================================
    // Proceed - 1ヶ月進む
    // ==========================================

    /// <summary>
    /// 5月から Proceed を呼び出したとき、ViewModel の年月が6月になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Proceed_5月から呼び出し_ViewModel年月が6月になる()
    {
        var mock = CreateRepositoryMockWithDummyEntity();
        var (model, vm) = CreateModelWithViewModel(mock);
        model.YearMonth  = new DateOnly(2026, 5, 1);
        vm.Year_Text     = 2026;
        vm.Month_Text    = 5;

        model.Proceed();

        Assert.Equal(2026, vm.Year_Text);
        Assert.Equal(6, vm.Month_Text);
        Assert.Equal(new DateOnly(2026, 6, 1), model.YearMonth);
    }

    /// <summary>
    /// 12月から Proceed を呼び出したとき、ViewModel の年月が翌年1月になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Proceed_12月から呼び出し_ViewModel年月が翌年1月になる()
    {
        var mock = CreateRepositoryMockWithDummyEntity();
        var (model, vm) = CreateModelWithViewModel(mock);
        model.YearMonth  = new DateOnly(2025, 12, 1);
        vm.Year_Text     = 2025;
        vm.Month_Text    = 12;

        model.Proceed();

        Assert.Equal(2026, vm.Year_Text);
        Assert.Equal(1, vm.Month_Text);
        Assert.Equal(new DateOnly(2026, 1, 1), model.YearMonth);
    }

    // ==========================================
    // IsValid_Year - 年の桁数検証
    // ==========================================

    /// <summary>
    /// 4桁の年を設定したとき、例外なく処理され Year_Text が変わらないことを確認する（正常系）
    /// </summary>
    [Theory]
    [InlineData(2026)]
    [InlineData(1000)]
    [InlineData(9999)]
    public void IsValid_Year_4桁の年_例外なく処理されYear_Textが変わらない(int year)
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Year_Text = year;

        var ex = Record.Exception(() => model.IsValid_Year());

        Assert.Null(ex);
        Assert.Equal(year, vm.Year_Text);
    }

    /// <summary>
    /// 3桁の年を設定したとき、早期リターンして Year_Text が変わらないことを確認する（異常系）
    /// </summary>
    [Theory]
    [InlineData(999)]
    [InlineData(100)]
    public void IsValid_Year_3桁以下の年_早期リターンしYear_Textが変わらない(int year)
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Year_Text = year;

        model.IsValid_Year();

        Assert.Equal(year, vm.Year_Text);
    }

    /// <summary>
    /// 5桁以上の年を設定したとき、早期リターンして Year_Text が変わらないことを確認する（異常系）
    /// </summary>
    [Theory]
    [InlineData(10000)]
    [InlineData(99999)]
    public void IsValid_Year_5桁以上の年_早期リターンしYear_Textが変わらない(int year)
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Year_Text = year;

        model.IsValid_Year();

        Assert.Equal(year, vm.Year_Text);
    }

    /// <summary>
    /// ゼロを設定したとき、早期リターンして Year_Text が変わらないことを確認する（境界値）
    /// </summary>
    [Fact]
    public void IsValid_Year_ゼロ_早期リターンしYear_Textが変わらない()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Year_Text = 0;

        model.IsValid_Year();

        Assert.Equal(0, vm.Year_Text);
    }

    // ==========================================
    // IsValid_Month - 月の範囲補正
    // ==========================================

    /// <summary>
    /// 月が 1 のとき、そのまま 1 になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void IsValid_Month_月が1_そのまま1になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Month_Text = 1;

        model.IsValid_Month();

        Assert.Equal(1, vm.Month_Text);
    }

    /// <summary>
    /// 月が 12 のとき、そのまま 12 になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void IsValid_Month_月が12_そのまま12になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Month_Text = 12;

        model.IsValid_Month();

        Assert.Equal(12, vm.Month_Text);
    }

    /// <summary>
    /// 月が 1〜12 の範囲内のとき、値がそのまま保持されることを確認する（正常系）
    /// </summary>
    [Theory]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(9)]
    public void IsValid_Month_有効範囲内の月_値がそのまま保持される(int month)
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Month_Text = month;

        model.IsValid_Month();

        Assert.Equal(month, vm.Month_Text);
    }

    /// <summary>
    /// 月が 0 のとき、1 に補正されることを確認する（境界値）
    /// </summary>
    [Fact]
    public void IsValid_Month_月が0_1に補正される()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Month_Text = 0;

        model.IsValid_Month();

        Assert.Equal(1, vm.Month_Text);
    }

    /// <summary>
    /// 月が負値のとき、1 に補正されることを確認する（異常系）
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void IsValid_Month_月が負値_1に補正される(int month)
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Month_Text = month;

        model.IsValid_Month();

        Assert.Equal(1, vm.Month_Text);
    }

    /// <summary>
    /// 月が 13 のとき、12 に補正されることを確認する（境界値）
    /// </summary>
    [Fact]
    public void IsValid_Month_月が13_12に補正される()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Month_Text = 13;

        model.IsValid_Month();

        Assert.Equal(12, vm.Month_Text);
    }

    /// <summary>
    /// 月が大きな値のとき、12 に補正されることを確認する（異常系）
    /// </summary>
    [Theory]
    [InlineData(100)]
    [InlineData(9999)]
    public void IsValid_Month_月が大きな値_12に補正される(int month)
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Month_Text = month;

        model.IsValid_Month();

        Assert.Equal(12, vm.Month_Text);
    }
}
