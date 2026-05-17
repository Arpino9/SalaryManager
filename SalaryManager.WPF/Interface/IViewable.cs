namespace SalaryManager.WPF.Interface;

/// <summary>
/// Interface - 閲覧用
/// </summary>
/// <remarks>
/// 表示用、入力用フォームがない画面が対象
/// </remarks>
public interface IViewable
{
    /// <summary>
    /// 初期化
    /// </summary>
    void Initialize();

    /// <summary>
    /// クリア
    /// </summary>
    void Clear();

    /// <summary>
    /// 再描画
    /// </summary>
    void Reload();
}
