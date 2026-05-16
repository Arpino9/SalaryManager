namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 基底
/// </summary>
public abstract class ViewModelBase<M> : BindableBase where M : class
{
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

    /// <summary> Window - Activated </summary>
    public DelegateCommand Window_Activated { get; set; }

    #endregion

}
