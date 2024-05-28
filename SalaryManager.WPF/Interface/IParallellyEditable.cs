namespace SalaryManager.WPF.Interface;

/// <summary>
/// Interface - 編集用
/// </summary>
/// <remarks>
/// IEditableのトランザクション対応版
/// </remarks>
internal interface IParallellyEditable : IViewable
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="transaction">トランザクション</param>
    public void Save(ITransactionRepository transaction);
}
