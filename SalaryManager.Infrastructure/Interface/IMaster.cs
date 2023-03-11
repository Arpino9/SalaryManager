namespace SalaryManager.Infrastructure.Interface
{
    /// <summary>
    /// Interface - マスタ画面
    /// </summary>
    public interface IMaster
    {
        /// <summary> 初期化 </summary>
        public void Initialize();

        /// <summary> 再描画 </summary>
        public void Refresh();

        /// <summary> リロード </summary>
        public void Reload();

        /// <summary> クリア </summary>
        public void Clear();

        /// <summary> 保存 </summary>
        public void Save();
    }
}
