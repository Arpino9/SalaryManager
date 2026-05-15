using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.ViewModels;
using System.Windows.Media;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - 経歴 テスト
/// </summary>
public class CareerTest
{
    #region ヘルパー

    /// <summary>テスト用ViewModelを生成する</summary>
    private static CareerViewModel CreateViewModel()
    {
        var mock = new Mock<ICareerRepository>();
        return new CareerViewModel(mock.Object);
    }

    /// <summary>テスト用CareerEntityを生成する</summary>
    private static CareerEntity CreateEntity(
        string workingStatus,
        string companyName,
        string employeeNumber,
        DateTime workingStart,
        DateTime workingEnd,
        bool perfectAttendance = false,
        bool education         = false,
        bool electricity       = false,
        bool certification     = false,
        bool overtime          = false,
        bool travel            = false,
        bool housing           = false,
        bool food              = false,
        bool lateNight         = false,
        bool area              = false,
        bool commuting         = false,
        bool prepaidRetirement = false,
        bool dependency        = false,
        bool executive         = false,
        bool special           = false,
        string remarks         = "")
    {
        var allowance = new AllowanceExistenceEntity(
            perfectAttendance, education, electricity, certification,
            overtime, travel, housing, food, lateNight, area,
            commuting, prepaidRetirement, dependency, executive, special);

        return new CareerEntity(1, workingStatus, companyName, employeeNumber,
                                workingStart, workingEnd, allowance, remarks);
    }

    #endregion

    // ==========================================
    // IDialogAware テスト
    // ==========================================

    /// <summary>
    /// Title が "経歴編集" であることを確認する
    /// </summary>
    [Fact]
    public void Title_コンストラクタ後_経歴編集である()
    {
        var vm = CreateViewModel();

        Assert.Equal("経歴編集", vm.Title);
    }

    /// <summary>
    /// CanCloseDialog() が常に true を返すことを確認する
    /// </summary>
    [Fact]
    public void CanCloseDialog_呼び出し_trueを返す()
    {
        var vm = CreateViewModel();

        Assert.True(vm.CanCloseDialog());
    }

    // ==========================================
    // 初期値テスト
    // ==========================================

    /// <summary>
    /// コンストラクタ後、WorkingStatus_ItemSource に 5 件の雇用形態が設定されていることを確認する
    /// </summary>
    [Fact]
    public void WorkingStatus_ItemSource_コンストラクタ後_5件の雇用形態が設定されている()
    {
        var vm = CreateViewModel();

        Assert.Equal(5, vm.WorkingStatus_ItemSource.Count);
    }

    /// <summary>
    /// コンストラクタ後、WorkingStatus_ItemSource の先頭が "正社員" であることを確認する
    /// </summary>
    [Fact]
    public void WorkingStatus_ItemSource_コンストラクタ後_先頭が正社員である()
    {
        var vm = CreateViewModel();

        Assert.Equal("正社員", vm.WorkingStatus_ItemSource.First());
    }

    /// <summary>
    /// コンストラクタ後、WorkingStatus_ItemSource に指定の雇用形態が含まれることを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 正社員
    /// InlineData 2: 契約社員
    /// InlineData 3: 派遣社員
    /// InlineData 4: 業務委託
    /// InlineData 5: アルバイト
    /// </remarks>
    [Theory]
    [InlineData("正社員")]
    [InlineData("契約社員")]
    [InlineData("派遣社員")]
    [InlineData("業務委託")]
    [InlineData("アルバイト")]
    public void WorkingStatus_ItemSource_コンストラクタ後_指定の雇用形態が含まれる(string status)
    {
        var vm = CreateViewModel();

        Assert.Contains(status, vm.WorkingStatus_ItemSource);
    }

    /// <summary>
    /// コンストラクタ後、Careers_ItemSource が空のコレクションであることを確認する
    /// </summary>
    [Fact]
    public void Careers_ItemSource_コンストラクタ後_空のコレクションである()
    {
        var vm = CreateViewModel();

        Assert.Empty(vm.Careers_ItemSource);
    }

    /// <summary>
    /// コンストラクタ後、Add_IsEnabled が false であることを確認する
    /// </summary>
    [Fact]
    public void Add_IsEnabled_コンストラクタ後_falseである()
    {
        var vm = CreateViewModel();

        Assert.False(vm.Add_IsEnabled);
    }

