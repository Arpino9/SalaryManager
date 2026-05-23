namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// Moldel - 職歴
/// </summary>
public class WorkingPlaceViewModel : ViewModelBase<WorkingPlaceModel>, IDialogAware
{
    public WorkingPlaceViewModel()
    {
        this.Model.ViewModel = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    public event Action<IDialogResult> RequestClose;

    protected override void BindEvents()
    {
        // 派遣元会社
        this.DispatchingCompanyName_SelectionChanged = new DelegateCommand(() => this.Model.EnableWaitingButton());
        // 派遣先会社
        this.DispatchedCompanyName_SelectionChanged = new DelegateCommand(() => this.Model.EnableWaitingButton());
        // 就業場所
        this.WorkingPlace_TextChanged = new DelegateCommand(() => this.Model.SearchAddress());
        // 就業中
        this.IsWorking_Checked = new DelegateCommand(() => this.Model.IsWorking_Checked());
        // 就業場所の住所
        this.Address_TextChanged = new DelegateCommand(() => this.Model.EnableAddButton());
        // 経歴一覧
        this.WorkingPlaces_SelectionChanged = new DelegateCommand(() => this.Model.ListView_SelectionChanged());

        // 追加
        this.Add_Command = new DelegateCommand(() => 
        {
            this.Model.Add();
            this.Model.Reload();
        });

        // 更新
        this.Update_Command = new DelegateCommand(() => 
        {
            this.Model.Update();
            this.Model.Reload();
        });

        // 削除
        this.Delete_Command = new DelegateCommand(() => 
        {
            this.Model.Delete();
            this.Model.Reload();
        });
    }

    /// <summary> タイトル </summary>
    public string Title => "就業場所登録";

    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        Invoker.AddCommand(new ReturnCommand(Title));
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        Invoker.AddCommand(new ProceedCommand(Title));
    }

    /// <summary> Model </summary>
    protected override WorkingPlaceModel Model { get; }
        = WorkingPlaceModel.GetInstance(new WorkingPlaceSQLite());

    #region 就業場所一覧

    /// <summary> 就業場所一覧 - ItemSource </summary>
    public ObservableCollection<WorkingPlaceEntity> WorkingPlaces_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<WorkingPlaceEntity>();

    /// <summary> 就業場所一覧 - SelectedIndex </summary>
    public int WorkingPlaces_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 就業場所一覧 - SelectionChanged </summary>
    public DelegateCommand WorkingPlaces_SelectionChanged { get; private set; }

    #endregion

    #region 会社名

    /// <summary> 就業場所 - ItemSource </summary>
    public ObservableCollection<string> CompanyName_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<string>();

    /// <summary> 派遣元会社名 - Text </summary>
    public string DispatchingCompanyName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 派遣元会社名 - SelectionChanged </summary>
    public DelegateCommand DispatchingCompanyName_SelectionChanged { get; private set; }

    /// <summary> 派遣先会社名 - Text </summary>
    public string DispatchedCompanyName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 派遣先会社名 - SelectionChanged </summary>
    public DelegateCommand DispatchedCompanyName_SelectionChanged { get; private set; }

    #endregion

    #region 就業期間

    /// <summary> 就業期間 - 開始 - SelectedDate </summary>
    public DateTime WorkingStart_SelectedDate
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 就業期間 - 終了 - SelectedDate </summary>
    public DateTime WorkingEnd_SelectedDate
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 待機チェックボックス

    /// <summary> 待機 - IsChecked </summary>
    public bool IsWaiting_IsChacked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 待機 - Visibility </summary>
    public Visibility IsWaiting_Visibility
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 就業中

    /// <summary> 就業中 - IsChecked </summary>
    public bool IsWorking_IsChacked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 派遣先会社名 - SelectionChanged </summary>
    public DelegateCommand IsWorking_Checked { get; private set; }

    #endregion

    #region 就業場所

    /// <summary> 就業場所 - ItemSource </summary>
    public ObservableCollection<string> WorkingPlace_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<string>();

    /// <summary> 就業場所 - Text </summary>
    public string WorkingPlace_Name_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 就業場所 - TextChanged </summary>
    public DelegateCommand WorkingPlace_TextChanged { get; private set; }

    #endregion

    #region 住所

    /// <summary> 住所 - Text </summary>
    public string WorkingPlace_Address_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 住所 - TextChanged </summary>
    public DelegateCommand Address_TextChanged { get; private set; }

    #endregion

    #region 労働

    /// <summary> 労働開始 - 時 - Text </summary>
    public int WorkingTime_Start_Hour_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 労働開始 -  分 - Text </summary>
    public int WorkingTime_Start_Minute_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 労働終了 - 時 - Text </summary>
    public int WorkingTime_End_Hour_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 労働終了 - 分 - Text </summary>
    public int WorkingTime_End_Minute_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 昼休憩

    /// <summary> 昼休憩開始 - 時 - Text </summary>
    public int LunchTime_Start_Hour_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 昼休憩開始 - 分 - Text </summary>
    public int LunchTime_Start_Minute_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 昼休憩終了 - 時 - Text </summary>
    public int LunchTime_End_Hour_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 昼休憩終了 - 分 - Text </summary>
    public int LunchTime_End_Minute_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 休憩

    /// <summary> 休憩開始 - 時 - Text </summary>
    public int BreakTime_Start_Hour_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 休憩開始 - 分 - Text </summary>
    public int BreakTime_Start_Minute_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 休憩終了 - 時 - Text </summary>
    public int BreakTime_End_Hour_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 休憩終了 - 分 - Text </summary>
    public int BreakTime_End_Minute_Text
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
