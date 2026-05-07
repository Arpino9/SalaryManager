using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 経歴
/// </summary>
public sealed class CareerModel : ModelBase<CareerViewModel>, IEditableMaster
{
    #region Get Instance

    private static CareerModel model = null;

    public static CareerModel GetInstance(ICareerRepository repository)
    {
        if (model == null)
        {
            model = new CareerModel(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private ICareerRepository _repository;

    public CareerModel(ICareerRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 職歴 </summary>
    internal override CareerViewModel ViewModel { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 未選択状態、かつ新規登録が可能な状態にする。
    /// </remarks>
    public void Initialize()
    {
        this.Window_Activated();

        this.Reload();

        this.ListView_SelectionChanged();
    }

    public void Window_Activated()
    {
        this.ViewModel.Window_FontFamily = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
        this.ViewModel.Window_FontSize   = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
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
                    && this.ViewModel.Careers_SelectedIndex >= 0;

        // 更新ボタン
        this.ViewModel.Update_IsEnabled = selected;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled = selected;
    }

    /// <summary>
    /// 経歴 - SelectionChanged
    /// </summary>
    public void ListView_SelectionChanged()
    {
        if (this.ViewModel.Careers_SelectedIndex.IsUnSelected())
        {
            return;
        }

        this.EnableControlButton();

        if (this.ViewModel.Careers_ItemSource.IsEmpty())
        {
            return;
        }

        var entity = this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex];
        // 雇用形態
        this.ViewModel.WorkingStatus_Text = entity.WorkingStatus;
        // 会社名
        this.ViewModel.CompanyName_Text = entity.CompanyName.Text;
        // 社員番号
        this.ViewModel.EmployeeNumber_Text = entity.EmployeeNumber;
        // 勤務開始日
        this.ViewModel.WorkingStart_SelectedDate = Convert.ToDateTime(entity.WorkingStartDate.Value);
        // 勤務終了日
        this.ViewModel.WorkingEnd_SelectedDate = entity.WorkingEndDate.IsWorking ?
                                                         DateTime.Today : Convert.ToDateTime(entity.WorkingEndDate.Value);
        // 就業中か
        this.ViewModel.Working_IsChecked = entity.WorkingEndDate.IsWorking;
        // 備考
        this.ViewModel.Remarks_Text = entity.Remarks;

        var allowance = entity.AllowanceExistence;
        // 皆勤手当
        this.ViewModel.PerfectAttendanceAllowance_IsChecked = allowance.PerfectAttendance.Value;
        // 教育手当
        this.ViewModel.EducationAllowance_IsChecked = allowance.Education.Value;
        // 在宅手当
        this.ViewModel.ElectricityAllowance_IsChecked = allowance.Electricity.Value;
        // 資格手当
        this.ViewModel.CertificationAllowance_IsChecked = allowance.Certification.Value;
        // 時間外手当
        this.ViewModel.OvertimeAllowance_IsChecked = allowance.Overtime.Value;
        // 出張手当
        this.ViewModel.TravelAllowance_IsChecked = allowance.Travel.Value;
        // 住宅手当
        this.ViewModel.HousingAllowance_IsChecked = allowance.Housing.Value;
        // 食事手当
        this.ViewModel.FoodAllowance_IsChecked = allowance.Food.Value;
        // 深夜手当
        this.ViewModel.LateNightAllowance_IsChecked = allowance.LateNight.Value;
        // 地域手当
        this.ViewModel.AreaAllowance_IsChecked = allowance.Area.Value;
        // 通勤手当
        this.ViewModel.CommutingAllowance_IsChecked = allowance.Commution.Value;
        // 前払退職金
        this.ViewModel.PrepaidRetirementPayment_IsChecked = allowance.PrepaidRetirement.Value;
        // 扶養手当
        this.ViewModel.DependencyAllowance_IsChecked = allowance.Dependency.Value;
        // 役職手当
        this.ViewModel.ExecutiveAllowance_IsChecked = allowance.Executive.Value;
        // 特別手当
        this.ViewModel.SpecialAllowance_IsChecked = allowance.Special.Value;
    }

    /// <summary>
    /// 就業中か - Checked
    /// </summary>
    public void IsWorking_Checked()
    {
        this.ViewModel.WorkingEnd_IsEnabled = this.ViewModel.Working_IsChecked ? false : true;

        if (this.ViewModel.Working_IsChecked)
        {
            this.ViewModel.WorkingEnd_SelectedDate = DateTime.Today;
        }
    }

    /// <summary>
    /// 会社名 - TextChanged
    /// </summary>
    public void EnableAddButton()
    {
        var inputted = !string.IsNullOrEmpty(this.ViewModel.CompanyName_Text);

        this.ViewModel.Add_IsEnabled = inputted;
    }

    /// <summary>
    /// 再描画
    /// </summary>
    /// <remarks>
    /// 年月の変更時などに、該当月の項目を取得する。
    /// </remarks>
    public void Reload()
    {
        using (var cursor = new CursorWaiting())
        {
            Careers.Create(_repository);

            // ListView
            this.Reload_ListView();

            // 入力用フォーム
            this.Reload_InputForm();
        }
    }

    /// <summary>
    /// 再描画 - ListView
    /// </summary>
    private void Reload_ListView()
    {
        this.ViewModel.Careers_SelectedIndex = 0;

        var entities = Careers.FetchByDescending();

        if (entities.IsEmpty())
        {
            return;
        }

        this.ViewModel.Careers_ItemSource.Clear();

        foreach (var entity in entities)
        {
            this.ViewModel.Careers_ItemSource.Add(entity);
        }

        this.ListView_SelectionChanged();
    }

    /// <summary>
    /// 再描画 - 入力用フォーム
    /// </summary>
    private void Reload_InputForm()
    {
        this.Clear_InputForm();

        // 就業中フラグ
        this.IsWorking_Checked();

        // 追加ボタン
        this.EnableAddButton();
        // 更新、削除ボタン
        this.EnableControlButton();
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
        this.ViewModel.WorkingStatus_Text = this.ViewModel.WorkingStatus_ItemSource.First();
        // 会社名
        this.ViewModel.CompanyName_Text = default(string);
        // 勤務開始日
        this.ViewModel.WorkingStart_SelectedDate = DateTime.Today;
        // 勤務終了日
        this.ViewModel.WorkingEnd_SelectedDate = DateTime.Today;
        this.IsWorking_Checked();
        // 社員番号
        this.ViewModel.EmployeeNumber_Text = default(string);
        // 備考
        this.ViewModel.Remarks_Text = default(string);

        // 皆勤手当
        this.ViewModel.PerfectAttendanceAllowance_IsChecked = default(bool);
        // 教育手当
        this.ViewModel.EducationAllowance_IsChecked = default(bool);
        // 在宅手当
        this.ViewModel.ElectricityAllowance_IsChecked = default(bool);
        // 資格手当
        this.ViewModel.CertificationAllowance_IsChecked = default(bool);
        // 時間外手当
        this.ViewModel.OvertimeAllowance_IsChecked = default(bool);
        // 出張手当
        this.ViewModel.TravelAllowance_IsChecked = default(bool);
        // 住宅手当
        this.ViewModel.HousingAllowance_IsChecked = default(bool);
        // 食事手当
        this.ViewModel.FoodAllowance_IsChecked = default(bool);
        // 深夜手当
        this.ViewModel.LateNightAllowance_IsChecked = default(bool);
        // 地域手当
        this.ViewModel.AreaAllowance_IsChecked = default(bool);
        // 通勤手当
        this.ViewModel.CommutingAllowance_IsChecked = default(bool);
        // 前払退職金
        this.ViewModel.PrepaidRetirementPayment_IsChecked = default(bool);
        // 扶養手当
        this.ViewModel.DependencyAllowance_IsChecked = default(bool);
        // 役職手当
        this.ViewModel.ExecutiveAllowance_IsChecked = default(bool);
        // 特別手当
        this.ViewModel.SpecialAllowance_IsChecked = default(bool);
        // 追加ボタン
        this.ViewModel.Add_IsEnabled = false;
        // 更新ボタン
        this.ViewModel.Update_IsEnabled = false;
        // 削除ボタン
        this.ViewModel.Delete_IsEnabled = false;
    }

    /// <summary>
    /// 追加
    /// </summary>
    public async void AddAsync()
    {
        var result = await base.MetroWindow.ShowMessageAsync(
                this.ViewModel.Title,
                "入力された職歴を追加しますか？",
                MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            this.ViewModel.Delete_IsEnabled = true;

            var entity = this.CreateEntity(this.ViewModel.Careers_ItemSource.Count + 1);
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
        var workingEndDate = this.ViewModel.Working_IsChecked ? DateTime.MaxValue : Convert.ToDateTime(this.ViewModel.WorkingEnd_SelectedDate);

        return new CareerEntity(
            id,
            this.ViewModel.WorkingStatus_Text,
            this.ViewModel.CompanyName_Text,
            this.ViewModel.EmployeeNumber_Text,
            this.ViewModel.WorkingStart_SelectedDate,
            workingEndDate,
            CreateAllowanceExistenceEntity(),
            this.ViewModel.Remarks_Text);

        // 手当有無の作成
        AllowanceExistenceEntity CreateAllowanceExistenceEntity()
        {
            return new AllowanceExistenceEntity(
                this.ViewModel.PerfectAttendanceAllowance_IsChecked,
                this.ViewModel.EducationAllowance_IsChecked,
                this.ViewModel.ElectricityAllowance_IsChecked,
                this.ViewModel.CertificationAllowance_IsChecked,
                this.ViewModel.OvertimeAllowance_IsChecked,
                this.ViewModel.TravelAllowance_IsChecked,
                this.ViewModel.HousingAllowance_IsChecked,
                this.ViewModel.FoodAllowance_IsChecked,
                this.ViewModel.LateNightAllowance_IsChecked,
                this.ViewModel.AreaAllowance_IsChecked,
                this.ViewModel.CommutingAllowance_IsChecked,
                this.ViewModel.PrepaidRetirementPayment_IsChecked,
                this.ViewModel.DependencyAllowance_IsChecked,
                this.ViewModel.ExecutiveAllowance_IsChecked,
                this.ViewModel.SpecialAllowance_IsChecked);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public async void UpdateAsync()
    {
        var result = await base.MetroWindow.ShowMessageAsync(
                        this.ViewModel.Title,
                        "選択中の職歴を更新しますか？",
                        MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var id = this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex].ID;

            var entity = this.CreateEntity(id);
            this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex] = entity;

            this.Save();
        }
    }

    /// <summary>
    /// 削除
    /// </summary>
    public async void DeleteAsync()
    {
        if (this.ViewModel.Careers_SelectedIndex.IsUnSelected() ||
            this.ViewModel.Careers_ItemSource.IsEmpty())
        {
            return;
        }

        var result = await base.MetroWindow.ShowMessageAsync(
                        this.ViewModel.Title,
                        "選択中の職歴を削除しますか？",
                        MessageDialogStyle.AffirmativeAndNegative);

        if (result != MessageDialogResult.Affirmative)
        {
            // キャンセル
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            var id = this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex].ID;

            _repository.Delete(id);

            this.ViewModel.Careers_ItemSource.RemoveAt(this.ViewModel.Careers_SelectedIndex);

            this.EnableControlButton();
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    private void Save()
    {
        foreach (var entity in this.ViewModel.Careers_ItemSource)
        {
            _repository.Save(entity);
        }
    }
}
