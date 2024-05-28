namespace SalaryManager.WPF.Interface;

/// <summary>
/// Interface - 閲覧用
/// </summary>
/// <remarks>
/// 表示用、入力用フォームがない画面が対象
/// </remarks>
internal interface IViewable
{
    /// <summary>
    /// 初期化
    /// </summary>
    void Initialize();

    /// <summary>
    /// 画面起動時の処理
    /// </summary>
    void Window_Activated();

    /// <summary>
    /// クリア
    /// </summary>
    void Clear();

    /// <summary>
    /// 再描画
    /// </summary>
    void Reload();
}
