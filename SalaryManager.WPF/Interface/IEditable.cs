namespace SalaryManager.Infrastructure.Interface;

/// <summary>
/// Interface - 編集用
/// </summary>
/// <remarks>
/// DBから給与明細の各項目のデータを取得・追加更新を可能にする。
/// </remarks>
internal interface IEditable : IViewable
{
    /// <summary> 
    /// 保存 
    /// </summary>
    public void Save();
}
