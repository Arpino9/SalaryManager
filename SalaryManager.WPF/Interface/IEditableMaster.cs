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
    void Add();

    /// <summary> 
    /// 更新 
    /// </summary>
    void Update();

    /// <summary> 
    /// 削除
    /// </summary>
    void Delete();
}
