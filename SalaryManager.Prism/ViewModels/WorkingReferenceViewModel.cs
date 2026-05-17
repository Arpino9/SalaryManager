namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 勤務参考
/// </summary>
public class WorkingReferenceViewModel : ViewModelBase<WorkingReferenceModel>
{
    public WorkingReferenceViewModel()
    {
        this.MainWindow.WorkingReference = this.Model;
        this.Model.ViewModel = this;

        base.Window_Activated();

        this.Model.Initialize();

        this.BindEvents();
    }

    private IWorkingReferencesRepository _workingReferencesRepository;

    /// <summary>
    /// 単体テスト用のコンストラクタ
    /// </summary>
    /// <param name="workingReferencesRepository">Repository - 勤務備考</param>
    public WorkingReferenceViewModel(IWorkingReferencesRepository workingReferencesRepository)
    {
        _workingReferencesRepository = workingReferencesRepository;
        WorkingReferenceModel.GetInstance(_workingReferencesRepository);

        this.MainWindow.WorkingReference = this.Model;
        this.Model.ViewModel             = this;

        this.Model.Clear();
    }

    protected override void BindEvents()
    {
        var entity = this.Model.Entity_LastYear;

        // 初期状態
        this.Default_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(0, 0));
        
        // 支給額-保険
        this.Insurance_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.Insurance_Text, entity?.Insurance.Value ?? 0));

        // 標準月額千円
        this.Norm_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.Norm_Text, entity?.Norm ?? 0));
    }

    /// <summary> Model </summary>
    protected override WorkingReferenceModel Model { get; }
        = WorkingReferenceModel.GetInstance(new WorkingReferenceSQLite());

    /// <summary> Model - メイン画面 </summary>
    public MainWindowModel MainWindow { get; set; }
        = MainWindowModel.GetInstance();

    #region 初期状態

    /// <summary> 初期状態 - MouseMove </summary>
    public DelegateCommand Default_MouseMove { get; private set; }

    #endregion

    #region 時間外時間

    /// <summary> 時間外時間 - Text </summary>
    public double OvertimeTime_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 休出時間

    /// <summary> 休出時間 - Text </summary>
    public double WeekendWorktime_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 深夜時間

    /// <summary> 深夜時間 - Text </summary>
    public double MidnightWorktime_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 遅刻早退欠勤H

    /// <summary> 遅刻早退欠勤H - Text </summary>
    public double LateAbsentH_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 支給額-保険

    /// <summary> 支給額-保険 - Text </summary>
    public double Insurance_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 支給額-保険 - MouseMove </summary>
    public DelegateCommand Insurance_MouseMove { get; private set; }

    #endregion

    #region 標準月額千円

    /// <summary> 標準月額千円 - Text </summary>
    public double Norm_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 標準月額千円 - MouseMove </summary>
    public DelegateCommand Norm_MouseMove { get; private set; }

    #endregion

    #region 扶養人数

    /// <summary> 扶養人数 - Text </summary>
    public double NumberOfDependent_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 有給残日数

    /// <summary> 有給残日数 - Text </summary>
    public double PaidVacation_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 勤務時間

    /// <summary> 勤務時間 - Text </summary>
    public double WorkingHours_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 備考

    /// <summary> 勤務時間 - Text </summary>
    public string Remarks_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

}