    /// <summary>
    /// コンストラクタ後、Update_IsEnabled が false であることを確認する
    /// </summary>
    [Fact]
    public void Update_IsEnabled_コンストラクタ後_falseである()
    {
        var vm = CreateViewModel();

        Assert.False(vm.Update_IsEnabled);
    }

    /// <summary>
    /// コンストラクタ後、Delete_IsEnabled が false であることを確認する
    /// </summary>
    [Fact]
    public void Delete_IsEnabled_コンストラクタ後_falseである()
    {
        var vm = CreateViewModel();

        Assert.False(vm.Delete_IsEnabled);
    }

    /// <summary>
    /// コンストラクタ後、Working_IsChecked が false であることを確認する
    /// </summary>
    [Fact]
    public void Working_IsChecked_コンストラクタ後_falseである()
    {
        var vm = CreateViewModel();

        Assert.False(vm.Working_IsChecked);
    }

    /// <summary>
    /// コンストラクタ後、WorkingEnd_IsEnabled が false であることを確認する
    /// </summary>
    [Fact]
    public void WorkingEnd_IsEnabled_コンストラクタ後_falseである()
    {
        var vm = CreateViewModel();

        Assert.False(vm.WorkingEnd_IsEnabled);
    }

    /// <summary>
    /// テスト用コンストラクタ後、Window_Background が null であることを確認する
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
    // プロパティ値テスト
    // ==========================================

    /// <summary>
    /// CompanyName_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の会社名（正常系）
    /// InlineData 2: 空文字（境界値）
    /// InlineData 3: null（異常系 / ViewModel は受け入れる）
    /// </remarks>
    [Theory]
    [InlineData("株式会社テスト")]
    [InlineData("")]
    [InlineData(null)]
    public void CompanyName_Text_値を設定後に読み取る_設定値と一致する(string? value)
    {
        var vm = CreateViewModel();

        vm.CompanyName_Text = value;

        Assert.Equal(value, vm.CompanyName_Text);
    }

    /// <summary>
    /// EmployeeNumber_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の社員番号（正常系）
    /// InlineData 2: 空文字（境界値）
    /// InlineData 3: null（異常系）
    /// </remarks>
    [Theory]
    [InlineData("EMP001")]
    [InlineData("")]
    [InlineData(null)]
    public void EmployeeNumber_Text_値を設定後に読み取る_設定値と一致する(string? value)
    {
        var vm = CreateViewModel();

        vm.EmployeeNumber_Text = value;

        Assert.Equal(value, vm.EmployeeNumber_Text);
    }

    /// <summary>
    /// Remarks_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の備考（正常系）
    /// InlineData 2: 空文字（境界値）
    /// InlineData 3: null（異常系）
    /// </remarks>
    [Theory]
    [InlineData("テスト備考")]
    [InlineData("")]
    [InlineData(null)]
    public void Remarks_Text_値を設定後に読み取る_設定値と一致する(string? value)
    {
        var vm = CreateViewModel();

        vm.Remarks_Text = value;

        Assert.Equal(value, vm.Remarks_Text);
    }

    /// <summary>
    /// WorkingStatus_Text に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 正社員（正常系）
    /// InlineData 2: 契約社員（正常系）
    /// InlineData 3: 派遣社員（正常系）
    /// </remarks>
    [Theory]
    [InlineData("正社員")]
    [InlineData("契約社員")]
    [InlineData("派遣社員")]
    public void WorkingStatus_Text_値を設定後に読み取る_設定値と一致する(string value)
    {
        var vm = CreateViewModel();

        vm.WorkingStatus_Text = value;

        Assert.Equal(value, vm.WorkingStatus_Text);
    }

    /// <summary>
    /// Working_IsChecked に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: true（就業中）
    /// InlineData 2: false（離職済み）
    /// </remarks>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Working_IsChecked_値を設定後に読み取る_設定値と一致する(bool value)
    {
        var vm = CreateViewModel();

        vm.Working_IsChecked = value;

        Assert.Equal(value, vm.Working_IsChecked);
    }

