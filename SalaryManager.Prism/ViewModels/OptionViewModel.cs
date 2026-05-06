namespace SalaryManager.Prism.ViewModels;

public class OptionViewModel : BindableBase, IDialogAware
{
    public OptionViewModel()
    {

    }

    public string Title => "オプション";

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
}
