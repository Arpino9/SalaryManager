namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - 副業
/// </summary>
public class ViewModel_SideBusiness : ViewModelBase<Model_SideBusiness>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <exception cref="Exception">読込失敗時</exception>
    public ViewModel_SideBusiness()
    {
        this.MainWindow.SideBusiness = this.Model;

        this.Model.ViewModel = this;
        this.Model.Initialize();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        // Mouse Leave
        this.Default_MouseLeave.Subscribe(_ => this.MainWindow.ComparePrice(0, 0));
        // 副業
        this.SideBusiness_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.SideBusiness_Text.Value, this.Model.Entity_LastYear?.SideBusiness));
        // 臨時収入
        this.Perquisite_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.Perquisite_Text.Value, this.Model.Entity_LastYear?.Perquisite));
        // その他
        this.Others_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.Others_Text.Value, this.Model.Entity_LastYear?.Others));
    }

    /// <summary> Model </summary>
    protected override Model_SideBusiness Model { get; } 
        = Model_SideBusiness.GetInstance(new SideBusinessSQLite());

    /// <summary> Model - メイン画面 </summary>
    public Model_MainWindow MainWindow { get; set; }
        = Model_MainWindow.GetInstance();

    #region Window

    /// <summary> Window - FontFamily </summary>
    public ReactiveProperty<FontFamily> Window_FontFamily { get; set; }
        = new ReactiveProperty<FontFamily>();

    /// <summary> Window - FontSize </summary>
    public ReactiveProperty<decimal> Window_FontSize { get; set; }
        = new ReactiveProperty<decimal>();

    /// <summary> Window - Background </summary>
    public ReactiveProperty<Brush> Window_Background { get; set; }
        = new ReactiveProperty<Brush>();

    #endregion

    #region Mouse Leave

    /// <summary> MouseLeave - MouseLeave </summary>
    public ReactiveCommand Default_MouseLeave { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 副業

    /// <summary> 副業 - Text </summary>
    public ReactiveProperty<double> SideBusiness_Text { get; set; }
        = new ReactiveProperty<double>();

    /// <summary> 副業 - MouseMove </summary>
    public ReactiveCommand SideBusiness_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 臨時収入

    /// <summary> 臨時収入 - Text </summary>
    public ReactiveProperty<double> Perquisite_Text { get; set; }
        = new ReactiveProperty<double>();

    /// <summary> 臨時収入 - MouseMove </summary>
    public ReactiveCommand Perquisite_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region その他

    /// <summary> その他 - Text </summary>
    public ReactiveProperty<double> Others_Text { get; set; }
        = new ReactiveProperty<double>();

    /// <summary> その他 - MouseMove </summary>
    public ReactiveCommand Others_MouseMove { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public ReactiveProperty<string> Remarks_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

}