    /// <summary>
    /// WorkingEnd_IsEnabled に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void WorkingEnd_IsEnabled_値を設定後に読み取る_設定値と一致する(bool value)
    {
        var vm = CreateViewModel();

        vm.WorkingEnd_IsEnabled = value;

        Assert.Equal(value, vm.WorkingEnd_IsEnabled);
    }

    /// <summary>
    /// Add_IsEnabled に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Add_IsEnabled_値を設定後に読み取る_設定値と一致する(bool value)
    {
        var vm = CreateViewModel();

        vm.Add_IsEnabled = value;

        Assert.Equal(value, vm.Add_IsEnabled);
    }

    /// <summary>
    /// Update_IsEnabled に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Update_IsEnabled_値を設定後に読み取る_設定値と一致する(bool value)
    {
        var vm = CreateViewModel();

        vm.Update_IsEnabled = value;

        Assert.Equal(value, vm.Update_IsEnabled);
    }

    /// <summary>
    /// Delete_IsEnabled に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Delete_IsEnabled_値を設定後に読み取る_設定値と一致する(bool value)
    {
        var vm = CreateViewModel();

        vm.Delete_IsEnabled = value;

        Assert.Equal(value, vm.Delete_IsEnabled);
    }

    /// <summary>
    /// Careers_SelectedIndex に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 0（先頭選択）
    /// InlineData 2: 1（2番目選択）
    /// InlineData 3: -1（未選択）
    /// </remarks>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void Careers_SelectedIndex_値を設定後に読み取る_設定値と一致する(int value)
    {
        var vm = CreateViewModel();

        vm.Careers_SelectedIndex = value;

        Assert.Equal(value, vm.Careers_SelectedIndex);
    }

