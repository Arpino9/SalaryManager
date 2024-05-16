namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - オプション - カレンダー
/// </summary>
public class ViewModel_CalendarOption : ViewModelBase
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_CalendarOption()
    {
        this.Model.CalendarOption = this;

        this.Model.Initialize_Calendar();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        this.SelectPrivateKey_Command.Subscribe(_ => this.Model.SelectPrivateKeyPath_Calendar());
    }

    /// <summary> Model - オプション </summary>
    public Model_Option Model = Model_Option.GetInstance();

    #region JSONの保存先パス

    /// <summary> 認証ファイルの保存先パス - Text </summary>
    public ReactiveProperty<string> SelectPrivateKey_Text { get; }
        = new ReactiveProperty<string>();

    /// <summary> 認証ファイルの保存先パス - Command </summary>
    public ReactiveCommand SelectPrivateKey_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region カレンダーID

    /// <summary> カレンダーIDの保存先パス - Text </summary>
    public ReactiveProperty<string> SelectCalendarID_Text { get; }
        = new ReactiveProperty<string>();

    #endregion

}
