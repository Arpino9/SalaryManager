namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 経歴
/// </summary>
public class CareerViewModel : ViewModelBase<CareerModel>, IDialogAware
{
    public CareerViewModel()
    {
        this.Model.ViewModel = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    private ICareerRepository _careerRepository;

    /// <summary>
    /// 単体テスト用のコンストラクタ
    /// </summary>
    /// <param name="repository">Repository - 職歴</param>
    public CareerViewModel(ICareerRepository repository)
    {
        _careerRepository = repository;
        CareerModel.GetInstance(_careerRepository);
        this.Model.ViewModel = this;
    }

    public event Action<IDialogResult> RequestClose;

    protected  override void BindEvents()
    {
        this.Window_Activated = new DelegateCommand(() => this.Model.Window_Activated());

        // 就業中
        this.Working_Checked = new DelegateCommand(() => this.Model.IsWorking_Checked());
        // 会社名
        this.CompanyName_TextChanged = new DelegateCommand(() => this.Model.EnableAddButton());
        // 経歴一覧
        this.Careers_SelectionChanged = new DelegateCommand(() => this.Model.ListView_SelectionChanged());

        // 追加
        this.Add_Command = new DelegateCommand(() => this.Model.AddAsync());
        // 更新
        this.Update_Command = new DelegateCommand(() => this.Model.UpdateAsync());
        // 削除
        this.Delete_Command = new DelegateCommand(() => this.Model.DeleteAsync());
    }

    /// <summary> タイトル </summary>
    public string Title => "経歴編集";

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
    }

    /// <summary> Model - 経歴 </summary>
    protected override CareerModel Model { get; } = CareerModel.GetInstance(new CareerSQLite());

    #region Window

    /// <summary> Window - FontFamily </summary>
    public FontFamily Window_FontFamily
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - FontSize </summary>
    public decimal Window_FontSize
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - Background </summary>
    public Brush Window_Background
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - Activated </summary>
    public DelegateCommand Window_Activated { get; private set; }

    #endregion

    #region 職歴一覧

    /// <summary> 職歴一覧 - ItemSource </summary>
    public ObservableCollection<CareerEntity> Careers_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<CareerEntity>();

    /// <summary> 職歴一覧 - SelectedIndex </summary>
    public int Careers_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 特別手当 - MouseMove </summary>
    public DelegateCommand Careers_SelectionChanged { get; private set; }

    #endregion

    #region 雇用形態

    /// <summary> 雇用形態 - ItemSource </summary>
    public ObservableCollection<string> WorkingStatus_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<string>() { "正社員", "契約社員", "派遣社員", "業務委託", "アルバイト" };

    /// <summary> 雇用形態 - SelectedIndex </summary>
    public int WorkingStatus_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 雇用形態 - Text </summary>
    public string WorkingStatus_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 会社名

    /// <summary> 会社名 - Text </summary>
    public string CompanyName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 会社名 - TextChanged </summary>
    public DelegateCommand CompanyName_TextChanged { get; private set; }

    #endregion

    #region 勤務期間

    /// <summary> 勤務開始日 - SelectedDate </summary>
    public DateTime WorkingStart_SelectedDate
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 勤務終了日 - SelectedDate </summary>
    public DateTime WorkingEnd_SelectedDate
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 勤務終了日 - IsEnabled </summary>
    public bool WorkingEnd_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 就業中 - IsChecked </summary>
    public bool Working_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 就業中 - Checked </summary>
    public DelegateCommand Working_Checked { get; private set; }

    #endregion

    #region 社員番号

    /// <summary> 社員番号 - Text </summary>
    public string EmployeeNumber_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 手当

    /// <summary> 皆勤手当 - IsChecked </summary>
    public bool PerfectAttendanceAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 教育手当 - IsChecked </summary>
    public bool EducationAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 在宅手当 - IsChecked </summary>
    public bool ElectricityAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 資格手当 - IsChecked </summary>
    public bool CertificationAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 時間外手当 - IsChecked </summary>
    public bool OvertimeAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 出張手当 - IsChecked </summary>
    public bool TravelAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 住宅手当 - IsChecked </summary>
    public bool HousingAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 食事手当 - IsChecked </summary>
    public bool FoodAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 深夜手当 - IsChecked </summary>
    public bool LateNightAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 地域手当 - IsChecked </summary>
    public bool AreaAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 通勤手当 - IsChecked </summary>
    public bool CommutingAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 前払退職金 - IsChecked </summary>
    public bool PrepaidRetirementPayment_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 扶養手当 - IsChecked </summary>
    public bool DependencyAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 役職手当 - IsChecked </summary>
    public bool ExecutiveAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 特別手当 - IsChecked </summary>
    public bool SpecialAllowance_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public string Remarks_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 追加

    /// <summary> 追加 - IsEnabled </summary>
    public bool Add_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 追加 - Command </summary>
    public DelegateCommand Add_Command { get; private set; }

    #endregion

    #region 更新

    /// <summary> 更新 - IsEnabled </summary>
    public bool Update_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 更新 - Command </summary>
    public DelegateCommand Update_Command { get; private set; }

    #endregion

    #region 削除

    /// <summary> 削除 - IsEnabled </summary>
    public bool Delete_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 削除 - Command </summary>
    public DelegateCommand Delete_Command { get; private set; }

    #endregion

}
