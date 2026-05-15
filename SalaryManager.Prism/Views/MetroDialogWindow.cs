using MahApps.Metro.Controls;
using Prism.Services.Dialogs;

namespace SalaryManager.Prism.Views;

/// <summary>
/// Prismダイアログ用のMetroWindowホスト。
/// MetroWindowのデザインを保ちつつ、IDialogWindowとして機能する。
/// </summary>
public class MetroDialogWindow : MetroWindow, IDialogWindow
{
    /// <inheritdoc/>
    public IDialogResult Result { get; set; }
}
