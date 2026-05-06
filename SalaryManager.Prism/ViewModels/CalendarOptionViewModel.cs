namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - オプション - カレンダー
/// </summary>
public class CalendarOptionViewModel : ViewModelBase<OptionModel>
{
    public CalendarOptionViewModel()
    {
        this.Model.CalendarOption = this;

        this.Model.Initialize_Calendar();

        this.BindEvents();
    }
    protected override void BindEvents()
    {
        this.SelectPrivateKey_Command = new DelegateCommand(() => this.Model.SelectPrivateKeyPath_Calendar());
    }

    /// <summary> Model - オプション </summary>
    protected override OptionModel Model { get; } = OptionModel.GetInstance();

    #region JSONの保存先パス

    /// <summary> 認証ファイルの保存先パス - Text </summary>
    public string SelectPrivateKey_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 認証ファイルの保存先パス - Command </summary>
    public DelegateCommand SelectPrivateKey_Command { get; private set; }

    #endregion

    #region カレンダーID

    /// <summary> カレンダーIDの保存先パス - Text </summary>
    public string SelectCalendarID_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

}
