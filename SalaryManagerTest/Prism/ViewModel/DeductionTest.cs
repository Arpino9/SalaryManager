using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - 控除額 テスト
/// </summary>
public class DeductionTest
{
    #region ヘルパー

    /// <summary>テスト用ViewModelを生成する</summary>
    private static DeductionViewModel CreateViewModel()
    {
        var mock = new Mock<IDeductionRepository>();
        return new DeductionViewModel(mock.Object);
    }

    /// <summary>テスト用Entityを生成する</summary>
    private static DeductionEntity CreateEntity(
        double healthInsurance,
        double nursingInsurance,
        double welfareAnnuity,
        double employmentInsurance,
        double incomeTax,
        double municipalTax,
        double friendshipAssociation,
        double yearEndTaxAdjustment,
        string remarks,
        double totalDeduct)
        => new DeductionEntity(
            1,
            DateUtils.Today,
            healthInsurance,
            nursingInsurance,
            welfareAnnuity,
            employmentInsurance,
            incomeTax,
            municipalTax,
            friendshipAssociation,
            yearEndTaxAdjustment,
            remarks,
            totalDeduct);

    #endregion

    // ==========================================
    // 初期化 - PropertyChanged 通知テスト
    // ==========================================

    /// <summary>
    /// 各プロパティに値を設定したとき PropertyChanged が発火することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 標準的な控除体系（正常系）
    /// InlineData 2: 年末調整がマイナス（逆調整ケース）
    /// InlineData 3: 大きな値（境界値）
    /// </remarks>
    [Theory]
    [InlineData(10000, 5000, 20000, 1000, 15000, 30000, 2000,   500, "",         83500)]
    [InlineData(25000, 3000, 35000, 1500, 20000, 40000, 3000, -5000, "備考テスト",  122500)]
    [InlineData(9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, 9999999, "最大値テスト", 9999999)]
    public void 初期化(
        double healthInsurance,
        double nursingInsurance,
        double welfareAnnuity,
        double employmentInsurance,
        double incomeTax,
        double municipalTax,
        double friendshipAssociation,
        double yearEndTaxAdjustment,
        string remarks,
        double totalDeduct)
    {
        var entity = CreateEntity(
            healthInsurance, nursingInsurance, welfareAnnuity,
            employmentInsurance, incomeTax, municipalTax,
            friendshipAssociation, yearEndTaxAdjustment,
            remarks, totalDeduct);

        var mock = new Mock<IDeductionRepository>();
        mock.Setup(x => x.GetEntity(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(entity);

        var vm = new DeductionViewModel(mock.Object);

        // Model.Clear() により各プロパティは default(double) = 0 に設定済み
        // entity の値と同値になる場合は事前にリセットして PropertyChanged が発火できる状態にする
        vm.HealthInsurance_Text       = 0;
        vm.NursingInsurance_Text      = 0;
        vm.WelfareAnnuity_Text        = 0;
        vm.EmploymentInsurance_Text   = 0;
        vm.IncomeTax_Text             = 0;
        vm.MunicipalTax_Text          = 0;
        vm.FriendshipAssociation_Text = 0;
        vm.YearEndTaxAdjustment_Text  = 0;
        vm.TotalDeduct_Text           = 0;
        // Remarks_Text は Model.Clear() により null に設定済みのため再設定不要

        イベント通知テスト(vm, entity);
    }

    // ==========================================
    // プロパティ値テスト
    // ==========================================

    /// <summary>
    /// HealthInsurance_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData(10000)]
    [InlineData(0)]
    [InlineData(-5000)]
    public void HealthInsurance_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.HealthInsurance_Text = value;

        Assert.Equal(value, vm.HealthInsurance_Text);
    }

    /// <summary>
    /// NursingInsurance_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系）
    /// </remarks>
    [Theory]
    [InlineData(3000)]
    [InlineData(0)]
    [InlineData(-1000)]
    public void NursingInsurance_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.NursingInsurance_Text = value;

        Assert.Equal(value, vm.NursingInsurance_Text);
    }

    /// <summary>
    /// WelfareAnnuity_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系）
    /// </remarks>
    [Theory]
    [InlineData(20000)]
    [InlineData(0)]
    [InlineData(-10000)]
    public void WelfareAnnuity_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.WelfareAnnuity_Text = value;

