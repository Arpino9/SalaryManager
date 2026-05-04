namespace SalaryManager.Domain.Modules.Helpers;

/// <summary>
/// 拡張クラス - Integer
/// </summary>
public static class IntegerUtils
{
    /// <param name="selectedIndex">コレクションのインデックス</param>
    extension(int selectedIndex)
    {
        /// <summary>
        /// コレクションが選択されていないか
        /// </summary>
        /// <returns>
        /// True : コレクション未選択 / False: コレクション選択済
        /// </returns>
        public bool IsUnSelected()
            => (selectedIndex == -1);
    }
}
