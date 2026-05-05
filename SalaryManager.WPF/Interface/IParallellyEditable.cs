namespace SalaryManager.WPF.Interface;

/// <summary>
/// Interface - 編集用
/// </summary>
/// <remarks>
/// IEditableのトランザクション対応版
/// </remarks>
public interface IParallellyEditable : IViewable
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="yearMonth">年月</param>
    /// <param name="transaction">トランザクション</param>
    public void Save(ITransactionRepository transaction, int id, DateOnly yearMonth);
}