        Assert.Equal(value, vm.WelfareAnnuity_Text);
    }

    /// <summary>
    /// EmploymentInsurance_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系）
    /// </remarks>
    [Theory]
    [InlineData(1000)]
    [InlineData(0)]
    [InlineData(-500)]
    public void EmploymentInsurance_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.EmploymentInsurance_Text = value;

        Assert.Equal(value, vm.EmploymentInsurance_Text);
    }

    /// <summary>
    /// IncomeTax_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系）
    /// </remarks>
    [Theory]
    [InlineData(15000)]
    [InlineData(0)]
    [InlineData(-8000)]
    public void IncomeTax_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.IncomeTax_Text = value;

        Assert.Equal(value, vm.IncomeTax_Text);
    }

    /// <summary>
    /// MunicipalTax_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系）
    /// </remarks>
    [Theory]
    [InlineData(30000)]
    [InlineData(0)]
    [InlineData(-15000)]
    public void MunicipalTax_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.MunicipalTax_Text = value;

        Assert.Equal(value, vm.MunicipalTax_Text);
    }

    /// <summary>
    /// FriendshipAssociation_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 負の値（異常系）
    /// </remarks>
    [Theory]
    [InlineData(2000)]
    [InlineData(0)]
    [InlineData(-1000)]
    public void FriendshipAssociation_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.FriendshipAssociation_Text = value;

        Assert.Equal(value, vm.FriendshipAssociation_Text);
    }

    /// <summary>
    /// YearEndTaxAdjustment_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// YearEndTaxAdjustment は MoneyValue でなく plain double のため、負値も許容される。
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// InlineData 3: 逆調整（年末調整が還付のケース）
    /// </remarks>
    [Theory]
    [InlineData(5000)]
    [InlineData(0)]
    [InlineData(-30000)]
    public void YearEndTaxAdjustment_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.YearEndTaxAdjustment_Text = value;

        Assert.Equal(value, vm.YearEndTaxAdjustment_Text);
    }

    /// <summary>
    /// TotalDeduct_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の正値（正常系）
    /// InlineData 2: ゼロ（境界値）
    /// </remarks>
    [Theory]
    [InlineData(83000)]
    [InlineData(0)]
    public void TotalDeduct_Text_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.TotalDeduct_Text = value;

        Assert.Equal(value, vm.TotalDeduct_Text);
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
    [InlineData("控除備考テスト")]
    [InlineData("あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん")]
    public void Remarks_Text_値を設定後に読み取る_設定値と一致する(string value)
    {
        var vm = CreateViewModel();

        vm.Remarks_Text = value;

        Assert.Equal(value, vm.Remarks_Text);
    }

    // ==========================================
    // 同値変更テスト
    // ==========================================

    /// <summary>
    /// HealthInsurance_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void HealthInsurance_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.HealthInsurance_Text = 10000;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.HealthInsurance_Text))
                fired = true;
        };

        vm.HealthInsurance_Text = 10000;

        Assert.False(fired);
    }

    /// <summary>
    /// TotalDeduct_Text に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void TotalDeduct_Text_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.TotalDeduct_Text = 83000;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.TotalDeduct_Text))
                fired = true;
        };

        vm.TotalDeduct_Text = 83000;

        Assert.False(fired);
    }

    // ==========================================
    // 初期値テスト
    // ==========================================

    /// <summary>
    /// コンストラクタ後、TotalDeduct_Foreground の初期値が Red であることを確認する
    /// </summary>
    [Fact]
    public void TotalDeduct_Foreground_コンストラクタ後_初期値はRedである()
    {
        var vm = CreateViewModel();

        Assert.Equal(Colors.Red, vm.TotalDeduct_Foreground?.Color);
    }

    // ==========================================
    // イベント通知テスト（ヘルパー）
    // ==========================================

    /// <summary>
    /// 全控除額プロパティの PropertyChanged イベント発火を検証する
    /// </summary>
    private void イベント通知テスト(DeductionViewModel vm, DeductionEntity entity)
    {
        Assert.PropertyChanged(vm, nameof(vm.HealthInsurance_Text),
                               () => vm.HealthInsurance_Text = entity.HealthInsurance.Value);

        Assert.PropertyChanged(vm, nameof(vm.NursingInsurance_Text),
                               () => vm.NursingInsurance_Text = entity.NursingInsurance.Value);

        Assert.PropertyChanged(vm, nameof(vm.WelfareAnnuity_Text),
                               () => vm.WelfareAnnuity_Text = entity.WelfareAnnuity.Value);

        Assert.PropertyChanged(vm, nameof(vm.EmploymentInsurance_Text),
                               () => vm.EmploymentInsurance_Text = entity.EmploymentInsurance.Value);

        Assert.PropertyChanged(vm, nameof(vm.IncomeTax_Text),
                               () => vm.IncomeTax_Text = entity.IncomeTax.Value);

        Assert.PropertyChanged(vm, nameof(vm.MunicipalTax_Text),
                               () => vm.MunicipalTax_Text = entity.MunicipalTax.Value);

        Assert.PropertyChanged(vm, nameof(vm.FriendshipAssociation_Text),
                               () => vm.FriendshipAssociation_Text = entity.FriendshipAssociation.Value);

        Assert.PropertyChanged(vm, nameof(vm.YearEndTaxAdjustment_Text),
                               () => vm.YearEndTaxAdjustment_Text = entity.YearEndTaxAdjustment);

        Assert.PropertyChanged(vm, nameof(vm.Remarks_Text),
                               () => vm.Remarks_Text = entity.Remarks);

        Assert.PropertyChanged(vm, nameof(vm.TotalDeduct_Text),
                               () => vm.TotalDeduct_Text = entity.TotalDeduct.Value);
    }
}
