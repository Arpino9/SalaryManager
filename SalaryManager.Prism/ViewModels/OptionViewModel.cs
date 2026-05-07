namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - オプション
/// </summary>
public class OptionViewModel : ViewModelBase<OptionModel>, IDialogAware
{
    public OptionViewModel()
    {

    }

    public string Title => "オプション";

    protected override OptionModel Model => throw new NotImplementedException();

    public event Action<IDialogResult> RequestClose;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
    }

    protected override void BindEvents()
    {
        
    }
}
