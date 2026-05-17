namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 副業
/// </summary>
public class SideBusinessViewModel : ViewModelBase<SideBusinessModel>
{
    public SideBusinessViewModel()
    {
        this.MainWindow.SideBusiness = this.Model;

        this.Model.ViewModel = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    private ISideBusinessRepository _sideBusinessRepository;

    /// <summary>
    /// 単体テスト用のコンストラクタ
    /// </summary>
    /// <param name="sideBusinessRepository">Repository - 副業</param>
    public SideBusinessViewModel(ISideBusinessRepository sideBusinessRepository)
    {
        _sideBusinessRepository = sideBusinessRepository;
        SideBusinessModel.GetInstance(_sideBusinessRepository);

        this.MainWindow.SideBusiness = this.Model;
        this.Model.ViewModel         = this;

        this.Model.Clear();
    }

    protected override void BindEvents()
    {
        var entity = this.Model.Entity_LastYear;

        // Mouse Leave
        this.Default_MouseLeave = new DelegateCommand(() => this.MainWindow.ComparePrice(0, 0));

        // 副業
        this.SideBusiness_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.SideBusiness_Text, entity?.SideBusiness ?? 0));

        // 臨時収入
        this.Perquisite_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.Perquisite_Text, entity?.Perquisite ?? 0));

        // その他
        this.Others_MouseMove = new DelegateCommand(() => this.MainWindow.ComparePrice(this.Others_Text, entity?.Others ?? 0));
    }

    /// <summary> Model </summary>
    protected override SideBusinessModel Model { get; }
        = SideBusinessModel.GetInstance(new SideBusinessSQLite());

    /// <summary> Model - メイン画面 </summary>
    public MainWindowModel MainWindow { get; set; }
        = MainWindowModel.GetInstance();

    #region Mouse Leave

    /// <summary> MouseLeave - MouseLeave </summary>
    public DelegateCommand Default_MouseLeave { get; private set; }

    #endregion

    #region 副業

    /// <summary> 副業 - Text </summary>
    public double SideBusiness_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 副業 - MouseMove </summary>
    public DelegateCommand SideBusiness_MouseMove { get; private set; }

    #endregion

    #region 臨時収入

    /// <summary> 臨時収入 - Text </summary>
    public double Perquisite_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 臨時収入 - MouseMove </summary>
    public DelegateCommand Perquisite_MouseMove { get; private set; }

    #endregion

    #region その他

    /// <summary> その他 - Text </summary>
    public double Others_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> その他 - MouseMove </summary>
    public DelegateCommand Others_MouseMove { get; private set; }

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public string Remarks_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

}
