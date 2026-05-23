using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.Models;
using SalaryManager.Prism.ViewModels;

namespace SalaryManagerTest;

/// <summary>
/// Model - 経歴 テスト
/// </summary>
/// <remarks>
/// 対象外メソッド:
/// - Initialize    : Reload() 経由で Careers.Create（静的ドメインクラス）に依存するため除外
/// - Reload        : Careers.Create（静的ドメインクラス）に依存するため除外
/// - AddAsync      : base.ShowConfirmMsgAsync（静的UIダイアログ）に依存するため除外
/// - UpdateAsync   : base.ShowConfirmMsgAsync（静的UIダイアログ）に依存するため除外
/// - DeleteAsync（本体）: base.ShowConfirmMsgAsync + Reload() に依存するため早期リターンのみテスト
/// - Save          : private メソッドのため除外
/// - CreateEntity  : private メソッドのため除外
/// </remarks>
public class CareerModelTest
{
    #region ヘルパー

    /// <summary>モックリポジトリを生成する</summary>
    private static Mock<ICareerRepository> CreateRepositoryMock() => new();

    /// <summary>テスト用 CareerModel を生成する（シングルトン非使用）</summary>
    private static CareerModel CreateModel(Mock<ICareerRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        return new CareerModel(mock.Object);
    }

    /// <summary>ViewModel を紐付けた CareerModel を生成する</summary>
    private static (CareerModel model, CareerViewModel vm) CreateModelWithViewModel(
        Mock<ICareerRepository>? mock = null)
    {
        mock ??= CreateRepositoryMock();
        var model = CreateModel(mock);
        var vm    = new CareerViewModel(mock.Object);
        model.ViewModel = vm;
        return (model, vm);
    }

    /// <summary>テスト用 CareerEntity を生成する</summary>
    private static CareerEntity CreateCareerEntity(
        int      id              = 1,
        string   workingStatus   = "正社員",
        string   companyName     = "テスト株式会社",
        string   employeeNumber  = "EMP001",
        DateTime workingStart    = default,
        DateTime workingEnd      = default,
        bool     perfectAttendance     = false,
        bool     education             = false,
        bool     electricity           = false,
        bool     certification         = false,
        bool     overtime              = false,
        bool     travel                = false,
        bool     housing               = false,
        bool     food                  = false,
        bool     lateNight             = false,
        bool     area                  = false,
        bool     commuting             = false,
        bool     prepaidRetirement     = false,
        bool     dependency            = false,
        bool     executive             = false,
        bool     special               = false,
        string   remarks               = "テスト備考")
    {
        var allowance = new AllowanceExistenceEntity(
            perfectAttendance, education, electricity, certification, overtime,
            travel, housing, food, lateNight, area, commuting,
            prepaidRetirement, dependency, executive, special);

        return new CareerEntity(
            id,
            workingStatus,
            companyName,
            employeeNumber,
            workingStart == default ? new DateTime(2020, 4, 1)  : workingStart,
            workingEnd   == default ? new DateTime(2023, 3, 31) : workingEnd,
            allowance,
            remarks);
    }

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

        var instance1 = CareerModel.GetInstance(mock.Object);
        var instance2 = CareerModel.GetInstance(mock.Object);

