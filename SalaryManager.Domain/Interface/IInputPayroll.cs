namespace SalaryManager.Domain.Interface
{
    /// <summary>
    /// Interface - 給与明細入力
    /// </summary>
    /// <remarks>
    /// DBから給与明細の各項目のデータを取得・追加更新を可能にする。
    /// </remarks>
    public interface IInputPayroll
    {
        /// <summary> 初期化 </summary>
        internal void Initialize();

        /// <summary> リロード </summary>
        internal void Reload();

        /// <summary> 再描画 </summary>
        internal void Refresh();

        /// <summary> リセット </summary>
        internal void Clear();

        /// <summary> 登録 </summary>
        internal void Register();
    }
}
