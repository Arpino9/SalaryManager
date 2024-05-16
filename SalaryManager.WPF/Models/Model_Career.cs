using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 職歴
/// </summary>
public class Model_Career : IMaster
{
    #region Get Instance

    private static Model_Career model = null;

    public static Model_Career GetInstance(ICareerRepository repository)
    {
        if (model == null)
        {
            model = new Model_Career(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private ICareerRepository _repository;

    public Model_Career(ICareerRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 職歴 </summary>
    public ViewModel_Career ViewModel { get; set; }

    /// <summary> Entity - 経歴 </summary>
    public IReadOnlyList<CareerEntity> Entities { get; internal set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 未選択状態、かつ新規登録が可能な状態にする。
    /// </remarks>
    public void Initialize()
    {
        Careers.Create(_repository);

        this.ViewModel.Window_FontFamily.Value = XMLLoader.FetchFontFamily();
        this.ViewModel.Window_FontSize.Value   = XMLLoader.FetchFontSize();

        this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();

        this.Entities = Careers.FetchByDescending();

        this.Reflesh_ListView();

        this.ViewModel.Careers_SelectedIndex.Value = -1;
        this.Clear_InputForm();
    }

    /// <summary>
    /// 再描画
    /// </summary>
    /// <remarks>
    /// 該当月に経歴情報が存在すれば、各項目を再描画する。
    /// </remarks>
    public void Refresh()
    {
        // ListView
        this.Reflesh_ListView();
        // 入力用フォーム
        this.Reflesh_InputForm();
    }

    /// <summary>
    /// 再描画 - ListView
    /// </summary>
    private void Reflesh_ListView()
    {
        this.Clear_InputForm();

        if (this.Entities.IsEmpty())
        {
            return;
        }

        this.ViewModel.Careers_ItemSource = this.Entities.ToReactiveCollection();
    }

    /// <summary>
    /// Enable - 操作ボタン
    /// </summary>
    /// <remarks>
    /// 追加ボタンは「会社名」に値があれば押下可能。
    /// </remarks>
    private void EnableControlButton()
    {
        var selected = this.ViewModel.Careers_ItemSource.Any() 
                    && this.ViewModel.Careers_SelectedIndex.Value >= 0;

        // 更新ボタン
        this.ViewModel.Update_IsEnabled.Value = selected;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled.Value = selected;
    }

    /// <summary>
    /// 経歴 - SelectionChanged
    /// </summary>
    public void Careers_SelectionChanged()
    {
        if (this.ViewModel.Careers_SelectedIndex.Value.IsUnSelected())
        {
            return;
        }

        this.EnableControlButton();

        if (this.ViewModel.Careers_ItemSource.IsEmpty())
        {
            return;
        }

        var entity = this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex.Value];
        // 雇用形態
        this.ViewModel.WorkingStatus_Text.Value = entity.WorkingStatus;
        // 会社名
        this.ViewModel.CompanyName_Text.Value   = entity.CompanyName.Text;
        // 社員番号
        this.ViewModel.EmployeeNumber_Text.Value     = entity.EmployeeNumber;
        // 勤務開始日
        this.ViewModel.WorkingStart_SelectedDate.Value = entity.WorkingStartDate.Value;
        // 勤務終了日
        this.ViewModel.WorkingEnd_SelectedDate.Value = entity.WorkingEndDate.IsWorking ? DateTime.Today : entity.WorkingEndDate.Value;
        // 就業中か
        this.ViewModel.Working_IsChecked.Value = entity.WorkingEndDate.IsWorking;
        // 備考
        this.ViewModel.Remarks_Text.Value            = entity.Remarks;

        var allowance = entity.AllowanceExistence;
        // 皆勤手当
        this.ViewModel.PerfectAttendanceAllowance_IsChecked.Value = allowance.PerfectAttendance.Value;
        // 教育手当
        this.ViewModel.EducationAllowance_IsChecked.Value         = allowance.Education.Value;
        // 在宅手当
        this.ViewModel.ElectricityAllowance_IsChecked.Value       = allowance.Electricity.Value;
        // 資格手当
        this.ViewModel.CertificationAllowance_IsChecked.Value     = allowance.Certification.Value;
        // 時間外手当
        this.ViewModel.OvertimeAllowance_IsChecked.Value          = allowance.Overtime.Value;
        // 出張手当
        this.ViewModel.TravelAllowance_IsChecked.Value            = allowance.Travel.Value;
        // 住宅手当
        this.ViewModel.HousingAllowance_IsChecked.Value           = allowance.Housing.Value;
        // 食事手当
        this.ViewModel.FoodAllowance_IsChecked.Value              = allowance.Food.Value;
        // 深夜手当
        this.ViewModel.LateNightAllowance_IsChecked.Value         = allowance.LateNight.Value;
        // 地域手当
        this.ViewModel.AreaAllowance_IsChecked.Value              = allowance.Area.Value;
        // 通勤手当
        this.ViewModel.CommutingAllowance_IsChecked.Value         = allowance.Commution.Value;
        // 前払退職金
        this.ViewModel.PrepaidRetirementPayment_IsChecked.Value   = allowance.PrepaidRetirement.Value;
        // 扶養手当
        this.ViewModel.DependencyAllowance_IsChecked.Value        = allowance.Dependency.Value;
        // 役職手当
        this.ViewModel.ExecutiveAllowance_IsChecked.Value         = allowance.Executive.Value;
        // 特別手当
        this.ViewModel.SpecialAllowance_IsChecked.Value           = allowance.Special.Value;
    }

    /// <summary>
    /// 就業中か - Checked
    /// </summary>
    public void IsWorking_Checked()
    {
        this.ViewModel.WorkingEnd_IsEnabled.Value = this.ViewModel.Working_IsChecked.Value ? false : true;

        if (this.ViewModel.Working_IsChecked.Value)
        {
            this.ViewModel.WorkingEnd_SelectedDate.Value = DateTime.Today;
        }            
    }

    /// <summary>
    /// 会社名 - TextChanged
    /// </summary>
    public void EnableAddButton()
    {
        var inputted = !string.IsNullOrEmpty(this.ViewModel.CompanyName_Text.Value);

        this.ViewModel.Add_IsEnabled.Value = inputted;
    }

    /// <summary>
    /// 再描画 - 入力用フォーム
    /// </summary>
    private void Reflesh_InputForm()
    {
        this.IsWorking_Checked();

        this.Careers_SelectionChanged();

        // 追加ボタン
        this.EnableAddButton();
        // 更新、削除ボタン
        this.EnableControlButton();
    }

    /// <summary>
    /// リロード
    /// </summary>
    /// <remarks>
    /// 年月の変更時などに、該当月の項目を取得する。
    /// </remarks>
    public void Reload()
    {
        using (var cursor = new CursorWaiting())
        {
            Careers.Create(_repository);

            this.Entities = Careers.FetchByDescending();

            this.Refresh();
        }
    }

    /// <summary>
    /// クリア
    /// </summary>
    /// <remarks>
    /// 各項目を初期化する。
    /// </remarks>
    public void Clear_InputForm()
    {
        // 雇用形態
        this.ViewModel.WorkingStatus_Text.Value = this.ViewModel.WorkingStatus_ItemSource.First();
        // 会社名
        this.ViewModel.CompanyName_Text.Value   = default(string);
        // 勤務開始日
        this.ViewModel.WorkingStart_SelectedDate.Value = DateTime.Now;
        // 勤務終了日
        this.ViewModel.WorkingEnd_SelectedDate.Value   = DateTime.Now;
        this.IsWorking_Checked();
        // 社員番号
        this.ViewModel.EmployeeNumber_Text.Value     = default(string);
        // 備考
        this.ViewModel.Remarks_Text.Value            = default(string);

        // 皆勤手当
        this.ViewModel.PerfectAttendanceAllowance_IsChecked.Value = default(bool);
        // 教育手当
        this.ViewModel.EducationAllowance_IsChecked.Value         = default(bool);
        // 在宅手当
        this.ViewModel.ElectricityAllowance_IsChecked.Value       = default(bool);
        // 資格手当
        this.ViewModel.CertificationAllowance_IsChecked.Value     = default(bool);
        // 時間外手当
        this.ViewModel.OvertimeAllowance_IsChecked.Value          = default(bool);
        // 出張手当
        this.ViewModel.TravelAllowance_IsChecked.Value            = default(bool);
        // 住宅手当
        this.ViewModel.HousingAllowance_IsChecked.Value           = default(bool);
        // 食事手当
        this.ViewModel.FoodAllowance_IsChecked.Value              = default(bool);
        // 深夜手当
        this.ViewModel.LateNightAllowance_IsChecked.Value         = default(bool);
        // 地域手当
        this.ViewModel.AreaAllowance_IsChecked.Value              = default(bool);
        // 通勤手当
        this.ViewModel.CommutingAllowance_IsChecked.Value         = default(bool);
        // 前払退職金
        this.ViewModel.PrepaidRetirementPayment_IsChecked.Value   = default(bool);
        // 扶養手当
        this.ViewModel.DependencyAllowance_IsChecked.Value        = default(bool);
        // 役職手当
        this.ViewModel.ExecutiveAllowance_IsChecked.Value         = default(bool);
        // 特別手当
        this.ViewModel.SpecialAllowance_IsChecked.Value           = default(bool);

        // 追加ボタン
        this.ViewModel.Add_IsEnabled.Value    = false;
        // 更新ボタン
        this.ViewModel.Update_IsEnabled.Value = false;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled.Value = false;
    }

    /// <summary>
    /// 追加
    /// </summary>
    public void Add()
    {
        if (!Message.ShowConfirmingMessage($"入力された職歴を追加しますか？", this.ViewModel.Window_Title.Value))
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Delete_IsEnabled.Value = true;

            var entity = this.CreateEntity(this.Entities.Count + 1);
            this.ViewModel.Careers_ItemSource.Add(entity);
            this.Save();
        }   
    }

    /// <summary>
    /// Create Entity
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>職歴</returns>
    private CareerEntity CreateEntity(int id)
    {
        var workingEndDate = this.ViewModel.Working_IsChecked.Value ? DateTime.MaxValue : this.ViewModel.WorkingEnd_SelectedDate.Value;

        return new CareerEntity(
            id,
            this.ViewModel.WorkingStatus_Text.Value,
            this.ViewModel.CompanyName_Text.Value,
            this.ViewModel.EmployeeNumber_Text.Value,
            this.ViewModel.WorkingStart_SelectedDate.Value,
            workingEndDate,
            CreateAllowanceExistenceEntity(),
            this.ViewModel.Remarks_Text.Value);

        // 手当有無の作成
        AllowanceExistenceEntity CreateAllowanceExistenceEntity()
        {
            return new AllowanceExistenceEntity(
                this.ViewModel.PerfectAttendanceAllowance_IsChecked.Value,
                this.ViewModel.EducationAllowance_IsChecked.Value,
                this.ViewModel.ElectricityAllowance_IsChecked.Value,
                this.ViewModel.CertificationAllowance_IsChecked.Value,
                this.ViewModel.OvertimeAllowance_IsChecked.Value,
                this.ViewModel.TravelAllowance_IsChecked.Value,
                this.ViewModel.HousingAllowance_IsChecked.Value,
                this.ViewModel.FoodAllowance_IsChecked.Value,
                this.ViewModel.LateNightAllowance_IsChecked.Value,
                this.ViewModel.AreaAllowance_IsChecked.Value,
                this.ViewModel.CommutingAllowance_IsChecked.Value,
                this.ViewModel.PrepaidRetirementPayment_IsChecked.Value,
                this.ViewModel.DependencyAllowance_IsChecked.Value,
                this.ViewModel.ExecutiveAllowance_IsChecked.Value,
                this.ViewModel.SpecialAllowance_IsChecked.Value);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        if (!Message.ShowConfirmingMessage($"選択中の職歴を更新しますか？", this.ViewModel.Window_Title.Value))
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var id = this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex.Value].ID;

            var entity = this.CreateEntity(id);
            this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex.Value] = entity;

            this.Save();
        }   
    }

    /// <summary>
    /// 削除
    /// </summary>
    public void Delete()
    {
        if (this.ViewModel.Careers_SelectedIndex.Value.IsUnSelected() ||
            this.ViewModel.Careers_ItemSource.IsEmpty()) 
        {
            return;
        }

        if (!Message.ShowConfirmingMessage($"選択中の職歴を削除しますか？", this.ViewModel.Window_Title.Value))
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var id = this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex.Value].ID;

            _repository.Delete(id);

            this.ViewModel.Careers_ItemSource.RemoveAt(this.ViewModel.Careers_SelectedIndex.Value);

            this.Reload();
            this.EnableControlButton();
        }   
    }

    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
        foreach (var entity in this.ViewModel.Careers_ItemSource)
        {
            _repository.Save(entity);
        }

        this.Reload();
    }
}
