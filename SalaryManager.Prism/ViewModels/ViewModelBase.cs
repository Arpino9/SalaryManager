namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 基底
/// </summary>
public abstract class ViewModelBase<M> : BindableBase where M : class
{
    protected ViewModelBase()
    {
        this.Window_Activated();
    }

    private static readonly log4net.ILog _logger =
     log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary> Model </summary>
    protected abstract M Model { get; }

    /// <summary>
    /// イベント登録
    /// </summary>
    /// <remarks>
    /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
    /// </remarks>
    protected abstract void BindEvents();

    /// <summary>
    /// リサイズモード
    /// </summary>
    protected enum ResizeMode
    {
        /// <summary> リサイズ不可 </summary>
        NoResize,

        /// <summary> 最小化可能 </summary>
        CanMinimize,

        /// <summary> リサイズ可能 </summary>
        CanResize,

        /// <summary> リサイズ可能（グリップ付き） </summary>
        CanResizeWithGrip
    }

    #region Window

    /// <summary> Window - FontFamily </summary>
    public FontFamily Window_FontFamily
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - FontSize </summary>
    public decimal Window_FontSize
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - Background </summary>
    public Brush Window_Background
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - ResizeMode </summary>
    public string Window_ResizeMode
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = ResizeMode.NoResize.ToString();

    /// <summary> Window - Title </summary>
    /// <remarks> メッセージ用 </remarks>
    public string Window_Title
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = Shared.SystemName;

    #endregion

    /// <summary>
    /// 画面起動時の処理
    /// </summary>
    private void Window_Activated()
    {
        try
        {
            this.Window_FontFamily = this.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
            this.Window_FontSize   = XMLLoader.FetchFontSize();
            this.Window_Background = this.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
        }
        catch (Exception ex)
        {
            _logger.Error("XMLの読み込みに失敗しました。", ex);
        }   
    }

    /// <summary>
    /// SixLabors.Fonts.FontFamily を System.Windows.Media.FontFamily に変換
    /// </summary>
    /// <param name="sixLaborsFontFamily">SixLabors.Fonts.FontFamily</param>
    /// <returns>System.Windows.Media.FontFamily</returns>
    /// <remarks>
    /// フォント名を取得し、WPF の FontFamily を作成する。
    /// フォントが見つからない場合はデフォルトフォントを返す。
    /// </remarks>
    private FontFamily ConvertToWpfFontFamily(SixLabors.Fonts.FontFamily sixLaborsFontFamily)
    {
        string fontName = sixLaborsFontFamily.Name;

        try
        {
            return new FontFamily(fontName);
        }
        catch (ArgumentException)
        {
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
