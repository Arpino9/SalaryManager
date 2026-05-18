using SalaryManager.WPF.Interface;

namespace SalaryManager.Infrastructure.Interface;

/// <summary>
/// Interface - 編集用マスタ
/// </summary>
public interface IEditableMaster : IViewableMaster
{
    /// <summary> 
    /// 追加 
    /// </summary>
    Task AddAsync();

    /// <summary> 
    /// 更新 
    /// </summary>
    Task UpdateAsync();

    /// <summary> 
    /// 削除
    /// </summary>
    Task DeleteAsync();
}