    /// <summary>
    /// WorkingStatus_SelectedIndex に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 0（正社員）
    /// InlineData 2: 2（派遣社員）
    /// InlineData 3: 4（アルバイト）
    /// </remarks>
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(4)]
    public void WorkingStatus_SelectedIndex_値を設定後に読み取る_設定値と一致する(int value)
    {
        var vm = CreateViewModel();

        vm.WorkingStatus_SelectedIndex = value;

        Assert.Equal(value, vm.WorkingStatus_SelectedIndex);
    }

    /// <summary>
    /// Window_FontSize に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常フォントサイズ（正常系）
    /// InlineData 2: 小さいフォントサイズ（境界値）
    /// InlineData 3: 大きいフォントサイズ（境界値）
    /// </remarks>
    [Theory]
    [InlineData(12.0)]
    [InlineData(8.0)]
    [InlineData(24.0)]
    public void Window_FontSize_値を設定後に読み取る_設定値と一致する(double value)
    {
        var vm = CreateViewModel();

        vm.Window_FontSize = (decimal)value;

        Assert.Equal((decimal)value, vm.Window_FontSize);
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

    /// <summary>
    /// WorkingStart_SelectedDate に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    [Fact]
    public void WorkingStart_SelectedDate_値を設定後に読み取る_設定値と一致する()
    {
        var vm = CreateViewModel();
        var date = new DateTime(2020, 4, 1);

        vm.WorkingStart_SelectedDate = date;

        Assert.Equal(date, vm.WorkingStart_SelectedDate);
    }

    /// <summary>
    /// WorkingEnd_SelectedDate に値を設定後、読み取り値が一致することを確認する
    /// </summary>
    [Fact]
    public void WorkingEnd_SelectedDate_値を設定後に読み取る_設定値と一致する()
    {
        var vm = CreateViewModel();
        var date = new DateTime(2024, 3, 31);

        vm.WorkingEnd_SelectedDate = date;

        Assert.Equal(date, vm.WorkingEnd_SelectedDate);
    }

    // ==========================================
    // 手当プロパティ値テスト
    // ==========================================

    /// <summary>
    /// 全手当 IsChecked プロパティに値を設定後、読み取り値が一致することを確認する
    /// </summary>
    [Fact]
    public void 手当IsChecked_全プロパティ_値を設定後に読み取る_設定値と一致する()
    {
        var vm = CreateViewModel();

        vm.PerfectAttendanceAllowance_IsChecked   = true;
        vm.EducationAllowance_IsChecked           = true;
        vm.ElectricityAllowance_IsChecked         = true;
        vm.CertificationAllowance_IsChecked       = true;
        vm.OvertimeAllowance_IsChecked            = true;
        vm.TravelAllowance_IsChecked              = true;
        vm.HousingAllowance_IsChecked             = true;
        vm.FoodAllowance_IsChecked                = true;
        vm.LateNightAllowance_IsChecked           = true;
        vm.AreaAllowance_IsChecked                = true;
        vm.CommutingAllowance_IsChecked           = true;
        vm.PrepaidRetirementPayment_IsChecked     = true;
        vm.DependencyAllowance_IsChecked          = true;
        vm.ExecutiveAllowance_IsChecked           = true;
        vm.SpecialAllowance_IsChecked             = true;

        Assert.True(vm.PerfectAttendanceAllowance_IsChecked);
        Assert.True(vm.EducationAllowance_IsChecked);
        Assert.True(vm.ElectricityAllowance_IsChecked);
        Assert.True(vm.CertificationAllowance_IsChecked);
        Assert.True(vm.OvertimeAllowance_IsChecked);
        Assert.True(vm.TravelAllowance_IsChecked);
        Assert.True(vm.HousingAllowance_IsChecked);
        Assert.True(vm.FoodAllowance_IsChecked);
        Assert.True(vm.LateNightAllowance_IsChecked);
        Assert.True(vm.AreaAllowance_IsChecked);
        Assert.True(vm.CommutingAllowance_IsChecked);
        Assert.True(vm.PrepaidRetirementPayment_IsChecked);
        Assert.True(vm.DependencyAllowance_IsChecked);
        Assert.True(vm.ExecutiveAllowance_IsChecked);
        Assert.True(vm.SpecialAllowance_IsChecked);
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
            () => vm.CompanyName_Text = "テスト株式会社");
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
    /// WorkingStatus_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void WorkingStatus_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.WorkingStatus_Text),
            () => vm.WorkingStatus_Text = "契約社員");
    }

    /// <summary>
    /// EmployeeNumber_Text を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void EmployeeNumber_Text_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.EmployeeNumber_Text),
            () => vm.EmployeeNumber_Text = "EMP999");
    }

    /// <summary>
    /// Working_IsChecked を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Working_IsChecked_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.Working_IsChecked),
            () => vm.Working_IsChecked = true);
    }

    /// <summary>
    /// WorkingEnd_IsEnabled を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void WorkingEnd_IsEnabled_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.WorkingEnd_IsEnabled),
            () => vm.WorkingEnd_IsEnabled = true);
    }

    /// <summary>
    /// Add_IsEnabled を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Add_IsEnabled_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.Add_IsEnabled),
            () => vm.Add_IsEnabled = true);
    }

    /// <summary>
    /// Update_IsEnabled を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Update_IsEnabled_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.Update_IsEnabled),
            () => vm.Update_IsEnabled = true);
    }

    /// <summary>
    /// Delete_IsEnabled を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Delete_IsEnabled_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.Delete_IsEnabled),
            () => vm.Delete_IsEnabled = true);
    }

    /// <summary>
    /// WorkingStart_SelectedDate を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void WorkingStart_SelectedDate_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.WorkingStart_SelectedDate),
            () => vm.WorkingStart_SelectedDate = new DateTime(2020, 4, 1));
    }

    /// <summary>
    /// WorkingEnd_SelectedDate を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void WorkingEnd_SelectedDate_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.WorkingEnd_SelectedDate),
            () => vm.WorkingEnd_SelectedDate = new DateTime(2024, 3, 31));
    }

    /// <summary>
    /// Careers_SelectedIndex を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Careers_SelectedIndex_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.Careers_SelectedIndex),
            () => vm.Careers_SelectedIndex = 1);
    }

    /// <summary>
    /// Window_FontSize を変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void Window_FontSize_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.Window_FontSize),
            () => vm.Window_FontSize = 14m);
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
    /// 全手当 IsChecked プロパティを変更したとき PropertyChanged が発火することを確認する
    /// </summary>
    [Fact]
    public void 手当IsChecked_全プロパティ_値変更時_PropertyChangedが発火する()
    {
        var vm = CreateViewModel();

        Assert.PropertyChanged(vm, nameof(vm.PerfectAttendanceAllowance_IsChecked),
            () => vm.PerfectAttendanceAllowance_IsChecked = true);

        Assert.PropertyChanged(vm, nameof(vm.EducationAllowance_IsChecked),
            () => vm.EducationAllowance_IsChecked = true);

        vm.ElectricityAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.ElectricityAllowance_IsChecked),
            () => vm.ElectricityAllowance_IsChecked = true);

        vm.CertificationAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.CertificationAllowance_IsChecked),
            () => vm.CertificationAllowance_IsChecked = true);

        vm.OvertimeAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.OvertimeAllowance_IsChecked),
            () => vm.OvertimeAllowance_IsChecked = true);

        vm.TravelAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.TravelAllowance_IsChecked),
            () => vm.TravelAllowance_IsChecked = true);

        vm.HousingAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.HousingAllowance_IsChecked),
            () => vm.HousingAllowance_IsChecked = true);

        vm.FoodAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.FoodAllowance_IsChecked),
            () => vm.FoodAllowance_IsChecked = true);

        vm.LateNightAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.LateNightAllowance_IsChecked),
            () => vm.LateNightAllowance_IsChecked = true);

        vm.AreaAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.AreaAllowance_IsChecked),
            () => vm.AreaAllowance_IsChecked = true);

        vm.CommutingAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.CommutingAllowance_IsChecked),
            () => vm.CommutingAllowance_IsChecked = true);

        vm.PrepaidRetirementPayment_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.PrepaidRetirementPayment_IsChecked),
            () => vm.PrepaidRetirementPayment_IsChecked = true);

        vm.DependencyAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.DependencyAllowance_IsChecked),
            () => vm.DependencyAllowance_IsChecked = true);

        vm.ExecutiveAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.ExecutiveAllowance_IsChecked),
            () => vm.ExecutiveAllowance_IsChecked = true);

        vm.SpecialAllowance_IsChecked = false;
        Assert.PropertyChanged(vm, nameof(vm.SpecialAllowance_IsChecked),
            () => vm.SpecialAllowance_IsChecked = true);
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
        vm.CompanyName_Text = "テスト株式会社";

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.CompanyName_Text))
                fired = true;
        };

        vm.CompanyName_Text = "テスト株式会社";

        Assert.False(fired);
    }

    /// <summary>
    /// Working_IsChecked に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void Working_IsChecked_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.Working_IsChecked = true;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Working_IsChecked))
                fired = true;
        };

        vm.Working_IsChecked = true;

        Assert.False(fired);
    }

    /// <summary>
    /// Add_IsEnabled に同じ値を再設定したとき PropertyChanged が発火しないことを確認する
    /// </summary>
    [Fact]
    public void Add_IsEnabled_同じ値を再設定_PropertyChangedが発火しない()
    {
        var vm = CreateViewModel();
        vm.Add_IsEnabled = true;

        var fired = false;
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Add_IsEnabled))
                fired = true;
        };

        vm.Add_IsEnabled = true;

        Assert.False(fired);
    }

    // ==========================================
    // Careers_ItemSource テスト
    // ==========================================

    /// <summary>
    /// Careers_ItemSource にエンティティを追加後、件数が 1 増えることを確認する
    /// </summary>
    [Fact]
    public void Careers_ItemSource_エンティティを追加後_件数が増える()
    {
        var vm = CreateViewModel();
        var entity = CreateEntity("正社員", "テスト株式会社", "001",
                                  new DateTime(2020, 4, 1), new DateTime(2024, 3, 31));

        vm.Careers_ItemSource.Add(entity);

        Assert.Single(vm.Careers_ItemSource);
    }

    /// <summary>
    /// Careers_ItemSource に追加したエンティティを取得したとき、設定値と一致することを確認する
    /// </summary>
    [Fact]
    public void Careers_ItemSource_エンティティを追加後_取得値が一致する()
    {
        var vm = CreateViewModel();
        var entity = CreateEntity("正社員", "テスト株式会社", "EMP001",
                                  new DateTime(2020, 4, 1), new DateTime(2024, 3, 31));

        vm.Careers_ItemSource.Add(entity);

        Assert.Equal("テスト株式会社", vm.Careers_ItemSource[0].CompanyName.Text);
        Assert.Equal("EMP001",       vm.Careers_ItemSource[0].EmployeeNumber);
    }

    /// <summary>
    /// Careers_ItemSource からエンティティを削除後、コレクションが空になることを確認する
    /// </summary>
    [Fact]
    public void Careers_ItemSource_エンティティを削除後_件数が減る()
    {
        var vm = CreateViewModel();
        var entity = CreateEntity("正社員", "テスト株式会社", "001",
                                  new DateTime(2020, 4, 1), new DateTime(2024, 3, 31));

        vm.Careers_ItemSource.Add(entity);
        vm.Careers_ItemSource.Remove(entity);

        Assert.Empty(vm.Careers_ItemSource);
    }

    // ==========================================
    // 初期化 - PropertyChanged 通知テスト
    // ==========================================

    /// <summary>
    /// 職歴エンティティの各情報をプロパティに設定したとき、PropertyChanged が発火することを確認する
    /// </summary>
    /// <remarks>
    /// InlineData 1: 通常の在職履歴（離職済み・正社員）
    /// InlineData 2: 就業中ケース（派遣社員・DateTime.MaxValue）
    /// </remarks>
    [Theory]
    [InlineData("正社員",   "テスト株式会社", "EMP001", "2020-04-01", "2024-03-31", false, "通常の在職履歴")]
    [InlineData("派遣社員", "派遣先会社",     "HAK001", "2022-01-01", "9999-12-31", true,  "就業中ケース")]
    public void 初期化_職歴エンティティの情報を設定したとき_各プロパティにPropertyChangedが発火する(
        string workingStatus,
        string companyName,
        string employeeNumber,
        string workingStartStr,
        string workingEndStr,
        bool   isWorking,
        string remarks)
    {
        var workingStart = DateTime.Parse(workingStartStr);
        var workingEnd   = isWorking ? DateTime.MaxValue : DateTime.Parse(workingEndStr);

        var entity = CreateEntity(workingStatus, companyName, employeeNumber,
                                  workingStart, workingEnd, remarks: remarks);

        var vm = CreateViewModel();

        // PropertyChanged が確実に発火するよう、entity の値と異なる初期値に設定する
        vm.CompanyName_Text    = string.Empty;
        vm.WorkingStatus_Text  = string.Empty;
        vm.EmployeeNumber_Text = string.Empty;
        vm.Remarks_Text        = string.Empty;
        vm.Working_IsChecked   = !isWorking;

        イベント通知テスト(vm, entity);
    }

    // ==========================================
    // イベント通知テスト（ヘルパー）
    // ==========================================

    /// <summary>
    /// 職歴エンティティの主要プロパティの PropertyChanged イベント発火を検証する
    /// </summary>
    private static void イベント通知テスト(CareerViewModel vm, CareerEntity entity)
    {
        Assert.PropertyChanged(vm, nameof(vm.CompanyName_Text),
            () => vm.CompanyName_Text = entity.CompanyName.Text);

        Assert.PropertyChanged(vm, nameof(vm.WorkingStatus_Text),
            () => vm.WorkingStatus_Text = entity.WorkingStatus);

        Assert.PropertyChanged(vm, nameof(vm.EmployeeNumber_Text),
            () => vm.EmployeeNumber_Text = entity.EmployeeNumber);

        Assert.PropertyChanged(vm, nameof(vm.WorkingStart_SelectedDate),
            () => vm.WorkingStart_SelectedDate = entity.WorkingStartDate.Value);

        var workingEndDate = entity.WorkingEndDate.IsWorking
            ? DateTime.Today
            : entity.WorkingEndDate.Value;

        Assert.PropertyChanged(vm, nameof(vm.WorkingEnd_SelectedDate),
            () => vm.WorkingEnd_SelectedDate = workingEndDate);

        Assert.PropertyChanged(vm, nameof(vm.Working_IsChecked),
            () => vm.Working_IsChecked = entity.WorkingEndDate.IsWorking);

        Assert.PropertyChanged(vm, nameof(vm.Remarks_Text),
            () => vm.Remarks_Text = entity.Remarks);
    }
}
