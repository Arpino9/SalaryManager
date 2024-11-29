namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 基底
/// </summary>
/// <typeparam name="T">ViewModel</typeparam>
public abstract class ModelBase<T> where T : class
{
    /// <summary> ViewModel </summary>
    internal abstract T ViewModel { get; set; }

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
