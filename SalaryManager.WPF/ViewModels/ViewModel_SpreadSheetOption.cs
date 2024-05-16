namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - オプション - SpreadSheet
/// </summary>
public class ViewModel_SpreadSheetOption : ViewModelBase
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_SpreadSheetOption()
    {
        this.Model.SpreadSheetOption = this;

        this.Model.Initialize_SpreadSheet();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        this.SelectPrivateKey_Command.Subscribe(_ => this.Model.SelectPrivateKeyPath_SpreadSheet());
    }

    /// <summary> Model - オプション </summary>
    public Model_Option Model = Model_Option.GetInstance();

    #region 認証ファイルの保存先パス

    /// <summary> 認証ファイルの保存先パス - Text </summary>
    public ReactiveProperty<string> SelectPrivateKey_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 認証ファイルの保存先パス - Command </summary>
    public ReactiveCommand SelectPrivateKey_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region シートID

    /// <summary> シートID - Text </summary>
    public ReactiveProperty<string> SheetId_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

}
