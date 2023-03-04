using System.Windows.Forms;

namespace SalaryManager.Domain.Modules.Logics
{
    /// <summary>
    /// Utility - ダイアログメッセージ
    /// </summary>
    public sealed class DialogMessage
    {
        /// <summary>
        /// 通常のメッセージを表示する (OKのみ)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="title">タイトル</param>
        public static void ShowResultMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// エラーメッセージを表示する
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="title">タイトル</param>
        public static void ShowErrorMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 確認メッセージを表示する (Yes / No)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="title">タイトル</param>
        /// <returns>確認結果</returns>
        public static bool ShowConfirmingMessage(string message, string title)
        {
            var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            return (result == DialogResult.Yes);
        }
    }
}
