using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;

namespace SalaryManagerTest;

/// <summary>
/// Model - 勤務場所 テスト
/// </summary>
/// <remarks>
/// 対象外メソッド:
/// - Initialize       : WorkingReferences.Create（静的ドメインクラス）に依存するため除外
/// - Reload（本体）   : WorkingReferences.Create / WorkingPlace.Create に依存するため除外
/// - Reload_InputForm : WorkingPlace.Create（静的ドメインクラス）に依存するため除外
/// - Clear            : WorkingPlace.FetchByDate（静的ドメインクラス）に依存するため除外
/// - GetWorkingPlace  : private メソッドのため除外
/// </remarks>
public class WorkPlaceModelTest
{
    #region ヘルパー

    /// <summary>テスト用 WorkPlaceModel を生成する（シングルトン非使用）</summary>
    private static WorkPlaceModel CreateModel() => new WorkPlaceModel();

    /// <summary>テスト用 WorkingReferencesEntity を生成する</summary>
    private static WorkingReferencesEntity CreateEntity(
        double paidVacation  = 10,
        string workingPlace  = "テスト勤務先",
        string remarks       = "テスト備考")
        => new WorkingReferencesEntity(
            1, DateUtils.Today,
            0, 0, 0, 0, 0, 0, 0,
            paidVacation, 0,
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
        var instance1 = WorkPlaceModel.GetInstance();
        var instance2 = WorkPlaceModel.GetInstance();

        Assert.Same(instance1, instance2);
    }

    // ==========================================
    // Reload - ViewModel が null のとき早期リターン
    // ==========================================

    /// <summary>
    /// ViewModel が null のとき、Reload が例外なく早期リターンすることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Reload_ViewModelがnull_例外なく早期リターンする()
    {
        var model = CreateModel();
        // ViewModel はデフォルトで null

        var ex = Record.Exception(() => model.Reload());

        Assert.Null(ex);
    }

    /// <summary>
    /// ViewModel が null のとき、Reload 後も Entity が変化しないことを確認する（正常系）
    /// </summary>
    [Fact]
    public void Reload_ViewModelがnull_Entityが変化しない()
    {
        var model = CreateModel();
        var entity = CreateEntity();
        model.Entity = entity;

        model.Reload();

        Assert.Same(entity, model.Entity);
    }

    /// <summary>
    /// ViewModel が null のとき、Reload 後も Entity_LastYear が変化しないことを確認する（正常系）
    /// </summary>
    [Fact]
    public void Reload_ViewModelがnull_Entity_LastYearが変化しない()
    {
        var model = CreateModel();
        var lastYear = CreateEntity(remarks: "昨年度テスト");
        model.Entity_LastYear = lastYear;

        model.Reload();

        Assert.Same(lastYear, model.Entity_LastYear);
    }

    /// <summary>
    /// ViewModel が null かつ Entity が null のとき、Reload 後も Entity が null のままであることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Reload_ViewModelがnull_Entityがnull_Entityはnullのまま()
    {
        var model = CreateModel();
        model.Entity = null;

        model.Reload();

        Assert.Null(model.Entity);
    }

    // ==========================================
    // Entity / Entity_LastYear - プロパティ
    // ==========================================

    /// <summary>
    /// Entity に値を設定後、読み取り値が一致することを確認する（正常系）
    /// </summary>
    [Fact]
    public void Entity_値を設定後に読み取る_設定値と一致する()
    {
        var model  = CreateModel();
        var entity = CreateEntity();

        model.Entity = entity;

        Assert.Same(entity, model.Entity);
    }

    /// <summary>
    /// Entity に null を設定後、null になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Entity_nullを設定後に読み取る_nullになる()
    {
        var model = CreateModel();
        model.Entity = CreateEntity();

        model.Entity = null;

        Assert.Null(model.Entity);
    }

    /// <summary>
    /// Entity_LastYear に値を設定後、読み取り値が一致することを確認する（正常系）
    /// </summary>
    [Fact]
    public void Entity_LastYear_値を設定後に読み取る_設定値と一致する()
    {
        var model  = CreateModel();
        var entity = CreateEntity(remarks: "昨年度");

        model.Entity_LastYear = entity;

        Assert.Same(entity, model.Entity_LastYear);
    }

    /// <summary>
    /// Entity_LastYear に null を設定後、null になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void Entity_LastYear_nullを設定後に読み取る_nullになる()
    {
        var model = CreateModel();
        model.Entity_LastYear = CreateEntity();

        model.Entity_LastYear = null;

        Assert.Null(model.Entity_LastYear);
    }

    /// <summary>
    /// Entity と Entity_LastYear は互いに独立していることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Entity_と_Entity_LastYear_互いに独立している()
    {
        var model       = CreateModel();
        var current     = CreateEntity(remarks: "今月");
        var lastYear    = CreateEntity(remarks: "昨年度");

        model.Entity          = current;
        model.Entity_LastYear = lastYear;

        Assert.Same(current,  model.Entity);
        Assert.Same(lastYear, model.Entity_LastYear);
        Assert.NotSame(model.Entity, model.Entity_LastYear);
    }

    // ==========================================
    // Save - 未実装メソッド
    // ==========================================

    /// <summary>
    /// Save を呼び出したとき NotImplementedException がスローされることを確認する（正常系）
    /// </summary>
    /// <remarks>
    /// 保存先が勤怠備考テーブルであるため WorkPlaceModel.Save は実装されていない。
    /// </remarks>
    [Fact]
    public void Save_呼び出し時_NotImplementedExceptionをスローする()
    {
        var model           = CreateModel();
        var mockTransaction = new Mock<ITransactionRepository>();

        Assert.Throws<NotImplementedException>(() =>
            model.Save(mockTransaction.Object, 1, DateOnly.FromDateTime(DateTime.Today)));
    }

    /// <summary>
    /// Save を異なる引数で呼び出しても、常に NotImplementedException がスローされることを確認する（正常系）
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void Save_異なるId_常にNotImplementedExceptionをスローする(int id)
    {
        var model           = CreateModel();
        var mockTransaction = new Mock<ITransactionRepository>();

        Assert.Throws<NotImplementedException>(() =>
            model.Save(mockTransaction.Object, id, DateOnly.FromDateTime(DateTime.Today)));
    }
}
