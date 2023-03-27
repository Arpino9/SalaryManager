using System.Windows.Media;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Repository - XML読み込み
    /// </summary>
    public interface IXMLLoaderRepository
    {
        /// <summary>
        /// デシリアライズ
        /// </summary>
        void Deserialize();

        /// <summary>
        /// SQLiteのパスを取得
        /// </summary>
        /// <returns>Excelテンプレートのパス</returns>
        string FetchSQLitePath();

        /// <summary>
        /// Excelテンプレートのパスを取得
        /// </summary>
        /// <returns>Excelテンプレートのパス</returns>
        string FetchExcelTemplatePath();

        /// <summary>
        /// フォントファミリを取得
        /// </summary>
        /// <returns>フォントファミリ</returns>
        string FetchFontFamilyText();

        /// <summary>
        /// フォントファミリを取得
        /// </summary>
        /// <returns>フォントファミリ</returns>
        FontFamily FetchFontFamily();

        /// <summary>
        /// 背景色を取得
        /// </summary>
        /// <returns>背景色</returns>
        System.Drawing.Color FetchBackgroundColor();

        /// <summary>
        /// 背景色を取得
        /// </summary>
        /// <returns>背景色</returns>
        decimal FetchFontSize();

        /// <summary>
        /// 背景色を取得
        /// </summary>
        /// <returns>背景色</returns>
        SolidColorBrush FetchBackgroundColorBrush();

        /// <summary>
        /// 「初期表示時にデフォルト明細を表示する」のチェック有無を取得する
        /// </summary>
        /// <returns></returns>
        bool FetchShowDefaultPayslip();
    }
}
