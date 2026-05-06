namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - オプション(PDF)
/// </summary>
public class PDFOptionViewModel : ViewModelBase<OptionModel>
{
    public PDFOptionViewModel()
    {
        this.Model.PDFOption = this;

        this.Model.Initialize_PDF();
    }

    protected override void BindEvents()
    {
        throw new System.NotImplementedException();
    }

    /// <summary> Model - オプション </summary>
    protected override OptionModel Model { get; } = OptionModel.GetInstance();

    #region パスワード

    /// <summary> パスワード - Text </summary>
    public string Password_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> パスワード - PasswordChar </summary>
    public char Password_PasswordChar
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = '⚫';

    #endregion

}
