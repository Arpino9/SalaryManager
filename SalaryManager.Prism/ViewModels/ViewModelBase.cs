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
}
