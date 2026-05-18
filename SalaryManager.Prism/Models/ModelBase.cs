namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 基底
/// </summary>
/// <typeparam name="VM">ViewModel</typeparam>
public abstract class ModelBase<VM> where VM : class 
{
    /// <summary> ViewModel </summary>
    internal abstract VM ViewModel { get; set; }

    /// <summary>
    /// MetroWindow
    /// </summary>
    private MahApps.Metro.Controls.MetroWindow MetroWindow =>
        System.Windows.Application.Current.Windows
            .OfType<MahApps.Metro.Controls.MetroWindow>()
            .FirstOrDefault(w => w.IsActive)
        ?? System.Windows.Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow;

    /// <summary>
    /// SixLabors.Fonts.FontFamily を System.Windows.Media.FontFamily に変換
    /// </summary>
    /// <param name="sixLaborsFontFamily">SixLabors.Fonts.FontFamily</param>
    /// <returns>System.Windows.Media.FontFamily</returns>
    public FontFamily ConvertToWpfFontFamily(SixLabors.Fonts.FontFamily sixLaborsFontFamily)
    {
        // フォント名を取得
        string fontName = sixLaborsFontFamily.Name;

        try
        {
            // WPF の FontFamily を作成
            return new FontFamily(fontName);
        }
        catch (ArgumentException)
        {
            // フォントが見つからない場合はデフォルトフォントを返す
            return new FontFamily("Segoe UI");
        }
    }

    /// <summary>
    /// 確認メッセージ表示
    /// </summary>
    /// <param name="title">タイトル</param>
    /// <param name="message">メッセージ</param>
    /// <returns>選択結果</returns>
    protected Task<MessageDialogResult> ShowConfirmMsgAsync(string title, string message)
        => this.MetroWindow.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative);

    /// <summary>
    /// メッセージ表示
    /// </summary>
    /// <param name="title">タイトル</param>
    /// <param name="message">メッセージ</param>
    /// <returns>void</returns>
    protected Task ShowMessageAsync(string title, string message)
        => this.MetroWindow.ShowMessageAsync(title, message);

    /// <summary>
    /// System.Drawing.Color を SolidColorBrush に変換
    /// </summary>
    /// <param name="drawingColor">System.Drawing.Color</param>
    /// <returns>SolidColorBrush</returns>
    public SolidColorBrush ConvertToBrush(System.Drawing.Color drawingColor)
    {
        return new SolidColorBrush(Color.FromArgb(
            drawingColor.A, // Alpha
            drawingColor.R, // Red
            drawingColor.G, // Green
            drawingColor.B  // Blue
        ));
    }
}
