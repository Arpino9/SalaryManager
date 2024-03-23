using System.Windows.Forms;

namespace SalaryManager.Domain.Modules.Helpers
{
    /// <summary>
    /// ディレクトリ・ファイル選択
    /// </summary>
    public sealed class DialogUtils
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
        public static string SelectDirectory(string description)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description         = description;
                dialog.SelectedPath        = DialogUtils.DefaultPath;                
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return string.Empty;
                }

                return dialog.SelectedPath;
            }
        }

        /// <summary>
        /// ファイルを開くダイアログを表示する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="filter">フィルタ</param>
        /// <param name="initialDirectory">初期ディレクトリ</param>
        /// <returns>指定されたパス</returns>
        public static string SelectFile(string fileName, string filter)
        {
            var dialog = new OpenFileDialog();

            dialog.FileName = fileName;
            dialog.InitialDirectory = string.Empty;
            dialog.Filter = filter;
            dialog.Title = "ファイルを開く";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return string.Empty;
            }

            return dialog.FileName;
        }

        /// <summary>
        /// 名前をつけて保存ダイアログを表示する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="filter">フィルタ</param>
        /// <returns>指定されたパス</returns>
        public static string SelectWithName(string fileName, string filter)
        {
            var dialog = new SaveFileDialog();

            dialog.FileName         = fileName;
            dialog.InitialDirectory = @"C:\";
            dialog.Filter           = filter;
            dialog.Title            = "名前をつけて保存";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return string.Empty;
            }

            return dialog.FileName;
        }
    }
}
