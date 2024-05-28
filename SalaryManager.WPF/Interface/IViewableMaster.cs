namespace SalaryManager.WPF.Interface;

/// <summary>
/// Interface - 閲覧用マスタ
/// </summary>
/// <remarks>
/// 機能がInfrastructureからの読込のみ
/// </remarks>
public interface IViewableMaster
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
    /// ListView - SelectionChanged
    /// </summary>
    void ListView_SelectionChanged();

    /// <summary>
    /// 再描画
    /// </summary>
    void Reload();

    /// <summary>
    /// Clear - 閲覧項目
    /// </summary>
    void Clear_InputForm();
}
