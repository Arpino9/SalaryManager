namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - オプション(PDF)
/// </summary>
public class ViewModel_PDFOption : ViewModelBase
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_PDFOption()
    {
        this.Model.PDFOption = this;

        this.Model.Initialize_PDF();
    }

    protected override void BindEvents()
    {
        throw new System.NotImplementedException();
    }

    /// <summary> Model - オプション </summary>
    public Model_Option Model = Model_Option.GetInstance();

    #region パスワード

    /// <summary> パスワード - Text </summary>
    public ReactiveProperty<string> Password_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> パスワード - PasswordChar </summary>
    public ReactiveProperty<char> Password_PasswordChar { get; }
        = new ReactiveProperty<char>('⚫');

    #endregion

}