        Assert.Same(instance1, instance2);
    }

    // ==========================================
    // Clear_InputForm - 入力フォームを初期値にリセット
    // ==========================================

    /// <summary>
    /// Clear_InputForm 後、雇用形態が「正社員」（先頭項目）になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_InputForm_実行後_雇用形態が正社員になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.WorkingStatus_Text = "契約社員";

        model.Clear_InputForm();

        Assert.Equal("正社員", vm.WorkingStatus_Text);
    }

    /// <summary>
    /// Clear_InputForm 後、会社名が null になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_InputForm_実行後_会社名がNullになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.CompanyName_Text = "テスト株式会社";

        model.Clear_InputForm();

        Assert.Null(vm.CompanyName_Text);
    }

    /// <summary>
    /// Clear_InputForm 後、社員番号が null になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_InputForm_実行後_社員番号がNullになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.EmployeeNumber_Text = "EMP001";

        model.Clear_InputForm();

        Assert.Null(vm.EmployeeNumber_Text);
    }

    /// <summary>
    /// Clear_InputForm 後、備考が null になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_InputForm_実行後_備考がNullになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Remarks_Text = "テスト備考";

        model.Clear_InputForm();

        Assert.Null(vm.Remarks_Text);
    }

    /// <summary>
    /// Clear_InputForm 後、勤務開始日が今日になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_InputForm_実行後_勤務開始日が今日になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.WorkingStart_SelectedDate = new DateTime(2000, 1, 1);

        model.Clear_InputForm();

        Assert.Equal(DateTime.Today, vm.WorkingStart_SelectedDate);
    }

    /// <summary>
    /// Clear_InputForm 後、Working_IsChecked が false のとき WorkingEnd_IsEnabled が true になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_InputForm_就業中Falseの状態_WorkingEnd_IsEnabledがTrueになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Working_IsChecked = false;
        vm.WorkingEnd_IsEnabled = false; // 非活性にしておく

        model.Clear_InputForm();

        Assert.True(vm.WorkingEnd_IsEnabled);
    }

    /// <summary>
    /// Clear_InputForm 後、全ての手当フラグが false になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_InputForm_実行後_全手当フラグがFalseになる()
    {
        var (model, vm) = CreateModelWithViewModel();
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

        model.Clear_InputForm();

        Assert.False(vm.PerfectAttendanceAllowance_IsChecked);
        Assert.False(vm.EducationAllowance_IsChecked);
        Assert.False(vm.ElectricityAllowance_IsChecked);
        Assert.False(vm.CertificationAllowance_IsChecked);
        Assert.False(vm.OvertimeAllowance_IsChecked);
        Assert.False(vm.TravelAllowance_IsChecked);
        Assert.False(vm.HousingAllowance_IsChecked);
        Assert.False(vm.FoodAllowance_IsChecked);
        Assert.False(vm.LateNightAllowance_IsChecked);
        Assert.False(vm.AreaAllowance_IsChecked);
        Assert.False(vm.CommutingAllowance_IsChecked);
        Assert.False(vm.PrepaidRetirementPayment_IsChecked);
        Assert.False(vm.DependencyAllowance_IsChecked);
        Assert.False(vm.ExecutiveAllowance_IsChecked);
        Assert.False(vm.SpecialAllowance_IsChecked);
    }

    /// <summary>
    /// Clear_InputForm 後、追加・更新・削除ボタンがすべて無効になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void Clear_InputForm_実行後_追加更新削除ボタンがDisabledになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Add_IsEnabled    = true;
        vm.Update_IsEnabled = true;
        vm.Delete_IsEnabled = true;

        model.Clear_InputForm();

        Assert.False(vm.Add_IsEnabled);
        Assert.False(vm.Update_IsEnabled);
        Assert.False(vm.Delete_IsEnabled);
    }

    // ==========================================
    // IsWorking_Checked - 就業中フラグの切り替え
    // ==========================================

    /// <summary>
    /// Working_IsChecked が true のとき、WorkingEnd_IsEnabled が false になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void IsWorking_Checked_就業中True_WorkingEnd_IsEnabledがFalseになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Working_IsChecked    = true;
        vm.WorkingEnd_IsEnabled = true;

        model.IsWorking_Checked();

        Assert.False(vm.WorkingEnd_IsEnabled);
    }

    /// <summary>
    /// Working_IsChecked が true のとき、WorkingEnd_SelectedDate が今日に設定されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void IsWorking_Checked_就業中True_WorkingEnd_SelectedDateが今日になる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Working_IsChecked         = true;
        vm.WorkingEnd_SelectedDate   = new DateTime(2023, 3, 31);

        model.IsWorking_Checked();

        Assert.Equal(DateTime.Today, vm.WorkingEnd_SelectedDate);
    }

    /// <summary>
    /// Working_IsChecked が false のとき、WorkingEnd_IsEnabled が true になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void IsWorking_Checked_就業中False_WorkingEnd_IsEnabledがTrueになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Working_IsChecked    = false;
        vm.WorkingEnd_IsEnabled = false;

        model.IsWorking_Checked();

        Assert.True(vm.WorkingEnd_IsEnabled);
    }

    /// <summary>
    /// Working_IsChecked が false のとき、WorkingEnd_SelectedDate が変化しないことを確認する（正常系）
    /// </summary>
    [Fact]
    public void IsWorking_Checked_就業中False_WorkingEnd_SelectedDateが変化しない()
    {
        var (model, vm) = CreateModelWithViewModel();
        var originalDate = new DateTime(2023, 3, 31);
        vm.Working_IsChecked       = false;
        vm.WorkingEnd_SelectedDate = originalDate;

        model.IsWorking_Checked();

        Assert.Equal(originalDate, vm.WorkingEnd_SelectedDate);
    }

    // ==========================================
    // EnableAddButton - 追加ボタンの有効・無効切り替え
    // ==========================================

    /// <summary>
    /// 会社名が null のとき、追加ボタンが無効になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void EnableAddButton_会社名がNull_Add_IsEnabledがFalseになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.CompanyName_Text = null;

        model.EnableAddButton();

        Assert.False(vm.Add_IsEnabled);
    }

    /// <summary>
    /// 会社名が空文字のとき、追加ボタンが無効になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void EnableAddButton_会社名が空文字_Add_IsEnabledがFalseになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.CompanyName_Text = string.Empty;

        model.EnableAddButton();

        Assert.False(vm.Add_IsEnabled);
    }

    /// <summary>
    /// 会社名に値があるとき、追加ボタンが有効になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void EnableAddButton_会社名に値あり_Add_IsEnabledがTrueになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.CompanyName_Text = "テスト株式会社";

        model.EnableAddButton();

        Assert.True(vm.Add_IsEnabled);
    }

    /// <summary>
    /// 会社名に空白のみのとき、追加ボタンが有効になることを確認する（境界値）
    /// </summary>
    /// <remarks>
    /// IsNullOrEmpty は空白文字を空でないと判定するため、Add_IsEnabled は true になる。
    /// </remarks>
    [Fact]
    public void EnableAddButton_会社名が空白のみ_Add_IsEnabledがTrueになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.CompanyName_Text = " ";

        model.EnableAddButton();

        Assert.True(vm.Add_IsEnabled);
    }

    // ==========================================
    // ListView_SelectionChanged - 選択変更時の処理
    // ==========================================

    /// <summary>
    /// SelectedIndex が -1（未選択）のとき、早期リターンして更新・削除ボタン状態が変化しないことを確認する（境界値）
    /// </summary>
    [Fact]
    public void ListView_SelectionChanged_SelectedIndexが未選択_早期リターンしてボタン状態が変化しない()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Careers_SelectedIndex = -1;
        // EnableControlButton が呼ばれた場合 false に設定されるはずなので、先に true に設定しておく
        vm.Update_IsEnabled = true;
        vm.Delete_IsEnabled = true;

        model.ListView_SelectionChanged();

        // 早期リターンで EnableControlButton が呼ばれていないため true のまま
        Assert.True(vm.Update_IsEnabled);
        Assert.True(vm.Delete_IsEnabled);
    }

    /// <summary>
    /// SelectedIndex が有効でも ItemSource が空のとき、更新・削除ボタンが無効になることを確認する（境界値）
    /// </summary>
    [Fact]
    public void ListView_SelectionChanged_SelectedIndex有効_ItemSourceが空_Update_Delete_がFalseになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Careers_SelectedIndex = 0;
        vm.Update_IsEnabled      = true;
        vm.Delete_IsEnabled      = true;
        // Careers_ItemSource は空

        model.ListView_SelectionChanged();

        // EnableControlButton() が呼ばれ、ItemSource.Any()=false のため selected=false
        Assert.False(vm.Update_IsEnabled);
        Assert.False(vm.Delete_IsEnabled);
    }

    /// <summary>
    /// ItemSource にエンティティがあり選択済みのとき、ViewModel にエンティティの値が反映されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void ListView_SelectionChanged_ItemSourceに値あり_ViewModelにEntityの値が反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        var entity = CreateCareerEntity(
            workingStatus:  "契約社員",
            companyName:    "サンプル株式会社",
            employeeNumber: "EMP999",
            workingStart:   new DateTime(2021, 4, 1),
            workingEnd:     new DateTime(2024, 3, 31),
            remarks:        "選択テスト備考");
        vm.Careers_ItemSource.Add(entity);
        vm.Careers_SelectedIndex = 0;

        model.ListView_SelectionChanged();

        Assert.Equal("契約社員",         vm.WorkingStatus_Text);
        Assert.Equal("サンプル株式会社",  vm.CompanyName_Text);
        Assert.Equal("EMP999",          vm.EmployeeNumber_Text);
        Assert.Equal(new DateTime(2021, 4, 1),  vm.WorkingStart_SelectedDate);
        Assert.Equal(new DateTime(2024, 3, 31), vm.WorkingEnd_SelectedDate);
        Assert.False(vm.Working_IsChecked);
        Assert.Equal("選択テスト備考",   vm.Remarks_Text);
    }

    /// <summary>
    /// 就業中エンティティが選択されたとき、WorkingEnd_SelectedDate が今日になり Working_IsChecked が true になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void ListView_SelectionChanged_就業中エンティティ選択_WorkingEndが今日でWorking_IsCheckedがTrue()
    {
        var (model, vm) = CreateModelWithViewModel();
        // workingEnd = DateTime.MaxValue → IsWorking = true
        var entity = CreateCareerEntity(workingEnd: DateTime.MaxValue);
        vm.Careers_ItemSource.Add(entity);
        vm.Careers_SelectedIndex = 0;

        model.ListView_SelectionChanged();

        Assert.True(vm.Working_IsChecked);
        Assert.Equal(DateTime.Today, vm.WorkingEnd_SelectedDate);
    }

    /// <summary>
    /// 手当フラグが設定されたエンティティが選択されたとき、ViewModel の手当フラグが正しく反映されることを確認する（正常系）
    /// </summary>
    [Fact]
    public void ListView_SelectionChanged_手当フラグあり_ViewModelの手当フラグが反映される()
    {
        var (model, vm) = CreateModelWithViewModel();
        var entity = CreateCareerEntity(
            perfectAttendance: true,
            education:         true,
            overtime:          true,
            housing:           true,
            commuting:         true);
        vm.Careers_ItemSource.Add(entity);
        vm.Careers_SelectedIndex = 0;

        model.ListView_SelectionChanged();

        Assert.True(vm.PerfectAttendanceAllowance_IsChecked);
        Assert.True(vm.EducationAllowance_IsChecked);
        Assert.True(vm.OvertimeAllowance_IsChecked);
        Assert.True(vm.HousingAllowance_IsChecked);
        Assert.True(vm.CommutingAllowance_IsChecked);
        // 設定しなかった手当は false のまま
        Assert.False(vm.ElectricityAllowance_IsChecked);
        Assert.False(vm.TravelAllowance_IsChecked);
        Assert.False(vm.SpecialAllowance_IsChecked);
    }

    /// <summary>
    /// ItemSource に値があり選択済みのとき、更新・削除ボタンが有効になることを確認する（正常系）
    /// </summary>
    [Fact]
    public void ListView_SelectionChanged_ItemSourceに値あり_Update_Delete_がEnabledになる()
    {
        var (model, vm) = CreateModelWithViewModel();
        vm.Careers_ItemSource.Add(CreateCareerEntity());
        vm.Careers_SelectedIndex = 0;

        model.ListView_SelectionChanged();

        Assert.True(vm.Update_IsEnabled);
        Assert.True(vm.Delete_IsEnabled);
    }

    // ==========================================
    // DeleteAsync - 早期リターン（未選択・空リスト）
    // ==========================================

    /// <summary>
    /// SelectedIndex が -1（未選択）のとき、早期リターンして Delete が呼ばれないことを確認する（境界値）
    /// </summary>
    [Fact]
    public async Task DeleteAsync_SelectedIndexが未選択_Deleteが呼ばれない()
    {
        var mockRepo = CreateRepositoryMock();
        var (model, vm) = CreateModelWithViewModel(mockRepo);
        vm.Careers_SelectedIndex = -1;

        await model.DeleteAsync();

        mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
    }

    /// <summary>
    /// ItemSource が空のとき、早期リターンして Delete が呼ばれないことを確認する（境界値）
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ItemSourceが空_Deleteが呼ばれない()
    {
        var mockRepo = CreateRepositoryMock();
        var (model, vm) = CreateModelWithViewModel(mockRepo);
        vm.Careers_SelectedIndex = 0;
        // Careers_ItemSource は空

        await model.DeleteAsync();

        mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
    }
}
