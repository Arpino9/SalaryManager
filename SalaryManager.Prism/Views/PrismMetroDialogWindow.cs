using MahApps.Metro.Controls;
using Prism.Services.Dialogs;

namespace SalaryManager.Prism.Views;

public class PrismMetroDialogWindow : MetroWindow, IDialogWindow
{
    public IDialogResult Result { get; set; }
}
