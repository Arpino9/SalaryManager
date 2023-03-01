using System.Windows.Forms;

namespace SalaryManager.Domain.Helpers
{
    /// <summary>
    /// ディレクトリ選択
    /// </summary>
    public sealed class DirectorySelector
    {
        /// <summary> 初期ディレクトリ </summary>
        public static readonly string DefaultPath = @"C:";

        /// <summary>
        /// ディレクトリ選択
        /// </summary>
        /// <param name="description">説明</param>
        /// <returns>ユーザーが選択したディレクトリ</returns>
        /// <remarks>
        /// 何も指定されなかった場合は空欄を返す。
        /// </remarks>
        public string Select(string description)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description         = description;
                dialog.SelectedPath        = DirectorySelector.DefaultPath;                
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return string.Empty;
                }

                return dialog.SelectedPath;
            }
        }
    }
}
