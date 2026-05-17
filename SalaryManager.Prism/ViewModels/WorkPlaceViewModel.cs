namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 勤務備考
/// </summary>
public class WorkPlaceViewModel : ViewModelBase<WorkPlaceModel>
{
    public WorkPlaceViewModel()
    {
        this.Model.ViewModel               = this;
        this.WorkingReference.WorkPlace    = this;
        this.MainWindow.WorkPlace          = this;
        this.Allowance.ViewModel_WorkPlace = this;

        base.Window_Activated();

        this.Model.Initialize();
    }

    /// <summary>
    /// 単体テスト用のコンストラクタ
    /// </summary>
    /// <param name="forTest">テスト用フラグ（true のみ）</param>
    public WorkPlaceViewModel(bool forTest)
    {
        this.Model.ViewModel = this;
    }

    protected override void BindEvents()
    {
        throw new NotImplementedException();
    }

    /// <summary> Model </summary>
    protected override WorkPlaceModel Model { get; }
        = WorkPlaceModel.GetInstance();

    /// <summary> Model - 勤怠備考 </summary>
    public WorkingReferenceModel WorkingReference { get; set; }
        = WorkingReferenceModel.GetInstance(new WorkingReferenceSQLite());

    /// <summary> Model - メイン画面 </summary>
    public MainWindowModel MainWindow { get; set; }
        = MainWindowModel.GetInstance();

    /// <summary> Model - 手当 </summary>
    public AllowanceModel Allowance { get; set; }
        = AllowanceModel.GetInstance(new AllowanceSQLite());

    #region 所属会社名

    /// <summary> 所属会社名 - Foreground </summary>
    public SolidColorBrush CompanyName_Foreground
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 所属会社名 - Text </summary>
    public string CompanyName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 勤務先

    /// <summary> 所属会社名 - Foreground </summary>
    public SolidColorBrush WorkPlace_Foreground
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 勤務先 - Text </summary>
    public string WorkPlace_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion
}
